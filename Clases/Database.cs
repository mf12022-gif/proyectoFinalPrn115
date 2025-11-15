using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoFinalPrn115.Clases
{
    /// <summary>
    /// Clase estática que maneja la conexión global a la base de datos
    /// y proporciona métodos reutilizables en todo el sistema.
    /// </summary>
    public class Database
    {
        // DATOS DE CONEXIÓN
        private static string servidor = "localhost";
        private static string bd = "igucosadb";
        private static string usuario = "root";
        private static string password = "root";

        // CADENA DE CONEXIÓN
        private static string cadenaConexion =
            $"Server={servidor};Database={bd};Uid={usuario};Pwd={password};";

        // CONEXIÓN GLOBAL (opcional, para cerrar al final)
        private static MySqlConnection conexionGlobal;

        // MÉTODO: Devuelve una nueva conexión
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(cadenaConexion);
        }

        // MÉTODO: Prueba la conexión (para el botón)
        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(cadenaConexion))
                {
                    conn.Open();
                    mensaje = $"Conexión exitosa a `{bd}`";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}\n\n" +
                          $"• Asegúrate de que XAMPP esté encendido\n" +
                          $"• MySQL esté en puerto 3306\n" +
                          $"• La base `{bd}` exista";
                return false;
            }
        }

        // MÉTODO: Abre conexión global (opcional)
        public static bool AbrirConexionGlobal()
        {
            try
            {
                if (conexionGlobal == null || conexionGlobal.State != System.Data.ConnectionState.Open)
                {
                    conexionGlobal = new MySqlConnection(cadenaConexion);
                    conexionGlobal.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // MÉTODO: Cierra conexión global al salir
        public static void CerrarConexionGlobal()
        {
            if (conexionGlobal != null)
            {
                try
                {
                    if (conexionGlobal.State == System.Data.ConnectionState.Open)
                        conexionGlobal.Close();
                    conexionGlobal.Dispose();
                }
                catch { }
                conexionGlobal = null;
            }
        }
    }
}
