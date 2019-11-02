using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Travel_Diary
{
    public partial class Form2 : Form
    {
        bool isUpLoadPicture;  //是否上传图片，用于在点击保存时，判断是否有图片，如果有则添加图片路径到
        string empUpLoadPictureFormat;  //上传的图片的后缀名 
        string empUpLoadPictureRealPos;  //上传图片的路径
        public Form2()
        {
            InitializeComponent();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您确认关闭当前窗口吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //关闭当前窗口
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != null && richTextBox1.Text != null)
            {
                DialogResult result = MessageBox.Show("您确认添加该条记录吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //关闭当前窗口
                    this.Close();

                    //String empImageName = "";
                    //开始保存:需要根据是否上传照片  
                    if (isUpLoadPicture == true)
                    {
                        //说明上传了相片  

                        //empImageName = DateTime.Now.Ticks / 10000 + "." + empUpLoadPictureFormat;
                        //OpenFileDialog dlg = new OpenFileDialog();
                        //File.Copy(dlg.FileName, Application.StartupPath + "\\images\\" + empUpLoadPictureFormat);
                        //string str2 = Application.StartupPath + "\\images\\" + empUpLoadPictureFormat;
                        string str2 = empUpLoadPictureRealPos;
                        Console.WriteLine(str2);

                        SqlConnection conn = new SqlConnection();
                        conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
                        //将路径保存到数据库中
                        try
                        {
                            conn.Open();
                            //错误操作：SqlCommand cmd = new SqlCommand("INSERT INTO diary VALUES ('"+str2+"','"+textBox1.Text+"','"+ dateTimePicker1.Value + "','"+ dateTimePicker1.Value + "')" , conn);
                            SqlCommand cmd = new SqlCommand("INSERT INTO diary VALUES (@Tpic,@Taddress,@Tdate,@Tlog)", conn);
                            SqlParameter parpic = new SqlParameter("@Tpic", str2);
                            cmd.Parameters.Add(parpic);
                            SqlParameter paraddr = new SqlParameter("@Taddress", textBox1.Text);
                            cmd.Parameters.Add(paraddr);
                            SqlParameter pardate = new SqlParameter("@Tdate", dateTimePicker1.Value);
                            cmd.Parameters.Add(pardate);
                            SqlParameter parlog = new SqlParameter("@Tlog", richTextBox1.Text);
                            cmd.Parameters.Add(parlog);
                            cmd.ExecuteNonQuery();
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
                        //没有上传照片  
                        Console.WriteLine("没有上传照片！");
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("请填写空白部分！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}
