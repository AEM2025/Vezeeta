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
    public partial class PatientLogin : Form
    {
        int close = 0;
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        public PatientLogin()
        {
            InitializeComponent();

            //using group functions  //select on row by stored
            OracleConnection con = new OracleConnection(conStr);
            con.Open();
            OracleCommand com = new OracleCommand();
            com.Connection = con;
            com.CommandText = "MAX_PATIENT_ID";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("maxi", OracleDbType.Int32, ParameterDirection.Output);
            com.ExecuteNonQuery();
            try
            {
                int maxi = Convert.ToInt32(com.Parameters["maxi"].Value.ToString());
                maxi++;
                pat_id.Text = maxi.ToString();
            }
            catch(Exception e)
            {
               pat_id.Text = "1";
            }           
            con.Close();
        }

        private void register_Click(object sender, EventArgs e)
        {
            //insert with stored proc
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "INS_PATIENT";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("nid", int.Parse(pat_id.Text));
            cmd.Parameters.Add("npass", pat_pass.Text);
            cmd.Parameters.Add("nname", pat_name.Text);
            cmd.Parameters.Add("nphone", pat_phone.Text);
            cmd.Parameters.Add("naccount", pat_acc.Text);
            cmd.Parameters.Add("birth", Convert.ToDateTime(dateTimePicker1.Text));
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //select multi rows with stored proc
            bool checker = false;
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "SELECT_ALL_PATIENTS";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("login", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string id = dr[0].ToString(), pass = dr[1].ToString();
                if(id == id_txt.Text && pass == pass_txt.Text)
                {
                    checker = true;
                    break;
                }
            }
            if (checker)
            {
                Patient_functions pf = new Patient_functions(id_txt.Text);
                pf.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Error in :(ID OR PASSWORD):");
            dr.Close();
            cn.Clone();
        }

        private void PatientLogin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void pat_acc_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
