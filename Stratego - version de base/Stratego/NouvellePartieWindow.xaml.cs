﻿using System;
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

        public Piece[,] TabPieceJoueurPositionnee = new Piece[TAILLE_GRILLE_POSITIONNEMENT_X, TAILLE_GRILLE_POSITIONNEMENT_Y];

        public Label[,] TabAffichagePion = new Label[TAILLE_GRILLE_POSITIONNEMENT_X, TAILLE_GRILLE_POSITIONNEMENT_Y];

        #region Static
        private const int TAILLE_GRILLE_POSITIONNEMENT_X = 10;
        private const int TAILLE_GRILLE_POSITIONNEMENT_Y = 4;
        #endregion

        public NouvellePartieWindow()
        {
            InitializeComponent();


            DiviserGrillePositionnement();
            ColorerGrilleJeu();
            DefinirZoneSelectionGrille();
        }

        private void btnLancerPartie_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(CouleurJoueurChoisi);

            this.Close();

            mainWindow.Show();

            

        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton) sender;

            if (radioButton == rdbRouge)
                CouleurJoueurChoisi = Couleur.Rouge;
            else
                CouleurJoueurChoisi = Couleur.Bleu;



        }

        /// <summary>
        /// Initialise l'objet ayant le type demandé par le joueur pour son positionnement dans le jeu
        /// </summary>
        /// <param name="NomPion"> reçoit le nom du pion pour sélectionner le bon type</param>
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
                case "Colonels":
                    PieceSelectionnee = new Colonel(CouleurJoueurChoisi);
                    break;
                case "Commandants":
                    PieceSelectionnee = new Commandant(CouleurJoueurChoisi);
                    break;
                case "Capitaines":
                    PieceSelectionnee = new Capitaine(CouleurJoueurChoisi);
                    break;
                case "Lieutenants":
                    PieceSelectionnee = new Lieutenant(CouleurJoueurChoisi);
                    break;
                case "Sergents":
                    PieceSelectionnee = new Sergent(CouleurJoueurChoisi);
                    break;
                case "Demineurs":
                    PieceSelectionnee = new Demineur(CouleurJoueurChoisi);
                    break;
                case "Eclaireurs":
                    PieceSelectionnee = new Eclaireur(CouleurJoueurChoisi);
                    break;
                case "Espion":
                    PieceSelectionnee = new Espion(CouleurJoueurChoisi);
                    break;
                case "Drapeau":
                    PieceSelectionnee = new Drapeau(CouleurJoueurChoisi);
                    break;
                case "Bombes":
                    PieceSelectionnee = new Bombe(CouleurJoueurChoisi);
                    break;
                default:
                    break;

            }
        }

        private void btnPlacerUnite_Click(object sender, RoutedEventArgs e)
        {
            Button PionBoutton = (Button)sender;
            string NomPionAPlacer = PionBoutton.Name.Substring(3);
            int NbPiecesDispo = Convert.ToInt16(PionBoutton.Content);

            if(NbPiecesDispo > 0)
            {

                // On enlève une pièce disponible pour le type de pion sélectionné
                NbPiecesDispo -= 1;
                // On met à jour le contenu du boutton pour indiquer le nb de pièces restantes
                PionBoutton.Content = NbPiecesDispo.ToString();
                // On initialise le type d'objet Piece pour le placer dans la grille
                TypePieceSelectionnee(NomPionAPlacer);
            }



        }

        private void PositionnerPionGrille(object sender, MouseButtonEventArgs e)
        {
            // Reçoit le rectangle sélectionner par la souris
            Rectangle caseSelectionnee = (Rectangle)sender;
            Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
            Label labelAffichage;

            if (/*e.LeftButton == MouseButtonState.Pressed &&*/ PieceSelectionnee != null)
            {
                // On place dans la grille de positionnement la pièce sélectionnée
                TabPieceJoueurPositionnee[pointSelectionne.X, pointSelectionne.Y] = PieceSelectionnee;

                TabAffichagePion[pointSelectionne.X, pointSelectionne.Y] = CreerAffichagePiece(PieceSelectionnee);

                labelAffichage = TabAffichagePion[pointSelectionne.X,pointSelectionne.Y];

                Grid.SetColumn(labelAffichage, pointSelectionne.X);
                Grid.SetRow(labelAffichage, pointSelectionne.Y);

                grdPlacementPion.Children.Add(labelAffichage);

                PieceSelectionnee = null;
            }


        }

        /*private void MouseClickRight_event(object sender, MouseButtonEventArgs e)
        {
            Button PionBoutton = (Button)sender;
            string SymbolePionAPlacer = PionBoutton.Name.Substring(3, 1);
            int NbPiecesDispo = Convert.ToInt16(PionBoutton.Content);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                // On remet une pièce disponible pour le type de pion sélectionné
                NbPiecesDispo += 1;
                // On met à jour le contenu du boutton pour indiquer le nb de pièces restantes
                PionBoutton.Content = NbPiecesDispo.ToString();
            }


        }*/

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

        private Label CreerAffichagePiece(Piece pieceAffichage)
        {
            Label labelAffichage = new Label();

            if (pieceAffichage is Bombe)
            {
                labelAffichage.Content = "B";
            }
            else if (pieceAffichage is Drapeau)
            {
                labelAffichage.Content = "D";
            }
            else
            {
                PieceMobile PieceMobileAffichage = (PieceMobile)pieceAffichage;
                labelAffichage.Content = PieceMobileAffichage.Force;
            }

            labelAffichage.FontSize = TAILLE_CASES_GRILLE * 0.6;
            labelAffichage.FontWeight = FontWeights.Bold;

            if (pieceAffichage.EstDeCouleur(Couleur.Rouge))
            {
                labelAffichage.Foreground = Brushes.DarkRed;
            }
            else
            {
                labelAffichage.Foreground = Brushes.Navy;
            }

            labelAffichage.HorizontalAlignment = HorizontalAlignment.Center;
            labelAffichage.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetZIndex(labelAffichage, 2);

            return labelAffichage;
        }

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

                }

            }

        }

    }

    
}
