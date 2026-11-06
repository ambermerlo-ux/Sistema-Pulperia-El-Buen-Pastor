using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PulperiaElBuenPastor
{
    public partial class FrmClientes : Form
    {
        private string conexionString = "Server=localhost;Database=PulperiaElBuenPastor;Trusted_Connection=True;";
        private int clienteIdSeleccionado = 0;

        public FrmClientes()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void CargarClientes()
        {
            string query = "SELECT cliente_id, nombre, telefono, estado FROM dbo.Clientes";
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvClientes.DataSource = dt; 
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string estado = cmbEstado.SelectedItem != null ? cmbEstado.SelectedItem.ToString() : "activo";

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("El nombre del cliente es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clienteIdSeleccionado == 0) 
            {
                string query = "INSERT INTO dbo.Clientes (nombre, telefono, estado) VALUES (@Nombre, @Telefono, @Estado)";
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
                        comando.Parameters.AddWithValue("@Estado", estado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Cliente registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else 
            {
                string query = "UPDATE dbo.Clientes SET nombre = @Nombre, telefono = @Telefono, estado = @Estado WHERE cliente_id = @ClienteId";
                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@ClienteId", clienteIdSeleccionado);
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
                        comando.Parameters.AddWithValue("@Estado", estado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Datos del cliente actualizados exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            LimpiarFormulario();
            CargarClientes();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];
                clienteIdSeleccionado = Convert.ToInt32(fila.Cells["cliente_id"].Value);
                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtTelefono.Text = fila.Cells["telefono"].Value != DBNull.Value ? fila.Cells["telefono"].Value.ToString() : "";
                cmbEstado.SelectedItem = fila.Cells["estado"].Value.ToString();
            }
        }

        private void LimpiarFormulario()
        {
            clienteIdSeleccionado = 0;
            txtNombre.Clear();
            txtTelefono.Clear();
            if (cmbEstado.Items.Count > 0) cmbEstado.SelectedIndex = 0;
        }
    }
}
