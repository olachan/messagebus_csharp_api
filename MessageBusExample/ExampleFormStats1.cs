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
    public partial class ExampleFormStats1 : Form
    {
        public ExampleFormStats1()
        {
            InitializeComponent();
        }

        private void btnGetStats_Click(object sender, EventArgs e)
        {
            getStats();
        }

        private void getStats()
        {

            // int.TryParse(tbFromDate.Text)

            var mb = MessageBusFactory.CreateStatsClient(tbApiKey.Text);
            var startDate = DateTime.Today.AddDays(int.Parse(tbFromDate.Text));
            var endDate = DateTime.Today.AddDays(int.Parse(tbToDate.Text));
            string tag = tbTag.Text;
            //MessageBusStatsResult[] stats = null;
            MessageBusStatsResult[] list;
            tbResults.Text = "";
            try
            {   
                
                list = mb.RetrieveStats(startDate, endDate, tag);
                string resultLine = String.Empty;
                foreach (var result in list)
                {
                    resultLine += result.Date + " " + result.Sent + "\r\n";

                }
                tbResults.Text = resultLine;
            }
            catch (MessageBusException we)
            {

            }

           

            return;
        }

    }
}
