using System.Linq;
using System.Windows;

namespace ElectronicSchoolDaybook
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var isUsername = DB.Context.Users.FirstOrDefault(u => (u.Username == username));
            var isPassword = DB.Context.Users.FirstOrDefault(u => (u.Password == password));

            if ((isUsername != null) && (isPassword != null))
            {
                Session.CurrentUser = DB.Context.Users.FirstOrDefault(u => ((u.Username == username) && (u.Password == password)));

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();
            }
            else if (isUsername == null)
            {
                MessageBox.Show("Неверный логин!");
            }
            else if (isPassword == null)
            {
                MessageBox.Show("Неверный пароль!");
            }
        }
    }
}
