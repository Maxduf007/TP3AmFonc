using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type capitaine
    /// </summary>
    public class Capitaine : PieceMobile
   {
      public Capitaine(Couleur couleurPiece) : base(couleurPiece, 6)
      {    
      }
   }
}
