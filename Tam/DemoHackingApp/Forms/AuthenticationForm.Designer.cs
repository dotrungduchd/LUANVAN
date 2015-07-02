namespace DemoHackingApp
{
    partial class AuthenticationForm
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
            this.pnPersonal = new System.Windows.Forms.Panel();
            this.cbRememberMe = new System.Windows.Forms.CheckBox();
            this.btAuth = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbPersonnal = new System.Windows.Forms.RadioButton();
            this.pnDomain = new System.Windows.Forms.Panel();
            this.rbSomeOneDomain = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbAllUserDomain = new System.Windows.Forms.RadioButton();
            this.rbOnlyMe = new System.Windows.Forms.RadioButton();
            this.rbDomain = new System.Windows.Forms.RadioButton();
            this.pnPersonal.SuspendLayout();
            this.pnDomain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnPersonal
            // 
            this.pnPersonal.BackColor = System.Drawing.Color.Transparent;
            this.pnPersonal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnPersonal.Controls.Add(this.cbRememberMe);
            this.pnPersonal.Controls.Add(this.tbPassword);
            this.pnPersonal.Controls.Add(this.tbID);
            this.pnPersonal.Controls.Add(this.label2);
            this.pnPersonal.Controls.Add(this.label1);
            this.pnPersonal.Location = new System.Drawing.Point(12, 35);
            this.pnPersonal.Name = "pnPersonal";
            this.pnPersonal.Size = new System.Drawing.Size(250, 200);
            this.pnPersonal.TabIndex = 0;
            // 
            // cbRememberMe
            // 
            this.cbRememberMe.AutoSize = true;
            this.cbRememberMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRememberMe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cbRememberMe.Location = new System.Drawing.Point(126, 100);
            this.cbRememberMe.Name = "cbRememberMe";
            this.cbRememberMe.Size = new System.Drawing.Size(105, 17);
            this.cbRememberMe.TabIndex = 5;
            this.cbRememberMe.Text = "Remember me";
            this.cbRememberMe.UseVisualStyleBackColor = true;
            this.cbRememberMe.CheckedChanged += new System.EventHandler(this.cbRememberMe_CheckedChanged);
            // 
            // btAuth
            // 
            this.btAuth.BackColor = System.Drawing.Color.White;
            this.btAuth.Location = new System.Drawing.Point(224, 241);
            this.btAuth.Name = "btAuth";
            this.btAuth.Size = new System.Drawing.Size(100, 42);
            this.btAuth.TabIndex = 4;
            this.btAuth.Text = "Authenticate";
            this.btAuth.UseVisualStyleBackColor = false;
            this.btAuth.Click += new System.EventHandler(this.btAuth_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tbPassword.Location = new System.Drawing.Point(126, 61);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tbID.Location = new System.Drawing.Point(126, 21);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(100, 20);
            this.tbID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(25, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // rbPersonnal
            // 
            this.rbPersonnal.AutoSize = true;
            this.rbPersonnal.BackColor = System.Drawing.Color.Transparent;
            this.rbPersonnal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPersonnal.ForeColor = System.Drawing.Color.SpringGreen;
            this.rbPersonnal.Location = new System.Drawing.Point(41, 12);
            this.rbPersonnal.Name = "rbPersonnal";
            this.rbPersonnal.Size = new System.Drawing.Size(197, 20);
            this.rbPersonnal.TabIndex = 0;
            this.rbPersonnal.Text = "Personnal Authentication";
            this.rbPersonnal.UseVisualStyleBackColor = false;
            this.rbPersonnal.CheckedChanged += new System.EventHandler(this.rbPersonnal_CheckedChanged);
            // 
            // pnDomain
            // 
            this.pnDomain.BackColor = System.Drawing.Color.Transparent;
            this.pnDomain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnDomain.Controls.Add(this.rbSomeOneDomain);
            this.pnDomain.Controls.Add(this.label3);
            this.pnDomain.Controls.Add(this.rbAllUserDomain);
            this.pnDomain.Controls.Add(this.rbOnlyMe);
            this.pnDomain.Location = new System.Drawing.Point(281, 35);
            this.pnDomain.Name = "pnDomain";
            this.pnDomain.Size = new System.Drawing.Size(250, 200);
            this.pnDomain.TabIndex = 1;
            // 
            // rbSomeOneDomain
            // 
            this.rbSomeOneDomain.AutoSize = true;
            this.rbSomeOneDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSomeOneDomain.ForeColor = System.Drawing.Color.Fuchsia;
            this.rbSomeOneDomain.Location = new System.Drawing.Point(34, 109);
            this.rbSomeOneDomain.Name = "rbSomeOneDomain";
            this.rbSomeOneDomain.Size = new System.Drawing.Size(197, 20);
            this.rbSomeOneDomain.TabIndex = 3;
            this.rbSomeOneDomain.Text = "Someone in your domain";
            this.rbSomeOneDomain.UseVisualStyleBackColor = true;
            this.rbSomeOneDomain.CheckedChanged += new System.EventHandler(this.rbSomeOneDomain_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Magenta;
            this.label3.Location = new System.Drawing.Point(26, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Who can access your file??";
            // 
            // rbAllUserDomain
            // 
            this.rbAllUserDomain.AutoSize = true;
            this.rbAllUserDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAllUserDomain.ForeColor = System.Drawing.Color.Fuchsia;
            this.rbAllUserDomain.Location = new System.Drawing.Point(34, 83);
            this.rbAllUserDomain.Name = "rbAllUserDomain";
            this.rbAllUserDomain.Size = new System.Drawing.Size(183, 20);
            this.rbAllUserDomain.TabIndex = 1;
            this.rbAllUserDomain.Text = "All user in your domain";
            this.rbAllUserDomain.UseVisualStyleBackColor = true;
            this.rbAllUserDomain.CheckedChanged += new System.EventHandler(this.rbAllUserDomain_CheckedChanged);
            // 
            // rbOnlyMe
            // 
            this.rbOnlyMe.AutoSize = true;
            this.rbOnlyMe.Checked = true;
            this.rbOnlyMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbOnlyMe.ForeColor = System.Drawing.Color.Fuchsia;
            this.rbOnlyMe.Location = new System.Drawing.Point(34, 57);
            this.rbOnlyMe.Name = "rbOnlyMe";
            this.rbOnlyMe.Size = new System.Drawing.Size(82, 20);
            this.rbOnlyMe.TabIndex = 0;
            this.rbOnlyMe.TabStop = true;
            this.rbOnlyMe.Text = "Only me";
            this.rbOnlyMe.UseVisualStyleBackColor = true;
            this.rbOnlyMe.CheckedChanged += new System.EventHandler(this.rbOnlyMe_CheckedChanged);
            // 
            // rbDomain
            // 
            this.rbDomain.AutoSize = true;
            this.rbDomain.BackColor = System.Drawing.Color.Transparent;
            this.rbDomain.Checked = true;
            this.rbDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDomain.ForeColor = System.Drawing.Color.Purple;
            this.rbDomain.Location = new System.Drawing.Point(326, 12);
            this.rbDomain.Name = "rbDomain";
            this.rbDomain.Size = new System.Drawing.Size(180, 20);
            this.rbDomain.TabIndex = 0;
            this.rbDomain.TabStop = true;
            this.rbDomain.Text = "Domain Authentication";
            this.rbDomain.UseVisualStyleBackColor = false;
            this.rbDomain.CheckedChanged += new System.EventHandler(this.rbDomain_CheckedChanged);
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DemoHackingApp.Properties.Resources.windows_7_Blue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(544, 290);
            this.Controls.Add(this.rbDomain);
            this.Controls.Add(this.btAuth);
            this.Controls.Add(this.rbPersonnal);
            this.Controls.Add(this.pnDomain);
            this.Controls.Add(this.pnPersonal);
            this.Name = "AuthenticationForm";
            this.Text = "AuthenticationForm";
            this.Load += new System.EventHandler(this.AuthenticationForm_Load);
            this.pnPersonal.ResumeLayout(false);
            this.pnPersonal.PerformLayout();
            this.pnDomain.ResumeLayout(false);
            this.pnDomain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnPersonal;
        private System.Windows.Forms.RadioButton rbPersonnal;
        private System.Windows.Forms.Panel pnDomain;
        private System.Windows.Forms.RadioButton rbDomain;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbAllUserDomain;
        private System.Windows.Forms.RadioButton rbOnlyMe;
        private System.Windows.Forms.CheckBox cbRememberMe;
        private System.Windows.Forms.Button btAuth;
        private System.Windows.Forms.RadioButton rbSomeOneDomain;
    }
}