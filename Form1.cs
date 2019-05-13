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
using System.Threading;
using System.Diagnostics;

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
            await BackupDataBase(txt_ServerIP.Text, txt_DB.Text,
                                Utility.ToInsecureString(Utility.DBPass), textBox1.Text);
        }
        private async Task BackupDataBase(string IP, string DB, string pass, string pathToBackup)
        {
            var comomand = "";
            var constr = Utility.MakeConnectionStr(IP, DB, pass);
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
                    var comm = string.Format(@" use master; OPEN MASTER KEY DECRYPTION BY PASSWORD = '{0}';BACKUP DATABASE a TO DISK=N'{1}\test.bak';",pass,pathToBackup);
                    SqlCommand command = new SqlCommand(comm, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Backup با موفقیت کپی شد");
                    conn.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

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
                    comm = string.Format(@"BACKUP MASTER KEY TO FILE = N'{0}\MasterKey.key'  ENCRYPTION BY PASSWORD = 'admin@123';",
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
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(IP, DB,
                      pass));
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
                                                      SET ENCRYPTION ON;", pass, DB), conn);
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

        private async Task RestoreCertificateAndDb(string IP, string DB, string pass,string masterKeyPath, string certificatePath,string privateKeyPath,string DbPath,string ldfPath,string fileType)
        {


            try
            {
                
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(IP, DB, pass));
                string textCommad;
                if (fileType == "mdf")
                {

                   textCommad = string.Format(@"use master;

RESTORE MASTER KEY   
                              FROM FILE = N'{0}'   
                              DECRYPTION BY PASSWORD = '{5}'   
                              ENCRYPTION BY PASSWORD = '{5}';  
                              OPEN MASTER KEY DECRYPTION BY PASSWORD = '{5}'  
                              use master;
                              create certificate MyServerCert
                              from file = N'{1}'
                              with private key
                                    ( file = N'{2}'
                                        , decryption by password = N'AReallyStr0ngK#y4You'
                                    )
                              CREATE DATABASE Sinad   
                              ON (FILENAME = '{3}'),   
                                 (FILENAME = '{4}')   
                              FOR ATTACH;", masterKeyPath, certificatePath, privateKeyPath, DbPath, ldfPath,"admin@123");
                }
                else
                {
                    textCommad = string.Format(@"RESTORE MASTER KEY   
                              FROM FILE = N'{0}'   
                              DECRYPTION BY PASSWORD = '{4}'
                              ENCRYPTION BY PASSWORD = '{4}';  
                              OPEN MASTER KEY DECRYPTION BY PASSWORD = '{4}'  
                              use master;
                              create certificate MyServerCert
                              from file = N'{1}'
                              with private key
                                    ( file = N'{2}'
                                        , decryption by password = N'AReallyStr0ngK#y4You'
                                    )

RESTORE DATABASE Sinad

FROM DISK = '{3}'", masterKeyPath, certificatePath, privateKeyPath, DbPath,pass);
                }
                SqlCommand command = new SqlCommand(textCommad, conn);
                await conn.OpenAsync();
                command.ExecuteNonQuery();
                MessageBox.Show("با موفقیت انجام شد");
                conn.Close();

            }
            catch(Exception ex)
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
            radioButton2.Checked = true;
            //SecTab.Enabled = false;
            panel1.Enabled=panel3.Enabled = false;
            label2.Text =label7.Text= label3.Text =label10.Text= label11.Text=label12.Text= label9.Text= label20.Text= label21.Text="";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private async void Button5_Click(object sender, EventArgs e)
        {

            try
            {
                if ((string.IsNullOrEmpty(txt_DB.Text.Trim())) || (string.IsNullOrEmpty(txt_ServerIP.Text.Trim())))
                {
                    MessageBox.Show("لطفا تمام فیلدها را پر نمایید");
                }
                else
                {
                   
                    var rawConstr ="provider=sqloledb.1;"+ Utility.MakeConnectionStr(txt_ServerIP.Text, txt_DB.Text,
                        Utility.ToInsecureString(Utility.DBPass));
                    //var res = await Utility.TestDbConnection(rawConstr);
                    //if (res != "Ok")
                    //{
                    //    MessageBox.Show("خطا در اتصال به دیتابیس " + res);
                    //    return;
                    //}

                  

                    string conStr = Utility.Encrypt(rawConstr);
                

                    ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
                    configXmlDocument.Load(textBox3.Text);
                    var c = configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).ChildNodes;
                  
                    bool found = false;
                    foreach (XmlNode node in configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).ChildNodes)
                    {
                        if (node.Attributes["key"].Value == "conn")
                        {
                            found = true;
                            node.Attributes["value"].Value = conStr;
                        }

                    }
                    if (found != true)
                    {
                        configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).InnerXml = string.Format("<add key=\"conn\" value=\"{0}\" />", conStr) + configXmlDocument.DocumentElement.GetElementsByTagName("appSettings").Item(0).InnerXml;
                    }


                    //foreach (XmlNode node in configXmlDocument.DocumentElement.GetElementsByTagName("userSettings").Item(0).ChildNodes)
                    //   "TinyServerID"

                    configXmlDocument.Save(textBox3.Text);
                    label20.Text = "کانفیگ سرور ساخته شد";
                    var newnode = configXmlDocument.DocumentElement.GetElementsByTagName("Sinad.Properties.Settings").Item(0).ChildNodes[6];
                    newnode.InnerText = textBox9.Text;

                    configXmlDocument.Save(textBox3.Text + ".client");
                    label21.Text = "کانفیگ کلاینت با ساخته شد";

                    label19.Text = "\u2714";


                }
            }catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private async void Button6_Click(object sender, EventArgs e)
        {
          
            
            try
            {
                var result = await ReadFromTiny();


                if (result == null || result == "error")
                {
                    panel1.Enabled = false;
                    MessageBox.Show("خطا در خواندن");
                    TinyCode.BackColor = Color.Red;

                    button6.BackColor = Color.Red;
                }
                else
                {
                    panel1.Enabled = true;
                    Utility.DBPass = Utility.ToSecureString(result.Trim());
                    Utility.passPhrase = Utility.ToSecureString(result.Trim());
                   
                    button6.BackColor = Color.Green;
                    TinyCode.BackColor = Color.Lime;

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        



        private void Txt_ServerIP_TextChanged(object sender, EventArgs e)
        {
            //SecTab.Enabled = false;
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
                    button8.BackColor = Color.Red;
                    panel3.Enabled = true;
                    label7.Text = "ابتدا پسورد را تغییر دهید";
                    return;
                }
                else
                {
                    //abel10.BackColor = Color.
                    button8.BackColor = Color.Lime;
                    panel3.Enabled = false;
                    label10.ForeColor = Color.Lime;
                    SecTab.Enabled = true;
                    label10.Text = "\u2714" ;
                }
            }
        }

        private void Txt_DB_TextChanged(object sender, EventArgs e)
        {
           // SecTab.Enabled = false;
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
            openFileDialog1.Filter = "key files(*.key)|*.key";
               var res= openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
                textBox2.Text = openFileDialog1.FileName;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Button10_Click(object sender, EventArgs e)
        {
            
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
            if (res == DialogResult.OK)
                textBox5.Text = folderBrowserDialog1.SelectedPath;
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "certificate files(*.cer)|*.cer";
            var res= openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
                textBox6.Text = openFileDialog1.FileName;
        }

        private void Button13_Click(object sender, EventArgs e)
        {
           
            var res= openFileDialog1.ShowDialog();
            if(res==DialogResult.OK)
                textBox7.Text = openFileDialog1.FileName;
        }

        private async void Button9_Click(object sender, EventArgs e)
        {
            var ldfpath =textBox7.Text.Remove(textBox7.Text.Length-3,3)+"ldf";
            var filetype = "mdf";
            if (radioButton1.Checked == true) filetype = "mdf"; else filetype = "bak";
            await RestoreCertificateAndDb(txt_ServerIP.Text,"master", Utility.ToInsecureString(Utility.DBPass),textBox2.Text
                ,textBox6.Text,textBox8.Text,textBox7.Text,ldfpath,filetype);
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "private key files(*.pvk)|*.pvk";
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
                textBox8.Text = openFileDialog1.FileName;
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(txt_ServerIP.Text, txt_DB.Text, textBox4.Text)))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(

                      string.Format(@"ALTER LOGIN [sa] WITH PASSWORD='{0}';ALTER LOGIN sa WITH DEFAULT_DATABASE = [master];", Utility.ToInsecureString(Utility.DBPass)), conn);


                    comm.ExecuteNonQuery();
                    textBox4.BackColor = Color.Lime;
                    label11.Text = "\u2714";

                    conn.Close();
                    button8.BackColor = Color.Green;
                    await DisableAllUserButSa(txt_ServerIP.Text, "sinad", Utility.ToInsecureString(Utility.DBPass));
                }
                catch (Exception ex)
                {

                    textBox4.BackColor = Color.Red;

                    MessageBox.Show(ex.Message);

                }
            }

        }

        public async Task DisableAllUserButSa(string address,string db,string pass)
        {
            try
            {
                string strCommand = @"SELECT 'ALTER LOGIN ' + QUOTENAME(sp.name) + ' DISABLE;'
                                  FROM sys.server_principals sp
                                  WHERE sp.principal_id > 100   
                                  AND sp.is_disabled = 0
                                  AND sp.type IN ( 'U'   , 'S'  ) ;";
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(address, db, pass));
                
                SqlCommand command = new SqlCommand(strCommand, conn);
                await conn.OpenAsync();
                var reader=command.ExecuteReader();
               
                List<string> commands = new List<string>();
                while (reader.Read())
                {
                    commands.Add(reader[0].ToString());
                   
                }
                conn.Close();
                conn.Open();
                SqlCommand alterLoginCommand;
                commands.ForEach(c =>
                {
                    alterLoginCommand = new SqlCommand(c, conn);
                    alterLoginCommand.ExecuteNonQuery();
                });

                
                MessageBox.Show("تمامی کاربران غیرفعال شدند");
                conn.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
           openFileDialog1.Filter = "config files(*.config)|*.config|All files(*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            var res=openFileDialog1.ShowDialog();

            if (res == DialogResult.OK)
                textBox3.Text = openFileDialog1.FileName;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "DB files(*.mdf)|*.mdf";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Backup files(*.bak)|*.bak";
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
          
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            var str = "eqU4G5/Vuvwzc+DRnTCjB2YcangOex3rK3IsrzV7u0nZQQazYUv4xF/ENi8x4VZmI9kaYUcO0ov1syGYzMkmZLdKYQW7LN5MYmHS5n/j7d6pcrwdJ6fNpB+r+eXvfBVX732eW8z9Rn2hZDZ4S9poajznX5dclPQXIkqYLa33Y4L390uz7wStBTADAquUKJoCZa38uHK94cUiVbRRaEkRJnRqoQbsQpi+tI81qznlnZYALLY0USo9nKzBXZYYTQmL";
            Debug.WriteLine(Utility.Decrypt(str));
        }
    }
}
