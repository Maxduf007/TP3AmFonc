using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
   public class GrilleJeu
   {
      #region Static

      /// <summary>
      /// La taille de la grille de jeu. Assume une grille de jeu carrée (X par X).
      /// </summary>
      public const int TAILLE_GRILLE_JEU = 10;

      #endregion
      private List<List<CaseJeu>> GrilleCases { get; set; }

      public GrilleJeu()
      {
         InitialiserGrille();
      }

      private void InitialiserGrille()
      {
         List<CaseJeu> colonne;
         GrilleCases = new List<List<CaseJeu>>();

         // Créer les cases et les structurer dans une grille à deux dimensions.
         for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
         {
            colonne = new List<CaseJeu>();

            for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
            {
               // Coordonnées des lacs : I (2, 3, 6, 7) - J (4, 5)
               if ((i == 2 || i == 3 || i == 6 || i == 7) && (j == 4 || j == 5))
               {
                  colonne.Add(new CaseJeu("Lac"));
               }
               else
               {
                  colonne.Add(new CaseJeu("Terrain"));
               }
            }

            GrilleCases.Add(colonne);
         }

         // Créer les liens de voisinage entre les cases de la grille.
         LierCasesGrille();
      }

      private void LierCasesGrille()
      {
         for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
            {
               // Les coins.
               if ((i == 0 || i == TAILLE_GRILLE_JEU - 1) && (j == 0 || j == TAILLE_GRILLE_JEU - 1))
               {
                  if (i == 0)
                  {
                     GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                  }
                  else
                  {
                     GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                  }

                  if (j == 0)
                  {
                     GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                  }
                  else 
                  {
                     GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                  }
               }
               // Côtés verticaux.
               else if (i == 0 || i == TAILLE_GRILLE_JEU - 1)
               {
                  if (i == 0)
                  {
                     GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                     GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                     GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                  }
                  else
                  {
                     GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                     GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                     GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                  }
               }
               // Côtés horizontaux.
               else if (j == 0 || j == TAILLE_GRILLE_JEU - 1)
               {
                  if (j == 0)
                  {
                     GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                     GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                     GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
                  }
                  else
                  {
                     GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                     GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                     GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                  }
               }
               else 
               {
                  GrilleCases[i][j].VoisinGauche = GrilleCases[i - 1][j];
                  GrilleCases[i][j].VoisinAvant = GrilleCases[i][j - 1];
                  GrilleCases[i][j].VoisinDroite = GrilleCases[i + 1][j];
                  GrilleCases[i][j].VoisinArriere = GrilleCases[i][j + 1];
               }
            }
         }
      }

      public ReponseDeplacement ResoudreDeplacement(Coordonnee coordonneeDepart, Coordonnee coordonneeCible)
      {
         ReponseDeplacement reponse = new ReponseDeplacement();
         reponse.PiecesEliminees = new List<Piece>();

         CaseJeu caseDepart, caseCible;

         if (EstCoordonneeValide(coordonneeDepart) && EstCoordonneeValide(coordonneeCible))
         {
            caseDepart = GrilleCases[(int)coordonneeDepart.X][(int)coordonneeDepart.Y];
            caseCible = GrilleCases[(int)coordonneeCible.X][(int)coordonneeCible.Y];

            if (caseDepart.EstOccupe() && EstDeplacementPermis(coordonneeDepart, coordonneeCible))
            {
               // Faire le déplacement.
               reponse.PiecesEliminees = caseCible.ResoudreAttaque(caseDepart.Occupant);
               caseDepart.Occupant = null;

               reponse.DeplacementFait = true;
            }
            else
            {
               reponse.DeplacementFait = false;
            }
         }
         else
         {
            reponse.DeplacementFait = false;
         }

         return reponse;
      }

      public bool EstDeplacementPermis(Coordonnee coordonneeDepart, Coordonnee coordonneeCible)
      {
         return ( EstCoordonneeValide(coordonneeDepart) && EstCoordonneeValide(coordonneeCible)
                && !EstCoordonneeLac(coordonneeDepart) && !EstCoordonneeLac(coordonneeCible)
                && GrilleCases[(int)coordonneeDepart.X][(int)coordonneeDepart.Y].EstDeplacementLegal(GrilleCases[(int)coordonneeCible.X][(int)coordonneeCible.Y])
                );
      }

      private bool EstCoordonneeValide(Coordonnee c)
      {
         if ((c.X >= 0 && c.X < TAILLE_GRILLE_JEU) && (c.Y >= 0 && c.Y < TAILLE_GRILLE_JEU))
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      public bool EstCoordonneeLac(Coordonnee c)
      {
         // Coordonnées des lacs : I (2, 3, 6, 7) - J (4, 5)
         if ((c.X == 2 || c.X == 3 || c.X == 6 || c.X == 7) && (c.Y == 4 || c.Y == 5))
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      public bool EstCaseOccupee(Coordonnee c)
      {
         return ((GrilleCases[(int)c.X][(int)c.Y]).EstOccupe());
      }

      public bool PositionnerPieces(List<Piece> lstPieces, Couleur couleurJoueur)
      {
         bool positionnementApplique = false;

         int compteur = 0;
         int decallage = 0;
         
         if (couleurJoueur == Couleur.Rouge)
         {
            decallage = 6;
         }

         if (!PositionnementFait(couleurJoueur) && lstPieces.Count == 40)
         {
            positionnementApplique = true;

            for (int j = 0 + decallage; j < 4 + decallage; j++)
            {
               for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
               {
                  GrilleCases[i][j].Occupant = lstPieces[compteur];

                  compteur++;
               }
            }
         }

         return positionnementApplique;
      }

      private bool PositionnementFait(Couleur couleurJoueur)
      {
         bool pieceTrouvee = false;

         for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
            {
               if (GrilleCases[i][j].Occupant != null 
                     && ((GrilleCases[i][j].Occupant.EstRouge() && couleurJoueur == Couleur.Rouge)
                        || (GrilleCases[i][j].Occupant.EstBleu() && couleurJoueur == Couleur.Bleu)))
               {
                  pieceTrouvee = true;

                  // Inutile de chercher plus.
                  j = TAILLE_GRILLE_JEU;
                  i = TAILLE_GRILLE_JEU;
               }
            }
         }

         return pieceTrouvee;
      }

      public Piece ObtenirPiece(Coordonnee c)
      {
         return GrilleCases[(int)c.X][(int)c.Y].Occupant;
      }

      public Couleur ObtenirCouleurPiece(Coordonnee c)
      {
         return GrilleCases[(int)c.X][(int)c.Y].Occupant.couleur;
      }

   }
}
