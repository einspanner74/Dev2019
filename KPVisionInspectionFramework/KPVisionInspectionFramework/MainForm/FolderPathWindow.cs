using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KPVisionInspectionFramework
{
    public partial class FolderPathWindow : Form
    {
        public delegate void SetDataPathHandler(string _DataPath);
        public event SetDataPathHandler SetDataPathEvent;

        public FolderPathWindow(bool _SimulationModeFlag)
        {
            InitializeComponent();
        }

        public void SetCurrentDataPath(string _CurrentDataPath)
        {
            textBoxInDataPath.Clear();

            textBoxInDataPath.Text = _CurrentDataPath;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (textBoxInDataPath.Text == null || textBoxInDataPath.Text == "") { MessageBox.Show("In Data 폴더 경로가 없습니다."); return; }

            string DataPath = textBoxInDataPath.Text;

            var _RecipeCopyEvent = SetDataPathEvent;
            _RecipeCopyEvent?.Invoke(DataPath);

            this.DialogResult = DialogResult.OK;
            this.Hide();
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
        
        private void btnSearchDataPath_Click(object sender, EventArgs e)
        {  
            FolderBrowserDialog FolderDialog = new FolderBrowserDialog();
            if (DialogResult.OK == FolderDialog.ShowDialog())
            {
                textBoxInDataPath.Text = FolderDialog.SelectedPath;
            }
        }
    }
}
