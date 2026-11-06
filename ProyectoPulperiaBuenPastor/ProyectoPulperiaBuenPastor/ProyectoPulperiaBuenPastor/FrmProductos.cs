
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoPulperiaBuenPastor
{
    public partial class FrmProductos : Form
    {
        private TextBox txtCodigo;
        private TextBox txtNombre;
        private ComboBox cmbCategoria;
        private ComboBox cmbUnidad;
        private TextBox txtCosto;
        private TextBox txtPrecio;
        private TextBox txtStockMinimo;
        private TextBox txtBuscar;
        private DataGridView dgvProductos;

        public FrmProductos()
        {
            InitializeComponent();
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            Text = "Gestión de productos";
            Size = new Size(1150, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label titulo = new Label
            {
                Text = "GESTIÓN DE PRODUCTOS",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                Location = new Point(25, 20),
                AutoSize = true
            };

            Controls.Add(titulo);

            GroupBox grupo = new GroupBox
            {
                Text = "Información del producto",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(25, 75),
                Size = new Size(330, 510)
            };

            txtCodigo = CrearTexto(20, 55, grupo, "Código");
            txtNombre = CrearTexto(20, 125, grupo, "Nombre");

            cmbCategoria = new ComboBox
            {
                Location = new Point(20, 195),
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbCategoria.Items.AddRange(new object[]
            {
                "Abarrotes", "Bebidas", "Lácteos",
                "Limpieza", "Otros"
            });

            grupo.Controls.Add(CrearEtiqueta("Categoría", 20, 170));
            grupo.Controls.Add(cmbCategoria);

            cmbUnidad = new ComboBox
            {
                Location = new Point(20, 265),
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbUnidad.Items.AddRange(new object[]
            {
                "Unidad", "Libra", "Kilogramo",
                "Litro", "Paquete", "Caja"
            });

            grupo.Controls.Add(CrearEtiqueta("Unidad", 20, 240));
            grupo.Controls.Add(cmbUnidad);

            txtCosto = CrearTexto(20, 335, grupo, "Costo");
            txtPrecio = CrearTexto(20, 405, grupo, "Precio de venta");
            txtStockMinimo = CrearTexto(20, 475, grupo, "Stock mínimo");

            Controls.Add(grupo);

            Button btnGuardar = CrearBoton(
                "GUARDAR", 25, 610, Color.SaddleBrown);

            Button btnLimpiar = CrearBoton(
                "LIMPIAR", 150, 610, Color.Gray);

            btnGuardar.Click += BtnGuardar_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();

            Controls.Add(btnGuardar);
            Controls.Add(btnLimpiar);

            Label lblBuscar = new Label
            {
                Text = "Buscar producto:",
                Location = new Point(390, 85),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            txtBuscar = new TextBox
            {
                Location = new Point(510, 80),
                Width = 450
            };

            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);

            dgvProductos = new DataGridView
            {
                Location = new Point(390, 125),
                Size = new Size(710, 460),
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };

            dgvProductos.Columns.Add("codigo", "Código");
            dgvProductos.Columns.Add("nombre", "Nombre");
            dgvProductos.Columns.Add("categoria", "Categoría");
            dgvProductos.Columns.Add("unidad", "Unidad");
            dgvProductos.Columns.Add("costo", "Costo");
            dgvProductos.Columns.Add("precio", "Precio venta");
            dgvProductos.Columns.Add("stock", "Stock mínimo");

            Controls.Add(dgvProductos);
        }

        private TextBox CrearTexto(
            int x, int y, GroupBox grupo, string texto)
        {
            grupo.Controls.Add(CrearEtiqueta(texto, x, y - 25));

            TextBox caja = new TextBox
            {
                Location = new Point(x, y),
                Width = 280
            };

            grupo.Controls.Add(caja);
            return caja;
        }

        private Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private Button CrearBoton(
            string texto, int x, int y, Color color)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(110, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            decimal costo;
            decimal precio;
            decimal stock;

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Ingrese el código del producto.");
                txtCodigo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.");
                txtNombre.Focus();
                return;
            }

            if (cmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }

            if (cmbUnidad.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una unidad.");
                return;
            }

            if (!decimal.TryParse(txtCosto.Text, out costo) || costo < 0)
            {
                MessageBox.Show("Ingrese un costo válido.");
                txtCosto.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor que cero.");
                txtPrecio.Focus();
                return;
            }

            if (!decimal.TryParse(txtStockMinimo.Text, out stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock mínimo válido.");
                txtStockMinimo.Focus();
                return;
            }

            dgvProductos.Rows.Add(
                txtCodigo.Text.Trim(),
                txtNombre.Text.Trim(),
                cmbCategoria.Text,
                cmbUnidad.Text,
                costo.ToString("0.00"),
                precio.ToString("0.00"),
                stock.ToString("0.00")
            );

            MessageBox.Show("Producto agregado correctamente.");

            LimpiarCampos();
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                string codigo = Convert.ToString(
                    fila.Cells["codigo"].Value);

                string nombre = Convert.ToString(
                    fila.Cells["nombre"].Value);

                fila.Visible =
                    codigo.IndexOf(txtBuscar.Text,
                        StringComparison.OrdinalIgnoreCase) >= 0
                    ||
                    nombre.IndexOf(txtBuscar.Text,
                        StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbUnidad.SelectedIndex = -1;
            txtCosto.Clear();
            txtPrecio.Clear();
            txtStockMinimo.Clear();

            txtCodigo.Focus();
        }
    }
}
