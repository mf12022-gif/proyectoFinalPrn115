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
    public partial class FormPrincipal : Form
    {
        // VARIABLES PRIVADAS PARA ALMACENAR DATOS DEL USUARIO
        private string cargo;    // Almacena el cargo del usuario (cajero, asesor, gerente)
        private string usuario;  // Almacena el nombre de usuario logueado

        // CONSTRUCTOR: Recibe el cargo y usuario desde el login
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
                btnGestionUsuarios.Enabled = false; // No puede gestionar usuarios
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
        // EVENTO: Botón CLIENTES - Abre el formulario de gestión de clientes
        private void btnClientes_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario de clientes
            // Pasa el cargo para que el formulario sepa qué permisos aplicar
            FormClientes formClientes = new FormClientes(cargo);
            formClientes.ShowDialog(); // Muestra como ventana modal (bloquea el principal hasta cerrar)

        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            // Abre el formulario de historial de transacciones
            FormHistorial formHistorial = new FormHistorial();
            formHistorial.ShowDialog(); // Ventana modal
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            // Solo accesible para gerente y asesor (cajero ya está deshabilitado)
            FormGestionUsuarios formUsuarios = new FormGestionUsuarios();
            formUsuarios.ShowDialog(); // Ventana modal
        }

        private void btnDarBaja_Click(object sender, EventArgs e)
        {
            // Solo accesible para gerente
            FormDarBajaCliente formBaja = new FormDarBajaCliente();
            formBaja.ShowDialog(); // Ventana modal
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Pregunta de confirmación para evitar cierre accidental
            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea cerrar sesión?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                this.Close(); // Cierra el formulario principal
                // El evento FormClosed del login se encargará de mostrarlo nuevamente
            }
        }
        // EVENTO: Al cerrar el formulario principal (opcional: limpiar recursos)
        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Muestra nuevamente el formulario de login al cerrar
            FormLogin login = new FormLogin();
            login.Show();
        }
    }
}
