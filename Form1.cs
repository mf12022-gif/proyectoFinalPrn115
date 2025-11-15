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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            ProbarConexionAlInicio(); // Prueba automática
        }


        // PRUEBA CONEXIÓN (AL INICIO Y AL CLIC)
        private void ProbarConexionAlInicio()
        {
            ProbarConexion();
        }

        private void ProbarConexion()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    lblEstadoConexion.Text = "Conectado a igucosadb";
                    lblEstadoConexion.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show("Conexión exitosa a la base de datos `igucosadb`",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    lblEstadoConexion.Text = "Falló conexión";
                    lblEstadoConexion.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show(
                        $"NO SE PUDO CONECTAR\n\n" +
                        $"Error: {ex.Message}\n\n" +
                        $"VERIFIQUE:\n" +
                        $"• MySQL encendido (XAMPP)\n" +
                        $"• Base de datos: igucosadb\n" +
                        $"• Usuario: root\n" +
                        $"• Contraseña: root",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Complete usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT e.Cargo FROM Usuarios u JOIN Empleados e ON u.IDEmpleado = e.IDEmpleado WHERE u.Usuario = @u AND u.Clave = @c", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", usuario);
                        cmd.Parameters.AddWithValue("@c", clave);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            this.Hide();
                            new FormPrincipal(result.ToString(), usuario).Show();
                        }
                        else
                        {
                            MessageBox.Show("Credenciales incorrectas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClave.Clear();
                            txtClave.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}