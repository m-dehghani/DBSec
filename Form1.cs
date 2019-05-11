using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Security;
using System.Configuration;
using System.Xml;

namespace DBSec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await BackupCertificate(txt_ServerIP.Text, txt_DB.Text,
                                Utility.ToInsecureString(Utility.DBPass),textBox1.Text);
        }

        private async Task BackupCertificate(string IP,string DB,string pass,string pathToBackup)
        {
            var constr = Utility.MakeConnectionStr(IP,DB,pass);
            var res = await Utility.TestDbConnection(constr);
            if (res != "Ok")
            {
                MessageBox.Show("خطا در اتصال به دیتابیس " + res);
                return;
            }
            using (SqlConnection conn = new SqlConnection(constr))

            {

                try
                {
                    await conn.OpenAsync();
                    var comm = string.Format(@"use master;
                                                    BACKUP CERTIFICATE MyServerCert TO FILE = N'{0}\MyServerCert.cer'
                                                    WITH PRIVATE KEY
                                                    (FILE = N'{0}\MyServerCert.pvk',ENCRYPTION BY PASSWORD = N'AReallyStr0ngK#y4You')",
                                                     pathToBackup);
                    SqlCommand command = new SqlCommand(comm, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("certificate با موفقیت کپی شد");
                    comm = string.Format(@"BACKUP MASTER KEY TO FILE = N'{0}\MasterKey'  ENCRYPTION BY PASSWORD = 'admin@123';",
                        pathToBackup);
                    command = new SqlCommand(comm, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Master Key با موفقیت کپی شد");
                    conn.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task EncryptDB(string IP, string DB, string pass)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(txt_ServerIP.Text, txt_DB.Text,
                      Utility.ToInsecureString(Utility.DBPass)));
                //SqlCommand testcom = new SqlCommand(string.Format(@"USE master;
                //CREATE MASTER KEY ENCRYPTION BY PASSWORD = '{0}';
                //CREATE CERTIFICATE MyServerCert WITH SUBJECT = 'My DEK Certificate';
                //USE {1};
                //CREATE DATABASE ENCRYPTION KEY
                //                                          WITH ALGORITHM = AES_128
                //                                          ENCRYPTION BY SERVER CERTIFICATE MyServerCert;
                //ALTER DATABASE {1}
                //SET ENCRYPTION ON; ", Utility.ToInsecureString(Utility.DBPass),"test2"), conn);
                SqlCommand command = new SqlCommand(string.Format(@"USE master;
                                                      CREATE MASTER KEY ENCRYPTION BY PASSWORD ='{0}';
                                                      CREATE CERTIFICATE MyServerCert WITH SUBJECT = 'My DEK Certificate';
                                                      USE {1};
                                                      CREATE DATABASE ENCRYPTION KEY
                                                      WITH ALGORITHM = AES_128
                                                      ENCRYPTION BY SERVER CERTIFICATE MyServerCert;  
                                                      ALTER DATABASE {1}
                                                      SET ENCRYPTION ON;", Utility.ToInsecureString(Utility.DBPass), txt_DB.Text),
                                                                    conn);
                //await conn.OpenAsync();
                //await testcom.ExecuteNonQueryAsync();
                //conn.Close();



                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                MessageBox.Show("encryption با موفقیت انجام شد");

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        public async Task<string> ReadFromTiny()
        {
            Tn.ServerIP = "127.0.0.1";
            Tn.NetWorkINIT = true;
            if (Tn.TinyErrCode == 0)
            {
                Tn.UserPassWord = TinyCode.Text;
                Tn.ShowTinyInfo = true;
                var data = Tn.DataPartition.Split('@');
                return data[1];
            }
            else
            {
                Tn.Initialize = true;
                if (Tn.TinyErrCode == 0)
                {
                    Tn.UserPassWord = TinyCode.Text;
                    Tn.ShowTinyInfo = true;
                    Tn.UserPassWord = TinyCode.Text;
                    Tn.ShowTinyInfo = true;
                    if (Tn.DataPartition == "") return "error";
                    var data = Tn.DataPartition.Split('@');

                    return data[1];
                }
                else
                {
                    return null;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            SecTab.Enabled = false;
            panel1.Enabled = false;
            label2.Text = label3.Text =label10.Text= label11.Text=label12.Text="";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private async void Button5_Click(object sender, EventArgs e)
        {

            if ((string.IsNullOrEmpty(txt_DB.Text.Trim())) || (string.IsNullOrEmpty(txt_ServerIP.Text.Trim())))
            {
                MessageBox.Show("لطفا تمام فیلدها را پر نمایید");
            }
            else
            {
                var rawConstr = Utility.MakeConnectionStr(txt_ServerIP.Text, txt_DB.Text,
                    Utility.ToInsecureString(Utility.DBPass));
                var res =await  Utility.TestDbConnection(rawConstr);
                if (res != "Ok")
                {
                    MessageBox.Show("خطا در اتصال به دیتابیس " + res);
                    return;
                }
                var secure = Utility.ToSecureString(
                    rawConstr);

                string conStr = Utility.EncryptString(secure);
                ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
                configXmlDocument.Load(textBox3.Text);
                configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).InnerXml = string.Format("<add key=\"conn1\" value=\"{0}\" />", conStr) + configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).InnerXml;
                configXmlDocument.Save(textBox3.Text);

            }
        }


        private async void Button6_Click(object sender, EventArgs e)
        {

            var result =await ReadFromTiny();
            if (result == null || result == "error")
            {
               
                panel1.Enabled = false;
                MessageBox.Show("خطا در خواندن");
                TinyCode.BackColor = Color.Red;
                txt_Pass.BackColor = Color.Red;
            }
            else
            {
                panel1.Enabled = true;
                Utility.DBPass = Utility.ToSecureString(result);
                Utility.entropy = System.Text.Encoding.Unicode.GetBytes(result);
                txt_Pass.BackColor = Color.Lime;
                TinyCode.BackColor = Color.Lime;
             
            }


        }

        private void Txt_ServerIP_TextChanged(object sender, EventArgs e)
        {
            SecTab.Enabled = false;
        }

        private async void Button8_Click(object sender, EventArgs e)
        {


            if ((string.IsNullOrEmpty(txt_DB.Text.Trim())) || (string.IsNullOrEmpty(txt_ServerIP.Text.Trim())))
            {
                MessageBox.Show("لطفا تمام فیلدها را پر نمایید");
            }
            else
            {
                var rawConstr = Utility.MakeConnectionStr(txt_ServerIP.Text, txt_DB.Text,
                    Utility.ToInsecureString(Utility.DBPass));
                var res =await Utility.TestDbConnection(rawConstr);
                if (res != "Ok")
                {
                    label10.ForeColor = Color.Red;
                    SecTab.Enabled = false;
                    label10.Text="خطا در اتصال به دیتابیس " + res;
                    return;
                }
                else
                {
                    //abel10.BackColor = Color.
                    label10.ForeColor = Color.Lime;
                    SecTab.Enabled = true;
                    label10.Text = "\u2714" ;
                }
            }
        }

        private void Txt_DB_TextChanged(object sender, EventArgs e)
        {
            SecTab.Enabled = false;
        }

        private async void Button7_Click(object sender, EventArgs e)
        {
            await EncryptDB(txt_ServerIP.Text, txt_DB.Text,
                                Utility.ToInsecureString(Utility.DBPass));
            await BackupCertificate(txt_ServerIP.Text, txt_DB.Text,
                                Utility.ToInsecureString(Utility.DBPass),textBox5.Text);
            label12.Text = "\u2714";
        }

        

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button10_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(txt_ServerIP.Text, "master", textBox4.Text)))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(

                      string.Format(@"ALTER LOGIN [sa] WITH PASSWORD='{0}';ALTER LOGIN sa WITH DEFAULT_DATABASE = [master];", Utility.ToInsecureString(Utility.DBPass)), conn);

                   
                    comm.ExecuteNonQuery();
                    textBox4.BackColor = Color.Lime;
                    label11.Text = "\u2714" ;
                   
                    conn.Close();
                }
                catch(Exception ex)
                {
                    
                    textBox4.BackColor = Color.Red;
                   
                    MessageBox.Show(ex.Message);
                    
                }
            }
        }

        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void TabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Button11_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            textBox5.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
