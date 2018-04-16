using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une pièce mobile
    /// </summary>
    public abstract class PieceMobile : Piece
    {
        public int Force { get; private set; }

        public PieceMobile(Couleur couleurPiece, int forcePiece) : base(couleurPiece)
        {
            Force = forcePiece;
        }
    }
}
