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
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        /// <summary>
        /// 设置目标窗体大小，位置
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="x">目标窗体新位置X轴坐标</param>
        /// <param name="y">目标窗体新位置Y轴坐标</param>
        /// <param name="nWidth">目标窗体新宽度</param>
        /// <param name="nHeight">目标窗体新高度</param>
        /// <param name="BRePaint">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const int WS_EX_TOPMOST = 0x00000008;

        IntPtr _playerHandle = new IntPtr(0);
        FrmPlayer player = new FrmPlayer();

        public FormMain()
        {
            InitializeComponent();



        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            player.Text = string.Empty;
            player.ShowInTaskbar = false;
            player.FormBorderStyle = FormBorderStyle.None;
            player.StartPosition = FormStartPosition.Manual;

            player.TopLevel = true;
            player.TopMost = true;
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

            player.Width = pnlMain.Width;
            player.Height = pnlMain.Height;
            var ps = PointToScreen(new Point(pnlMain.Left, pnlMain.Top));
            player.Top = ps.Y;
            player.Left = ps.X;

            System.Threading.Thread.Sleep(3000);

                //var p = System.Diagnostics.Process.GetProcessById(Convert.ToInt32(txtPID.Text));
            var pNames = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            if (pNames.Length > 0)
            {
                var p = pNames[0];
                player.PptProcess = p;
                _playerHandle = p.MainWindowHandle;

                //SetParent(_playerHandle, player.ClientArea.Handle);
                MoveWindow(_playerHandle, ps.X, ps.Y, pnlMain.Width, pnlMain.Height, true);
            }

            System.Threading.Thread.Sleep(500);

            //SetParent(_playerHandle, player.ClientArea.Handle);
            MoveWindow(_playerHandle, ps.X, ps.Y, pnlMain.Width, pnlMain.Height, true);
            System.Threading.Thread.Sleep(500);
            //player.Activate();
            SetForegroundWindow(_playerHandle);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 1, 1, 0, 0);

            

        }
        /// <summary>
        /// 下一张幻灯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            //axFramerControl1.Activate();
            SetForegroundWindow(_playerHandle);
            SendKeys.SendWait("{Down}");
        }
        /// <summary>
        /// 上一张幻灯片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            //axFramerControl1.Activate();

            SetForegroundWindow(_playerHandle);
            SendKeys.SendWait("{Up}");
        }

        private void btnInnerPlay_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(100);

            //SetParent(_playerHandle, player.ClientArea.Handle);
            //MoveWindow(_playerHandle, 0, 0, pnlMain.Width, pnlMain.Height, true);

            //player.Activate();
            SetForegroundWindow(_playerHandle);
            var ps = PointToScreen(new Point(1, 1));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 1, 1, 0, 0);

        }
        /// <summary>
        /// ppt|pptx 文件处理程序
        /// </summary>
        public string ProcessName { get; set; } = "wpp";

        private void button2_Click(object sender, EventArgs e)
        {
            //var pWindow = FindWindowEx(player.Handle, IntPtr.Zero, null, null);
            //if (pWindow != null)
            //{
            //    MessageBox.Show("Found.");
            //}
            //else
            //{
            //    MessageBox.Show("Null sub window");
            //}
        }
    }


}
