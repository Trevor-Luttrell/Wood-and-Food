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
	
	[Export]
	int Stamina = 10;
	[Export]
	int MaxStamina = 10;
	
	public int MoveCounter = 0;
	
	public Vector2I Coords
	{
		get {
			return new Vector2I(
				(int)Math.Round(Position.X) / (int)Constants.TILE_WIDTH,
				(int)Math.Round(Position.Y) / (int)Constants.TILE_HEIGHT
			);
		}
	}
	
	private AnimatedSprite2D sprite;
	private ProgressBar HealthBar;
	private Label StaminaLabel;
	
	public override void _Ready()
	{
		Stamina = MaxStamina;
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.MaxValue = MaxStamina;
		HealthBar.Value = Stamina;
		
		StaminaLabel = GetNode<Label>("../CanvasLayer/Panel/StaminaLabel");
		
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("default"); 
	}
	
	public void Move(Direction dir)
	{
		Vector2 offset = Vector2.Zero;
		string animation = "";
		float time = 0.0F;
		MoveCounter += 1;
		Stamina -= 1;
		
		UpdateStaminaLabel();
		HealthBar.Value = Stamina;
		
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
	
	public void Heal()
	{
		Stamina = MaxStamina;
		HealthBar.Value = Stamina;
		
		UpdateStaminaLabel();
	}
	
	private void OnTweenFinish()
	{
		sprite.Stop();
		sprite.Play("default");
		EmitSignal(SignalName.OnMoveFinished);
	}
	
	private void UpdateStaminaLabel()
	{
		StaminaLabel.Text = $"Stamina: {Stamina}/{MaxStamina}";
	}
}
