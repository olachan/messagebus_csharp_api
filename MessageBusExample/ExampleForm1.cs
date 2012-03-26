// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MessageBus.API;
using System.Net;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;


namespace MessageBusExample {
    public partial class ExampleForm1 : Form {
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
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private OpenFileDialog openFileDialog1;
        private Label label1;
        private Button button1;
        private TextBox tbMailingList;
        private GroupBox groupBox1;
        private Label label2;
        private TextBox tbMailingListError;
        private Label label3;
        private TextBox tbMailingListId;
        private Label label4;
        private TextBox tbMailingListFailed;
        private Label label5;
        private TextBox tbMailingListPassed;
        private Button button2;
        private Label label6;
        private TextBox tbCampaignId;
        private Button button3;
        private ProgressBar pbarMailingList;
        private Label lblToEmail;

        public ExampleForm1() {
            InitializeComponent();
        }

        private void InitializeComponent() {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMailingListError = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCampaignId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMailingListId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMailingListFailed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbMailingListPassed = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbMailingList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pbarMailingList = new System.Windows.Forms.ProgressBar();
            this.gbMessageStatus.SuspendLayout();
            this.gbMessage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(605, 156);
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
            this.gbMessageStatus.Location = new System.Drawing.Point(10, 64);
            this.gbMessageStatus.Name = "gbMessageStatus";
            this.gbMessageStatus.Size = new System.Drawing.Size(762, 86);
            this.gbMessageStatus.TabIndex = 1;
            this.gbMessageStatus.TabStop = false;
            this.gbMessageStatus.Text = "Message Status";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.Location = new System.Drawing.Point(228, 46);
            this.lblErrorMessage.Name = "lblErrorMessage";
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
            this.gbMessage.Location = new System.Drawing.Point(12, 12);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(784, 295);
            this.gbMessage.TabIndex = 2;
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
            this.tbApiKey.Text = "<Put API Key Here>";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(5, 100);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 9;
            this.lblSubject.Text = "Subject:";
            // 
            // lblToEmail
            // 
            this.lblToEmail.AutoSize = true;
            this.lblToEmail.Location = new System.Drawing.Point(5, 74);
            this.lblToEmail.Name = "lblToEmail";
            this.lblToEmail.Size = new System.Drawing.Size(51, 13);
            this.lblToEmail.TabIndex = 8;
            this.lblToEmail.Text = "To Email:";
            // 
            // lblPlaintextBody
            // 
            this.lblPlaintextBody.AutoSize = true;
            this.lblPlaintextBody.Location = new System.Drawing.Point(282, 171);
            this.lblPlaintextBody.Name = "lblPlaintextBody";
            this.lblPlaintextBody.Size = new System.Drawing.Size(77, 13);
            this.lblPlaintextBody.TabIndex = 7;
            this.lblPlaintextBody.Text = "Plaintext Body:";
            // 
            // lblHtmlBody
            // 
            this.lblHtmlBody.AutoSize = true;
            this.lblHtmlBody.Location = new System.Drawing.Point(299, 22);
            this.lblHtmlBody.Name = "lblHtmlBody";
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
            // lblFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(5, 48);
            this.lblFromEmail.Name = "lblFromEmail";
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 313);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(786, 233);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSendMessage);
            this.tabPage1.Controls.Add(this.gbMessageStatus);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(778, 207);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Send a Single Email via the API";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pbarMailingList);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tbMailingListError);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.tbMailingList);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(778, 207);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Upload a List and Send a Campaign via the API";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(614, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(158, 39);
            this.button3.TabIndex = 8;
            this.button3.Text = "Upload List";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Message:";
            // 
            // tbMailingListError
            // 
            this.tbMailingListError.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbMailingListError.Enabled = false;
            this.tbMailingListError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMailingListError.ForeColor = System.Drawing.Color.Green;
            this.tbMailingListError.Location = new System.Drawing.Point(70, 171);
            this.tbMailingListError.Name = "tbMailingListError";
            this.tbMailingListError.ReadOnly = true;
            this.tbMailingListError.Size = new System.Drawing.Size(473, 20);
            this.tbMailingListError.TabIndex = 6;
            this.tbMailingListError.Text = "Welcome! Upload a List and then Send A Campaign :-)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbCampaignId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbMailingListId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbMailingListFailed);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbMailingListPassed);
            this.groupBox1.Location = new System.Drawing.Point(10, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(762, 86);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(228, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Campaign ID:";
            // 
            // tbCampaignId
            // 
            this.tbCampaignId.Location = new System.Drawing.Point(301, 45);
            this.tbCampaignId.Name = "tbCampaignId";
            this.tbCampaignId.ReadOnly = true;
            this.tbCampaignId.Size = new System.Drawing.Size(232, 20);
            this.tbCampaignId.TabIndex = 6;
            this.tbCampaignId.Text = "<Campaign-ID>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mailing List ID:";
            // 
            // tbMailingListId
            // 
            this.tbMailingListId.Location = new System.Drawing.Point(301, 19);
            this.tbMailingListId.Name = "tbMailingListId";
            this.tbMailingListId.ReadOnly = true;
            this.tbMailingListId.Size = new System.Drawing.Size(232, 20);
            this.tbMailingListId.TabIndex = 4;
            this.tbMailingListId.Text = "<MailingList-ID>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Failed:";
            // 
            // tbMailingListFailed
            // 
            this.tbMailingListFailed.Location = new System.Drawing.Point(69, 46);
            this.tbMailingListFailed.Name = "tbMailingListFailed";
            this.tbMailingListFailed.ReadOnly = true;
            this.tbMailingListFailed.Size = new System.Drawing.Size(100, 20);
            this.tbMailingListFailed.TabIndex = 2;
            this.tbMailingListFailed.Text = "0";
            this.tbMailingListFailed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Passed:";
            // 
            // tbMailingListPassed
            // 
            this.tbMailingListPassed.Location = new System.Drawing.Point(69, 20);
            this.tbMailingListPassed.Name = "tbMailingListPassed";
            this.tbMailingListPassed.ReadOnly = true;
            this.tbMailingListPassed.Size = new System.Drawing.Size(100, 20);
            this.tbMailingListPassed.TabIndex = 0;
            this.tbMailingListPassed.Text = "0";
            this.tbMailingListPassed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(614, 162);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 39);
            this.button2.TabIndex = 3;
            this.button2.Text = "Send Campaign";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(513, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbMailingList
            // 
            this.tbMailingList.Location = new System.Drawing.Point(127, 20);
            this.tbMailingList.Name = "tbMailingList";
            this.tbMailingList.ReadOnly = true;
            this.tbMailingList.Size = new System.Drawing.Size(380, 20);
            this.tbMailingList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mailing List (*.csv) :";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            // 
            // pbarMailingList
            // 
            this.pbarMailingList.Location = new System.Drawing.Point(127, 47);
            this.pbarMailingList.Name = "pbarMailingList";
            this.pbarMailingList.Size = new System.Drawing.Size(380, 17);
            this.pbarMailingList.Step = 1;
            this.pbarMailingList.TabIndex = 9;
            // 
            // ExampleForm1
            // 
            this.ClientSize = new System.Drawing.Size(808, 558);
            this.Controls.Add(this.gbMessage);
            this.Controls.Add(this.tabControl1);
            this.Name = "ExampleForm1";
            this.Text = "Messagebus Example Form";
            this.gbMessageStatus.ResumeLayout(false);
            this.gbMessageStatus.PerformLayout();
            this.gbMessage.ResumeLayout(false);
            this.gbMessage.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Button to send test message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMessage_Click(object sender, EventArgs e) {
            sendMessage();
        }

        /// <summary>
        /// Send message via API
        /// </summary>
        private void sendMessage() {

            var mb = MessageBusFactory.CreateEmailClient(tbApiKey.Text);
            mb.Transmitted += mb_Transmitted;

            try {
                using (mb) {
                    mb.Send(new MessageBusEmail {
                        FromEmail = tbFromEmail.Text,
                        ToEmail = tbToEmail.Text,
                        Subject = tbSubject.Text,
                        HtmlBody = tbHtmlBody.Text,
                        PlaintextBody = tbPlaintext.Text
                    });
                }
            } catch (MessageBusException we) {
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
        void mb_Transmitted(IMessageBusTransmissionEvent transmissionEvent) {

            tbFailedCount.Text = transmissionEvent.FailureCount.ToString();
            tbSuccessCount.Text = transmissionEvent.SuccessCount.ToString();
            tbErrorMessage.Text = string.Empty;

            if (transmissionEvent.Statuses.Count == 1) {
                tbMessageID.Text = transmissionEvent.Statuses[0].MessageId;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                tbMailingList.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            var client = MessageBusFactory.CreateMailingListClient(tbApiKey.Text);
            tbMailingListError.ForeColor = Color.Orange;
            tbMailingListError.Text = "Working...";
            tbMailingListError.Refresh();

            client.UploadProgress += progress;
            try {
                var result = client.UploadMailingList("Example", new FileInfo(tbMailingList.Text));
                tbMailingListPassed.Text = result.ValidCount.ToString();
                tbMailingListFailed.Text = result.InvalidCount.ToString();
                tbMailingListId.Text = result.MailingListKey;
                tbMailingListError.ForeColor = Color.Green;
                tbMailingListError.Text = "Success!";
            } catch (Exception x) {
                tbMailingListError.ForeColor = Color.Red;
                tbMailingListError.Text = x.Message;
            }
        }

        void progress(long uploaded, long total) {
            pbarMailingList.Maximum = (int)total;
            pbarMailingList.Minimum = 0;
            pbarMailingList.Value = (int)uploaded;
            pbarMailingList.Refresh();
        }

        private void button2_Click(object sender, EventArgs e) {
            var client = MessageBusFactory.CreateCampaignClient(tbApiKey.Text);
            tbMailingListError.ForeColor = Color.Orange;
            tbMailingListError.Text = "Working...";
            tbMailingListError.Refresh();
            var request = new MessageBusCampaign() {
                CampaignName = "Example",
                FromEmail = tbFromEmail.Text,
                Subject = tbSubject.Text,
                HtmlBody = tbHtmlBody.Text,
                PlaintextBody = tbPlaintext.Text,
                MailingListKey = tbMailingListId.Text,
                Tags = new[] { "CSHARP_TEST" }
            };
            try {
                var result = client.SendCampaign(request);
                tbCampaignId.Text = result.CampaignKey;
                tbMailingListError.ForeColor = Color.Green;
                tbMailingListError.Text = "Success!";
            } catch (Exception x) {
                tbMailingListError.ForeColor = Color.Red;
                tbMailingListError.Text = x.Message;
            }
        }
    }
}
