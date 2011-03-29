using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmartPacMan
{
	/// <summary>
	/// Generic character structure.
	/// </summary>
	public class Character
	{
		public Character()
		{
			effect = SpriteEffects.None;
		}

		/*********************************************************************/
		/* Class Variables */
		/*********************************************************************/
		public Map.Pos pos;

		/// <summary>
		/// Character's last performed move. Auxiliary variable to help scene drwiwng process.
		/// </summary>
		public Pathfinder.Direction lastMove { get; set; }

		/// <summary>
		/// Character's current sprite effect. Auxiliary variable to help scene drawing process.
		/// </summary>
		public SpriteEffects effect { get; set; }
	}
}
