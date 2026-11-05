namespace PulperiaElBuenPastor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnProbarConexion = new Button();
            btnProbarCategorias = new Button();
            btnProbarProductos = new Button();
            btnActualizarPrecio = new Button();
            btnActualizarProducto = new Button();
            btnProbarInventario = new Button();
            btnProbarSalida = new Button();
            btnProbarAjuste = new Button();
            SuspendLayout();
            // 
            // btnProbarConexion
            // 
            btnProbarConexion.Location = new Point(27, 12);
            btnProbarConexion.Name = "btnProbarConexion";
            btnProbarConexion.Size = new Size(123, 128);
            btnProbarConexion.TabIndex = 0;
            btnProbarConexion.Text = "Probar conexión";
            btnProbarConexion.UseVisualStyleBackColor = true;
            btnProbarConexion.Click += btnProbarConexion_Click;
            // 
            // btnProbarCategorias
            // 
            btnProbarCategorias.Location = new Point(27, 156);
            btnProbarCategorias.Name = "btnProbarCategorias";
            btnProbarCategorias.Size = new Size(123, 128);
            btnProbarCategorias.TabIndex = 1;
            btnProbarCategorias.Text = "\tProbar categorías";
            btnProbarCategorias.UseVisualStyleBackColor = true;
            btnProbarCategorias.Click += btnProbarCategorias_Click;
            // 
            // btnProbarProductos
            // 
            btnProbarProductos.Location = new Point(27, 310);
            btnProbarProductos.Name = "btnProbarProductos";
            btnProbarProductos.Size = new Size(123, 128);
            btnProbarProductos.TabIndex = 2;
            btnProbarProductos.Text = "\tProbar productos";
            btnProbarProductos.UseVisualStyleBackColor = true;
            btnProbarProductos.Click += btnProbarProductos_Click;
            // 
            // btnActualizarPrecio
            // 
            btnActualizarPrecio.Location = new Point(176, 156);
            btnActualizarPrecio.Name = "btnActualizarPrecio";
            btnActualizarPrecio.Size = new Size(123, 128);
            btnActualizarPrecio.TabIndex = 3;
            btnActualizarPrecio.Text = "Actualizar precio";
            btnActualizarPrecio.UseVisualStyleBackColor = true;
            btnActualizarPrecio.Click += btnActualizarPrecio_Click;
            // 
            // btnActualizarProducto
            // 
            btnActualizarProducto.Location = new Point(176, 12);
            btnActualizarProducto.Name = "btnActualizarProducto";
            btnActualizarProducto.Size = new Size(123, 128);
            btnActualizarProducto.TabIndex = 4;
            btnActualizarProducto.Text = "Actualizar producto";
            btnActualizarProducto.UseVisualStyleBackColor = true;
            btnActualizarProducto.Click += button1_Click;
            // 
            // btnProbarInventario
            // 
            btnProbarInventario.Location = new Point(176, 310);
            btnProbarInventario.Name = "btnProbarInventario";
            btnProbarInventario.Size = new Size(123, 128);
            btnProbarInventario.TabIndex = 5;
            btnProbarInventario.Text = "\tProbar entrada de inventario";
            btnProbarInventario.UseVisualStyleBackColor = true;
            btnProbarInventario.Click += btnProbarInventario_Click;
            // 
            // btnProbarSalida
            // 
            btnProbarSalida.Location = new Point(325, 12);
            btnProbarSalida.Name = "btnProbarSalida";
            btnProbarSalida.Size = new Size(123, 128);
            btnProbarSalida.TabIndex = 6;
            btnProbarSalida.Text = "Probar salida";
            btnProbarSalida.UseVisualStyleBackColor = true;
            btnProbarSalida.Click += btnSalida_Click;
            // 
            // btnProbarAjuste
            // 
            btnProbarAjuste.Location = new Point(325, 156);
            btnProbarAjuste.Name = "btnProbarAjuste";
            btnProbarAjuste.Size = new Size(123, 128);
            btnProbarAjuste.TabIndex = 7;
            btnProbarAjuste.Text = "\tProbar ajuste";
            btnProbarAjuste.UseVisualStyleBackColor = true;
            btnProbarAjuste.Click += btnProbarAjuste_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnProbarAjuste);
            Controls.Add(btnProbarSalida);
            Controls.Add(btnProbarInventario);
            Controls.Add(btnActualizarProducto);
            Controls.Add(btnActualizarPrecio);
            Controls.Add(btnProbarProductos);
            Controls.Add(btnProbarCategorias);
            Controls.Add(btnProbarConexion);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnProbarConexion;
        private Button btnProbarCategorias;
        private Button btnProbarProductos;
        private Button btnActualizarPrecio;
        private Button btnActualizarProducto;
        private Button btnProbarInventario;
        private Button btnProbarSalida;
        private Button btnProbarAjuste;
    }
}
