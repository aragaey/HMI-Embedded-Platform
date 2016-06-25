using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using Microsoft.Expression.Encoder.Devices;
using WebcamControl;
namespace HMI_EP
{
    /// <summary>
    /// Interaction logic for RemoteWindow.xaml
    /// </summary>
    public partial class RemoteWindow : Window
    {
        Serial serial_port;
        public RemoteWindow(Serial s)
        {
            serial_port = s;
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {

            WebcamCtrl.FrameRate = 30;
            WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

            Binding binding_1 = new Binding("SelectedValue");
            binding_1.Source = VideoDevicesComboBox;
            WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);
            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            VideoDevicesComboBox.ItemsSource = vidDevices;
            VideoDevicesComboBox.SelectedIndex = 0;
        }

        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("pack://siteoforigin:,,,/" + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            image.Source = GetImage("Resources/ar_up_b.png");
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Source = GetImage("Resources/ar_up.png");
        }

        private void image_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy.Source = GetImage("Resources/ar_left_b.png");
        }



        private void image_Copy_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy.Source = GetImage("Resources/ar_left.png");
        }

        private void image_Copy1_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy1.Source = GetImage("Resources/ar_right.png");
        }

        private void image_Copy1_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy1.Source = GetImage("Resources/ar_right_b.png");
        }

        private void image_Copy2_MouseEnter(object sender, MouseEventArgs e)
        {
            image_Copy2.Source = GetImage("Resources/ar_down_b.png");
        }

        private void image_Copy2_MouseLeave(object sender, MouseEventArgs e)
        {
            image_Copy2.Source = GetImage("Resources/ar_down.png");
        }
        bool connected = false;
        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {

            if (!connected)
            {
                WebcamCtrl.StartPreview();
                button_Copy1.Content = "Disconnect";
            }
            else {
                WebcamCtrl.StopRecording();
                button_Copy1.Content = "Connect";
            }
            

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //up
            string msg = "";
            msg += "$CTRL$";
            msg += "UP_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }

        private void image_Copy1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //right
            string msg = "";
            msg += "$CTRL$";
            msg += "DOWN_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }

        private void image_Copy2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //down
            string msg = "";
            msg += "$CTRL$";
            msg += "DOWN_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }

        private void image_Copy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //left
            string msg = "";
            msg += "$CTRL$";
            msg += "LEFT_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //forward
            string msg = "";
            msg += "$CTRL$";
            msg += "FORWARD_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }

        private void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //still
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            //backward
            string msg = "";
            msg += "$CTRL$";
            msg += "BACKWARD_G";
            msg += ";";
            serial_port.SerialSend(msg);
        }
    }
}
