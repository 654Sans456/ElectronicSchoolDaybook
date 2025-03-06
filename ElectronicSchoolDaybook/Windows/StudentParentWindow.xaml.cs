using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{public partial class StudentParentWindow : Window
    {
        private StudentParent selectedStudentParent;

        public StudentParentWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            StudentParentDataGrid.ItemsSource = DB.Context.StudentParents.ToList();
        }

        private void StudentParentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudentParent = StudentParentDataGrid.SelectedItem as StudentParent;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudentParent != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить связь родителья и студента школы под номером: {selectedStudentParent.ID}?",
                                              "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.StudentParents.Remove(selectedStudentParent);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите связь родителья и студента школы для удаления.");
            }
        }

        private void StudentParentDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    StudentParent studentParent = e.Row.Item as StudentParent;
                    if (studentParent != null)
                    {
                        if (DB.Context.Students.FirstOrDefault(s => (s.ID == studentParent.StudentID)) == null)
                        {
                            MessageBox.Show("Поле StudentID было не верно указанно.");
                            return;
                        }
                        else if (DB.Context.Parents.FirstOrDefault(p => (p.ID == studentParent.ParentID)) == null)
                        {
                            MessageBox.Show("Поле ParentID было не верно указанно.");
                            return;
                        }

                        if (DB.Context.Entry(studentParent).State == EntityState.Detached)
                        {
                            DB.Context.StudentParents.AddOrUpdate(studentParent);
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

        private void OpenUsersWindow_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.Show();
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
