using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proginterface;
using RemoteObject;

namespace Kursovoy
{
    public partial class Form1 : Form
    {
        private Admin formAdmin;
        private User formUser;
        Interface newInterface = new ServerObject();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Kursovoy\App.config", false);
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, true);
            ServerObject remoteObject = (ServerObject)Activator.GetObject(typeof(ServerObject), "tcp://localhost:8086/tcp");
            // Подключение к HTTP серверу
            ServerObject httpRemoteObject = (ServerObject)Activator.GetObject(typeof(ServerObject), "http://localhost:8085/ServerObject.soap");
            //RemotingConfiguration.Configure(@"C:\Users\User\Desktop\КАИ\3 курс\2 семак\ТП\Курсач\Моё\Код\Login\Kursovoy\App.config", false);
            newInterface.Connecting();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                label10.Visible = true;
                label10.Text = "Поля логина и пароля обязательны.";
                return;
            }
            user = newInterface.Autorization(textBox1.Text, textBox2.Text);
            if (user == null)
            {
                
                label10.Text = "Неправильный логин или пароль";
                label10.Visible = true;
                
            }
            else if (textBox1.Text.IndexOf("admin") != -1)
            {
                this.DialogResult = DialogResult.OK;
                this.Visible = false;
                formAdmin = new Admin("Admin");
                formAdmin.Activate();
                formAdmin.Show();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Visible = false;
                formUser = new User(textBox2.Text);
                formUser.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user;
            if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                label11.Visible = true;
                label11.Text = "Поля логина и пароля, ФИО, Телефон обязательны.";
                return;
            }
            user = newInterface.Registration(textBox4.Text, textBox3.Text, textBox5.Text,textBox6.Text);
            label11.Text = user;
            label11.Visible = true;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
