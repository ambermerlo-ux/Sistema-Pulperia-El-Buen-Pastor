
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PulperiaElBuenPastor
{
    public partial class FrmLogin : Form
    {
        private TextBox txtUsuario;
        private TextBox txtContrasena;

        public FrmLogin()
        {
            InitializeComponent();
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            Text = "Inicio de sesión";
            Size = new Size(450, 450);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label titulo = new Label();
            titulo.Text = "PULPERÍA EL BUEN PASTOR";
            titulo.Font = new Font("Arial", 17, FontStyle.Bold);
            titulo.ForeColor = Color.SaddleBrown;
            titulo.AutoSize = true;
            titulo.Location = new Point(55, 45);

            Label subtitulo = new Label();
            subtitulo.Text = "Sistema de gestión y control";
            subtitulo.Location = new Point(125, 90);
            subtitulo.AutoSize = true;

            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario";
            lblUsuario.Location = new Point(65, 145);
            lblUsuario.AutoSize = true;

            txtUsuario = new TextBox();
            txtUsuario.Location = new Point(65, 170);
            txtUsuario.Width = 300;

            Label lblContrasena = new Label();
            lblContrasena.Text = "Contraseña";
            lblContrasena.Location = new Point(65, 220);
            lblContrasena.AutoSize = true;

            txtContrasena = new TextBox();
            txtContrasena.Location = new Point(65, 245);
            txtContrasena.Width = 300;
            txtContrasena.UseSystemPasswordChar = true;

            Button btnIngresar = new Button();
            btnIngresar.Text = "INICIAR SESIÓN";
            btnIngresar.Location = new Point(65, 310);
            btnIngresar.Size = new Size(300, 45);
            btnIngresar.BackColor = Color.SaddleBrown;
            btnIngresar.ForeColor = Color.White;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Click += BtnIngresar_Click;

            Controls.AddRange(new Control[]
            {
                titulo, subtitulo, lblUsuario, txtUsuario,
                lblContrasena, txtContrasena, btnIngresar
            });
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Ingrese su usuario y contraseña.");
                return;
            }

            MessageBox.Show("Inicio de sesión de prueba.");

            Hide();

            using (FrmPrincipal principal = new FrmPrincipal())
            {
                principal.ShowDialog();
            }

            Close();
        }
    }
}
