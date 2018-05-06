using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type commandant
    /// </summary>
    public class Commandant : PieceMobile
   {
      public Commandant(Couleur couleurPiece) : base(couleurPiece, 7)
      {                           
      }
   }
}
