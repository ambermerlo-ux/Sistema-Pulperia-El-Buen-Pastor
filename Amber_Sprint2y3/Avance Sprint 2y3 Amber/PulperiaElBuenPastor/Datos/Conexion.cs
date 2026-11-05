using System;
using Microsoft.Data.SqlClient;

namespace PulperiaElBuenPastor.Datos
{
    public static class Conexion
    {
        // Si la instancia de SQL Server tiene otro nombre, cambie .\\SQLEXPRESS
        // por el nombre exacto que aparece en SSMS.
        // Integrated Security usa la cuenta de Windows; no almacene contraseñas aquí.
        private static readonly string cadenaConexion =
            @"Server=DESKTOP-NSGSTHB\SQLEXPRESS;Database=PulperiaElBuenPastor;Integrated Security=True;TrustServerCertificate=True;";
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (SqlConnection conexion = ObtenerConexion())
                {
                    conexion.Open();
                }

                mensaje = "Conexión exitosa con PulperiaElBuenPastor.";
                return true;
            }
            catch (SqlException ex)
            {
                mensaje = "No se pudo conectar a SQL Server. Verifique el nombre de la instancia, que el servicio esté iniciado y que la base exista. Detalle: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error al probar la conexión: " + ex.Message;
                return false;
            }
        }
    }
}
