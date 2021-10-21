using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для pageAdminMenuV2.xaml
    /// </summary>
    public partial class pageAdminMenuV2 : Page
    {
        public List<auth> Userdata;
        public pageAdminMenuV2()
        {
            InitializeComponent();
            Userdata = DB.DataBase.auth.ToList();
            listBoxInfoUsers.ItemsSource = Userdata;
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox senderBox = (ListBox)sender;
            int index = Convert.ToInt32(senderBox.Uid);
            
            senderBox.ItemsSource = DB.DataBase.users_to_traits.Where(x => x.id_user == index).ToList();
            senderBox.DisplayMemberPath = "traits.trait";
        }

        private void GroupBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GroupBox senderGroupBox = (GroupBox)sender;
            int index = Convert.ToInt32(senderGroupBox.Uid);
            auth SelectedUser = DB.DataBase.auth.FirstOrDefault(x => x.id == index);
            User.mainFrame.Navigate(new pageAdminEditUsersData(SelectedUser));
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя из системы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    auth SelectedUser = (auth)listBoxInfoUsers.SelectedItem;
                    DB.DataBase.auth.Remove(SelectedUser);
                    DB.DataBase.SaveChanges();
                    MessageBox.Show("Пользователь успешно удалён!");
                    listBoxInfoUsers.ItemsSource = DB.DataBase.auth.ToList();
                }
            }
            catch 
            {
                MessageBox.Show("Ошибка при удалении пользователя! Убедитесь, что Вы выбрали пользователя системы, затем повторите попытку.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<auth> SelectedUsers = Userdata.Skip(Convert.ToInt32(txtOT.Text) - 1).Take(Convert.ToInt32(txtDO.Text) + 1 - Convert.ToInt32(txtOT.Text)).ToList();
                listBoxInfoUsers.ItemsSource = SelectedUsers;
            }
            catch
            {
                MessageBox.Show("Введите интервал выбора пользователей!", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            listBoxInfoUsers.ItemsSource = DB.DataBase.auth.ToList();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.Navigate(new pageAdminEditUsersData(new auth()));
        }
    }
}
