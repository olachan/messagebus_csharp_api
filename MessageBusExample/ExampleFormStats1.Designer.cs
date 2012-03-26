namespace MessageBusExample
{
    partial class ExampleFormStats1
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
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblToEmail = new System.Windows.Forms.Label();
            this.tbTag = new System.Windows.Forms.TextBox();
            this.tbToDate = new System.Windows.Forms.TextBox();
            this.lblFromEmail = new System.Windows.Forms.Label();
            this.tbFromDate = new System.Windows.Forms.TextBox();
            this.btnGetStats = new System.Windows.Forms.Button();
            this.gbStats = new System.Windows.Forms.GroupBox();
            this.tbResults = new System.Windows.Forms.TextBox();
            this.gbMessage.SuspendLayout();
            this.gbStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMessage
            // 
            this.gbMessage.Controls.Add(this.lblApiKey);
            this.gbMessage.Controls.Add(this.tbApiKey);
            this.gbMessage.Controls.Add(this.lblSubject);
            this.gbMessage.Controls.Add(this.lblToEmail);
            this.gbMessage.Controls.Add(this.tbTag);
            this.gbMessage.Controls.Add(this.tbToDate);
            this.gbMessage.Controls.Add(this.lblFromEmail);
            this.gbMessage.Controls.Add(this.tbFromDate);
            this.gbMessage.Location = new System.Drawing.Point(12, 12);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(322, 155);
            this.gbMessage.TabIndex = 3;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Message";
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(5, 22);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(48, 13);
            this.lblApiKey.TabIndex = 11;
            this.lblApiKey.Text = "API Key:";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Location = new System.Drawing.Point(72, 19);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(221, 20);
            this.tbApiKey.TabIndex = 10;
            this.tbApiKey.Text = "4BF302C40EB396495CA37581EC277722";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(5, 100);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(29, 13);
            this.lblSubject.TabIndex = 9;
            this.lblSubject.Text = "Tag:";
            // 
            // lblToEmail
            // 
            this.lblToEmail.AutoSize = true;
            this.lblToEmail.Location = new System.Drawing.Point(5, 74);
            this.lblToEmail.Name = "lblToEmail";
            this.lblToEmail.Size = new System.Drawing.Size(49, 13);
            this.lblToEmail.TabIndex = 8;
            this.lblToEmail.Text = "To Date:";
            // 
            // tbTag
            // 
            this.tbTag.Location = new System.Drawing.Point(72, 97);
            this.tbTag.Name = "tbTag";
            this.tbTag.Size = new System.Drawing.Size(221, 20);
            this.tbTag.TabIndex = 3;
            // 
            // tbToDate
            // 
            this.tbToDate.Location = new System.Drawing.Point(72, 71);
            this.tbToDate.Name = "tbToDate";
            this.tbToDate.Size = new System.Drawing.Size(221, 20);
            this.tbToDate.TabIndex = 2;
            this.tbToDate.Text = "0";
            // 
            // lblFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(5, 48);
            this.lblFromEmail.Name = "lblFromEmail";
            this.lblFromEmail.Size = new System.Drawing.Size(59, 13);
            this.lblFromEmail.TabIndex = 1;
            this.lblFromEmail.Text = "From Date:";
            // 
            // tbFromDate
            // 
            this.tbFromDate.Location = new System.Drawing.Point(72, 45);
            this.tbFromDate.Name = "tbFromDate";
            this.tbFromDate.Size = new System.Drawing.Size(221, 20);
            this.tbFromDate.TabIndex = 0;
            this.tbFromDate.Text = "-30";
            // 
            // btnGetStats
            // 
            this.btnGetStats.Location = new System.Drawing.Point(173, 432);
            this.btnGetStats.Name = "btnGetStats";
            this.btnGetStats.Size = new System.Drawing.Size(167, 45);
            this.btnGetStats.TabIndex = 4;
            this.btnGetStats.Text = "Get Stats";
            this.btnGetStats.UseVisualStyleBackColor = true;
            this.btnGetStats.Click += new System.EventHandler(this.btnGetStats_Click);
            // 
            // gbStats
            // 
            this.gbStats.Controls.Add(this.tbResults);
            this.gbStats.Location = new System.Drawing.Point(13, 180);
            this.gbStats.Name = "gbStats";
            this.gbStats.Size = new System.Drawing.Size(320, 241);
            this.gbStats.TabIndex = 5;
            this.gbStats.TabStop = false;
            this.gbStats.Text = "Stats";
            // 
            // tbResults
            // 
            this.tbResults.Location = new System.Drawing.Point(7, 20);
            this.tbResults.Multiline = true;
            this.tbResults.Name = "tbResults";
            this.tbResults.Size = new System.Drawing.Size(307, 215);
            this.tbResults.TabIndex = 0;
            // 
            // ExampleFormStats1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 489);
            this.Controls.Add(this.gbStats);
            this.Controls.Add(this.btnGetStats);
            this.Controls.Add(this.gbMessage);
            this.Name = "ExampleFormStats1";
            this.Text = "ExampleFormStats1";
            this.gbMessage.ResumeLayout(false);
            this.gbMessage.PerformLayout();
            this.gbStats.ResumeLayout(false);
            this.gbStats.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMessage;
        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox tbApiKey;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblToEmail;
        private System.Windows.Forms.TextBox tbTag;
        private System.Windows.Forms.TextBox tbToDate;
        private System.Windows.Forms.Label lblFromEmail;
        private System.Windows.Forms.TextBox tbFromDate;
        private System.Windows.Forms.Button btnGetStats;
        private System.Windows.Forms.GroupBox gbStats;
        private System.Windows.Forms.TextBox tbResults;
    }
}