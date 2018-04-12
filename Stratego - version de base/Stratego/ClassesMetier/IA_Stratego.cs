using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stratego
{
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

      private string CouleurIA { get; set; }

      public IA_Stratego(JeuStrategoControl jeu) : this(jeu, "Bleu") { }

      public IA_Stratego(JeuStrategoControl jeu, string couleur)
      {
         Jeu = jeu;
         CouleurIA = couleur;

         // Abonner l'IA à l'interface du jeu.
         jeu.Subscribe(this);
      }

      private void JouerCoup(JeuStrategoControl jeu)
      {
         List<List<Point>> ListeCoupsPermis;
         Random rnd = new Random(DateTime.Now.Millisecond);
         int choixRnd;

         ListeCoupsPermis = TrouverCoupsPermis(jeu.GrillePartie);

         choixRnd = rnd.Next(0, ListeCoupsPermis.Count);
         jeu.ExecuterCoup(ListeCoupsPermis[choixRnd][0], ListeCoupsPermis[choixRnd][1]);
      }

      private List<List<Point>> TrouverCoupsPermis(GrilleJeu grillePartie)
      {
         List<List<Point>> listeCoups = new List<List<Point>>();
         Point pointDepart, pointCible;

         for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
         {
            for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
            {
               pointDepart = new Point(i, j);

               if (Jeu.GrillePartie.EstCaseOccupee(pointDepart) 
                  && Jeu.GrillePartie.ObtenirCouleurPiece(pointDepart) == "Bleu")
               {
                  // Valider un coup vers la gauche.
                  pointCible = new Point(pointDepart.X - 1, pointDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Point>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers l'avant.
                  pointCible = new Point(pointDepart.X, pointDepart.Y - 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Point>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers la droite.
                  pointCible = new Point(pointDepart.X + 1, pointDepart.Y);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Point>() { pointDepart, pointCible });
                  }

                  // Valider un coup vers l'arrière.
                  pointCible = new Point(pointDepart.X, pointDepart.Y + 1);
                  if (Jeu.GrillePartie.EstDeplacementPermis(pointDepart, pointCible))
                  {
                     listeCoups.Add(new List<Point>() { pointDepart, pointCible });
                  }
               }
            }
         }

         return listeCoups;
      }
   }
}
