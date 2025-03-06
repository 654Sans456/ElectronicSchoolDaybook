using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class SubjectsWindow : Window
    {
        private Role selectedSubject;

        public SubjectsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            SubjectsDataGrid.ItemsSource = DB.Context.Subjects.ToList();
        }

        private void SubjectsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSubject = SubjectsDataGrid.SelectedItem as Role;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSubject != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить предмет: {selectedSubject.Name}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Roles.Remove(selectedSubject);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите предмет для удаления.");
            }
        }

        private void SubjectsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Subject subject = e.Row.Item as Subject;
                    if (subject != null)
                    {
                        if (string.IsNullOrWhiteSpace(subject.Name))
                        {
                            MessageBox.Show("Поле Name обязательна для заполнения.");
                            return;
                        }

                        if (DB.Context.Entry(subject).State == EntityState.Detached)
                        {
                            DB.Context.Subjects.AddOrUpdate(subject);
                        }

                        DB.Context.SaveChanges();
                        LoadData();
                    }
                });
            }
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

        private void OpenStudentsWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            studentsWindow.Show();
        }

        private void OpenStudentParentWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentParentWindow studentParentWindow = new StudentParentWindow();
            studentParentWindow.Show();
        }

        private void OpenRolesWindow_Click(object sender, RoutedEventArgs e)
        {
            RolesWindow rolesWindow = new RolesWindow();
            rolesWindow.Show();
        }
    }
}
