using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using Terminals.ClassesHelp;
using Terminals.Model;

namespace Terminals.Windows
{
    /// <summary>
    /// Логика взаимодействия для wndPostGlobal.xaml
    /// </summary>
    
    public partial class wndPostGlobal : Window
    {

        HranitelPRO7 db = new HranitelPRO7();
        public wndPostGlobal()
        {
            InitializeComponent();

            var divisions = db.Podrazdelenia.ToList();
            lvDivisions.ItemsSource = divisions;

            var appliList = new List<Zajavki>();
            foreach (Zajavki z in db.Zajavki.ToList())
            {
                if (z.ID_Statusa == 3)
                {
                    appliList.Add(z);
                }
            }            


            var appli = appliList.Join(db.Polzovateli.ToList(), c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
            {
                Nomer_zajavki = c.Nomer_zajavki,
                FI = z.Familia + " " + z.Imya,
                Date = c.Data_poseshenia,
                CountVisitors = c.GrupZajavki.Count()                               
            });

            dtgApplications.ItemsSource = appli;

            

        }

        private void btnPeopleListShow_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dtgApplications_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dtgPeoplesList.ItemsSource = null;

            if (dtgApplications.SelectedItems.Count > 0)
            {

                object item = dtgApplications.SelectedItem;
                string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                if (number != null)
                { 
                    Zajavki appli = new Zajavki();
                    foreach (Zajavki z in db.Zajavki.ToList())
                    {
                        if (z.Nomer_zajavki == number)
                        { appli = z; break; }
                    }

                    List<int> peoplesID = new List<int>();
                    foreach (GrupZajavki g in db.GrupZajavki.ToList())
                    {
                        if (appli.ID_Zajavki == g.ID_Zajavki)
                        {
                            peoplesID.Add(g.ID_Posetitelia);
                        }
                    }

                    List<Posetiteli> visitors = new List<Posetiteli>();
                    foreach (Posetiteli p in db.Posetiteli.ToList())
                    {
                        for (int i = 0; i < peoplesID.Count; i++)
                        {
                            if (p.ID_Psetitelia == peoplesID[i])
                            {
                                visitors.Add(p);
                                break;
                            }
                        }
                    }

                    if (visitors != null)
                    { dtgPeoplesList.ItemsSource = visitors;}                    
                }                
            }           
        }

        private void lvVidi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void lvDivisions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDivisions.SelectedItems.Count > 0)
            {
                Podrazdelenia division = lvDivisions.SelectedItems[0] as Podrazdelenia;

                string div = "<" + division.Nazvanie_goroda + "> " + division.Nazvanie_ylici + " " + division.Nomer_stroenia;

                List<Zajavki> appliList = new List<Zajavki>();
                foreach (Zajavki z in db.Zajavki.ToList())
                {
                    if (z.ID_Podrazdelenia == division.ID_Podrazdelenia && z.ID_Statusa == 3)
                    { appliList.Add(z); }
                }

                var users = db.Polzovateli.ToList();
                var appliInput = appliList.Join(users, c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
                {
                    Nomer_zajavki = c.Nomer_zajavki,
                    FI = z.Familia + " " + z.Imya,
                    Date = c.Data_poseshenia,
                    CountVisitors = c.GrupZajavki.Count()
                });

                tbAppliTitle.Text = div;
                dtgApplications.ItemsSource = appliInput;
            }
        }

        private void tbSearchAppli_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void tbSearchAppli_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchAppli.Text == "Поиск...")
            {
                tbSearchAppli.Clear();  
            } 
        }

        private void tbSearchAppli_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchAppli.Text == "")
            { tbSearchAppli.Text = "Поиск..."; }
            
        }

        private void tbSearchAppli_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void btnFilDtgApplicationsFullContent_Click(object sender, RoutedEventArgs e)
        {
            tbAppliTitle.Text = "Все заявки";
            lvDivisions.SelectedItem = null;

            var appliList = new List<Zajavki>();
            foreach (Zajavki z in db.Zajavki.ToList())
            {
                if (z.ID_Statusa == 3)
                {
                    appliList.Add(z);
                }
            }

            var appli = appliList.Join(db.Polzovateli.ToList(), c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
            {
                Nomer_zajavki = c.Nomer_zajavki,
                FI = z.Familia + " " + z.Imya,
                Date = c.Data_poseshenia,
                CountVisitors = c.GrupZajavki.Count()
            });

            dtgApplications.ItemsSource = appli;

        }

        private void btnAcceptAppli_Click(object sender, RoutedEventArgs e)
        {
            if(dtgApplications.SelectedItem != null)
            {
                object item = dtgApplications.SelectedItem;
                string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                var appli = new Zajavki();

                foreach (Zajavki z in db.Zajavki.ToList())
                {
                    if (z.Nomer_zajavki == number)
                    {
                        appli = z;
                    }
                }

                appli.ID_Statusa = 1;
                appli.Soobshenie = "Ждем вас\n" + appli.Data_poseshenia.Day + "." + appli.Data_poseshenia.Month 
                    + "." + appli.Data_poseshenia.Year;

                MessageBoxResult result = MessageBox.Show("Вы действительно хотите принять заявку?\n№ " + number, 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                    return;

                db.Entry(appli);
                db.SaveChanges();                
                MessageBox.Show("Заявка успешно принята.","Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

                RefreshToAcceptOrDiscard();
            }
            else { MessageBox.Show("Укажите заявку.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning); }
            
        }

        public void btnDiscardAppli_Click(object sender, RoutedEventArgs e)
        {
            if (dtgApplications.SelectedItem != null)
            {
                object item = dtgApplications.SelectedItem;
                string number = (dtgApplications.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                var appli = db.Zajavki.Where(x => x.Nomer_zajavki == number).FirstOrDefault();

                wndMessage message = new wndMessage(appli);
                message.ShowDialog();

                dtgApplications.SelectedItem = null;

                RefreshToAcceptOrDiscard();

            }
            else { MessageBox.Show("Укажите заявку.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }

        public void RefreshToAcceptOrDiscard()
        {
            var appliList = new List<Zajavki>();
            foreach (Zajavki z in db.Zajavki.ToList())
            {
                if (z.ID_Statusa == 3)
                { appliList.Add(z); }
            }
            var users = db.Polzovateli.ToList();
            var appliInput = appliList.Join(users, c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
            {
                Nomer_zajavki = c.Nomer_zajavki,
                ID_Podrazdelenia = c.ID_Podrazdelenia,
                FI = z.Familia + " " + z.Imya,
                Date = c.Data_poseshenia,
                CountVisitors = c.GrupZajavki.Count()
            });

            if (lvDivisions.SelectedItems.Count == 0 && (tbSearchAppli.Text == "" || tbSearchAppli.Text == "Поиск..."))
            {
                dtgApplications.ItemsSource = appliInput;
            }
            else
            {
                Search();
            }
        }

        public void Search()
        {
            if (lvDivisions != null)
            {
                if (lvDivisions.SelectedItems.Count > 0)
                {
                    Podrazdelenia division = lvDivisions.SelectedItems[0] as Podrazdelenia;

                    List<Zajavki> appliList = new List<Zajavki>();
                    foreach (Zajavki z in db.Zajavki.ToList())
                    {
                        if (z.ID_Podrazdelenia == division.ID_Podrazdelenia && z.ID_Statusa == 3)
                        { appliList.Add(z); }
                    }

                    var users = db.Polzovateli.ToList();
                    var appliInput = appliList.Join(users, c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
                    {
                        Nomer_zajavki = c.Nomer_zajavki,
                        FI = z.Familia + " " + z.Imya,
                        Date = c.Data_poseshenia,
                        CountVisitors = c.GrupZajavki.Count()
                    });

                    if (dtgApplications != null)
                    {
                        if (tbSearchAppli.Text != "" && tbSearchAppli.Text != "Поиск...")
                        {
                            var filteredList = appliInput.Where(x => x.Nomer_zajavki.ToLower().Contains(tbSearchAppli.Text.ToLower())
                            || x.FI.ToLower().Contains(tbSearchAppli.Text.ToLower()));
                            dtgApplications.ItemsSource = null;
                            dtgApplications.ItemsSource = filteredList;
                        }
                        else
                        { dtgApplications.ItemsSource = appliInput; }
                    }
                }
                else
                {
                    var appliList = new List<Zajavki>();
                    foreach (Zajavki z in db.Zajavki.ToList())
                    {
                        if (z.ID_Statusa == 3)
                        {
                            appliList.Add(z);
                        }
                    }
                    var appli = appliList.Join(db.Polzovateli.ToList(), c => c.ID_Polzovatelia, z => z.ID_Polzovatelia, (c, z) => new
                    {
                        Nomer_zajavki = c.Nomer_zajavki,
                        FI = z.Familia + " " + z.Imya,
                        Date = c.Data_poseshenia,
                        CountVisitors = c.GrupZajavki.Count()
                    });

                    if (dtgApplications != null)
                    {
                        if (tbSearchAppli.Text != "" && tbSearchAppli.Text != "Поиск...")
                        {
                            var filteredList = appli.Where(x => x.Nomer_zajavki.ToLower().Contains(tbSearchAppli.Text.ToLower())
                            || x.FI.ToLower().Contains(tbSearchAppli.Text.ToLower()));
                            dtgApplications.ItemsSource = null;
                            dtgApplications.ItemsSource = filteredList;
                        }
                        else
                        {
                            dtgApplications.ItemsSource = appli;
                        }
                    }
                }
            }
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {

            var number = new List<string>();
            for (int i = 0; i < dtgApplications.Items.Count; i++)
            {               
                    number.Add((dtgApplications.Columns[0].GetCellContent(dtgApplications.Items[i]) as TextBlock).Text);         
            }

            var appliList = new List<Zajavki>();
            foreach (Zajavki z in db.Zajavki.ToList())
            {
                for (int i = 0; i < number.Count; i++)
                {
                    if (z.Nomer_zajavki == number[i])
                    {
                        appliList.Add(z);
                        break;
                    }
                }    
            }

            dtgPeoplesList.ItemsSource = appliList;
        }
    }
}
