using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
            DisplayCustomer();
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            DisplayCustomer();
            CusDateDtp.ValueChanged += CusDateDtp_ValueChanged;
        }

        private void DisplayCustomer()
        {
            string query = "SELECT * FROM [Customer]";
            CustomerDgv.DataSource = Connection.ExecuteQuery(query);
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (ValidateCustomerForm())
            {
                string checkQuery = "SELECT COUNT(*) FROM [Customer] WHERE CusId = @ID";
                SqlParameter[] checkParameters = { new SqlParameter("@ID", CusIdTb.Text) };
                int count = Convert.ToInt32(Connection.ExecuteScalar(checkQuery, checkParameters));

                if (count > 0)
                {
                    MessageBox.Show("ID pelanggan sudah ada.");
                    return;
                }

                string insertQuery = "INSERT INTO [Customer] (CusId, CusName, CusDate, CusPhone) VALUES (@ID, @Name, @CusDate, @Phone)";
                SqlParameter[] insertParameters = {
                    new SqlParameter("@ID", CusIdTb.Text),
                    new SqlParameter("@Name", CusNameTb.Text),
                    new SqlParameter("@CusDate", CusDateDtp.Value),
                    new SqlParameter("@Phone", CusPhoneTb.Text)
                };

                if (Connection.ExecuteNonQuery(insertQuery, insertParameters) > 0)
                {
                    
                    MessageBox.Show("Data berhasil ditambahkan!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayCustomer();
                    ResetBtn_Click(null, null);
                }
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (ValidateCustomerForm())
            {
                string updateQuery = "UPDATE [Customer] SET CusName = @Name, CusPhone = @Phone, CusDate = @CusDate WHERE CusId = @ID";
                SqlParameter[] updateParameters = {
                    new SqlParameter("@ID", CusIdTb.Text),
                    new SqlParameter("@Name", CusNameTb.Text),
                    new SqlParameter("@CusDate", CusDateDtp.Value),
                    new SqlParameter("@Phone", CusPhoneTb.Text)
                };

                if (Connection.ExecuteNonQuery(updateQuery, updateParameters) > 0)
                {
                    
                    MessageBox.Show("Data berhasil diperbarui!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayCustomer();
                    ResetBtn_Click(null, null);
                }
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CusIdTb.Text))
            {
                DialogResult result = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM [Customer] WHERE CusId = @ID";
                    SqlParameter[] deleteParameters = { new SqlParameter("@ID", CusIdTb.Text) };
                    if (Connection.ExecuteNonQuery(deleteQuery, deleteParameters) > 0)
                    {
                       
                        MessageBox.Show("Data berhasil dihapus!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayCustomer();
                        ResetBtn_Click(null, null);
                    }
                }
            }
            else
            {
                MessageBox.Show("ID pelanggan harus diisi.");
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            CusIdTb.Text = "";
            CusNameTb.Text = "";
            CusPhoneTb.Text = "";
            CusDateDtp.Value = DateTime.Now;
            DisplayCustomer();
        }

        private void CustomerDgv_DoubleClick(object sender, EventArgs e)
        {
            if (CustomerDgv.CurrentRow != null && CustomerDgv.CurrentRow.Index != -1)
            {
                CusIdTb.Text = CustomerDgv.CurrentRow.Cells["CusId"].Value.ToString();
                CusNameTb.Text = CustomerDgv.CurrentRow.Cells["CusName"].Value.ToString();
                CusPhoneTb.Text = CustomerDgv.CurrentRow.Cells["CusPhone"].Value.ToString();
                CusDateDtp.Value = Convert.ToDateTime(CustomerDgv.CurrentRow.Cells["CusDate"].Value);
            }
        }

        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Productlbl_Click(object sender, EventArgs e)
        {
            Product obj = new Product();
            obj.Show();
            this.Hide();
        }

        private void Billlbl_Click(object sender, EventArgs e)
        {
            Bill obj = new Bill();
            obj.Show();
            this.Hide();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (Search.Text == "")
            {
                DisplayCustomer();
                return;
            }

            try
            {
                string query = "SELECT * FROM [Customer] WHERE CusId LIKE @search OR CusName LIKE @search";
                SqlParameter[] parameters = { new SqlParameter("@search", "%" + Search.Text + "%") };
                CustomerDgv.DataSource = Connection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mencari data: " + ex.Message);
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(sender, e);
        }

        private void CusDateDtp_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM [Customer] WHERE CAST(CusDate AS DATE) = @SelectedDate";
                SqlParameter[] parameters = { new SqlParameter("@SelectedDate", CusDateDtp.Value.Date) };
                var result = Connection.ExecuteQuery(query, parameters);
                if (result.Rows.Count > 0)
                    CustomerDgv.DataSource = result;
                else
                    MessageBox.Show("Tidak ada data untuk tanggal ini.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memfilter data berdasarkan tanggal: " + ex.Message);
            }
        }

        private bool ValidateCustomerForm()
        {
            if (string.IsNullOrEmpty(CusIdTb.Text) || string.IsNullOrEmpty(CusNameTb.Text) || string.IsNullOrEmpty(CusPhoneTb.Text))
            {
                MessageBox.Show("Semua kolom harus diisi.");
                return false;
            }
            return true;
        }
    }
}
