using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class ParentsWindow : Window
    {
        private Parent selectedParent;

        public ParentsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ParentsDataGrid.ItemsSource = DB.Context.Parents.ToList();
        }

        private void ParentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedParent = ParentsDataGrid.SelectedItem as Parent;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedParent != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить родителя: {selectedParent.FirstName}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Parents.Remove(selectedParent);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите родителя для удаления.");
            }
        }

        private void ParentsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Parent parent = e.Row.Item as Parent;
                    if (parent != null)
                    {
                        if (string.IsNullOrWhiteSpace(parent.FirstName))
                        {
                            MessageBox.Show("Поле FirstName обязательна для заполнения.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(parent.LastName))
                        {
                            MessageBox.Show("Поле LastName обязательна для заполнения.");
                            return;
                        }
                        else if (DB.Context.Users.FirstOrDefault(u => (u.ID == parent.UserID)) == null)
                        {
                            MessageBox.Show("Поле UserID было не верно указанно.");
                            return;
                        }

                        if (DB.Context.Entry(parent).State == EntityState.Detached)
                        {
                            DB.Context.Parents.AddOrUpdate(parent);
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

        private void OpenSubjectsWindow_Click(object sender, RoutedEventArgs e)
        {
            SubjectsWindow subjectsWindow = new SubjectsWindow();
            subjectsWindow.Show();
        }

        private void OpenTeachersWindow_Click(object sender, RoutedEventArgs e)
        {
            TeachersWindow teachersWindow = new TeachersWindow();
            teachersWindow.Show();
        }
    }
}
