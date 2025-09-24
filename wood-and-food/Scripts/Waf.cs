using Godot;
using System;

public partial class Waf : Node2D
{
	private enum State { Ready, Moving };
	
	private State state = State.Ready;
	private Player player = null;
	private TileMapLayer resources = null;
	private TileMapLayer floor = null;
	
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		resources = GetNode<TileMapLayer>("Resources");
		floor = GetNode<TileMapLayer>("Board");
	}
	
	public override void _Input(InputEvent @event)
	{
		if(state != State.Ready) return;
		
		Direction? movedir = null;
		
		if(@event.IsActionPressed("move_left"))
		{
			movedir = Direction.Left;
		}
		else if(@event.IsActionPressed("move_right"))
		{
			movedir = Direction.Right;
		}
		else if(@event.IsActionPressed("move_up"))
		{
			movedir = Direction.Up;
		}
		else if(@event.IsActionPressed("move_down"))
		{
			movedir = Direction.Down;
		}
		
		if (movedir != null)
		{
			if(PlayerCanMove((Direction)movedir)){
				player.Move((Direction)movedir);
				state = State.Moving;
			}
		}
	}
	
	public void OnPlayerMoveFinished()
	{
		state = State.Ready;
	}
	
	private bool PlayerCanMove(Direction dir){
		var targetCell = player.Coords.Offset(dir);
		GD.Print(targetCell);
		return resources.GetCellTileData(targetCell) == null && floor.GetCellTileData(targetCell) != null;
	}
}
