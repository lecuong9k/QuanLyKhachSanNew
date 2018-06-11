using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLKhoHang
{
    public partial class QLKhoHang : Form
    {
        public QLKhoHang()
        {
            InitializeComponent();
        }

        string strconn = @"Data Source=DESKTOP-JDF49AU\SQLEXPRESS;Initial Catalog=QLKho;Integrated Security=True";
        SqlConnection conn;

        public void LoadData()
        {
            SqlDataAdapter da = new SqlDataAdapter("select*from KhoHang", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void QLKhoHang_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strconn);
            conn.Open();
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtMaKho.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clMaKho"].Value);
                txtTenKho.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clTenKho"].Value);
                txtDiaChi.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clDiaChi"].Value);
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells["clSTT"].Value = e.RowIndex + 1;
        }

        private void bntThem_Click(object sender, EventArgs e)
        {
            txtMaKho.Enabled = false;
            txtTenKho.Text = "";
            txtDiaChi.Text = "";
            bntLuu.Enabled = true;
        }

        private void bntLuu_Click(object sender, EventArgs e)
        {
            int count1 = 0;
            count1 = dataGridView1.Rows.Count;
            string c1 = "";
            int c2 = 0;
            c1 = Convert.ToString(dataGridView1.Rows[count1 - 2].Cells[1].Value);
            c2 = Convert.ToInt32((c1.Remove(0, 3)));
            if (c2 + 1 < 10)
            {
                txtMaKho.Text = "K110" + (c2 + 1).ToString();
            }
            else if (c2 + 1 < 100)
            {
                txtMaKho.Text = "K11" + (c2 + 1).ToString();
            }

            if (txtTenKho.Text.Trim() == "")
            {
                MessageBox.Show("Hãy nhập tên kho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActiveControl = txtTenKho;
                return;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("Hãy nhập địa chỉ của kho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActiveControl = txtDiaChi;
                return;
            }

            SqlCommand cmd = new SqlCommand("ThemKH", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p = new SqlParameter("@MaKho", txtMaKho.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@TenKho", txtTenKho.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@DiaChi", txtDiaChi.Text);
            cmd.Parameters.Add(p);

            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                MessageBox.Show("Đã thêm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Không thể thêm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            bntLuu.Enabled = false;
        }

        private void bntSua_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SuaKH", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p = new SqlParameter("@MaKho", txtMaKho.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@TenKho", txtTenKho.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@DiaChi", txtDiaChi.Text);
            cmd.Parameters.Add(p);

            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                MessageBox.Show("Đã sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
                MessageBox.Show("Không thể sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bntXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn xóa thông tin này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("XoaKH", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter p = new SqlParameter("@MaKho", txtMaKho.Text);
                cmd.Parameters.Add(p);

                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                    MessageBox.Show("Không thể xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txtMaKho.Text = "";
            txtTenKho.Text = "";
            txtDiaChi.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from KhoHang where TenKho like '" + "%" + txtSearch.Text + "%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }
    }
}
