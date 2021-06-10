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
using System.Security.Cryptography;
using System.IO;

namespace lab03_nhom
{
    public partial class Form17 : Form
    {
        public string connectString;
        public string tenDN;
        public string pass;
        public string manv;
        public string hoten;
        SqlConnection con;

        public Form17()
        {
            InitializeComponent();
        }

        private void Form17_Load(object sender, EventArgs e)
        {
            connectString = "Data Source=NDHHUY;Initial Catalog=QLSVNhom;Integrated Security=True";
            con = new SqlConnection(connectString);
            con.Open();

            label13.Text = hoten;
            label12.Text = manv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msnv = textBox1.Text;
            string htnv = textBox3.Text;
            string email = textBox6.Text;
            string luong = textBox2.Text;
            string tendn = textBox4.Text;
            string mk = textBox5.Text;
            string pub = "";
            mk = "0x" + SimpleHash.HexHash(SHA1.Create(), mk);
            
            var cryptoServiceProvider = new RSACryptoServiceProvider(2048);
            var privateKey = cryptoServiceProvider.ExportParameters(true);
            var publicKey = cryptoServiceProvider.ExportParameters(false);

            string publicKeyString = RSAAlgorithm.GetKeyString(publicKey);
            string privateKeyString = RSAAlgorithm.GetKeyString(privateKey);

            
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            string FoderName = "";
            dlg.Description = "Chọn folder lưu key";
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                FoderName = dlg.SelectedPath;
                MessageBox.Show("Key lưu tại " + FoderName);
                Environment.SpecialFolder root = dlg.RootFolder;
            }

            dlg.ShowNewFolderButton = true;
           
         
            string fname1 = "PublicKey_" + msnv + ".xml";
            string fname2 = "PrivateKey_" + msnv + ".xml";

            string cbine1 = Path.Combine(FoderName, fname1);
            string cbine2 = Path.Combine(FoderName, fname2);

            if (File.Exists(FoderName + @"\" + fname1))
            {
                File.Delete(cbine1);
            }

            if (File.Exists(FoderName + @"\" + fname2))
            {
                File.Delete(cbine2);
            }

            File.Create(cbine1).Close();
            File.Create(cbine2).Close();

            System.IO.File.WriteAllText(FoderName + @"\" + fname1, publicKeyString);
            System.IO.File.WriteAllText(FoderName + @"\" + fname2, privateKeyString);

            luong = "0x" + RSAAlgorithm.Base64ToHex(RSAAlgorithm.Encrypt(luong, publicKeyString));

            pub = publicKeyString;

            try
            {
                string sqlexec1 = "EXEC SP_INS_PUBLIC_ENCRYPT_NHANVIEN '" + msnv + "', N'" + htnv + "', '" + email + "', '" + luong + "', '" + tendn + "', '" + mk + "', '" + pub + "'";
                SqlCommand cmd1 = new SqlCommand(sqlexec1, con);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Thêm nhân viên thành công.");
                con.Close();
                this.Close();
            }
            catch (SqlException error)
            {
                string errorStr = error.ToString();
                string[] arrStr0 = errorStr.Split(':');
                string[] arrStr = arrStr0[1].Split('\n');
                MessageBox.Show(arrStr[0].ToString() + "\nThêm nhân viên không thành công.");
            }
        }
    }
}
