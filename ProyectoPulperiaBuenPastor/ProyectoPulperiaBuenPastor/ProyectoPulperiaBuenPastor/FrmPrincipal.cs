
using System;
using System.Drawing;
using System.Windows.Forms;
using ProyectoPulperiaBuenPastor;

namespace PulperiaElBuenPastor
{
    public partial class FrmPrincipal : Form
    {
        private Panel panelContenido;

        public FrmPrincipal()
        {
            ConstruirFormulario();
        }

        private void ConstruirFormulario()
        {
            Text = "Sistema de Gestión - Pulpería El Buen Pastor";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            Panel menu = new Panel();
            menu.Dock = DockStyle.Left;
            menu.Width = 230;
            menu.BackColor = Color.SaddleBrown;

            Label titulo = new Label();
            titulo.Text = "EL BUEN PASTOR";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Arial", 16, FontStyle.Bold);
            titulo.Location = new Point(20, 30);
            titulo.AutoSize = true;

            menu.Controls.Add(titulo);

            string[] opciones =
            {
                "Inicio",
                "Usuarios",
                "Roles y permisos",
                "Productos",
                "Inventario",
                "Entradas",
                "Ventas",
                "Fiados y pagos",
                "Reportes",
                "Cerrar sesión"
            };

            int posicion = 100;

            foreach (string opcion in opciones)
            {
                Button boton = new Button();
                boton.Text = opcion;
                boton.Location = new Point(15, posicion);
                boton.Size = new Size(200, 42);
                boton.ForeColor = Color.White;
                boton.BackColor = Color.SaddleBrown;
                boton.FlatStyle = FlatStyle.Flat;
                boton.Font = new Font("Arial", 10);
                boton.FlatAppearance.BorderSize = 0;

                boton.Click += (sender, e) => AbrirModulo(opcion);

                menu.Controls.Add(boton);
                posicion += 50;
            }

            panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.BackColor = Color.WhiteSmoke;

            Label bienvenida = new Label();
            bienvenida.Text = "Bienvenido al Sistema de Gestión\nPulpería El Buen Pastor";
            bienvenida.Font = new Font("Arial", 23, FontStyle.Bold);
            bienvenida.ForeColor = Color.SaddleBrown;
            bienvenida.AutoSize = true;
            bienvenida.Location = new Point(150, 220);

            panelContenido.Controls.Add(bienvenida);

            Controls.Add(panelContenido);
            Controls.Add(menu);
        }

        private void AbrirModulo(string opcion)
        {
            if (opcion == "Cerrar sesión")
            {
                Close();
                return;
            }

            if (opcion == "Productos")
            {
                using (FrmProductos formulario = new FrmProductos())
                {
                    formulario.ShowDialog(this);
                }

                return;
            }

            if (opcion == "Entradas")
            {
                using (FrmEntradas formulario = new FrmEntradas())
                {
                    formulario.ShowDialog(this);
                }

                return;
            }

            MessageBox.Show("Módulo seleccionado: " + opcion);
        }
    }
}
