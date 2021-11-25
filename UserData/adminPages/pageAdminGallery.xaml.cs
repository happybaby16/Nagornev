using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для pageAdminGallery.xaml
    /// </summary>
    public partial class pageAdminGallery : Page
    {
        public int selectedUser { get; set; }
        public List<usersimage> UI { get; set; }
        public users U { get; set; }

        int currentImg;
        public pageAdminGallery(int selectedUser)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            currentImg = 0;//Стартовая картинка пользователя (картинка, которая была добавлена последней)
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            U = DB.DataBase.users.FirstOrDefault(x => x.id == selectedUser);
            UI = DB.DataBase.usersimage.Where(x => x.id_user == selectedUser).ToList();
            UI.Reverse();
            BitmapImage BI = new BitmapImage();
            if (UI[currentImg].path != null)//если присутствует путь к картинке
            {
                BI = new BitmapImage(new Uri(UI[currentImg].path, UriKind.Relative));
            }
            else//если присутствуют двоичные данные
            {
                BI.BeginInit();
                BI.StreamSource = new MemoryStream(UI[currentImg].image);
                BI.EndInit();
            }
            imgSelectedImg.Source = BI;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.GoBack();
        }


        private void GalleryPagination(object sender, RoutedEventArgs e)
        {

            Button btnSender = (Button)sender;
            //Выбираем в какую сторону двигаться
            if (btnSender.Uid == "Next") currentImg++;
            else currentImg--;

            //Проверяем границы пагинации по картинкам
            if (currentImg >= UI.Count - 1) currentImg = UI.Count - 1;
            if (currentImg <= 0) currentImg = 0;

            //Изменяем картинку 
            BitmapImage BI = new BitmapImage();
            if (U != null)//Пррверяем пользователя на наличие данных о себе в таблице users
            {
                try
                {
                    if (UI.Count != 0)//если для текущего пользователя существует запись о его катринке
                    {
                        if (UI[currentImg].path != null)//если присутствует путь к картинке
                        {
                            BI = new BitmapImage(new Uri(UI[currentImg].path, UriKind.Relative));
                        }
                        else//если присутствуют двоичные данные
                        {
                            BI.BeginInit();
                            BI.StreamSource = new MemoryStream(UI[currentImg].image);
                            BI.EndInit();
                        }
                    }
                    imgSelectedImg.Source = BI;
                }
                catch { }
            }
        }

        private void btnSetAvatar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Вы уверены, что хотите сделать эту фотографию своим аватаром?", "Аватар?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                for (int i = 0; i < UI.Count; i++)
                {
                    UI[i].avatar = false;
                }
                UI[currentImg].avatar = true;
                MessageBox.Show("Аватар успешно изменён!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                User.mainFrame.GoBack();
            }
            
            
        }

        private void btnRemoveImg_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Вы уверены, что хотите удалить фотографию?", "Удалить?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
            {
                DB.DataBase.usersimage.Remove(UI[currentImg]);
                UI.RemoveAt(currentImg);
                MessageBox.Show("Фотография успешно удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                if (UI.Count != 0)
                {
                    UI[0].avatar = true;
                }
                DB.DataBase.SaveChanges();


            }
        }
    }
}
