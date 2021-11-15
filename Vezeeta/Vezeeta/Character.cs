using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Vezeeta
{
    public partial class Character : Form
    {
        int close = 0;
        public Character()
        {
            InitializeComponent();
        }

        private void admins_btn_Click(object sender, EventArgs e)
        {
            AdminLogin ad = new AdminLogin();
            ad.Show();
            this.Hide();
        }

        private void Doc_btn_Click(object sender, EventArgs e)
        {
            DocLogin dl = new DocLogin();
            dl.Show();
            this.Hide();
        }

        private void Patient_btn_Click(object sender, EventArgs e)
        {
            PatientLogin pl = new PatientLogin();
            pl.Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close == 0)
            {
                close = 1;
                DialogResult dialog = MessageBox.Show("Are you sure you want to exit ?", "Exit", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    System.Windows.Forms.Application.Exit();
                }
                
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }

        
    }
}
