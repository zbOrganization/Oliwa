using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace stu_position
{
    public partial class Form3 : Form
    {
        List<CurWifi> list;
        DBcomunicator com;
        List<wifirouter> dbrouter = null;
        Form1 f;
        public Form3(List<CurWifi> list, DBcomunicator com,Form1 f)
        {
            this.f = f;
            this.list = list;
            this.com = com;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
     dbrouter= com.Select<wifirouter>();

            foreach(CurWifi wifi in list)
            {
                dataGridView1.Rows.Add(new string[]{ wifi.BSSID, wifi.SSID, wifi.Signal.ToString() });
 
            }
            SetDataToGrid(dbrouter, dataGridView2);
          
        }
        public void SetDataToGrid(List<wifirouter> l,DataGridView grid  )
        {
            grid.Rows.Clear();
            foreach (wifirouter wifir in l)
            {
               grid.Rows.Add(new string[] { wifir.Id.ToString(), wifir.mac, wifir.name });

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string macs = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string names = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                 
                if (dbrouter.Count(x => x.mac.Equals(macs)) == 0)
                {
                    com.Add<wifirouter>(new wifirouter() { mac = macs, name = names });
                    dbrouter = com.Select<wifirouter>();
                    SetDataToGrid(dbrouter, dataGridView2);
                }
                else MessageBox.Show("ასეთი wifi როუტერი უკვე ბაზაშია");
              
            }
            catch(Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string macs = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
           

            wifirouter r=  com.Select<wifirouter>(x=>x.mac.Equals(macs))[0];
         List<wifisignal> signals=  com.Select<wifisignal>(z=>z.wifirouterID==r.Id);
           com.Delete<wifirouter>(x =>x.Id==r.Id);
            com.Delete<position>(z=>signals.Any(x=>x.positionID==z.Id));
                   

                dbrouter = com.Select<wifirouter>();
                SetDataToGrid(dbrouter, dataGridView2);
                f.SetPosInPicture();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            this.Hide();
        }
    }
}
