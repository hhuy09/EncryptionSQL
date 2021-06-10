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

namespace lab03_nhom
{
    public partial class Form18 : Form
    {
        public string connectString;
        public string tenDN;
        public string pass;
        public string manv;
        public string hoten;
        public string msnv;
        SqlConnection con;
        public Form18()
        {
            InitializeComponent();
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            connectString = "Data Source=NDHHUY;Initial Catalog=QLSVNhom;Integrated Security=True";
            con = new SqlConnection(connectString);
            con.Open();

            label1.Text = msnv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlexec1 = "EXEC SP_DEL_NHANVIEN '" + msnv + "'";
                SqlCommand cmd1 = new SqlCommand(sqlexec1, con);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Xóa nhân viên thành công.");
                con.Close();
                this.Close();
            }
            catch (SqlException error)
            {
                string errorStr = error.ToString();
                string[] arrStr0 = errorStr.Split(':');
                string[] arrStr = arrStr0[1].Split('\n');
                MessageBox.Show(arrStr[0].ToString() + "\nXóa nhân viên không thành công.");
            }
        }
    }
}
