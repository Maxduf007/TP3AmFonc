using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
   public class CaseJeu
   {
      public CaseJeu VoisinAvant { get; set; }
      public CaseJeu VoisinArriere { get; set; }
      public CaseJeu VoisinGauche { get; set; }
      public CaseJeu VoisinDroite { get; set; }

      public Piece Occupant { get; set; }

      public string TypeCase { get; set; }

      public CaseJeu(string type)
      {
         TypeCase = type;
      }

      public bool EstOccupe()
      {
         return (Occupant != null);
      }

      public List<Piece> ResoudreAttaque(PieceMobile attaquant)
      {
         List<Piece> piecesEliminees = new List<Piece>();

         if (Occupant != null)
         {
                // Dans le cas que l'occupant est une pièce mobile, on compare la force
            if(Occupant is PieceMobile)
            {
                PieceMobile OccupantMobile = (PieceMobile)Occupant;

                if (attaquant.Force < OccupantMobile.Force)
                {
                    piecesEliminees.Add(attaquant);
                }
                else if (attaquant.Force > OccupantMobile.Force)
                {
                    piecesEliminees.Add(Occupant);
                    Occupant = attaquant;
                }
                else
                {
                    piecesEliminees.Add(attaquant);
                    piecesEliminees.Add(Occupant);
                    Occupant = null;
                }
            }
            else
            {
                if(Occupant is Bombe)
                {
                    if (attaquant is Demineur)
                        piecesEliminees.Add(Occupant);
                    else
                        piecesEliminees.Add(attaquant);
                }
                else
                {
                    piecesEliminees.Add(Occupant);
                }
            }
            
         }
         else
         { 
            Occupant = attaquant;
         }

         return piecesEliminees;
      }

      public bool EstVoisineDe(CaseJeu caseCible)
      {
         if ( caseCible != null
            && (this.VoisinGauche == caseCible || this.VoisinAvant == caseCible
               || this.VoisinDroite == caseCible || this.VoisinArriere == caseCible)
            )
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      public bool EstDeplacementLegal(CaseJeu caseCible)
      {
         bool resultat = false;

         if (this.EstVoisineDe(caseCible))
         {
            if (!caseCible.EstOccupe()
               || !this.Occupant.EstDeCouleur(caseCible.Occupant.couleur))
            {
               resultat = true;
            }
         }

         return resultat;
      }
   }
}
