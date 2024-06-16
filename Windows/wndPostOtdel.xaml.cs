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
using System.Windows.Shapes;
using Terminals.Model;

namespace Terminals.Windows
{
    /// <summary>
    /// Логика взаимодействия для wndPostOtdel.xaml
    /// </summary>
    public partial class wndPostOtdel : Window
    {
        DateTime dateSort = DateTime.Now.Date;
        Sotrudniki curWorker = new Sotrudniki();
        HranitelPRO7 db = HranitelPRO7.GetContext();
        public wndPostOtdel(Sotrudniki s)
        {
            curWorker = s;

            InitializeComponent();
            FillDate(dateSort);
            FillApplications();

            

        }

        private void dtgApplications_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(dtgApplications.SelectedItems.Count > 0)
            {
                FillPeoples();

                gridVisitors.Visibility = Visibility.Visible;
                gridApplications.Visibility = Visibility.Collapsed;
            }  
        }

        private void btnBackToAppli_Click(object sender, RoutedEventArgs e)
        {
            gridVisitors.Visibility = Visibility.Collapsed;
            gridApplications.Visibility = Visibility.Visible;
        }

        public void FillApplications()
        {
            var _applicationsList = db.Zajavki.Where(x => x.ID_Statusa == 1 && x.ID_Podrazdelenia == curWorker.ID_Podrazdelenia
                        && x.Data_poseshenia == dateSort).ToList();


            var applicationsList = _applicationsList.Join(db.Polzovateli, x => x.ID_Polzovatelia, y => y.ID_Polzovatelia, (x, y) => new
            {
                Nomer_zajavki = x.Nomer_zajavki,
                FI = y.Familia + " " + y.Imya,
                Login = y.Login,
                CountVisitors = x.GrupZajavki.Count
            });

            dtgApplications.ItemsSource = applicationsList;
        }

        public void FillPeoples()
        {
            object item = dtgApplications.SelectedItem;
            string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            var appli = db.Zajavki.Where(x => x.Nomer_zajavki == number).FirstOrDefault();

            var group = db.GrupZajavki.Where(x => x.ID_Zajavki == appli.ID_Zajavki).ToList();

            var peoplesList = group.Join(db.Posetiteli, x => x.ID_Posetitelia, y => y.ID_Psetitelia, (x, y) => new
            {
                FIO = y.Familia + " " + y.Imya + " " + y.Otchestvo,
                Passport = y.Seria_pas + " " + y.Nomer_pas,
                DateBirth = y.Data_rozhdenia,
                PhoneNumber = y.Nomer_telefona
            });

            tbkAppliNumber.DataContext = appli;
            dtgVisitors.ItemsSource = peoplesList;


            //Кнопка организации
            var user = db.Polzovateli.Where(x => x.ID_Polzovatelia == appli.ID_Polzovatelia).FirstOrDefault();
            var organization = db.Organizacii.Where(x => x.ID_Organizacii == user.ID_Organizacii).FirstOrDefault();

            if (organization != null && user.Status != "+")
            { btnChekOrganization.Visibility = Visibility.Visible; }
            else { btnChekOrganization.Visibility = Visibility.Collapsed; }
            //Кнопка организации
        }

        private void btnShowCalendar_Click(object sender, RoutedEventArgs e)
        {
            WindowCalendar calendar = new WindowCalendar();
            calendar.ShowDialog();

            FillDate(calendar.InputValue);
            FillApplications();
        }

        public void FillDate(DateTime dt)
        {
            if (dt.ToString() == "01.01.0001 0:00:00")
            { return; }
            string date = dt.ToShortDateString();
            if (dt == DateTime.Now.Date)
                date = "Сегодня" + "(" + date + ")";
            if (dt == DateTime.Now.Date.AddDays(1))
                date = "Завтра" + "(" + date + ")";

            dateSort = dt;
            tbkDate.Text = date;
        }

        private void btnAddVisitorToBlackList_Click(object sender, RoutedEventArgs e)
        {
            if (dtgVisitors.SelectedItem == null)
            { MessageBox.Show("Выберите гостя, для добавления в черный список", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            if (dtgVisitors.SelectedItems.Count > 0)
            {
                object item = dtgVisitors.SelectedItem;
                string passport = (dtgVisitors.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

                var visitor = db.Posetiteli.Where(x => x.Seria_pas + " " + x.Nomer_pas == passport).FirstOrDefault();
                var groupVisitor = db.GrupZajavki.Where(x => x.ID_Posetitelia == visitor.ID_Psetitelia).FirstOrDefault();


                MessageBoxResult result = MessageBox.Show("Вы действительно хотите добавить \n<" 
                    + (dtgVisitors.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text +
                    ">\nВ черный список?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                    return;



                var blackList = new CherniySpisok();
                blackList.Nomer_pas = visitor.Nomer_pas;
                blackList.Seria_pas = visitor.Seria_pas;
                blackList.Familia = visitor.Familia;
                blackList.Imya = visitor.Imya;
                blackList.Otchestvo = visitor.Otchestvo;
                blackList.Nomer_telefona = visitor.Nomer_telefona;

                db.CherniySpisok.Add(blackList);
                db.GrupZajavki.Remove(groupVisitor);
                db.Posetiteli.Remove(visitor);
                db.SaveChanges();

                MessageBox.Show("Вы успешно добавили \n<"
                    + (dtgVisitors.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text +
                    ">\nВ черный список","Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                FillPeoples();
            }            
        }

        private void btnClosedAppli_Click(object sender, RoutedEventArgs e)
        {
            if (dtgApplications.SelectedItems.Count > 0)
            {
                object item = dtgApplications.SelectedItem;
                string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                var appli = db.Zajavki.Where(x => x.Nomer_zajavki == number).FirstOrDefault();

                var group = db.GrupZajavki.Where(x => x.ID_Zajavki == appli.ID_Zajavki).ToList();

                var visitors = new List<Posetiteli>();
                for(int i = 0; i < group.Count; i++)
                {
                    visitors.Add(db.Posetiteli.ToList().Where(x => x.ID_Psetitelia == group[i].ID_Posetitelia).FirstOrDefault());                
                }

                MessageBoxResult result = MessageBox.Show("Вы действительно хотите закрыть заявку\n№"+ number, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                    return;

                db.Posetiteli.RemoveRange(visitors);
                db.GrupZajavki.RemoveRange(group);
                appli.ID_Statusa = 4;
                appli.Soobshenie = "Посещение прошло успешно\nЖдем вас еще";
                db.SaveChanges();

                MessageBox.Show("Вы успешно закрыли заяввку\n№" + number, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                gridVisitors.Visibility = Visibility.Collapsed;
                gridApplications.Visibility = Visibility.Visible;
                FillApplications();
            }            
        }

        private void btnChekOrganization_Click(object sender, RoutedEventArgs e)
        {
            object item = dtgApplications.SelectedItem;
            string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            var appli = db.Zajavki.Where(x=>x.Nomer_zajavki == number).FirstOrDefault();
            var user = db.Polzovateli.Where(x => x.ID_Polzovatelia == appli.ID_Polzovatelia).FirstOrDefault();
            var organization = db.Organizacii.Where(x => x.ID_Organizacii == user.ID_Organizacii).FirstOrDefault();


            WindowCheckOrganization checkOrganization = new WindowCheckOrganization(user, organization);
            checkOrganization.ShowDialog();

            if (checkOrganization.IsChecked)
                MessageBox.Show("Вы успешно подтвердили организацию", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Вы успешно отклонили организацию", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            FillPeoples();
        }
    }
}
