using System.ComponentModel;
using System.Windows;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.View;

namespace Me.App.Form
{
    /// <summary>
    /// Interaction logic for FrmLogin.xaml
    /// </summary>
    public partial class FrmLogin : Window
    {
        private readonly AuthenticationRepository _authentication = new AuthenticationRepository();

        public FrmLogin()
        {
            InitializeComponent();

        }

        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                lblError.Content = "Please input Username and Password";
                ClearCheckNull.TextBox(this);
            }
            else
            {


                var pass = await _authentication.UserLogin(txtUserName.Text, txtPassword.Password);
                if (pass == null)
                {
                    lblError.Content = "Invalid input";
                }
                else
                {
                    CreateHelper<AuthenticationView> ch = new CreateHelper<AuthenticationView>();

                    ch.CreateFolder();
                    ch.CleanDirectory();
                    ch.WriteJson(pass, "user");

                    var main = new FrmMain();
                    main.Show();
                    this.Hide();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = false;
            Application.Current.Shutdown();
        }
    }
}
