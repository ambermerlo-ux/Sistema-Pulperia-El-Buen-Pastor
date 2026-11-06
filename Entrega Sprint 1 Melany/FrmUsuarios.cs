using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PulperiaElBuenPastor
{
    public partial class FrmUsuarios : Form
    {
        private string conexionString = "Server=localhost;Database=PulperiaElBuenPastor;Trusted_Connection=True;";

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Usuarios (nombre, login, password_hash, estado) VALUES (@Nombre, @Login, HASHBYTES('SHA2_256', @Password), 1)";

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@Login", login);
                    comando.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al registrar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
