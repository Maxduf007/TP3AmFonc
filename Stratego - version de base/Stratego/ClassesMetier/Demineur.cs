using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type demineur
    /// </summary>
    public class Demineur : PieceMobile
   {
      public Demineur(Couleur couleurPiece) : base(couleurPiece, 3)
      {
      }
   }
}
