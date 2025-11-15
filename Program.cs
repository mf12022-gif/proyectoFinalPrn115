using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoFinalPrn115.Clases;

namespace proyectoFinalPrn115
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Inicia el formulario principal
                Application.Run(new FormLogin());
            }
            finally
            {
                // CIERRE TOTAL OBLIGATORIO
                Database.CerrarConexionGlobal();
                Application.Exit();           // Cierra todos los forms
                Environment.Exit(0);          // MATA EL PROCESO
            }
        }
    }
}