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
    public partial class FormGestionUsuarios : Form
    {
        // =============================================
        // VARIABLES PRIVADAS
        // =============================================
        private string duiSeleccionado = "";  // DUI del empleado seleccionado para editar

        // =============================================
        // CONSTRUCTOR
        // =============================================
        public FormGestionUsuarios()

        {
            InitializeComponent();
            // Configura DataGridView
            ConfigurarDataGridView();

            // Carga todos los empleados
            CargarEmpleados();

            // Configura ComboBox de cargo
            ConfigurarComboBoxCargo();

            // Deshabilita campos al inicio
            DeshabilitarCamposEdicion();
        }

        // =============================================
        // MÉTODO: Configura el DataGridView
        // =============================================
        private void ConfigurarDataGridView()
        {
            dgvEmpleados.Columns.Clear();

            dgvEmpleados.Columns.Add("IDEmpleado", "ID");
            dgvEmpleados.Columns.Add("DUI", "DUI");
            dgvEmpleados.Columns.Add("NombreCompleto", "Nombre Completo");
            dgvEmpleados.Columns.Add("Cargo", "Cargo");
            dgvEmpleados.Columns.Add("Usuario", "Usuario");

            // Anchos
            dgvEmpleados.Columns["IDEmpleado"].Width = 50;
            dgvEmpleados.Columns["DUI"].Width = 100;
            dgvEmpleados.Columns["NombreCompleto"].Width = 180;
            dgvEmpleados.Columns["Cargo"].Width = 80;
            dgvEmpleados.Columns["Usuario"].Width = 100;

            // Estilo
            dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.MultiSelect = false;
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.BackgroundColor = System.Drawing.Color.White;
        }

        // =============================================
        // MÉTODO: Configura ComboBox de Cargo
        // =============================================
        private void ConfigurarComboBoxCargo()
        {
            cmbCargo.Items.Clear();
            cmbCargo.Items.AddRange(new string[] { "cajero", "asesor", "gerente" });
            cmbCargo.SelectedIndex = 0;
        }

        // =============================================
        // MÉTODO: Carga todos los empleados
        // =============================================
        private void CargarEmpleados()
        {
            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                consulta.Connection = conn;
                consulta.CommandText = @"
                    SELECT 
                        e.IDEmpleado,
                        e.DUI,
                        CONCAT(p.Nombre, ' ', p.Apellido) AS NombreCompleto,
                        e.Cargo,
                        u.Usuario
                    FROM Empleados e
                    JOIN Personas p ON e.DUI = p.DUI
                    LEFT JOIN Usuarios u ON e.IDEmpleado = u.IDEmpleado
                    ORDER BY e.Cargo, p.Apellido";

                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvEmpleados.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    dgvEmpleados.Rows.Add(
                        row["IDEmpleado"].ToString(),
                        row["DUI"].ToString(),
                        row["NombreCompleto"].ToString(),
                        row["Cargo"].ToString().ToUpper(),
                        row["Usuario"] == DBNull.Value ? "SIN USUARIO" : row["Usuario"].ToString()
                    );
                }

                lblTotalEmpleados.Text = $"Total empleados: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarEmpleado_Click(object sender, EventArgs e)
        {
            string dui = txtBuscarDUI.Text.Trim().Replace("-", "");

            if (string.IsNullOrWhiteSpace(dui))
            {
                MessageBox.Show("Ingrese un DUI para buscar.", "Campo vacío",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dui.Length != 9 || !dui.All(char.IsDigit))
            {
                MessageBox.Show("DUI debe tener 9 dígitos numéricos.", "Formato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                consulta.Connection = conn;
                consulta.CommandText = @"
                    SELECT 
                        e.IDEmpleado,
                        e.DUI,
                        CONCAT(p.Nombre, ' ', p.Apellido) AS NombreCompleto,
                        e.Cargo,
                        u.Usuario
                    FROM Empleados e
                    JOIN Personas p ON e.DUI = p.DUI
                    LEFT JOIN Usuarios u ON e.IDEmpleado = u.IDEmpleado
                    WHERE e.DUI = '{0}'";

                consulta.CommandText = string.Format(consulta.CommandText, dui);

                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvEmpleados.Rows.Clear();

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    dgvEmpleados.Rows.Add(
                        row["IDEmpleado"].ToString(),
                        row["DUI"].ToString(),
                        row["NombreCompleto"].ToString(),
                        row["Cargo"].ToString().ToUpper(),
                        row["Usuario"] == DBNull.Value ? "SIN USUARIO" : row["Usuario"].ToString()
                    );

                    // Rellena campos para edición
                    duiSeleccionado = dui;
                    txtNombreEmpleado.Text = row["NombreCompleto"].ToString().Split(' ')[0] + " " +
                                            string.Join(" ", row["NombreCompleto"].ToString().Split(' ').Skip(1));
                    cmbCargo.Text = row["Cargo"].ToString();
                    txtUsuario.Text = row["Usuario"] == DBNull.Value ? "" : row["Usuario"].ToString();
                    txtClave.Text = "";
                    HabilitarCamposEdicion();
                }
                else
                {
                   
                   DialogResult resultado= MessageBox.Show("Empleado no encontrado. ¿Desea registrarlo?", "Nuevo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        duiSeleccionado = dui;
                        HabilitarCamposEdicion();
                        txtNombreEmpleado.Text = "";
                        txtUsuario.Text = "";
                        txtClave.Text = "";
                        cmbCargo.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarEmpleado_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposEmpleado()) return;

            MySqlConnection conn = Database.GetConnection();
            MySqlCommand consulta = new MySqlCommand();

            try
            {
                conn.Open(); // Abre conexión

                // 1. INSERTAR EN Personas (si no existe)
                consulta.Connection = conn;
                consulta.CommandText = @"
                    INSERT INTO Personas (DUI, Nombre, Apellido, FechaNacimiento) 
                    VALUES ('{0}', '{1}', '{2}', '1990-01-01')
                    ON DUPLICATE KEY UPDATE 
                        Nombre = VALUES(Nombre), 
                        Apellido = VALUES(Apellido)";

                string[] nombres = txtNombreEmpleado.Text.Trim().Split(' ');
                string nombre = nombres[0];
                string apellido = nombres.Length > 1 ? string.Join(" ", nombres.Skip(1)) : " ";

                consulta.CommandText = string.Format(consulta.CommandText, duiSeleccionado, nombre, apellido);
                MySqlDataAdapter adapter1 = new MySqlDataAdapter(consulta);
                DataTable temp1 = new DataTable();
                adapter1.Fill(temp1);

                // 2. INSERTAR EN Empleados
                consulta.CommandText = @"
                    INSERT INTO Empleados (DUI, Cargo) 
                    VALUES ('{0}', '{1}')
                    ON DUPLICATE KEY UPDATE Cargo = VALUES(Cargo)";

                consulta.CommandText = string.Format(consulta.CommandText, duiSeleccionado, cmbCargo.Text);
                MySqlDataAdapter adapter2 = new MySqlDataAdapter(consulta);
                DataTable temp2 = new DataTable();
                adapter2.Fill(temp2);

                // 3. INSERTAR EN Usuarios (si tiene usuario)
                if (!string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    consulta.CommandText = @"
                        INSERT INTO Usuarios (Usuario, Clave, IDEmpleado)
                        SELECT '{0}', '{1}', IDEmpleado
                        FROM Empleados WHERE DUI = '{2}'
                        ON DUPLICATE KEY UPDATE Clave = VALUES(Clave)";

                    consulta.CommandText = string.Format(consulta.CommandText,
                        txtUsuario.Text.Trim(), txtClave.Text, duiSeleccionado);

                    MySqlDataAdapter adapter3 = new MySqlDataAdapter(consulta);
                    DataTable temp3 = new DataTable();
                    adapter3.Fill(temp3);
                }

                MessageBox.Show("Empleado guardado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarEmpleados();
                DeshabilitarCamposEdicion();
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

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("No hay usuario para eliminar.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"¿Eliminar usuario '{txtUsuario.Text}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MySqlConnection conn = Database.GetConnection();
                MySqlCommand consulta = new MySqlCommand();

                try
                {
                    conn.Open();
                    consulta.Connection = conn;
                    consulta.CommandText = $"DELETE FROM Usuarios WHERE Usuario = '{txtUsuario.Text.Trim()}'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(consulta);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);

                    MessageBox.Show("Usuario eliminado.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarEmpleados();
                    txtUsuario.Clear();
                    txtClave.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DeshabilitarCamposEdicion();
            txtBuscarDUI.Clear();
            txtNombreEmpleado.Clear();
            cmbCargo.SelectedIndex = -1;
            txtUsuario.Clear();
            txtBuscarDUI.Focus();
            duiSeleccionado = "";
        }

        // =============================================
        // VALIDACIÓN DE CAMPOS
        // =============================================
        private bool ValidarCamposEmpleado()
        {
            if (string.IsNullOrWhiteSpace(duiSeleccionado))
            {
                MessageBox.Show("Busque un empleado primero.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text))
            {
                MessageBox.Show("Nombre completo obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!txtNombreEmpleado.Text.Trim().Contains(" "))
            {
                MessageBox.Show("Ingrese nombre y apellido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtUsuario.Text) && string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Clave obligatoria si hay usuario.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // =============================================
        // HABILITAR / DESHABILITAR CAMPOS
        // =============================================
        private void HabilitarCamposEdicion()
        {
            txtNombreEmpleado.Enabled = true;
            cmbCargo.Enabled = true;
            txtUsuario.Enabled = true;
            txtClave.Enabled = true;
            btnGuardarEmpleado.Enabled = true;
            btnEliminarUsuario.Enabled = true;
        }

        private void DeshabilitarCamposEdicion()
        {
            txtNombreEmpleado.Enabled = false;
            cmbCargo.Enabled = false;
            txtUsuario.Enabled = false;
            txtClave.Enabled = false;
            btnGuardarEmpleado.Enabled = false;
            btnEliminarUsuario.Enabled = false;
        }

        private void txtBuscarDUI_KeyPress(object sender, KeyPressEventArgs e)
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
            if (char.IsDigit(e.KeyChar) && txtBuscarDUI.Text.Length >= 9)
            {
                e.Handled = true; // ← No permite escribir más números
            }
        }
    }
}