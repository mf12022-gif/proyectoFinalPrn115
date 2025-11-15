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
        // DATOS DE CONEXIÓN A igucosadb
        private static string servidor = "localhost";
        private static string bd = "igucosadb";   //BASE DE DATOS
        private static string usuario = "root";
        private static string password = "root";

        

        // MÉTODO: Devuelve conexión
        public static MySqlConnection GetConnection()
        {
            // CADENA DE CONEXIÓN
            string cadenaConexion =
               $"Server={servidor};Database={bd};Uid={usuario};Pwd={password};";
            return new MySqlConnection(cadenaConexion);
        }
        public static void CerrarConexion(MySqlConnection conexionBD)
        {
            conexionBD.Close();
        }
    }
}
