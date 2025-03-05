using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class StudentsWindow : Window
    {
        private Student selectedStudent;

        public StudentsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            StudentsDataGrid.ItemsSource = DB.Context.Students.ToList();
        }

        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudent = StudentsDataGrid.SelectedItem as Student;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить студента: {selectedStudent.FirstName}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Students.Remove(selectedStudent);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите студента для удаления.");
            }
        }

        private void StudentsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Student Student = e.Row.Item as Student;
                    if (Student != null)
                    {
                        if (string.IsNullOrWhiteSpace(Student.FirstName))
                        {
                            MessageBox.Show("Поле FirstName обязательна для заполнения.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(Student.LastName))
                        {
                            MessageBox.Show("Поле LastName обязательна для заполнения.");
                            return;
                        }
                        else if (DB.Context.Users.FirstOrDefault(u => (u.ID == Student.UserID)) == null)
                        {
                            MessageBox.Show("Поле UserID было не верно указанно.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(Student.Class))
                        {
                            MessageBox.Show("Поле Class обязательна для заполнения.");
                            return;
                        }

                        if (DB.Context.Entry(Student).State == EntityState.Detached)
                        {
                            DB.Context.Students.AddOrUpdate(Student);
                        }

                        DB.Context.SaveChanges();
                        LoadData();
                    }
                });
            }
        }

        private void OpenRolesWindow_Click(object sender, RoutedEventArgs e)
        {
            RolesWindow rolesWindow = new RolesWindow();
            rolesWindow.Show();
        }

        private void OpenUsersWindow_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.Show();
        }

        private void OpenParentsWindow_Click(object sender, RoutedEventArgs e)
        {
            ParentsWindow parentsWindow = new ParentsWindow();
            parentsWindow.Show();
        }

        private void OpenStudentParentWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentParentWindow studentParentWindow = new StudentParentWindow();
            studentParentWindow.Show();
        }
    }
}
