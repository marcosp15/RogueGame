using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueGame.Entities;
using RogueGame.Rooms;

namespace RogueGame.Core
{
    public class SceneManager
    {
        private SpriteFont _font;
        private string _gameOverMessage = "PERDISTE MAMAHUEVO";

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Arial");
        }

        public void Update(GameTime gameTime, RoomManager roomManager, Player player)
        {
            switch (Data.CurrentState)
            {
                case Data.SceneState.Menu:
                    UpdateMenu();
                    break;
                case Data.SceneState.Game:
                    roomManager.Update(gameTime, player);
                    if (!player.IsAlive)
                        Data.CurrentState = Data.SceneState.GameOver;
                    break;
                case Data.SceneState.GameOver:
                    UpdateGameOver();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, RoomManager roomManager, Player player)
        {
            switch (Data.CurrentState)
            {
                case Data.SceneState.Menu:
                    DrawMenu(spriteBatch);
                    break;
                case Data.SceneState.Game:
                    roomManager.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;
                case Data.SceneState.GameOver:
                    DrawGameOver(spriteBatch);
                    break;
            }
        }

        private void UpdateMenu() 
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Data.CurrentState = Data.SceneState.Game;
            }
        }
         private void UpdateGameOver()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Data.CurrentState = Data.SceneState.Menu;
            }
        }
        private void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Presiona Enter para jugar", new Vector2(300, 200), Color.White);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _gameOverMessage, new Vector2(200, 200), Color.Red);
        }
    }

}