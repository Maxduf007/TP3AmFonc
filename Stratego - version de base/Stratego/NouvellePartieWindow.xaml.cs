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

namespace Stratego
{
    /// <summary>
    /// Logique d'interaction pour NouvellePartieWindow.xaml
    /// </summary>
    public partial class NouvellePartieWindow : Window
    {
        public Couleur CouleurJoueurChoisi { get; set; }
        public Piece PieceSelectionnee { get; set; }

        private const int TAILLE_CASES_GRILLE = 40;

        public Piece[,] TabPieceJoueurPositionnee = new Piece[TAILLE_GRILLE_POSITIONNEMENT_Y, TAILLE_GRILLE_POSITIONNEMENT_X];

        public Image[,] TabAffichagePion = new Image[TAILLE_GRILLE_POSITIONNEMENT_Y, TAILLE_GRILLE_POSITIONNEMENT_X];

        #region Static
        private const int TAILLE_GRILLE_POSITIONNEMENT_X = 10;
        private const int TAILLE_GRILLE_POSITIONNEMENT_Y = 4;
        #endregion

        /// <summary>
        /// Initialise le fenêtre avec ses composantes. 
        /// </summary>
        public NouvellePartieWindow()
        {
            InitializeComponent();


            DiviserGrillePositionnement();
            ColorerGrilleJeu();
            DefinirZoneSelectionGrille();
        }

        /// <summary>
        /// Bouton qui lance la partie en créant une fenêtre mainWindow et en lui passant les paramètres sélectionnés par le joueur comme sa couleur et le positionnement de ses pions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLancerPartie_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(CouleurJoueurChoisi, TabPieceJoueurPositionnee);

            this.Close();

            mainWindow.Show();

            

        }

        /// <summary>
        /// Permet au joueur de sélectionner sa couleur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton) sender;

            if (radioButton == rdbRouge)
                CouleurJoueurChoisi = Couleur.Rouge;
            else
                CouleurJoueurChoisi = Couleur.Bleu;



        }

        /// <summary>
        /// Initialise l'objet ayant le type demandé par le joueur pour son positionnement dans le jeu.
        /// </summary>
        /// <param name="NomPion"> reçoit le nom du pion pour sélectionner le bon type.</param>
        private void TypePieceSelectionnee(string NomPion)
        {
            switch (NomPion)
            {
                case "Marechal":
                    PieceSelectionnee = new Marechal(CouleurJoueurChoisi);
                    break;
                case "General":
                    PieceSelectionnee = new General(CouleurJoueurChoisi);
                    break;
                case "Colonel":
                    PieceSelectionnee = new Colonel(CouleurJoueurChoisi);
                    break;
                case "Commandant":
                    PieceSelectionnee = new Commandant(CouleurJoueurChoisi);
                    break;
                case "Capitaine":
                    PieceSelectionnee = new Capitaine(CouleurJoueurChoisi);
                    break;
                case "Lieutenant":
                    PieceSelectionnee = new Lieutenant(CouleurJoueurChoisi);
                    break;
                case "Sergent":
                    PieceSelectionnee = new Sergent(CouleurJoueurChoisi);
                    break;
                case "Demineur":
                    PieceSelectionnee = new Demineur(CouleurJoueurChoisi);
                    break;
                case "Eclaireur":
                    PieceSelectionnee = new Eclaireur(CouleurJoueurChoisi);
                    break;
                case "Espion":
                    PieceSelectionnee = new Espion(CouleurJoueurChoisi);
                    break;
                case "Drapeau":
                    PieceSelectionnee = new Drapeau(CouleurJoueurChoisi);
                    break;
                case "Bombe":
                    PieceSelectionnee = new Bombe(CouleurJoueurChoisi);
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// Fonction qui gère les boutons de placement des pièces. S'assure de mettre à jour le bon nombre restant de pièces à placer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlacerUnite_Click(object sender, RoutedEventArgs e)
        {
            Button PionBoutton = (Button)sender;
            string NomPionAPlacer = PionBoutton.Name.Substring(3);
            int NbPiecesDispo = Convert.ToInt16(PionBoutton.Content);

            if(NbPiecesDispo > 0 && PieceSelectionnee == null)
            {

                // On enlève une pièce disponible pour le type de pion sélectionné
                NbPiecesDispo -= 1;
                // On met à jour le contenu du boutton pour indiquer le nb de pièces restantes
                PionBoutton.Content = NbPiecesDispo.ToString();
                // On initialise le type d'objet Piece pour le placer dans la grille
                TypePieceSelectionnee(NomPionAPlacer);
            }



        }

        /// <summary>
        /// Permet de placer ou enlever le pion dans la grille de positionnement. Vérifie qu'une case est libre pour la pièce.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionnerPionGrille(object sender, MouseButtonEventArgs e)
        {
            // Reçoit le rectangle sélectionner par la souris
            Rectangle caseSelectionnee = (Rectangle)sender;
            Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
            Image ImageAffichage;

            if (e.ChangedButton == MouseButton.Left && PieceSelectionnee != null)
            {
                if(TabPieceJoueurPositionnee[pointSelectionne.Y, pointSelectionne.X] == null)
                {

                    // On place dans la grille de positionnement la pièce sélectionnée
                    TabPieceJoueurPositionnee[pointSelectionne.Y, pointSelectionne.X] = PieceSelectionnee;

                    // On place dans le tableau d'affichage de référence l'affichage de la pièce
                    TabAffichagePion[pointSelectionne.Y, pointSelectionne.X] = CreerAffichagePiece(PieceSelectionnee);

                    // On ajoute l'affichage dans la grid
                    ImageAffichage = TabAffichagePion[pointSelectionne.Y, pointSelectionne.X];

                    Grid.SetColumn(ImageAffichage, pointSelectionne.X);
                    Grid.SetRow(ImageAffichage, pointSelectionne.Y);

                    grdPlacementPion.Children.Add(ImageAffichage);

                    PieceSelectionnee = null;
                }
            }
            else if(e.ChangedButton == MouseButton.Right && PieceSelectionnee == null)
            {
                if (TabPieceJoueurPositionnee[pointSelectionne.Y, pointSelectionne.X] != null)
                {
                    // On enlève de la grille de positionnement la pièce sélectionnée
                    RemettrePionDansChoix(TabPieceJoueurPositionnee[pointSelectionne.Y, pointSelectionne.X]);
                    TabPieceJoueurPositionnee[pointSelectionne.Y, pointSelectionne.X] = null;

                    //On enlève l'affichage du grid
                    ImageAffichage = TabAffichagePion[pointSelectionne.Y, pointSelectionne.X];

                    grdPlacementPion.Children.Remove(ImageAffichage);


                    // On enlève l'affichage du tableau de référence pour la grid
                    TabAffichagePion[pointSelectionne.Y, pointSelectionne.X] = null;

                }
            }


        }

        /// <summary>
        /// Réajuste le nombre de pion dans son label lorsque le joueur enlève un pion de la grille.
        /// </summary>
        /// <param name="PieceSupprimee">Ladite pièce qui faut remettre dans les pièces disponibles</param>
        private void RemettrePionDansChoix(Piece PieceSupprimee)
        {
            StringBuilder NomBouton = new StringBuilder();
            string TypePiece = PieceSupprimee.GetType().ToString();
            Button BoutonPiece = new Button();
            int iNbPieceDispo;

            // On récupère le bouton dans la grid pour réajuster le nombre de pièce disponible
            NomBouton.Append("btn" + TypePiece.Substring(9));

            BoutonPiece = (Button)grdPlacementPion.FindName(NomBouton.ToString());

            iNbPieceDispo = Convert.ToInt16(BoutonPiece.Content);

            iNbPieceDispo++;

            BoutonPiece.Content = iNbPieceDispo.ToString();

        }

        /// <summary>
        /// Crée la structure de la grid en grille de 10*4
        /// </summary>
        private void DiviserGrillePositionnement()
        {
            ColumnDefinition colonneDef;
            RowDefinition ligneDef;

            for (int i = 0; i < TAILLE_GRILLE_POSITIONNEMENT_X; i++)
            {
                colonneDef = new ColumnDefinition();
                colonneDef.Width = new GridLength(TAILLE_CASES_GRILLE);
                grdPlacementPion.ColumnDefinitions.Add(colonneDef);
                
            }

            for (int j = 0; j < TAILLE_GRILLE_POSITIONNEMENT_Y; j++)
            {
                ligneDef = new RowDefinition();
                ligneDef.Height = new GridLength(TAILLE_CASES_GRILLE);
                grdPlacementPion.RowDefinitions.Add(ligneDef);
            }
        }

        /// <summary>
        /// Ajoute de la coloration à chaque case selon son type de terrain et ajoute des lignes verticalement et horizontalement.
        /// </summary>
        private void ColorerGrilleJeu()
        {
            Rectangle ligne;

            for (int i = 0; i < TAILLE_GRILLE_POSITIONNEMENT_X; i++)
            {
                grdPlacementPion.Children.Add(CreerLigneGrille(i, true));

                for (int j = 0; j < TAILLE_GRILLE_POSITIONNEMENT_Y; j++)
                {
                    grdPlacementPion.Children.Add(CreerFondCase(i, j));

                    if (i == 0)
                    {
                        grdPlacementPion.Children.Add(CreerLigneGrille(j, false));
                    }
                }
            }

            ligne = CreerLigneGrille(0, true);
            ligne.HorizontalAlignment = HorizontalAlignment.Left;
            grdPlacementPion.Children.Add(ligne);

            ligne = CreerLigneGrille(0, false);
            ligne.VerticalAlignment = VerticalAlignment.Top;
            grdPlacementPion.Children.Add(ligne);
        }

        /// <summary>
        /// Crée un fond selon le type de terrain que la case est. 
        /// </summary>
        /// <param name="colonne">L'axe x dans lequel la case est positionnée.</param>
        /// <param name="rangee">L'axe y dans lequel la case est positionnée.</param>
        /// <returns></returns>
        private Rectangle CreerFondCase(int colonne, int rangee)
        {
            Rectangle rect = new Rectangle();

            rect.Width = TAILLE_CASES_GRILLE;
            rect.Height = TAILLE_CASES_GRILLE;

            rect.Fill = Brushes.OliveDrab;

            Grid.SetZIndex(rect, 0);
            Grid.SetColumn(rect, colonne);
            Grid.SetRow(rect, rangee);

            return rect;
        }

        /// <summary>
        /// Crée une ligne selon la position verticale ou horizontale demandée.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="estColonne">Indique si la ligne doit être créée horizontalement ou verticalement</param>
        /// <returns></returns>
        private Rectangle CreerLigneGrille(int position, bool estColonne)
        {
            Rectangle ligne = new Rectangle();
            ligne.Fill = Brushes.Gainsboro;
            Grid.SetZIndex(ligne, 1);

            if (estColonne)
            {
                ligne.Width = 1;
                ligne.Height = 10 * TAILLE_CASES_GRILLE;
                ligne.HorizontalAlignment = HorizontalAlignment.Right;
                Grid.SetColumn(ligne, position);
                Grid.SetRow(ligne, 0);
                Grid.SetRowSpan(ligne, 10);
            }
            else
            {
                ligne.Width = 10 * TAILLE_CASES_GRILLE;
                ligne.Height = 1;
                ligne.VerticalAlignment = VerticalAlignment.Bottom;
                Grid.SetColumn(ligne, 0);
                Grid.SetColumnSpan(ligne, 10);
                Grid.SetRow(ligne, position);
            }

            return ligne;
        }

        /// <summary>
        /// Crée l'image dans la grille de jeu.
        /// </summary>
        /// <param name="pieceAffichage">La pièce avec laquelle on y crée une image dans la grille</param>
        /// <returns></returns>
        private Image CreerAffichagePiece(Piece pieceAffichage)
        {
            Image ImageAffichage = new Image();
            BitmapImage bitmap = new BitmapImage();
            // Sert seulement à aller chercher des fonctions
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(RetournerNomSourceImage(pieceAffichage.couleur, pieceAffichage), UriKind.Relative);
            bitmap.EndInit();

            ImageAffichage.Stretch = Stretch.Fill;
            ImageAffichage.Source = bitmap;

            ImageAffichage.HorizontalAlignment = HorizontalAlignment.Center;
            ImageAffichage.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetZIndex(ImageAffichage, 2);

            return ImageAffichage;
        }


        /// <summary>
        /// Retourne la source de l'image selon son type et son état de visibilité
        /// </summary>
        /// <param name="couleur"></param>
        /// <param name="pieceAAfficher"></param>
        /// <returns></returns>
        private string RetournerNomSourceImage(Couleur couleur, Piece pieceAAfficher)
        {
            // On met le chemin relatif de l'image
            StringBuilder nomImageSource = new StringBuilder();
            if (couleur == Couleur.Rouge)
                nomImageSource.Append("Images/Rouge/");
            else
                nomImageSource.Append("Images/Bleu/");

            string nomImageSourceFinal;

            // On s'assure que le nom de la pièce soit en minuscule
            string nomPiece = pieceAAfficher.GetType().ToString();
            nomPiece = nomPiece.Substring(9);
            nomPiece = nomPiece.ToLower();

            // On l'ajoute 
            nomImageSource.Append(nomPiece);

            if (couleur == Couleur.Rouge)
                nomImageSource.Append("R.png");
            else
                nomImageSource.Append("B.png");

            // On converti en string pour le retourner
            nomImageSourceFinal = nomImageSource.ToString();

            return nomImageSourceFinal;
        }

        /// <summary>
        /// Crée un rectangle de sélection pour chaque case et y insère une fonction pour activé la sélection de la case
        /// lorsque le joueur clic avec la souris.
        /// </summary>
        private void DefinirZoneSelectionGrille()
        {
            Rectangle rect;

            for (int i = 0; i < TAILLE_GRILLE_POSITIONNEMENT_X; i++)
            {
                for (int j = 0; j < TAILLE_GRILLE_POSITIONNEMENT_Y; j++)
                {
                    rect = new Rectangle();

                    rect.Width = TAILLE_CASES_GRILLE;
                    rect.Height = TAILLE_CASES_GRILLE;
                    rect.Fill = Brushes.Transparent;
                    Grid.SetZIndex(rect, 5);
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, j);

                    grdPlacementPion.Children.Add(rect);

                    rect.MouseLeftButtonUp += PositionnerPionGrille;
                    rect.MouseRightButtonUp += PositionnerPionGrille;

                }

            }

        }

    }

    
}
