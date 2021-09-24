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
            User.mainFrame.Navigate(new pageInfoUser());
        }
    }
}
