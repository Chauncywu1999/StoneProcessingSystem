using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetBtnStyle(button1);
            //hide two controls
            if (PublicValues.I>0)
            {
                this.panel1.Hide();
                this.button1.Hide();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            panel1.Hide();
        }

        private void SetBtnStyle(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.Transparent;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
            MySqlConnection mysqlConnect = new MySqlConnection(str);
            mysqlConnect.Open();

            if((textBox1.Text != "0")||(textBox2.Text != "0"))
            {
                //search the name and the password whether it is in the person_info(table)
                string nameSql = "select count(name) from person_info where name='" + textBox1.Text+"'";
                MySqlCommand name = new MySqlCommand(nameSql, mysqlConnect);
                
                string passwordSql = "select count(password) from person_info where password='" + textBox2.Text+"'";
                MySqlCommand password = new MySqlCommand(passwordSql, mysqlConnect);

                //judging the correctness of the results
                if (name.ExecuteScalar().ToString() !="0" && password.ExecuteScalar().ToString() !="0")
                {
                    this.Hide();
                    Form3 f3 = new Form3();
                    f3.Show();
                }
                else
                {
                    MessageBox.Show(text:"请输入正确的用户名或密码", caption:"ERROR");
                }
            }
            //Form1 f1 = new Form1();
            //f1.Dispose();
            //f1.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.Show();
            //when return the form 1,the picturebox1 and button1 will be hide
            PublicValues.I++;
        }

        //define a class about the public variable
        public class PublicValues
        {
            public static int I;
        }

        private void Button2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
                MySqlConnection mysqlConnect = new MySqlConnection(str);
                mysqlConnect.Open();

                if ((textBox1.Text != "0") || (textBox2.Text != "0"))
                {
                    //search the name and the password whether it is in the person_info(table)
                    string nameSql = "select count(name) from person_info where name='" + textBox1.Text + "'";
                    MySqlCommand name = new MySqlCommand(nameSql, mysqlConnect);

                    string passwordSql = "select count(password) from person_info where password='" + textBox2.Text + "'";
                    MySqlCommand password = new MySqlCommand(passwordSql, mysqlConnect);

                    //judging the correctness of the results
                    if (name.ExecuteScalar().ToString() != "0" && password.ExecuteScalar().ToString() != "0")
                    {
                        this.Hide();
                        Form3 f3 = new Form3();
                        f3.Show();
                    }
                    else
                    {
                        MessageBox.Show(text: "请输入正确的用户名或密码", caption: "ERROR");
                    }
                }
            }

        }
    }
}
