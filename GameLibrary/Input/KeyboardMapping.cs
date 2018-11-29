using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
  public class KeyboardMapping
  {
    public Keys Up      { get; private set; }
    public Keys Right   { get; private set; }
    public Keys Down    { get; private set; }
    public Keys Left    { get; private set; }
    public Keys Button1 { get; private set; }
    public Keys Button2 { get; private set; }

    public KeyboardMapping(Keys up, Keys right, Keys down, Keys left, Keys button1, Keys button2)
    {
      this.Up      = up;
      this.Right   = right;
      this.Down    = down;
      this.Left    = left;
      this.Button1 = button1;
      this.Button2 = button2;
    }
  }
}