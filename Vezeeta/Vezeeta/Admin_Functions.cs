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
    public partial class Admin_Functions : Form
    {
        int close = 0;
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        public Admin_Functions()
        {
            InitializeComponent();

            //slect without where
            OracleConnection con = new OracleConnection(conStr);
            con.Open();
            OracleCommand com = new OracleCommand();
            com.Connection = con;
            com.CommandText = "select NAME,PHONE from CLINIC";
            com.CommandType = CommandType.Text;
            OracleDataReader or = com.ExecuteReader();
            while (or.Read())
            {
                listBox1.Items.Add(or[0].ToString() + " , " + or[1].ToString());
                listBox2.Items.Add(or[0].ToString() + " , " + or[1].ToString());
            }
            or.Close();
            con.Close();
        }

        private void del_doc_Click(object sender, EventArgs e)
        {
            //delete without stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = cn;
            cmd1.CommandText = "delete from doctors where ID = :id";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("id", int.Parse(doc_id.Text));
            cmd1.ExecuteNonQuery();

            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = cn;
            cmd2.CommandText = "delete from doctor where ACCOUNT = :acc";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("acc", doc_account.Text);
            cmd2.ExecuteNonQuery();

            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = cn;
            cmd3.CommandText = "delete from WORK_AT where DOCTOR_ACCOUNT = :acc1";
            cmd3.CommandType = CommandType.Text;
            cmd3.Parameters.Add("acc1", doc_account.Text);
            cmd3.ExecuteNonQuery();

            OracleCommand cmd4 = new OracleCommand();
            cmd4.Connection = cn;
            cmd4.CommandText = "delete from APPOINTMENT where DOCTOR_ACCOUNT = :acc2";
            cmd4.CommandType = CommandType.Text;
            cmd4.Parameters.Add("acc2", doc_account.Text);
            cmd4.ExecuteNonQuery();

            OracleCommand cmd5 = new OracleCommand();
            cmd5.Connection = cn;
            cmd5.CommandText = "delete from ABOUT_DOCTOR where DOCTOR_ACCOUNT = :acc3";
            cmd5.CommandType = CommandType.Text;
            cmd5.Parameters.Add("acc3", doc_account.Text);
            cmd5.ExecuteNonQuery();

            cn.Close();

            MessageBox.Show("DONE");
        }

        private void del_clinic_Click(object sender, EventArgs e)
        {
            string[] spliter = new string[2];
            spliter = listBox1.SelectedItem.ToString().Split(',');
            string target = "";
            for(int i = 1; i < spliter[1].Count(); i++)
            {
                target = target + spliter[1][i];
            }

            //delete with stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "DEL_CLINIC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("mob", target);
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void add_doc_Click(object sender, EventArgs e)
        {
            //insert without stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "insert into DOCTOR values(:acc,:sp,:ag,:fee,:name)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("acc", doc_acc.Text);
            cmd.Parameters.Add("sp", doc_sp.Text);
            cmd.Parameters.Add("ag", int.Parse(doc_age.Text));
            cmd.Parameters.Add("fee", int.Parse(doc_fees.Text));
            cmd.Parameters.Add("name", doc_name.Text);
            cmd.ExecuteNonQuery();

            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = cn;
            cmd1.CommandText = "insert into ABOUT_DOCTOR values(:acc1,:edu,:st,:back)";
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.Add("acc1", doc_acc.Text);
            cmd1.Parameters.Add("edu", edu_txt.Text);
            cmd1.Parameters.Add("st", state_txt.Text);
            cmd1.Parameters.Add("back", back_txt.Text);
            cmd1.ExecuteNonQuery();

            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = cn;
            cmd2.CommandText = "insert into WORK_ADDRESS values(:acc2,:ad)";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("acc2", doc_acc.Text);
            cmd2.Parameters.Add("ad", doc_add.Text);         
            cmd2.ExecuteNonQuery();

            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = cn;
            cmd3.CommandText = "insert into DOCTORS values(:ids,:psw)";
            cmd3.CommandType = CommandType.Text;
            cmd3.Parameters.Add("ids", int.Parse(doc_id_txt.Text));
            cmd3.Parameters.Add("psw", Doc_pas_txt.Text);
            cmd3.ExecuteNonQuery();

            cn.Close();

            MessageBox.Show("DONE");
        }

        private void add_clinic_Click(object sender, EventArgs e)
        {
            //insert with stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "ADD_CLINIC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("mob", clinic_phone.Text);
            cmd.Parameters.Add("namee", clinic_name.Text);
            cmd.Parameters.Add("addr", clinic_add.Text);
            cmd.Parameters.Add("cos", int.Parse(clinic_cost.Text));
            cmd.Parameters.Add("timee", int.Parse(clinic_time.Text));
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void upd_Click(object sender, EventArgs e)
        {
            //update with stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "UPD_ADMIN";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("idd", int.Parse(admin_id.Text));
            cmd.Parameters.Add("pass", admin_pass.Text);
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void doc_clinic_Click(object sender, EventArgs e)
        {
            string DOC_account = doclinic_account.Text;
            string[] spliter = new string[2];
            spliter = listBox2.SelectedItem.ToString().Split(',');
            string target = "";
            for (int i = 1; i < spliter[1].Count(); i++)
            {
                target = target + spliter[1][i];
            }

            //insert with stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "ADD_DOC_IN_CLINIC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("mob", target);
            cmd.Parameters.Add("pass", DOC_account);
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void Admin_Functions_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Admin_Functions_Load(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void panel14_Paint(object sender, PaintEventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void panel12_Paint(object sender, PaintEventArgs e) { }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
