using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataEncryptionLab5_Kot_PC_21_MH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bInFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory= true,
        })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tInFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void bOutFile_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tOutFilePath.Text = saveFileDialog.FileName;
            }
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            var watch = new Stopwatch();

            watch.Start();
            var input = File.ReadAllBytes(tInFilePath.Text);

            var encodedBytes = MyEncoding(input);

            File.WriteAllBytes(tOutFilePath.Text, encodedBytes);
            watch.Stop();

            codingTime.Text = watch.Elapsed.ToString();
            labelFileInputSize.Text = input.Length.ToString();
            labelFileOutputSize.Text = encodedBytes.Length.ToString();

            var resultInString = BitConverter.ToString(encodedBytes).Replace("-", "");
            MessageBox.Show("У файлі записане число (контрольна сума вхідного файлу):\n" + resultInString,
                "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private byte[] MyEncoding(byte[] inArr)
        {
            var result = Array.Empty<byte>();

            if (rb_CRC32.Checked)
            {
                return BitConverter.GetBytes(Crc.Crc32(inArr));
            }

            if (rB_HAVAL.Checked)
            {
                return KeyedHashAlgorithm.Create().ComputeHash(inArr);
            }

            if (rB_RIPEMD160.Checked)
            {
                return RIPEMD160.Create().ComputeHash(inArr);
            }

            if (rB_MD5.Checked)
            {
                return MD5.Create().ComputeHash(inArr);
            }

            if (rB_SHA1.Checked)
            {
                return SHA1.Create().ComputeHash(inArr);
            }

            if (rB_SHA256.Checked)
            {
                return SHA256.Create().ComputeHash(inArr);
            }

            if (rB_SHA384.Checked)
            {
                return SHA384.Create().ComputeHash(inArr);
            }

            if (rB_SHA512.Checked)
            {
                return SHA512.Create().ComputeHash(inArr);
            }

            return result;
        }

        private void bClean_Click(object sender, EventArgs e)
        {
            labelFileOutputSize.Text = labelFileInputSize.Text = "";
            tInFilePath.Text = tOutFilePath.Text = "";
            codingTime.Text = "";
        }
    }
}
