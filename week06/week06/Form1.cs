﻿using System;
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

namespace week06
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates;
        string Results;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            chart1.DataSource = Rates;
            atvaltas();
            xml();
            
            
        }
        public void atvaltas()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
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

        }
    }

    
}
