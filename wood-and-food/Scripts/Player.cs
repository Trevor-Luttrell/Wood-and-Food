using Godot;
using System;

public partial class Player : Node2D
{
	[Signal]
	public delegate void OnMoveFinishedEventHandler();
	
	[Export]
	float MoveTimeHorz = 1.0F / 3.0F;
	[Export]
	float MoveTimeVert = 1.0F / 3.0F;
	
	private AnimatedSprite2D sprite;
	
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void Move(Direction dir)
	{
		Vector2 offset = Vector2.Zero;
		string animation = "";
		float time = 0.0F;
		
		switch(dir)
		{
			case Direction.Left:
				offset = Vector2.Left * Constants.TILE_WIDTH;
				animation = "move";
				time = MoveTimeHorz;
				break;
			case Direction.Right:
				offset = Vector2.Right * Constants.TILE_WIDTH;
				animation = "move";
				time = MoveTimeHorz;
				break;
			case Direction.Up:
				offset = Vector2.Up * Constants.TILE_HEIGHT;
				animation = "move";
				time = MoveTimeVert;
				break;
			case Direction.Down:
				offset = Vector2.Down * Constants.TILE_HEIGHT;
				animation = "move";
				time = MoveTimeVert;
				break;
			
		}
		
		if(offset != Vector2.Zero)
		{
	 		sprite.Play(animation);
			var tween = CreateTween();
			tween.TweenProperty(this, "position", Position + offset, time);
			tween.Finished += OnTweenFinish;
		}
	}
	
	private void OnTweenFinish()
	{
		sprite.Stop();
		EmitSignal(SignalName.OnMoveFinished);
	}
}
