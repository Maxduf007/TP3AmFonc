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
        /// Initialise la fenêtre
        /// </summary>
        /// <param name="CouleurJoueur">Couleur du joueur choisie dans les paramètres du jeu</param>
        /// <param name="TabPiecePositionJoueur">Liste des pions placés par le joueur dans les paramètres du jeu</param>
        public MainWindow(Couleur CouleurJoueur, Piece[,] TabPiecePositionJoueur)
        {
            InitializeComponent();

            Jeu = new JeuStrategoControl(CouleurJoueur, TabPiecePositionJoueur, this);

            grdPrincipale.Children.Add(Jeu);
        }

        /// <summary>
        /// Bouton qui permet de lancer une nouvelle partie. L'application sera redémarrée pour retournée à la fenêtre NouvellePartieWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Ajuste les labels de chaque type de pièce lorsqu'un pièce adverse est éliminée.
        /// </summary>
        /// <param name="lstPieceElimineeAjustement">Liste qui contient le nombre d'un type de pièce éliminée de l'adversaire pour faire l'ajustement au label</param>
        /// <param name="NomLabel">Nom du label qui contient le type de pièce à ajuster</param>
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