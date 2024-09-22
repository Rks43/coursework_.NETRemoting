using Proginterface;
using RemoteObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovoy
{
    public partial class Admin : Form
    {
        MyClientSponsor sponsor;
        ILease lease;


        public Admin()
        {
            InitializeComponent();
        }

        static Interface newInterface = new ServerObject();

        public Admin(string name)
        {
            InitializeComponent();
            label2.Text = name;
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Kursovoy\App.config", false);
            newInterface.Connecting();
            this.lease = (ILease)newInterface.InitializeLifetimeService();
            this.sponsor = new MyClientSponsor();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();

            try
            {
                List<string> products = newInterface.GetProducts();
                foreach (string product in products)
                {
                    listBox1.Items.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении списка товаров/услуг: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string service = listBox1.SelectedItem.ToString();
            //MessageBox.Show(service);
            string[] parts = listBox1.SelectedItem.ToString()
                .Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string productName = parts[0].Trim();
                MessageBox.Show(newInterface.RemoveService(productName));
            }

            listBox1.Update();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string serviceName = textBox1.Text; 
            int servicePrice;

            if (!int.TryParse(textBox2.Text, out servicePrice))
            {
                MessageBox.Show("Введите корректную цену для услуги.");
                return; 
            }

       
            string result = newInterface.AddService(serviceName, servicePrice);

       
            MessageBox.Show(result);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                List<string> reviews = newInterface.GetReviews();

                foreach (string review in reviews)
                {
                    listBox1.Items.Add(review);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении отзывов: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string feedbacks = listBox1.SelectedItem.ToString();
            //MessageBox.Show(service);
            string[] parts = listBox1.SelectedItem.ToString()
                .Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string nameUser = parts[0].Trim();
                string feedback = parts[1].Trim();
                MessageBox.Show(newInterface.RemoveFeedback(nameUser, feedback));
            }

            listBox1.Update();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                List<string> requests = newInterface.GetRequests();

                foreach (string request in requests)
                {
                    listBox1.Items.Add(request);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при получении отзывов: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedRequest = listBox1.SelectedItem.ToString();

                int price = ExtractPrice(selectedRequest);

                MessageBox.Show(newInterface.DeleteRequest(price));
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления.");
            }
        }

        private int ExtractPrice(string requestString)
        {
            Regex regex = new Regex(@"Цена:\s*(\d+(\.\d+)?)");
            Match match = regex.Match(requestString);

            if (match.Success)
            {
                string priceString = match.Groups[1].Value;
                if (int.TryParse(priceString, out int price))
                {
                    return price;
                }
            }

            return 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Sponsors();
            label5.Text = "Спонсирование сработало";
            label5.Visible = true;
        }
        private void Sponsors()
        {
            this.lease.Register(this.sponsor);
        }

        public class MyClientSponsor : MarshalByRefObject, ISponsor
        {
            public MyClientSponsor()
            {
                Console.WriteLine("\nСпонсор создан ");
            }

            public TimeSpan Renewal(ILease lease)
            {
                return TimeSpan.FromSeconds(20);

            }
        }
    }
}
