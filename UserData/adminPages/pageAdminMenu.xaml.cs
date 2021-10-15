using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserData.adminPages
{
    /// <summary>
    /// Логика взаимодействия для pageAdminMenu.xaml
    /// </summary>
    public partial class pageAdminMenu : Page
    {

        public async void Load()
        {
            while (true)
            {
                usersData.ItemsSource = DB.DataBase.auth.ToList();
                await Task.Delay(1000);
            }
        }
       

        public pageAdminMenu()
        {
            InitializeComponent();
            usersData.ItemsSource = DB.DataBase.auth.ToList();
            Load();
        }


        private auth SelectedUser()
        {
            return (auth)usersData.SelectedItem;
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.Navigate(new pageAdminEditUsersData(SelectedUser()));
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя из системы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DB.DataBase.auth.Remove(SelectedUser());
                DB.DataBase.SaveChanges();
                MessageBox.Show("Пользователь успешно удалён!");
                usersData.ItemsSource = DB.DataBase.auth.ToList();
            }
        }
    }
}
