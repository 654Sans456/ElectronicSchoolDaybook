﻿using ElectronicSchoolDaybook.Windows;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicSchoolDaybook
{
    public partial class UsersWindow : Window
    {
        private User selectedUser;

        public UsersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            UsersDataGrid.ItemsSource = DB.Context.Users.ToList();
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersDataGrid.SelectedItem as User;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя: {selectedUser.Username}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Users.Remove(selectedUser);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
            }
        }

        private void UsersDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    User user = e.Row.Item as User;
                    if (user != null)
                    {
                        if (string.IsNullOrWhiteSpace(user.Username))
                        {
                            MessageBox.Show("Поле Username обязательна для заполнения.");
                            return;
                        }
                        else if (string.IsNullOrWhiteSpace(user.Password))
                        {
                            MessageBox.Show("Поле Password обязательна для заполнения.");
                            return;
                        }
                        else if (DB.Context.Roles.FirstOrDefault(r => (r.ID == user.RoleID)) == null)
                        {
                            MessageBox.Show("Поле RoleID было не верно указанно.");
                            return;
                        }

                        if (DB.Context.Entry(user).State == EntityState.Detached)
                        {
                            DB.Context.Users.AddOrUpdate(user);
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

        private void OpenGradesWindow_Click(object sender, RoutedEventArgs e)
        {
            GradesWindow gradesWindow = new GradesWindow();
            gradesWindow.Show();
        }
    }
}
