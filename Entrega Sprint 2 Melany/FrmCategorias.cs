using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PulperiaElBuenPastor
{
    public partial class FrmCategorias : Form
    {
        private string conexionString = "Server=localhost;Database=PulperiaElBuenPastor;Trusted_Connection=True;";
        private int categoriaIdSeleccionada = 0;

        public FrmCategorias()
        {
            InitializeComponent();
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            string query = "SELECT categoria_id, nombre, descripcion FROM dbo.Categorias";
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCategorias.DataSource = dt;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que no existan nombres duplicados
            if (ExisteNombreDuplicado(nombre, categoriaIdSeleccionada))
            {
                MessageBox.Show("Ya existe una categoría registrada con este nombre.", "Nombre Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (categoriaIdSeleccionada == 0) // Crear
            {
                string query = "INSERT INTO dbo.Categorias (nombre, descripcion) VALUES (@Nombre, @Descripcion)";
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(descripcion) ? (object)DBNull.Value : descripcion);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Categoría creada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else // Modificar
            {
                string query = "UPDATE dbo.Categorias SET nombre = @Nombre, descripcion = @Descripcion WHERE categoria_id = @CategoriaId";
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@CategoriaId", categoriaIdSeleccionada);
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(descripcion) ? (object)DBNull.Value : descripcion);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Categoría actualizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            LimpiarFormulario();
            CargarCategorias();
        }

        private bool ExisteNombreDuplicado(string nombre, int idActual)
        {
            string query = "SELECT COUNT(*) FROM dbo.Categorias WHERE nombre = @Nombre AND categoria_id <> @IdActual";
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@IdActual", idActual);
                    conexion.Open();
                    int count = Convert.ToInt32(comando.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (categoriaIdSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una categoría para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Regla de integridad: verificar si tiene productos asociados
            string queryVerificar = "SELECT COUNT(*) FROM dbo.Productos WHERE categoria_id = @CategoriaId";
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand(queryVerificar, conexion))
                {
                    comando.Parameters.AddWithValue("@CategoriaId", categoriaIdSeleccionada);
                    conexion.Open();
                    int productosAsociados = Convert.ToInt32(comando.ExecuteScalar());

                    if (productosAsociados > 0)
                    {
                        MessageBox.Show("No se puede eliminar la categoría porque tiene productos asociados. Coordine con el equipo si se requiere desactivación.", "Regla de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }

            // Eliminar si no tiene restricciones
            string queryEliminar = "DELETE FROM dbo.Categorias WHERE categoria_id = @CategoriaId";
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand(queryEliminar, conexion))
                {
                    comando.Parameters.AddWithValue("@CategoriaId", categoriaIdSeleccionada);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Categoría eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LimpiarFormulario();
            CargarCategorias();
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvCategorias.Rows[e.RowIndex];
                categoriaIdSeleccionada = Convert.ToInt32(fila.Cells["categoria_id"].Value);
                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["descripcion"].Value != DBNull.Value ? fila.Cells["descripcion"].Value.ToString() : "";
            }
        }

        private void LimpiarFormulario()
        {
            categoriaIdSeleccionada = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
        }
    }
}
