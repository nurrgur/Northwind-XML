using MyModel;
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

namespace NorthwindXML.AzraNurGür
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NorthwindEntities db = new NorthwindEntities();
        private void Form1_Load(object sender, EventArgs e)
        {
            SatisGetir();
        }

        private void SatisGetir()
        {
            
            var sorgu1 = (from emp in db.Employees
                          join o in db.Orders on emp.EmployeeID equals o.EmployeeID
                          join c in db.Customers on o.CustomerID equals c.CustomerID
                          join od in db.Order_Details on o.OrderID equals od.OrderID
                          select new
                          {
                              Calisan = emp.FirstName,
                              musteri = c.CompanyName,
                              Tarih = o.OrderDate,
                              Tutar = od.UnitPrice * od.Quantity,
                              indirim=(1-od.Discount)
                          }).ToList();
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Calisan";
            dataGridView1.Columns[1].Name = "Musteri";
            dataGridView1.Columns[2].Name = "Tarih";
            dataGridView1.Columns[3].Name = "Tutar";

            foreach (var item in sorgu1)
            {
                decimal k = (decimal)item.indirim;
                string tutar = (k* item.Tutar).ToString();

                dataGridView1.Rows.Add(item.Calisan,item.musteri,item.Tarih.Value.ToString(),tutar);
            }
        }

        private void btnXml_Click(object sender, EventArgs e)
        {
            decimal i,k = 0;
            
            XmlDocument xdoc = new XmlDocument();
            XmlElement satislar = xdoc.CreateElement("satislar");

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                XmlElement satis = xdoc.CreateElement("satis");

                XmlElement calisan = xdoc.CreateElement("calisan");
                calisan.InnerText = item.Cells["Calisan"].Value.ToString();

                XmlElement musteri = xdoc.CreateElement("musteri");
                musteri.InnerText = item.Cells["Musteri"].Value.ToString();

                XmlElement tarih = xdoc.CreateElement("tarih");
                tarih.InnerText= item.Cells["Tarih"].Value.ToString(); ;

                XmlElement tutar = xdoc.CreateElement("tutar");
                tutar.InnerText = item.Cells["Tutar"].Value.ToString(); 

                satis.AppendChild(musteri);
                satis.AppendChild(tarih);
                satis.AppendChild(tutar);

                satislar.AppendChild(satis);

            }
            xdoc.AppendChild(satislar);
            xdoc.Save(@"..\..\Satislar.xml");
            MessageBox.Show("xml oluşturuldu.");

        }
    }
}
