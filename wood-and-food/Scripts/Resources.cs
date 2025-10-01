using Godot;
using System;
using System.Collections.Generic;

public partial class Resources : TileMapLayer
{
	private Dictionary<Vector2I, int> ResourceHealth = new();
	
	private int GetHealth(Vector2I coords)
	{
		if(!ResourceHealth.ContainsKey(coords))
		{
			ResourceHealth[coords] = 10;
		}
		
		return ResourceHealth[coords];
	}
	
	public string GetResourceType(Vector2I ResourceCoords)
	{
		var tileData = GetCellTileData(ResourceCoords);
		
		return tileData.GetCustomData("ResourceType").AsString();
	}
	
	public void DamageResource(Vector2I coords)
	{
		int currentHealth = GetHealth(coords);
		
		currentHealth -= 2;
		
		ResourceHealth[coords] = currentHealth;
		
		GD.Print($"Tile at {coords} now has {currentHealth} HP");
		
		if (currentHealth <= 0)
		{
			SetCell(coords, -1);
			ResourceHealth.Remove(coords);
			GD.Print($"Resource at {coords} destroyed!");
		}
	}
}
