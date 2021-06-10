using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace lab03_nhom
{
    public partial class Form12 : Form
    {
        public string connectString;
        public string tenDN;
        public string pass;
        public string manv;
        public string hoten;
        string msnv;
        string pubkey;
        SqlConnection con;
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            connectString = "Data Source=NDHHUY;Initial Catalog=QLSVNhom;Integrated Security=True";
            con = new SqlConnection(connectString);
            con.Open();

            label1.Text = hoten;
            label2.Text = manv;

            string sqlexec1 = "EXEC SP_SEL_PUBLIC_NHANVIEN_CLIENT";
            SqlCommand cmd1 = new SqlCommand(sqlexec1, con);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);       
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["HOTEN"].Width = 170;

            int count = dataGridView1.Rows.Count - 1;
            label5.Text = count.ToString();

            msnv = dataGridView1.Rows[0].Cells[0].Value.ToString();
            pubkey = dataGridView1.Rows[0].Cells[6].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            con.Close();
            Form5 f5 = new Form5();
            f5.tenDN = tenDN;
            f5.pass = pass;
            this.Close();
            f5.Show();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form17 f17 = new Form17();
            f17.connectString = connectString;
            f17.manv = manv;
            f17.pass = pass;
            f17.hoten = hoten;
            f17.tenDN = tenDN;
            f17.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlexec1 = "EXEC SP_SEL_PUBLIC_NHANVIEN_CLIENT";
            SqlCommand cmd1 = new SqlCommand(sqlexec1, con);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["HOTEN"].Width = 170;

            int count = dataGridView1.Rows.Count - 1;
            label5.Text = count.ToString();

            msnv = dataGridView1.Rows[0].Cells[0].Value.ToString();
            pubkey = dataGridView1.Rows[0].Cells[6].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form18 f18 = new Form18();
            f18.msnv = msnv;
            f18.connectString = connectString;
            f18.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                msnv = row.Cells[0].Value.ToString();
                pubkey = row.Cells[6].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(manv != msnv)
            {
                MessageBox.Show("Không có quyền cập nhật nhân viên này.");
            }
            else
            {
                Form19 f19 = new Form19();
                f19.msnv = msnv;
                f19.htnv = hoten;
                f19.manv = manv;
                f19.pubkey = pubkey;
                f19.connectString = connectString;
                f19.Show();
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.Rows.Count - 1;

            string prikey = "";

            OpenFileDialog dlg = new OpenFileDialog();
            string path = null;
            dlg.Title = "Chọn privatekey";
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = dlg.FileName;
            }

            try
            {
                prikey = System.IO.File.ReadAllText(path);
            }
            catch
            {

            }

            for (int r =0; r<row; r++)
            {
                string lg = dataGridView1.Rows[r].Cells[3].Value.ToString();

                try
                {
                    if (lg.Length > 0)
                    {
                        lg = lg.Substring(2);
                        lg = RSAAlgorithm.Decrypt(RSAAlgorithm.HexToBase64(lg), prikey);
                        dataGridView1.Rows[r].Cells[3].Value = lg;
                    }
                }
                catch
                {
                    dataGridView1.Rows[r].Cells[3].Value = null;
                }
            }
        }
    }
}
