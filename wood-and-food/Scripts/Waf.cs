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

	private Enemy enemy ;

	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		campfire = GetNode<Campfire>("Campfire");
		enemy = GetNode<Enemy>("Enemy");
		resources = GetNode<TileMapLayer>("Resources");
		floor = GetNode<TileMapLayer>("Board");
		 player.OnMoveFinished += OnPlayerMoveFinished;
	}

	public override void _Input(InputEvent @event)
	{
		if (state != State.Ready) return;

		Direction? movedir = null;

		if (@event.IsActionPressed("move_left"))
		{
			movedir = Direction.Left;
		}
		else if (@event.IsActionPressed("move_right"))
		{
			movedir = Direction.Right;
		}
		else if (@event.IsActionPressed("move_up"))
		{
			movedir = Direction.Up;
		}
		else if (@event.IsActionPressed("move_down"))
		{
			movedir = Direction.Down;
		}

		if (movedir != null)
		{
			RunPlayerMoveChecks((Direction)movedir);
		}
	}

	public void RunPlayerMoveChecks(Direction dir)
	{
		if (PlayerCanMove(dir))
		{
			player.Move(dir);
			state = State.Moving;
		}

		CheckForHeal(dir);
	}

	public void OnPlayerMoveFinished()
	{
		state = State.Ready;
	if (player.MoveCounter % 5 == 0)
      {
        SpawnEnemyNearPlayer();
      }
	}

	private bool PlayerCanMove(Direction dir)
	{
		var TargetCell = player.Coords.Offset(dir);
		var CampfireCell = campfire.Coords;

		return resources.GetCellTileData(TargetCell) == null && floor.GetCellTileData(TargetCell) != null && TargetCell != CampfireCell;
	}

	private void CheckForHeal(Direction dir)
	{
		var TargetCell = player.Coords.Offset(dir);
		var CampfireCell = campfire.Coords;

		if (TargetCell == CampfireCell)
		{
			player.Heal();
		}
	}



	private void SpawnEnemyNearPlayer()
	{
		var directions = new Direction[]
		{
		Direction.Up,
		Direction.Down,
		Direction.Left,
		Direction.Right
		};

		foreach (var dir in directions)
		{
			var targetCell = player.Coords.Offset(dir);

			if (resources.GetCellTileData(targetCell) == null &&
				floor.GetCellTileData(targetCell) != null &&
				targetCell != campfire.Coords)
			{
				enemy.SpawnAt(targetCell); // call Enemy.cs method
				GD.Print($"Enemy spawned at {targetCell}");
				return;
			}
		}

		GD.Print("No valid spot near player to spawn enemy.");



	}



	



}
