using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using UserData.adminPages;

namespace UserData.pages.pageRegLog
{
    /// <summary>
    /// Логика взаимодействия для pageLogin.xaml
    /// </summary>
    public partial class pageLogin : Page
    {
        private void CreateCaptcha()
        {
            willAnswer = 0;
            int temp = r.Next(0, 9);
            txtCaptcha.Text = temp.ToString();
            willAnswer += temp;
            txtCaptcha.Text += " + ";
            temp = r.Next(0, 9);
            txtCaptcha.Text += temp.ToString();
            willAnswer += temp;
        }
        Random r = new Random();
        int willAnswer;//Captcha answer


        public pageLogin()
        {
            InitializeComponent();
            txtLogin.Text = "pupsik";
            txtPassword.Password = "123";
            CreateCaptcha();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.Navigate(new pageRegistration());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (willAnswer.GetHashCode() == Convert.ToInt32(txtAnswerCaptcha.Text).GetHashCode())
                {
                    SHA256 mySHA256 = SHA256.Create();
                    byte[] passwordByte = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Password));
                    string passwordHash = Convert.ToBase64String(passwordByte);
                    auth currentUser = DB.DataBase.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == passwordHash);
                    if (currentUser != null)
                    {
                        MessageBox.Show($"Добро пожаловать, {currentUser.login}!");
                        DB.currentUser = currentUser;
                        if (DB.currentUser.role == 1)
                        {
                            User.mainFrame.Navigate(new pageAdminMenuV2());
                            User.mainWindow.imgAddBanner.Visibility = Visibility.Hidden;
                            User.mainWindow.lbBanner.Visibility = Visibility.Hidden;
                            User.mainWindow.mainGrid.ColumnDefinitions.RemoveAt(0);
                            User.mainWindow.mainGrid.RowDefinitions.RemoveAt(2);
                        }
                        else if (DB.currentUser.role == 2)
                        {
                            User.mainFrame.Navigate(new pageInfoUser());
                        }

                    }
                    else
                    {
                        MessageBox.Show("Неверно введен логин или пароль!");
                    }
                }
                else
                {
                    MessageBox.Show("Неверно введена капча, повторите попытку!");
                    CreateCaptcha();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введена капча, повторите попытку!");
                CreateCaptcha();
            }
            
        }

        private void btnRefresh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateCaptcha();
        }
    }
}
