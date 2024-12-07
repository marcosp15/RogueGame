using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueGame.Core
{
    public static class Data
    {
        public static int ScreenW {get; set;} = 1024;
        public static int ScreenH {get; set;} = 896;

        public static bool Exit {get; set;} = false;

        public enum SceneState {Menu, Game, Settings, GameOver}
        public static SceneState CurrentState {get; set;} = SceneState.Menu;
    }
}