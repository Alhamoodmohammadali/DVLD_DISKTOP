using DVLD_Buisness;
using System;
using System.Windows.Forms;
using DVLD.Global_Classes;
using static DVLD.Classes.clsGlobal;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(),Securty.ComputeHash(txtPassword.Text.Trim()));
            if (user != null)
            {
                if (chkRememberMe.Checked)
                    RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                else
                    RememberUsernameAndPassword(null, null);
                //incase the user is not active
                if (!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CurrentUser = user;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = null, Password = null;
            if (GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }
    }
}
