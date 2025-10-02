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
			if(GetResourceType(coords) != "Crystal")
				ResourceHealth[coords] = 10;
			else
				ResourceHealth[coords] = 1;
		}
		
		return ResourceHealth[coords];
	}
	
	public string GetResourceType(Vector2I ResourceCoords)
	{
		var tileData = GetCellTileData(ResourceCoords);
		return tileData.GetCustomData("ResourceType").AsString();
	}
	
	public void DamageResource(Vector2I coords, int dmg = 2)
	{
		int currentHealth = GetHealth(coords);

		currentHealth -= dmg;
		ResourceHealth[coords] = currentHealth;
		
		if (currentHealth <= 0)
		{
			SetCell(coords, -1);
			ResourceHealth.Remove(coords);
		}
	}
}
