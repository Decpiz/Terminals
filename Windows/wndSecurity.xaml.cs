using System;
using System.Collections;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Terminals.Model;

namespace Terminals.Windows
{
    /// <summary>
    /// Логика взаимодействия для wndSecurity.xaml
    /// </summary>
    public partial class wndSecurity : Window
    {
        DateTime dateSort = DateTime.Now.Date;
        Sotrudniki curWorker = new Sotrudniki();
        HranitelPRO7 db = HranitelPRO7.GetContext();
        public wndSecurity(Sotrudniki s)
        {
            curWorker = s;

            InitializeComponent();

            FillDate(dateSort);
            FillPeoplesList();
        }

        private void btnShowCalendar_Click(object sender, RoutedEventArgs e)
        {

            WindowCalendar calendar = new WindowCalendar();
            calendar.ShowDialog();

            FillDate(calendar.InputValue);
            FillPeoplesList();
            tbSearchPeoples.Text = "Поиск...";
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

        public void FillPeoplesList()
        {
            var appli = db.Zajavki.Where(x => x.ID_Statusa == 1 && x.ID_Podrazdelenia == curWorker.ID_Podrazdelenia).ToList();

            var group = new List<GrupZajavki>();
            foreach (Zajavki z in appli)
            {
                if (z.Data_poseshenia == dateSort)
                {
                    group.AddRange(db.GrupZajavki.Where(x => x.ID_Zajavki == z.ID_Zajavki).ToList());
                }
            }

            var peoplesList = group.Join(db.Posetiteli, x => x.ID_Posetitelia, y => y.ID_Psetitelia, (x, y) => new
            {
                FIO = y.Familia + " " + y.Imya + " " + y.Otchestvo,
                Passport = y.Seria_pas + " " + y.Nomer_pas,
                DateBirth = y.Data_rozhdenia,
                PhoneNumber = y.Nomer_telefona
            });

            dtgVisitors.ItemsSource = peoplesList;
            
            if (dtgVisitors.Items.Count == 0 || dtgVisitors == null)
            {
                _tbkEmptyList.Visibility = Visibility.Visible;
                tbkEmptyList.Visibility = Visibility.Visible;
            }
            else
            {
                _tbkEmptyList.Visibility = Visibility.Collapsed;
                tbkEmptyList.Visibility = Visibility.Collapsed;
            }
        }

        private void tbSearchAppli_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchPeoples.Text == "Поиск...")
            {
                tbSearchPeoples.Clear();
            }
        }

        private void tbSearchAppli_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchPeoples.Text == "")
            { tbSearchPeoples.Text = "Поиск..."; }
        }

        private void tbSearchPeoples_TextChanged(object sender, TextChangedEventArgs e)
        {
            var appli = db.Zajavki.Where(x => x.ID_Statusa == 1 && x.ID_Podrazdelenia == curWorker.ID_Podrazdelenia).ToList();

            var group = new List<GrupZajavki>();
            foreach (Zajavki z in appli)
            {
                if (z.Data_poseshenia == dateSort)
                {
                    group.AddRange(db.GrupZajavki.Where(x => x.ID_Zajavki == z.ID_Zajavki).ToList());
                }
            }

            var peoplesList = group.Join(db.Posetiteli, x => x.ID_Posetitelia, y => y.ID_Psetitelia, (x, y) => new
            {
                FIO = y.Familia + " " + y.Imya + " " + y.Otchestvo,
                Passport = y.Seria_pas + " " + y.Nomer_pas,
                DateBirth = y.Data_rozhdenia,
                PhoneNumber = y.Nomer_telefona
            });

            if (dtgVisitors != null)
            {
                if (tbSearchPeoples.Text != "" && tbSearchPeoples.Text != "Поиск...")
                {
                    var sortedList = peoplesList.Where(x => x.FIO.ToLower().Contains(tbSearchPeoples.Text.ToLower())
                                || x.Passport.ToLower().Contains(tbSearchPeoples.Text.ToLower()));


                    dtgVisitors.ItemsSource = sortedList;
                }
                else
                { dtgVisitors.ItemsSource = peoplesList; }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbSearchPeoples.Text = "Поиск...";
            FillPeoplesList();
        }
    }
}
