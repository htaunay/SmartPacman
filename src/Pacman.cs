using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPacMan
{
	/// <summary>
	/// Main charatcer class in game. Controlled by user.
	/// </summary>
	public class Pacman : Character
	{
		public Pacman()
		{
			state = PacmanState.Closed;
			lastMove = Pathfinder.Direction.Right;
		}

		/*********************************************************************/
		/* Class Types */
		/*********************************************************************/
		/// <summary>
		/// Pacman's current state. Basically varies between open and closed mouth.
		/// </summary>
		public enum PacmanState
		{
			Open,
			Closed
		};

		/*********************************************************************/
		/* Class Variables */
		/*********************************************************************/
		public PacmanState state { get; set; }

		/*********************************************************************/
		/* Class Methods */
		/*********************************************************************/
		/// <summary>
		/// Inverts Pacman's current state.
		/// </summary>
		public void switchState()
		{
			if (state == PacmanState.Open)
				state = PacmanState.Closed;
			else
				state = PacmanState.Open;
		}
	}
}
