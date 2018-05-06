using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type espion
    /// </summary>
    public class Espion : PieceMobile
   {
      public Espion(Couleur couleurPiece) : base(couleurPiece, 1)
      {                       
      }
   }
}
