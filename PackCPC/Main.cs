using System;
using System.IO;
using System.Windows.Forms;

namespace PackCPC {
	public partial class Main : Form {
		public enum PackMethode { None = 0, Standard, ZX0, ZX1, ZX0_V2 };

		public Main() {
			InitializeComponent();
			comboPackMethode.SelectedIndex = 0;
		}

		private void WriteInfo(string txt) {
			listBox1.Items.Add(txt);
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
		}

		private PackMethode GetPkMethod() {
			PackMethode pkMethod = PackMethode.None;
			switch (comboPackMethode.SelectedItem.ToString()) {
				case "Standard":
					pkMethod = PackMethode.Standard;
					break;

				case "ZX0":
					pkMethod = PackMethode.ZX0;
					break;

				case "ZX0 V2":
					pkMethod = PackMethode.ZX0_V2;
					break;

				case "ZX1":
					pkMethod = PackMethode.ZX1;
					break;
			}
			return pkMethod;
		}

		private void SaveFile(string fileOut, byte[] result, int lDest, bool isCpc) {
			if (fileOut != "") {
				if (chkAsm.Checked || chkAsmWord.Checked) {
					StreamWriter sw = File.CreateText(fileOut);
					if (GetPkMethod() != PackMethode.None && !chkNoDepack.Checked) {
						SaveAsm.GenereDepack(sw, GetPkMethod());
						sw.WriteLine("\n\r;Datas");
					}
					SaveAsm.GenereDatas(sw, result, lDest, 16, chkAsmWord.Checked);
					sw.Close();
				}
				else {
					FileStream fs = new FileStream(fileOut, FileMode.Create, FileAccess.ReadWrite);
					BinaryWriter bw = new BinaryWriter(fs);
					if (isCpc && !chkNoAmsdos.Checked) {
						string name = Path.GetFileName(fileOut);
						bw.Write(CpcSystem.AmsdosToByte(CpcSystem.CreeEntete(name, 0x4000, (short)lDest, 0)));
					}
					bw.Write(result, 0, lDest);
					bw.Close();
				}
			}
		}

		private void BpPack_Click(object sender, EventArgs e) {
			byte[] multiResult = null;
			int[] multiPos = null;
			int nbMulti = 0;
			int offsetMulti = 0;
			if (chkMulti.Checked) {
				multiResult = new byte[0x100000];
				multiPos = new int[0x100];
			}
			OpenFileDialog dlgOpen = new OpenFileDialog { Filter = "All files (*.*)|*.*", Multiselect = true };
			if (dlgOpen.ShowDialog() == DialogResult.OK) {
				bool multiple = dlgOpen.FileNames.Length > 1;
				foreach (string file in dlgOpen.FileNames) {
					FileStream fileScr = new FileStream(file, FileMode.Open, FileAccess.Read);
					int lSource = (int)fileScr.Length;
					byte[] tabBytes = new byte[lSource];
					byte[] result = new byte[lSource + 128];
					fileScr.Read(tabBytes, 0, lSource);
					fileScr.Close();
					bool isCpc = CpcSystem.CheckAmsdos(tabBytes);
					if (isCpc) {
						if (chkNoAmsdos.Checked || chkAsm.Checked || chkAsmWord.Checked)
							WriteInfo("Amsdos header detected, but no Amsdos header will be written to destination file");
						else
							WriteInfo("Amsdos header detected, will write Amsdos header to destination file");

						lSource -= 128;
						Array.Copy(tabBytes, 128, tabBytes, 0, lSource);
					}
					WriteInfo("Source file (" + Path.GetFileName(file) + ") size:" + lSource + " (#" + lSource.ToString("X4") + ")");
					int lDest = lSource;
					if (GetPkMethod() == PackMethode.None)
						Array.Copy(tabBytes, result, lSource);
					else {
						if (!chkCutFiles.Checked) {
							PackModule p = new PackModule();
							lDest = p.Pack(tabBytes, lSource, result, 0, GetPkMethod());
							WriteInfo("output file size:" + lDest + " (#" + lDest.ToString("X4") + ") (gain=" + (100 - (100 * lDest / lSource)).ToString("00") + "%)");
						}
					}
					if (chkMulti.Checked) {
						Array.Copy(result, 0, multiResult, offsetMulti, lDest);
						multiPos[nbMulti++] = offsetMulti;
						offsetMulti += lDest;
					}
					else {
						if (chkCutFiles.Checked && txbBytesSize.Text.Length > 0) {
							string strSize = txbBytesSize.Text;
							int size = strSize[0] == '#' ? Int32.Parse(strSize.Substring(1), System.Globalization.NumberStyles.HexNumber) : Int32.Parse(strSize);
							if (size > 0) {
								SaveFileDialog dlgSave = new SaveFileDialog { Filter = chkAsm.Checked || chkAsmWord.Checked ? "Assembler files (*.asm)|*.asm" : "All files (*.*)|*.*" };
								if (dlgSave.ShowDialog() == DialogResult.OK) {
									int startAdr = 0;
									string firstFile = Path.GetFileNameWithoutExtension(dlgSave.FileName);
									string path = Path.GetDirectoryName(dlgSave.FileName);
									byte[] tmpArray = new byte[size];
									PackModule p = new PackModule();
									int i = 0;
									while (size > 0) {
										Array.Copy(tabBytes, startAdr, tmpArray, 0, size);
										startAdr += size;
										lDest = p.Pack(tmpArray, size, result, 0, GetPkMethod());
										WriteInfo("output file size:" + lDest + " (#" + lDest.ToString("X4") + ") (gain=" + (100 - (100 * lDest / size)).ToString("00") + "%)");
										if (startAdr + size > lSource)
											size = lSource - startAdr;

										string fullName = path + "\\" + firstFile + "_" + (i++).ToString() + ".asm";
										SaveFile(fullName, result, lDest, isCpc);
									}
								}
							}
						}
						else {
							string fileOut = "";
							if (multiple)
								fileOut = file + ".pack";
							else {
								SaveFileDialog dlgSave = new SaveFileDialog { Filter = chkAsm.Checked || chkAsmWord.Checked ? "Assembler files (*.asm)|*.asm" : "All files (*.*)|*.*" };
								if (dlgSave.ShowDialog() == DialogResult.OK)
									fileOut = dlgSave.FileName;
							}
							SaveFile(fileOut, result, lDest, isCpc);
						}
					}
				}
				if (chkMulti.Checked) {
					nbMulti = 0;
					WriteInfo("----------------------------------------");
					foreach (string file in dlgOpen.FileNames)
						WriteInfo("Offset #" + multiPos[nbMulti++].ToString("X5") + " : " + Path.GetFileName(file));

					WriteInfo("----------------------------------------");
					WriteInfo("Total length: #" + offsetMulti.ToString("X5"));
					SaveFileDialog dlgSave = new SaveFileDialog { Filter = chkAsm.Checked || chkAsmWord.Checked ? "Assembler files (*.asm)|*.asm" : "All files (*.*)|*.*" };
					if (dlgSave.ShowDialog() == DialogResult.OK) {
						SaveFile(dlgSave.FileName, multiResult, offsetMulti, true);
						using (StreamWriter sw = File.CreateText(dlgSave.FileName + ".txt")) {
							nbMulti = 0;
							foreach (string file in dlgOpen.FileNames)
								sw.WriteLine("Offset #" + multiPos[nbMulti++].ToString("X5") + " : " + Path.GetFileName(file));
						}
					}
				}
			}
		}

		private void ChkAsm_CheckedChanged(object sender, EventArgs e) {
			chkNoAmsdos.Enabled = !chkAsm.Checked && !chkAsmWord.Checked;
			chkNoDepack.Visible = chkAsm.Checked || chkAsmWord.Checked;
			chkCutFiles.Enabled = !chkMulti.Checked;
			txbBytesSize.Enabled = chkCutFiles.Checked && !chkMulti.Checked;
			if (chkAsm.Checked)
				chkAsmWord.Checked = false;
		}

		private void ChkAsmWord_CheckedChanged(object sender, EventArgs e) {
			chkNoAmsdos.Enabled = !chkAsm.Checked && !chkAsmWord.Checked;
			chkNoDepack.Visible = chkAsm.Checked || chkAsmWord.Checked;
			chkCutFiles.Enabled = !chkMulti.Checked;
			txbBytesSize.Enabled = chkCutFiles.Checked && !chkMulti.Checked;
			if (chkAsmWord.Checked)
				chkAsm.Checked = false;
		}

		private void ChkCutFiles_CheckedChanged(object sender, EventArgs e) {
			txbBytesSize.Enabled = chkCutFiles.Checked;
		}

	}
}
