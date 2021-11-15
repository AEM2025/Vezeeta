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
    public partial class Doc_Functions : Form
    {
        string id; int close = 0;
        string conStr = "Data source=orcl;User Id=Vezeeta; Password=hassan;";
        public Doc_Functions(string gets)
        {
            InitializeComponent();
            id = gets;
        }

        private void upd_Click(object sender, EventArgs e)
        {
            //update without stored
            OracleConnection cn = new OracleConnection(conStr);
            cn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = "update DOCTORS set PASSWORD = :pass where ID = :id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("pass", Doc_pass.Text);
            cmd.Parameters.Add("id", int.Parse(id));
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("DONE");
        }

        private void show_btn_Click(object sender, EventArgs e)
        {
            //master detail
            DataSet ds = new DataSet();
            OracleDataAdapter dp1 = new OracleDataAdapter("select * from doctor where ACCOUNT = :occ",conStr);
            dp1.SelectCommand.Parameters.Add("occ", doc_acc.Text);
            dp1.Fill(ds, "docData");
            OracleDataAdapter dp2 = new OracleDataAdapter("select * from APPOINTMENT where DOCTOR_ACCOUNT = :occ1", conStr);
            dp2.SelectCommand.Parameters.Add("occ1", doc_acc.Text);
            dp2.Fill(ds, "appData");
            DataRelation r = new DataRelation("da", ds.Tables[0].Columns["ACCOUNT"], ds.Tables[1].Columns["DOCTOR_ACCOUNT"]);
            ds.Relations.Add(r);
            BindingSource master = new BindingSource(ds, "docData");
            BindingSource child = new BindingSource(master, "da");
            dataGridView2.DataSource = master;
            dataGridView1.DataSource = child;
        }

        private void Doc_Functions_FormClosing(object sender, FormClosingEventArgs e)
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



        private void label4_Click(object sender, EventArgs e) { }
        private void Doc_Functions_Load(object sender, EventArgs e) { }
    }
}
