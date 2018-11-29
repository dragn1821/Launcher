using Microsoft.Xna.Framework;

namespace GameLibrary.Input
{
  public class Controller
  {
    private KeyboardMapping mapping;
    private KeyboardInput input;
    //private Vector2 direction;

    //public Vector2 Direction  { get { return direction; } }

    public Controller(KeyboardMapping mapping, KeyboardInput input)
    {
      this.mapping   = mapping;
      this.input     = input;
      //this.direction = Vector2.Zero;
    }

    #region Public Methods

    //public void Update(GameTime gameTime)
    //{
    //  direction = Vector2.Zero;

    //  if (input.IsKeyDown(mapping.Up))
    //  {
    //    direction.Y = -1;
    //  }
    //  else if (input.IsKeyDown(mapping.Down))
    //  {
    //    direction.Y = 1;
    //  }

    //  if (input.IsKeyDown(mapping.Right))
    //  {
    //    direction.X = 1;
    //  }
    //  else if (input.IsKeyDown(mapping.Left))
    //  {
    //    direction.X = -1;
    //  }
    //}

    public bool IsDirectionUp()
    {
      return input.IsKeyPressed(mapping.Up);
    }

    public bool IsDirectionDown()
    {
      return input.IsKeyPressed(mapping.Down);
    }

    public bool IsDirectionLeft()
    {
      return input.IsKeyPressed(mapping.Left);
    }

    public bool IsDirectionRight()
    {
      return input.IsKeyPressed(mapping.Right);
    }

    public bool IsButton1Down()
    {
      return input.IsKeyDown(mapping.Button1);
    }

    public bool IsButton1Pressed()
    {
      return input.IsKeyPressed(mapping.Button1);
    }

    public bool IsButton1Released()
    {
      return input.IsKeyReleased(mapping.Button1);
    }

    public bool IsButton2Down()
    {
      return input.IsKeyDown(mapping.Button2);
    }

    public bool IsButton2Pressed()
    {
      return input.IsKeyPressed(mapping.Button2);
    }

    public bool IsButton2Released()
    {
      return input.IsKeyReleased(mapping.Button2);
    }

    #endregion
  }
}