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
                                Utility.ToInsecureString(Utility.DBPass),textBox1.Text,Utility.ToInsecureString(Utility.PharmacyName));
            await BackupDataBase(txt_ServerIP.Text, txt_DB.Text,
                                Utility.ToInsecureString(Utility.DBPass), textBox1.Text,Utility.ToInsecureString(Utility.PharmacyName));
        }
        private async Task BackupDataBase(string IP, string DB, string pass, string pathToBackup,string pharmacyName)
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
                    var comm = string.Format(@" use master; OPEN MASTER KEY DECRYPTION BY PASSWORD = '{0}';BACKUP DATABASE {2} TO DISK=N'{1}\{3}.bak';",pass,pathToBackup,DB,pharmacyName);
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
        private async Task BackupCertificate(string IP,string DB,string pass,string pathToBackup,string pharmacyName)
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
 OPEN MASTER KEY DECRYPTION BY PASSWORD = '{2}';                                                    
BACKUP CERTIFICATE MyServerCert TO FILE = N'{0}\{1}.cer'
                                                    WITH PRIVATE KEY
                                                    (FILE = N'{0}\{1}.pvk',ENCRYPTION BY PASSWORD = N'AReallyStr0ngK#y4You')",
                                                     pathToBackup,pharmacyName,pass);
                    SqlCommand command = new SqlCommand(comm, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("certificate با موفقیت کپی شد");
                    comm = string.Format(@"BACKUP MASTER KEY TO FILE = N'{0}\{2}.key'  ENCRYPTION BY PASSWORD = '{1}';",
                        pathToBackup,pass,pharmacyName);
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

                SqlCommand command;



                 command = new SqlCommand(string.Format(@"USE master;
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
                              FOR ATTACH;", masterKeyPath, certificatePath, privateKeyPath, DbPath, ldfPath,pass);
                }
                else
                {
                    textCommad = string.Format(@"use master;
                              RESTORE MASTER KEY   
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

FROM DISK = '{3}' WITH REPLACE", masterKeyPath, certificatePath, privateKeyPath, DbPath,pass);
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
                   // var data = Tn.DataPartition.Split('@');

                    return Tn.DataPartition;
                }
                else
                {
                    return null;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Bitmap image = new Bitmap(@"C:\Users\Administrator\Downloads\d_helix-css-gif-_50fps-selective_-1a.gif");
            //image.MakeTransparent();
            //pictureBox1.Image = image;

            radioButton2.Checked = true;
            SecTab.Enabled = false;
            panel3.Enabled=  button8.Enabled= panel1.Enabled= false;
            label23.Text= label22.Text= label2.Text =label7.Text= label3.Text =label10.Text= label11.Text=label12.Text= label9.Text= label20.Text= label21.Text="";
           // pictureBox1.Location = new Point(0, 0);
            

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
                    label21.Text = "کانفیگ کلاینت ساخته شد";

                    label19.Text = "\u2714";


                }
            }catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private async void Button6_Click(object sender, EventArgs e)
        {
          
            
            try
            {
                var dataOfTiny = (await ReadFromTiny()).Split('@');
                Utility.PharmacyName =Utility.ToSecureString(dataOfTiny[0]);
                var result = dataOfTiny[1];
               // label22.Text = dataOfTiny[0];
              

                if (result == null || result == "error")
                {
                    panel1.Enabled = false;
                    MessageBox.Show("خطا در خواندن");
                    TinyCode.BackColor = Color.Red;
                    SecTab.Enabled = false;
                    button6.BackColor = Color.Red;
                }
                else
                {
                    panel1.Enabled = true;
                    Utility.DBPass = Utility.ToSecureString(result.Trim());
                    Utility.passPhrase = Utility.ToSecureString(result.Trim());
                    SecTab.Enabled = true;
                    panel3.Enabled = false;
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
            txt_DB.Items.Clear();
            txt_DB.Text = "";
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
                    await CheckForDbEncrypted(txt_ServerIP.Text, txt_DB.Text,Utility.ToInsecureString(Utility.DBPass));
                    
                    //abel10.BackColor = Color.
                    //button8.BackColor = Color.Lime;
                    panel3.Enabled = false;
                    //label10.ForeColor = Color.Lime;
                    //SecTab.Enabled = true;
                    label10.Text = "\u2714" ;
                }
            }
        }




        private async Task CheckForDbEncrypted(string IP, string DB, string pass)
        {

            var constr = Utility.MakeConnectionStr(IP, DB, pass);
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand command;
            command = new SqlCommand(string.Format(@"SELECT
    db.name,
    db.is_encrypted,
    dm.encryption_state,
    dm.percent_complete,
    dm.key_algorithm,
    dm.key_length
FROM
    sys.databases db
    LEFT OUTER JOIN sys.dm_database_encryption_keys dm
        ON db.database_id = dm.database_id WHERE name = '{0}'; ", DB), conn);
            await conn.OpenAsync();
            var dbreader = await command.ExecuteReaderAsync();
            dbreader.Read();
            var test =(bool) dbreader[1];
            conn.Close();
            if (test == true)
            {
                var res = MessageBox.Show("این دیتابیس قبلا رمز نگاری شده است");
                conn.Close();
                return;
            }
            




            command = new SqlCommand("use master;select COUNT(*) from sys.certificates where name='myservercert'", conn);
            await conn.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            reader.Read();
            if ((int)reader[0] > 0)
            {
                conn.Close();
                var res = MessageBox.Show("certificate در این سیستم وجود دارد.آیا مایلید پاک شود؟", "", MessageBoxButtons.YesNo);
                if (res != DialogResult.Yes)
                    return;
                else
                {
                    command = new SqlCommand("use master;drop certificate myservercert;drop master key;", conn);
                    await conn.OpenAsync();
                    command.ExecuteNonQuery();
                    MessageBox.Show("با موفقیت حذف شد");
                    conn.Close();
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
                                Utility.ToInsecureString(Utility.DBPass),textBox5.Text,Utility.ToInsecureString(Utility.PharmacyName));
            await BackupDataBase(txt_ServerIP.Text, txt_DB.Text,
                               Utility.ToInsecureString(Utility.DBPass), textBox5.Text, Utility.ToInsecureString(Utility.PharmacyName));
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
            openFileDialog1.Filter = "bak files(*.bak)|*.bak";
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
            
               
                   
                   //// button8.BackColor = Color.Green;
            await ChangePassAndRenameSa(txt_ServerIP.Text, txt_DB.Text, Utility.ToInsecureString(Utility.DBPass));
           
                
               
          

        }
        private async Task ChangePassAndRenameSa(string address, string db, string pass)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(address, db, pass, "sa"));
                conn.Open();
                SqlCommand comm = new SqlCommand(

                  string.Format(@"USE MASTER
                                  ALTER LOGIN sa WITH NAME = BastaniTeb,
                                  PASSWORD = '{0}'; ", Utility.ToInsecureString(Utility.DBPass)), conn);


                comm.ExecuteNonQuery();
                textBox4.BackColor = Color.Lime;
                label11.Text = "\u2714";

                conn.Close();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

        }
        public async Task DisableAllUserButSa(string address,string db,string pass)
        {
            try
            {
                string strCommand =string.Format(@"SELECT 'use master;Deny connect to ' + QUOTENAME(sp.name) 
                                  FROM sys.server_principals sp
                                  WHERE sp.principal_id > 100   
                                  AND sp.is_disabled = 0
                                  AND sp.type IN ('G','s','u') and name<> '##MS_PolicyTsqlExecutionLogin##' ;", db);
             
                SqlConnection conn = new SqlConnection(Utility.MakeConnectionStr(address, db, pass));
                
                SqlCommand command = new SqlCommand(strCommand, conn);
                await conn.OpenAsync();
                var reader=command.ExecuteReader();
               
                List<string> commands = new List<string>();
                while (reader.Read())
                {
                    commands.Add(reader[0].ToString());
                   
                }
                strCommand = string.Format(@"SELECT 'use master;ALTER LOGIN ' + QUOTENAME(sp.name) + ' DISABLE;'
FROM sys.server_principals sp
WHERE sp.principal_id > 100
    AND sp.is_disabled = 0
    AND sp.type IN (
        'U'
         
        , 'S' 
        );");
                command = new SqlCommand(strCommand, conn);
                reader.Close();
                reader = command.ExecuteReader();

               
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

                
                
                label23.Text = "\u2714";
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
           
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private async void comboBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                txt_DB.Text = "";
                txt_DB.Items.Clear();
                string constr = Utility.MakeConnectionStr(txt_ServerIP.Text, "Master", Utility.ToInsecureString(Utility.DBPass));
                SqlConnection conn = new SqlConnection(constr);
                SqlCommand command = new SqlCommand("SELECT name FROM master.sys.databases", conn);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txt_DB.Items.Add(reader[0]);
                }
                conn.Close();
                button8.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطایی رخ داده.لطفا آدرس را چک نمایید");
                button8.Enabled = false;
            }
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            var text = "G8DsphpGhaw06it2kUrnr3B3Nz5kXSIne7+n+jLQfchr7QqtUlUMoR2TWV6Wqe8LNtskUEo70wcK+oV0/HeFp9ij70VkGjxW4MSphNLcBec56WSvV0py06uSP/2rscTL2iGp5J4xb4aCOdaMRkicDJz4EY7aIdHsMDKb8wErRJvcME/uEFbc6xiaEyWbboWPm8JagNyisZaoKJWMWCtT557HnxZZHM6llqXSXuz/LiCzUchd85ynxUdhUJzeKPT/DGWdOCLlP8DAbfpbEeL0ad5XcdHyRjZus1/DLHljkBM=";
            var dec=  Utility.Decrypt(text);

        }

        private async void button10_Click_3(object sender, EventArgs e)
        {
            await DisableAllUserButSa(txt_ServerIP.Text, txt_DB.Text, Utility.ToInsecureString(Utility.DBPass));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
        }
        private async Task PutMaintenancePlan()
        {
            var textCommand =string.Format(@"USE msdb ;  
EXEC dbo.sp_add_job  
    @job_name = N'Weekly Sinad Data Backup' ;  
EXEC sp_add_jobstep  
    @job_name = N'Weekly Sinad Data Backup',  
    @step_name = N'Set database to read only',  
    @subsystem = N'TSQL',  
    @command = N'BACKUP DATABASE Sinad TO DISK=''d:\\ertest.bak''',   
    @retry_attempts = 5,  
    @retry_interval = 5 ;  
EXEC dbo.sp_add_schedule  
    @schedule_name = N'RunWeekly',  
    @freq_type = 8,  
    @active_start_time = 171500 ;  
USE msdb ;  
EXEC sp_attach_schedule  
   @job_name = N'Weekly Sinad Data Backup',  
   @schedule_name = N'RunWeekly';  
EXEC dbo.sp_add_jobserver  
    @job_name = N'Weekly Sinad Data Backup';  
  
EXEC dbo.sp_add_job  
    @job_name = N'Daily Sinad Data Backup' ;  
  
EXEC sp_add_jobstep  
    @job_name = N'Daily Sinad Data Backup',  
    @step_name = N'Set database to read only',  
    @subsystem = N'TSQL',  
    @command = N'BACKUP DATABASE Sinad  TO DISK=''d:\\ertest.bak'' WITH DIFFERENTIAL',   
    @retry_attempts = 5,  
    @retry_interval = 5 ;  
  
EXEC dbo.sp_add_schedule  
    @schedule_name = N'RunDaily',  
    @freq_type = 4,  
    @active_start_time = 171500 ;  
USE msdb ;  
  
EXEC sp_attach_schedule  
   @job_name = N'Dail Sinad Data Backup',  
   @schedule_name = N'RunDaily';  
  
EXEC dbo.sp_add_jobserver  
    @job_name = N'Daily Sinad Data Backup';  
    ");
            SqlCommand command = new SqlCommand();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()!=DialogResult.Cancel )
            {
                textBox11.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.Cancel)
            {
                textBox10.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
