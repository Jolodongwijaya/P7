using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemManajemenTokoEmas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();


            PasswordTb.UseSystemPasswordChar = true;
        }

        
        private void CrossBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            UNameTb.Text = "";
            PasswordTb.Text = "";
        }

        
        private void LogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (UNameTb.Text.Trim() == "" || PasswordTb.Text.Trim() == "")
                {
                    MessageBox.Show("Nama pengguna atau kata sandi tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                else if (UNameTb.Text == "Admin" && PasswordTb.Text == "Admin")
                {
                    MessageBox.Show("Login Berhasil!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Customer obj = new Customer(); 
                    obj.Show();
                    this.Hide();
                }
                
                else
                {
                    MessageBox.Show("Tolong masukkan nama dan password yang benar.", "Gagal Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTb.UseSystemPasswordChar = !checkBox1.Checked;
        }

        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
