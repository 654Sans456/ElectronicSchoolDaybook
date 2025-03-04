using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ElectronicSchoolDaybook
{
    public partial class RolesWindow : Window
    {
        private Role selectedRole;

        public RolesWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            RolesDataGrid.ItemsSource = DB.Context.Roles.ToList();
        }

        private void RolesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRole = RolesDataGrid.SelectedItem as Role;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRole != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить роль: {selectedRole.Name}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.Context.Roles.Remove(selectedRole);
                    DB.Context.SaveChanges();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите роль для удаления.");
            }
        }

        private void RolesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Role role = e.Row.Item as Role;
                    if (role != null)
                    {
                        if (string.IsNullOrWhiteSpace(role.Name))
                        {
                            MessageBox.Show("Поле Name обязательна для заполнения.");
                            return;
                        }

                        if (DB.Context.Entry(role).State == EntityState.Detached)
                        {
                            DB.Context.Roles.AddOrUpdate(role);
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
    }
}
