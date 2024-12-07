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
    private Player _player;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _sceneManager = new SceneManager();
        _roomManager = new RoomManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D playerTexture = Content.Load<Texture2D>("player");
        _player = new Player(playerTexture,new Vector2(100,100));

        _sceneManager.LoadContent(Content);
        _roomManager.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _sceneManager.Update(gameTime, _roomManager, _player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        _sceneManager.Draw(_spriteBatch, _roomManager, _player);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
