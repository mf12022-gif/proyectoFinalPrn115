
namespace proyectoFinalPrn115
{
    partial class FormHistorial
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtBuscarDUI = new System.Windows.Forms.TextBox();
            this.btnBuscarDUI = new System.Windows.Forms.Button();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.lblTotalTransacciones = new System.Windows.Forms.Label();
            this.btnMostrarTodo = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(98, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DUI :";
            // 
            // txtBuscarDUI
            // 
            this.txtBuscarDUI.Location = new System.Drawing.Point(162, 84);
            this.txtBuscarDUI.Name = "txtBuscarDUI";
            this.txtBuscarDUI.Size = new System.Drawing.Size(121, 20);
            this.txtBuscarDUI.TabIndex = 1;
            this.txtBuscarDUI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBuscarDUI_KeyPress);
            // 
            // btnBuscarDUI
            // 
            this.btnBuscarDUI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscarDUI.Location = new System.Drawing.Point(313, 82);
            this.btnBuscarDUI.Name = "btnBuscarDUI";
            this.btnBuscarDUI.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarDUI.TabIndex = 2;
            this.btnBuscarDUI.Text = "BUSCAR";
            this.btnBuscarDUI.UseVisualStyleBackColor = true;
            this.btnBuscarDUI.Click += new System.EventHandler(this.btnBuscarDUI_Click);
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(3, 126);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.Size = new System.Drawing.Size(776, 245);
            this.dgvHistorial.TabIndex = 6;
            // 
            // lblTotalTransacciones
            // 
            this.lblTotalTransacciones.AutoSize = true;
            this.lblTotalTransacciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTransacciones.Location = new System.Drawing.Point(34, 402);
            this.lblTotalTransacciones.Name = "lblTotalTransacciones";
            this.lblTotalTransacciones.Size = new System.Drawing.Size(111, 13);
            this.lblTotalTransacciones.TabIndex = 7;
            this.lblTotalTransacciones.Text = "TRANSACCIONES";
            // 
            // btnMostrarTodo
            // 
            this.btnMostrarTodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMostrarTodo.Location = new System.Drawing.Point(433, 82);
            this.btnMostrarTodo.Name = "btnMostrarTodo";
            this.btnMostrarTodo.Size = new System.Drawing.Size(105, 23);
            this.btnMostrarTodo.TabIndex = 9;
            this.btnMostrarTodo.Text = "MOSTRAR TODO";
            this.btnMostrarTodo.UseVisualStyleBackColor = true;
            this.btnMostrarTodo.Click += new System.EventHandler(this.btnMostrarTodo_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.Location = new System.Drawing.Point(692, 402);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(87, 23);
            this.btnCerrar.TabIndex = 10;
            this.btnCerrar.Text = "CERRAR";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(239, -1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "COOPERATIVA IGUCOSA";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FormHistorial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnMostrarTodo);
            this.Controls.Add(this.lblTotalTransacciones);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.btnBuscarDUI);
            this.Controls.Add(this.txtBuscarDUI);
            this.Controls.Add(this.label1);
            this.Name = "FormHistorial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Historial-Transacciones";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBuscarDUI;
        private System.Windows.Forms.Button btnBuscarDUI;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Label lblTotalTransacciones;
        private System.Windows.Forms.Button btnMostrarTodo;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label label2;
    }
}