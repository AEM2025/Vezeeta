using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;


namespace Crystal_Reports
{
    
    public partial class Form3 : Form
    {
        CrystalReport3 cr;
        public Form3()
        {
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            cr = new CrystalReport3();

            foreach (ParameterDiscreteValue v in cr.Parameter_Day.DefaultValues)
                cmb_day.Items.Add(v.Value);
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            cr.SetParameterValue(0, cmb_day.Text);
            crystalReportViewer3.ReportSource = cr;
        }
        
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void crystalReport2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void crystalReport3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
