using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class SubjectTeacherWindow : Window
    {
        private SubjectTeacher selectedSubjectTeacher;

        public SubjectTeacherWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            SubjectTeacherDataGrid.ItemsSource = DB.Context.SubjectTeachers.ToList();
        }

        private void SubjectTeacherDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSubjectTeacher = SubjectTeacherDataGrid.SelectedItem as SubjectTeacher;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSubjectTeacher != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить связь учителя и их предета под номером: {selectedSubjectTeacher.ID}?",
                                              "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.SubjectTeachers.Remove(selectedSubjectTeacher);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите связь учителя и их предета для удаления.");
            }
        }

        private void SubjectTeacherDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    SubjectTeacher subjectTeacher = e.Row.Item as SubjectTeacher;
                    if (subjectTeacher != null)
                    {
                        if (DB.Context.Subjects.FirstOrDefault(s => (s.ID == subjectTeacher.SubjectID)) == null)
                        {
                            MessageBox.Show("Поле SubjectID было не верно указанно.");
                            return;
                        }
                        else if (DB.Context.Teachers.FirstOrDefault(t => (t.ID == subjectTeacher.TeacherID)) == null)
                        {
                            MessageBox.Show("Поле TeacherID было не верно указанно.");
                            return;
                        }

                        if (DB.Context.Entry(subjectTeacher).State == EntityState.Detached)
                        {
                            DB.Context.SubjectTeachers.AddOrUpdate(subjectTeacher);
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

        private void OpenStudentParentWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentParentWindow studentParentWindow = new StudentParentWindow();
            studentParentWindow.Show();
        }
    }
}
