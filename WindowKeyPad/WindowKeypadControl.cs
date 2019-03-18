using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowKeyPad
{
    public partial class WindowKeypadControl : Form
    {
        static int CHARACTOR_COUNT = 52;
        Button[] btnKeyPads = new Button[CHARACTOR_COUNT];
        bool bCapsLock = false;
        bool bKeyEnabledFlag = false;

        public string strKeyPadCharactor = "";

        public WindowKeypadControl(bool bKeyEnabled)
        {
            InitializeComponent();
            Initialize(bKeyEnabled);
        }

        public void Initialize(bool bKeyEnabled)
        {
            btnKeyPads = new Button[]   {   btnA,           btnB,       btnC,               btnD,               btnE,           btnF,           btnG,       btnH,       btnI,       btnJ,       btnK,       btnL,       btnN,       btnM,               btnO,
                                            btnP,           btnQ,       btnR,               btnS,               btnT,           btnU,           btnV,       btnW,       btnX,       btnY,       btnZ,       btnMinus,   btnEqual,   btnSemi,            btnComma,   
                                            btnSlash,       btnWon,     btnRightBlacket,    btnLeftBlacket,     btnQuotation,   btnCaps,        btnShift,   btnShift2,  btnEnter,   btnPoint,   btnOK,      btnCancel,   
                                            btn1,           btn2,       btn3,               btn4,               btn5,           btn6,           btn7,       btn8,       btn9,       btn0,   };
            bKeyEnabledFlag = bKeyEnabled;
            EnableCharactorKey();

            //Motion 부분 때문에 Minus Keypad 활성화
            btnMinus.Enabled = true;
        }

        private void EnableCharactorKey()
        {
            //for (int iLoopCount = 0; iLoopCount < CHARACTOR_COUNT; ++iLoopCount) btnKeyPads[iLoopCount].Enabled = true;
            for (int iLoopCount = 0; iLoopCount < CHARACTOR_COUNT - 13; ++iLoopCount) btnKeyPads[iLoopCount].Enabled = bKeyEnabledFlag;
        }

        public void SetKeyPadValue(string strValue)
        {
            strKeyPadCharactor = strValue;
            btnKeyPadCharactor.Text = strValue;
        }

        public void PressKeyDown(string strKey)
        {
            strKeyPadCharactor += strKey;
            //if(strKeyPadCharactor)
            btnKeyPadCharactor.Text = strKeyPadCharactor;
            //btnKeyPadCharactor.Text += "*";
        }

        public void PressKeyButtonDown(string strKey)
        {
            if (true == bCapsLock) strKey = strKey.ToUpper();
            else if (false == bCapsLock) strKey = strKey.ToLower();
            strKeyPadCharactor += strKey;
            //if(strKeyPadCharactor)
            btnKeyPadCharactor.Text = strKeyPadCharactor;
            //btnKeyPadCharactor.Text += "*";
        }

        public void PressBackSpace()
        {
            if (strKeyPadCharactor.Length == 0) return;
            strKeyPadCharactor = strKeyPadCharactor.Substring(0, strKeyPadCharactor.Length - 1);
            btnKeyPadCharactor.Text = strKeyPadCharactor;
            //btnKeyPadCharactor.Text         = btnKeyPadCharactor.Text.Substring(0, btnKeyPadCharactor.Text.Length - 1);
        }

        public void PressKeyClear()
        {
            strKeyPadCharactor = "";
            btnKeyPadCharactor.Text = "";
        }

        private void WindowKeyPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) PressBackSpace();
            else if (false == bKeyEnabledFlag && (e.KeyChar < 48 || e.KeyChar > 57))    return;
            else
            {
                string strCharactor = e.KeyChar.ToString();
                if ((int)Keys.Escape == e.KeyChar) PressKeyClear();
                else if ((int)Keys.Enter == e.KeyChar) PressEnter();
                else PressKeyDown(strCharactor);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PressEnter();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            PressCancel();
        }

        private void PressEnter()
        {
            if (false == bKeyEnabledFlag && "" == strKeyPadCharactor) strKeyPadCharactor = "0";
            btnKeyPadCharactor.Text = "";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void PressCancel()
        {
            this.DialogResult = DialogResult.Cancel;
            btnKeyPadCharactor.Text = "";
            this.Close();
        }

        private void PressShift()
        {
            bCapsLock = !bCapsLock;
            if (true == bCapsLock) { btnShift.ForeColor = Color.Blue; btnShift2.ForeColor = Color.Blue; }
            else if (false == bCapsLock) { btnShift.ForeColor = Color.Black; btnShift2.ForeColor = Color.Black; }
        }

        private void ButtonsClickEvent(object sender, EventArgs e)
        {
            Button KeyPadButton = (Button)sender;
            switch (KeyPadButton.Text)
            {
                case "Shift":
                case "Shift2":  PressShift();       break;
                case "Back":    PressBackSpace();   break;
                case "ESC":     PressKeyClear();    break;
                case "Enter":   PressEnter();       break;
                case "OK":      PressEnter();       break;
                case "Cancel":  PressCancel();      break;
                default:        PressKeyButtonDown(KeyPadButton.Text); break;
            }
        }
    }
}
