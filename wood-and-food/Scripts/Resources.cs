using Godot;
using System;

public partial class Resources : TileMapLayer
{
	//private int ResourceHealth = 10;
	
	public string GetResourceType(Vector2I ResourceCoords)
	{
		var tileData = GetCellTileData(ResourceCoords);
		
		return tileData.GetCustomData("ResourceType").AsString();
	}
	
	public void DamageResource()
	{
		//ResourceHealth -= 2;
	}
}
