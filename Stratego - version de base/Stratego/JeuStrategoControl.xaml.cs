using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
   /// Logique d'interaction pour JeuStrategoControl.xaml
   /// </summary>
   public partial class JeuStrategoControl : UserControl
   {
      #region Static

      private const int TAILLE_CASES_GRILLE = 50;

      #endregion

      public GrilleJeu GrillePartie { get; private set; }

      private List<List<Label>> GrillePieces { get; set; }

      private Rectangle SelectionActive { get; set; }

      public Couleur TourJeu { get; private set; }

      #region Code relié au patron observateur

      List<IObserver<JeuStrategoControl>> observers;

      // Oui, une classe privée (et interne).
      private class Unsubscriber : IDisposable
      {
         private List<IObserver<JeuStrategoControl>> _observers;
         private IObserver<JeuStrategoControl> _observer;

         public Unsubscriber(List<IObserver<JeuStrategoControl>> observers, IObserver<JeuStrategoControl> observer)
         {
            this._observers = observers;
            this._observer = observer;
         }

         public void Dispose()
         {
            if (!(_observer == null)) _observers.Remove(_observer);
         }
      }

      public IDisposable Subscribe(IObserver<JeuStrategoControl> observer)
      {
         if (!observers.Contains(observer))
            observers.Add(observer);

         return new Unsubscriber(observers, observer);
      }

      private void Notify()
      {
         foreach (IObserver<JeuStrategoControl> ob in observers)
         {
            ob.OnNext(this);
         }
      }
      #endregion

      private IA_Stratego IA { get; set; }

      public JeuStrategoControl()
      {
         InitializeComponent();

         GrillePartie = new GrilleJeu();

         DiviserGrilleJeu();
         ColorerGrilleJeu();
         DefinirZoneSelectionGrille();
         InitialiserSelectionActive();

         PositionnerPieces();
         InitialiserAffichagePieces();

         #region Tests

         // Code des tests initiaux.
         /*
         ReponseDeplacement deplacement;

         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(0, 6), new Coordonnee(0, 5)); // Deplacement

         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(0, 5), new Coordonnee(-1, 5)); // Coord invalide
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 6), new Coordonnee(2, 5)); // Lac

         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 6), new Coordonnee(3, 6)); // Piece vs sa propre couleur

         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 6), new Coordonnee(1, 5));
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 5), new Coordonnee(1, 4));
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 4), new Coordonnee(1, 3)); // Prise par attaquant

         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 3), new Coordonnee(1, 2));
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 2), new Coordonnee(1, 1));
         // deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 1), new Coordonnee(1, 0)); // 2 pièces éliminées
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 1), new Coordonnee(2, 1));
         deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 1), new Coordonnee(2, 0)); // Attaquant éliminé
         */

         #endregion

         TourJeu = Couleur.Rouge;

         // Initialise la liste d'observateurs.
         observers = new List<IObserver<JeuStrategoControl>>();

         // Initialiser l'IA.
         IA = new IA_Stratego(this);
      }

      /// <summary>
      /// Cette méthode existe principalement pour que le jeu soit testable.
      /// On ne veut évidemment pas toujours commencer une partie avec exactement les même positions.
      /// </summary>
      private void PositionnerPieces()
      {
         List<Piece> piecesRouges = new List<Piece>() { new Marechal(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge)
                                                , new Sergent(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Colonel(Couleur.Rouge), new Colonel(Couleur.Rouge), new General(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Lieutenant(Couleur.Rouge)
                                                , new Commandant(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Commandant(Couleur.Rouge), new Espion(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Bombe(Couleur.Rouge), new Sergent(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Eclaireur(Couleur.Rouge)
                                                , new Commandant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Drapeau(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Demineur(Couleur.Rouge)
                                                };

         List<Piece> piecesBleues = new List<Piece>() { new Commandant(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu), new Drapeau(Couleur.Bleu)
                                                , new Capitaine(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Sergent(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                , new Eclaireur(Couleur.Bleu), new Commandant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Marechal(Couleur.Bleu), new Commandant(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Demineur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu)
                                                , new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Demineur(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Espion(Couleur.Bleu), new General(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                };

         GrillePartie.PositionnerPieces(piecesRouges, Couleur.Rouge);
         GrillePartie.PositionnerPieces(piecesBleues, Couleur.Bleu);
      }

      private void DiviserGrilleJeu()
      {
         ColumnDefinition colonneDef;
         RowDefinition ligneDef;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            colonneDef = new ColumnDefinition();
            colonneDef.Width = new GridLength(TAILLE_CASES_GRILLE);
            grdPartie.ColumnDefinitions.Add(colonneDef);

            ligneDef = new RowDefinition();
            ligneDef.Height = new GridLength(TAILLE_CASES_GRILLE);
            grdPartie.RowDefinitions.Add(ligneDef);
         }
      }

      private void ColorerGrilleJeu()
      {
         Rectangle ligne;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            grdPartie.Children.Add(CreerLigneGrille(i, true));

            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               grdPartie.Children.Add(CreerFondCase(i, j));

               if (i == 0)
               {
                  grdPartie.Children.Add(CreerLigneGrille(j, false));
               }
            }
         }

         ligne = CreerLigneGrille(0, true);
         ligne.HorizontalAlignment = HorizontalAlignment.Left;
         grdPartie.Children.Add(ligne);

         ligne = CreerLigneGrille(0, false);
         ligne.VerticalAlignment = VerticalAlignment.Top;
         grdPartie.Children.Add(ligne);
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

      private Rectangle CreerFondCase(int colonne, int rangee)
      {
         Rectangle rect = new Rectangle();

         rect.Width = TAILLE_CASES_GRILLE;
         rect.Height = TAILLE_CASES_GRILLE;

         if (GrillePartie.EstCoordonneeLac(new Coordonnee(colonne, rangee)))
         {
            rect.Fill = Brushes.CornflowerBlue;
         }
         else
         {
            rect.Fill = Brushes.OliveDrab;
         }

         Grid.SetZIndex(rect, 0);
         Grid.SetColumn(rect, colonne);
         Grid.SetRow(rect, rangee);

         return rect;
      }

      private void DefinirZoneSelectionGrille()
      {
         Rectangle rect;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               rect = new Rectangle();

               rect.Width = TAILLE_CASES_GRILLE;
               rect.Height = TAILLE_CASES_GRILLE;
               rect.Fill = Brushes.Transparent;
               Grid.SetZIndex(rect, 5);
               Grid.SetColumn(rect, i);
               Grid.SetRow(rect, j);

               grdPartie.Children.Add(rect);

               rect.MouseLeftButtonUp += ResoudreSelectionCase;
            }

         }

      }

      private void InitialiserSelectionActive()
      {
         SelectionActive = new Rectangle();

         SelectionActive.Width = TAILLE_CASES_GRILLE;
         SelectionActive.Height = TAILLE_CASES_GRILLE;
         SelectionActive.Fill = Brushes.Yellow;
         Grid.SetZIndex(SelectionActive, 0);
      }

      private void InitialiserAffichagePieces()
      {
         Coordonnee position;
         Label labelAffichage;

         GrillePieces = new List<List<Label>>();

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            GrillePieces.Add(new List<Label>());

            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               position = new Coordonnee(i, j);

               if (GrillePartie.EstCaseOccupee(position))
               {
                  labelAffichage = CreerAffichagePiece(GrillePartie.ObtenirPiece(position));

                  Grid.SetColumn(labelAffichage, i);
                  Grid.SetRow(labelAffichage, j);

                  grdPartie.Children.Add(labelAffichage);

                  GrillePieces[i].Add(labelAffichage);
               }
               else
               {
                  GrillePieces[i].Add(null);
               }
            }
         }
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

      private void ResoudreSelectionCase(object sender, MouseButtonEventArgs e)
      {
         Rectangle caseSelectionnee = (Rectangle)sender;

         Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
         Coordonnee pointActif;

         ReponseDeplacement reponse;

         if (TourJeu == Couleur.Rouge)
         {
            if (grdPartie.Children.Contains(SelectionActive))
            {
               pointActif = new Coordonnee(Grid.GetColumn(SelectionActive), Grid.GetRow(SelectionActive));

               if (pointSelectionne == pointActif)
               {
                  grdPartie.Children.Remove(SelectionActive);
               }
               else
               {
                  reponse = ExecuterCoup(pointActif, pointSelectionne);

                  if (reponse.DeplacementFait)
                  {
                     grdPartie.Children.Remove(SelectionActive);
                  }
               }
            }
            else
            {
               if (GrillePartie.EstCaseOccupee(pointSelectionne)
                  && GrillePartie.ObtenirCouleurPiece(pointSelectionne) == Couleur.Rouge)
               {
                  Grid.SetColumn(SelectionActive, (int)pointSelectionne.X);
                  Grid.SetRow(SelectionActive, (int)pointSelectionne.Y);

                  grdPartie.Children.Add(SelectionActive);
               }
            }
         }
      }

      public ReponseDeplacement ExecuterCoup(Coordonnee caseDepart, Coordonnee caseCible)
      {
         Thread executionIA = new Thread(LancerIA);

         ReponseDeplacement reponse = new ReponseDeplacement();

         Piece attaquant;
         Label affichageAttaquant;

         if (caseCible != caseDepart)
         {
            // Prendre les informations avant de faire le coup.
            attaquant = GrillePartie.ObtenirPiece(caseDepart);
            affichageAttaquant = GrillePieces[(int)caseDepart.X][(int)caseDepart.Y];
            reponse = GrillePartie.ResoudreDeplacement(caseDepart, caseCible);

            if (reponse.DeplacementFait)
            {

               // Retrait de la pièce attaquante de sa position d'origine.
               grdPartie.Children.Remove(affichageAttaquant);
               GrillePieces[(int)caseDepart.X][(int)caseDepart.Y] = null;

               if (reponse.PiecesEliminees.Count == 2)
               {
                  // Retrait de la pièce attaquée.
                  grdPartie.Children.Remove(GrillePieces[(int)caseCible.X][(int)caseCible.Y]);
                  GrillePieces[(int)caseCible.X][(int)caseCible.Y] = null;
               }
               else if (reponse.PiecesEliminees.Count == 1 && reponse.PiecesEliminees[0] != attaquant
                       || reponse.PiecesEliminees.Count == 0)
               {
                  // Remplacement de la pièce attaquée par la pièce attaquante.
                  grdPartie.Children.Remove(GrillePieces[(int)caseCible.X][(int)caseCible.Y]);
                  GrillePieces[(int)caseCible.X][(int)caseCible.Y] = null;

                  GrillePieces[(int)caseCible.X][(int)caseCible.Y] = affichageAttaquant;

                  Grid.SetColumn(affichageAttaquant, (int)caseCible.X);
                  Grid.SetRow(affichageAttaquant, (int)caseCible.Y);
                  grdPartie.Children.Add(affichageAttaquant);
               }

               // Permet de faire jouer l'IA.
               if (TourJeu == Couleur.Rouge)
               {
                  ChangerTourJeu();
                  executionIA.Start();
               }
               else
               {
                  ChangerTourJeu();
               }
            }
         }
         else
         {
            reponse.DeplacementFait = false;
         }

         return reponse;
      }

      private void LancerIA()
      {
         // Pause d'une seconde, pour permettre à l'humain de mieux comprendre le déroulement.
         Thread.Sleep(1000);

         Dispatcher.Invoke(() =>
         {
            Notify();
         });
      }

      public void ChangerTourJeu()
      { 
         if (TourJeu == Couleur.Rouge)
         {
            TourJeu = Couleur.Bleu;
         }
         else
         {
            TourJeu = Couleur.Rouge;
         }
      }
   }
}
