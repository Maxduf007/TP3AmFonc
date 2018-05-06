using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une pièce mobile. Permet de créer une sous catégorie de pièce mobile.
    /// </summary>
    public abstract class PieceMobile : Piece
    {
        public int Force { get; private set; }

        /// <summary>
        /// On oblige de définir la force et la couleur lorsqu'une pièce mobile est instanciée
        /// </summary>
        /// <param name="couleurPiece"> la couleur qui définira la pièce</param>
        /// <param name="forcePiece">la force qui définira la pièce</param>
        public PieceMobile(Couleur couleurPiece, int forcePiece) : base(couleurPiece)
        {
            Force = forcePiece;
        }
    }
}
