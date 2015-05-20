using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Linq;
namespace stu_position
{
    public partial class Form2 : Form
    {
       private DBcomunicator com;
       private List<CurWifi> list;
       private string x;
       private string y;
       private Form1 f;
       string posname;

      public Form2(string pos,Form1 f,DBcomunicator com,List<CurWifi> list,string x,string y)
       {
           this.posname = pos;
           this.f = f;
            this.com = com;
            this.list = list;
            this.x = x;
            this.y = y;
            InitializeComponent();
        }
      
        private void Form2_Load(object sender, EventArgs e)
      {
          dataGridView1.AutoGenerateColumns = true;
           
           
          label1.Text += x;
          label2.Text += y;
         
          textBox1.Text = posname;

          dataGridView1.DataSource = list;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            position pos = new position() { pname = textBox1.Text, pixelxy = "x:" + x + "y:" + y };
            foreach(CurWifi wif in list)
            {
                wifisignal sign = new wifisignal { signal = wif.Signal };
                
                sign.wifirouter = com.Select<wifirouter>(z => z.mac == wif.BSSID).First();
              
                pos.wifisignals.Add(sign);

            }
            com.Add<position>(pos);
             f.SetPosInPicture();

             this.Hide();
        }










    }
}
