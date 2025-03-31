using System.Windows;

namespace ElectronicSchoolDaybook
{
    public partial class LoginTestWindow : Window
    {
        public LoginTestWindow()
        {
            InitializeComponent();
        }

        private void btnTestSuccess_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.txtUsername.Text = "admin";
            loginWindow.txtPassword.Password = "admin";

            loginWindow.btnLogin_Click(sender, e);
        }

        private void btnTestFail_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.txtUsername.Text = "login";
            loginWindow.txtPassword.Password = "password";

            loginWindow.btnLogin_Click(sender, e);
        }

        private void btnTestFailLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.txtUsername.Text = "login";
            loginWindow.txtPassword.Password = "admin";

            loginWindow.btnLogin_Click(sender, e);
        }

        private void btnTestFailPassword_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.txtUsername.Text = "admin";
            loginWindow.txtPassword.Password = "password";

            loginWindow.btnLogin_Click(sender, e);
        }

        private void btnTestEmpty_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.txtUsername.Text = "";
            loginWindow.txtPassword.Password = "";

            loginWindow.btnLogin_Click(sender, e);
        }
    }
}
