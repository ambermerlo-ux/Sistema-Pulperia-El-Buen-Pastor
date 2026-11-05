using System;
using Microsoft.Data.SqlClient;
using System.Data;
using PulperiaElBuenPastor.Datos;

namespace PulperiaElBuenPastor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            bool conectado = Conexion.ProbarConexion(out string mensaje);

            if (conectado)
            {
                MessageBox.Show(
                    mensaje,
                    "Conexión exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    mensaje,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnProbarCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                CategoriasDAO dao = new CategoriasDAO();

                // Consultar las categorías actuales
                DataTable categorias = dao.ListarCategorias();

                bool existe = false;

                foreach (DataRow fila in categorias.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(fila["nombre"]),
                        "Bebidas",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        existe = true;
                        break;
                    }
                }

                // Guardar solamente si todavía no existe
                if (!existe)
                {
                    dao.GuardarCategoria(
                        "Bebidas",
                        "Bebidas y refrescos"
                    );
                }

                // Volver a consultar las categorías
                categorias = dao.ListarCategorias();

                string listado = "";

                foreach (DataRow fila in categorias.Rows)
                {
                    listado +=
                        fila["categoria_id"] + " - " +
                        fila["nombre"] + "\n";
                }

                MessageBox.Show(
                    (existe
                        ? "La categoría ya estaba registrada."
                        : "Categoría guardada correctamente.")
                    + "\n\nCategorías registradas:\n"
                    + listado,
                    "Prueba de categorías",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al trabajar con categorías: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnProbarProductos_Click(object sender, EventArgs e)
        {
            try
            {
                // Consultar las categorías
                CategoriasDAO categoriasDAO = new CategoriasDAO();

                DataTable categorias = categoriasDAO.ListarCategorias();

                int categoriaId = 0;

                foreach (DataRow fila in categorias.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(fila["nombre"]),
                        "Bebidas",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        categoriaId = Convert.ToInt32(fila["categoria_id"]);
                        break;
                    }
                }

                if (categoriaId == 0)
                {
                    throw new Exception(
                        "No se encontró la categoría Bebidas. Regístrela primero.");
                }

                // Consultar los productos actuales
                ProductosDAO productosDAO = new ProductosDAO();

                DataTable productos = productosDAO.ListarProductos();

                bool existe = false;

                foreach (DataRow fila in productos.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(fila["codigo"]),
                        "PROD001",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        existe = true;
                        break;
                    }
                }

                // Registrar el producto si todavía no existe
                if (!existe)
                {
                    productosDAO.GuardarProducto(
                        "PROD001",
                        "Coca-Cola 600 ml",
                        categoriaId,
                        "unidad",
                        18.00m,
                        25.00m,
                        10.00m
                    );
                }

                // Consultar nuevamente
                productos = productosDAO.ListarProductos();

                string listado = "";

                foreach (DataRow fila in productos.Rows)
                {
                    listado +=
                        fila["codigo"] + " - " +
                        fila["nombre"] + " - " +
                        fila["categoria"] + "\n";
                }

                MessageBox.Show(
                    (existe
                        ? "El producto ya estaba registrado."
                        : "Producto registrado correctamente.")
                    + "\n\nProductos:\n" + listado,
                    "Prueba de productos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar producto: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnActualizarPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosDAO dao = new ProductosDAO();

                // Consultar los productos
                DataTable productos = dao.ListarProductos();

                int productoId = 0;

                // Buscar Coca-Cola por su código
                foreach (DataRow fila in productos.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(fila["codigo"]),
                        "PROD001",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        productoId = Convert.ToInt32(fila["producto_id"]);
                        break;
                    }
                }

                if (productoId == 0)
                    throw new Exception("No se encontró el producto PROD001.");

                // Actualizar costo y precio
                dao.ActualizarPrecio(
                    productoId,
                    19.00m,
                    27.00m
                );

                MessageBox.Show(
                    "El costo y precio se actualizaron correctamente.",
                    "Actualización de productos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al actualizar: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosDAO productosDAO = new ProductosDAO();
                CategoriasDAO categoriasDAO = new CategoriasDAO();

                // Buscar el producto
                DataTable productos = productosDAO.ListarProductos();

                int productoId = 0;

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToString(fila["codigo"]) == "PROD001")
                    {
                        productoId = Convert.ToInt32(fila["producto_id"]);
                        break;
                    }
                }

                if (productoId == 0)
                    throw new Exception("No se encontró el producto.");

                // Buscar la categoría Bebidas
                DataTable categorias = categoriasDAO.ListarCategorias();

                int categoriaId = 0;

                foreach (DataRow fila in categorias.Rows)
                {
                    if (Convert.ToString(fila["nombre"]) == "Bebidas")
                    {
                        categoriaId = Convert.ToInt32(fila["categoria_id"]);
                        break;
                    }
                }

                if (categoriaId == 0)
                    throw new Exception("No se encontró la categoría Bebidas.");

                // Actualizar información del producto
                productosDAO.ActualizarProducto(
                    productoId,
                    "PROD001",
                    "Coca-Cola 600 ml",
                    categoriaId,
                    "botella",
                    19.00m,
                    12.00m,
                    true
                );

                MessageBox.Show(
                    "Producto actualizado correctamente.",
                    "Actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnProbarInventario_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosDAO productosDAO = new ProductosDAO();

                MovimientosInventarioDAO movimientosDAO =
                    new MovimientosInventarioDAO();

                // Buscar el producto Coca-Cola
                DataTable productos = productosDAO.ListarProductos();

                int productoId = 0;

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToString(fila["codigo"]) == "PROD001")
                    {
                        productoId = Convert.ToInt32(fila["producto_id"]);
                        break;
                    }
                }

                if (productoId == 0)
                    throw new Exception("No se encontró Coca-Cola.");

                // Coloca aquí el ID real del usuario obtenido en SSMS
                int usuarioId = 1; // REEMPLAZAR por el ID correcto

                // Identificar si ya se realizó esta prueba
                DataTable historial =
                    movimientosDAO.ListarMovimientosPorProducto(productoId);

                bool pruebaRealizada = false;

                foreach (DataRow fila in historial.Rows)
                {
                    if (Convert.ToString(fila["tipo"]) == "ENTRADA" &&
                        Convert.ToInt32(fila["usuario_id"]) == usuarioId &&
                        Convert.ToString(fila["motivo"]) ==
                        "Prueba de entrada Sprint 3 - Amber")
                    {
                        pruebaRealizada = true;
                        break;
                    }
                }

                if (!pruebaRealizada)
                {
                    movimientosDAO.RegistrarEntrada(
                        productoId,
                        usuarioId,
                        5.00m,
                        "Prueba de entrada Sprint 3 - Amber"
                    );
                }

                // Consultar el producto actualizado
                productos = productosDAO.ListarProductos();

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToInt32(fila["producto_id"]) == productoId)
                    {
                        MessageBox.Show(
                            "Producto: " + fila["nombre"] +
                            "\nStock actual: " + fila["stock_actual"] +
                            "\nStock mínimo: " + fila["stock_minimo"] +
                            "\n\nMovimiento: " +
                            (pruebaRealizada ? "Ya registrado anteriormente." :
                            "Entrada registrada correctamente."),
                            "Prueba de inventario",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error en inventario: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosDAO productosDAO = new ProductosDAO();
                MovimientosInventarioDAO movimientosDAO =
                    new MovimientosInventarioDAO();

                DataTable productos = productosDAO.ListarProductos();

                int productoId = 0;
                decimal stockAntes = 0;

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToString(fila["codigo"]) == "PROD001")
                    {
                        productoId = Convert.ToInt32(fila["producto_id"]);
                        stockAntes = Convert.ToDecimal(fila["stock_actual"]);
                        break;
                    }
                }

                if (productoId == 0)
                    throw new Exception("No se encontró Coca-Cola.");

                int usuarioId = 1;
                string motivo = "Prueba salida Sprint 3";

                DataTable historial =
                    movimientosDAO.ListarMovimientosPorProducto(productoId);

                foreach (DataRow fila in historial.Rows)
                {
                    if (Convert.ToString(fila["tipo"]) == "SALIDA" &&
                        Convert.ToInt32(fila["usuario_id"]) == usuarioId &&
                        Convert.ToString(fila["motivo"]) == motivo)
                    {
                        MessageBox.Show(
                            "Esta prueba ya fue realizada.\nStock actual: " +
                            stockAntes,
                            "Prueba de salida"
                        );
                        return;
                    }
                }

                movimientosDAO.RegistrarSalida(
                    productoId,
                    usuarioId,
                    2.00m,
                    motivo
                );

                productos = productosDAO.ListarProductos();

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToInt32(fila["producto_id"]) == productoId)
                    {
                        decimal stockDespues =
                            Convert.ToDecimal(fila["stock_actual"]);

                        MessageBox.Show(
                            "Salida registrada correctamente." +
                            "\nStock anterior: " + stockAntes +
                            "\nStock actual: " + stockDespues,
                            "Prueba de inventario"
                        );

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error en la salida: " + ex.Message,
                    "Error de inventario"
                );
            }
        }

        private void btnProbarAjuste_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosDAO productosDAO = new ProductosDAO();
                MovimientosInventarioDAO movimientosDAO =
                    new MovimientosInventarioDAO();

                DataTable productos = productosDAO.ListarProductos();

                int productoId = 0;
                decimal stockAntes = 0;

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToString(fila["codigo"]) == "PROD001")
                    {
                        productoId = Convert.ToInt32(fila["producto_id"]);
                        stockAntes = Convert.ToDecimal(fila["stock_actual"]);
                        break;
                    }
                }

                if (productoId == 0)
                    throw new Exception("No se encontró Coca-Cola.");

                int usuarioId = 1;
                string motivo = "Prueba ajuste Sprint 3";

                DataTable historial =
                    movimientosDAO.ListarMovimientosPorProducto(productoId);

                foreach (DataRow fila in historial.Rows)
                {
                    if (Convert.ToString(fila["tipo"]) == "AJUSTE" &&
                        Convert.ToInt32(fila["usuario_id"]) == usuarioId &&
                        Convert.ToString(fila["motivo"]) == "AUMENTO: " + motivo)
                    {
                        MessageBox.Show("Esta prueba ya fue realizada.");
                        return;
                    }
                }

                movimientosDAO.RegistrarAjuste(
                    productoId,
                    usuarioId,
                    3.00m,
                    true,
                    motivo
                );

                productos = productosDAO.ListarProductos();

                foreach (DataRow fila in productos.Rows)
                {
                    if (Convert.ToInt32(fila["producto_id"]) == productoId)
                    {
                        decimal stockDespues =
                            Convert.ToDecimal(fila["stock_actual"]);

                        MessageBox.Show(
                            "Ajuste registrado correctamente." +
                            "\nStock anterior: " + stockAntes +
                            "\nStock actual: " + stockDespues,
                            "Prueba de ajuste"
                        );

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error en el ajuste: " + ex.Message,
                    "Error de inventario"
                );
            }
        }
    }
}




