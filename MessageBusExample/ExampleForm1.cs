using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MessageBus.API;
using MessageBus.API.V2;
using MessageBus.API.V2.Debug;
using System.Net;


namespace MessageBusExample
{
    public partial class ExampleForm1 : Form
    {
        private Button btnSendMessage;
        private GroupBox gbMessageStatus;
        private Label lblSuccessCount;
        private TextBox tbSuccessCount;
        private Label lblFailed;
        private TextBox tbFailedCount;
        private Label lblMID;
        private TextBox tbMessageID;
        private GroupBox gbMessage;
        private Label label1;
        private TextBox tbFromEmail;
        private TextBox tbToEmail;
        private TextBox tbSubject;
        private Label label3;
        private Label label2;
        private TextBox tbPlaintext;
        private TextBox tbHtmlBody;
        private Label label5;
        private Label lbApiKey;
        private TextBox tbApiKey;
        private Label label4;
    
        public ExampleForm1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.gbMessageStatus = new System.Windows.Forms.GroupBox();
            this.lblMID = new System.Windows.Forms.Label();
            this.tbMessageID = new System.Windows.Forms.TextBox();
            this.lblFailed = new System.Windows.Forms.Label();
            this.tbFailedCount = new System.Windows.Forms.TextBox();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.tbSuccessCount = new System.Windows.Forms.TextBox();
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPlaintext = new System.Windows.Forms.TextBox();
            this.tbHtmlBody = new System.Windows.Forms.TextBox();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbToEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFromEmail = new System.Windows.Forms.TextBox();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.lbApiKey = new System.Windows.Forms.Label();
            this.gbMessageStatus.SuspendLayout();
            this.gbMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(605, 405);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(167, 45);
            this.btnSendMessage.TabIndex = 0;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // gbMessageStatus
            // 
            this.gbMessageStatus.Controls.Add(this.lblMID);
            this.gbMessageStatus.Controls.Add(this.tbMessageID);
            this.gbMessageStatus.Controls.Add(this.lblFailed);
            this.gbMessageStatus.Controls.Add(this.tbFailedCount);
            this.gbMessageStatus.Controls.Add(this.lblSuccessCount);
            this.gbMessageStatus.Controls.Add(this.tbSuccessCount);
            this.gbMessageStatus.Location = new System.Drawing.Point(10, 313);
            this.gbMessageStatus.Name = "gbMessageStatus";
            this.gbMessageStatus.Size = new System.Drawing.Size(762, 86);
            this.gbMessageStatus.TabIndex = 1;
            this.gbMessageStatus.TabStop = false;
            this.gbMessageStatus.Text = "Message Status";
            // 
            // lblMID
            // 
            this.lblMID.AutoSize = true;
            this.lblMID.Location = new System.Drawing.Point(228, 23);
            this.lblMID.Name = "lblMID";
            this.lblMID.Size = new System.Drawing.Size(67, 13);
            this.lblMID.TabIndex = 5;
            this.lblMID.Text = "Message ID:";
            // 
            // tbMessageID
            // 
            this.tbMessageID.Location = new System.Drawing.Point(301, 19);
            this.tbMessageID.Name = "tbMessageID";
            this.tbMessageID.ReadOnly = true;
            this.tbMessageID.Size = new System.Drawing.Size(266, 20);
            this.tbMessageID.TabIndex = 4;
            this.tbMessageID.Text = "<Message-ID>";
            this.tbMessageID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFailed
            // 
            this.lblFailed.AutoSize = true;
            this.lblFailed.Location = new System.Drawing.Point(18, 49);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(38, 13);
            this.lblFailed.TabIndex = 3;
            this.lblFailed.Text = "Failed:";
            // 
            // tbFailedCount
            // 
            this.tbFailedCount.Location = new System.Drawing.Point(101, 46);
            this.tbFailedCount.Name = "tbFailedCount";
            this.tbFailedCount.ReadOnly = true;
            this.tbFailedCount.Size = new System.Drawing.Size(100, 20);
            this.tbFailedCount.TabIndex = 2;
            this.tbFailedCount.Text = "0";
            this.tbFailedCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSuccessCount
            // 
            this.lblSuccessCount.AutoSize = true;
            this.lblSuccessCount.Location = new System.Drawing.Point(18, 23);
            this.lblSuccessCount.Name = "lblSuccessCount";
            this.lblSuccessCount.Size = new System.Drawing.Size(65, 13);
            this.lblSuccessCount.TabIndex = 1;
            this.lblSuccessCount.Text = "Succeeded:";
            // 
            // tbSuccessCount
            // 
            this.tbSuccessCount.Location = new System.Drawing.Point(101, 20);
            this.tbSuccessCount.Name = "tbSuccessCount";
            this.tbSuccessCount.ReadOnly = true;
            this.tbSuccessCount.Size = new System.Drawing.Size(100, 20);
            this.tbSuccessCount.TabIndex = 0;
            this.tbSuccessCount.Text = "0";
            this.tbSuccessCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbMessage
            // 
            this.gbMessage.Controls.Add(this.lbApiKey);
            this.gbMessage.Controls.Add(this.tbApiKey);
            this.gbMessage.Controls.Add(this.label5);
            this.gbMessage.Controls.Add(this.label4);
            this.gbMessage.Controls.Add(this.label3);
            this.gbMessage.Controls.Add(this.label2);
            this.gbMessage.Controls.Add(this.tbPlaintext);
            this.gbMessage.Controls.Add(this.tbHtmlBody);
            this.gbMessage.Controls.Add(this.tbSubject);
            this.gbMessage.Controls.Add(this.tbToEmail);
            this.gbMessage.Controls.Add(this.label1);
            this.gbMessage.Controls.Add(this.tbFromEmail);
            this.gbMessage.Location = new System.Drawing.Point(10, 12);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(762, 295);
            this.gbMessage.TabIndex = 2;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Message";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Subject:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "To Email:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Plain Text Body:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "HTML Body:";
            // 
            // tbPlaintext
            // 
            this.tbPlaintext.Location = new System.Drawing.Point(372, 168);
            this.tbPlaintext.Multiline = true;
            this.tbPlaintext.Name = "tbPlaintext";
            this.tbPlaintext.Size = new System.Drawing.Size(377, 121);
            this.tbPlaintext.TabIndex = 5;
            this.tbPlaintext.Text = "This is a test message.";
            // 
            // tbHtmlBody
            // 
            this.tbHtmlBody.Location = new System.Drawing.Point(372, 18);
            this.tbHtmlBody.Multiline = true;
            this.tbHtmlBody.Name = "tbHtmlBody";
            this.tbHtmlBody.Size = new System.Drawing.Size(377, 144);
            this.tbHtmlBody.TabIndex = 4;
            this.tbHtmlBody.Text = "<html><body>Hi,<br/>This is a test message.</body></html>";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(72, 97);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(205, 20);
            this.tbSubject.TabIndex = 3;
            this.tbSubject.Text = "API test email";
            // 
            // tbToEmail
            // 
            this.tbToEmail.Location = new System.Drawing.Point(72, 71);
            this.tbToEmail.Name = "tbToEmail";
            this.tbToEmail.Size = new System.Drawing.Size(205, 20);
            this.tbToEmail.TabIndex = 2;
            this.tbToEmail.Text = "apitest1@messagebus.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From Email:";
            // 
            // tbFromEmail
            // 
            this.tbFromEmail.Location = new System.Drawing.Point(72, 45);
            this.tbFromEmail.Name = "tbFromEmail";
            this.tbFromEmail.Size = new System.Drawing.Size(205, 20);
            this.tbFromEmail.TabIndex = 0;
            this.tbFromEmail.Text = "test@messagebus.com";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Location = new System.Drawing.Point(72, 19);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(205, 20);
            this.tbApiKey.TabIndex = 10;
            this.tbApiKey.Text = "<Put API Key Here>";
            // 
            // lbApiKey
            // 
            this.lbApiKey.AutoSize = true;
            this.lbApiKey.Location = new System.Drawing.Point(5, 22);
            this.lbApiKey.Name = "lbApiKey";
            this.lbApiKey.Size = new System.Drawing.Size(48, 13);
            this.lbApiKey.TabIndex = 11;
            this.lbApiKey.Text = "API Key:";
            // 
            // ExampleForm1
            // 
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.gbMessage);
            this.Controls.Add(this.gbMessageStatus);
            this.Controls.Add(this.btnSendMessage);
            this.Name = "ExampleForm1";
            this.Text = "Messagebus Example Form";
            this.gbMessageStatus.ResumeLayout(false);
            this.gbMessageStatus.PerformLayout();
            this.gbMessage.ResumeLayout(false);
            this.gbMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Button to send test message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            sendMessage();
        }

        /// <summary>
        /// Send message via API
        /// </summary>
        private void sendMessage()
        {

            var mb = MessageBus.API.MessageBus.CreateClient(tbApiKey.Text);
            mb.Transmitted += new MessageTransmissionHandler(mb_Transmitted);

            var debug = mb as IMessageBusDebugging;
            debug.Domain = "https://apitest.messagebus.com";
            debug.SslVerifyPeer = false;
            debug.Credentials = new NetworkCredential("demo", "319MBPmiller");
            mb.FromEmail = tbFromEmail.Text;
            using (mb)
            {
                mb.Send(new MessageBusEmail
                {
                    ToEmail = tbToEmail.Text,
                    Subject = tbSubject.Text,
                    HtmlBody = tbHtmlBody.Text,
                    PlaintextBody = tbPlaintext.Text
                });
            }
            return;
        }

        /// <summary>
        /// Call back with transmission status for message
        /// </summary>
        /// <param name="transmissionEvent"></param>
        void mb_Transmitted(IMessageBusTransmissionEvent transmissionEvent)
        {

            tbFailedCount.Text = transmissionEvent.FailureCount.ToString();
            tbSuccessCount.Text = transmissionEvent.SuccessCount.ToString();

            if (transmissionEvent.Statuses.Count == 1)
            {
                tbMessageID.Text = transmissionEvent.Statuses[0].MessageId;
            }
        }

    }
}
