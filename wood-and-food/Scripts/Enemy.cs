using Godot;
using System;

public partial class Enemy : Node2D
{
	public int EnemyHealth = 10;
	public int MaxEnemyHealth = 10;
	
	private AnimatedSprite2D sprite;
	private ProgressBar EnemyBar;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("default");
		Visible = false;
		
		EnemyBar = GetNode<ProgressBar>("EnemyBar");
		EnemyBar.MaxValue = MaxEnemyHealth;
		EnemyBar.Value = EnemyHealth;
	}

	public void SpawnAt(Vector2I coords)
	{
		Position = new Vector2(
			coords.X * Constants.TILE_WIDTH,
			coords.Y * Constants.TILE_HEIGHT
		);

		EnemyHealth = MaxEnemyHealth;
		EnemyBar.Value = EnemyHealth;

		Visible = true;
	}
	
	
	public void DamageEnemy(int dmg = 1)
	{
		EnemyHealth -= dmg;
		EnemyBar.Value = EnemyHealth;
		
		if (EnemyHealth <= 0)
		{
			Visible = false;
		}
	}
}
