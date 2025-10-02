using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node2D
{
	[Signal]
	public delegate void OnMoveFinishedEventHandler();

	[Export]
	float MoveTimeHorz = 1.0F / 3.0F;
	[Export]
	float MoveTimeVert = 1.0F / 3.0F;

	[Export]
	int Stamina = 20;
	[Export]
	int MaxStamina = 20;

	public int MoveCounter = 0;

	private Dictionary<string, int> inventory = new Dictionary<string, int>()
	{
		{"Rock", 0},
		{"Wood", 0},
		{"Berry", 0},
		{"Crystal", 0}
	};

	public Vector2I Coords
	{
		get
		{
			return new Vector2I(
				(int)Math.Round(Position.X) / (int)Constants.TILE_WIDTH,
				(int)Math.Round(Position.Y) / (int)Constants.TILE_HEIGHT
			);
		}
	}

	private AnimatedSprite2D sprite;
	private ProgressBar HealthBar;
	private Label StaminaLabel;
	private Label BerryLabel;
	private Label WoodLabel;
	private Label RockLabel;
	private Label CrystalLabel;
	private Panel WinScreen;
	private Panel DeathScreen;

	public override void _Ready()
	{
		Stamina = MaxStamina;
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.MaxValue = MaxStamina;
		HealthBar.Value = Stamina;

		StaminaLabel = GetNode<Label>("../CanvasLayer/Panel/StaminaLabel");

		BerryLabel = GetNode<Label>("../CanvasLayer/Panel/GridContainer/BerryLabel");
		WoodLabel = GetNode<Label>("../CanvasLayer/Panel/GridContainer/WoodLabel");
		RockLabel = GetNode<Label>("../CanvasLayer/Panel/GridContainer/RockLabel");
		CrystalLabel = GetNode<Label>("../CanvasLayer/Panel/GridContainer/CrystalLabel");

		WinScreen = GetNode<Panel>("../CanvasWinScreen/WinScreen");
		DeathScreen = GetNode<Panel>("../CanvasDeathScreen/DeathScreen");

		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("default");
	}

	public void Move(Direction dir)
	{
		Vector2 offset = Vector2.Zero;
		string animation = "";
		float time = 0.0F;
		MoveCounter += 1;
		Stamina -= 2;

		UpdateStaminaLabel();
		HealthBar.Value = Stamina;

		switch (dir)
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

		if (offset != Vector2.Zero)
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

		MoveCounter = 0;
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
		CheckForDeath();
	}

	private void CheckForDeath()
	{
		if (Stamina <= 0)
		{
			DeathScreen.Visible = true;
		}
	}

	public void DamagePlayer()
	{
		Stamina -= 1;
		HealthBar.Value = Stamina;
		UpdateStaminaLabel();
	}

	private void UpdateInventoryLabels()
	{
		BerryLabel.Text = $"Berry: {inventory["Berry"]}";
		WoodLabel.Text = $"Wood: {inventory["Wood"]}";
		RockLabel.Text = $"Rock: {inventory["Rock"]}";
		CrystalLabel.Text = $"Crystal: {inventory["Crystal"]}";
	}

	public void GivePlayerItem(string ResourceType)
	{
		inventory[ResourceType]++;
		UpdateInventoryLabels();
	}

	public void CheckForWin()
	{
		if (inventory["Crystal"] == 4)
		{
			WinScreen.Visible = true;
		}
	}

	public void EatBerry()
	{
		if (inventory["Berry"] > 0)
		{
			inventory["Berry"] -= 1;

			// Restore stamina, but not above max
			int restoreAmount = 5; // tweak this number for balance
			Stamina = Math.Min(Stamina + restoreAmount, MaxStamina);

			HealthBar.Value = Stamina;
			UpdateStaminaLabel();
			UpdateInventoryLabels();

			GD.Print($"Ate a berry! Restored {restoreAmount} stamina.");
		}
		else
		{
			GD.Print("No berries left!");
		}
	}

}
