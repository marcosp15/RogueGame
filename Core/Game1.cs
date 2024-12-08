using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueGame.Entities;
using RogueGame.Rooms;

namespace RogueGame.Core;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;
    private RoomManager _roomManager;
    public Player _player;
    public MiniMap _miniMap;
    public static SpriteFont font {get; set;}

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = Data.ScreenH;
        _graphics.PreferredBackBufferWidth = Data.ScreenW;
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _sceneManager = new SceneManager();
        _roomManager = new RoomManager(Content, _miniMap);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("Arial");
        Texture2D playerTexture = Content.Load<Texture2D>("player");
        Texture2D proyectilTexture = Content.Load<Texture2D>("defaultProyectil");
        Texture2D miniMapRoomTexture = Content.Load<Texture2D>("minimapTexture");
        _player = new Player(playerTexture,Data.ScreenCenter);
        _player.ProyectilTexture = proyectilTexture;
        _miniMap = new MiniMap(miniMapRoomTexture, new Vector2(24,24), _roomManager.GetRooms());

        _sceneManager.LoadContent(Content,_roomManager);
        
    }

    protected override void Update(GameTime gameTime)
    {
        _sceneManager.Update(gameTime, _roomManager, _player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkKhaki);
        _spriteBatch.Begin();
        _sceneManager.Draw(_spriteBatch, _roomManager, _player);
        _miniMap.Draw(_spriteBatch,_roomManager.GetCurrentRoom());
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
