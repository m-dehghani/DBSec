namespace DBSec
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.SecTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button9 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.Tn = new AxTINYLib.AxTiny();
            this.axTinyPlusCtrl1 = new AxTINYLib.AxTinyPlusCtrl();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_ServerIP = new System.Windows.Forms.TextBox();
            this.txt_Pass = new System.Windows.Forms.TextBox();
            this.txt_DB = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button6 = new System.Windows.Forms.Button();
            this.TinyCode = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.SecTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTinyPlusCtrl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(297, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "BackUp Certificate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // SecTab
            // 
            this.SecTab.Controls.Add(this.tabPage1);
            this.SecTab.Controls.Add(this.tabPage2);
            this.SecTab.Controls.Add(this.tabPage3);
            this.SecTab.Controls.Add(this.tabPage5);
            this.SecTab.Location = new System.Drawing.Point(23, 245);
            this.SecTab.Name = "SecTab";
            this.SecTab.SelectedIndex = 0;
            this.SecTab.Size = new System.Drawing.Size(869, 351);
            this.SecTab.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.textBox5);
            this.tabPage1.Controls.Add(this.button11);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(861, 322);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Encrypt Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(324, 189);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(205, 101);
            this.button7.TabIndex = 0;
            this.button7.Text = "Encrypt DB";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(861, 322);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Backup Certificate";
            this.tabPage2.Click += new System.EventHandler(this.TabPage2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(46, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(727, 400);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(294, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(294, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(278, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "یک مسیر را برای کپی انتخاب نمایید.";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 83);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(578, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "D:\\SqlBackup";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(590, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 27);
            this.button2.TabIndex = 0;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button9);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(861, 322);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "RestoreCertificate";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(307, 195);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(135, 51);
            this.button9.TabIndex = 2;
            this.button9.Text = "Restore";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(143, 92);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(418, 22);
            this.textBox2.TabIndex = 1;
            this.textBox2.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(567, 87);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 32);
            this.button3.TabIndex = 0;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click_1);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.button5);
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.textBox3);
            this.tabPage5.Controls.Add(this.button4);
            this.tabPage5.Controls.Add(this.Tn);
            this.tabPage5.Controls.Add(this.axTinyPlusCtrl1);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(861, 322);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "EncryptConfig";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(523, 315);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 19;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.Maroon;
            this.button5.Location = new System.Drawing.Point(332, 190);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(143, 67);
            this.button5.TabIndex = 10;
            this.button5.Text = "Encrypt";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "فایل کانفیگ را انتخاب نمایید";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(132, 110);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(365, 22);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = "G:\\secureDBPrj\\app.config";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(503, 105);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(114, 32);
            this.button4.TabIndex = 7;
            this.button4.Text = "Browse";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // Tn
            // 
            this.Tn.Enabled = true;
            this.Tn.Location = new System.Drawing.Point(12, 221);
            this.Tn.Name = "Tn";
            this.Tn.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Tn.OcxState")));
            this.Tn.Size = new System.Drawing.Size(117, 50);
            this.Tn.TabIndex = 18;
            this.Tn.Visible = false;
            // 
            // axTinyPlusCtrl1
            // 
            this.axTinyPlusCtrl1.Enabled = true;
            this.axTinyPlusCtrl1.Location = new System.Drawing.Point(12, 358);
            this.axTinyPlusCtrl1.Name = "axTinyPlusCtrl1";
            this.axTinyPlusCtrl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTinyPlusCtrl1.OcxState")));
            this.axTinyPlusCtrl1.Size = new System.Drawing.Size(240, 240);
            this.axTinyPlusCtrl1.TabIndex = 17;
            this.axTinyPlusCtrl1.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Host Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(99, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Pass";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(111, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "DB";
            // 
            // txt_ServerIP
            // 
            this.txt_ServerIP.Location = new System.Drawing.Point(176, 19);
            this.txt_ServerIP.Name = "txt_ServerIP";
            this.txt_ServerIP.Size = new System.Drawing.Size(153, 22);
            this.txt_ServerIP.TabIndex = 13;
            this.txt_ServerIP.Text = "192.168.0.156\\sql2008";
            this.txt_ServerIP.TextChanged += new System.EventHandler(this.Txt_ServerIP_TextChanged);
            // 
            // txt_Pass
            // 
            this.txt_Pass.Location = new System.Drawing.Point(176, 97);
            this.txt_Pass.Name = "txt_Pass";
            this.txt_Pass.PasswordChar = '*';
            this.txt_Pass.ReadOnly = true;
            this.txt_Pass.Size = new System.Drawing.Size(153, 22);
            this.txt_Pass.TabIndex = 12;
            // 
            // txt_DB
            // 
            this.txt_DB.Location = new System.Drawing.Point(176, 56);
            this.txt_DB.Name = "txt_DB";
            this.txt_DB.Size = new System.Drawing.Size(153, 22);
            this.txt_DB.TabIndex = 11;
            this.txt_DB.Text = "Sinad";
            this.txt_DB.TextChanged += new System.EventHandler(this.Txt_DB_TextChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(25, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(157, 45);
            this.button6.TabIndex = 2;
            this.button6.Text = "Read The Tiny";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // TinyCode
            // 
            this.TinyCode.Location = new System.Drawing.Point(188, 23);
            this.TinyCode.Name = "TinyCode";
            this.TinyCode.PasswordChar = '&';
            this.TinyCode.Size = new System.Drawing.Size(360, 22);
            this.TinyCode.TabIndex = 3;
            this.TinyCode.Text = "BF18884B6F96B379185D4C96E673649";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button10);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txt_Pass);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_DB);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txt_ServerIP);
            this.panel1.Location = new System.Drawing.Point(27, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(861, 135);
            this.panel1.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label11.Location = new System.Drawing.Point(786, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 17);
            this.label11.TabIndex = 20;
            this.label11.Text = "label11";
            this.label11.Click += new System.EventHandler(this.Label11_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(660, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "Current Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(345, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 17);
            this.label10.TabIndex = 18;
            this.label10.Text = "label10";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Window;
            this.textBox4.Location = new System.Drawing.Point(660, 92);
            this.textBox4.Name = "textBox4";
            this.textBox4.PasswordChar = '*';
            this.textBox4.Size = new System.Drawing.Size(173, 22);
            this.textBox4.TabIndex = 4;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(348, 70);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(132, 49);
            this.button8.TabIndex = 17;
            this.button8.Text = "Test Connection";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(550, 72);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(104, 47);
            this.button10.TabIndex = 1;
            this.button10.Text = "Change Sa Password";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.TinyCode);
            this.panel2.Location = new System.Drawing.Point(27, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(861, 81);
            this.panel2.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label12.Location = new System.Drawing.Point(387, 160);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 17);
            this.label12.TabIndex = 19;
            this.label12.Text = "label12";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(338, 69);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(171, 17);
            this.label13.TabIndex = 22;
            this.label13.Text = "یک مسیر را برای کپی انتخاب نمایید.";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(66, 121);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(578, 22);
            this.textBox5.TabIndex = 21;
            this.textBox5.Text = "D:\\SqlBackup";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(650, 119);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(114, 27);
            this.button11.TabIndex = 20;
            this.button11.Text = "Browse";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 632);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SecTab);
            this.Name = "Form1";
            this.Text = "DBsec";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SecTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTinyPlusCtrl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl SecTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txt_DB;
        private System.Windows.Forms.TextBox txt_Pass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_ServerIP;
        private AxTINYLib.AxTiny Tn;
        private AxTINYLib.AxTinyPlusCtrl axTinyPlusCtrl1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TinyCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button11;
    }
}

