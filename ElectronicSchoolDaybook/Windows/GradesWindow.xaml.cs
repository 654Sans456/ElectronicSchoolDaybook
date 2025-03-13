using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook.Windows
{
    public partial class GradesWindow : Window
    {
        private Grade selectedGrade;
        private bool isSearch = false;

        public GradesWindow()
        {
            InitializeComponent();
            LoadData();
            LoadSubject();
        }

        private void LoadData()
        {
            GradesDataGrid.ItemsSource = DB.Context.Grades.ToList();
        }

        private void LoadSubject()
        {
            var subjects = DB.Context.Subjects.ToList();
            subjects.Insert(0, new Subject { ID = 0, Name = "<выбор предмета>" });
            ComboBoxWithSubjects.ItemsSource = subjects;
            ComboBoxWithSubjects.SelectedIndex = 0;
        }

        private void GradesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGrade = GradesDataGrid.SelectedItem as Grade;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGrade != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить оценку под номером: {selectedGrade.ID}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Grades.Remove(selectedGrade);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите оценку для удаления.");
            }
        }

        private void GradesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Grade grade = e.Row.Item as Grade;
                    if (grade != null)
                    {
                        DateTime dataStart = new DateTime(2020, 3, 8);
                        DateTime dataEnd = DateTime.Now;

                        if ((grade.Grade1 > 5) || (grade.Grade1 < 1))
                        {
                            MessageBox.Show("Поле Grade можно указать от 1 до 5.");
                            return;
                        }
                        else if (grade.Date < dataStart)
                        {
                            MessageBox.Show("В поле Date указана маленькая дата.");
                            return;
                        }
                        else if (grade.Date > dataEnd)
                        {
                            MessageBox.Show("В поле Date указана большая дата.");
                            return;
                        }
                        else if (DB.Context.Students.FirstOrDefault(s => (s.ID == grade.StudentID)) == null)
                        {
                            MessageBox.Show("Поле StudentID было не верно указанно.");
                            return;
                        }
                        else if (DB.Context.SubjectTeachers.FirstOrDefault(st => (st.ID == grade.SubjectTeacherID)) == null)
                        {
                            MessageBox.Show("Поле SubjectTeacherID было не верно указанно.");
                            return;
                        }

                        if (DB.Context.Entry(grade).State == EntityState.Detached)
                        {
                            DB.Context.Grades.AddOrUpdate(grade);
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

        private void OpenTeachersWindow_Click(object sender, RoutedEventArgs e)
        {
            TeachersWindow teachersWindow = new TeachersWindow();
            teachersWindow.Show();
        }

        private void OpenSubjectTeacherWindow_Click(object sender, RoutedEventArgs e)
        {
            SubjectTeacherWindow subjectTeacherWindow = new SubjectTeacherWindow();
            subjectTeacherWindow.Show();
        }

        private void OpenStudentsWindow_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            studentsWindow.Show();
        }

        public void SliderMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int number = (int)SliderMin.Value;
            LaberGradeMin.Content = "Минимальная оценка: " + number.ToString();
        }

        public void SliderMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int number = (int)SliderMax.Value;
            LaberGradeMax.Content = "Максимальная оценка: " + number.ToString();
        }

        private void SearchStudents_Click(object sender, RoutedEventArgs e)
        {
            int selectedSubjectID = (int)ComboBoxWithSubjects.SelectedValue;

            if (!isSearch && (selectedSubjectID == 0))
            {
                GradesDataGrid.ItemsSource = DB.Context.Grades.Include(g => g.SubjectTeacher)
                    .Where(g => g.Grade1 >= (int)SliderMin.Value)
                    .Where(g => g.Grade1 <= (int)SliderMax.Value)
                    .ToList();

                SearchButton.Content = "Сбросить";
            }
            else if (!isSearch)
            {
                GradesDataGrid.ItemsSource = DB.Context.Grades.Include(g => g.SubjectTeacher)
                    .Where(g => g.Grade1 >= (int)SliderMin.Value)
                    .Where(g => g.Grade1 <= (int)SliderMax.Value)
                    .Where(g => g.SubjectTeacherID == DB.Context.SubjectTeachers.FirstOrDefault(st => st.ID == g.SubjectTeacherID).ID)
                    .Where(g => DB.Context.SubjectTeachers.FirstOrDefault(st => st.ID == g.SubjectTeacherID).SubjectID == selectedSubjectID)
                    .ToList();

                SearchButton.Content = "Сбросить";
            }
            else
            {
                LoadData();
                SearchButton.Content = "Поиск";
            }

            isSearch = (isSearch == false);
        }
    }
}
