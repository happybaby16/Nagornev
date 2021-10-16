﻿using System;
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
using UserData.adminPages;

namespace UserData.pages.pageRegLog
{
    /// <summary>
    /// Логика взаимодействия для pageLogin.xaml
    /// </summary>
    public partial class pageLogin : Page
    {
        public pageLogin()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.Navigate(new pageRegistration());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            auth currentUser = DB.DataBase.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == txtPassword.Password);
            if (currentUser != null)
            {
                MessageBox.Show($"Добро пожаловать, {currentUser.login}!");
                DB.currentUser = currentUser;
                if (DB.currentUser.role == 1)
                {
                    User.mainFrame.Navigate(new pageAdminMenuV2());
                    User.mainWindow.imgAddBanner.Visibility = Visibility.Hidden;
                    User.mainWindow.lbBanner.Visibility = Visibility.Hidden;
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
    }
}
