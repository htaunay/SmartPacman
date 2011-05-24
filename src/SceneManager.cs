using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SmartPacMan
{
	/// <summary>
	/// As the name says, class that manages everything in a current scene 
	/// in the game, from dealing with game logic and user input, to loading
	/// textures into memory and drawing the scene on the canvas, this is your guy ;).
	/// Oh, and its a singleton to, no need for +1 SceneManager, just change the current scene.
	/// </summary>
	class SceneManager
	{
		private SceneManager()
		{
			paused = true;
		}

		/*********************************************************************/
		/* Class Variables */
		/*********************************************************************/
		private static SceneManager instance; // used in singleton

		public Scene currentScene;
		public bool paused; // if game is paused or not

		// Graphic related variables
		private ContentManager content;
		private Texture2D emptyTexture;
		private Texture2D openPacman;
		private Texture2D closedPacman;
		private Texture2D ghostTexture;
		private Texture2D pissedGhostTexture;
		private Texture2D wallSimpleUp;
		private Texture2D wallSimpleDown;	
		private Texture2D wallSimpleLeft;	
		private Texture2D wallSimpleRight;
		private Texture2D wallLineHorizontal;
		private Texture2D wallLineVertical;
		private Texture2D wallLUp;
		private Texture2D wallLDown;
		private Texture2D wallLLeft;
		private Texture2D wallLRight;
		private Texture2D wallTUp;
		private Texture2D wallTDown;
		private Texture2D wallTLeft;
		private Texture2D wallTRight;
		private Texture2D wallCross;
		private Texture2D wallDot;

		/*********************************************************************/
		/* Class Methods */
		/*********************************************************************/
		/// <summary>
		/// Returns if the given ghost managed to reach Pac-man.
		/// </summary>
		private bool pacmanReached(Ghost ghost)
		{
			if ((ghost.pos.x == currentScene.pacman.pos.x) &&
				(ghost.pos.y == currentScene.pacman.pos.y) )
				return true;

			return ghost.calcPathToPac(currentScene.map, currentScene.pacman.pos);
		}

		/// <summary>
		/// Returns if the given space is empty or not.
		/// </summary>
		private bool hasWall(Map.Pos space)
		{
			if (currentScene.map.getSpaceType( space) == Map.SpaceType.Wall)
				return true;

			return false;
		}

		private bool hasSomeone(Map.Pos pos)
		{
			if (currentScene.pacman.pos == pos)
				return true;

			foreach (Ghost ghost in currentScene.ghosts)
				if (ghost.pos == pos)
					return true;

			return false;
		}

		/// <summary>
		/// Returns a wall texture, taking in account its adjacent sapces.
		/// Look, I know im loading the same textures flipped 3 times, but
		/// honestly, each one is like 1K, fucking 1K. I rather load 1K four
		/// times, and flip the images in Gimp, than implement a 100 line logic
		/// code that does is automatically.
		/// </summary>
		private Texture2D getWallTexture(SpriteBatch sb, Map.Pos pos )
		{
			Map.SpaceType a, b, c, d;

			a = currentScene.map.getSpaceType(pos - Map.unity);
			b = currentScene.map.getSpaceType(pos - Map.unitx);
			c = currentScene.map.getSpaceType(pos + Map.unitx);
			d = currentScene.map.getSpaceType(pos + Map.unity);

			if (a == Map.SpaceType.Empty)
			{
				if (b == Map.SpaceType.Empty)
				{
					if (c == Map.SpaceType.Empty)
					{
						if (d == Map.SpaceType.Empty)
							return wallDot;
						else
							return wallSimpleUp;
					}
					else
					{
						if (d == Map.SpaceType.Empty)
							return wallSimpleLeft;
						else
							return wallLDown;
					}
				}
				else
				{
					if (c == Map.SpaceType.Empty)
					{
						if (d == Map.SpaceType.Empty)
							return wallSimpleRight;
						else
							return wallLLeft;
					}
					else
					{
						if (d == Map.SpaceType.Empty)
							return wallLineHorizontal;
						else
							return wallTDown;
					}
				}
			}
			else
			{
				if (b == Map.SpaceType.Empty)
				{
					if (c == Map.SpaceType.Empty)
					{
						if (d == Map.SpaceType.Empty)
							return wallSimpleDown;
						else
							return wallLineVertical;
					}
					else
					{
						if (d == Map.SpaceType.Empty)
							return wallLRight;
						else
							return wallTRight;
					}
				}
				else
				{
					if (c == Map.SpaceType.Empty)
					{
						if (d == Map.SpaceType.Empty)
							return wallLUp;
						else
							return wallTLeft;
					}
					else
					{
						if (d == Map.SpaceType.Empty)
							return wallTUp;
						else
							return wallCross;
					}
				}
			}
		}

		//TODO
		private void moveGhost(Ghost ghost, Pathfinder.Direction dir)
		{
			bool moved = false;

			switch (dir)
			{
				case Pathfinder.Direction.Left:
					if (!hasSomeone(ghost.pos - Map.unitx))
					{
						ghost.pos.x--;
						moved = true;
					}
					break;

				case Pathfinder.Direction.Right:
					if (!hasSomeone(ghost.pos + Map.unitx))
					{
						ghost.pos.x++;
						moved = true;
					}
					break;

				case Pathfinder.Direction.Up:
					if (!hasSomeone(ghost.pos - Map.unity))
					{
						ghost.pos.y--;
						moved = true;
					}
					break;

				case Pathfinder.Direction.Down:
					if (!hasSomeone(ghost.pos + Map.unity))
					{
						ghost.pos.y++;
						moved = true;
					}
					break;

				default:
					break;
			}

			if (moved)
				ghost.lastMove = dir;
		}

		/// <summary>
		/// Returns a instane of the SceneManager.
		/// </summary>
		public static SceneManager getInstance()
		{
			if (instance == null)
				instance = new SceneManager();

			return instance;
		}

		/// <summary>
		/// Sets the games content manager to the scenemanager as well, so 
		/// that it can load the textures.
		/// </summary>
		/// <param name="cm">Game's content manager, inherited by Game class.</param>
		public void setContentManager(ContentManager cm)
		{
			content = cm;
		}

		/// <summary>
		/// Loads the games textures/sprites into memory.
		/// </summary>
		public void loadTextures()
		{
			emptyTexture		= content.Load<Texture2D>("Sprites/empty");
			openPacman			= content.Load<Texture2D>("Sprites/pacmanopen");
			closedPacman		= content.Load<Texture2D>("Sprites/pacmanclosed");
			ghostTexture		= content.Load<Texture2D>("Sprites/ghost");
			pissedGhostTexture	= content.Load<Texture2D>("Sprites/pissedghost");
			wallSimpleUp		= content.Load<Texture2D>("Sprites/mapSimpleUp");
			wallSimpleDown		= content.Load<Texture2D>("Sprites/mapSimpleDown");
			wallSimpleLeft		= content.Load<Texture2D>("Sprites/mapSimpleLeft");
			wallSimpleRight		= content.Load<Texture2D>("Sprites/mapSimpleRight");
			wallLineHorizontal	= content.Load<Texture2D>("Sprites/mapLineHorizontal");
			wallLineVertical	= content.Load<Texture2D>("Sprites/mapLineVertical");
			wallLUp				= content.Load<Texture2D>("Sprites/mapLUp");
			wallLDown			= content.Load<Texture2D>("Sprites/mapLDown");
			wallLLeft			= content.Load<Texture2D>("Sprites/mapLLeft");
			wallLRight			= content.Load<Texture2D>("Sprites/mapLRight");
			wallTUp				= content.Load<Texture2D>("Sprites/mapTUp");
			wallTDown			= content.Load<Texture2D>("Sprites/mapTDown");
			wallTLeft			= content.Load<Texture2D>("Sprites/mapTLeft");
			wallTRight			= content.Load<Texture2D>("Sprites/mapTRight");
			wallCross			= content.Load<Texture2D>("Sprites/mapCross");
			wallDot				= content.Load<Texture2D>("Sprites/mapDot");
																			  
		}

		/// <summary>
		/// Updates all of the ghost's positions in the current scene, using
		/// their own A* pathfinder.
		/// </summary>
		public void updateGhosts()
		{
			foreach (Ghost ghost in currentScene.ghosts)
			{
				// Got that bastard!!!
				if (pacmanReached(ghost))
				{
					// set on future respawn position
				}
				// Move torward pacman, almost there!!!
				else if (ghost.getPathToPac() != null)
				{
					ghost.state = Ghost.GhostState.OK;
					Pathfinder.Direction dir = ghost.getPathToPac().Pop();
					ghost.lastMove = dir;

					switch (dir)
					{
						case Pathfinder.Direction.Left:
							moveGhost(ghost, Pathfinder.Direction.Left);
							break;

						case Pathfinder.Direction.Right:
							moveGhost(ghost, Pathfinder.Direction.Right);
							break;

						case Pathfinder.Direction.Up:
							moveGhost(ghost, Pathfinder.Direction.Up);
							break;

						case Pathfinder.Direction.Down:
							moveGhost(ghost, Pathfinder.Direction.Down);
							break;

						default:
							break;
					}
				}
				// Fuck..., no way to pacman
				else
					ghost.state = Ghost.GhostState.Pissed;
			}
		}

		/// <summary>
		/// Updates Pacman's postion on the current scene using the user's input.
		/// </summary>
		/// <param name="ks">PC keyboard's current state.</param>
		public void updatePacman(KeyboardState ks)
		{
			if (ks.IsKeyDown(Keys.Left) && !hasWall(currentScene.pacman.pos - Map.unitx))
			{
				currentScene.pacman.pos.x--;
				currentScene.pacman.lastMove = Pathfinder.Direction.Left;
				currentScene.pacman.switchState();
			}

			if (ks.IsKeyDown(Keys.Right) && !hasWall(currentScene.pacman.pos + Map.unitx))
			{
				currentScene.pacman.pos.x++;
				currentScene.pacman.lastMove = Pathfinder.Direction.Right;
				currentScene.pacman.switchState();
			}

			if (ks.IsKeyDown(Keys.Up) && !hasWall(currentScene.pacman.pos - Map.unity))
			{
				currentScene.pacman.pos.y--;
				currentScene.pacman.lastMove = Pathfinder.Direction.Down;
				currentScene.pacman.switchState();
			}

			if (ks.IsKeyDown(Keys.Down) && !hasWall(currentScene.pacman.pos + Map.unity))
			{
				currentScene.pacman.pos.y++;
				currentScene.pacman.lastMove = Pathfinder.Direction.Up;
				currentScene.pacman.switchState();
			}
		}

		/// <summary>
		/// Draws the map into the game's canvas. Until now, the map only presents
		/// walls and empty spaces.
		/// This step is VERY INNEFICIENT, I am aware. The maps walls never change, and they are
		/// 'discovered' every time. I should be able to save the background texture as a whole
		/// and only redraw it. But first, I must find out how =P
		/// </summary>
		/// <param name="sb">Game's main sprite batch.</param>
		public void drawMap( SpriteBatch sb)
		{
			Texture2D auxText = emptyTexture;
			Map.Pos auxPos;

			for (int x = 0; x < currentScene.map.width; x++)
			{
				for (int y = 0; y < currentScene.map.height; y++)
				{
					auxPos.x = x; auxPos.y = y;
					switch (currentScene.map.getSpaceType(auxPos))
					{
						case Map.SpaceType.Empty:
							auxText = emptyTexture;
							break;

						case Map.SpaceType.Wall:
							auxText = getWallTexture(sb, auxPos);
							break;

						default:
							break;
					}

					sb.Draw(auxText, new Vector2(x * SmartPacMan.SQUARE_SIZE, y * SmartPacMan.SQUARE_SIZE), null, Color.White);
				}
			}
		}

		/// <summary>
		/// Draws the ghosts in the current scene onto the canvas.
		/// </summary>
		/// <param name="sb">Game's main sprite batch.</param>
		public void drawGhosts(SpriteBatch sb)
		{
			foreach( Ghost ghost in currentScene.ghosts )
			{
				Texture2D texture;

				if (ghost.state == Ghost.GhostState.OK)
					texture = ghostTexture;
				else
					texture = pissedGhostTexture;

				sb.Draw(texture, new Vector2(ghost.pos.x * SmartPacMan.SQUARE_SIZE, ghost.pos.y * SmartPacMan.SQUARE_SIZE), null, ghost.color);
			}
		}

		/// <summary>
		/// Draws pacman of the current scene onto the canvas.
		/// </summary>
		/// <param name="sb">Game's main sprite batch.</param>
		public void drawPacman(SpriteBatch sb)
		{
			Pacman pac = currentScene.pacman;
			int px = (int)pac.pos.x;
			int py = (int)pac.pos.y;

			Texture2D texture;
			if (pac.state == Pacman.PacmanState.Open)
				texture = openPacman;
			else
				texture = closedPacman;

			if( pac.lastMove == Pathfinder.Direction.Left )
				pac.effect = SpriteEffects.FlipHorizontally;
			else if( pac.lastMove == Pathfinder.Direction.Right )
				pac.effect = SpriteEffects.None;

			float rotation = 0;
			int flipped = 1;

			if(pac.effect == SpriteEffects.FlipHorizontally)
				flipped *= -1;

			if( pac.lastMove == Pathfinder.Direction.Up )
				rotation = MathHelper.ToRadians(flipped*90);
			else if( pac.lastMove == Pathfinder.Direction.Down )
				rotation = MathHelper.ToRadians(flipped *(-90));

			sb.Draw(texture, new Vector2((px * SmartPacMan.SQUARE_SIZE) + (SmartPacMan.SQUARE_SIZE / 2), (py * SmartPacMan.SQUARE_SIZE) + (SmartPacMan.SQUARE_SIZE/2)), null, Color.White, rotation, new Vector2(15, 15), 1, pac.effect, 0);
		}
	}
}
