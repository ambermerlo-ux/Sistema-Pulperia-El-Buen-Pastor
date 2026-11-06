using System;
using System.Data.SqlClient;

namespace PulperiaElBuenPastor
{
    public static class ControlRoles
    {
        private static string conexionString = "Server=localhost;Database=PulperiaElBuenPastor;Trusted_Connection=True;";

        public static bool EsAdministrador(int usuarioId)
        {
            bool esAdmin = false;
            string query = @"SELECT r.nombre 
                             FROM Usuario_Rol ur 
                             INNER JOIN Roles r ON ur.rol_id = r.rol_id 
                             WHERE ur.usuario_id = @UsuarioId AND r.nombre = 'Administrador'";

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    try
                    {
                        conexion.Open();
                        object resultado = comando.ExecuteScalar();
                        if (resultado != null) esAdmin = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al verificar rol: " + ex.Message);
                    }
                }
            }
            return esAdmin;
        }

        public static bool ValidarPermiso(int usuarioId, string accionPermitida)
        {
            if (accionPermitida == "GestionarUsuarios" || accionPermitida == "AnularVenta")
            {
                return EsAdministrador(usuarioId);
            }
            return true;
        }
    }
}
