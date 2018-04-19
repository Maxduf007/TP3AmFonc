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

      public Couleur CouleurIA { get; set; }

        //public IA_Stratego(JeuStrategoControl jeu) : this(jeu, Couleur couleur) { }

      public IA_Stratego(JeuStrategoControl jeu, Couleur couleur)
      {
         Jeu = jeu;
         CouleurIA = couleur;

         // Abonner l'IA à l'interface du jeu.
         jeu.Subscribe(this);
      }

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
   }
}
