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
using UserData.pages.pageRegLog;


namespace UserData
{
    public static class User
    {
        public static Frame mainFrame;
        public static MainWindow mainWindow;
    }
    public static class DB
    {
        public static Entities DataBase;
        public static auth currentUser;
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            User.mainFrame = mainFrame;
            User.mainWindow = this;
            mainFrame.Navigate(new pageLogin());
            DB.DataBase = new Entities();
            
        }
    }
}
