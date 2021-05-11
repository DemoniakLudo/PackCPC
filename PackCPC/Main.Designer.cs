namespace PackCPC {
	partial class Main {
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent() {
			this.bpPack = new System.Windows.Forms.Button();
			this.comboPackMethode = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.chkNoAmsdos = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// bpPack
			// 
			this.bpPack.Location = new System.Drawing.Point(12, 33);
			this.bpPack.Name = "bpPack";
			this.bpPack.Size = new System.Drawing.Size(157, 23);
			this.bpPack.TabIndex = 0;
			this.bpPack.Text = "Open file to pack";
			this.bpPack.UseVisualStyleBackColor = true;
			this.bpPack.Click += new System.EventHandler(this.bpPack_Click);
			// 
			// comboPackMethode
			// 
			this.comboPackMethode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPackMethode.FormattingEnabled = true;
			this.comboPackMethode.Items.AddRange(new object[] {
            "Standard",
            "ZX0",
            "ZX1"});
			this.comboPackMethode.Location = new System.Drawing.Point(126, 6);
			this.comboPackMethode.Name = "comboPackMethode";
			this.comboPackMethode.Size = new System.Drawing.Size(90, 21);
			this.comboPackMethode.TabIndex = 67;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 13);
			this.label1.TabIndex = 68;
			this.label1.Text = "Compression method :";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(6, 75);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(388, 134);
			this.listBox1.TabIndex = 69;
			// 
			// chkNoAmsdos
			// 
			this.chkNoAmsdos.AutoSize = true;
			this.chkNoAmsdos.Location = new System.Drawing.Point(224, 39);
			this.chkNoAmsdos.Name = "chkNoAmsdos";
			this.chkNoAmsdos.Size = new System.Drawing.Size(164, 17);
			this.chkNoAmsdos.TabIndex = 70;
			this.chkNoAmsdos.Text = "Save without Amsdos header";
			this.chkNoAmsdos.UseVisualStyleBackColor = true;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 213);
			this.Controls.Add(this.chkNoAmsdos);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboPackMethode);
			this.Controls.Add(this.bpPack);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Main";
			this.Text = "CPC packer files";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bpPack;
		private System.Windows.Forms.ComboBox comboPackMethode;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.CheckBox chkNoAmsdos;
	}
}

