using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HMI_EP
{
    /// <summary>
    /// Interaction logic for InteriorWindow.xaml
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class InteriorWindow : Window
    {
        public InteriorWindow()
        {
            InitializeComponent();
            string curDir = Directory.GetCurrentDirectory();
            webbrowser.Source = new Uri(String.Format("file:///{0}/bin/interior/index/default.html", curDir));
            
        }
    }
}
