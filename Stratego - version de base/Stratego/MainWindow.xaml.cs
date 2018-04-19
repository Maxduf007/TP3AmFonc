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

namespace Stratego
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public JeuStrategoControl Jeu { get; set; }

        public MainWindow(Couleur CouleurJoueur)
        {
            InitializeComponent();

            Jeu = new JeuStrategoControl(CouleurJoueur);

            grdPrincipale.Children.Add(Jeu);
        }

        private void btnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat;
            resultat = MessageBox.Show("Êtes-vous sûr de vouloir lancer une nouvelle partie?"
                                       , "Nouvelle partie"
                                      , MessageBoxButton.YesNo);
            if (resultat == MessageBoxResult.No)
            {
                
            }
            else
            {
                Application.Current.Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }
    }
}