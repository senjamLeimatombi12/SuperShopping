using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperShopping
{
    public partial class FormProduct : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormProduct()
        {
            InitializeComponent();
        }

        private void FormProduct_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'onLineShoppingDataSet10.tblCategory' table. You can move, or remove it, as needed.
            this.tblCategoryTableAdapter1.Fill(this.onLineShoppingDataSet10.tblCategory);
            // TODO: This line of code loads data into the 'onLineShoppingDataSet9.spGetCategory' table. You can move, or remove it, as needed.
            this.spGetCategoryTableAdapter.Fill(this.onLineShoppingDataSet9.spGetCategory);
            // TODO: This line of code loads data into the 'onLineShoppingDataSet3.tblCategory' table. You can move, or remove it, as needed.


            BindCategory();
            BindProductList();
            lblProdID.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            btnAdd.Visible = true;
            SearcgBy_Category();

            // Ensure auto-generating columns
            dataGridView1.AutoGenerateColumns = true;


        }

        private void BindCategory()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SearcgBy_Category()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbCon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbsearch.DataSource = dt;
            cmbsearch.DisplayMember = "CategoryName";
            cmbsearch.ValueMember = "CatID";
            dbCon.CloseCon();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;
                }
                else if (Convert.ToInt32(txtPrice.Text) < 0 || txtPrice.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter valid price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQty.Text == String.Empty || Convert.ToInt32(txtQty.Text) < 0)
                {
                    MessageBox.Show("Please Enter valid Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spInsertProduct", dbCon.GetCon());
                        //cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                        cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                        cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));
                       

                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                        }
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtClear()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblProdID.Text == "" && txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter ProductID and name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;
                }
                else if (txtPrice.Text == String.Empty && Convert.ToInt32(txtPrice.Text) >= 0)
                {
                    MessageBox.Show("Please Enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQty.Text == String.Empty && Convert.ToInt32(txtQty.Text) >= 0)
                {
                    MessageBox.Show("Please Enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)


                    {
                        SqlCommand cmd1 = new SqlCommand("spUpdateProduct", dbCon.GetCon());
                        cmd1.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                        cmd1.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                        cmd1.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                        cmd1.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                        cmd1.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));

                        cmd1.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd1.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            lblProdID.Visible = true;
                            btnAdd.Visible = true;
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Updation Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    dbCon.CloseCon();
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

     
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (lblProdID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Product ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (lblProdID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spDeleteProduct", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProdID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            btnUpdate.Visible = true;
                            btnDelete.Visible = true;
                            btnAdd.Visible = true;
                            lblProdID.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Delete failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                        dbCon.CloseCon();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        
        private void Searched_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_SearchByCat", dbCon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbsearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
               
                lblProdID.Text = row.Cells[0].Value?.ToString();
                txtProdName.Text = row.Cells[1].Value?.ToString();
                cmbCategory.SelectedValue = row.Cells[2].Value?.ToString();
                txtPrice.Text = row.Cells[3].Value?.ToString();
                txtQty.Text = row.Cells[4].Value?.ToString();
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            Searched_ProductList();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BindProductList();
        }

        private void cmbsearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CausesValidationChanged(object sender, EventArgs e)
        {

        }
    }
}
