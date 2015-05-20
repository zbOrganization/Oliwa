using NativeWifi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
namespace stu_position
{
    public partial class Form1 : Form
    {
        PictureBox pb;

        WlanClient client = null;
        List<CurWifi> list = null;
        DBcomunicator com = null;
        public Form1()
        {


            client = new WlanClient();
            com = DBcomunicator.GetInstance();
            InitializeComponent();
            list = getWifis();

            for (int i = 5; i <= 55;i++ )
            {
             Controls["button" + i.ToString()].Parent = pictureBox1;

             pictureBox1.Controls["button" + i.ToString()].Location = new Point(pictureBox1.Controls["button" + i.ToString()].Location.X - pictureBox1.Location.X, pictureBox1.Controls["button" + i.ToString()].Location.Y - pictureBox1.Location.Y);
            }

 

                button5.BackColor = Color.Transparent;
            pb = new PictureBox();
            pb.Visible = false;
            pb.Size = pictureBox3.Size;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Image = pictureBox3.Image;
            pb.BackColor = Color.Transparent;
            pictureBox1.Controls.Add(pb);
            pb.BringToFront();

        }

        private void roomclick(object sender, EventArgs e)
        {
          
            if (list != null && list.Count > 0)
            {

                

                List<wifirouter> routers = com.Select<wifirouter>();
               
               List<CurWifi> l = list.Where(x =>routers.Any(y => y.mac.Equals(x.BSSID))).ToList<CurWifi>();

               
              
                
                    new Form2(((Button)sender).Tag.ToString(), this, com, l, (((Button)sender).Location.X + 5).ToString(), (((Button)sender).Location.Y + 5).ToString()).Show();
                 
                
            }








        }
        public void MyPos()
        {
            try
            {
                List<wifirouter> routers = com.Select<wifirouter>();
                IEnumerable<CurWifi> l = list.Where(x => routers.Any(y => y.mac.Equals(x.BSSID)));





                List<position> mypos = com.Select<position>(x => x.wifisignals.All(y => l.Any(z => z.BSSID.Equals(y.wifirouter.mac) && z.Signal >= y.signal - int.Parse(textBox1.Text) && z.Signal <= y.signal + int.Parse(textBox1.Text))));

                if (mypos.Count != 0)
                {
                    if (mypos.Count > 1) MessageBox.Show("ამ სიგნალს შეესაბამება ბევრი პოზიცია");
                    string cors = mypos[0].pixelxy;

                    int xp = int.Parse(cors.Substring(2, cors.IndexOf("y") - 2));
                    int yp = int.Parse(cors.Substring(cors.LastIndexOf(":") + 1, cors.Length - 1 - cors.LastIndexOf(":")));





                    new ToolTip().SetToolTip(pb, mypos[0].pname);

                    pb.Location = new System.Drawing.Point(xp, yp);

                    pb.Visible = true;
                    pb.BringToFront();
                    richTextBox2.AppendText("პოზიცია" + mypos[0].pname + Environment.NewLine);
                    label3.Text = "ჩემი ადგილმდებარეობა სიტყვიერად:" + mypos[0].pname;

                    return;

                }
                else
                {
                    label3.Text = "";

                    pb.Visible = false;
                }

            }
            catch (Exception e) { MessageBox.Show(e.ToString()+"  my pos error"); }
         
            #region old
            /*
            DataTable res = com.GetPositions.executecommand("select * from positions").Tables[0];
            foreach (DataRow row in res.Rows)
            {

                List<CurWifi> l = JsonConvert.DeserializeObject<List<CurWifi>>(row["wifisignals"].ToString());

                int count = 0;
                foreach (CurWifi wifihe in list)
                {
                    foreach (CurWifi wifidb in l)
                    {

                        if (wifidb.BSSID.Equals(wifihe.BSSID) && wifihe.Signal >= wifidb.Signal - int.Parse(textBox1.Text) && wifihe.Signal <= wifidb.Signal + int.Parse(textBox1.Text))
                        {
                            count++;


                        }
                    }

                }
                if (count == l.Count)
                {

                    string cors = row["pixelsxy"].ToString();

                    int x = int.Parse(cors.Substring(2, cors.IndexOf("y") - 2));
                    int y = int.Parse(cors.Substring(cors.LastIndexOf(":") + 1, cors.Length - 1 - cors.LastIndexOf(":")));





                    new ToolTip().SetToolTip(pb, row["pname"].ToString() + Environment.NewLine + row["wifisignals"]);

                    pb.Location = new System.Drawing.Point(x, y);

                    pb.Visible = true;

                    richTextBox2.AppendText("პოზიცია" + row["pname"].ToString() + Environment.NewLine);
                    label3.Text = "ჩემი ადგილმდებარეობა სიტყვიერად:" + row["pname"];
                    
                    return;
                }
                else
                {
                    label3.Text = "";

                    pb.Visible = false;
                }



            }
            */
            #endregion




        }
        private void timer1_Tick(object sender, EventArgs e)
        {
           
            try
            {
                richTextBox1.Text = "";
                list = getWifis();
                foreach (CurWifi wifi in list)
                    richTextBox1.AppendText("MAC:" + wifi.BSSID + " NAME:" + wifi.SSID + " SIGNAL:" + wifi.Signal.ToString() + Environment.NewLine);

           MyPos();





            }
            catch (Exception ex)
            {

                MessageBox.Show("timer error:" + ex.ToString());
            }


        }

        public List<CurWifi> getWifis()
        {
            List<CurWifi> wifis = null;
            try
            {
                wifis = new List<CurWifi>();



                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {

                    Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                    foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                    {
                        byte[] macAddr = network.dot11Bssid;
                        string tMac = "";
                        for (int i = 0; i < macAddr.Length; i++)
                        {
                            tMac += macAddr[i].ToString("x2").PadLeft(2, '0').ToUpper();
                        }
                        wifis.Add(new CurWifi(GetStringForSSID(network.dot11Ssid), tMac,Convert.ToInt32( network.linkQuality)));


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("getwifis error:" + ex.ToString());
            }




            return wifis;

        }



        public string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.UTF8.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

  

        public void SetPosInPicture()
        {
            

            
            pictureBox1.Controls.RemoveByKey("poss");


          foreach(  position pos in com.Select<position>())
          {

              string cors = pos.pixelxy;

              int x = int.Parse(cors.Substring(2, cors.IndexOf("y") - 2));
              int y = int.Parse(cors.Substring(cors.LastIndexOf(":") + 1, cors.Length - 1 - cors.LastIndexOf(":")));

              PictureBox pb = new PictureBox();
              pb.Name="poss";
              new ToolTip().SetToolTip(pb, pos.pname);
              pb.Location = new System.Drawing.Point(x, y);
              pb.BackColor = Color.Transparent;
              pb.Image = pictureBox2.Image;
              pb.Size = pictureBox2.Size;
              pb.SizeMode = PictureBoxSizeMode.StretchImage;

              pictureBox1.Controls.Add(pb);
              pb.BringToFront();
          }
 #region old2
            /*
            try
            {
               
                
                DataTable res = com.executecommand("select * from positions").Tables[0];

                foreach (DataRow row in res.Rows)
                {


                    string cors = row["pixelsxy"].ToString();

                    int x = int.Parse(cors.Substring(2, cors.IndexOf("y") - 2));
                    int y = int.Parse(cors.Substring(cors.LastIndexOf(":") + 1, cors.Length - 1 - cors.LastIndexOf(":")));

                    PictureBox pb = new PictureBox();
                    new ToolTip().SetToolTip(pb, row["pname"].ToString() + Environment.NewLine + row["wifisignals"]);
                    pb.Location = new System.Drawing.Point(x, y);
                    pb.BackColor = Color.Transparent;
                    pb.Image = pictureBox2.Image;
                    pb.Size = pictureBox2.Size;
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;

                    pictureBox1.Controls.Add(pb);


                }

            }
            catch (Exception ex)
            {


                MessageBox.Show("form1 error" + ex.ToString());
            }
                 */
#endregion

        }
        private void Form1_Load(object sender, EventArgs e)
        {




            timer1.Start();
         
SetPosInPicture();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form3(list, com,this).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form4(com,this).Show();


        }

        private void button4_Click(object sender, EventArgs e)
        {  position pos = new position() { pname = "123", pixelxy = "x:" + 2 + "y:" + 2 };

        wifirouter ro = new wifirouter() { mac = "tetm", name = "vava" };
        com.Add<wifirouter>(ro);

            wifisignal sign = new wifisignal { signal = 100 };

            sign.wifirouter =  com.Select<wifirouter>(z =>z.mac == "tetm")[0];

            
            pos.wifisignals.Add(sign);


            com.Add<position>(pos);
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Dispose();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form5(this, com).Show();
        }
        

    }
}

