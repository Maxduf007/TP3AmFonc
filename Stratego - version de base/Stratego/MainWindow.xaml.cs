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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CouleurJoueur"></param>
        /// <param name="TabPiecePositionJoueur"></param>
        public MainWindow(Couleur CouleurJoueur, Piece[,] TabPiecePositionJoueur)
        {
            InitializeComponent();

            Jeu = new JeuStrategoControl(CouleurJoueur, TabPiecePositionJoueur, this);

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

        public void AjustementPieceEliminee(List<Piece> lstPieceElimineeAjustement, string NomLabel)
        {
            switch(NomLabel)
            {
                case "Marechal": lblMarechal.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "General":
                    lblGeneral.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Colonel":
                    lblColonel.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Commandant":
                    lblCommandant.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Capitaine":
                    lblCapitaine.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Lieutenant":
                    lblLieutenant.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Sergent":
                    lblSergent.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Demineur":
                    lblDemineur.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Eclaireur":
                    lblEclaireur.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Espion":
                    lblEspion.Content = lstPieceElimineeAjustement.Count();
                    break;
                case "Bombe":
                    lblBombe.Content = lstPieceElimineeAjustement.Count();
                    break;
            }
        }
    }
}