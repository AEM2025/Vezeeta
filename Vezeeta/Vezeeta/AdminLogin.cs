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
    public partial class AdminLogin : Form
    {
        int close = 0;
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //select one row by stored proc
            string id = id_txt.Text, pass = pass_txt.Text;
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "AdminLOGIN";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add("password", OracleDbType.Varchar2, 300, null, ParameterDirection.Output);
            cmd.Parameters["password"].DbType = DbType.String;
            cmd.ExecuteNonQuery();

            string id2 = cmd.Parameters["id"].Value.ToString(), pass2 = cmd.Parameters["password"].Value.ToString();
            if ((id == id2) && (pass == pass2)) 
            {
                Admin_Functions adf = new Admin_Functions();
                adf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error in :( ID OR PASSWORD ):");
            }
            cn.Close();
        }

        private void panel4_Paint(object sender, PaintEventArgs e) { }

        private void AdminLogin_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
