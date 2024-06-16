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
    /// Логика взаимодействия для WindowCheckOrganization.xaml
    /// </summary>
    public partial class WindowCheckOrganization : Window
    {
        HranitelPRO7 db = HranitelPRO7.GetContext();
        Polzovateli curUser;
        Organizacii curOrg;
        internal bool IsChecked;
        public WindowCheckOrganization(Polzovateli p, Organizacii o)
        {
            curUser = db.Polzovateli.Where(x=>x.ID_Polzovatelia == p.ID_Polzovatelia).FirstOrDefault();
            curOrg = db.Organizacii.Where(x => x.ID_Organizacii == o.ID_Organizacii).FirstOrDefault();

            InitializeComponent();

            spOrganization.DataContext = curOrg;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            curUser.Status = "+";
            db.SaveChanges();

            IsChecked = true;
            Close();
        }

        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            curUser.Status = null;
            curUser.ID_Organizacii = null;
            db.Organizacii.Remove(curOrg);
            db.SaveChanges();

            IsChecked = false;
            Close();
        }
    }
}
