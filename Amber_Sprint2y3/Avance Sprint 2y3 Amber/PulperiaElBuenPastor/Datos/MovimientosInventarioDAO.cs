using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PulperiaElBuenPastor.Datos
{
    public class MovimientosInventarioDAO
    {
        // REGISTRAR ENTRADA DE PRODUCTOS
        public void RegistrarEntrada(
            int productoId,
            int usuarioId,
            decimal cantidad,
            string motivo)
        {
            if (productoId <= 0)
                throw new Exception("Seleccione un producto válido.");

            if (usuarioId <= 0)
                throw new Exception("Usuario no válido.");

            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("El motivo es obligatorio.");

            if (motivo.Trim().Length > 200)
                throw new Exception("El motivo no puede superar 200 caracteres.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction transaccion = cn.BeginTransaction())
                {
                    try
                    {
                        // Verificar que el usuario esté activo
                        string consultaUsuario = @"
                            SELECT COUNT(*)
                            FROM dbo.Usuarios
                            WHERE usuario_id = @UsuarioID
                            AND estado = 1;";

                        using (SqlCommand cmd = new SqlCommand(
                            consultaUsuario, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@UsuarioID", SqlDbType.Int
                            ).Value = usuarioId;

                            if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                                throw new Exception("El usuario no existe o está inactivo.");
                        }

                        // Consultar y bloquear el producto
                        decimal stockActual;

                        string consultaStock = @"
                            SELECT stock_actual
                            FROM dbo.Productos WITH (UPDLOCK, ROWLOCK)
                            WHERE producto_id = @ProductoID
                            AND estado = 1;";

                        using (SqlCommand cmd = new SqlCommand(
                            consultaStock, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ProductoID", SqlDbType.Int
                            ).Value = productoId;

                            object resultado = cmd.ExecuteScalar();

                            if (resultado == null ||
                                resultado == DBNull.Value)
                            {
                                throw new Exception(
                                    "El producto no existe o está inactivo.");
                            }

                            stockActual = Convert.ToDecimal(resultado);
                        }

                        decimal nuevoStock = stockActual + cantidad;

                        // Registrar movimiento
                        string insertarMovimiento = @"
                            INSERT INTO dbo.Movimientos_Inventario
                            (
                                producto_id,
                                usuario_id,
                                tipo,
                                cantidad,
                                motivo
                            )
                            VALUES
                            (
                                @ProductoID,
                                @UsuarioID,
                                N'ENTRADA',
                                @Cantidad,
                                @Motivo
                            );";

                        using (SqlCommand cmd = new SqlCommand(
                            insertarMovimiento, cn, transaccion))
                        {
                            cmd.Parameters.Add(
                                "@ProductoID", SqlDbType.Int
                            ).Value = productoId;

                            cmd.Parameters.Add(
                                "@UsuarioID", SqlDbType.Int
                            ).Value = usuarioId;

                            SqlParameter pCantidad =
                                cmd.Parameters.Add(
                                    "@Cantidad", SqlDbType.Decimal);

                            pCantidad.Precision = 10;
                            pCantidad.Scale = 2;
                            pCantidad.Value = cantidad;

                            cmd.Parameters.Add(
                                "@Motivo", SqlDbType.NVarChar, 200
                            ).Value = motivo.Trim();

                            cmd.ExecuteNonQuery();
                        }

                        // Actualizar existencias
                        string actualizarStock = @"
                            UPDATE dbo.Productos
                            SET stock_actual = @NuevoStock
                            WHERE producto_id = @ProductoID;";

                        using (SqlCommand cmd = new SqlCommand(
                            actualizarStock, cn, transaccion))
                        {
                            SqlParameter pStock =
                                cmd.Parameters.Add(
                                    "@NuevoStock", SqlDbType.Decimal);

                            pStock.Precision = 10;
                            pStock.Scale = 2;
                            pStock.Value = nuevoStock;

                            cmd.Parameters.Add(
                                "@ProductoID", SqlDbType.Int
                            ).Value = productoId;

                            cmd.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        // CONSULTAR HISTORIAL DE MOVIMIENTOS
        public DataTable ListarMovimientos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        m.movimiento_id,
                        p.nombre AS producto,
                        u.nombre AS responsable,
                        m.tipo,
                        m.cantidad,
                        m.motivo,
                        m.fecha
                    FROM dbo.Movimientos_Inventario m
                    INNER JOIN dbo.Productos p
                        ON m.producto_id = p.producto_id
                    INNER JOIN dbo.Usuarios u
                        ON m.usuario_id = u.usuario_id
                    ORDER BY m.fecha DESC;";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(consulta, cn))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // CONSULTAR MOVIMIENTOS DE UN PRODUCTO
        public DataTable ListarMovimientosPorProducto(int productoId)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        movimiento_id,
                        producto_id,
                        usuario_id,
                        tipo,
                        cantidad,
                        motivo,
                        fecha
                    FROM dbo.Movimientos_Inventario
                    WHERE producto_id = @ProductoID
                    ORDER BY fecha DESC;";

                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.Add(
                        "@ProductoID", SqlDbType.Int
                    ).Value = productoId;

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // CONSULTAR PRODUCTOS CON STOCK BAJO
        public DataTable ListarStockBajo()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        producto_id,
                        nombre,
                        stock_actual,
                        stock_minimo
                    FROM dbo.Productos
                    WHERE estado = 1
                    AND stock_actual <= stock_minimo
                    ORDER BY stock_actual ASC;";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(consulta, cn))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }
        public void RegistrarSalida(
        int productoId,
        int usuarioId,
        decimal cantidad,
        string motivo)
        {
            if (productoId <= 0 || usuarioId <= 0 || cantidad <= 0)
                throw new Exception("Los datos del movimiento no son válidos.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("Debe indicar el motivo de la salida.");

            if (motivo.Trim().Length > 200)
                throw new Exception("El motivo no puede superar 200 caracteres.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction tx =
                    cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        string usuarioSql = @"
                    SELECT COUNT(*)
                    FROM dbo.Usuarios
                    WHERE usuario_id = @UsuarioID
                    AND estado = 1";

                        using (SqlCommand cmd = new SqlCommand(usuarioSql, cn, tx))
                        {
                            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioId;

                            if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                                throw new Exception("El usuario no existe o está inactivo.");
                        }

                        string stockSql = @"
                    SELECT stock_actual
                    FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
                    WHERE producto_id = @ProductoID";

                        decimal stock;

                        using (SqlCommand cmd = new SqlCommand(stockSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;

                            object resultado = cmd.ExecuteScalar();

                            if (resultado == null || resultado == DBNull.Value)
                                throw new Exception("El producto no existe.");

                            stock = Convert.ToDecimal(resultado);
                        }

                        if (stock < cantidad)
                            throw new Exception("No hay existencias suficientes.");

                        string movimientoSql = @"
                    INSERT INTO dbo.Movimientos_Inventario
                    (producto_id, usuario_id, tipo, cantidad, motivo)
                    VALUES
                    (@ProductoID, @UsuarioID, 'SALIDA', @Cantidad, @Motivo)";

                        using (SqlCommand cmd = new SqlCommand(movimientoSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioId;

                            SqlParameter p = cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                            p.Precision = 10;
                            p.Scale = 2;
                            p.Value = cantidad;

                            cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 200).Value =
                                motivo.Trim();

                            cmd.ExecuteNonQuery();
                        }

                        string actualizarSql = @"
                    UPDATE dbo.Productos
                    SET stock_actual = stock_actual - @Cantidad
                    WHERE producto_id = @ProductoID
                    AND stock_actual >= @Cantidad";

                        using (SqlCommand cmd = new SqlCommand(actualizarSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                            cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;

                            if (cmd.ExecuteNonQuery() != 1)
                                throw new Exception("No se pudo actualizar el inventario.");
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    public void RegistrarAjuste(
    int productoId,
    int usuarioId,
    decimal cantidad,
    bool aumentar,
    string motivo)
        {
            if (productoId <= 0 || usuarioId <= 0 || cantidad <= 0)
                throw new Exception("Los datos del ajuste no son válidos.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("Debe indicar el motivo del ajuste.");

            string detalle = (aumentar ? "AUMENTO: " : "DISMINUCION: ")
                             + motivo.Trim();

            if (detalle.Length > 200)
                throw new Exception("El motivo supera los 200 caracteres.");

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction tx =
                    cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        string usuarioSql = @"
                    SELECT COUNT(*)
                    FROM dbo.Usuarios
                    WHERE usuario_id = @UsuarioID
                    AND estado = 1";

                        using (SqlCommand cmd = new SqlCommand(usuarioSql, cn, tx))
                        {
                            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioId;

                            if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                                throw new Exception("El usuario no existe o está inactivo.");
                        }

                        string stockSql = @"
                    SELECT stock_actual
                    FROM dbo.Productos WITH (UPDLOCK, HOLDLOCK)
                    WHERE producto_id = @ProductoID";

                        using (SqlCommand cmd = new SqlCommand(stockSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;

                            object resultado = cmd.ExecuteScalar();

                            if (resultado == null || resultado == DBNull.Value)
                                throw new Exception("El producto no existe.");

                            decimal stock = Convert.ToDecimal(resultado);

                            if (!aumentar && stock < cantidad)
                                throw new Exception("El ajuste dejaría existencias negativas.");
                        }

                        string actualizarSql = aumentar
                            ? @"UPDATE dbo.Productos
                        SET stock_actual = stock_actual + @Cantidad
                        WHERE producto_id = @ProductoID"
                            : @"UPDATE dbo.Productos
                        SET stock_actual = stock_actual - @Cantidad
                        WHERE producto_id = @ProductoID
                        AND stock_actual >= @Cantidad";

                        using (SqlCommand cmd = new SqlCommand(actualizarSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                            cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;

                            if (cmd.ExecuteNonQuery() != 1)
                                throw new Exception("No se pudo actualizar el stock.");
                        }

                        string movimientoSql = @"
                    INSERT INTO dbo.Movimientos_Inventario
                    (producto_id, usuario_id, tipo, cantidad, motivo)
                    VALUES
                    (@ProductoID, @UsuarioID, 'AJUSTE', @Cantidad, @Motivo)";

                        using (SqlCommand cmd = new SqlCommand(movimientoSql, cn, tx))
                        {
                            cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioId;
                            cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;
                            cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 200).Value = detalle;

                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
    }