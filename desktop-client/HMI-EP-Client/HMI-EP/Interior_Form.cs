using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMI_EP
{
    public partial class Interior_Form : Form
    {
        public Interior_Form()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            string curDir = Directory.GetCurrentDirectory();
            webBrowser1.Url = new Uri(String.Format("file:///{0}/bin/interior/index/default.html", curDir));

        }
    }
}
