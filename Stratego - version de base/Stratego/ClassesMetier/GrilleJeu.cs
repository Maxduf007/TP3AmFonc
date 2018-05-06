using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
    /// <summary>
    /// Une grille de jeu contenant des caseJeu
    /// </summary>
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

        /// <summary>
        /// Permet d'initialiser une grille de 10 par 10 ayant des caseJeu initialisé avec des voisins et un type de terrain
        /// </summary>
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

        /// <summary>
        /// Lie chaque CaseJeu de la grille avec ses voisins le cas écheant.
        /// </summary>
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
        /// <summary>
        /// Vérifie si les coordonnées de départ et cible sont valide pour le déplacement d'une pièce.
        /// Par exemple, si la coordonnéeCible pointe sur une caseJeuCible ayant un lac, déplacement ne sera pas valide
        /// et on fait recommencer la sélection au joueur.
        /// Autre exemple, si le joueur sélectionne une caseJeu qui ne lui appartient pas, on lui fait recommencer sa sélection.
        /// 
        /// </summary>
        /// <param name="coordonneeDepart">coordonnée de départ de la pièce déplacée venant de la GrillePartie</param>
        /// <param name="coordonneeCible">coordonnée cible de la pièce déplacée venant de la GrillePartie</param>
        /// <returns></returns>
        public ReponseDeplacement ResoudreDeplacement(Coordonnee coordonneeDepart, Coordonnee coordonneeCible)
      {
         ReponseDeplacement reponse = new ReponseDeplacement();
         reponse.PiecesEliminees = new List<Piece>();

         CaseJeu caseDepart, caseCible;

            // On vérifie si les coordonnees ne dépassent pas les limites de la carte
         if (EstCoordonneeValide(coordonneeDepart) && EstCoordonneeValide(coordonneeCible))
         {
                // On obtient les adresses des cases de la grille avec lesquel on effectue les modifications
            caseDepart = GrilleCases[(int)coordonneeDepart.X][(int)coordonneeDepart.Y];
            caseCible = GrilleCases[(int)coordonneeCible.X][(int)coordonneeCible.Y];

                //Modif: Si la pièce est mobile, on effectue le déplacement
            if (caseDepart.EstOccupe() && EstDeplacementPermis(coordonneeDepart, coordonneeCible) && caseDepart.Occupant is PieceMobile)
            {

               // Faire le déplacement.
               reponse.PiecesEliminees = caseCible.ResoudreAttaque((PieceMobile)caseDepart.Occupant);

               caseDepart.Occupant = null;

                    // Si la pièce éliminée est un drapeau, on termine la partie.
                    if(reponse.PiecesEliminees.Count > 0 && reponse.PiecesEliminees[0] is Drapeau)
                        reponse.FinPartie = true;

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

        /// <summary>
        /// Vérifie si les coordonnées données sont à l'intérieur des limites de la taille de GrilleJeu, que les coordonnées ne sont pas des lacs
        /// et que le déplacement ce fait sans conflit entre la case de départ et la case cible.
        /// </summary>
        /// <param name="coordonneeDepart">coordonnée de départ venant de la GrillePartie</param>
        /// <param name="coordonneeCible">coordonnée cible venant de la GrillePartie</param>
        /// <returns></returns>
        public bool EstDeplacementPermis(Coordonnee coordonneeDepart, Coordonnee coordonneeCible)
      {

        return ( EstCoordonneeValide(coordonneeDepart) && EstCoordonneeValide(coordonneeCible)
            && !EstCoordonneeLac(coordonneeDepart) && !EstCoordonneeLac(coordonneeCible)
            && GrilleCases[(int)coordonneeDepart.X][(int)coordonneeDepart.Y].EstDeplacementLegal(GrilleCases[(int)coordonneeCible.X][(int)coordonneeCible.Y])
            );

      }

        /// <summary>
        /// Vérifie que la coordonnée passée en paramètre est situé dans les limites de la GrilleJeu
        /// </summary>
        /// <param name="c">coordonnée à vérifier </param>
        /// <returns></returns>
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

        /// <summary>
        /// Vérifie si la coordonnée correspond avec une caseJeu de type lac
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Vérifie si la case référée par la coordonnée en paramètre est occupé par une pièce
        /// </summary>
        /// <param name="c">coordonnée de la GrillePartie</param>
        /// <returns></returns>
      public bool EstCaseOccupee(Coordonnee c)
      {
         return ((GrilleCases[(int)c.X][(int)c.Y]).EstOccupe());
      }

        /// <summary>
        /// Effectue le positionnement de chaque pion selon le joueur à placer dans la GrilleJeu. 
        /// Un décalage de 6 est effectuer sur l'axe y pour commencer le positionnement des pièces du joueur humain en bas.
        /// </summary>
        /// <param name="lstPieces">Liste de pièce d'un joueur à positionner</param>
        /// <param name="PositionnementJoueur">true = on positionne le joueur false = on positionne l'IA</param>
        /// <returns></returns>
      public bool PositionnerPieces(List<Piece> lstPieces, bool PositionnementJoueur)
      {
         bool positionnementApplique = false;
            Piece pieceDeterminerCouleur = lstPieces.First();
            Couleur couleurPieceAPositionner = pieceDeterminerCouleur.couleur;

         int compteur = 0;
         int decallage = 0;
         
         if (PositionnementJoueur)
         {
            decallage = 6;
         }

         if (!PositionnementFait(couleurPieceAPositionner) && lstPieces.Count == 40)
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

        /// <summary>
        /// Vérifie si le positionnement d'une pièce lors du tour de positionnement d'un joueur a bien été effectué
        /// </summary>
        /// <param name="couleurJoueur">Couleur du joueur en positionnement</param>
        /// <returns></returns>
      private bool PositionnementFait(Couleur couleurJoueur)
      {
         bool pieceTrouvee = false;

         for (int i = 0; i < TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
            {
               if (GrilleCases[i][j].Occupant != null 
                     && ((GrilleCases[i][j].Occupant.EstDeCouleur(Couleur.Rouge) && couleurJoueur == Couleur.Rouge)
                        || (GrilleCases[i][j].Occupant.EstDeCouleur(Couleur.Bleu) && couleurJoueur == Couleur.Bleu)))
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

        /// <summary>
        /// Retourne la pièce qui occupe la caseJeu référée par la coordonnée
        /// </summary>
        /// <param name="c">coordonnée de la case dans la GrillePartie</param>
        /// <returns></returns>
      public Piece ObtenirPiece(Coordonnee c)
      {
         return GrilleCases[(int)c.X][(int)c.Y].Occupant;
      }

        /// <summary>
        /// Retourne la couleur de la pièce qui occupe la caseJeu référée par la coordonnée
        /// </summary>
        /// <param name="c">coordonnée de la case dans la GrillePartie</param>
        /// <returns></returns>
        public Couleur ObtenirCouleurPiece(Coordonnee c)
      {
         return GrilleCases[(int)c.X][(int)c.Y].Occupant.couleur;
      }

        /// <summary>
        /// Retourne la coordonnée de l'emplacement de la caseJeu
        /// </summary>
        /// <param name="caseJeuReferee">la caseJeu qu'on veut obtenir sa coordonnée</param>
        /// <returns></returns>
        public Coordonnee ObtenirCoordonneeCaseJeu(CaseJeu caseJeuReferee)
        {

            for(int i = 0; i < TAILLE_GRILLE_JEU; i++)
            {
                for (int j = 0; j < TAILLE_GRILLE_JEU; j++)
                {
                    if(GrilleCases[i][j] == caseJeuReferee)
                    {
                        return new Coordonnee(i, j);
                    }
                }
            }

            return null;
        }

   }
}
