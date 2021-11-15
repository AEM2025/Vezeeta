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
    public partial class Patient_functions : Form
    {
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        int close = 0, maxi;
        string id, loc, tm = "";
        public Patient_functions(string gates)
        {
            InitializeComponent();
            id = gates;

            all_clinics();
            all_phones();
        }

        void all_clinics()
        {
            //select all rows (discon).
            OracleDataAdapter adapter = new OracleDataAdapter("select * from CLINIC", conStr);
            DataSet dst = new DataSet();
            adapter.Fill(dst);
            dataGridView2.DataSource = dst.Tables[0];
        }

        void all_phones()
        {
            //select without where
            OracleConnection con = new OracleConnection(conStr);
            con.Open();
            OracleCommand com = new OracleCommand();
            com.Connection = con;
            com.CommandText = "select_clinic_phones";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("src", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader odr = com.ExecuteReader();
            while (odr.Read())
            {
                cmb_clinic.Items.Add(odr[0].ToString());
            }
            odr.Close();
            con.Close();
        }

        private void Vote_Click(object sender, EventArgs e)
        {

            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "ADD_REVIEW";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("acc", vote_acc.Text);
            cmd.Parameters.Add("grade", int.Parse(rate.Text));
            cmd.Parameters.Add("com", comment.Text);
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }

        private void upd_pass_Click(object sender, EventArgs e)
        {
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "UPD_PATIENTPASS";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("identifer", int.Parse(id));
            cmd.Parameters.Add("pass", pass_upd.Text);
            cmd.ExecuteNonQuery();
            cn.Close();

            MessageBox.Show("DONE");
        }


        OracleDataAdapter adp;
        DataSet das;
        OracleCommandBuilder builder;
        private void show_btn_Click(object sender, EventArgs e)
        {
            adp = new OracleDataAdapter("select * from APPOINTMENT where PATIENT_ACCOUNT = :pacc", conStr);
            das = new DataSet();
            adp.SelectCommand.Parameters.Add("pacc", acc_pat.Text);
            adp.Fill(das);
            dataGridView3.DataSource = das.Tables[0];
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            builder = new OracleCommandBuilder(adp);
            adp.Update(das.Tables[0]);
            MessageBox.Show("DONE");
        }

        private void cmb_clinic_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_sp.Items.Clear();
            Dictionary<string, int> mp = new Dictionary<string, int>();  //No Duplicate in Speciality.

            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "select DOCTOR.SPECIALTY from DOCTOR,WORK_AT where WORK_AT.DOCTOR_ACCOUNT = DOCTOR.ACCOUNT and WORK_AT.CLINIC_PHONE = :ph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("ph", cmb_clinic.Text);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                try
                {
                    int tmp = mp[dr[0].ToString()];
                }
                catch(Exception eq)
                {
                    cmb_sp.Items.Add(dr[0].ToString());
                    mp[dr[0].ToString()] = 1;
                }
                
            }
            dr.Close();
            cn.Close();
        }

        private void cmb_sp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string com_txt = @"select DOCTOR.DR_NAME,DOCTOR.FEES,DOCTOR.AGE,DOCTOR.ACCOUNT
                               from DOCTOR,WORK_AT where DOCTOR.SPECIALTY = :sp and
                                WORK_AT.DOCTOR_ACCOUNT = DOCTOR.ACCOUNT and
                                WORK_AT.CLINIC_PHONE = :cp";
            OracleDataAdapter dp = new OracleDataAdapter(com_txt, conStr);
            dp.SelectCommand.Parameters.Add("sp", cmb_sp.Text);
            dp.SelectCommand.Parameters.Add("cp", cmb_clinic.Text);
            DataSet ds = new DataSet();
            dp.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        void get_needed()
        {
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "select ADDRESS from CLINIC where PHONE = :ph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("ph", cmb_clinic.Text);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                loc = dr[0].ToString();
            dr.Close();


            OracleCommand cmd1 = new OracleCommand();
            cmd1.Connection = cn;
            cmd1.CommandText = "MAX_APPOINTMENT_ID";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add("iden", OracleDbType.Int32, ParameterDirection.Output);
            cmd1.ExecuteNonQuery();
            try
            {
                maxi = Convert.ToInt32(cmd1.Parameters["iden"].Value.ToString());
                maxi++;
            }
            catch(Exception e)
            {
                maxi = 1;
            }

            tm = "";
            if (am.Checked)
                tm = tm + tim.Text + "am";
            else
                tm = tm + tim.Text + "pm";

            cn.Close();
        }

        bool check_avial(string date, string t,string doc_acc)
        {
            bool ans = true;
            OracleConnection on = new OracleConnection(conStr);
            on.Open();
            OracleCommand cm = new OracleCommand();
            cm.Connection = on;
            cm.CommandText = "select DAYS,TIME from APPOINTMENT where DOCTOR_ACCOUNT = :dc";
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add("dc", doc_acc);
            OracleDataReader dor = cm.ExecuteReader();
            while (dor.Read())
            {
                if (Convert.ToDateTime(date).ToString() == dor[0].ToString() && dor[1].ToString() == t)
                {
                    ans = false; break;
                }
            }
            dor.Close();
            on.Close();
            
            return ans;
        }

        private void book_btn_Click(object sender, EventArgs e)
        {
            get_needed();
            bool ch = check_avial(dateTimePicker1.Text, tim.Text, book_account.Text);

            if (ch == true)
            {
                OracleConnection cn = new OracleConnection(conStr);
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.CommandText = "INS_APP";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("iden", maxi);
                cmd.Parameters.Add("ph", cmb_clinic.Text);
                cmd.Parameters.Add("loc", loc);
                cmd.Parameters.Add("youm", Convert.ToDateTime(dateTimePicker1.Text));
                cmd.Parameters.Add("tm", tm);
                cmd.Parameters.Add("pa", pat_accc.Text);
                cmd.Parameters.Add("da", book_account.Text);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("DONE");
            }
            else
                MessageBox.Show("Doctor Busy in this time");

        }

        private void Patient_functions_FormClosing(object sender, FormClosingEventArgs e)
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


        private void panel12_Paint(object sender, PaintEventArgs e) { }
        private void vote_acc_TextChanged(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel7_Paint(object sender, PaintEventArgs e) { }

        
    }
}
