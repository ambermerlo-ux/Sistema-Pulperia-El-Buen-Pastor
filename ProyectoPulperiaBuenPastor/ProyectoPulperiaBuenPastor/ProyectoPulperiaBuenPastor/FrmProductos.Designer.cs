
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoPulperiaBuenPastor
{
    partial class FrmProductos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "FrmProductos";
            Text = "Gestión de productos";

            ResumeLayout(false);
        }
    }
}
