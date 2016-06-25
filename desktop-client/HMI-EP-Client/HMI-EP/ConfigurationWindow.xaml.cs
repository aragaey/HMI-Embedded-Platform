using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HMI_EP
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        Serial serial_port;
        public ConfigurationWindow(Serial s)
        {
            InitializeComponent();
            serial_port = s;
            try
            {
                string text = System.IO.File.ReadAllText(@"config.cfg");
                string[] cmds = text.Split('#');
                textBox1.Text = cmds[0].Split('=')[1];
                textBox2.Text = cmds[1].Split('=')[1];
                textBox3.Text = cmds[2].Split('=')[1];
                textBox4.Text = cmds[3].Split('=')[1];
                textBox5.Text = cmds[4].Split('=')[1];
                textBox6.Text = cmds[5].Split('=')[1];
            }
            catch { }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string msg = "";
                msg += "$CFG$";
                msg += "UP_G=" + textBox1.Text + "#";
                msg += "DOWN_G=" + textBox2.Text + "#";
                msg += "RIGHT_G=" + textBox3.Text + "#";
                msg += "LEFT_G=" + textBox4.Text + "#";
                msg += "FORWARD_G=" + textBox5.Text + "#";
                msg += "BACKWORD_G=" + textBox6.Text + "#";
                msg += ";";
                System.IO.File.WriteAllText(@"config.cfg", msg);
                serial_port.SerialSend(msg);
                Hide();
            }
            catch
            {

            }
        }
    }
}
