using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoFinalPrn115.Clases;

namespace proyectoFinalPrn115
{
    public partial class FormClientes : Form
    {
        // VARIABLES PRIVADAS
        private string cargo;        // Almacena el rol del usuario (cajero, asesor, gerente)
        private string duiActual;    // Almacena el DUI del cliente actualmente consultado
        public FormClientes(string c)
        {
            InitializeComponent();
            cargo = c; // Asigna el cargo recibido

            // Si es CAJERO: no puede registrar ni modificar clientes
            if (cargo == "cajero")
            {
                btnGuardar.Enabled = false; // Deshabilita botón de guardar
                btnGuardar.BackColor = System.Drawing.Color.LightGray; // Visualmente deshabilitado
            }

            // Inicializa campos deshabilitados
            DeshabilitarCampos();
        }
        // EVENTO: Botón BUSCAR - Busca cliente por DUI
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtiene DUI y elimina guiones o espacios
            string dui = txtDUI.Text.Trim().Replace("-", "").Replace(" ", "");

            // VALIDACIÓN: DUI debe tener exactamente 9 dígitos
            if (dui.Length != 9)
            {
                MessageBox.Show("El DUI debe tener 9 dígitos.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDUI.Focus();
                return;
            }

            // VALIDACIÓN: DUI debe ser numérico
            if (!dui.All(char.IsDigit))
            {
                MessageBox.Show("El DUI solo debe contener números.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDUI.Focus();
                return;
            }

            // Asigna DUI actual para uso en transacciones
            duiActual = dui;

            // Conexión y comando
            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open(); // Abre conexión a la base de datos

                // ASIGNA CONEXIÓN Y COMANDO
                consulta.Connection = conn;
                consulta.CommandText = $"SELECT p.Nombre, p.Apellido, p.FechaNacimiento, c.BilleteraVirtual " +
                                       $"FROM Clientes c JOIN Personas p ON c.DUI = p.DUI WHERE c.DUI = '{dui}'";

                // Ejecuta consulta con DataAdapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt); // Llena resultados

                // CLIENTE ENCONTRADO
                if (dt.Rows.Count > 0)
                {
                    // Llena los campos con datos del cliente
                    txtNombre.Text = dt.Rows[0]["Nombre"].ToString();
                    txtApellido.Text = dt.Rows[0]["Apellido"].ToString();
                    dtpNacimiento.Value = Convert.ToDateTime(dt.Rows[0]["FechaNacimiento"]);
                    lblSaldo.Text = "Saldo: $" + Convert.ToDecimal(dt.Rows[0]["BilleteraVirtual"]).ToString("F2");

                    // Habilita botones de transacciones
                    HabilitarTransacciones();
                }
                // CLIENTE NO EXISTE
                else
                {
                    // Solo asesores y gerentes pueden registrar
                    if (cargo != "cajero")
                    {
                        DialogResult resultado = MessageBox.Show(
                            "Cliente no encontrado. ¿Desea registrarlo?",
                            "Nuevo cliente",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (resultado == DialogResult.Yes)
                        {
                            // Habilita campos para registro
                            HabilitarRegistro();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cliente no encontrado.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar cliente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close(); // Cierra conexión
            }
        }

        private void btnAbonar_Click(object sender, EventArgs e)
        {
            // VALIDACIÓN: Monto debe ser mayor a 0
            if (nudMonto.Value <= 0)
            {
                MessageBox.Show("El monto debe ser mayor a 0.", "Monto inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudMonto.Focus();
                return;
            }

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();

                // ACTUALIZA SALDO
                consulta.Connection = conn;
                consulta.CommandText = $"UPDATE Clientes SET BilleteraVirtual = BilleteraVirtual + {nudMonto.Value} WHERE DUI = '{duiActual}'";
                MySqlDataAdapter adapter1 = new MySqlDataAdapter(consulta);
                DataTable temp1 = new DataTable();
                adapter1.Fill(temp1);

                // REGISTRA TRANSACCIÓN
                consulta.CommandText = $"INSERT INTO Transacciones (DUI_Cliente, Tipo, Monto, Descripcion) " +
                                       $"VALUES ('{duiActual}', 'abono', {nudMonto.Value}, 'Abono en billetera virtual')";
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(consulta);
                DataTable temp2 = new DataTable();
                adapter2.Fill(temp2);

                MessageBox.Show("Abono realizado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Actualiza saldo mostrado
                ActualizarSaldo();

                // Limpia monto
                nudMonto.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abonar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            if (nudMonto.Value <= 0)
            {
                MessageBox.Show("El monto debe ser mayor a 0.", "Monto inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();

                // Verifica saldo actual
                consulta.Connection = conn;
                consulta.CommandText = $"SELECT BilleteraVirtual FROM Clientes WHERE DUI = '{duiActual}'";
                MySqlDataAdapter adapterCheck = new MySqlDataAdapter(consulta);
                DataTable dtCheck = new DataTable();
                adapterCheck.Fill(dtCheck);

                decimal saldoActual = Convert.ToDecimal(dtCheck.Rows[0][0]);

                if (nudMonto.Value > saldoActual)
                {
                    MessageBox.Show("Saldo insuficiente para retiro.", "Fondos insuficientes",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Realiza retiro
                consulta.CommandText = $"UPDATE Clientes SET BilleteraVirtual = BilleteraVirtual - {nudMonto.Value} WHERE DUI = '{duiActual}'";
                MySqlDataAdapter adapter1 = new MySqlDataAdapter(consulta);
                DataTable temp1 = new DataTable();
                adapter1.Fill(temp1);

                // Registra transacción
                consulta.CommandText = $"INSERT INTO Transacciones (DUI_Cliente, Tipo, Monto, Descripcion) " +
                                       $"VALUES ('{duiActual}', 'retiro', {nudMonto.Value}, 'Retiro en billetera virtual')";
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(consulta);
                DataTable temp2 = new DataTable();
                adapter2.Fill(temp2);

                MessageBox.Show("Retiro realizado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ActualizarSaldo();
                nudMonto.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al retirar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            if (nudMonto.Value <= 0)
            {
                MessageBox.Show("El monto debe ser mayor a 0.", "Monto inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();

                // Verifica saldo
                consulta.Connection = conn;
                consulta.CommandText = $"SELECT BilleteraVirtual FROM Clientes WHERE DUI = '{duiActual}'";
                MySqlDataAdapter adapterCheck = new MySqlDataAdapter(consulta);
                DataTable dtCheck = new DataTable();
                adapterCheck.Fill(dtCheck);

                decimal saldoActual = Convert.ToDecimal(dtCheck.Rows[0][0]);

                if (nudMonto.Value > saldoActual)
                {
                    MessageBox.Show("Saldo insuficiente para compra.", "Fondos insuficientes",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Registra compra (no reduce saldo si es tarjeta de crédito, pero aquí usamos billetera)
                consulta.CommandText = $"INSERT INTO Transacciones (DUI_Cliente, Tipo, Monto, Descripcion) " +
                                       $"VALUES ('{duiActual}', 'compra', {nudMonto.Value}, 'Compra con tarjeta débito')";
                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable temp = new DataTable();
                adapter.Fill(temp);

                MessageBox.Show("Compra registrada exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                nudMonto.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar compra: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Valida todos los campos
            if (!ValidarCamposCliente()) return;

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();

                // INSERTA O ACTUALIZA EN Personas
                consulta.Connection = conn;
                consulta.CommandText = $"INSERT INTO Personas (DUI, Nombre, Apellido, FechaNacimiento) VALUES " +
                                       $"('{duiActual}', '{txtNombre.Text.Trim()}', '{txtApellido.Text.Trim()}', '{dtpNacimiento.Value:yyyy-MM-dd}') " +
                                       $"ON DUPLICATE KEY UPDATE Nombre='{txtNombre.Text.Trim()}', Apellido='{txtApellido.Text.Trim()}', " +
                                       $"FechaNacimiento='{dtpNacimiento.Value:yyyy-MM-dd}'";
                MySqlDataAdapter adapter1 = new MySqlDataAdapter(consulta);
                DataTable temp1 = new DataTable();
                adapter1.Fill(temp1);

                // INSERTA EN Clientes si no existe
                consulta.CommandText = $"INSERT IGNORE INTO Clientes (DUI, BilleteraVirtual) VALUES ('{duiActual}', 0)";
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(consulta);
                DataTable temp2 = new DataTable();
                adapter2.Fill(temp2);

                MessageBox.Show("Cliente guardado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpia y deshabilita
                DeshabilitarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DeshabilitarCampos();
            txtDUI.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtDUI.Focus();
            lblSaldo.Text = "Saldo: $0.00";
        }

        // MÉTODO: Valida todos los campos del cliente
        private bool ValidarCamposCliente()
        {
            // Nombre obligatorio
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Campo vacío",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Apellido obligatorio
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Campo vacío",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            // Nombre solo letras y espacios
            if (!txtNombre.Text.Trim().All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El nombre solo debe contener letras.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Apellido solo letras y espacios
            if (!txtApellido.Text.Trim().All(c => char.IsLetter(c) || c == ' '))
            {
                MessageBox.Show("El apellido solo debe contener letras.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            // Mayor de edad (18 años)
            if (dtpNacimiento.Value > DateTime.Today.AddYears(-18))
            {
                MessageBox.Show("El cliente debe ser mayor de edad (18 años).", "Edad inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNacimiento.Focus();
                return false;
            }

            return true; // Todo válido
        }

        // MÉTODO: Actualiza el saldo mostrado en pantalla
        private void ActualizarSaldo()
        {
            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open();
                consulta.Connection = conn;
                consulta.CommandText = $"SELECT BilleteraVirtual FROM Clientes WHERE DUI = '{duiActual}'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblSaldo.Text = "Saldo: $" + Convert.ToDecimal(dt.Rows[0][0]).ToString("F2");
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        // MÉTODOS DE HABILITACIÓN/DESHABILITACIÓN
        private void HabilitarTransacciones()
        {
            nudMonto.Enabled = true;
            btnAbonar.Enabled = true;
            btnRetirar.Enabled = true;
            btnComprar.Enabled = true;
        }

        private void HabilitarRegistro()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            dtpNacimiento.Enabled = true;
            btnGuardar.Enabled = true;
        }

        private void DeshabilitarCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            dtpNacimiento.Enabled = false;
            nudMonto.Enabled = false;
            btnAbonar.Enabled = false;
            btnRetirar.Enabled = false;
            btnComprar.Enabled = false;
            btnGuardar.Enabled = false;
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
