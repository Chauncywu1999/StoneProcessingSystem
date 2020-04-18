using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;


using MySql.Data.MySqlClient;

namespace test3
{
    public partial class Form3 : Form
    {
        ////define a global picture variable
        //string pic = "";
        //define a global variable
        string machinabilityGrade = "";
        bool pictureBoxStatus = false;
        string toolNumber;


        //resize the pictureBox
        private Matrix transform = new Matrix();
        private float m_dZoomscale = 1.0f;
        public const float s_dScrollValue = 0.1f;


        //connect to the mysql database
        string str = "server = 127.0.0.1;user id = root;password=wuxi*2001;database=stone_processing_system";


        //define the stoneNumber
        string stoneNumber = "";
        public Form3()
        {
            InitializeComponent();
            timer1.Start();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection mysqlconncetion1 = new MySqlConnection(str);
                mysqlconncetion1.Open();
                string stone_information = "select stone_baseinfo.stonenumber,stonename,machinablegrade,length,width,height,stonehardness,cuttingforce,abrasionresistance from machinability,stone_baseinfo where stone_baseinfo.stonenumber = machinability.stonenumber";
                MySqlDataAdapter stoneinfo_adapter = new MySqlDataAdapter(stone_information, mysqlconncetion1);
                DataSet stoneinfo = new DataSet();
                stoneinfo_adapter.Fill(stoneinfo, "stone_information");
                for (int i = 0; i < stoneinfo.Tables["stone_information"].Rows.Count; i++)
                {
                    int index = dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = stoneinfo.Tables["stone_information"].Rows[i][0].ToString();
                    this.dataGridView1.Rows[index].Cells[1].Value = stoneinfo.Tables["stone_information"].Rows[i][1].ToString();
                    this.dataGridView1.Rows[index].Cells[2].Value = stoneinfo.Tables["stone_information"].Rows[i][2].ToString();
                    this.dataGridView1.Rows[index].Cells[3].Value = stoneinfo.Tables["stone_information"].Rows[i][3].ToString();
                    this.dataGridView1.Rows[index].Cells[4].Value = stoneinfo.Tables["stone_information"].Rows[i][4].ToString();
                    this.dataGridView1.Rows[index].Cells[5].Value = stoneinfo.Tables["stone_information"].Rows[i][5].ToString();
                    this.dataGridView1.Rows[index].Cells[6].Value = stoneinfo.Tables["stone_information"].Rows[i][6].ToString();
                    this.dataGridView1.Rows[index].Cells[7].Value = stoneinfo.Tables["stone_information"].Rows[i][7].ToString();
                    this.dataGridView1.Rows[index].Cells[8].Value = stoneinfo.Tables["stone_information"].Rows[i][8].ToString();
                }

                mysqlconncetion1.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("出现错误" + ex.Message);
            }

            // TODO: This line of code loads data into the 'stone_processing_systemDataSet.machinability' table. You can move, or remove it, as needed.
            this.machinabilityTableAdapter.Fill(this.stone_processing_systemDataSet.machinability);
            // TODO: This line of code loads data into the 'stone_processing_systemDataSet.stone_manager' table. You can move, or remove it, as needed.
            this.stone_managerTableAdapter.Fill(this.stone_processing_systemDataSet.stone_manager);
            // TODO: This line of code loads data into the 'stone_processing_systemDataSet.stone_elseinfo' table. You can move, or remove it, as needed.
            this.stone_elseinfoTableAdapter.Fill(this.stone_processing_systemDataSet.stone_elseinfo);
            // TODO: This line of code loads data into the 'stone_processing_systemDataSet.person_info' table. You can move, or remove it, as needed.
            this.person_infoTableAdapter.Fill(this.stone_processing_systemDataSet.person_info);
            // TODO: This line of code loads data into the 'stone_processing_systemDataSet.stone_baseinfo' table. You can move, or remove it, as needed.
            this.stone_baseinfoTableAdapter.Fill(this.stone_processing_systemDataSet.stone_baseinfo);
            treeView1.ExpandAll();

            //fill the comboBox7
            comboBox7.Items.Add("石材编号");
            comboBox7.Items.Add("石材名称");
            comboBox7.Items.Add("可加工等级");
            comboBox7.Items.Add("长");
            comboBox7.Items.Add("宽");
            comboBox7.Items.Add("高");
            comboBox7.Items.Add("肖氏硬度");
            comboBox7.Items.Add("切削力");
            comboBox7.Items.Add("耐磨率");
            comboBox7.Text = comboBox7.Items[0].ToString();

            //fill the comboBox8
            comboBox8.Items.Add("成分4");
            comboBox8.Items.Add("成分5");
            comboBox8.Items.Add("成分6");
            comboBox8.Items.Add("成分7");
            comboBox8.Items.Add("成分8");
            comboBox8.Items.Add("成分9");
            comboBox8.Items.Add("成分10");
            comboBox8.Text = comboBox8.Items[0].ToString();

            //fill the comboBox9
            comboBox9.Items.Add("刀具编号");
            comboBox9.Items.Add("刀具名称");
            comboBox9.Items.Add("长度");
            comboBox9.Items.Add("直径");
            comboBox9.Items.Add("刀尖半径");
            comboBox9.Items.Add("材料");
            comboBox9.Items.Add("槽数");
            comboBox9.Items.Add("所允许最大转速");
            comboBox9.Items.Add("最大进给");
            comboBox9.Text = comboBox9.Items[0].ToString();

            //fill the comboBox10
            comboBox10.Items.Add("刀柄编号");
            comboBox10.Items.Add("刀柄名称");
            comboBox10.Items.Add("直径");
            comboBox10.Items.Add("长度");
            comboBox10.Items.Add("最大夹持长度");
            comboBox10.Items.Add("适用刀具编号");
            comboBox10.Text = comboBox10.Items[0].ToString();

            //fill the comboBox11
            comboBox11.Items.Add("双向");
            comboBox11.Items.Add("顺时针");
            comboBox11.Items.Add("逆时针");
            comboBox11.Text = comboBox11.Items[0].ToString();

            //fill the comboBox12
            comboBox12.Items.Add("设备编号");
            comboBox12.Items.Add("型号");
            comboBox12.Items.Add("总负载");
            comboBox12.Items.Add("第六轴负载");
            comboBox12.Items.Add("绝对定位精度");
            comboBox12.Items.Add("重复定位精度");
            comboBox12.Items.Add("IP防护等级");
            comboBox12.Items.Add("转台直径");
            comboBox12.Items.Add("转台高度");
            comboBox12.Text = comboBox12.Items[0].ToString();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = System.DateTime.Now.ToString();
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.ToString() == "主要参数")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel4.Visible = false;
                panel3.Visible = false;
                tabControl1.Visible = true;
                tabControl1.SelectTab(tabPage1);
            }
            if (e.Node.Text.ToString() == "基本参数")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel4.Visible = false;
                panel3.Visible = false;
                tabControl1.Visible = true;
                tabControl1.SelectTab(tabPage2);
            }
            if (e.Node.Text.ToString() == "石材库")
            {
                tabControl1.Visible = false;
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel4.Visible = false;
                panel3.Visible = true;
            }
            if (e.Node.Text.ToString() == "刀具创建")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
            }
            if (e.Node.Text.ToString() == "端铣刀")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                tabControl2.SelectTab(tabPage4);
            }
            if (e.Node.Text.ToString() == "球头刀")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                tabControl2.SelectTab(tabPage5);
            }
            if (e.Node.Text.ToString() == "锥度球铣刀")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                tabControl2.SelectTab(tabPage6);
            }
            if (e.Node.Text.ToString() == "自定义刀具")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                tabControl2.SelectTab(tabPage7);
            }
            if (e.Node.Text.ToString() == "刀柄")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel5.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                tabControl2.SelectTab(tabPage3);
            }
            if (e.Node.Text.ToString() == "刀具库")
            {
                panel7.Visible = false;
                panel6.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = true;
            }
            if (e.Node.Text.ToString() == "刀柄库")
            {
                panel7.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = true;
            }
            if(e.Node.Text.ToString() == "基础信息")
            {
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = true;
                tabControl3.SelectTab(tabPage8);
            }
            if(e.Node.Text.ToString() == "设备库")
            {
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = true;
                tabControl3.SelectTab(tabPage9);
            }
            if(e.Node.Text.ToString() == "基本信息")
            {

                panel2.Visible = true;
            }
            

            //switch (e.Node.Text.ToString())
            //{
            //    case "主要参数":
            //        tabControl1.Visible = true;
            //        tabControl1.SelectTab(tabPage1);
            //        break;
            //    case "基本参数":
            //        tabControl1.Visible = true;
            //        tabControl1.SelectTab(tabPage2);
            //        break;
            //    case "石材库":
            //        panel3.Visible = true;
            //        break;
            //    case "刀具创建":
            //        panel4.Visible = true;
            //        break;
            //    case "端铣刀":
            //        panel4.Visible = true;
            //        tabControl2.SelectTab(tabPage4);
            //        break;
            //    case "球头刀":
            //        panel4.Visible = true;
            //        tabControl2.SelectTab(tabPage5);
            //        break;
            //    case "锥度球铣刀":
            //        panel4.Visible = true;
            //        tabControl2.SelectTab(tabPage6);
            //        break;
            //    case "自定义刀具":
            //        panel4.Visible = true;
            //        tabControl2.SelectTab(tabPage7);
            //        break;
            //    case "刀柄":
            //        panel4.Visible = true;
            //        tabControl2.SelectTab(tabPage3);
            //        break;
            //    case "刀具库":
            //        panel5.Visible = true;
            //        break;
            //    case "刀柄库":
            //        panel6.Visible = true;
            //        break;
            //    default:
            //        break;
            //}
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //panel1.Visible = false;
            //panel2.Visible = false;
            //panel3.Visible = false;
            //panel4.Visible = false;
            //panel5.Visible = false;
            //panel6.Visible = false;
        }

        private void TreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            //if ((sender as TreeView) != null)
            //{
            //    treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            //}

            //panel1.Visible = false;
            //panel2.Visible = false;
            //panel3.Visible = false;
            //panel4.Visible = false;
            //panel5.Visible = false;
            //panel6.Visible = false;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            //create the connection string
            //string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
            MySqlConnection mySqlConnection = new MySqlConnection(str);
            mySqlConnection.Open();

            if ((textBox12.Text != "0") && (textBox8.Text != "0") && (comboBox1.Text != "0") &&
                (textBox9.Text != "0") && (textBox34.Text != "0") && (textBox10.Text != "0") &&
                (textBox11.Text != "0") && (textBox7.Text != "0") && (pictureBox3.Image != null))
            {
                //convert the image to the imageStream 
                MemoryStream imageStream = new MemoryStream();
                pictureBox3.Image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                //get byte array of the picture (request memory for the image stream)
                byte[] imageByte = imageStream.GetBuffer();

                //set command parameters about table stone_elseInfo
                string stoneElseInfo_sql = "insert into stone_elseInfo  values('" + textBox12.Text + "','" +
                    textBox8.Text + "','" + comboBox1.Text + "','" + textBox9.Text + "'," + "?imageByte)";
                MySqlCommand stoneElseInfo = new MySqlCommand(stoneElseInfo_sql, mySqlConnection);
                stoneElseInfo.Parameters.Add(new MySqlParameter("?imageByte", MySqlDbType.MediumBlob)).Value = imageByte;
                stoneElseInfo.ExecuteNonQuery();

                //set command parameters about table stone_manager
                string stoneManager_sql = "insert into stone_manager values('" + textBox9.Text + "','" + textBox12.Text + "','" + 
                    textBox10.Text + "','" + textBox34.Text + "','" + textBox7.Text + "')";
                MySqlCommand stoneManager = new MySqlCommand(stoneManager_sql, mySqlConnection);
                stoneManager.ExecuteNonQuery();

                //release resources
                stoneElseInfo.Dispose();
                stoneManager.Dispose();
                mySqlConnection.Close();

                stoneNumber = textBox12.Text;

                MessageBox.Show("其他信息已输入数据库中");
            }
            else
            {
                MessageBox.Show("请填写完整的石材信息!", "ERROR");
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "请选择要上传的图片";
            openfile.Filter = "上传图片 (*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|AllFiles(*.*)|*.*";
            if (DialogResult.OK == openfile.ShowDialog())
            {
                try
                {
                    string image_path = openfile.FileName;
                    pictureBox3.ImageLocation = image_path;
                    pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                    //Bitmap bmp = new Bitmap(openfile.FileName);
                    //pictureBox3.Image = bmp;
                    //pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                    //MemoryStream ms = new MemoryStream();
                    //bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    //byte[] arr = new byte[ms.Length];
                    //ms.Position = 0;
                    //ms.Read(arr, 0, (int)ms.Length);
                    //ms.Close();

                    //pic = Convert.ToBase64String(arr);
                }
                catch { }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if ((checkBox1.Checked != true) || (checkBox2.Checked != true) || (checkBox3.Checked != true))
            {
                //if (checkBox1.Checked == true)
                //{
                //    textBox16.Text = ((100.0 - Convert.ToDouble(textBox13.Text)) / 10.0).ToString();
                //}
                if(checkBox2.Checked == true)
                {
                    if (Convert.ToSingle(textBox14.Text) < 600 && Convert.ToSingle(textBox14.Text) > 500)
                        textBox16.Text = "一";
                    if (Convert.ToSingle(textBox14.Text) < 700 && Convert.ToSingle(textBox14.Text) >= 600)
                        textBox16.Text = "二";
                    if (Convert.ToSingle(textBox14.Text) < 800 && Convert.ToSingle(textBox14.Text) >= 700)
                        textBox16.Text = "三";
                    if (Convert.ToSingle(textBox14.Text) < 900 && Convert.ToSingle(textBox14.Text) >= 800)
                        textBox16.Text = "四";
                    if (Convert.ToSingle(textBox14.Text) < 1000 && Convert.ToSingle(textBox14.Text) >= 900)
                        textBox16.Text = "五";
                    if (Convert.ToSingle(textBox14.Text) >= 1000)
                        textBox16.Text = "六";
                }
            }
            else
            {
                MessageBox.Show("请至少勾选一项进行计算");
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //create the connection string 
            //string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
            MySqlConnection mysqlConnection = new MySqlConnection(str);
            mysqlConnection.Open();

            //define the value of the machinabilityGrade
            machinabilityGrade = textBox16.Text;

            //set command parameters
            string machinability_sql = "insert into machinability values('" + textBox12.Text + "','" + 
                textBox13.Text + "',?Double,'" + textBox15.Text + "','" + textBox16.Text + "')";
            MySqlCommand machinability = new MySqlCommand(machinability_sql, mysqlConnection);
            if (textBox14.Text == "")
                machinability.Parameters.Add(new MySqlParameter("?Double", MySqlDbType.Text)).Value = null;
            else
                machinability.Parameters.Add(new MySqlParameter("?Double", MySqlDbType.Double)).Value = Convert.ToDouble(textBox14.Text);

            machinability.ExecuteNonQuery();

            machinability.Dispose();
            mysqlConnection.Close();

            //remind the user everything is OK
            MessageBox.Show("该部分信息已输入数据库中");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            //create connect string
            //string str = "server = 127.0.0.1; User Id = root; password = wuxi*2001; Database = stone_processing_system";
            MySqlConnection mysqlConnection = new MySqlConnection(str);
            mysqlConnection.Open();

            if ((textBox30.Text != null) && (textBox31.Text != null) && (comboBox2.Text != null) &&
                (textBox32.Text != null) && (textBox33.Text != null)&& (textBox1.Text != null)&&
                (textBox2.Text != null)&&(textBox3.Text != null)&&(textBox4.Text != null))
            {
                //set command parameters
                string stonePhysicalInfo_sql = "insert into stone_baseInfo values('" + stoneNumber + "','" + textBox1.Text + "','" + textBox2.Text +
                    "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox30.Text + "','" + textBox31.Text + "','" + comboBox2.Text +
                    "','" + textBox33.Text + "','" +textBox32.Text + "','null')";
                MySqlCommand stonePhysicalInfo = new MySqlCommand(stonePhysicalInfo_sql, mysqlConnection);
                stonePhysicalInfo.ExecuteNonQuery();

                //release resources
                stonePhysicalInfo.Dispose();
                mysqlConnection.Close();

                MessageBox.Show("物理信息已输入数据库中");
            }
            else
            {
                MessageBox.Show("请补充完整石材数据");
            }

        }

        private void TabControl1_Click(object sender, EventArgs e)
        {
            textBox29.Text = stoneNumber;
        }

        private void Button19_Click(object sender, EventArgs e)
        {
            pictureBoxStatus = true;
            pictureBox5.Refresh();
        }

        private void PictureBox5_Paint(object sender, PaintEventArgs e)
        {
            //if the status is false 
            if (pictureBoxStatus == false) return;
            Graphics g = e.Graphics;
            g.Transform = transform;

            float length = Convert.ToSingle(textBox47.Text);
            float width = Convert.ToSingle(textBox48.Text);

            //draw the rectangle
            Pen pen = new Pen(Color.Black, 5);
            g.DrawRectangle(pen, 50, 50, length * 1.5F, width * 1.5F);
            
            //fill the rectangle
            Brush brush = new SolidBrush(Color.Yellow);
            g.FillRectangle(brush, 53, 53, length * 1.5F - 5.0F, width * 1.5F - 5.0F);

            //draw the center line
            Pen dashPen = new Pen(Color.Red, 3);
            dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            dashPen.DashPattern = new float[] { 6, 5 };
            g.DrawLine(dashPen, 30.0f, width * 1.5F / 2.0f + 50.0f, 70.0f + length * 1.5F, width * 1.5F / 2.0f + 50.0f);
        }

        //mouseWheel event
        protected override void OnMouseWheel(MouseEventArgs mouseEvent)
        {
            pictureBox5.Focus();
            if (pictureBox5.Focus() == true && mouseEvent.Delta !=0)
            {
                Point pictureBoxPoint = pictureBox5.PointToClient(this.PointToScreen(mouseEvent.Location));
                ZoomScroll(pictureBoxPoint, mouseEvent.Delta > 0,pictureBox5);
            }

            pictureBox6.Focus();
            if (pictureBox6.Focus() == true && mouseEvent.Delta !=0)
            {
                Point pictureBoxPoint = pictureBox6.PointToClient(this.PointToScreen(mouseEvent.Location));
                ZoomScroll(pictureBoxPoint, mouseEvent.Delta > 0,pictureBox6);
            }

            pictureBox8.Focus();
            if (pictureBox8.Focus() == true && mouseEvent.Delta != 0)
            {
                Point pictureBoxPoint = pictureBox8.PointToClient(this.PointToScreen(mouseEvent.Location));
                ZoomScroll(pictureBoxPoint, mouseEvent.Delta > 0, pictureBox8);
            }
        }


        //zoom scroll
        private void ZoomScroll(Point location, bool zoonIn,PictureBox pictureBox)
        {
            float newScale = Math.Min(Math.Max(m_dZoomscale + (zoonIn ? s_dScrollValue : -s_dScrollValue), 0.1f), 10);

            if (newScale != m_dZoomscale)
            {
                float adjust = newScale / m_dZoomscale;
                m_dZoomscale = newScale;

                transform.Translate(-location.X, -location.Y, MatrixOrder.Append);
                transform.Scale(adjust, adjust, MatrixOrder.Append);
                transform.Translate(location.X, location.Y, MatrixOrder.Append);
                pictureBox.Invalidate(); //overload control
            }
        }


        private void TextBox35_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button20_Click(object sender, EventArgs e)
        {
            pictureBoxStatus = true;
            pictureBox6.Refresh();
        }

        private void PictureBox6_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBoxStatus == false) return;
            //define the variables
            float width = Convert.ToSingle(textBox49.Text);
            float length = Convert.ToSingle(textBox50.Text);


            Graphics g = e.Graphics;
            g.Transform = transform;

            //draw outline
            Pen pen = new Pen(Color.Black, 5);
            RectangleF rect = new RectangleF(length - width + 50.0f, 50.0f, width, width);
            float startAngle = -90.0f;
            float sweepAngle = 180.0f;

            PointF[] points =
            {
                new PointF(length - width + 50.0f+width/2.0f,50.0f),
                new PointF(50.0f,50.0f),
                new PointF(50.0f,width+50.0f),
                new PointF(length - width + 50.0f+width/2.0f,50.0f+width)
            };
            g.DrawArc(pen, rect, startAngle, sweepAngle);
            g.DrawLines(pen, points);

            //fill outline
            Brush brush = new SolidBrush(Color.Yellow);
            g.FillEllipse(brush, rect);
            g.FillPolygon(brush, points);

            //draw the centerline
            Pen dashPen = new Pen(Color.Red, 3);
            dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            dashPen.DashPattern = new float[] { 6, 5 };
            g.DrawLine(dashPen, 30.0f, width / 2.0f + 50.0f, 70.0f + length, width / 2.0f + 50.0f);
        }

        private void TabControl2_TabStopChanged(object sender, EventArgs e)
        {
            
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxStatus = false;
            textBox47.Text = "";
            textBox48.Text = "";
            textBox49.Text = "";
            textBox50.Text = "";
        }

        private void Button28_Click(object sender, EventArgs e)
        {
            pictureBoxStatus = true;
            pictureBox8.Refresh();
        }

        private void PictureBox8_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBoxStatus == false) return;

            //define the variables
            float angle = Convert.ToSingle(textBox77.Text);
            float radius = Convert.ToSingle(textBox63.Text);
            float width = Convert.ToSingle(textBox78.Text);
            float length = Convert.ToSingle(textBox64.Text);

            Graphics g = e.Graphics;
            g.Transform = transform;

            //draw the outline
            Pen pen = new Pen(Color.Black, 5);
            RectangleF rectF = new RectangleF(length - radius / Convert.ToSingle(Math.Sin(angle)) - radius + 50.0f, 50.0f + width / 2 - radius, radius * 2.0f, radius * 2.0f);

            float startAngle = angle-90.0f;
            float sweepAngle = 180.0f - 2.0f * angle;

            PointF[] points =
            {
                new PointF(length - radius / Convert.ToSingle(Math.Sin(angle))+50.0f+radius*Convert.ToSingle(Math.Sin(angle)),50.0f+width/2.0f-radius*Convert.ToSingle(Math.Cos(angle))),
                new PointF(50.0f+length-width/Convert.ToSingle(Math.Tan(angle)),50.0f),
                new PointF(50.0f,50.0f),
                new PointF(50.0f,50.0f+width),
                new PointF(50.0f+length-width/Convert.ToSingle(Math.Tan(angle)),50.0f+width),
                new PointF(length - radius / Convert.ToSingle(Math.Sin(angle))+50.0f+radius*Convert.ToSingle(Math.Sin(angle)),50.0f+width/2.0f+radius*Convert.ToSingle(Math.Cos(angle)))
            };
            g.DrawArc(pen, rectF, startAngle, sweepAngle);
            g.DrawLines(pen, points);

            //fill outline
            Brush brush = new SolidBrush(Color.Yellow);
            g.FillEllipse(brush, rectF);
            g.FillPolygon(brush, points);

            //draw the centerline
            Pen dashPen = new Pen(Color.Red, 3);
            dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            dashPen.DashPattern = new float[] { 6, 5 };
            g.DrawLine(dashPen, 30.0f, width / 2.0f + 50.0f, 70.0f + length, width / 2.0f + 50.0f);
        }

        private void TextBox36_TextChanged(object sender, EventArgs e)
        {
            textBox35.Text = "D-" + textBox36.Text;
            //textBox35.Enabled = false;
        }

        private void TextBox61_TextChanged(object sender, EventArgs e)
        {
            textBox62.Text = "Q-" + textBox61.Text;
        }

        private void TextBox75_TextChanged(object sender, EventArgs e)
        {
            textBox76.Text = "Z-" + textBox75.Text;
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex.ToString() == "0")
            {
                textBox38.Enabled = true;
                //textBox38.Refresh();
            }
            else
            {
                textBox38.Enabled = false;
                //textBox38.Refresh();
            }
        }

        private void Button27_Click(object sender, EventArgs e)
        {
            ////connect to the mysql
            //string str = "server = 127.0.0.1;user id = root;password=wuxi*2001;database=stone_processing_system";
            //MySqlConnection mySqlConnection = new MySqlConnection(str);
            //mySqlConnection.Open();

            //if((textBox35.Text !=null)&&(textBox36.Text != null)&&(textBox37.Text != null)&&(comboBox3.Text !=null))
            //{
            //    string toolBaseinfo_sql = "insert into toolbaseinfo values(" + textBox35.Text + "," + textBox36.Text + "," + comboBox3.Text + "," + textBox37.Text + "," + textBox38.Text + ")";
            //    MySqlCommand toolBaseinfo = new MySqlCommand(toolBaseinfo_sql, mySqlConnection);
            //    toolBaseinfo.ExecuteNonQuery();

            //    toolBaseinfo.Dispose();
            //    mySqlConnection.Close();
            //    MessageBox.Show("已加入数据库中");
            //}
            //else
            //{
            //    MessageBox.Show("请补充完整刀具基本信息");
            //}
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            //connect to the mysql
            MySqlConnection mySqlConnection = new MySqlConnection(str);
            mySqlConnection.Open();

            if ((textBox35.Text != null) && (textBox36.Text != null) && (textBox37.Text != null) && (comboBox3.Text != null))
            {
                string toolBaseinfo_sql = "insert into toolbaseinfo values('" + textBox35.Text + "','" + textBox36.Text + "','" + comboBox3.Text + "','" + textBox37.Text + "','" + textBox38.Text + "')";
                MySqlCommand toolBaseinfo = new MySqlCommand(toolBaseinfo_sql, mySqlConnection);
                toolBaseinfo.ExecuteNonQuery();

                toolBaseinfo.Dispose();
                mySqlConnection.Close();
                MessageBox.Show("已加入数据库中");
            }
            else
            {
                MessageBox.Show("请补充完整刀具基本信息");
            }
        }

        private void ComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button46_Click(object sender, EventArgs e)
        {
            if(textBox100.Text != null)
            {
                int rownumber = dataGridView1.Rows.Count;
                for(int i=0;i<rownumber-1;i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() != textBox100.Text)
                        dataGridView1.Rows.Remove(dataGridView1.Rows[i]);
                }
            }
        }

        private void Button56_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();
        }

        private void Button59_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection mysqlconncetion1 = new MySqlConnection(str);
                mysqlconncetion1.Open();
                string stone_information = "select stone_baseinfo.stonenumber,stonename,machinablegrade,length,width,height,stonehardness,cuttingforce,abrasionresistance from machinability,stone_baseinfo where stone_baseinfo.stonenumber = machinability.stonenumber";
                MySqlDataAdapter stoneinfo_adapter = new MySqlDataAdapter(stone_information, mysqlconncetion1);
                DataSet stoneinfo = new DataSet();
                stoneinfo_adapter.Fill(stoneinfo, "stone_information");
                dataGridView1.Rows.Clear();
                for (int i = 0; i < stoneinfo.Tables["stone_information"].Rows.Count; i++)
                {
                    int index = dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = stoneinfo.Tables["stone_information"].Rows[i][0].ToString();
                    this.dataGridView1.Rows[index].Cells[1].Value = stoneinfo.Tables["stone_information"].Rows[i][1].ToString();
                    this.dataGridView1.Rows[index].Cells[2].Value = stoneinfo.Tables["stone_information"].Rows[i][2].ToString();
                    this.dataGridView1.Rows[index].Cells[3].Value = stoneinfo.Tables["stone_information"].Rows[i][3].ToString();
                    this.dataGridView1.Rows[index].Cells[4].Value = stoneinfo.Tables["stone_information"].Rows[i][4].ToString();
                    this.dataGridView1.Rows[index].Cells[5].Value = stoneinfo.Tables["stone_information"].Rows[i][5].ToString();
                    this.dataGridView1.Rows[index].Cells[6].Value = stoneinfo.Tables["stone_information"].Rows[i][6].ToString();
                    this.dataGridView1.Rows[index].Cells[7].Value = stoneinfo.Tables["stone_information"].Rows[i][7].ToString();
                    this.dataGridView1.Rows[index].Cells[8].Value = stoneinfo.Tables["stone_information"].Rows[i][8].ToString();
                }

                mysqlconncetion1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现错误" + ex.Message);
            }
        }
    }
}
