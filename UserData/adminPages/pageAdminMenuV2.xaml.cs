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
using Library;

namespace UserData.adminPages
{
    /// <summary>
    /// Логика взаимодействия для pageAdminMenuV2.xaml
    /// </summary>
    public partial class pageAdminMenuV2 : Page
    {


        public List<auth> Userdata;
        List<auth> FilterListUsers;
        int currentPage = 0;
        double countInPage = 2;
        int countAllPages = 0;


        public pageAdminMenuV2()
        {
            InitializeComponent();
            FilterListUsers = DB.DataBase.auth.ToList();
            listBoxInfoUsers.ItemsSource = FilterListUsers;

            //Заполнение выподающего списка гендеров
            txtSearchGender.ItemsSource = DB.DataBase.genders.ToList();
            txtSearchGender.SelectedValuePath = "id";
            txtSearchGender.DisplayMemberPath = "gender";

            txtAgeAVG.Text = $"Среднее количество лет всех пользователей системы: {Math.Round(AgeAVG(FilterListUsers), 2)}";



            countAllPages = (int)Math.Ceiling(Convert.ToDouble(FilterListUsers.Count / countInPage)); //Определение количества страниц
            txtAllPages.Text = Convert.ToString(countAllPages);
            Pagination("Previous");//Создание пагинирующей страницы
        }


        private double AgeAVG(List<auth> Userdata)
        {
            List<DateTime> dateBirth = new List<DateTime>();
            foreach (auth user in Userdata)
            {
                if (user.users != null)
                {
                    dateBirth.Add(user.users.dr);
                }
            }
            return Func.AgeAVG(dateBirth);
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
                List<auth> SelectedUsers = FilterListUsers.Skip(Convert.ToInt32(txtOT.Text) - 1).Take(Convert.ToInt32(txtDO.Text) + 1 - Convert.ToInt32(txtOT.Text)).ToList();
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
            txtSearchLine.Text = "";
            txtSearchGender.SelectedIndex = -1;
            FilterListUsers = DB.DataBase.auth.ToList();
            txtOT.Text = "";
            txtDO.Text = "";
            currentPage = 0;
            Pagination("Previous");
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.Navigate(new pageAdminEditUsersData(new auth()));
        }



        private void Filter(object sender, RoutedEventArgs e)
        {
            //Создание списка для изменения содержимого по фильтру
            FilterListUsers = DB.DataBase.auth.ToList();

            //Выбор пользователей по имени или его части через DLL библиотеку
            if (txtSearchLine.Text != "")
            {
                List<int> Id = new List<int>();
                List<string> Name = new List<string>();
                foreach (auth user in FilterListUsers)
                {
                    if (user.users != null)
                    {
                        Id.Add(user.id);
                        Name.Add(user.users.name);
                    }
                }
                Id = Func.SelectUsersByName(Id, Name, txtSearchLine.Text);
                List<auth> UsersBySearchLine = new List<auth>();
                for (int i = 0; i < Id.Count; i++)
                {
                    int id = Id[i];
                    UsersBySearchLine.Add(DB.DataBase.auth.FirstOrDefault(x => x.id == id));
                }
                FilterListUsers = FilterListUsers.Intersect(UsersBySearchLine).ToList();
                listBoxInfoUsers.ItemsSource = FilterListUsers;
            }

            if (txtSearchGender.SelectedIndex != -1)
            {
                int id = Convert.ToInt32(txtSearchGender.SelectedValue);
                FilterListUsers = FilterListUsers.Where(x => x.users != null && x.users.genders.id == id).ToList();
                listBoxInfoUsers.ItemsSource = FilterListUsers;
            }

            if (txtOT.Text != "" && txtDO.Text != "")
            {
                try
                {
                    FilterListUsers = FilterListUsers.Skip(Convert.ToInt32(txtOT.Text) - 1).Take(Convert.ToInt32(txtDO.Text) + 1 - Convert.ToInt32(txtOT.Text)).ToList();
                    listBoxInfoUsers.ItemsSource = FilterListUsers;
                }
                catch
                {
                    MessageBox.Show("Введите интервал выбора пользователей!", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }


            
            
            currentPage = 0; //Установка первой страницы для обновлённого списка
            double pages = Convert.ToDouble(FilterListUsers.Count / countInPage);
            countAllPages = (int)Math.Ceiling(pages); ;//Определяем количество страниц у обновлённого списка
            txtAllPages.Text = Convert.ToString(countAllPages);
            Pagination("Previous"); //Переход на первую страницу и её отображение
        }

   
        private void Pagination(string Uid)
        {
            switch (Uid)
            {
                case "Next":
                    if (currentPage >= countAllPages - 1)
                    {
                        currentPage = countAllPages - 1;
                    }
                    else
                    {
                        currentPage++;
                    }
                    break;
                case "Previous":
                    if (currentPage <= 0)
                    {
                        currentPage = 0;
                    }
                    else
                    {
                        currentPage--;
                    }
                    break;
            }
            List<auth> SelectedUsers = FilterListUsers.Skip((int)countInPage * currentPage).Take((int)countInPage).ToList();
            listBoxInfoUsers.ItemsSource = SelectedUsers;
            txtCurrentPage.Text = Convert.ToString(currentPage + 1);
            txtCurrentPageNavigation.Text = Convert.ToString(currentPage + 1);
        }

        private void Pagination(object sender, MouseButtonEventArgs e)
        {
            TextBlock ob = (TextBlock)sender;
            switch (ob.Uid)
            {
                case "Next":
                    if (currentPage == countAllPages - 1)
                    {
                        currentPage = countAllPages - 1;
                    }
                    else
                    {
                        currentPage++;
                    }
                    break;
                case "Previous":
                    if (currentPage == 0)
                    {
                        currentPage = 0;
                    }
                    else
                    {
                        currentPage--;
                    }
                    break;
            }
            List<auth> SelectedUsers = FilterListUsers.Skip((int)countInPage * currentPage).Take((int)countInPage).ToList();
            listBoxInfoUsers.ItemsSource = SelectedUsers;
            txtCurrentPage.Text = Convert.ToString(currentPage + 1);
            txtCurrentPageNavigation.Text = Convert.ToString(currentPage + 1);
        }

        private void btn_GoToPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtGoToPage.Text != "")
                {
                    currentPage = Convert.ToInt32(txtGoToPage.Text)-2;
                    if (currentPage > 0)
                    {
                        Pagination("Next");
                    }
                    else
                    {
                        Pagination("Previous");
                    }
                    
                }
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
