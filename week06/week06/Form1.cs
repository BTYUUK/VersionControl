using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using week06.MnbServiceReference;
using week06.Entities;
using System.Windows.Forms.DataVisualization.Charting;

namespace week06
{
    public partial class Form1 : Form
    {
        
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        string Results;

        public Form1()
        {
            InitializeComponent();
            combo(); 
            RefreshData();
        }
        public void atvaltas()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            Results = result;
        }
        public void xml()
        {
            
            var xml = new XmlDocument();
            xml.LoadXml(Results);
            foreach (XmlElement element in xml.DocumentElement)
            {
                
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));
                
                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null)
                    continue;
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
                
            }
            
        }
        public void chartolas()
        {
            chart1.DataSource = Rates;
            var series = chart1.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chart1.Legends[0];
            legend.Enabled = false;

            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
        public void RefreshData ()
        {
            Rates.Clear();
            atvaltas();
            xml();
            chartolas();
            dataGridView1.DataSource = Rates;
        }

        public void combo()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            var response = mnbService.GetCurrencies(request);
            var result = response.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                string currency;
                int o = 0;
                int x = element.ChildNodes.Count;
                for (int i = 0; i < x; i++)
                {
                    var ch = element.ChildNodes[o];
                    currency = ch.InnerText;
                    Currencies.Add(currency);
                    o++;
                }
            }
            comboBox1.DataSource = Currencies;
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }

    
}
