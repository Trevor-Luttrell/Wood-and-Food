public partial class Constants
{
	public const float TILE_WIDTH = 32;
	public const float TILE_HEIGHT = 32;
}

public enum Direction
{
	Up, Down, Left, Right
}

namespace Godot
{
	public static class Vector2IExtensions
	{
		public static Vector2I Offset(this Vector2I coords, Direction dir)
		{
			switch(dir)
			{
				case Direction.Left:
					return coords + Vector2I.Left;
				case Direction.Right:
					return coords + Vector2I.Right;
				case Direction.Up:
					return coords + Vector2I.Up;
				case Direction.Down:
					return coords + Vector2I.Down;
				default:
					return coords;
			}
		}
	}
}
