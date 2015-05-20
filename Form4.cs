using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace stu_position
{
    public partial class Form4 : Form
    {
        DBcomunicator com;
        Form1 f;
        public Form4(DBcomunicator com,Form1 f)
        {
            this.f = f;
            this.com = com;
            InitializeComponent();
        }
        
        private void Form4_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = com.Select<position>();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            com.Delete<position>(x=>x.pname.Equals(dataGridView1.SelectedRows[0].Cells["pname"].Value));
            dataGridView1.DataSource = com.Select<position>();
            f.SetPosInPicture();
        }
    }
}
