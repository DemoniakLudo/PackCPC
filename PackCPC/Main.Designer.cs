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
			this.chkAsm = new System.Windows.Forms.CheckBox();
			this.chkNoDepack = new System.Windows.Forms.CheckBox();
			this.chkMulti = new System.Windows.Forms.CheckBox();
			this.chkCutFiles = new System.Windows.Forms.CheckBox();
			this.txbBytesSize = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkAsmWord = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// bpPack
			// 
			this.bpPack.Location = new System.Drawing.Point(12, 33);
			this.bpPack.Name = "bpPack";
			this.bpPack.Size = new System.Drawing.Size(157, 23);
			this.bpPack.TabIndex = 0;
			this.bpPack.Text = "Open file(s) to pack/save";
			this.bpPack.UseVisualStyleBackColor = true;
			this.bpPack.Click += new System.EventHandler(this.BpPack_Click);
			// 
			// comboPackMethode
			// 
			this.comboPackMethode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPackMethode.FormattingEnabled = true;
			this.comboPackMethode.Items.AddRange(new object[] {
            "-- None --",
            "Standard",
            "ZX0",
            "ZX0 V2",
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
			this.listBox1.Location = new System.Drawing.Point(4, 108);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(388, 134);
			this.listBox1.TabIndex = 69;
			// 
			// chkNoAmsdos
			// 
			this.chkNoAmsdos.AutoSize = true;
			this.chkNoAmsdos.Location = new System.Drawing.Point(224, 33);
			this.chkNoAmsdos.Name = "chkNoAmsdos";
			this.chkNoAmsdos.Size = new System.Drawing.Size(164, 17);
			this.chkNoAmsdos.TabIndex = 70;
			this.chkNoAmsdos.Text = "Save without Amsdos header";
			this.chkNoAmsdos.UseVisualStyleBackColor = true;
			// 
			// chkAsm
			// 
			this.chkAsm.AutoSize = true;
			this.chkAsm.Location = new System.Drawing.Point(224, 52);
			this.chkAsm.Name = "chkAsm";
			this.chkAsm.Size = new System.Drawing.Size(140, 17);
			this.chkAsm.TabIndex = 71;
			this.chkAsm.Text = "Save as asm file (DEFB)";
			this.chkAsm.UseVisualStyleBackColor = true;
			this.chkAsm.CheckedChanged += new System.EventHandler(this.ChkAsm_CheckedChanged);
			// 
			// chkNoDepack
			// 
			this.chkNoDepack.AutoSize = true;
			this.chkNoDepack.Location = new System.Drawing.Point(224, 12);
			this.chkNoDepack.Name = "chkNoDepack";
			this.chkNoDepack.Size = new System.Drawing.Size(168, 17);
			this.chkNoDepack.TabIndex = 72;
			this.chkNoDepack.Text = "Save without depack function";
			this.chkNoDepack.UseVisualStyleBackColor = true;
			this.chkNoDepack.Visible = false;
			// 
			// chkMulti
			// 
			this.chkMulti.AutoSize = true;
			this.chkMulti.Location = new System.Drawing.Point(12, 62);
			this.chkMulti.Name = "chkMulti";
			this.chkMulti.Size = new System.Drawing.Size(197, 17);
			this.chkMulti.TabIndex = 71;
			this.chkMulti.Text = "Save multiple files in one packed file";
			this.chkMulti.UseVisualStyleBackColor = true;
			this.chkMulti.CheckedChanged += new System.EventHandler(this.ChkAsm_CheckedChanged);
			// 
			// chkCutFiles
			// 
			this.chkCutFiles.AutoSize = true;
			this.chkCutFiles.Location = new System.Drawing.Point(12, 85);
			this.chkCutFiles.Name = "chkCutFiles";
			this.chkCutFiles.Size = new System.Drawing.Size(110, 17);
			this.chkCutFiles.TabIndex = 73;
			this.chkCutFiles.Text = "Cut file in x files of";
			this.chkCutFiles.UseVisualStyleBackColor = true;
			this.chkCutFiles.CheckedChanged += new System.EventHandler(this.ChkCutFiles_CheckedChanged);
			// 
			// txbBytesSize
			// 
			this.txbBytesSize.Enabled = false;
			this.txbBytesSize.Location = new System.Drawing.Point(120, 82);
			this.txbBytesSize.MaxLength = 5;
			this.txbBytesSize.Name = "txbBytesSize";
			this.txbBytesSize.Size = new System.Drawing.Size(41, 20);
			this.txbBytesSize.TabIndex = 74;
			this.txbBytesSize.Text = "#1000";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(164, 86);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 75;
			this.label2.Text = "bytes";
			// 
			// chkAsmWord
			// 
			this.chkAsmWord.AutoSize = true;
			this.chkAsmWord.Location = new System.Drawing.Point(224, 75);
			this.chkAsmWord.Name = "chkAsmWord";
			this.chkAsmWord.Size = new System.Drawing.Size(144, 17);
			this.chkAsmWord.TabIndex = 71;
			this.chkAsmWord.Text = "Save as asm file (DEFW)";
			this.chkAsmWord.UseVisualStyleBackColor = true;
			this.chkAsmWord.CheckedChanged += new System.EventHandler(this.ChkAsmWord_CheckedChanged);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(397, 244);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txbBytesSize);
			this.Controls.Add(this.chkCutFiles);
			this.Controls.Add(this.chkNoDepack);
			this.Controls.Add(this.chkMulti);
			this.Controls.Add(this.chkAsmWord);
			this.Controls.Add(this.chkAsm);
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
		private System.Windows.Forms.CheckBox chkAsm;
		private System.Windows.Forms.CheckBox chkNoDepack;
		private System.Windows.Forms.CheckBox chkMulti;
		private System.Windows.Forms.CheckBox chkCutFiles;
		private System.Windows.Forms.TextBox txbBytesSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkAsmWord;
	}
}

