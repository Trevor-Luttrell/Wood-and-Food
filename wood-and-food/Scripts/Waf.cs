using Godot;
using System;

public partial class Waf : Node2D
{
	private enum State { Ready, Moving };
	
	private State state = State.Ready;
	private Player player = null;
	
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
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
			player.Move((Direction)movedir);
			state = State.Moving;
		}
	}
	
	public void OnPlayerMoveFinished()
	{
		state = State.Ready;
	}
}
