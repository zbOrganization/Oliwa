using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
namespace stu_position
{
    public partial class Form5 : Form
    {
        DBcomunicator com;
        Form1 f;
        List<Series> seriesl=new List<Series>();
        public Form5(Form1 form,DBcomunicator com)
        {
            InitializeComponent();
            this.com = com;
            this.f = form;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetValues(typeof(SeriesChartType));

          List<wifirouter> wifilist= com.Select<wifirouter>();
          List<position> positions = com.Select<position>();
          foreach (position pos in positions)
              listBox1.Items.Add(pos.pname);
          IEnumerable<CurWifi> l = f.getWifis().Where(x => wifilist.Any(y => y.mac.Equals(x.BSSID)));
          foreach (CurWifi rout in l)
              listBox2.Items.Add(rout.SSID+" "+rout.BSSID);
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int j = 0;
           foreach(var i in listBox2.SelectedItems){
               try
               {
                   CurWifi w = f.getWifis().Where(x => i.ToString().Contains(x.BSSID)).ElementAt(0);
                    seriesl[j++].Points.Add(w.Signal);

                    richTextBox1.Text += "mac:" + w.BSSID + " signal:" + w.Signal+Environment.NewLine;
               }
               catch
               {
                   richTextBox1.Text += i.ToString()+ " signal lost"+ Environment.NewLine;
                   j++;

               }

       
                 
             


           }

           richTextBox1.Text += "-----------------------------------------------------------" + Environment.NewLine;





        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            seriesl.Clear();
            richTextBox1.Text = "";
            this.chart1.Series.Clear();





            foreach (var i in listBox2.SelectedItems)
            {
                Series series = this.chart1.Series.Add(i.ToString().Insert(i.ToString().Length-12,Environment.NewLine));

                series.ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBox1.SelectedItem.ToString());
                seriesl.Add(series);
                
            }
            richTextBox1.Text += "მონაცემების ჩაწერა ოთახი :" + listBox1.SelectedItem.ToString() + Environment.NewLine;
            timer1.Interval = int.Parse(textBox1.Text)*1000;

            timer1.Start();
            listBox1.Enabled = false;
            listBox2.Enabled = false;
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            listBox1.Enabled = false;
            listBox2.Enabled = false;

            button1.Enabled = true;
            File.WriteAllLines("logs.txt", richTextBox1.Lines);
            using (Bitmap bitmap = new Bitmap(chart1.ClientSize.Width,
                                  chart1.ClientSize.Height))
            {
                chart1.DrawToBitmap(bitmap, chart1.ClientRectangle);
                bitmap.Save("chart.png", ImageFormat.Png);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string temp = "1234tttttttttttt";
            Series series = this.chart1.Series.Add (temp.Insert(temp.Length-12,Environment.NewLine));
            Series series2 = this.chart1.Series.Add("22222222");
            series.ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBox1.SelectedItem.ToString());
            series2.ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBox1.SelectedItem.ToString());
          
            for (int i = 0; i <= 300;i++ )
            {

                series.Points.Add(i*i+i);
                series2.Points.Add(Math.Sqrt(i));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
    }
}
