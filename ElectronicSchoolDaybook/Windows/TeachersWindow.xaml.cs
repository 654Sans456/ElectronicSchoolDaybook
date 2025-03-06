using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class TeachersWindow : Window
    {
        private Teacher selectedTeacher;

        public TeachersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            TeachersDataGrid.ItemsSource = DB.Context.Teachers.ToList();
        }

        private void TeachersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeacher = TeachersDataGrid.SelectedItem as Teacher;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTeacher != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить учителя: {selectedTeacher.FirstName}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Teachers.Remove(selectedTeacher);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите учителя для удаления.");
            }
        }

        private void TeachersDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Teacher teacher = e.Row.Item as Teacher;
                    if (teacher != null)
                    {
                        if (string.IsNullOrWhiteSpace(teacher.FirstName))
                        {
                            MessageBox.Show("Поле FirstName обязательна для заполнения.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(teacher.LastName))
                        {
                            MessageBox.Show("Поле LastName обязательна для заполнения.");
                            return;
                        }
                        else if (DB.Context.Users.FirstOrDefault(u => (u.ID == teacher.UserID)) == null)
                        {
                            MessageBox.Show("Поле UserID было не верно указанно.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(teacher.Department))
                        {
                            MessageBox.Show("Поле Department обязательна для заполнения.");
                            return;
                        }

                        if (DB.Context.Entry(teacher).State == EntityState.Detached)
                        {
                            DB.Context.Teachers.AddOrUpdate(teacher);
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

        private void OpenSubjectsWindow_Click(object sender, RoutedEventArgs e)
        {
            SubjectsWindow subjectsWindow = new SubjectsWindow();
            subjectsWindow.Show();
        }

        private void OpenStudentsWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            studentsWindow.Show();
        }

        private void OpenSubjectTeacherWindow_Click(object sender, RoutedEventArgs e)
        {
            SubjectTeacherWindow subjectTeacherWindow = new SubjectTeacherWindow();
            subjectTeacherWindow.Show();
        }
    }
}
