using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PulperiaElBuenPastor.Datos
{
    public class CategoriasDAO
    {
        // Consultar todas las categorías
        public DataTable ListarCategorias()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string consulta = @"
                    SELECT categoria_id, nombre, descripcion
                    FROM dbo.Categorias
                    ORDER BY nombre";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(consulta, cn))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // Registrar una categoría
        public void GuardarCategoria(
            string nombre,
            string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
                    INSERT INTO dbo.Categorias
                    (nombre, descripcion)
                    VALUES
                    (@Nombre, @Descripcion)";

                using (SqlCommand cmd =
                    new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add(
                        "@Nombre", SqlDbType.NVarChar, 70
                    ).Value = nombre.Trim();

                    cmd.Parameters.Add(
                        "@Descripcion", SqlDbType.NVarChar, 150
                    ).Value = string.IsNullOrWhiteSpace(descripcion)
                        ? DBNull.Value
                        : (object)descripcion.Trim();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Actualizar una categoría
        public void ActualizarCategoria(
            int categoriaId,
            string nombre,
            string descripcion)
        {
            if (categoriaId <= 0)
                throw new Exception("Categoría no válida.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
                    UPDATE dbo.Categorias
                    SET nombre = @Nombre,
                        descripcion = @Descripcion
                    WHERE categoria_id = @Id";

                using (SqlCommand cmd =
                    new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add(
                        "@Nombre", SqlDbType.NVarChar, 70
                    ).Value = nombre.Trim();

                    cmd.Parameters.Add(
                        "@Descripcion", SqlDbType.NVarChar, 150
                    ).Value = string.IsNullOrWhiteSpace(descripcion)
                        ? DBNull.Value
                        : (object)descripcion.Trim();

                    cmd.Parameters.Add(
                        "@Id", SqlDbType.Int
                    ).Value = categoriaId;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("No se encontró la categoría.");
                }
            }
        }

        // Verificar si una categoría tiene productos relacionados
        public bool TieneProductosAsociados(int categoriaId)
        {
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
                    SELECT COUNT(*)
                    FROM dbo.Productos
                    WHERE categoria_id = @Id";

                using (SqlCommand cmd =
                    new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add(
                        "@Id", SqlDbType.Int
                    ).Value = categoriaId;

                    return Convert.ToInt32(
                        cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}