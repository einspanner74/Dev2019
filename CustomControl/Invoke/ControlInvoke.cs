using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControl
{
    public static class ControlInvoke
    {
        public static void GradientLabelText(GradientLabel _Control, string _Text)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () { _Control.Text = _Text; }));
            }
            else
            {
                _Control.Text = _Text;
            }
        }

        public static void GradientLabelText(GradientLabel _Control, Color _FontColor, Color _BackColor)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () 
                {
                    _Control.ForeColor = _FontColor;
                    _Control.ColorTop = _BackColor;
                    _Control.ColorBottom = _BackColor;

                }));
            }
            else
            {
                _Control.ForeColor = _FontColor;
                _Control.ColorTop = _BackColor;
                _Control.ColorBottom = _BackColor;
            }
        }

        public static void GradientLabelText(GradientLabel _Control, string _Text, Color _FontColor)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () { _Control.Text = _Text; _Control.ForeColor = _FontColor; }));
            }
            else
            {
                _Control.Text = _Text; _Control.ForeColor = _FontColor;
            }
        }

        public static void GradientLabelColor(GradientLabel _Control, Color _ColorTop, Color _ColorBottom)
        {
            if (_Control.InvokeRequired)
            {
                _Control.Invoke(new MethodInvoker(delegate () { _Control.ColorTop = _ColorTop; _Control.ColorBottom = _ColorBottom; }));
            }
            else
            {
                _Control.ColorTop = _ColorTop; _Control.ColorBottom = _ColorBottom;
            }
            _Control.Refresh();
        }
    }
}
