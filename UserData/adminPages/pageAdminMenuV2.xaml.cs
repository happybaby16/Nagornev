using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Library;
using Microsoft.Win32;

namespace UserData.adminPages
{
    /// <summary>
    /// Логика взаимодействия для pageAdminMenuV2.xaml
    /// </summary>
    public partial class pageAdminMenuV2 : Page
    {


        public List<auth> Userdata;
        List<auth> FilterListUsers;
        int countInPage = 2;
        PageChange pc = new PageChange();


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

            DataContext = pc;//поместил объект в ресурсы страницы
            pc.CountPage = countInPage;
            pc.Countlist = FilterListUsers.Count;
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
            pc.Countlist = FilterListUsers.Count;
            UpdatePages();
        }



        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;   
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            listBoxInfoUsers.ItemsSource = FilterListUsers.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
        }

        private void UpdatePages()
        {
            pc.CurrentPage = 1;
            listBoxInfoUsers.ItemsSource = FilterListUsers.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePages();
        }

        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = DB.DataBase.users.FirstOrDefault(x => x.id == ind);//запись о текущем пользователе
            List<usersimage> UI = DB.DataBase.usersimage.Where(x => x.id_user == ind).ToList();//получаем запись о картинке для текущего пользователя
            UI.Reverse();
            BitmapImage BI = new BitmapImage();
            if (U != null)
            {
                if (UI.Count != 0)//если для текущего пользователя существует запись о его катринке
                {
                    for (int i = 0; i < UI.Count; i++)
                    {
                        if (UI[i].avatar)
                        { 
                            if (UI[i].path != null)//если присутствует путь к картинке
                            {
                                BI = new BitmapImage(new Uri(UI[i].path, UriKind.Relative));
                            }
                            else//если присутствуют двоичные данные
                            {
                                BI.BeginInit();
                                BI.StreamSource = new MemoryStream(UI[i].image);
                                BI.EndInit();
                            }
                            break;
                        }
                    }
                }
                else
                {
                    switch (U.gender)
                    {
                        case 1:
                            BI = new BitmapImage(new Uri(@"/images/male.png", UriKind.Relative));
                            break;
                        case 2:
                            BI = new BitmapImage(new Uri(@"/images/female.png", UriKind.Relative));
                            break;
                        default:
                            BI = new BitmapImage(new Uri(@"/images/other.png", UriKind.Relative));
                            break;
                    }
                }
                IMG.Height = 100;
                IMG.Width = 100;
                IMG.Source = BI;
            }
            
        }

        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; 
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
                ImageConverter IC = new ImageConverter();
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));
                usersimage UI = new usersimage() { id_user = ind, image = ByteArr };
                MessageBoxResult answer = MessageBox.Show("Вы хотите сделать выбранную фотографию как основной аватар?", "Аватар?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    List<usersimage> UIlist = DB.DataBase.usersimage.Where(x => x.id_user == ind).ToList();
                    for (int i = 0; i < UIlist.Count; i++)
                    {
                        UIlist[i].avatar = false;
                    }
                    UI.avatar = true;
                }
                else UI.avatar = false;
                DB.DataBase.usersimage.Add(UI);
                DB.DataBase.SaveChanges();
                MessageBox.Show("Картинка пользователя добавлена в базу");
            }
            else
            {
                MessageBox.Show("Операция выбора изображения отменена");
            }
        }




        private void btnGoToGallery_Click(object sender, RoutedEventArgs e)
        {
            Button btnOpenGallery = (Button)sender;
            int idSelectedUser = Convert.ToInt32(btnOpenGallery.Uid);
            User.mainFrame.Navigate(new pageAdminGallery(idSelectedUser));
        }

        /// <summary>
        /// Функция, которая скрывет галерею пользователя, если у него отсутствуют картинки в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoToGallery_Loaded(object sender, RoutedEventArgs e)
        {
            Button btnSender = (Button)sender;
            int idLoadedUser = Convert.ToInt32(btnSender.Uid);
            List<usersimage> UI = DB.DataBase.usersimage.Where(x => x.id_user == idLoadedUser).ToList();
            if (UI.Count==0)
            {
                btnSender.Visibility = Visibility.Hidden;
            }
        }
    }
}
