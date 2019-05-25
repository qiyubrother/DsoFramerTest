using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace DsoFramerTest
{
    public partial class FrmPlayer : Form
    {
        public FrmPlayer()
        {
            InitializeComponent();

            KeyPreview = true;
            var subControlsCount = Controls.Count;
            //Task.Run(() =>
            //{
            //    do
            //    {
            //        if (Visible && PptProcess != null && PptProcess.MainWindowHandle == IntPtr.Zero)
            //        {
            //            Close();
            //            break;
            //        }
            //        System.Threading.Thread.Sleep(500);
            //    } while (true);
            //});
        }
        public Form ClientArea { get => this; }
        public Process PptProcess { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == (Keys.Control | Keys.A | Keys.Shift))
            if (keyData == (Keys.Escape))
            {
                MessageBox.Show("Esc");
                //Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    switch (keyData)
        //    {
        //        case Keys.Escape:
        //            Close();
        //            return false;
        //    }
        //    return false;
        //}
    }
   
}
