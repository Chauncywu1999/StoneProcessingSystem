using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace test3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        //define a global variable as verification code
        string code = "";
        //define a string , where include the verification code element
        string a = "0123456789";
        //define a time variable
        int time;
        private void Button1_Click(object sender, EventArgs e)
        {
            if (code == textBox4.Text)
            {
                string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
                MySqlConnection mysql_connect = new MySqlConnection(str);
                //open the database
                mysql_connect.Open();

                if ((textBox1.Text != "0") && (textBox2.Text != "0") && (textBox3.Text != "0") && (textBox4.Text != "0"))
                {
                    string person_info = "insert into person_info values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')";
                    MySqlCommand person = new MySqlCommand(person_info, mysql_connect);
                    //insert data
                    person.ExecuteNonQuery();
                    //disable the connection
                    mysql_connect.Close();
                    //display the form1
                    this.Hide();
                    Form1 f1 = new Form1();
                    f1.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("验证码有误,请重试", "ERROR");
            }
            //initialize the code
            code = "";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //instantiate a random number
            Random b = new Random();
            //loop 6 times then get a six-digit verification code
            for (int i = 0; i < 6; i++)
            {
                code += a.Substring(b.Next(0, a.Length), 1);
            }

            //create the server object
            SmtpClient smtp = new SmtpClient("smtp.163.com");
            //create mail object to send
            MailAddress mail1 = new MailAddress("Chauncywu1999@163.com");
            try
            {
                //get the recipient's mailbox
                MailAddress mail2 = new MailAddress(textBox3.Text);
                //create the mail object 
                MailMessage mess = new MailMessage(mail1, mail2);
                //mail's title 
                mess.Subject = "verification code";
                //mail's content
                mess.Body = "your verification code is " + code + ", please verify the mail address in 30 minutes, do not reply to system mail";
                //initialize the code
                //code = "";
                //create the net security certificate
                NetworkCredential cred = new NetworkCredential("Chauncywu1999@163.com", "wuxi2001");
                //bind the certificate to server object for server to verify
                smtp.Credentials = cred;
                //start to send 
                smtp.Send(mess);
                //button can't use
                button2.Enabled = false;
                //start to count time 
                timer1.Enabled = true;
                //define the time as 60
                time = 60;
                MessageBox.Show("Sent successfully");
            }
            catch
            {
                MessageBox.Show("Please input the right mailbox format");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            time--;
            button2.Text = "time";
            if (time <= 0)
            {
                button2.Text = "发送验证码";
                timer1.Enabled = false;
                button2.Enabled = true;
            }
        }
    }
}
