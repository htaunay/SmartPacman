using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPacMan
{
	/// <summary>
	/// Class representing a scene's map, consisted of a 2D grid, where each
	/// space has a space type, currently divided between empty spaces and walls.
	/// </summary>
    public class Map
    {
        /*********************************************************************/
        /* Class Types */
        /*********************************************************************/
		/// <summary>
		/// Objects's current position.
		/// </summary>
		public struct Pos
		{
			public int x;
			public int y;

			public Pos(int nx, int ny)
			{
				x = nx;
				y = ny;
			}

			public static Pos operator +(Pos p1, Pos p2)
			{
				return new Pos(p1.x + p2.x, p1.y + p2.y);
			}

			public static Pos operator -(Pos p1, Pos p2)
			{
				return new Pos(p1.x - p2.x, p1.y - p2.y);
			}

			public static bool operator ==(Pos p1, Pos p2)
			{
				return ((p1.x == p2.x) && (p1.y == p2.y));
			}

			public static bool operator !=(Pos p1, Pos p2)
			{
				return ((p1.x != p2.x) || (p1.y != p2.y));
			}
		}
		
		public enum SpaceType
		{
            Empty,
            Wall
        };

        /*********************************************************************/
        /* Class Variables */
        /*********************************************************************/
        public int height { get; set; }
		public int width { get; set; }

        public SpaceType[,] spaces;

		/// <summary>
		/// Auxiliary 2d unitary values.
		/// </summary>
		public static Pos unitx = new Map.Pos(1, 0);
		public static Pos unity = new Map.Pos(0, 1);

        /*********************************************************************/
        /* Class Methods */
        /*********************************************************************/
		public bool setSpaceType( SpaceType st, int x, int y )
        {
            spaces[x,y] = st;
            return true;
        }

        public SpaceType getSpaceType( Pos pos )
        {
			if ((pos.x > (width - 1)) || (pos.x < 0) || (pos.y > (height - 1)) || (pos.y < 0))
				return SpaceType.Empty;

			return spaces[pos.x, pos.y];
        }
    }
}
