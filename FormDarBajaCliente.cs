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
    /// Formulario para dar de baja (eliminar) a un cliente del sistema.
    /// Solo accesible por gerentes.
    /// Valida DUI en el botón BUSCAR: no vacío, solo números, exactamente 9 dígitos.
    /// Usa ON DELETE CASCADE en la base de datos.
    /// </summary>
    public partial class FormDarBajaCliente: Form
    {
        private string duiCliente; // Almacena el DUI válido tras búsqueda

        /// <summary>
        /// Constructor del formulario.
        /// Recibe el cargo del usuario para validar permisos.
        /// </summary>
        /// <param name="cargo">Rol del usuario (cajero, asesor, gerente)</param>
        public FormDarBajaCliente(string cargo)
        {
            InitializeComponent();
            ConfigurarPermisos(cargo);
            txtDUI.Focus();
        }
        /// <summary>
        /// Valida que el usuario tenga permisos para dar de baja.
        /// Solo asesor y gerente pueden acceder.
        /// </summary>
        /// <param name="cargo">Rol del usuario</param>
        private void ConfigurarPermisos(string cargo)
        {
            if (cargo.ToLower() != "asesor" && cargo.ToLower() != "gerente")
            {
                MessageBox.Show(
                    "Acceso denegado. Solo gerentes pueden dar de baja clientes.",
                    "Permiso Requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop
                );
                this.Close(); // Cierra el formulario si no tiene permiso
            }
        }
        // ========================================
        // VALIDACIÓN DUI (9 DÍGITOS, SIN ESPACIOS)
        // ========================================
        private void txtDUI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // PERMITIR SOLO NÚMEROS (0-9) Y TECLA BORRAR (Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Bloquea cualquier otra tecla
                return;
            }

            //  MÁXIMO 9 DÍGITOS (excepto borrar)
            if (txtDUI.Text.Length >= 9 && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                MessageBox.Show(
                    "El DUI debe tener exactamente 9 dígitos.\nUse ← (Backspace) para corregir.",
                    "Límite de 9 dígitos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
        /// <summary>
        /// Evento del botón BUSCAR.
        /// Valida DUI y busca al cliente en la base de datos.
        /// </summary>
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string dui = txtDUI.Text.Trim();

            // === VALIDACIÓN COMPLETA DEL DUI ===
            if (string.IsNullOrEmpty(dui))
            {
                MessageBox.Show(
                    "El campo DUI no puede estar vacío.",
                    "Campo Requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtDUI.Focus();
                return;
            }

            if (!long.TryParse(dui, out _))
            {
                MessageBox.Show(
                    "El DUI solo puede contener números (0-9).",
                    "Solo Números",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtDUI.Focus();
                txtDUI.SelectAll();
                return;
            }

            // === SI PASA TODAS LAS VALIDACIONES → BUSCAR ===
            duiCliente = dui;
            BuscarClienteEnBaseDatos(duiCliente);
        }

        /// <summary>
        /// Busca al cliente en la base de datos.
        /// </summary>
        /// <param name="dui">DUI validado</param>
        private void BuscarClienteEnBaseDatos(string dui)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sql = @"
                        SELECT p.Nombre, p.Apellido, c.BilleteraVirtual 
                        FROM Personas p
                        JOIN Clientes c ON p.DUI = c.DUI
                        WHERE p.DUI = @dui";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@dui", dui);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Mostrar datos
                                txtNombre.Text = $"Cliente: {reader["Nombre"]} {reader["Apellido"]}";
                                lblSaldo.Text = $"Saldo actual: ${Convert.ToDecimal(reader["BilleteraVirtual"]):F2}";
                                txtNombre.Visible = true;
                                lblSaldo.Visible = true;
                                btnProcesarBaja.Enabled = true;

                                // Advertencia si tiene saldo
                                if (Convert.ToDecimal(reader["BilleteraVirtual"]) > 0)
                                {
                                    lblAdvertencia.Text = "¡ADVERTENCIA! El cliente tiene saldo en su billetera.";
                                    lblAdvertencia.ForeColor = System.Drawing.Color.Red;
                                    lblAdvertencia.Visible = true;
                                }
                                else
                                {
                                    lblAdvertencia.Visible = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Cliente no encontrado con ese DUI.",
                                    "No Encontrado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                                LimpiarDatos();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error al buscar cliente: " + ex.Message,
                        "Error de Base de Datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
        /// <summary>
        /// Evento del botón DAR DE BAJA.
        /// Elimina el cliente (usa ON DELETE CASCADE).
        /// </summary>
        private void btnProcesarBaja_Click(object sender, EventArgs e)
        {
            //  VALIDAR TODOS LOS CAMPOS DE ENTRADA 
            // ❌ Antes: if (!ValidarCampo(txtSolicitante, "Nombre del solicitante")) return;
            // ✅ Ahora:
            if (!ValidarNombreCompleto(txtSolicitante, "Nombre y Apellido del Solicitante")) return;

            if (!ValidarComboBox(cmbParentesco, "Parentesco")) return;
            if (!ValidarFechaFallecimiento()) return;
            bool ValidarNombreCompleto(TextBox txt, string nombreCampo)
            {
                string texto = txt.Text.Trim();

                // 1. Validar que no esté vacío (reusa la lógica de ValidarCampo)
                if (string.IsNullOrWhiteSpace(texto))
                {
                    MostrarAdvertencia($"El campo '{nombreCampo}' es obligatorio.");
                    txt.Focus();
                    return false;
                }

                // 2. Validar que contenga al menos dos palabras (Nombre y Apellido)
                // Divide el texto por espacios y verifica que haya 2 o más partes.
                string[] partes = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length < 2)
                {
                    MostrarAdvertencia($"El campo '{nombreCampo}' debe contener al menos el **Nombre** y el **Apellido** del solicitante.");
                    txt.Focus();
                    txt.SelectAll();
                    return false;
                }

                return true;
            }

            //SI TODO ESTÁ LLENO → CONFIRMAR 
            ConfirmarYProcesarBaja();
        }

        private bool ValidarCampo(TextBox txt, string nombre)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                MostrarAdvertencia($"El campo '{nombre}' es obligatorio.");
                txt.Focus();
                return false;
            }
            return true;
        }

        private bool ValidarComboBox(ComboBox cmb, string nombre)
        {
            if (cmb.SelectedIndex == -1)
            {
                MostrarAdvertencia($"Debe seleccionar un '{nombre}'.");
                cmb.Focus();
                return false;
            }
            return true;
        }

        private bool ValidarFechaFallecimiento()
        {
            if (dtpFechaFallecimiento.Value > DateTime.Today)
            {
                MostrarAdvertencia("La fecha de fallecimiento no puede ser futura.");
                dtpFechaFallecimiento.Focus();
                return false;
            }
            return true;
        }

        private void MostrarAdvertencia(string mensaje)
        {
            MessageBox.Show(mensaje, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ConfirmarYProcesarBaja()
        {
            string resumen = $"DUI: {duiCliente}\n" +
                            $"Cliente: {txtNombre.Text}\n" +
                            $"Solicitante: {txtSolicitante.Text}\n" +
                            $"Parentesco: {cmbParentesco.SelectedItem}\n" +
                            $"Fecha fallecimiento: {dtpFechaFallecimiento.Value:dd/MM/yyyy}";

            var confirm = MessageBox.Show(
                $"¿Confirmar baja por fallecimiento?\n\n{resumen}",
                "Confirmar Baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                EjecutarBajaEnBaseDatos();
            }
        }

        private void EjecutarBajaEnBaseDatos()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM Personas WHERE DUI = @dui";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@dui", duiCliente);
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                        MessageBox.Show("Baja procesada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // <summary>
        /// Evento del botón CANCELAR.
        /// Cierra el formulario sin hacer cambios.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Limpia los datos mostrados del cliente.
        /// </summary>
        private void LimpiarDatos()
        {
            txtNombre.Visible = false;
            lblSaldo.Visible = false;
            lblAdvertencia.Visible = false;
            btnProcesarBaja.Enabled = false;
            duiCliente = null;
            txtDUI.Focus();
        }
        
    }
}