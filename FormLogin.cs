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
    /// Clase que maneja el inicio de sesion de los usuario gerente asesor y cajero a la base de datos
    /// y dependiendo su cargo asi tendran acceso a los diferentes campos del programa
    /// </summary>
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            txtUsuario.Focus();

            // Configuracion botón de probar conexion
            btnProbarConexion.Text = "Probar Conexión";
           
            // Etiqueta de estado
            lblEstadoConexion.Text = "Sin probar";
            lblEstadoConexion.ForeColor = System.Drawing.Color.Gray;
        }
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Complete usuario y contraseña.", "Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
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

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            string mensaje;
            if (Database.ProbarConexion(out mensaje))
            {
                lblEstadoConexion.Text = "Conectado";
                lblEstadoConexion.ForeColor = System.Drawing.Color.Green;
                MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblEstadoConexion.Text = "Desconectado";
                lblEstadoConexion.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(mensaje, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = false;
                return;
            }

            // Desvincular temporalmente para evitar múltiples mensajes
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this)
                {
                    form.FormClosing -= FormLogin_FormClosing;
                }
            }

            var result = MessageBox.Show(
                "¿Estás seguro de salir del sistema?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2
            );

            if (result == DialogResult.Yes)
            {
                Database.CerrarConexionGlobal();
                Application.Exit();
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;

                // Volver a vincular el evento
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (form != this)
                    {
                        form.FormClosing += FormLogin_FormClosing;
                    }
                }
            }
        }
    }
}