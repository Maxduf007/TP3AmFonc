using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
    /// <summary>
    /// Règle de positionnement des pions pour l'IA
    /// 1. Le drapeau doit être positionné dans la dernière ligne en arrière du front
    /// 2. Le drapeau doit être derrière un des deux lacs
    /// 3. 3 bombes doivent entourées le drapeau. Dont une à droite, une en haut et une à gauche.
    /// 4. 2 bombes doivent être placé sur le front à l'entrée de l'une des 3 accès.
    /// </summary>
   public class IA_Stratego : IObserver<JeuStrategoControl>
   {
      #region Code relié au patron observateur

      private IDisposable unsubscriber;

      public void Subscribe(IObservable<JeuStrategoControl> provider)
      {
         unsubscriber = provider.Subscribe(this);
      }

      public void Unsubscribe()
      {
         unsubscriber.Dispose();
      }

      public void OnCompleted()
      {
         // Ne fait rien pour l'instant.
      }

      public void OnError(Exception error)
      {
         // Ne fait rien pour l'instant.
      }

      public void OnNext(JeuStrategoControl g)
      {
         JouerCoup(g);
      }
      #endregion

      private JeuStrategoControl Jeu { get; set; }

      public Couleur CouleurIA { get; set; }

        public int AxeX = 10;
        public int AxeY = 4;

        //public IA_Stratego(JeuStrategoControl jeu) : this(jeu, Couleur couleur) { }

            /// <summary>
            /// Constructeur de l'IA où l'on initialise la partie dans lequel il joue et sa couleur.
            /// </summary>
            /// <param name="jeu">le jeuStrategoControl dans lequel l'IA va intéragir</param>
            /// <param name="couleur">la couleur qui lui est imposé</param>
      public IA_Stratego(JeuStrategoControl jeu, Couleur couleur)
      {
         Jeu = jeu;
         CouleurIA = couleur;

         // Abonner l'IA à l'interface du jeu.
         jeu.Subscribe(this);
      }

        /// <summary>
        /// l'IA prend au hasard un coup à jouer parmi la liste des coups permis.
        /// </summary>
        /// <param name="jeu">Le jeu dans lequel le coup est effectué par l'IA</param>
      private void JouerCoup(JeuStrategoControl jeu)
      {
            ReponseDeplacement reponse = new ReponseDeplacement();
         List<List<Coordonnee>> ListeCoupsPermis;
         Random rnd = new Random(DateTime.Now.Millisecond);
         int choixRnd;

         ListeCoupsPermis = TrouverCoupsPermis(jeu.GrillePartie);

         choixRnd = rnd.Next(0, ListeCoupsPermis.Count);
  
            reponse = jeu.ExecuterCoup(ListeCoupsPermis[choixRnd][0], ListeCoupsPermis[choixRnd][1]);

      }

        /// <summary>
        /// Vérifie dans toutes la grille du jeu tous les coups possibles de chaque case et retourne une liste des coups permis par l'IA
        /// </summary>
        /// <param name="grillePartie">la GrilleJeu de la partie</param>
        /// <returns></returns>
      private List<List<Coordonnee>> TrouverCoupsPermis(GrilleJeu grillePartie)
      {
         List<List<Coordonnee>> listeCoups = new List<List<Coordonnee>>();
         Coordonnee coordonneeDepart, coordonneeCible;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               coordonneeDepart = new Coordonnee(i, j);
                   
               if (Jeu.GrillePartie.EstCaseOccupee(coordonneeDepart) 
                  && Jeu.GrillePartie.ObtenirCouleurPiece(coordonneeDepart) == CouleurIA && Jeu.GrillePartie.ObtenirPiece(coordonneeDepart) is PieceMobile)
               {
                  // Valider un coup vers la gauche.
                  coordonneeCible = new Coordonnee(coordonneeDepart.X - 1, coordonneeDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(coordonneeDepart, coordonneeCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { coordonneeDepart, coordonneeCible });
                  }

                  // Valider un coup vers l'avant.
                  coordonneeCible = new Coordonnee(coordonneeDepart.X, coordonneeDepart.Y - 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(coordonneeDepart, coordonneeCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { coordonneeDepart, coordonneeCible });
                  }

                  // Valider un coup vers la droite.
                  coordonneeCible = new Coordonnee(coordonneeDepart.X + 1, coordonneeDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(coordonneeDepart, coordonneeCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { coordonneeDepart, coordonneeCible });
                  }

                  // Valider un coup vers l'arrière.
                  coordonneeCible = new Coordonnee(coordonneeDepart.X, coordonneeDepart.Y + 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(coordonneeDepart, coordonneeCible))
                  {
                     listeCoups.Add(new List<Coordonnee>() { coordonneeDepart, coordonneeCible });
                  }
               }
            }
         }

         return listeCoups;
      }

        /// <summary>
        /// Replace de façon aléatoire les pièces de LstPieceDisponible pour l'IA. Par contre, le positionnement doit 
        /// respecter 4 conditions qui ont été mentionné au dessus de la déclaration de la classe.
        /// </summary>
        /// <param name="LstPieceDisponible">la liste de pièce à repositionner</param>
        public void PreparerPositionsPieces(List<Piece> LstPieceDisponible)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int iChoixRandom;

            Piece[,] tabPiecePositionnee = new Piece[AxeX, AxeY];
            Piece test = new Marechal(Couleur.Bleu);

            PositionnerDrapeau(tabPiecePositionnee, LstPieceDisponible);
            PositionnerBombes(tabPiecePositionnee, LstPieceDisponible);

            // On place chaque pion qui reste
            for(int i = 0; i < AxeX; i++)
            {
                for (int j = 0; j < AxeY && LstPieceDisponible.Count != 0; j++)
                {
                    iChoixRandom = rnd.Next(LstPieceDisponible.Count);

                    Piece PieceAPositionner = LstPieceDisponible[iChoixRandom];

                    if(tabPiecePositionnee[i, j] == null)
                    {
                        tabPiecePositionnee[i, j] = PieceAPositionner;
                        LstPieceDisponible.Remove(PieceAPositionner);
                    }

                }

            }

            // On remet les pièces placé dans LstPieceDisponible
            for(int y = 0; y < AxeY; y++)
            {
                for (int x = 0; x < AxeX; x++)
                {
                    LstPieceDisponible.Add(tabPiecePositionnee[x, y]);
                }
            }

        }

        /// <summary>
        /// On place le drapeau sur la dernière ligne derrière le front et derrière un lac.
        /// </summary>
        /// <param name="tabPiecePositionnee"> tableau final des pièces positionnées sur le jeu</param>
        /// <param name="LstPieceDisponible"> Liste de pièces qui restent à positionnées</param>
        private void PositionnerDrapeau(Piece[,] tabPiecePositionnee, List<Piece> LstPieceDisponible)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int iChoixRandomX;

            int[] tabPositionsPossiblesAxeX = { 2, 3, 6, 7 };

            iChoixRandomX = rnd.Next(tabPositionsPossiblesAxeX.Length);

            // On cherche dans la liste des pièces disponibles la pièce positionnée pour la retirer des disponibles.
            foreach (Piece PieceDispo in LstPieceDisponible)
            {
                if (PieceDispo is Drapeau)
                {
                    tabPiecePositionnee[tabPositionsPossiblesAxeX[iChoixRandomX], 0] = PieceDispo;
                    LstPieceDisponible.Remove(PieceDispo);
                    break;
                }
            }
            
        }

        /// <summary>
        /// 3 bombes doivent minimalement entourer le drapeau. 2 doit être placé sur la ligne de front devant des ouvertures.
        /// </summary>
        /// <param name="tabPiecePositionnee"> tableau final des pièces positionnées sur le jeu</param>
        /// <param name="LstPieceDisponible"> Liste de pièces qui restent à positionnées</param>
        private void PositionnerBombes(Piece[,] tabPiecePositionnee, List<Piece> LstPieceDisponible)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int iChoixRandomX;

            int[,] TabChoixPositionsBombes = { { 0, 1 }, { 4, 5 }, { 8, 9 } };
            Coordonnee coordonneeBombe = new Coordonnee(0, 0);

            Bombe BombeDispo = new Bombe(CouleurIA);
            Coordonnee coordonneeDrapeau = new Coordonnee(0, 0);
 
            // On trouve les coordonnées du drapeau
            for(int i = 0; i < AxeX; i++)
            {
                for (int j = 0; j < AxeY; j++)
                {
                    if(tabPiecePositionnee[i, j] is Drapeau)
                    {
                        coordonneeDrapeau.X = i;
                        coordonneeDrapeau.Y = j;
                    }
                }
            }

            // On positionne 3 bombes autour du drapeau
            PositionnerBombesAutourDrapeau(tabPiecePositionnee, LstPieceDisponible, coordonneeDrapeau);

            // On choisit au hasard la position des 2 dernières bombes à placer
            iChoixRandomX = rnd.Next(3);

            for(int i = 0; i < 2; i++)
            {
                // On vérifie et enlève un bombe de la liste de pieces diponibles
                foreach (Piece PieceDispo in LstPieceDisponible)
                {
                    if (PieceDispo is Bombe)
                    {
                        BombeDispo = (Bombe)PieceDispo;
                        LstPieceDisponible.Remove(BombeDispo);
                        break;
                    }
                }

                int Xsenti = TabChoixPositionsBombes[iChoixRandomX, i];

                // Une fois qu'on a la bombe et la coordonnée X, on place la bombe dans la tabPiecePositionnee
                tabPiecePositionnee[Xsenti, AxeY - 1] = BombeDispo;

            }
           
        }

        /// <summary>
        /// Place les bombes selon les règles définis au début de la déclaration de classe. 
        /// </summary>
        /// <param name="tabPiecePositionnee">tableau temporaire pour les nouvelles positions des pièces</param>
        /// <param name="LstPieceDisponible">liste de pièces qui reste à placer</param>
        /// <param name="coordonneeDrapeau">la coordonnée du drapeau dans le tableau temporaire pour l'entourer de bombes</param>
        private void PositionnerBombesAutourDrapeau(Piece[,] tabPiecePositionnee, List<Piece> LstPieceDisponible, Coordonnee coordonneeDrapeau)
        {
            Bombe BombeDispo = new Bombe(CouleurIA);

            for (int i = 0; i < 3; i++)
            {
                // On regarde en premier si on a une bombe de disponible
                foreach (Piece PieceDispo in LstPieceDisponible)
                {

                    if (PieceDispo is Bombe)
                    {
                        BombeDispo = (Bombe)PieceDispo;
                        LstPieceDisponible.Remove(BombeDispo);
                        break;
                    }
                }

                //Ensuite, on place la bombe
                switch (i)
                {
                    case 0:
                        tabPiecePositionnee[coordonneeDrapeau.X - 1, coordonneeDrapeau.Y] = BombeDispo; // à gauche du drapeau
                        break;
                    case 1:
                        tabPiecePositionnee[coordonneeDrapeau.X, coordonneeDrapeau.Y + 1] = BombeDispo; // en haut du drapeau
                        break;
                    case 2:
                        tabPiecePositionnee[coordonneeDrapeau.X + 1, coordonneeDrapeau.Y] = BombeDispo; // à droite du drapeau
                        break;
                }


            }

        }

    }

 
}
