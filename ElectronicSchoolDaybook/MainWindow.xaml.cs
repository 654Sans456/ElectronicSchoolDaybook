using System.Linq;
using System.Windows;

namespace ElectronicSchoolDaybook
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int UserID = Session.CurrentUser.ID;

            var isStudent = DB.Context.Students.FirstOrDefault(s => (s.UserID == UserID));
            var isTeacher = DB.Context.Teachers.FirstOrDefault(s => (s.UserID == UserID));

            string FirstNameUser = "", LastNameUser = "";

            if (isStudent != null)
            {
                FirstNameUser = isStudent.FirstName;
                LastNameUser = isStudent.LastName;
            }
            else if (isTeacher != null)
            {
                FirstNameUser = isTeacher.FirstName;
                LastNameUser = isTeacher.LastName;
            }

            welcomeLabel.Content = "Добро пожаловать, " + LastNameUser + " " + FirstNameUser + "!";
        }
    }
}
