using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Travel_Diary
{
    public partial class Form3 : Form
    {
        bool isUpLoadPicture;  //是否上传图片，用于在点击保存时，判断是否有图片，如果有则添加图片路径到
        string empUpLoadPictureFormat;  //上传的图片的后缀名 
        string empUpLoadPictureRealPos;  //上传图片的路径
        string addr;
        public Form3()
        {
            InitializeComponent();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
        }

        public Form3(string addr)
        {
            InitializeComponent();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
            this.addr = addr;
            string searchSQL = "SELECT * FROM diary WHERE Taddress='" + addr + "'";
            try
            {
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(searchSQL, conn);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    textBox2.Text = ((string)reader2["Tpic"]).Trim();
                    pictureBox1.ImageLocation = (string)reader2["Tpic"]; //显示图片
                    Console.WriteLine(reader2["Tpic"]);
                    textBox1.Text =  ((string)reader2["Taddress"]).Trim(); //显示目的地
                    Console.WriteLine(reader2["Taddress"]);
                    dateTimePicker1.Text = (reader2["Tdate"].ToString()).Trim(); //显示时间
                    Console.WriteLine(reader2["Tdate"]);
                    richTextBox1.Text = ((string)reader2["Tlog"]).Trim(); //显示记录
                    Console.WriteLine(reader2["Tlog"]);
                }
            }
            catch
            {
                Console.WriteLine("数据库连接失败！/数据读取失败！");
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "*.jpg|*.jpg|*.png|*.png|*.bmp|*.bmp|*.tiff|*.tiff";//图片格式  
                if (dlg.ShowDialog() == DialogResult.OK) //打开文件会话窗口
                {
                    isUpLoadPicture = true;//记录是否上传了相片，用于后面保存操作使用：是一个成员变量  
                    try
                    {
                        empUpLoadPictureRealPos = dlg.FileName;//物理路径：实际的文件路径+文件名
                        textBox2.Text = empUpLoadPictureRealPos;
                        //String[] empImageData = empUpLoadPictureRealPos.Split('.');
                        //empImageData[1]:是上传的图片的后缀名  
                        //empUpLoadPictureFormat = empImageData[1];
                        pictureBox1.Image = Image.FromFile(empUpLoadPictureRealPos);//将图片显示在pitureBox控件中
                    }
                    catch
                    {
                        MessageBox.Show("您选择的图片不能被读取或文件类型不对！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("上传相片出错: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && richTextBox1.Text != null)
            {
                DialogResult result = MessageBox.Show("您确认修改该条记录吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //关闭当前窗口
                    this.Close();
                    
                    if (isUpLoadPicture == true)
                    {
                        string str2 = empUpLoadPictureRealPos;  //如果重新上传了照片重新赋值
                        Console.WriteLine(str2);

                        SqlConnection conn = new SqlConnection();
                        conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
                        //将路径保存到数据库中
                        try
                        {
                            conn.Open();
                            Console.WriteLine(addr);
                            SqlCommand cmd = new SqlCommand("UPDATE diary SET Tpic=@Tpic,Taddress=@Taddress,Tdate=@Tdate,Tlog=@Tlog WHERE Taddress='"+addr+"'", conn);
                            SqlParameter parpic = new SqlParameter("@Tpic", str2);
                            cmd.Parameters.Add(parpic);
                            SqlParameter paraddr = new SqlParameter("@Taddress", textBox1.Text);
                            cmd.Parameters.Add(paraddr);
                            SqlParameter pardate = new SqlParameter("@Tdate", dateTimePicker1.Value);
                            cmd.Parameters.Add(pardate);
                            SqlParameter parlog = new SqlParameter("@Tlog", richTextBox1.Text);
                            cmd.Parameters.Add(parlog);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch
                        {
                            Console.WriteLine("记录添加至数据库失败！");
                        }
                        finally
                        {
                            if (conn != null && conn.State == ConnectionState.Open)
                                conn.Close();
                            Form1 fm2 = new Form1();
                            fm2.Show();
                        }
                    }
                    else
                    {
                        string str2 = textBox2.Text;  //如果没有上传照片就给str2赋原值
                        Console.WriteLine(str2);

                        SqlConnection conn = new SqlConnection();
                        conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
                        //将路径保存到数据库中
                        try
                        {
                            conn.Open();
                            Console.WriteLine(addr);
                            SqlCommand cmd = new SqlCommand("UPDATE diary SET Tpic=@Tpic,Taddress=@Taddress,Tdate=@Tdate,Tlog=@Tlog WHERE Taddress='" + addr + "'", conn);
                            SqlParameter parpic = new SqlParameter("@Tpic", str2);
                            cmd.Parameters.Add(parpic);
                            SqlParameter paraddr = new SqlParameter("@Taddress", textBox1.Text);
                            cmd.Parameters.Add(paraddr);
                            SqlParameter pardate = new SqlParameter("@Tdate", dateTimePicker1.Value);
                            cmd.Parameters.Add(pardate);
                            SqlParameter parlog = new SqlParameter("@Tlog", richTextBox1.Text);
                            cmd.Parameters.Add(parlog);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch
                        {
                            Console.WriteLine("记录添加至数据库失败！");
                        }
                        finally
                        {
                            if (conn != null && conn.State == ConnectionState.Open)
                                conn.Close();
                            Form1 fm2 = new Form1();
                            fm2.Show();
                        }
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("请填写空白部分！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您确认关闭当前窗口吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //关闭当前窗口
                this.Close();
            }
        }
    }
}
