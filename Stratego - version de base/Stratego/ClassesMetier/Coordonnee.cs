using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    /// <summary>
    /// Coordonnée indiquée par un x et un y
    /// </summary>
    public class Coordonnee
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// La construction de la coordonnée doit obtenir un x et un y. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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
