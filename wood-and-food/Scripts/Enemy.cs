using Godot;
using System;

public partial class Enemy : Node2D
{
	public int EnemyHealth = 10;
	
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("default");
		Visible = false;
	}

	public void SpawnAt(Vector2I coords)
	{
		Position = new Vector2(
			coords.X * Constants.TILE_WIDTH,
			coords.Y * Constants.TILE_HEIGHT
		);
		Visible = true;
	}
	
	public void DamageEnemy()
	{
		if(EnemyHealth > 0)
		{	
			EnemyHealth -= 2;
		}
		else
		{
			this.Visible = false;
		}
	}
}
