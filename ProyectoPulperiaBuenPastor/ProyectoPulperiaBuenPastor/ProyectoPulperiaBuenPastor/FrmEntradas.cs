
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoPulperiaBuenPastor
{
    public partial class FrmEntradas : Form
    {
        private ComboBox cmbProducto;
        private ComboBox cmbTipoMovimiento;
        private TextBox txtCantidad;
        private TextBox txtMotivo;
        private Button btnRegistrar;
        private Button btnLimpiar;
        private DataGridView dgvMovimientos;

        public FrmEntradas()
        {
            InitializeComponent();
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            Text = "Movimientos de inventario";
            Size = new Size(950, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            Label titulo = new Label
            {
                Text = "MOVIMIENTOS DE INVENTARIO",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true,
                ForeColor = Color.SaddleBrown
            };
            Controls.Add(titulo);

            CrearEtiqueta("Producto:", 40, 85);

            cmbProducto = new ComboBox
            {
                Location = new Point(200, 80),
                Size = new Size(350, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbProducto.Items.AddRange(new object[]
            {
                "Producto de prueba 1",
                "Producto de prueba 2",
                "Producto de prueba 3"
            });

            Controls.Add(cmbProducto);

            CrearEtiqueta("Tipo de movimiento:", 40, 130);

            cmbTipoMovimiento = new ComboBox
            {
                Location = new Point(200, 125),
                Size = new Size(350, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbTipoMovimiento.Items.AddRange(new object[]
            {
                "Entrada",
                "Salida",
                "Ajuste"
            });

            Controls.Add(cmbTipoMovimiento);

            CrearEtiqueta("Cantidad:", 40, 175);

            txtCantidad = new TextBox
            {
                Location = new Point(200, 170),
                Size = new Size(350, 30)
            };

            Controls.Add(txtCantidad);

            CrearEtiqueta("Motivo:", 40, 220);

            txtMotivo = new TextBox
            {
                Location = new Point(200, 215),
                Size = new Size(350, 65),
                Multiline = true,
                MaxLength = 200
            };

            Controls.Add(txtMotivo);

            btnRegistrar = new Button
            {
                Text = "Registrar movimiento",
                Location = new Point(200, 300),
                Size = new Size(180, 40),
                BackColor = Color.SaddleBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnRegistrar.Click += BtnRegistrar_Click;
            Controls.Add(btnRegistrar);

            btnLimpiar = new Button
            {
                Text = "Limpiar",
                Location = new Point(400, 300),
                Size = new Size(150, 40)
            };

            btnLimpiar.Click += BtnLimpiar_Click;
            Controls.Add(btnLimpiar);

            Label lblHistorial = new Label
            {
                Text = "HISTORIAL DE MOVIMIENTOS",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(30, 375),
                AutoSize = true,
                ForeColor = Color.SaddleBrown
            };

            Controls.Add(lblHistorial);

            dgvMovimientos = new DataGridView
            {
                Location = new Point(30, 415),
                Size = new Size(870, 250),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            dgvMovimientos.Columns.Add("Producto", "Producto");
            dgvMovimientos.Columns.Add("Tipo", "Tipo de movimiento");
            dgvMovimientos.Columns.Add("Cantidad", "Cantidad");
            dgvMovimientos.Columns.Add("Motivo", "Motivo");

            Controls.Add(dgvMovimientos);
        }

        private void CrearEtiqueta(string texto, int x, int y)
        {
            Controls.Add(new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Arial", 10)
            });
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            if (cmbTipoMovimiento.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el tipo de movimiento.");
                return;
            }

            if (!decimal.TryParse(txtCantidad.Text, out decimal cantidad)
                || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad mayor que cero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("El motivo es obligatorio.");
                return;
            }

            dgvMovimientos.Rows.Add(
                cmbProducto.Text,
                cmbTipoMovimiento.Text,
                cantidad,
                txtMotivo.Text.Trim()
            );

            MessageBox.Show(
                "Movimiento agregado al historial correctamente.",
                "Confirmación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            LimpiarCampos();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            cmbProducto.SelectedIndex = -1;
            cmbTipoMovimiento.SelectedIndex = -1;
            txtCantidad.Clear();
            txtMotivo.Clear();
        }
    }
}
