namespace DemoHackingApp
{
    partial class HackingAppForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HackingAppForm));
            this.btAuthenticate = new System.Windows.Forms.Button();
            this.btFileExtension = new System.Windows.Forms.Button();
            this.btAbout = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.pnAuthenticate = new System.Windows.Forms.Panel();
            this.lbAuthMode = new System.Windows.Forms.Label();
            this.pnDomain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btDomainApply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnPersonal = new System.Windows.Forms.Panel();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.lbHello = new System.Windows.Forms.Label();
            this.btPersonalLogin = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRememberMe = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbDomain = new System.Windows.Forms.RadioButton();
            this.rbPersonal = new System.Windows.Forms.RadioButton();
            this.pnExtension = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.cbPdf = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cbZip = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cbRar = new System.Windows.Forms.CheckBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.cbPptx = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbPpt = new System.Windows.Forms.CheckBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cbXlsx = new System.Windows.Forms.CheckBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.cbDocx = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbXls = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbDoc = new System.Windows.Forms.CheckBox();
            this.pnAuthenticate.SuspendLayout();
            this.pnDomain.SuspendLayout();
            this.pnPersonal.SuspendLayout();
            this.pnLogin.SuspendLayout();
            this.pnExtension.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btAuthenticate
            // 
            this.btAuthenticate.BackColor = System.Drawing.Color.Transparent;
            this.btAuthenticate.BackgroundImage = global::DemoHackingApp.Properties.Resources.key;
            this.btAuthenticate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAuthenticate.Location = new System.Drawing.Point(7, 8);
            this.btAuthenticate.Name = "btAuthenticate";
            this.btAuthenticate.Size = new System.Drawing.Size(100, 100);
            this.btAuthenticate.TabIndex = 0;
            this.btAuthenticate.UseVisualStyleBackColor = false;
            this.btAuthenticate.Click += new System.EventHandler(this.btAuthenticate_Click);
            // 
            // btFileExtension
            // 
            this.btFileExtension.BackColor = System.Drawing.Color.Transparent;
            this.btFileExtension.BackgroundImage = global::DemoHackingApp.Properties.Resources.extension;
            this.btFileExtension.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btFileExtension.Location = new System.Drawing.Point(7, 114);
            this.btFileExtension.Name = "btFileExtension";
            this.btFileExtension.Size = new System.Drawing.Size(100, 100);
            this.btFileExtension.TabIndex = 1;
            this.btFileExtension.UseVisualStyleBackColor = false;
            this.btFileExtension.Click += new System.EventHandler(this.btFileExtension_Click);
            // 
            // btAbout
            // 
            this.btAbout.BackColor = System.Drawing.Color.Transparent;
            this.btAbout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btAbout.BackgroundImage")));
            this.btAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAbout.Location = new System.Drawing.Point(7, 220);
            this.btAbout.Name = "btAbout";
            this.btAbout.Size = new System.Drawing.Size(100, 100);
            this.btAbout.TabIndex = 2;
            this.btAbout.UseVisualStyleBackColor = false;
            this.btAbout.Click += new System.EventHandler(this.btAbout_Click);
            // 
            // btExit
            // 
            this.btExit.BackColor = System.Drawing.Color.Transparent;
            this.btExit.BackgroundImage = global::DemoHackingApp.Properties.Resources.exit1;
            this.btExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btExit.Location = new System.Drawing.Point(7, 326);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(100, 100);
            this.btExit.TabIndex = 3;
            this.btExit.UseVisualStyleBackColor = false;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // pnAuthenticate
            // 
            this.pnAuthenticate.BackColor = System.Drawing.Color.Transparent;
            this.pnAuthenticate.Controls.Add(this.pnDomain);
            this.pnAuthenticate.Controls.Add(this.lbAuthMode);
            this.pnAuthenticate.Controls.Add(this.pnPersonal);
            this.pnAuthenticate.Controls.Add(this.rbDomain);
            this.pnAuthenticate.Controls.Add(this.rbPersonal);
            this.pnAuthenticate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnAuthenticate.Location = new System.Drawing.Point(113, 8);
            this.pnAuthenticate.Name = "pnAuthenticate";
            this.pnAuthenticate.Size = new System.Drawing.Size(444, 417);
            this.pnAuthenticate.TabIndex = 4;
            // 
            // lbAuthMode
            // 
            this.lbAuthMode.AutoSize = true;
            this.lbAuthMode.Location = new System.Drawing.Point(42, 11);
            this.lbAuthMode.Name = "lbAuthMode";
            this.lbAuthMode.Size = new System.Drawing.Size(44, 13);
            this.lbAuthMode.TabIndex = 4;
            this.lbAuthMode.Text = "You are";
            this.lbAuthMode.Click += new System.EventHandler(this.lbAuthMode_Click);
            // 
            // pnDomain
            // 
            this.pnDomain.BackColor = System.Drawing.Color.Transparent;
            this.pnDomain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnDomain.Controls.Add(this.panel2);
            this.pnDomain.Controls.Add(this.btDomainApply);
            this.pnDomain.Controls.Add(this.label3);
            this.pnDomain.Location = new System.Drawing.Point(13, 62);
            this.pnDomain.Name = "pnDomain";
            this.pnDomain.Size = new System.Drawing.Size(419, 164);
            this.pnDomain.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::DemoHackingApp.Properties.Resources.domai;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(273, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(133, 136);
            this.panel2.TabIndex = 7;
            // 
            // btDomainApply
            // 
            this.btDomainApply.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btDomainApply.Location = new System.Drawing.Point(136, 110);
            this.btDomainApply.Name = "btDomainApply";
            this.btDomainApply.Size = new System.Drawing.Size(122, 40);
            this.btDomainApply.TabIndex = 4;
            this.btDomainApply.Text = "Apply";
            this.btDomainApply.UseVisualStyleBackColor = true;
            this.btDomainApply.Click += new System.EventHandler(this.btDomainApply_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(6, 21);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(187, 103);
            this.label3.TabIndex = 2;
            this.label3.Text = "If you want people in your domain can access your file, please click Apply button" +
    "";
            // 
            // pnPersonal
            // 
            this.pnPersonal.BackColor = System.Drawing.Color.Transparent;
            this.pnPersonal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnPersonal.Controls.Add(this.pnLogin);
            this.pnPersonal.Controls.Add(this.btPersonalLogin);
            this.pnPersonal.Controls.Add(this.panel1);
            this.pnPersonal.Controls.Add(this.cbRememberMe);
            this.pnPersonal.Controls.Add(this.tbPassword);
            this.pnPersonal.Controls.Add(this.tbID);
            this.pnPersonal.Controls.Add(this.label2);
            this.pnPersonal.Controls.Add(this.label1);
            this.pnPersonal.Location = new System.Drawing.Point(13, 62);
            this.pnPersonal.Name = "pnPersonal";
            this.pnPersonal.Size = new System.Drawing.Size(419, 164);
            this.pnPersonal.TabIndex = 2;
            // 
            // pnLogin
            // 
            this.pnLogin.Controls.Add(this.lbHello);
            this.pnLogin.Location = new System.Drawing.Point(3, 11);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(269, 97);
            this.pnLogin.TabIndex = 4;
            // 
            // lbHello
            // 
            this.lbHello.AutoSize = true;
            this.lbHello.Font = new System.Drawing.Font("Showcard Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHello.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbHello.Location = new System.Drawing.Point(24, 27);
            this.lbHello.Name = "lbHello";
            this.lbHello.Size = new System.Drawing.Size(49, 17);
            this.lbHello.TabIndex = 0;
            this.lbHello.Text = "Hello";
            // 
            // btPersonalLogin
            // 
            this.btPersonalLogin.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btPersonalLogin.Location = new System.Drawing.Point(99, 119);
            this.btPersonalLogin.Name = "btPersonalLogin";
            this.btPersonalLogin.Size = new System.Drawing.Size(94, 28);
            this.btPersonalLogin.TabIndex = 7;
            this.btPersonalLogin.Text = "Login";
            this.btPersonalLogin.UseVisualStyleBackColor = true;
            this.btPersonalLogin.Click += new System.EventHandler(this.btPersonalLogin_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::DemoHackingApp.Properties.Resources.personalAthen;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(273, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(133, 136);
            this.panel1.TabIndex = 6;
            // 
            // cbRememberMe
            // 
            this.cbRememberMe.AutoSize = true;
            this.cbRememberMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRememberMe.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cbRememberMe.Location = new System.Drawing.Point(99, 96);
            this.cbRememberMe.Name = "cbRememberMe";
            this.cbRememberMe.Size = new System.Drawing.Size(94, 17);
            this.cbRememberMe.TabIndex = 5;
            this.cbRememberMe.Text = "Remember me";
            this.cbRememberMe.UseVisualStyleBackColor = true;
            this.cbRememberMe.CheckedChanged += new System.EventHandler(this.cbRememberMe_CheckedChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbPassword.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbPassword.Location = new System.Drawing.Point(99, 61);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(159, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbID.Location = new System.Drawing.Point(99, 21);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(159, 20);
            this.tbID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(9, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // rbDomain
            // 
            this.rbDomain.AutoSize = true;
            this.rbDomain.Location = new System.Drawing.Point(267, 39);
            this.rbDomain.Name = "rbDomain";
            this.rbDomain.Size = new System.Drawing.Size(132, 17);
            this.rbDomain.TabIndex = 1;
            this.rbDomain.TabStop = true;
            this.rbDomain.Text = "Domain Authentication";
            this.rbDomain.UseVisualStyleBackColor = true;
            this.rbDomain.CheckedChanged += new System.EventHandler(this.rbDomain_CheckedChanged);
            // 
            // rbPersonal
            // 
            this.rbPersonal.AutoSize = true;
            this.rbPersonal.Location = new System.Drawing.Point(45, 39);
            this.rbPersonal.Name = "rbPersonal";
            this.rbPersonal.Size = new System.Drawing.Size(137, 17);
            this.rbPersonal.TabIndex = 0;
            this.rbPersonal.TabStop = true;
            this.rbPersonal.Text = "Personal Authentication";
            this.rbPersonal.UseVisualStyleBackColor = true;
            this.rbPersonal.CheckedChanged += new System.EventHandler(this.rbPersonal_CheckedChanged);
            // 
            // pnExtension
            // 
            this.pnExtension.BackColor = System.Drawing.Color.Transparent;
            this.pnExtension.Controls.Add(this.label4);
            this.pnExtension.Controls.Add(this.panel11);
            this.pnExtension.Controls.Add(this.panel7);
            this.pnExtension.Controls.Add(this.panel6);
            this.pnExtension.Controls.Add(this.panel8);
            this.pnExtension.Controls.Add(this.panel5);
            this.pnExtension.Controls.Add(this.panel9);
            this.pnExtension.Controls.Add(this.panel10);
            this.pnExtension.Controls.Add(this.panel4);
            this.pnExtension.Controls.Add(this.panel3);
            this.pnExtension.Location = new System.Drawing.Point(113, 8);
            this.pnExtension.Name = "pnExtension";
            this.pnExtension.Size = new System.Drawing.Size(444, 417);
            this.pnExtension.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(133, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 63);
            this.label4.TabIndex = 15;
            this.label4.Text = "Choose the file extension that you want to protect";
            // 
            // panel11
            // 
            this.panel11.BackgroundImage = global::DemoHackingApp.Properties.Resources.pdf;
            this.panel11.Controls.Add(this.cbPdf);
            this.panel11.Location = new System.Drawing.Point(15, 283);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(99, 122);
            this.panel11.TabIndex = 11;
            // 
            // cbPdf
            // 
            this.cbPdf.AutoSize = true;
            this.cbPdf.BackColor = System.Drawing.Color.Transparent;
            this.cbPdf.Location = new System.Drawing.Point(71, 96);
            this.cbPdf.Name = "cbPdf";
            this.cbPdf.Size = new System.Drawing.Size(15, 14);
            this.cbPdf.TabIndex = 7;
            this.cbPdf.UseVisualStyleBackColor = false;
            this.cbPdf.CheckedChanged += new System.EventHandler(this.cbPdf_CheckedChanged);
            // 
            // panel7
            // 
            this.panel7.BackgroundImage = global::DemoHackingApp.Properties.Resources.zip;
            this.panel7.Controls.Add(this.cbZip);
            this.panel7.Location = new System.Drawing.Point(330, 148);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(99, 122);
            this.panel7.TabIndex = 11;
            // 
            // cbZip
            // 
            this.cbZip.AutoSize = true;
            this.cbZip.BackColor = System.Drawing.Color.Transparent;
            this.cbZip.Location = new System.Drawing.Point(71, 95);
            this.cbZip.Name = "cbZip";
            this.cbZip.Size = new System.Drawing.Size(15, 14);
            this.cbZip.TabIndex = 6;
            this.cbZip.UseVisualStyleBackColor = false;
            this.cbZip.CheckedChanged += new System.EventHandler(this.cbZip_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.BackgroundImage = global::DemoHackingApp.Properties.Resources.rar;
            this.panel6.Controls.Add(this.cbRar);
            this.panel6.Location = new System.Drawing.Point(330, 14);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(99, 122);
            this.panel6.TabIndex = 9;
            // 
            // cbRar
            // 
            this.cbRar.AutoSize = true;
            this.cbRar.BackColor = System.Drawing.Color.Transparent;
            this.cbRar.Location = new System.Drawing.Point(70, 95);
            this.cbRar.Name = "cbRar";
            this.cbRar.Size = new System.Drawing.Size(15, 14);
            this.cbRar.TabIndex = 4;
            this.cbRar.UseVisualStyleBackColor = false;
            this.cbRar.CheckedChanged += new System.EventHandler(this.cbRar_CheckedChanged);
            // 
            // panel8
            // 
            this.panel8.BackgroundImage = global::DemoHackingApp.Properties.Resources.pptx;
            this.panel8.Controls.Add(this.cbPptx);
            this.panel8.Location = new System.Drawing.Point(225, 148);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(99, 122);
            this.panel8.TabIndex = 12;
            // 
            // cbPptx
            // 
            this.cbPptx.AutoSize = true;
            this.cbPptx.BackColor = System.Drawing.Color.Transparent;
            this.cbPptx.Location = new System.Drawing.Point(71, 96);
            this.cbPptx.Name = "cbPptx";
            this.cbPptx.Size = new System.Drawing.Size(15, 14);
            this.cbPptx.TabIndex = 5;
            this.cbPptx.UseVisualStyleBackColor = false;
            this.cbPptx.CheckedChanged += new System.EventHandler(this.cbPptx_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = global::DemoHackingApp.Properties.Resources.ppt;
            this.panel5.Controls.Add(this.cbPpt);
            this.panel5.Location = new System.Drawing.Point(225, 14);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(99, 122);
            this.panel5.TabIndex = 9;
            // 
            // cbPpt
            // 
            this.cbPpt.AutoSize = true;
            this.cbPpt.BackColor = System.Drawing.Color.Transparent;
            this.cbPpt.Location = new System.Drawing.Point(71, 96);
            this.cbPpt.Name = "cbPpt";
            this.cbPpt.Size = new System.Drawing.Size(15, 14);
            this.cbPpt.TabIndex = 2;
            this.cbPpt.UseVisualStyleBackColor = false;
            this.cbPpt.CheckedChanged += new System.EventHandler(this.cbPpt_CheckedChanged);
            // 
            // panel9
            // 
            this.panel9.BackgroundImage = global::DemoHackingApp.Properties.Resources.xlsx;
            this.panel9.Controls.Add(this.cbXlsx);
            this.panel9.Location = new System.Drawing.Point(120, 148);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(99, 122);
            this.panel9.TabIndex = 13;
            // 
            // cbXlsx
            // 
            this.cbXlsx.AutoSize = true;
            this.cbXlsx.BackColor = System.Drawing.Color.Transparent;
            this.cbXlsx.Location = new System.Drawing.Point(72, 95);
            this.cbXlsx.Name = "cbXlsx";
            this.cbXlsx.Size = new System.Drawing.Size(15, 14);
            this.cbXlsx.TabIndex = 4;
            this.cbXlsx.UseVisualStyleBackColor = false;
            this.cbXlsx.CheckedChanged += new System.EventHandler(this.cbXlsx_CheckedChanged);
            // 
            // panel10
            // 
            this.panel10.BackgroundImage = global::DemoHackingApp.Properties.Resources.docx;
            this.panel10.Controls.Add(this.cbDocx);
            this.panel10.Location = new System.Drawing.Point(15, 148);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(99, 122);
            this.panel10.TabIndex = 10;
            // 
            // cbDocx
            // 
            this.cbDocx.AutoSize = true;
            this.cbDocx.BackColor = System.Drawing.Color.Transparent;
            this.cbDocx.Location = new System.Drawing.Point(73, 95);
            this.cbDocx.Name = "cbDocx";
            this.cbDocx.Size = new System.Drawing.Size(15, 14);
            this.cbDocx.TabIndex = 3;
            this.cbDocx.UseVisualStyleBackColor = false;
            this.cbDocx.CheckedChanged += new System.EventHandler(this.cbDocx_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::DemoHackingApp.Properties.Resources.xls;
            this.panel4.Controls.Add(this.cbXls);
            this.panel4.Location = new System.Drawing.Point(120, 14);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(99, 122);
            this.panel4.TabIndex = 9;
            // 
            // cbXls
            // 
            this.cbXls.AutoSize = true;
            this.cbXls.BackColor = System.Drawing.Color.Transparent;
            this.cbXls.Location = new System.Drawing.Point(73, 96);
            this.cbXls.Name = "cbXls";
            this.cbXls.Size = new System.Drawing.Size(15, 14);
            this.cbXls.TabIndex = 1;
            this.cbXls.UseVisualStyleBackColor = false;
            this.cbXls.CheckedChanged += new System.EventHandler(this.cbXls_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::DemoHackingApp.Properties.Resources.doc1;
            this.panel3.Controls.Add(this.cbDoc);
            this.panel3.Location = new System.Drawing.Point(15, 14);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(99, 122);
            this.panel3.TabIndex = 8;
            // 
            // cbDoc
            // 
            this.cbDoc.AutoSize = true;
            this.cbDoc.BackColor = System.Drawing.Color.Transparent;
            this.cbDoc.Location = new System.Drawing.Point(72, 96);
            this.cbDoc.Name = "cbDoc";
            this.cbDoc.Size = new System.Drawing.Size(15, 14);
            this.cbDoc.TabIndex = 0;
            this.cbDoc.UseVisualStyleBackColor = false;
            this.cbDoc.CheckedChanged += new System.EventHandler(this.cbDoc_CheckedChanged);
            // 
            // HackingAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::DemoHackingApp.Properties.Resources.windows_7_Blue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(565, 431);
            this.Controls.Add(this.pnAuthenticate);
            this.Controls.Add(this.pnExtension);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btAbout);
            this.Controls.Add(this.btFileExtension);
            this.Controls.Add(this.btAuthenticate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HackingAppForm";
            this.Text = "Hacking App";
            this.Load += new System.EventHandler(this.HackingAppForm_Load);
            this.Resize += new System.EventHandler(this.HackingAppForm_Resize);
            this.pnAuthenticate.ResumeLayout(false);
            this.pnAuthenticate.PerformLayout();
            this.pnDomain.ResumeLayout(false);
            this.pnPersonal.ResumeLayout(false);
            this.pnPersonal.PerformLayout();
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            this.pnExtension.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btAuthenticate;
        private System.Windows.Forms.Button btFileExtension;
        private System.Windows.Forms.Button btAbout;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Panel pnAuthenticate;
        private System.Windows.Forms.Panel pnDomain;
        private System.Windows.Forms.Panel pnPersonal;
        private System.Windows.Forms.CheckBox cbRememberMe;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.RadioButton rbDomain;
        private System.Windows.Forms.RadioButton rbPersonal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btDomainApply;
        private System.Windows.Forms.Button btPersonalLogin;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnLogin;
        private System.Windows.Forms.Label lbHello;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbAuthMode;
        private System.Windows.Forms.Panel pnExtension;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox cbPdf;
        private System.Windows.Forms.CheckBox cbZip;
        private System.Windows.Forms.CheckBox cbRar;
        private System.Windows.Forms.CheckBox cbPptx;
        private System.Windows.Forms.CheckBox cbPpt;
        private System.Windows.Forms.CheckBox cbXlsx;
        private System.Windows.Forms.CheckBox cbDocx;
        private System.Windows.Forms.CheckBox cbXls;
        private System.Windows.Forms.CheckBox cbDoc;





    }
}

