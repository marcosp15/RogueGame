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
using RogueGame.Core;

namespace RogueGame.Core
{
    public class SceneManager
    {
        private string _gameOverMessage = "PERDISTE MAMAHUEVO";

        public void LoadContent(ContentManager content, RoomManager roomManager)
        {
            if (Data.CurrentState == Data.SceneState.Game)
                roomManager.LoadContent(content);
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
                    player.Update(gameTime,player);
                    if (!player.IsAlive)
                        Data.CurrentState = Data.SceneState.GameOver;
                    break;
                case Data.SceneState.GameOver:
                    UpdateGameOver(roomManager, player);
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
        private void UpdateGameOver(RoomManager roomManager, Player player)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Data.CurrentState = Data.SceneState.Menu;
                player.Health = 3;
                player.Position = Data.ScreenCenter;

                roomManager.ResetRoom(player);
            }
        }
        private void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, "Presiona Enter para jugar", Data.ScreenCenter, Color.White);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, _gameOverMessage, Data.ScreenCenter, Color.Red);
        }
    }

}