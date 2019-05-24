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

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            var op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                string strFileName = op.FileName;
                //If the user does not cancel, open the document.
                if (strFileName.Length != 0)
                {
                    OpenFile(strFileName);
                }
            }
        }
        /// <summary>
        /// 打开幻灯片文件
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenFile(string fileName)
        {
            string fileFullPath = $@"{System.Environment.CurrentDirectory}\dsoframer.ocx";

            if (!File.Exists(fileFullPath))
            {
                // 容器控件丢失
                MessageBox.Show("容器控件丢失！");
                return;
            }

            bool isRegisted = IsRegistered("00460182-9E5E-11D5-B7C8-B8269041DD57");
            if (!isRegisted)
            {
                axFramerControl1.Toolbars = false;
                axFramerControl1.Menubar = false;
                axFramerControl1.BorderStyle = DSOFramer.dsoBorderStyle.dsoBorderNone;

                File.Copy(fileFullPath, $@"{System.Environment.SystemDirectory}\dsoframer.ocx", true);

                if (!Regist($@"{System.Environment.SystemDirectory}\dsoframer.ocx"))
                {
                    // 注册失败
                    MessageBox.Show("注册容器控件失败！");
                    return;
                }
            }

            this.axFramerControl1.Open(fileName);

            return;
        }
        /// <summary>
        /// 注册dsoframer.ocx
        /// </summary>
        /// <param name="fileFullName">ocx控件完整路径</param>
        /// <returns></returns>
        private static bool Regist(string fileFullName)
        {
            bool result = false;

             System.Diagnostics.Process p = System.Diagnostics.Process.Start("regsvr32", fileFullName);//注册完毕显示是否成功的提示
            if (p != null && p.HasExited)
            {
                Int32 exitCode = p.ExitCode;
                if (exitCode == 0)
                    result = true;
            }
            return result;
        }
        /// <summary>
        /// 判断控件是否被注册
        /// </summary>
        /// <param name="CLSID"></param>
        /// <returns></returns>
        private static bool IsRegistered(String CLSID) => !String.IsNullOrEmpty(CLSID) && Registry.ClassesRoot.OpenSubKey(String.Format(@"CLSID\{{{0}}}", CLSID)) != null;
        /// <summary>
        /// 播放幻灯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{F5}");

        }
        /// <summary>
        /// 下一张幻灯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{Down}");
        }
        /// <summary>
        /// 上一张幻灯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{Up}");
        }
    }
}
