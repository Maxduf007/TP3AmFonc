using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une class abtraite piece 
    /// </summary>
    public abstract class Piece
   {
      public Couleur couleur { get; private set; }

        public bool EstRevele { get; set; }

        /// <summary>
        /// On oblige la définition de la couleur de la pièce créée
        /// </summary>
        /// <param name="couleurPiece"> On passe la couleur du joueur qui détient la piece </param>
      public Piece(Couleur couleurPiece)
      {
         couleur = couleurPiece;
            EstRevele = false;
         //Force = forcePiece;
      }
        /// <summary>
        /// Vérifie si la couleur passée en paramètre est égale à la couleur de la pièce
        /// </summary>
        /// <param name="couleurRecherche"> couleur comparée </param>
        /// <returns></returns>
        public bool EstDeCouleur(Couleur couleurRecherche)
        {
            return (couleur == couleurRecherche);
        }

        

    }
}
