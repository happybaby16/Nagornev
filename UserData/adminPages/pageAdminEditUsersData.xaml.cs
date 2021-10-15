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
    /// Логика взаимодействия для pageAdminEditUsersData.xaml
    /// </summary>
    public partial class pageAdminEditUsersData : Page
    {
        private auth SelectedUser;
        public pageAdminEditUsersData(auth SelectedUSer)
        {
            InitializeComponent();
            genderListBox.ItemsSource = DB.DataBase.genders.ToList();
            genderListBox.SelectedValuePath = "id";
            genderListBox.DisplayMemberPath = "gender";


            this.SelectedUser = SelectedUSer;

            txtLogin.Text = SelectedUser.login;
            txtName.Text = SelectedUser.users.name;
            dateBirth.SelectedDate = SelectedUser.users.dr;
            genderListBox.SelectedIndex = SelectedUser.users.gender-1;
            List<users_to_traits> trs = SelectedUser.users.users_to_traits.ToList();
            foreach (users_to_traits tr in trs)
            {
                string trName = DB.DataBase.traits.FirstOrDefault(x => x.id == tr.id_trait).trait;
                if ((string)goodCB.Content == trName)
                {
                    goodCB.IsChecked = true;
                }
                if ((string)nejnCB.Content == trName)
                {
                    nejnCB.IsChecked = true;
                }
                if ((string)laskovCB.Content == trName)
                {
                    laskovCB.IsChecked = true;
                }

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string password;
            if (txtLogin.Text != "" && DB.DataBase.auth.FirstOrDefault(x => x.login == txtLogin.Text) == null || txtLogin.Text==SelectedUser.login)
            {
                if (txtName.Text != "")
                {
                    if (txtPassword.Password == "")
                    {
                        password = SelectedUser.password;
                    }
                    else
                    {
                        password = txtPassword.Password;
                    }

                    //Изменение данных выбранного пользователя
                    SelectedUser.login = txtLogin.Text;
                    SelectedUser.password = txtPassword.Password;
                    SelectedUser.users.name = txtName.Text;
                    SelectedUser.users.dr = (DateTime)dateBirth.SelectedDate;
                    SelectedUser.users.gender = (int)genderListBox.SelectedValue;
                   

                    //Вставка изменённых качеств
                    List<users_to_traits> trOnDelete = DB.DataBase.users_to_traits.Where(x => x.id_user == SelectedUser.id).ToList();
                    foreach (users_to_traits tr in trOnDelete)
                    {
                        DB.DataBase.users_to_traits.Remove(tr);
                    }
                    DB.DataBase.SaveChanges();

                    if (goodCB.IsChecked == true)
                    {
                        users_to_traits tr = new users_to_traits();
                        tr.id_user = SelectedUser.id;
                        tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == goodCB.Content.ToString()).id;
                        DB.DataBase.users_to_traits.Add(tr);
                    }
                    if (nejnCB.IsChecked == true)
                    {
                        users_to_traits tr = new users_to_traits();
                        tr.id_user = SelectedUser.id;
                        tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == nejnCB.Content.ToString()).id;
                        DB.DataBase.users_to_traits.Add(tr);
                    }
                    if (laskovCB.IsChecked == true)
                    {
                        users_to_traits tr = new users_to_traits();
                        tr.id_user = SelectedUser.id;
                        tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == laskovCB.Content.ToString()).id;
                        DB.DataBase.users_to_traits.Add(tr);
                    }

                    DB.DataBase.SaveChanges();
                    MessageBox.Show("Изменение данных выполнено успешно");
                    User.mainFrame.GoBack();

                }
                else 
                {
                    MessageBox.Show("Поле \"Имя\" не заполнено!");
                }

            }
            else 
            {
                MessageBox.Show("Поле \"Логин\" не заполнено или пользователь с таким логином существует! Придумайте новый логин!");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.GoBack();
        }
    }
}
