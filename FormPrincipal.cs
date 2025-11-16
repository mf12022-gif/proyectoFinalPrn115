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
    /// Formulario principal del sistema IGUCOSA.
    /// Recibe el cargo y el nombre de usuario desde FormLogin.
    /// Muestra menú dinámico según el rol del usuario.
    /// </summary>
    public partial class FormPrincipal : Form
    {
        // VARIABLES PRIVADAS PARA ALMACENAR DATOS DEL USUARIO
        private string cargo;    // Almacena el cargo del usuario (cajero, asesor, gerente)
        private string usuario;  // Almacena el nombre de usuario logueado

        // <summary>
        /// Constructor del formulario principal.
        /// Recibe el cargo y el nombre de usuario desde FormLogin.
        /// </summary>
        /// <param name="cargo">Rol del usuario (cajero, asesor, gerente)</param>
        /// <param name="nombreUsuario">Nombre de usuario</param>
        public FormPrincipal(string c, string u)
        {
            InitializeComponent();// Inicializa todos los controles del formulario (botones, labels, etc.)
            // Asigna los valores recibidos desde el formulario de login
            cargo = c;    // Ej: "gerente", "cajero", "asesor"
            usuario = u;  // Ej: "gerente1"

            // Actualiza las etiquetas de bienvenida
            lblBienvenida.Text = "Bienvenido, " + usuario; // Muestra el nombre del usuario
            lblCargo.Text = "Cargo: " + c.ToUpper();       // Muestra el cargo en mayúsculas

            // Aplica restricciones según el rol del usuario
            AplicarPermisos();
        }

        // MÉTODO: Aplica permisos según el cargo del usuario
        private void AplicarPermisos()
        {
            // Si es CAJERO: solo puede ver clientes y transacciones
            if (cargo == "cajero")
            {
                btnGestionUsuarios.Enabled = false; // No puede gestionar usuarios
                btnDarBaja.Enabled = false;         // No puede dar de baja clientes
                btnGestionUsuarios.BackColor = System.Drawing.Color.LightGray; // Visualmente deshabilitado
                btnDarBaja.BackColor = System.Drawing.Color.LightGray;
            }
            // Si es ASESOR: puede registrar/modificar clientes, pero no usuarios
            else if (cargo == "asesor")
            {
                btnDarBaja.Enabled = false;// No puede gestionar dar de baja
                btnGestionUsuarios.BackColor = System.Drawing.Color.LightGray;
            }
            // Si es GERENTE: tiene acceso total a todo el sistema
            else if (cargo == "gerente")
            {
                // Todos los botones habilitados por defecto
                btnGestionUsuarios.Enabled = true;
                btnDarBaja.Enabled = true;
            }
        }
        /// <summary>
        /// Abre el formulario de gestión de clientes
        /// </summary>
        private void btnClientes_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario de clientes
            // Pasa el cargo para que el formulario sepa qué permisos aplicar
            FormClientes formClientes = new FormClientes(cargo);
            formClientes.ShowDialog(); // Muestra como ventana modal (bloquea el principal hasta cerrar)

        }
        /// <summary>
        /// Abre el formulario de historial.
        /// </summary>
        private void btnHistorial_Click(object sender, EventArgs e)
        {
            // Abre el formulario de historial de transacciones
            FormHistorial formHistorial = new FormHistorial();
            formHistorial.ShowDialog(); // Ventana modal
        }
        /// <summary>
        /// Abre el formulario de gestión de usuarios (solo asesor y gerente).
        /// </summary>
        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            // Solo accesible para gerente y asesor (cajero ya está deshabilitado)
            FormGestionUsuarios formUsuarios = new FormGestionUsuarios();
            formUsuarios.ShowDialog(); // Ventana modal
        }
        /// <summary>
        /// Abre el formulario Dar de Baja (solo gerente).
        /// </summary>
        private void btnDarBaja_Click(object sender, EventArgs e)
        {
            // Valida que el cargo permita acceso (opcional, ya lo hace el form)
            if (cargo != "asesor" && cargo != "gerente")
            {
                MessageBox.Show("Acceso denegado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            new FormDarBajaCliente(cargo).Show(); // ← ABRE EL FORMULARIO
        }
        /// <summary>
        /// Cierra sesión y regresa al formulario de login.
        /// </summary>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cerrar sesión y volver al inicio?",
        "Cerrar Sesión", MessageBoxButtons.YesNo,
        MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Database.CerrarConexionGlobal(); // Cierra conexión
                this.Hide();                     // Oculta el menú principal
                new FormLogin().Show();          // Muestra el login
            }
        }
        // ================================
        // X DEL FORMULARIO → CIERRE TOTAL
        // ================================
        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "¿Estás seguro de salir del sistema?\n\nLa aplicación se cerrará por completo.",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                );

                if (result == DialogResult.Yes)
                {
                    Database.CerrarConexionGlobal();
                    Application.Exit();
                    Environment.Exit(0); // CIERRE TOTAL DEL PROCESO
                }
                else
                {
                    e.Cancel = true; // Cancela el cierre
                }
            }

        }    
    }
}

