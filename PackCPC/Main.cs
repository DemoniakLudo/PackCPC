using System;
using System.IO;
using System.Windows.Forms;

namespace PackCPC {
	public partial class Main : Form {
		public enum PackMethode { None = 0, Standard, ZX0, ZX1 };

		static private byte[] result = new byte[0x10000];

		public Main() {
			InitializeComponent();
			comboPackMethode.SelectedItem = "Standard";
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
				fileScr.Read(tabBytes, 0, lSource);
				fileScr.Close();
				bool isCpc = CpcSystem.CheckAmsdos(tabBytes);
				if (isCpc) {
					if (chkNoAmsdos.Checked)
						WriteInfo("Amsdos header detected, but no Amsdos header will be written to destination file");
					else
						WriteInfo("Amsdos header detected, will write Amsdos header to destination file");

					Array.Copy(tabBytes, 128, tabBytes, 0, lSource - 128);
					lSource -= 128;
				}
				WriteInfo("Source file size:" + lSource + " (#" + lSource.ToString("X4") + ")");
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
				PackModule p = new PackModule();
				int lDest = p.Pack(tabBytes, lSource, result, 0, pkMethode);
				WriteInfo("output file size:" + lDest + " (#" + lDest.ToString("X4") + ") (gain=" + (100 - (100 * lDest / lSource)).ToString("00") + "%)");
				SaveFileDialog dlgSave = new SaveFileDialog();
				dlgSave.Filter = "All files (*.*)|*.*";
				if (dlgSave.ShowDialog() == DialogResult.OK) {
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
}
