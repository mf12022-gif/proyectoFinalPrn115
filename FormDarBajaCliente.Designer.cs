
namespace proyectoFinalPrn115
{
    partial class FormDarBajaCliente
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
            this.txtDUI = new System.Windows.Forms.TextBox();
            this.btnBuscarDui = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbParentesco = new System.Windows.Forms.ComboBox();
            this.txtSolicitante = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpFechaFallecimiento = new System.Windows.Forms.DateTimePicker();
            this.btnProcesarBaja = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblAdvertencia = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DUI :";
            // 
            // txtDUI
            // 
            this.txtDUI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDUI.Location = new System.Drawing.Point(118, 80);
            this.txtDUI.Name = "txtDUI";
            this.txtDUI.Size = new System.Drawing.Size(158, 20);
            this.txtDUI.TabIndex = 1;
            this.txtDUI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDUI_KeyPress);
            // 
            // btnBuscarDui
            // 
            this.btnBuscarDui.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscarDui.Location = new System.Drawing.Point(283, 78);
            this.btnBuscarDui.Name = "btnBuscarDui";
            this.btnBuscarDui.Size = new System.Drawing.Size(80, 23);
            this.btnBuscarDui.TabIndex = 2;
            this.btnBuscarDui.Text = "BUSCAR CLIENTE";
            this.btnBuscarDui.UseVisualStyleBackColor = true;
            this.btnBuscarDui.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "NOMBRE :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "SALDO ACTUAL :";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(118, 119);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(158, 20);
            this.txtNombre.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "SOLICITANTE :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "PARENTESCO :";
            // 
            // cmbParentesco
            // 
            this.cmbParentesco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentesco.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbParentesco.FormattingEnabled = true;
            this.cmbParentesco.Items.AddRange(new object[] {
            "Padre",
            "Madre",
            "Esposo(a)",
            "Hijo(a)",
            "Hermano(a)",
            "Otro"});
            this.cmbParentesco.Location = new System.Drawing.Point(118, 227);
            this.cmbParentesco.Name = "cmbParentesco";
            this.cmbParentesco.Size = new System.Drawing.Size(158, 21);
            this.cmbParentesco.TabIndex = 8;
            // 
            // txtSolicitante
            // 
            this.txtSolicitante.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSolicitante.Location = new System.Drawing.Point(118, 197);
            this.txtSolicitante.Name = "txtSolicitante";
            this.txtSolicitante.Size = new System.Drawing.Size(158, 20);
            this.txtSolicitante.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "F.FALLECIMIENTO :";
            // 
            // dtpFechaFallecimiento
            // 
            this.dtpFechaFallecimiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaFallecimiento.Location = new System.Drawing.Point(141, 259);
            this.dtpFechaFallecimiento.Name = "dtpFechaFallecimiento";
            this.dtpFechaFallecimiento.Size = new System.Drawing.Size(237, 20);
            this.dtpFechaFallecimiento.TabIndex = 12;
            // 
            // btnProcesarBaja
            // 
            this.btnProcesarBaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesarBaja.Location = new System.Drawing.Point(100, 302);
            this.btnProcesarBaja.Name = "btnProcesarBaja";
            this.btnProcesarBaja.Size = new System.Drawing.Size(122, 23);
            this.btnProcesarBaja.TabIndex = 13;
            this.btnProcesarBaja.Text = "PROCESAR BAJA";
            this.btnProcesarBaja.UseVisualStyleBackColor = true;
            this.btnProcesarBaja.Click += new System.EventHandler(this.btnProcesarBaja_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(252, 302);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(89, 23);
            this.btnCancelar.TabIndex = 14;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblSaldo
            // 
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.Location = new System.Drawing.Point(115, 156);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(91, 13);
            this.lblSaldo.TabIndex = 15;
            this.lblSaldo.Text = "---------------------";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(42, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(282, 25);
            this.label7.TabIndex = 16;
            this.label7.Text = "COOPERATIVA IGUCOSA";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(126, 46);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(173, 20);
            this.lblTitulo.TabIndex = 17;
            this.lblTitulo.Text = "ELIMINAR CLIENTE";
            // 
            // lblAdvertencia
            // 
            this.lblAdvertencia.AutoSize = true;
            this.lblAdvertencia.Location = new System.Drawing.Point(138, 169);
            this.lblAdvertencia.Name = "lblAdvertencia";
            this.lblAdvertencia.Size = new System.Drawing.Size(28, 13);
            this.lblAdvertencia.TabIndex = 18;
            this.lblAdvertencia.Text = "-------";
            // 
            // FormDarBajaCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(431, 354);
            this.Controls.Add(this.lblAdvertencia);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarBaja);
            this.Controls.Add(this.dtpFechaFallecimiento);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSolicitante);
            this.Controls.Add(this.cmbParentesco);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBuscarDui);
            this.Controls.Add(this.txtDUI);
            this.Controls.Add(this.label1);
            this.Name = "FormDarBajaCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baja-Usuarios";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDUI;
        private System.Windows.Forms.Button btnBuscarDui;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbParentesco;
        private System.Windows.Forms.TextBox txtSolicitante;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpFechaFallecimiento;
        private System.Windows.Forms.Button btnProcesarBaja;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblAdvertencia;
    }
}