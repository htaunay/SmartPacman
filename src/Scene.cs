using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPacMan
{
	/// <summary>
	/// Generic scene object, that contains a game map, a list of Ghost
	/// characters, and who else than Pacman himself \o/.
	/// </summary>
	public class Scene
	{
		public Scene()
		{
			map		= new Map();
			pacman	= new Pacman();
			ghosts	= new List<Ghost>();
		}

		/*********************************************************************/
		/* Class Variables */
		/*********************************************************************/
		public Map map;
		public Pacman pacman;
		public List<Ghost> ghosts;

		/*********************************************************************/
		/* Class Methods */
		/*********************************************************************/
		/// <summary>
		/// Loads a scene file, containg map info, such as walls, empty spaces,
		/// and eventually ghost/pacman initial/respawning points.
		/// For now, only the characters initial positions are set in the scene
		/// file.
		/// See more info on how to create a scene file in README.
		/// </summary>
		/// <param name="fileName">Scene filename to be loaded, preferably located 
		/// in the games 'Contents' folder.</param>
		public bool loadSceneFile( String fileName )
		{
			// Create file path string.
			StringBuilder path = new StringBuilder();
			path.Append("Content/");
			path.Append(fileName);

			// Load file's first line with width/height info
			StreamReader tr = new StreamReader(path.ToString());
			String[] firstLine = tr.ReadLine().Split(' ');

			map.width = int.Parse(firstLine[0]);
			map.height = int.Parse(firstLine[1]);
			map.spaces = new Map.SpaceType[map.width, map.height];

			// Loads map info
			String otherLine;
			try
			{
				for (int i = 0; i < map.height; i++)
				{
					otherLine = tr.ReadLine();
					for (int j = 0; j < map.width; j++)
					{
						switch (otherLine[j])
						{
							case 'P':
								map.spaces[j, i] = Map.SpaceType.Empty;
								pacman.pos.x = j;
								pacman.pos.y = i;
								break;

							case 'G':
								map.spaces[j, i] = Map.SpaceType.Empty;
								ghosts.Add(new Ghost(i * j));
								ghosts.ElementAt(ghosts.Count - 1).pos.x = j;
								ghosts.ElementAt(ghosts.Count - 1).pos.y = i;
								break;

							case '-':
								map.spaces[j, i] = Map.SpaceType.Wall;
								break;

							case '.':
								map.spaces[j, i] = Map.SpaceType.Empty;
								break;

							default:
								break;
						};
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				// Scene file loading error - probably inconsistent data!!!
				String debugmsg = ex.Message;
				return false;
			}
		}
	}
}
