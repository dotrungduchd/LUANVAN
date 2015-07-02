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
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btAuthenticate = new System.Windows.Forms.Button();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btExtensions = new System.Windows.Forms.Button();
            this.pnlBtm = new System.Windows.Forms.Panel();
            this.lbEventLog = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btCustom = new System.Windows.Forms.Button();
            this.pnlTopLeft.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            this.pnlBtm.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.BackColor = System.Drawing.Color.Transparent;
            this.pnlTopLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlTopLeft.Controls.Add(this.label1);
            this.pnlTopLeft.Controls.Add(this.btAuthenticate);
            this.pnlTopLeft.Location = new System.Drawing.Point(14, 13);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(250, 100);
            this.pnlTopLeft.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "You have to authenticate to use";
            // 
            // btAuthenticate
            // 
            this.btAuthenticate.BackColor = System.Drawing.Color.Cyan;
            this.btAuthenticate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btAuthenticate.FlatAppearance.BorderSize = 0;
            this.btAuthenticate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btAuthenticate.Location = new System.Drawing.Point(17, 52);
            this.btAuthenticate.Name = "btAuthenticate";
            this.btAuthenticate.Size = new System.Drawing.Size(75, 23);
            this.btAuthenticate.TabIndex = 0;
            this.btAuthenticate.Text = "Authenticate";
            this.btAuthenticate.UseVisualStyleBackColor = false;
            this.btAuthenticate.Click += new System.EventHandler(this.btAuthenticate_Click);
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlTopRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlTopRight.Controls.Add(this.label2);
            this.pnlTopRight.Controls.Add(this.btExtensions);
            this.pnlTopRight.Location = new System.Drawing.Point(270, 13);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(250, 100);
            this.pnlTopRight.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.OrangeRed;
            this.label2.Location = new System.Drawing.Point(5, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "List all file extension supported";
            // 
            // btExtensions
            // 
            this.btExtensions.BackColor = System.Drawing.Color.DarkOrange;
            this.btExtensions.Location = new System.Drawing.Point(152, 52);
            this.btExtensions.Name = "btExtensions";
            this.btExtensions.Size = new System.Drawing.Size(75, 23);
            this.btExtensions.TabIndex = 0;
            this.btExtensions.Text = "Extensions";
            this.btExtensions.UseVisualStyleBackColor = false;
            this.btExtensions.Click += new System.EventHandler(this.btExtensions_Click);
            // 
            // pnlBtm
            // 
            this.pnlBtm.BackColor = System.Drawing.Color.Transparent;
            this.pnlBtm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBtm.Controls.Add(this.lbEventLog);
            this.pnlBtm.Controls.Add(this.label5);
            this.pnlBtm.Font = new System.Drawing.Font("Snap ITC", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBtm.Location = new System.Drawing.Point(14, 179);
            this.pnlBtm.Name = "pnlBtm";
            this.pnlBtm.Size = new System.Drawing.Size(506, 175);
            this.pnlBtm.TabIndex = 0;
            // 
            // lbEventLog
            // 
            this.lbEventLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEventLog.FormattingEnabled = true;
            this.lbEventLog.ItemHeight = 16;
            this.lbEventLog.Location = new System.Drawing.Point(14, 26);
            this.lbEventLog.Name = "lbEventLog";
            this.lbEventLog.Size = new System.Drawing.Size(480, 132);
            this.lbEventLog.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Snap ITC", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(12, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 22);
            this.label5.TabIndex = 0;
            this.label5.Text = "Event Log";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btCustom);
            this.panel1.Location = new System.Drawing.Point(14, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 43);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(343, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Custom application";
            // 
            // btCustom
            // 
            this.btCustom.BackColor = System.Drawing.Color.Lime;
            this.btCustom.Location = new System.Drawing.Point(17, 7);
            this.btCustom.Name = "btCustom";
            this.btCustom.Size = new System.Drawing.Size(75, 23);
            this.btCustom.TabIndex = 0;
            this.btCustom.Text = "Custom";
            this.btCustom.UseVisualStyleBackColor = false;
            // 
            // HackingAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::DemoHackingApp.Properties.Resources.windows_7_Blue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(534, 361);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTopRight);
            this.Controls.Add(this.pnlBtm);
            this.Controls.Add(this.pnlTopLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HackingAppForm";
            this.Text = "Hacking App";
            this.Load += new System.EventHandler(this.HackingAppForm_Load);
            this.Resize += new System.EventHandler(this.HackingAppForm_Resize);
            this.pnlTopLeft.ResumeLayout(false);
            this.pnlTopLeft.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopRight.PerformLayout();
            this.pnlBtm.ResumeLayout(false);
            this.pnlBtm.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopLeft;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.Panel pnlBtm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lbEventLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btAuthenticate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btExtensions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btCustom;




    }
}

