using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人脸识别客户端前端界面
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button2.BackColor = Color.Navy;
            PanelRightMain.Controls.Clear();
            Form3 sub = new Form3();
            sub.TopLevel = false;
            sub.Dock = DockStyle.Fill;//把子窗体设置为控件

            sub.FormBorderStyle = FormBorderStyle.None;
            PanelRightMain.Controls.Add(sub);
            sub.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.BackColor = Color.Navy;
            PanelRightMain.Controls.Clear();
            Form4 sub = new Form4();
            sub.TopLevel = false;
            sub.Dock = DockStyle.Fill;//把子窗体设置为控件

            sub.FormBorderStyle = FormBorderStyle.None;
            PanelRightMain.Controls.Add(sub);
            sub.Show();
        }

        private void panelLeftMenu_Paint(object sender, PaintEventArgs e)
        {
            /*ControlPaint.DrawBorder(e.Graphics,
                                       this.panelLeftMenu.ClientRectangle,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid);*/
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            /*ControlPaint.DrawBorder(e.Graphics,
                                       this.panel1.ClientRectangle,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid,
                                       Color.SkyBlue,
                                       3,
                                       ButtonBorderStyle.Solid);*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.button1.BackColor = System.Drawing.Color.SkyBlue;
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            this.button2.BackColor = System.Drawing.Color.SkyBlue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PanelRightMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
