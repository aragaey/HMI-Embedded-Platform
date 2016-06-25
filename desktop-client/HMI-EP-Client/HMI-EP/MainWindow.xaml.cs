using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMI_EP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Serial serial_port;
        public MainWindow()
        {
            InitializeComponent();
            label.MouseEnter += Label_MouseEnter;
            label.MouseLeave += Label_MouseLeave;
            label1.MouseEnter += Label1_MouseEnter;
            label1.MouseLeave += Label1_MouseLeave;
            label2.MouseEnter += Label2_MouseEnter;
            label2.MouseLeave += Label2_MouseLeave;
            label.MouseLeftButtonUp += Label_MouseLeftButtonUp;
            try
            {
                serial_port = new Serial("COM4", 115200, (Parity)Enum.Parse(typeof(Parity), "None"), 8, (StopBits)Enum.Parse(typeof(StopBits), "One"));
            }
            catch { }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ConfigurationWindow cw = new ConfigurationWindow(serial_port);
            cw.Show();
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            label.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2bb0b5"));
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            label.BorderBrush = Brushes.Black;
        }
        private void Label1_MouseLeave(object sender, MouseEventArgs e)
        {
            label1.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2bb0b5"));
        }

        private void Label1_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.BorderBrush = Brushes.Black;
        }
        private void Label2_MouseLeave(object sender, MouseEventArgs e)
        {
            label2.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2bb0b5"));
        }

        private void Label2_MouseEnter(object sender, MouseEventArgs e)
        {
            label2.BorderBrush = Brushes.Black;
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void label2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //InteriorWindow iw = new InteriorWindow();
            //iw.Show();
            Interior_Form if_ = new Interior_Form();
            if_.Show();
        }

        private void label1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoteWindow rw = new RemoteWindow(serial_port);
            rw.Show();
        }
    }
}
