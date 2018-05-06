using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type colonel
    /// </summary>
    public class Colonel : PieceMobile
   {
      public Colonel(Couleur couleurPiece) : base(couleurPiece, 8)
      {
      }
   }
}
