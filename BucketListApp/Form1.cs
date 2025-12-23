using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BucketListApp
{
    public partial class Form1 : Form
    {
        string dbPath = "Data Source=bucketlist.db";

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var con = new SQLiteConnection(dbPath))
            {
                con.Open();
                string tableQuery = @"CREATE TABLE IF NOT EXISTS BucketList (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Dream TEXT,
                                        Category TEXT)";
                SQLiteCommand cmd = new SQLiteCommand(tableQuery, con);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDream.Text == "" || txtCategory.Text == "")
            {
                MessageBox.Show("Please enter both Dream and Category!");
                return;
            }

            using (var con = new SQLiteConnection(dbPath))
            {
                con.Open();
                string insertQuery = "INSERT INTO BucketList (Dream, Category) VALUES (@dream, @category)";
                SQLiteCommand cmd = new SQLiteCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@dream", txtDream.Text);
                cmd.Parameters.AddWithValue("@category", txtCategory.Text);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Dream added successfully!");
            txtDream.Clear();
            txtCategory.Clear();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var con = new SQLiteConnection(dbPath))
            {
                con.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * FROM BucketList", con);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvList.DataSource = dt;
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                txtDream.Text = dgvList.SelectedRows[0].Cells["Dream"].Value.ToString();
                txtCategory.Text = dgvList.SelectedRows[0].Cells["Category"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvList.SelectedRows[0].Cells["Id"].Value);
                using (var con = new SQLiteConnection(dbPath))
                {
                    con.Open();
                    string updateQuery = "UPDATE BucketList SET Dream=@dream, Category=@category WHERE Id=@id";
                    SQLiteCommand cmd = new SQLiteCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@dream", txtDream.Text);
                    cmd.Parameters.AddWithValue("@category", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Dream updated successfully!");
                txtDream.Clear();
                txtCategory.Clear();
            }
            else
            {
                MessageBox.Show("Please select a row to update!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvList.SelectedRows[0].Cells["Id"].Value);
                using (var con = new SQLiteConnection(dbPath))
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM BucketList WHERE Id=@id";
                    SQLiteCommand cmd = new SQLiteCommand(deleteQuery, con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Dream deleted successfully!");
            }
            else
            {
                MessageBox.Show("Please select a row to delete!");
            }
        }
    }
}