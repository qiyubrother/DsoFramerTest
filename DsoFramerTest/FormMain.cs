using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace DsoFramerTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.FileName.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)
                        || openFileDialog1.FileName.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.axFramerControl1.Open(openFileDialog1.FileName, true, "Excel.Sheet", null, null);
                    }
                    else if (openFileDialog1.FileName.EndsWith(".pptx", StringComparison.InvariantCultureIgnoreCase)
                        || openFileDialog1.FileName.EndsWith(".ppt", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.axFramerControl1.Open(openFileDialog1.FileName, true, "PowerPoint.Show", null, null);
                    }
                    else if (openFileDialog1.FileName.EndsWith(".docx", StringComparison.InvariantCultureIgnoreCase)
                        || openFileDialog1.FileName.EndsWith(".doc", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.axFramerControl1.Open(openFileDialog1.FileName, true, "Word.Document", null, null);
                    }
                }
                catch(System.Runtime.InteropServices.COMException come)
                {
                    MessageBox.Show(come.Message);
                }

            }
            
        }
    }
}
