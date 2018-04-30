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

        public bool EstRevele { get; set; }


      public Piece(Couleur couleurPiece)
      {
         couleur = couleurPiece;
            EstRevele = false;
         //Force = forcePiece;
      }

        public bool EstDeCouleur(Couleur couleurRecherche)
        {
            return (couleur == couleurRecherche);
        }

        

    }
}
