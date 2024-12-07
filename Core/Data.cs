using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueGame.Core
{
    public static class Data
    {
        public static int ScreenW {get;} = 1200;
        public static int ScreenH {get;} = 800;
        public static Microsoft.Xna.Framework.Vector2 ScreenCenter = new Microsoft.Xna.Framework.Vector2(ScreenW/2, ScreenH/2);
        public static bool Exit {get; set;} = false;

        public enum SceneState {Menu, Game, Settings, GameOver}
        public static SceneState CurrentState {get; set;} = SceneState.Game;
    }
}