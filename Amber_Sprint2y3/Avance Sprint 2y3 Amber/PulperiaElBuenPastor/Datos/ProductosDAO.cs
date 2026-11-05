using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PulperiaElBuenPastor.Datos
{
    public class ProductosDAO
    {
        // CONSULTAR PRODUCTOS
        public DataTable ListarProductos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        p.producto_id,
                        p.codigo,
                        p.nombre,
                        c.nombre AS categoria,
                        p.unidad,
                        p.costo,
                        p.precio_venta,
                        p.stock_actual,
                        p.stock_minimo,
                        p.estado
                    FROM dbo.Productos p
                    INNER JOIN dbo.Categorias c
                        ON p.categoria_id = c.categoria_id
                    ORDER BY p.nombre;";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(consulta, cn))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // REGISTRAR PRODUCTOS
        public void GuardarProducto(
            string codigo,
            string nombre,
            int categoriaId,
            string unidad,
            decimal costo,
            decimal precioVenta,
            decimal stockMinimo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");

            if (categoriaId <= 0)
                throw new Exception("Seleccione una categoría válida.");

            if (costo < 0)
                throw new Exception("El costo no puede ser negativo.");

            if (precioVenta <= 0)
                throw new Exception("El precio debe ser mayor que cero.");

            if (stockMinimo < 0)
                throw new Exception("El stock mínimo no puede ser negativo.");

            if (nombre.Trim().Length > 120)
                throw new Exception("El nombre supera los 120 caracteres.");

            if (!string.IsNullOrWhiteSpace(codigo) && codigo.Trim().Length > 40)
                throw new Exception("El código supera los 40 caracteres.");

            if (!string.IsNullOrWhiteSpace(unidad) && unidad.Trim().Length > 20)
                throw new Exception("La unidad supera los 20 caracteres.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
                    INSERT INTO dbo.Productos
                    (
                        codigo,
                        nombre,
                        categoria_id,
                        unidad,
                        costo,
                        precio_venta,
                        stock_actual,
                        stock_minimo,
                        estado
                    )
                    VALUES
                    (
                        @Codigo,
                        @Nombre,
                        @CategoriaID,
                        @Unidad,
                        @Costo,
                        @PrecioVenta,
                        0,
                        @StockMinimo,
                        1
                    );";

                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 40).Value =
                        string.IsNullOrWhiteSpace(codigo)
                        ? DBNull.Value
                        : (object)codigo.Trim();

                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value =
                        nombre.Trim();

                    cmd.Parameters.Add("@CategoriaID", SqlDbType.Int).Value =
                        categoriaId;

                    cmd.Parameters.Add("@Unidad", SqlDbType.NVarChar, 20).Value =
                        string.IsNullOrWhiteSpace(unidad)
                        ? "unidad"
                        : unidad.Trim();

                    SqlParameter pCosto =
                        cmd.Parameters.Add("@Costo", SqlDbType.Decimal);
                    pCosto.Precision = 10;
                    pCosto.Scale = 2;
                    pCosto.Value = costo;

                    SqlParameter pPrecio =
                        cmd.Parameters.Add("@PrecioVenta", SqlDbType.Decimal);
                    pPrecio.Precision = 10;
                    pPrecio.Scale = 2;
                    pPrecio.Value = precioVenta;

                    SqlParameter pStock =
                        cmd.Parameters.Add("@StockMinimo", SqlDbType.Decimal);
                    pStock.Precision = 10;
                    pStock.Scale = 2;
                    pStock.Value = stockMinimo;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ACTUALIZAR INFORMACIÓN DEL PRODUCTO
        public void ActualizarProducto(
            int productoId,
            string codigo,
            string nombre,
            int categoriaId,
            string unidad,
            decimal costo,
            decimal stockMinimo,
            bool estado)
        {
            if (productoId <= 0)
                throw new Exception("Producto no válido.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");

            if (categoriaId <= 0)
                throw new Exception("Seleccione una categoría.");

            if (costo < 0)
                throw new Exception("El costo no puede ser negativo.");

            if (stockMinimo < 0)
                throw new Exception("El stock mínimo no puede ser negativo.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
            UPDATE dbo.Productos
            SET
                codigo = @Codigo,
                nombre = @Nombre,
                categoria_id = @CategoriaID,
                unidad = @Unidad,
                costo = @Costo,
                stock_minimo = @StockMinimo,
                estado = @Estado
            WHERE producto_id = @ProductoID;";

                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 40).Value =
                        string.IsNullOrWhiteSpace(codigo)
                        ? DBNull.Value
                        : (object)codigo.Trim();

                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 120).Value =
                        nombre.Trim();

                    cmd.Parameters.Add("@CategoriaID", SqlDbType.Int).Value =
                        categoriaId;

                    cmd.Parameters.Add("@Unidad", SqlDbType.NVarChar, 20).Value =
                        string.IsNullOrWhiteSpace(unidad)
                        ? "unidad"
                        : unidad.Trim();

                    SqlParameter pCosto =
                        cmd.Parameters.Add("@Costo", SqlDbType.Decimal);
                    pCosto.Precision = 10;
                    pCosto.Scale = 2;
                    pCosto.Value = costo;

                    SqlParameter pMinimo =
                        cmd.Parameters.Add("@StockMinimo", SqlDbType.Decimal);
                    pMinimo.Precision = 10;
                    pMinimo.Scale = 2;
                    pMinimo.Value = stockMinimo;

                    cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;

                    cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value =
                        productoId;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("No se encontró el producto.");
                }
            }
        }
    // ACTUALIZAR COSTO Y PRECIO DE VENTA
public void ActualizarPrecio(
    int productoId,
    decimal nuevoCosto,
    decimal nuevoPrecio)
        {
            if (productoId <= 0)
                throw new Exception("Producto no válido.");

            if (nuevoCosto < 0)
                throw new Exception("El costo no puede ser negativo.");

            if (nuevoPrecio <= 0)
                throw new Exception("El precio debe ser mayor que cero.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = @"
            UPDATE dbo.Productos
            SET
                costo = @Costo,
                precio_venta = @Precio
            WHERE producto_id = @ProductoID;";

                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    SqlParameter pCosto =
                        cmd.Parameters.Add("@Costo", SqlDbType.Decimal);
                    pCosto.Precision = 10;
                    pCosto.Scale = 2;
                    pCosto.Value = nuevoCosto;

                    SqlParameter pPrecio =
                        cmd.Parameters.Add("@Precio", SqlDbType.Decimal);
                    pPrecio.Precision = 10;
                    pPrecio.Scale = 2;
                    pPrecio.Value = nuevoPrecio;

                    cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value =
                        productoId;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("No se encontró el producto.");
                }
            }
        }
    }
}