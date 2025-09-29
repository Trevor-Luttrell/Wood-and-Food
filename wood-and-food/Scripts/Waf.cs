using Godot;
using System;

public partial class Waf : Node2D
{
	private enum State { Ready, Moving };
	
	private State state = State.Ready;
	private Player player = null;
	private TileMapLayer resources = null;
	private TileMapLayer floor = null;
	private Campfire campfire = null;
	
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		campfire = GetNode<Campfire>("Campfire");
		
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
			CheckForHeal((Direction)movedir);
		}
	}
	
	public void OnPlayerMoveFinished()
	{
		state = State.Ready;
	}
	
	private bool PlayerCanMove(Direction dir){
		var TargetCell = player.Coords.Offset(dir);
		var CampfireCell = campfire.Coords;
		
		return resources.GetCellTileData(TargetCell) == null && floor.GetCellTileData(TargetCell) != null && TargetCell != CampfireCell;
	}
	
	private void CheckForHeal(Direction dir){
		var TargetCell = player.Coords.Offset(dir);
		var CampfireCell = campfire.Coords;
		
		if(TargetCell == CampfireCell){
			player.Heal();
		}
	}
}
