using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public class Coordonnee
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordonnee(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Permet de vérifier si deux coordonnees ont les mêmes x et y
        /// </summary>
        /// <param name="coordonnee">l'objet coordonnee avec laquel on compare</param>
        /// <returns></returns>
        public bool EstEgal(Coordonnee coordonnee)
        {
            return (X == coordonnee.X && Y == coordonnee.Y);
        }
    }
}
