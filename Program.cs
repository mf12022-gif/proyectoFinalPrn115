using System;
using System.Collections.Generic;
using System.Linq;
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

            // Deja que frmLogin controle el cierre
            Application.Run(new FormLogin());
        }
    }
}