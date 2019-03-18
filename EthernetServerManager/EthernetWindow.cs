using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using CustonMsgBoxManager;

namespace EthernetServerManager
{
    public partial class EthernetWindow : Form
    {
        public bool IsShowWindow = false;

        private string CommonFolderPath = "";

        private CEtherentServerManager ServerSock;
        private Queue<string> CmdQueue = new Queue<string>();

        private string IPAddress = "127.1.1.3";
        private short PortNumber = 5000;
        private string ClientIPAddress = "127.0.0.0";

        private bool IsConnected = false;
        private Timer ConnectCheckTimer;

        public delegate void ReceiveStringHandler(string[] _ReceiveMsasage);
        public event ReceiveStringHandler ReceiveStringEvent;

        #region Initialize & DeInitialize
        public EthernetWindow()
        {
            InitializeComponent();
        }

        public void Initialize(string _CommonFolderPath)
        {
            CommonFolderPath = _CommonFolderPath;

            ReadEthernetInfoFile();

            textBoxIPAddress.Text = IPAddress;
            textBoxPortNumber.Text = PortNumber.ToString();

            ServerSock = new CEtherentServerManager();
            ServerSock.Initialize(IPAddress, PortNumber);
            ServerSock.RecvMessageEvent += new CEtherentServerManager.RecvMessageHandler(SetReceiveMessage);

            ConnectCheckTimer = new Timer();
            ConnectCheckTimer.Tick += ConnectCheckTimer_Tick;
            ConnectCheckTimer.Interval = 250;
            ConnectCheckTimer.Start();

            
        }

        public void DeInitialize()
        {
            ConnectCheckTimer.Stop();

            ServerSock.RecvMessageEvent -= new CEtherentServerManager.RecvMessageHandler(SetReceiveMessage);
            ServerSock.DeInitialize();
        }

        #region Read & Write Ethernet Information
        private XmlNodeList GetNodeList(string _XmlFilePath)
        {
            XmlNodeList _XmlNodeList = null;

            try
            {
                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.Load(_XmlFilePath);
                XmlElement _XmlRoot = _XmlDocument.DocumentElement;
                _XmlNodeList = _XmlRoot.ChildNodes;
            }

            catch
            {
                _XmlNodeList = null;
            }

            return _XmlNodeList;
        }

        private void ReadEthernetInfoFile()
        {
            DirectoryInfo _DirInfo = new DirectoryInfo(@CommonFolderPath);
            if (false == _DirInfo.Exists) { _DirInfo.Create(); System.Threading.Thread.Sleep(100); }

            string _EthernetInfoFileName = string.Format(@"{0}EthernetInformation.xml", CommonFolderPath);
            if (false == File.Exists(_EthernetInfoFileName))
            {
                File.Create(_EthernetInfoFileName).Close();
                WriteEthernetInfoFile();
                System.Threading.Thread.Sleep(100);
            }

            else
            {
                XmlNodeList _XmlNodeList = GetNodeList(_EthernetInfoFileName);
                if (null == _XmlNodeList) return;
                foreach (XmlNode _Node in _XmlNodeList)
                {
                    if (null == _Node) return;
                    switch (_Node.Name)
                    {
                        case "IPAddress": IPAddress = _Node.InnerText; break;
                        case "PortNumber": PortNumber = Convert.ToInt16(_Node.InnerText); break;
                        case "ClientIPAddress": ClientIPAddress = _Node.InnerText; break;
                    }
                }
            }
        }

        private void WriteEthernetInfoFile()
        {
            DirectoryInfo _DirInfo = new DirectoryInfo(@CommonFolderPath);
            if (false == _DirInfo.Exists) { _DirInfo.Create(); System.Threading.Thread.Sleep(100); }

            string _EthernetInfoFileName = string.Format(@"{0}EthernetInformation.xml", CommonFolderPath);
            XmlTextWriter _XmlWriter = new XmlTextWriter(_EthernetInfoFileName, Encoding.Unicode);
            _XmlWriter.Formatting = Formatting.Indented;
            _XmlWriter.WriteStartDocument();
            _XmlWriter.WriteStartElement("EthernetInformation");
            {
                _XmlWriter.WriteElementString("IPAddress", IPAddress);
                _XmlWriter.WriteElementString("PortNumber", PortNumber.ToString());
                _XmlWriter.WriteElementString("ClientIPAddress", ClientIPAddress);
            }
            _XmlWriter.WriteEndElement();
            _XmlWriter.WriteEndDocument();
            _XmlWriter.Close();
        }
        #endregion Read & Write Ethernet Information
        #endregion Initialize & DeInitialize

        #region Control Default Event
        private void EthernetWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4) e.Handled = true;
        }

        private void labelTitle_MouseMove(object sender, MouseEventArgs e)
        {
            var s = sender as Label;
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            s.Parent.Left = this.Left + (e.X - ((Point)s.Tag).X);
            s.Parent.Top = this.Top + (e.Y - ((Point)s.Tag).Y);

            this.Cursor = Cursors.Default;
        }

        private void labelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            var s = sender as Label;
            s.Tag = new Point(e.X, e.Y);
        }

        private void labelTitle_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.labelTitle.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panelMain.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }
        #endregion Control Default Event

        #region Button Event
        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress = textBoxIPAddress.Text;
            PortNumber = Convert.ToInt16(textBoxPortNumber.Text);

            ServerSock.Connection(IPAddress, PortNumber);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            ServerSock.DisConnection();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //if (false == IsConnected) { MessageBox.Show("Disconnected"); return; }
            if (false == IsConnected) { CMsgBoxManager.Show("Disconnected", "", 2000); return; }
            ServerSock.Send(textBoxManualData.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsShowWindow = false;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            WriteEthernetInfoFile();
            IsShowWindow = false;
            this.Hide();
        }
        #endregion Button Event

        #region "Control Invoke"
        private void ControlInvoke(Control _Control, string _Msg)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate ()
                {
                    _Control.Text = _Msg;
                }
                ));
            }
            else
            {
                _Control.Text = _Msg;
            }
        }
        private void ControlInvoke(Control _Control, Color _Color)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate ()
                {
                    _Control.BackColor = _Color;
                }
                ));
            }
            else
            {
                _Control.BackColor = _Color;
            }
        }
        #endregion "Control Invoke"


        private void ConnectCheckTimer_Tick(object sender, EventArgs e)
        {
            string[] _ConnectedList = ServerSock.GetConnectedClientList();

            IsConnected = false;
            for (int iLoopCount = 0; iLoopCount < _ConnectedList.Length; ++iLoopCount)
            {
                if (_ConnectedList[iLoopCount] == ClientIPAddress)
                {
                    IsConnected = true;
                    break;
                }
            }

            if (true == IsConnected)    ControlInvoke(picConnection, Color.Green);
            else                        ControlInvoke(picConnection, Color.Red);

            if (true == ServerSock.GetServerAlready())  ControlInvoke(picServerStatus, Color.Green);
            else                                        ControlInvoke(picServerStatus, Color.Red);
        }

        private void SetReceiveMessage(string _RecvMessage)
        {
            string _RecvString = _RecvMessage.Trim();
            _RecvString = _RecvString.Replace(" ", "");
            _RecvString = _RecvString.Replace("\0", "");
            string[] _RecvCmd = _RecvString.Split(',');

            for (int iLoopCount = 0; iLoopCount < _RecvCmd.Length; ++iLoopCount)
                CmdQueue.Enqueue(_RecvCmd[iLoopCount]);

            if (true == ProtocolCommandProcess())
            {
                //CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Receive Data : " + _RecvString);
                //CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "ProtocolCommandProcess : Good");
            }

            else
            {
                //CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Receive Data : " + _RecvString);
                //CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "ProtocolCommandProcess : Bad");
            }

            ControlInvoke(textBoxRecvString, _RecvMessage);
        }

        private bool ProtocolCommandProcess()
        {
            if (CmdQueue.Contains(CEtherentServerManager.STX.ToString()) == false) return false;
            if (CmdQueue.Contains(CEtherentServerManager.ETX.ToString()) == false) return false;
            if (CmdQueue.Count < 4) return false;

            string _Data = "";
            while (_Data != CEtherentServerManager.STX.ToString()) _Data = CmdQueue.Dequeue();

            string[] _Datas = new string[3];
            for (int iLoopCount = 0; iLoopCount < _Datas.Length; ++iLoopCount)
                _Datas[iLoopCount] = CmdQueue.Dequeue();

            if (_Datas[_Datas.Length - 1] != CEtherentServerManager.ETX.ToString()) return false;

            var _ReceiveStringEvent = ReceiveStringEvent;
            _ReceiveStringEvent?.Invoke(_Datas);

            return true;
        }

        public void SendResultData(string _ResultDataString)
        {
            //if (false == IsConnected) { MessageBox.Show("Not connected"); return; }
            if (false == IsConnected) { CMsgBoxManager.Show("Not connected", "", 2000); return; }
            ServerSock.Send(_ResultDataString);
            //CLogManager.AddSystemLog(CLogManager.LOG_TYPE.INFO, "Receive Data : " + _ResultDataString);
        }

        public void ShowEthernetWindow()
        {
            textBoxIPAddress.Text = IPAddress;
            textBoxPortNumber.Text = PortNumber.ToString();

            IsShowWindow = true;
            this.Show();
        }
    }
}
