using MySql.Data.MySqlClient;
using proyectoFinalPrn115.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoFinalPrn115
{
    public partial class FormDarBajaCliente : Form
    {
        // =============================================
        // VARIABLES PRIVADAS
        // =============================================
        private string duiCliente = "";  // DUI del cliente a dar de baja
        private string nombreCliente = "";

        // =============================================
        // CONSTRUCTOR
        // =============================================
        public FormDarBajaCliente()
        {
            InitializeComponent();
            // Configura controles
            ConfigurarControles();

            // Deshabilita campos al inicio
            DeshabilitarCampos();
        }

        // =============================================
        // MÉTODO: Configura controles iniciales
        // =============================================
        private void ConfigurarControles()
        {
            // Configura DateTimePicker
            dtpFechaFallecimiento.Value = DateTime.Today;
            dtpFechaFallecimiento.MaxDate = DateTime.Today;

            // Configura ComboBox de parentesco
            cmbParentesco.Items.AddRange(new string[] {
                "Cónyuge",
                "Hijo/a",
                "Padre/Madre",
                "Hermano/a",
                "Otro"
            });
            cmbParentesco.SelectedIndex = 0;

            // Título del formulario
            this.Text = "DAR DE BAJA CLIENTE - FALLECIMIENTO";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string dui = txtDUI.Text.Trim().Replace("-", "");

            // VALIDACIÓN DUI
            if (string.IsNullOrWhiteSpace(dui))
            {
                MessageBox.Show("Ingrese un DUI para buscar.", "Campo vacío",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDUI.Focus();
                return;
            }

            if (dui.Length != 9 || !dui.All(char.IsDigit))
            {
                MessageBox.Show("DUI debe tener 9 dígitos numéricos.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDUI.Focus();
                return;
            }

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                consulta.Connection = conn;
                consulta.CommandText = @"
                    SELECT 
                        p.Nombre, 
                        p.Apellido, 
                        c.BilleteraVirtual,
                        CONCAT(p.Nombre, ' ', p.Apellido) AS NombreCompleto
                    FROM Clientes c
                    JOIN Personas p ON c.DUI = p.DUI
                    WHERE c.DUI = '{0}'";

                consulta.CommandText = string.Format(consulta.CommandText, dui);

                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    nombreCliente = row["NombreCompleto"].ToString();
                    lblNombreCliente.Text = "Cliente: " + nombreCliente;
                    lblSaldoActual.Text = "Saldo actual: $" + Convert.ToDecimal(row["BilleteraVirtual"]).ToString("F2");

                    duiCliente = dui;
                    HabilitarCampos();
                    txtSolicitante.Focus();
                }
                else
                {
                    MessageBox.Show("Cliente no encontrado en el sistema.", "No existe",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarTodo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar cliente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnProcesarBaja_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposBaja()) return;

            // CONFIRMACIÓN FINAL
            DialogResult confirmacion = MessageBox.Show(
                $"¿CONFIRMAR DAR DE BAJA AL CLIENTE?\n\n" +
                $"Cliente: {nombreCliente}\n" +
                $"DUI: {duiCliente}\n" +
                $"Fecha de fallecimiento: {dtpFechaFallecimiento.Value:dd/MM/yyyy}\n" +
                $"Solicitante: {txtSolicitante.Text.Trim()} ({cmbParentesco.Text})\n\n" +
                $"Esta acción es IRREVERSIBLE.",
                "Confirmar Baja por Fallecimiento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation
            );

            if (confirmacion != DialogResult.Yes) return;

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();

                // 1. REGISTRAR TRANSACCIÓN DE RETIRO TOTAL (saldo a 0)
                decimal saldo = ObtenerSaldoCliente(duiCliente);
                if (saldo > 0)
                {
                    consulta.Connection = conn;
                    consulta.CommandText = @"
                        INSERT INTO Transacciones 
                        (DUI_Cliente, Tipo, Monto, Descripcion, Fecha) 
                        VALUES ('{0}', 'retiro', {1}, 'Retiro total por fallecimiento', '{2}')";

                    consulta.CommandText = string.Format(consulta.CommandText,
                        duiCliente,
                        saldo.ToString().Replace(",", "."),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    MySqlDataAdapter adapter1 = new MySqlDataAdapter(consulta);
                    DataTable temp1 = new DataTable();
                    adapter1.Fill(temp1);
                }

                // 2. ACTUALIZAR SALDO A 0
                consulta.CommandText = $"UPDATE Clientes SET BilleteraVirtual = 0 WHERE DUI = '{duiCliente}'";
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(consulta);
                DataTable temp2 = new DataTable();
                adapter2.Fill(temp2);

                // 3. REGISTRAR MOTIVO DE BAJA EN TABLA AUXILIAR (opcional)
                // Si existe tabla BajasClientes, se inserta
                try
                {
                    consulta.CommandText = @"
                        INSERT INTO BajasClientes 
                        (DUI_Cliente, FechaFallecimiento, Solicitante, Parentesco, FechaRegistro, ProcesadoPor)
                        VALUES ('{0}', '{1}', '{2}', '{3}', NOW(), 'GERENTE')";

                    consulta.CommandText = string.Format(consulta.CommandText,
                        duiCliente,
                        dtpFechaFallecimiento.Value.ToString("yyyy-MM-dd"),
                        txtSolicitante.Text.Trim(),
                        cmbParentesco.Text);

                    MySqlDataAdapter adapter3 = new MySqlDataAdapter(consulta);
                    DataTable temp3 = new DataTable();
                    adapter3.Fill(temp3);
                }
                catch { /* Si no existe la tabla, se ignora */ }

                // 4. ELIMINAR CLIENTE (ON DELETE CASCADE en Personas)
                consulta.CommandText = $"DELETE FROM Clientes WHERE DUI = '{duiCliente}'";
                MySqlDataAdapter adapter4 = new MySqlDataAdapter(consulta);
                DataTable temp4 = new DataTable();
                adapter4.Fill(temp4);

                MessageBox.Show(
                    $"CLIENTE DADO DE BAJA EXITOSAMENTE\n\n" +
                    $"Cliente: {nombreCliente}\n" +
                    $"DUI: {duiCliente}\n" +
                    $"Saldo retirado: ${saldo:F2}\n" +
                    $"Registrado por: {txtSolicitante.Text.Trim()} ({cmbParentesco.Text})",
                    "Baja Procesada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimpiarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar baja: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // =============================================
        // MÉTODO: Obtiene saldo actual del cliente
        // =============================================
        private decimal ObtenerSaldoCliente(string dui)
        {
            decimal saldo = 0;
            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                consulta.Connection = conn;
                consulta.CommandText = $"SELECT BilleteraVirtual FROM Clientes WHERE DUI = '{dui}'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    saldo = Convert.ToDecimal(dt.Rows[0][0]);
                }
            }
            catch { }

            return saldo;
        }

        // =============================================
        // VALIDACIÓN DE CAMPOS
        // =============================================
        private bool ValidarCamposBaja()
        {
            if (string.IsNullOrWhiteSpace(duiCliente))
            {
                MessageBox.Show("Busque un cliente primero.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSolicitante.Text))
            {
                MessageBox.Show("Nombre del solicitante obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSolicitante.Focus();
                return false;
            }

            if (!txtSolicitante.Text.Trim().Contains(" "))
            {
                MessageBox.Show("Ingrese nombre y apellido del solicitante.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSolicitante.Focus();
                return false;
            }

            if (dtpFechaFallecimiento.Value > DateTime.Today)
            {
                MessageBox.Show("Fecha de fallecimiento no puede ser futura.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cancelar operación de baja?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarTodo();
            }
        }

        // =============================================
        // MÉTODOS DE LIMPIEZA Y HABILITACIÓN
        // =============================================
        private void LimpiarTodo()
        {
            txtDUI.Clear();
            lblNombreCliente.Text = "-";
            lblSaldoActual.Text = "$0.00";
            duiCliente = "";
            txtSolicitante.Clear();
            cmbParentesco.SelectedIndex = -1;
            DeshabilitarCampos();
            txtDUI.Focus();
        }

        private void HabilitarCampos()
        {
            txtSolicitante.Enabled = true;
            cmbParentesco.Enabled = true;
            dtpFechaFallecimiento.Enabled = true;
            btnProcesarBaja.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void DeshabilitarCampos()
        {
            txtSolicitante.Enabled = false;
            cmbParentesco.Enabled = false;
            dtpFechaFallecimiento.Enabled = false;
            btnProcesarBaja.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void txtDUI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite: Backspace, Delete, teclas de navegación
            if (e.KeyChar == (char)Keys.Back ||
                e.KeyChar == (char)Keys.Delete ||
                char.IsControl(e.KeyChar))
            {
                e.Handled = false; // ← Permite borrar
                return;
            }

            // Bloquea cualquier dígito si ya hay 9
            if (char.IsDigit(e.KeyChar) && txtDUI.Text.Length >= 9)
            {
                e.Handled = true; // ← No permite escribir más números
            }
        }
    }
}