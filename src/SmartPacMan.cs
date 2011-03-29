using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SmartPacMan
{
    /// <summary>
    /// Simple Pacman-like game, inspired by a under-grad assignment
	/// envolving developing a A* search based game.
	/// Developed by: Henrique Taunay @ PUC-Rio
	/// Version 0.1 Date: 28/03/2011
    /// </summary>
    public class SmartPacMan : Microsoft.Xna.Framework.Game
    {
		/*********************************************************************/
		/* Generated Variables */
		/*********************************************************************/
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		/*********************************************************************/
		/* Custom Variables */
		/*********************************************************************/
		private SceneManager sm;
		private Scene scene1;

		private TimeSpan onedecsec; // 0.1 seconds
		private TimeSpan control; // Game control timer

		KeyboardState kbState;

		public const int SQUARE_SIZE = 30; // pixels for the side of square in game

		/*********************************************************************/
		/* Generated Methods */
		/*********************************************************************/
        public SmartPacMan()
        {
			// Initializing default variables
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			// Initializing custom variables
			sm = SceneManager.getInstance();
			sm.setContentManager( Content );
			scene1 = new Scene();

			onedecsec = new TimeSpan(1000000);
			control = new TimeSpan(0);
        }

        /// <summary>
        /// Initializes basic window and loads map of current scene from file.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "SmartPacMan";

			//scene1.loadSceneFile( "mapok.txt" );
			//scene1.loadSceneFile("mapnotok.txt");
			//scene1.loadSceneFile("openmap.txt");
			scene1.loadSceneFile("classic.txt");
			sm.currentScene = scene1;

			graphics.PreferredBackBufferWidth = scene1.map.width * SQUARE_SIZE;
			graphics.PreferredBackBufferHeight = scene1.map.height * SQUARE_SIZE;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// Loads all necessary textures into SceneManager
        /// </summary>
        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			sm.loadTextures();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Nothing to do here for now...
        }

        /// <summary>
        /// Processes mouse and keyboard inputs, and updates the user's actions,
		/// as well as the IA evolution. Everything (until now) frame'rate
		/// independent.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			// Process user input
			kbState = Keyboard.GetState();
			processKeyboard( kbState );

			// Updates time controller with elapsed time since last interation.
			// Actions occur/are treated every 0.1 seconds.
			control = control.Add(gameTime.ElapsedGameTime);

			if (!sm.paused && (onedecsec.CompareTo(control) == -1))
			{
				// Updates characters
				sm.updatePacman(kbState);
				sm.updateGhosts();
				
				// Reset time controller
				control = control.Subtract( control );
			}

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws sprites (characters,map) to canvas.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
			sm.drawMap(spriteBatch);
			sm.drawPacman(spriteBatch);
			sm.drawGhosts(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /*********************************************************************/
        /* Custom Methods */
        /*********************************************************************/
		/// <summary>
		/// Processes generic keyboard input. Pacman motion input is treated
		/// inside SceneManager.
		/// </summary>
		/// <param name="kbState">Provides a snapshot of the keyboard's state.</param>
		private void processKeyboard(KeyboardState kbState)
		{
			if (kbState.IsKeyDown(Keys.Enter))
			{
				if (sm.paused)
					sm.paused = false;
				else
					sm.paused = true;
			}
		}
    }
}
