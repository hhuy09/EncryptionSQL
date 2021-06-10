using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace lab03_nhom
{
    public partial class Form19 : Form
    {
        public string connectString;
        public string mssv;
        public string manv;
        public string htnv;
        public string msnv;
        public string pubkey;
        SqlConnection con;
        public Form19()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string mk = textBox5.Text;
                mk = "0x" + SimpleHash.HexHash(SHA1.Create(), mk);
                string luong = textBox2.Text;
                luong = "0x" + RSAAlgorithm.Base64ToHex(RSAAlgorithm.Encrypt(luong, pubkey));

                string sqlexec = "EXEC SP_UPDATE_NHANVIEN_CLIENT N'" + mssv + "', N'" + textBox3.Text + "', N'" + textBox6.Text + "', N'" + luong + "', N'" + textBox4.Text + "', '" + mk + "'"; 
                SqlCommand cmd = new SqlCommand(sqlexec, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thông tin thành công.");
                con.Close();
                this.Close();
            }
            catch (SqlException error)
            {
                string errorStr = error.ToString();
                string[] arrStr0 = errorStr.Split(':');
                string[] arrStr = arrStr0[1].Split('\n');
                MessageBox.Show(arrStr[0].ToString() + "\nCập nhật thông tin không thành công.");
            }
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            connectString = "Data Source=NDHHUY;Initial Catalog=QLSVNhom;Integrated Security=True";
            con = new SqlConnection(connectString);
            con.Open();

            label12.Text = manv;
            label13.Text = htnv;
            label6.Text = msnv;

            SqlCommand cmd = new SqlCommand("SP_SEL1_NHANVIEN_CLIENT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter pMANV = new SqlParameter("@MANV", SqlDbType.NVarChar, 20);
            SqlParameter pHOTEN = new SqlParameter("@HOTEN", SqlDbType.NVarChar, 100);
            SqlParameter pEM = new SqlParameter("@EMAIL", SqlDbType.VarChar, 20);
            SqlParameter pL = new SqlParameter("@LUONG", SqlDbType.VarChar, 1000000000);
            SqlParameter pTENDN = new SqlParameter("@TENDN", SqlDbType.NVarChar, 20);
            pMANV.Value = msnv;
            pHOTEN.Direction = ParameterDirection.Output;
            pTENDN.Direction = ParameterDirection.Output;
            pL.Direction = ParameterDirection.Output;
            pEM.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pMANV);
            cmd.Parameters.Add(pHOTEN);
            cmd.Parameters.Add(pTENDN);
            cmd.Parameters.Add(pL);
            cmd.Parameters.Add(pEM);
            cmd.ExecuteNonQuery();
            textBox3.Text = (string)pHOTEN.Value;
            textBox6.Text = (string)pEM.Value;
            //textBox2.Text = (string)pL.Value;
            textBox4.Text = (string)pTENDN.Value;
            cmd.Parameters.Clear();

            string lg = (string)pL.Value;
            lg = lg.Substring(2);

            //lg = RSAAlgorithm.Decrypt(RSAAlgorithm.HexToBase64(lg), );
        }
    }
}
