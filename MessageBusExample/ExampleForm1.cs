using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MessageBus.API;
using System.Net;
using MessageBus.API.V3;


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
        private Label lblFromEmail;
        private TextBox tbFromEmail;
        private TextBox tbToEmail;
        private TextBox tbSubject;
        private Label lblPlaintextBody;
        private Label lblHtmlBody;
        private TextBox tbPlaintext;
        private TextBox tbHtmlBody;
        private Label lblSubject;
        private Label lblApiKey;
        private TextBox tbApiKey;
        private Label lblErrorMessage;
        private TextBox tbErrorMessage;
        private Label lblToEmail;
    
        public ExampleForm1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.gbMessageStatus = new System.Windows.Forms.GroupBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.tbErrorMessage = new System.Windows.Forms.TextBox();
            this.lblMID = new System.Windows.Forms.Label();
            this.tbMessageID = new System.Windows.Forms.TextBox();
            this.lblFailed = new System.Windows.Forms.Label();
            this.tbFailedCount = new System.Windows.Forms.TextBox();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.tbSuccessCount = new System.Windows.Forms.TextBox();
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblToEmail = new System.Windows.Forms.Label();
            this.lblPlaintextBody = new System.Windows.Forms.Label();
            this.lblHtmlBody = new System.Windows.Forms.Label();
            this.tbPlaintext = new System.Windows.Forms.TextBox();
            this.tbHtmlBody = new System.Windows.Forms.TextBox();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbToEmail = new System.Windows.Forms.TextBox();
            this.lblFromEmail = new System.Windows.Forms.Label();
            this.tbFromEmail = new System.Windows.Forms.TextBox();
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
            this.gbMessageStatus.Controls.Add(this.lblErrorMessage);
            this.gbMessageStatus.Controls.Add(this.tbErrorMessage);
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
            // lbErrorMessage
            // 
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.Location = new System.Drawing.Point(228, 46);
            this.lblErrorMessage.Name = "lbErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(55, 13);
            this.lblErrorMessage.TabIndex = 7;
            this.lblErrorMessage.Text = "Error Msg:";
            // 
            // tbErrorMessage
            // 
            this.tbErrorMessage.Location = new System.Drawing.Point(301, 42);
            this.tbErrorMessage.Name = "tbErrorMessage";
            this.tbErrorMessage.ReadOnly = true;
            this.tbErrorMessage.Size = new System.Drawing.Size(448, 20);
            this.tbErrorMessage.TabIndex = 6;
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
            this.tbMessageID.Size = new System.Drawing.Size(232, 20);
            this.tbMessageID.TabIndex = 4;
            this.tbMessageID.Text = "<Message-ID>";
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
            this.gbMessage.Controls.Add(this.lblApiKey);
            this.gbMessage.Controls.Add(this.tbApiKey);
            this.gbMessage.Controls.Add(this.lblSubject);
            this.gbMessage.Controls.Add(this.lblToEmail);
            this.gbMessage.Controls.Add(this.lblPlaintextBody);
            this.gbMessage.Controls.Add(this.lblHtmlBody);
            this.gbMessage.Controls.Add(this.tbPlaintext);
            this.gbMessage.Controls.Add(this.tbHtmlBody);
            this.gbMessage.Controls.Add(this.tbSubject);
            this.gbMessage.Controls.Add(this.tbToEmail);
            this.gbMessage.Controls.Add(this.lblFromEmail);
            this.gbMessage.Controls.Add(this.tbFromEmail);
            this.gbMessage.Location = new System.Drawing.Point(10, 12);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(762, 295);
            this.gbMessage.TabIndex = 2;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Message";
            // 
            // lbApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(5, 22);
            this.lblApiKey.Name = "lbApiKey";
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
            this.tbApiKey.Text = "<Put API Key Here>";
            // 
            // lbSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(5, 100);
            this.lblSubject.Name = "lbSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 9;
            this.lblSubject.Text = "Subject:";
            // 
            // lbToEmail
            // 
            this.lblToEmail.AutoSize = true;
            this.lblToEmail.Location = new System.Drawing.Point(5, 74);
            this.lblToEmail.Name = "lbToEmail";
            this.lblToEmail.Size = new System.Drawing.Size(51, 13);
            this.lblToEmail.TabIndex = 8;
            this.lblToEmail.Text = "To Email:";
            // 
            // lbPlaintextBody
            // 
            this.lblPlaintextBody.AutoSize = true;
            this.lblPlaintextBody.Location = new System.Drawing.Point(282, 171);
            this.lblPlaintextBody.Name = "lbPlaintextBody";
            this.lblPlaintextBody.Size = new System.Drawing.Size(77, 13);
            this.lblPlaintextBody.TabIndex = 7;
            this.lblPlaintextBody.Text = "Plaintext Body:";
            // 
            // lbHtmlBody
            // 
            this.lblHtmlBody.AutoSize = true;
            this.lblHtmlBody.Location = new System.Drawing.Point(299, 22);
            this.lblHtmlBody.Name = "lbHtmlBody";
            this.lblHtmlBody.Size = new System.Drawing.Size(67, 13);
            this.lblHtmlBody.TabIndex = 6;
            this.lblHtmlBody.Text = "HTML Body:";
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
            this.tbSubject.Size = new System.Drawing.Size(221, 20);
            this.tbSubject.TabIndex = 3;
            this.tbSubject.Text = "API test email";
            // 
            // tbToEmail
            // 
            this.tbToEmail.Location = new System.Drawing.Point(72, 71);
            this.tbToEmail.Name = "tbToEmail";
            this.tbToEmail.Size = new System.Drawing.Size(221, 20);
            this.tbToEmail.TabIndex = 2;
            this.tbToEmail.Text = "apitest1@messagebus.com";
            // 
            // lbFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(5, 48);
            this.lblFromEmail.Name = "lbFromEmail";
            this.lblFromEmail.Size = new System.Drawing.Size(61, 13);
            this.lblFromEmail.TabIndex = 1;
            this.lblFromEmail.Text = "From Email:";
            // 
            // tbFromEmail
            // 
            this.tbFromEmail.Location = new System.Drawing.Point(72, 45);
            this.tbFromEmail.Name = "tbFromEmail";
            this.tbFromEmail.Size = new System.Drawing.Size(221, 20);
            this.tbFromEmail.TabIndex = 0;
            this.tbFromEmail.Text = "test@messagebus.com";
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

            var mb = MessageBus.API.MessageBus.CreateEmailClient(tbApiKey.Text);
            mb.Transmitted += mb_Transmitted;

            try
            {
                using (mb)
                {
                    mb.Send(new MessageBusEmail
                    {
                        FromEmail = tbFromEmail.Text,
                        ToEmail = tbToEmail.Text,
                        Subject = tbSubject.Text,
                        HtmlBody = tbHtmlBody.Text,
                        PlaintextBody = tbPlaintext.Text
                    });
                }
            }
            catch (MessageBusException we)
            {
                tbFailedCount.Text = "1";
                tbSuccessCount.Text = "0";
                tbMessageID.Text = "Error occurred";
                tbErrorMessage.Text = we.Message;
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
            tbErrorMessage.Text = string.Empty;

            if (transmissionEvent.Statuses.Count == 1)
            {
                tbMessageID.Text = transmissionEvent.Statuses[0].MessageId;
            }
        }


    }
}
