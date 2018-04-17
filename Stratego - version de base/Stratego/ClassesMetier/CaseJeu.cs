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

        public bool EstNull()
        {
            if (VoisinArriere == null || VoisinAvant == null || VoisinDroite == null || VoisinGauche == null)
                return true;
            else
                return false;
            
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


        // Dans le cas d'un éclaireur, on peut le faire avancer en ligne droite sans limite tant qu'aucun autre pion 
        // le bloque. 
            if(Occupant is Eclaireur)
            {

                if (AtteindreCaseCibleValide(VoisinAvant, caseCible, Direction.Avant)
                    || AtteindreCaseCibleValide(VoisinArriere, caseCible, Direction.Arriere)
                    || AtteindreCaseCibleValide(VoisinDroite, caseCible, Direction.Droite)
                    || AtteindreCaseCibleValide(VoisinGauche, caseCible, Direction.Gauche))
                {
                    resultat = true;
                }

            }
            else if (this.EstVoisineDe(caseCible))
            {
                if (!caseCible.EstOccupe()
                   || !this.Occupant.EstDeCouleur(caseCible.Occupant.couleur))
                {
                    resultat = true;
                }
            }

            return resultat;
      }

        public bool AtteindreCaseCibleValide(CaseJeu caseJeuVoisin, CaseJeu caseCible, Direction directionVoisin)
        {

            // On vérifie chaque case voisin
            // Elle ne doit pas être null ou occupée
            while (caseJeuVoisin != null && ((caseCible.EstOccupe() && !caseCible.Occupant.EstDeCouleur(Occupant.couleur)) || !caseCible.EstOccupe()) && (!caseJeuVoisin.EstOccupe() || caseJeuVoisin == caseCible))
            {
                
                // Si on atteint la caseCible, on autorise le déplacement
                if (caseJeuVoisin == caseCible)
                {
                    return true;
                }

                switch(directionVoisin)
                {
                    case Direction.Avant:
                        caseJeuVoisin = caseJeuVoisin.VoisinAvant;
                        break;
                    case Direction.Arriere:
                        caseJeuVoisin = caseJeuVoisin.VoisinArriere;
                        break;
                    case Direction.Droite:
                        caseJeuVoisin = caseJeuVoisin.VoisinDroite;
                        break;
                    case Direction.Gauche:
                        caseJeuVoisin = caseJeuVoisin.VoisinGauche;
                        break;
                    default:
                        break;
                }

                if(caseJeuVoisin == null)
                {
                    break;
                }
            }
            

            return false;
        }
   }
}
