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
/// <summary>
/// Autor : Hugo Mejia 
/// fecha : 16/11/2025
/// version:1.0
/// </summary>
///programa que lleva registros de una cooperativa llamada igucosa y hace la conexion a una base de datos
///</summary>
///<param>
///solicita que cada usuario se loguee dependiendo su cargo (gerente, asesor y cajero)
///</param>

namespace proyectoFinalPrn115
{
    /// <summary>
    /// Clase que maneja la el historial de todas las transacciones de los usuarios de la base de datos
    /// </summary>
    public partial class FormHistorial : Form
    {
        public FormHistorial()
        {
            InitializeComponent();// Inicializa controles del formulario
            // Configura el DataGridView
            ConfigurarDataGridView();

            // Carga todas las transacciones al abrir
            CargarHistorialCompleto();
        }

        // =============================================
        // MÉTODO: Configura el DataGridView
        // =============================================
        private void ConfigurarDataGridView()
        {
            // Limpia columnas existentes
            dgvHistorial.Columns.Clear();

            // Agrega columnas personalizadas
            dgvHistorial.Columns.Add("ID", "ID");
            dgvHistorial.Columns.Add("DUI", "DUI Cliente");
            dgvHistorial.Columns.Add("Nombre", "Nombre Completo");
            dgvHistorial.Columns.Add("Tipo", "Tipo");
            dgvHistorial.Columns.Add("Monto", "Monto ($)");
            dgvHistorial.Columns.Add("Fecha", "Fecha");
            dgvHistorial.Columns.Add("Descripcion", "Descripción");
        }

        // CARGA TODAS LAS TRANSACCIONES (HISTORIAL COMPLETO)
        private void CargarHistorialCompleto()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Consulta: Une Transacciones + Clientes + Personas
                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT " +
                        "    t.IDTransaccion, " +
                        "    t.DUI_Cliente, " +
                        "    CONCAT(p.Nombre, ' ', p.Apellido) AS NombreCompleto, " +
                        "    t.Tipo, " +
                        "    t.Monto, " +
                        "    t.Fecha, " +
                        "    t.Descripcion " +
                        "FROM Transacciones t " +
                        "JOIN Clientes c ON t.DUI_Cliente = c.DUI " +
                        "JOIN Personas p ON c.DUI = p.DUI " +
                        "ORDER BY t.Fecha DESC", conn))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Limpia filas anteriores
                        dgvHistorial.Rows.Clear();

                        // Llena el DataGridView
                        foreach (DataRow row in dt.Rows)
                        {
                            dgvHistorial.Rows.Add(
                                row["IDTransaccion"],
                                row["DUI_Cliente"],
                                row["NombreCompleto"],
                                row["Tipo"].ToString().ToUpper(),
                                Convert.ToDecimal(row["Monto"]).ToString("N2"),
                                Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy HH:mm"),
                                row["Descripcion"] ?? "Sin descripción"
                            );
                        }

                        // Actualiza etiqueta
                        lblTotalTransacciones.Text = $"Total: {dt.Rows.Count} transacciones";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar historial: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscarDUI_Click(object sender, EventArgs e)
        {
            string dui = txtBuscarDUI.Text.Trim().Replace("-", "");

            // Validación de DUI
            if (string.IsNullOrWhiteSpace(dui))
            {
                MessageBox.Show("Ingrese un DUI para buscar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBuscarDUI.Focus();
                return;
            }

            if (dui.Length != 9 || !dui.All(char.IsDigit))
            {
                MessageBox.Show("DUI debe tener 9 dígitos numéricos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Busca transacciones del cliente
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT " +
                        "    t.IDTransaccion, " +
                        "    t.DUI_Cliente, " +
                        "    CONCAT(p.Nombre, ' ', p.Apellido) AS NombreCompleto, " +
                        "    t.Tipo, " +
                        "    t.Monto, " +
                        "    t.Fecha, " +
                        "    t.Descripcion " +
                        "FROM Transacciones t " +
                        "JOIN Clientes c ON t.DUI_Cliente = c.DUI " +
                        "JOIN Personas p ON c.DUI = p.DUI " +
                        "WHERE t.DUI_Cliente = @dui " +
                        "ORDER BY t.Fecha DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@dui", dui);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvHistorial.Rows.Clear();

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                dgvHistorial.Rows.Add(
                                    row["IDTransaccion"],
                                    row["DUI_Cliente"],
                                    row["NombreCompleto"],
                                    row["Tipo"].ToString().ToUpper(),
                                    Convert.ToDecimal(row["Monto"]).ToString("N2"),
                                    Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy HH:mm"),
                                    row["Descripcion"] ?? "Sin descripción"
                                );
                            }
                            lblTotalTransacciones.Text = $"Filtrado: {dt.Rows.Count} transacciones";
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron transacciones para este DUI.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblTotalTransacciones.Text = "Filtrado: 0 transacciones";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en búsqueda: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // EVENTO: Botón "Mostrar Todo"
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            txtBuscarDUI.Clear();
            CargarHistorialCompleto();
        }
        // EVENTO: Enter en el campo de búsqueda
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
        // EVENTO: Cerrar formulario
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
