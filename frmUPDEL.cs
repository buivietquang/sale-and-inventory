using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace skelot
{
    public partial class FrmUpdate : Form
    {
        SqlCommand cm;
        SqlConnection cn;
        SqlDataReader dr;
        ListViewItem lst;
        //string connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        frmLogin login = new frmLogin();


        public FrmUpdate()
        {
            //khởi tạo
            InitializeComponent();
            cn = new SqlConnection(login.connection);
            cn.Open();
        }

        public void Clear() 
        {

            txtSearch.Text = "";
            txtProductID.Text = "";
            txtPrice.Text = "";
            txtProductName.Text = "";
        }

        public void getData2()
        {
            try
            {
                // Xóa các mục và cột hiện có trong ListView
                listView1.Items.Clear();
                listView1.Columns.Clear();

                // Thêm các cột mới vào ListView
                listView1.Columns.Add("ID", 0);
                listView1.Columns.Add("Desc", 210);
                listView1.Columns.Add("Price", 80);
                listView1.Columns.Add("Type", 80);
                listView1.Columns.Add("Size", 80);
                listView1.Columns.Add("Brand", 80);
                listView1.Columns.Add("Stock", 80);

                // Tạo câu lệnh SQL để lấy dữ liệu sản phẩm, sử dụng dữ liệu từ txtSearch
                string sql = @"Select * from tblProduct where Desc like '" + txtSearch.Text + "%'";
                cm = new SqlCommand(sql, cn);

                // Thực thi câu lệnh và lấy dữ liệu
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    // Tạo một mục mới trong ListView với dữ liệu lấy được
                    ListViewItem lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString()); // Desc
                    lst.SubItems.Add(dr[2].ToString()); // Price
                    lst.SubItems.Add(dr[3].ToString()); // Type
                    lst.SubItems.Add(dr[4].ToString()); // Size
                    lst.SubItems.Add(dr[5].ToString()); // Brand
                    lst.SubItems.Add(dr[6].ToString()); // Stock
                }

                // Đóng DataReader sau khi hoàn thành
                dr.Close();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void getData()
        {
            // hiển thị data từ database đến list view
            try
            {

                listView1.Items.Clear();
                listView1.Columns.Clear();

                listView1.Columns.Add("ID", 0);
                listView1.Columns.Add("Desc", 210);
                listView1.Columns.Add("Price", 80);
                listView1.Columns.Add("Type", 80);
                listView1.Columns.Add("Size", 80);
                listView1.Columns.Add("Brand", 80);
                listView1.Columns.Add("Stock", 80);



                string sql = @"Select * from tblProduct";
                cm = new SqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lst = listView1.Items.Add(dr[0].ToString());
                    lst.SubItems.Add(dr[1].ToString());
                    lst.SubItems.Add(dr[2].ToString());
                    lst.SubItems.Add(dr[3].ToString());
                    lst.SubItems.Add(dr[4].ToString());
                    lst.SubItems.Add(dr[5].ToString());
                    lst.SubItems.Add(dr[6].ToString());
               

                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();// ản form hiện tại
            FrmAdminMenu frm7 = new FrmAdminMenu(); // tạo instance
            frm7.Show();// hiển thị
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            //gọi hàm
          getData();
          timer1.Start();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void UpdateTrail() 
        {

            //chèn dữ liệu mới vào bảng tblAuditTrail
            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("@Transactype", "Updation");
                cm.Parameters.AddWithValue("@Description", "Item: '" + txtProductName.Text + "' was UPDATED!");
                cm.Parameters.AddWithValue("@Authority", "Admin");

                cm.ExecuteNonQuery();
            
            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again.!");
                MessageBox.Show(l.Message);
            }
        }
        public void DeleteTrail() 
        {
            // chèn dữ liệu mới sau khi xóa vào bảng tblAuditTrail
            try
            {
                string sql = @"INSERT INTO tblAuditTrail VALUES(@Dater,@Transactype,@Description,@Authority)";
                cm = new SqlCommand(sql, cn);
                cm.Parameters.AddWithValue("@Dater", lblDate.Text);
                cm.Parameters.AddWithValue("Transactype", "Deletion");
                cm.Parameters.AddWithValue("@Description", "Item: '" + txtProductName.Text + "' was DELETED from inventory!");
                cm.Parameters.AddWithValue("@Authority", "Admin");


                cm.ExecuteNonQuery();
           


            }
            catch (SqlException l)
            {
                MessageBox.Show("Re-input again.!");
                MessageBox.Show(l.Message);
            }
  
        }
       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // nếu textbox trông thì không làm gì cả
            if(txtPrice.Text == "")
            {
                return;
            }
          try
            {
                // nếu không có sản phẩm được chọn
                    if(txtProductID.Text == ""){

                        MessageBox.Show("select listview properly");
                        return;
                    }
                    // câu lệnh cập nhật giá sp
               string up = @"UPDATE tblProduct SET [Price]='" + txtPrice.Text + "' where [ID]='" + txtProductID.Text + "'";
                cm = new SqlCommand(up, cn);             
               // thêm tham số vào câu lệnh sql
                cm.Parameters.AddWithValue("@Price", txtPrice.Text);
          
                  cm.ExecuteNonQuery();// thực thi
                  // hàm cập nhật
                  UpdateTrail();
                  Clear();
                  getData();
                // thành công
                    MessageBox.Show("Successfully Updated!");
            }
            catch (Exception ex)
            {
                // báo lỗi
                MessageBox.Show(ex.Message, "No Items to Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          }

        private void btnDelete_Click(object sender, EventArgs e)
        {
              if (txtProductID.Text == "")
            {
                MessageBox.Show("Can't Delete if Product ID is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteTrail();
                    deleteRecords();                   
                }
            }
        }
        public void deleteRecords()
        {
            try
            {
                listView1.FocusedItem.Remove();
                string del = "DELETE from tblProduct where ID='" + txtProductID.Text + "'";
                cm = new SqlCommand(del, cn);
                cm.ExecuteNonQuery();

                MessageBox.Show("Successfully Deleted!");
                Clear();

            }
            catch (Exception)
            {
                MessageBox.Show("No Item to Remove", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
   

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProductID.Text = listView1.FocusedItem.Text;
            txtProductName.Text = listView1.FocusedItem.SubItems[1].Text;
            txtPrice.Text = "";
            txtPrice.Focus();
          
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getData2();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;

            lblDate.Text = time.ToString();
        }
  
        }
    }
