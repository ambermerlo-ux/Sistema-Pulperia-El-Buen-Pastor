
using System;
using Microsoft.Data.SqlClient;

namespace PulperiaElBuenPastor.Datos
{
    public class PruebaConexion
    {
        public static void Probar()
        {
            bool conectado = Conexion.ProbarConexion(out string mensaje);

            Console.WriteLine(mensaje);
        }
    }
}
