using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type lieutenant
    /// </summary>
    public class Lieutenant : PieceMobile
   {
      public Lieutenant(Couleur couleurPiece) : base(couleurPiece, 5)
      {                           
      }
   }
}
