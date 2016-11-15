/*
 * Created by SharpDevelop.
 * User: clayton.neal
 * Date: 24/07/2013
 * Time: 11:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace NRobot.Server
{
    partial class AboutForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ProjectHomeLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.ProjectHomeLink);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.VersionLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 146);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 120);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // ProjectHomeLink
            // 
            this.ProjectHomeLink.Location = new System.Drawing.Point(145, 56);
            this.ProjectHomeLink.Name = "ProjectHomeLink";
            this.ProjectHomeLink.Size = new System.Drawing.Size(135, 23);
            this.ProjectHomeLink.TabIndex = 3;
            this.ProjectHomeLink.TabStop = true;
            this.ProjectHomeLink.Text = "NRobot Server Website";
            this.ProjectHomeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ProjectHomeLinkLinkClicked);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(145, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = ".Net Robot Framework keyword server\r\n";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(251, 8);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(140, 23);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "VersionLabel";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(145, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "NRobot Server";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(185, 164);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 199);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About NRobot Server";
            this.Shown += new System.EventHandler(this.AboutFormShown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel ProjectHomeLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Panel panel1;
    }
}
