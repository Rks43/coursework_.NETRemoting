using Proginterface;
using RemoteObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovoy
{
    public partial class User : Form
    {
        private int cost;
        private int codeUser;
        private Interface newInterface = new ServerObject();
        public User()
        {
            InitializeComponent();
        }

        private string login;
        public User(string login)
        {
            //RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Kursovoy\App.config", false);
            newInterface.Connecting();
            InitializeComponent();
            label2.Text = login;
            this.login = login;
            this.codeUser = newInterface.GetCodeUser(login);
        }
        
        private void User_Load(object sender, EventArgs e)
        {
            RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Kursovoy\App.config", false);
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private List<String> nabor = new List<string>();
        private void button2_Click(object sender, EventArgs e)
        {
            string[] parts = listBox1.SelectedItem.ToString().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                string productName = parts[0].Trim(); 
                int productPrice = int.Parse(parts[1].Trim().Split(':')[1].Trim()); 

                cost += productPrice; 
                label4.Text = Convert.ToString(cost);
                nabor.Add(productName); 
            }
        }

        private string nabor2;
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (string str in nabor)
            {
                nabor2 += str+" ";
            }

            MessageBox.Show(nabor2);
           label5.Text = newInterface.AddRequest(nabor2, this.login, int.Parse(label4.Text));
           label5.Visible = true;
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

        private void button5_Click(object sender, EventArgs e)
        {
            label5.Text = newInterface.AddReview(this.codeUser, textBox1.Text);
            label5.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }
    }
}
