using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMI_EP_Client
{
    public partial class Main_Form : Form
    {
        private bool mouse_in_config;

        public Main_Form()
        {
            InitializeComponent();
            label_title.AutoSize = false;
            label_title.TextAlign = ContentAlignment.BottomCenter;
            label_title.Dock = DockStyle.Fill;
            
            panel_config.Paint += Panel_config_Paint;
            panel_config.MouseEnter += Panel_config_MouseEnter;
            panel_config.MouseLeave += Panel_config_MouseLeave;
            panel_interior.Paint += Panel_interior_Paint;
            panel_remote.Paint += Panel_remote_Paint;
        }

        private void Panel_config_MouseLeave(object sender, EventArgs e)
        {
            mouse_in_config = false;
            Console.WriteLine("mouse leave");
            //panel_config.Refresh();
            panel_config.Hide();
            panel_config.Show();
        }

        private void Panel_config_MouseEnter(object sender, EventArgs e)
        {
            Console.WriteLine("mouse enter");
            mouse_in_config = true;
            panel_config.Hide();
            panel_config.Show();
        }

        private void Panel_config_Paint(object sender, PaintEventArgs e)
        {
            Color c = System.Drawing.ColorTranslator.FromHtml("#000000");
            if (mouse_in_config)
            {
                c = System.Drawing.ColorTranslator.FromHtml("#000000");
            }
            else {
                c = System.Drawing.ColorTranslator.FromHtml("#2bb0b5");
            }
            int thickness = 3;//it's up to you
            int halfThickness = thickness / 2;
            using (Pen p = new Pen(c, thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                          halfThickness,
                                                          panel_config.ClientSize.Width - thickness,
                                                          panel_config.ClientSize.Height - thickness));
            }

        }

        private void Panel_interior_Paint(object sender, PaintEventArgs e)
        {

            int thickness = 3;//it's up to you
            int halfThickness = thickness / 2;
            using (Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#2bb0b5"), thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                          halfThickness,
                                                          panel_interior.ClientSize.Width - thickness,
                                                          panel_interior.ClientSize.Height - thickness));
            }

        }

        private void Panel_remote_Paint(object sender, PaintEventArgs e)
        {

            int thickness = 3;//it's up to you
            int halfThickness = thickness / 2;
            using (Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#2bb0b5"), thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                          halfThickness,
                                                          panel_remote.ClientSize.Width - thickness,
                                                          panel_remote.ClientSize.Height - thickness));
            }

        }

        
    }
}
