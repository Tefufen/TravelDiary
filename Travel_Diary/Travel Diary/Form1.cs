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
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
            try
            {
                conn.Open();
                var list = new List<string>();
                int i = 0;
                SqlCommand cmd1 = new SqlCommand("SELECT Taddress FROM diary", conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();  //定义一个数据阅读器
                while (reader1.Read())
                {
                    list.Add(reader1["Taddress"].ToString());
                    i++;
                }
                if (reader1 != null && !reader1.IsClosed)
                    reader1.Close();
                foreach(var item in list)
                {                 
                    comboBox1.Items.Add(item);  //将数据库中Taddress获取的值加入到下拉框中
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        //添加记录，转到Form2
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 fm = new Form2();
            fm.ShowDialog();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        //查询记录并显示在界面上
        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex != -1)
            {
                string addr = (string)comboBox1.SelectedItem;
                Console.WriteLine(addr);
                string searchSQL = "SELECT * FROM diary WHERE Taddress='" + addr +"'";
                //Console.WriteLine(searchSQL);
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
                try
                {
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand(searchSQL, conn);
                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    if (reader2.Read())
                    {
                        pictureBox1.ImageLocation = (string)reader2["Tpic"]; //显示图片
                        Console.WriteLine(reader2["Tpic"]);
                        label4.Text = (string)reader2["Taddress"]; //显示目的地
                        Console.WriteLine(reader2["Taddress"]);
                        label5.Text = reader2["Tdate"].ToString(); //显示时间
                        Console.WriteLine(reader2["Tdate"]);
                        richTextBox1.Text = (string)reader2["Tlog"]; //显示记录
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
            else
            {
                MessageBox.Show("请先选择目的地！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //删除记录
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string addr = (string)comboBox1.SelectedItem;
                Console.WriteLine(addr);
                string deleteSQL = "delete FROM diary WHERE Taddress=@Taddress";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=LAPTOP-RCE8OGQ0;Initial Catalog=Travel_Diary;Integrated Security=TRUE";
                try
                {
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(deleteSQL, conn);
                    cmd3.Parameters.Add(new SqlParameter("@Taddress", addr));
                    int rows = cmd3.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("已删除" + rows + "条记录！", "操作提示");
                        this.Hide();
                        Form1 fm2 = new Form1();
                        fm2.Show();
                    }
                    else
                    {
                        MessageBox.Show("没有删除对应的记录！", "操作提示");
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
            else
            {
                MessageBox.Show("请先选择目的地！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //修改记录，跳转到Form3
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string addr = (string)comboBox1.SelectedItem;
                Form3 fm3 = new Form3(addr);
                fm3.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("请先选择目的地！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
