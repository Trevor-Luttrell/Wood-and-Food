using Godot;
using System;

public partial class Campfire : Node2D
{
	public Vector2I Coords
	{
		get {
			return new Vector2I(
				(int)Math.Round(Position.X) / (int)Constants.TILE_WIDTH,
				(int)Math.Round(Position.Y) / (int)Constants.TILE_HEIGHT
			);
		}
	}
}
