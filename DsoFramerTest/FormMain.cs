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

        private void button1_Click(object sender, EventArgs e)
        {
            string fileFullPath = $@"{System.Environment.CurrentDirectory}\dsoframer.ocx";
            if (!File.Exists(fileFullPath))
            {
                // 容器控件丢失
                return;
            }

            bool isRegisted = IsRegistered("00460182-9E5E-11D5-B7C8-B8269041DD57");
            if (isRegisted)
            {
                var op = new OpenFileDialog();
                op.ShowDialog();
                string strFileName = op.FileName;
                //If the user does not cancel, open the document.
                if (strFileName.Length != 0)
                {
                    this.axFramerControl1.Open(strFileName);
                }
                return;
            }
            else
            {
                axFramerControl1.Toolbars = false;
                axFramerControl1.Menubar = false;
                axFramerControl1.BorderStyle = DSOFramer.dsoBorderStyle.dsoBorderNone;

                File.Copy(fileFullPath, $@"{System.Environment.SystemDirectory}\dsoframer.ocx", true);

                Regist($@"{System.Environment.SystemDirectory}\dsoframer.ocx");
            }
        }

        //注册dsoframer.ocx
        private static bool Regist(string fileFullName)
        {
            bool result = false;
         //   System.Diagnostics.Process p = System.Diagnostics.Process.Start("regsvr32", fileFullName + " /s");//注册完毕不显示是否成功的提示

             System.Diagnostics.Process p = System.Diagnostics.Process.Start("regsvr32", fileFullName);//注册完毕显示是否成功的提示
            if (p != null && p.HasExited)
            {
                Int32 exitCode = p.ExitCode;
                if (exitCode == 0)
                    result = true;
            }
            return result;
        }
        //获取当前操作系统的位数
        private static string GetOSBitCount()
        {
            ConnectionOptions oConn = new ConnectionOptions();
            System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn);
            System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oReturnCollection = oSearcher.Get();
            string addressWidth = null;

            foreach (ManagementObject oReturn in oReturnCollection)
                addressWidth = oReturn["AddressWidth"].ToString();

            return addressWidth;
        }
        //判断控件是否已经注册
        private static bool IsRegistered(String CLSID)
        {
            if (String.IsNullOrEmpty(CLSID))
                return false;

            String key = String.Format(@"CLSID\{{{0}}}", CLSID);
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(key);
            if (regKey != null)
                return true;
            else
                return false;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{F5}");

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{Down}");
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            axFramerControl1.Activate();

            SendKeys.SendWait("{Up}");
        }
    }
}
