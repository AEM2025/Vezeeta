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
    public partial class DocLogin : Form
    {
        int close = 0;
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        public DocLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //select with where
            bool checker = false;
            string id = id_txt.Text, pass = pass_txt.Text;
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "select PASSWORD from DOCTORS where ID = :id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", int.Parse(id_txt.Text));
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if(dr[0].ToString() == pass)
                {
                    checker = true;
                    break;
                }
            }
            if (checker)
            {
                Doc_Functions df = new Doc_Functions(id);
                df.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Error in :(ID OR PASSWORD):");
            dr.Close();
            cn.Close();
        }

        private void DocLogin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void id_txt_TextChanged(object sender, EventArgs e) { }
        private void pass_txt_TextChanged(object sender, EventArgs e) { }
    }
}
