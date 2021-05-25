using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.Models;

namespace SZMK.Desktop.Views.PDO
{
    public partial class PDO_ChangeOrder_F : Form
    {
        public PDO_ChangeOrder_F()
        {
            InitializeComponent();
        }
        private void PDOChangeOrder_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    if (Comment_CB.Checked)
                    {
                        if (String.IsNullOrWhiteSpace(Comment_TB.Text))
                        {
                            Comment_TB.Focus();
                            throw new Exception("Необходимо указать комментарий");
                        }
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }
        private void Comment_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!Comment_CB.Checked)
            {
                Comment_TB.Enabled = false;
            }
            else
            {
                Comment_TB.Enabled = true;
            }
        }

        private void HideRewrite_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!HideRewrite_CB.Checked)
            {
                Hide_CB.Enabled = false;
            }
            else
            {
                Hide_CB.Enabled = true;
            }
        }
    }
}
