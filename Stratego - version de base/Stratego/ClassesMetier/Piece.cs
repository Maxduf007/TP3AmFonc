using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
   public abstract class Piece
   {
      public Couleur couleur { get; private set; }


      public Piece(Couleur couleurPiece)
      {
         couleur = couleurPiece;
         //Force = forcePiece;
      }

      /*public bool EstRouge()
      {
         return (couleur == Couleur.Rouge);
      }*/

      /*public bool EstBleu()
      {
         return !EstRouge();
      }*/

        public bool EstDeCouleur(Couleur couleurRecherche)
        {
            return (couleur == couleurRecherche);
        }

    }
}
