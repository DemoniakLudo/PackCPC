using System;
using System.IO;
using System.Windows.Forms;

namespace PackCPC {
	public partial class Main : Form {
		public enum PackMethode { None = 0, Standard, ZX0, ZX1 };

		public Main() {
			InitializeComponent();
			comboPackMethode.SelectedIndex = 0;
		}

		private void WriteInfo(string txt) {
			listBox1.Items.Add(txt);
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
		}

		private void bpPack_Click(object sender, EventArgs e) {
			OpenFileDialog dlgOpen = new OpenFileDialog();
			dlgOpen.Filter = "All files (*.*)|*.*";
			if (dlgOpen.ShowDialog() == DialogResult.OK) {
				FileStream fileScr = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);
				int lSource = (int)fileScr.Length;
				byte[] tabBytes = new byte[lSource];
				byte[] result = new byte[lSource + 128];
				fileScr.Read(tabBytes, 0, lSource);
				fileScr.Close();
				bool isCpc = CpcSystem.CheckAmsdos(tabBytes);
				if (isCpc) {
					if (chkNoAmsdos.Checked || chkAsm.Checked)
						WriteInfo("Amsdos header detected, but no Amsdos header will be written to destination file");
					else
						WriteInfo("Amsdos header detected, will write Amsdos header to destination file");

					Array.Copy(tabBytes, 128, tabBytes, 0, lSource - 128);
					lSource -= 128;
				}
				WriteInfo("Source file (" + Path.GetFileName(dlgOpen.FileName) + ") size:" + lSource + " (#" + lSource.ToString("X4") + ")");
				PackMethode pkMethode = PackMethode.None;
				switch (comboPackMethode.SelectedItem.ToString()) {
					case "Standard":
						pkMethode = PackMethode.Standard;
						break;

					case "ZX0":
						pkMethode = PackMethode.ZX0;
						break;

					case "ZX1":
						pkMethode = PackMethode.ZX1;
						break;
				}
				int lDest = lSource;
				if (pkMethode != PackMethode.None) {
					PackModule p = new PackModule();
					lDest = p.Pack(tabBytes, lSource, result, 0, pkMethode);
				}
				else {
					Array.Copy(tabBytes, result, lSource);
				}
				WriteInfo("output file size:" + lDest + " (#" + lDest.ToString("X4") + ") (gain=" + (100 - (100 * lDest / lSource)).ToString("00") + "%)");
				SaveFileDialog dlgSave = new SaveFileDialog();
				dlgSave.Filter = chkAsm.Checked ? "Assembler files (*.asm)|*.asm" : "All files (*.*)|*.*";
				if (dlgSave.ShowDialog() == DialogResult.OK) {
					if (chkAsm.Checked) {
						StreamWriter sw = File.CreateText(dlgSave.FileName);
						if (pkMethode != PackMethode.None && !chkNoDepack.Checked) {
							SaveAsm.GenereDepack(sw, pkMethode);
							sw.WriteLine("\n\r;Datas");
						}
						SaveAsm.GenereDatas(sw, result, lDest, 16);
						sw.Close();
					}
					else {
						FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create, FileAccess.ReadWrite);
						BinaryWriter bw = new BinaryWriter(fs);
						if (isCpc && !chkNoAmsdos.Checked) {
							string name = Path.GetFileName(dlgSave.FileName);
							bw.Write(CpcSystem.AmsdosToByte(CpcSystem.CreeEntete(name, 0x4000, (short)lDest, 0)));
						}
						bw.Write(result, 0, lDest);
						bw.Close();
					}
				}
			}
		}

		private void chkAsm_CheckedChanged(object sender, EventArgs e) {
			chkNoAmsdos.Enabled = !chkAsm.Checked;
			chkNoDepack.Visible = chkAsm.Checked;
		}
	}
}
