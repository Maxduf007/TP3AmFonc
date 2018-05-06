using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Une piece mobile de type eclaireur
    /// </summary>
    public class Eclaireur : PieceMobile
   {
      public Eclaireur(Couleur couleurPiece) : base(couleurPiece, 2)
      {                          
      }
   }
}
