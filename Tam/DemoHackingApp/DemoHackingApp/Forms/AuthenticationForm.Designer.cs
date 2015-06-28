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
            this.rbPersonnal = new System.Windows.Forms.RadioButton();
            this.pnDomain = new System.Windows.Forms.Panel();
            this.rbDomain = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pnPersonal.SuspendLayout();
            this.pnDomain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnPersonal
            // 
            this.pnPersonal.BackColor = System.Drawing.Color.Transparent;
            this.pnPersonal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnPersonal.Controls.Add(this.checkBox1);
            this.pnPersonal.Controls.Add(this.button1);
            this.pnPersonal.Controls.Add(this.tbPassword);
            this.pnPersonal.Controls.Add(this.tbID);
            this.pnPersonal.Controls.Add(this.label2);
            this.pnPersonal.Controls.Add(this.label1);
            this.pnPersonal.Location = new System.Drawing.Point(12, 35);
            this.pnPersonal.Name = "pnPersonal";
            this.pnPersonal.Size = new System.Drawing.Size(250, 200);
            this.pnPersonal.TabIndex = 0;
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
            this.pnDomain.Controls.Add(this.label3);
            this.pnDomain.Controls.Add(this.radioButton2);
            this.pnDomain.Controls.Add(this.radioButton1);
            this.pnDomain.Location = new System.Drawing.Point(281, 35);
            this.pnDomain.Name = "pnDomain";
            this.pnDomain.Size = new System.Drawing.Size(250, 200);
            this.pnDomain.TabIndex = 1;
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
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tbID.Location = new System.Drawing.Point(126, 21);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(100, 20);
            this.tbID.TabIndex = 2;
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
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.Color.Fuchsia;
            this.radioButton1.Location = new System.Drawing.Point(60, 57);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(82, 20);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Only me";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.ForeColor = System.Drawing.Color.Fuchsia;
            this.radioButton2.Location = new System.Drawing.Point(60, 97);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(183, 20);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "All user in your domain";
            this.radioButton2.UseVisualStyleBackColor = true;
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
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(126, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 42);
            this.button1.TabIndex = 4;
            this.button1.Text = "Authenticate";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(126, 100);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(105, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Remember me";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DemoHackingApp.Properties.Resources.windows_7_Blue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(544, 251);
            this.Controls.Add(this.rbDomain);
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
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}