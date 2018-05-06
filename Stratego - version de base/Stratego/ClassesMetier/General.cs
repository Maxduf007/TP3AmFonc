using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type general
    /// </summary>
    public class General : PieceMobile
   {
      public General(Couleur couleurPiece) : base(couleurPiece, 9)
      {     
      }
   }
}
