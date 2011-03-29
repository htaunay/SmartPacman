using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SmartPacMan
{
	/// <summary>
	/// Enemy class in game. Controlled by A* pathfining IA.
	/// </summary>
	public class Ghost : Character
	{
		/// <summary>
		/// Creates new ghost, assigning to it its own pathfinder, as well
		/// as a random color.
		/// </summary>
		/// <param name="seed">Seed value for random color generator.</param>
		public Ghost( int seed = 31415 )
		{
			state = GhostState.OK;
			pathfinder = new Pathfinder();
			Random rand = new Random( seed );
			color = new Color(	(byte)rand.Next(0, 255),
								(byte)rand.Next(0, 255),
								(byte)rand.Next(0, 255));
		}

		/*********************************************************************/
		/* Class Types */
		/*********************************************************************/
		/// <summary>
		/// Ghost's current state. Normally OK, but pissed if he can't reach Pacman.
		/// </summary>
		public enum GhostState
		{
			OK,
			Pissed
		};

		/*********************************************************************/
		/* Class Variables */
		/*********************************************************************/
		public Color color { get; set; }
		public GhostState state { get; set; }

		private Pathfinder pathfinder;
		private Stack<Pathfinder.Direction> pathToPac;

		/*********************************************************************/
		/* Class Methods */
		/*********************************************************************/
		/// <summary>
		/// Calculates and returns, if possible, best route for ghost to reach pacman.
		/// </summary>
		/// <param name="map">Map used in search algorithm.</param>
		/// <param name="pacpos">Pacman's position in map.</param>
		public bool calcPathToPac(Map map, Map.Pos pacpos)
		{
			pathToPac = pathfinder.findBestPath(map, this.pos, pacpos);

			if( (pathToPac != null) && (pathToPac.Count != 0) )
				return false;
			return true;
		}

		/// <summary>
		/// Returns Direction stack that represents the best path to reach pacman.
		/// </summary>
		public Stack<Pathfinder.Direction> getPathToPac()
		{
			return pathToPac;
		}
	}
}
