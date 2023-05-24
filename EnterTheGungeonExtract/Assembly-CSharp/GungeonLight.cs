using System;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02001328 RID: 4904
public class GungeonLight : MonoBehaviour
{
	// Token: 0x06006F0A RID: 28426 RVA: 0x002C06C0 File Offset: 0x002BE8C0
	public static void UpdateTilemapLighting(tk2dTileMap map)
	{
		GungeonLight[] array = (GungeonLight[])UnityEngine.Object.FindObjectsOfType(typeof(GungeonLight));
		if (map.ColorChannel == null)
		{
			map.CreateColorChannel();
		}
		ColorChannel colorChannel = map.ColorChannel;
		for (int i = 0; i < map.width; i++)
		{
			for (int j = 0; j < map.height; j++)
			{
				colorChannel.SetColor(i, j, new Color(0.5f, 0.5f, 0.5f, 1f));
			}
		}
		foreach (GungeonLight gungeonLight in array)
		{
			IntVector2 intVector = new IntVector2(Mathf.FloorToInt(gungeonLight.transform.position.x), Mathf.FloorToInt(gungeonLight.transform.position.y));
			for (int l = intVector.x - gungeonLight.lightRadius; l < intVector.x + gungeonLight.lightRadius; l++)
			{
				for (int m = intVector.y - gungeonLight.lightRadius; m < intVector.y + gungeonLight.lightRadius; m++)
				{
					IntVector2 intVector2 = new IntVector2(l, m);
					float num = Vector2.Distance(intVector2.ToVector2(), new Vector2(gungeonLight.transform.position.x, gungeonLight.transform.position.y));
					float num2 = 1f - Mathf.Clamp01(num / (float)gungeonLight.lightRadius);
					colorChannel.SetColor(l, m, Color.Lerp(colorChannel.GetColor(l, m), gungeonLight.lightColor, num2));
				}
			}
		}
		map.ForceBuild();
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x002C0894 File Offset: 0x002BEA94
	private void Start()
	{
		this.position = base.transform.position;
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x002C08A8 File Offset: 0x002BEAA8
	private void Update()
	{
		if (this.thesisforsucks || base.transform.position != this.position)
		{
			GungeonLight.UpdateTilemapLighting((tk2dTileMap)UnityEngine.Object.FindObjectOfType(typeof(tk2dTileMap)));
			this.position = base.transform.position;
			this.thesisforsucks = false;
		}
	}

	// Token: 0x04006E84 RID: 28292
	public int lightRadius = 10;

	// Token: 0x04006E85 RID: 28293
	public Color lightColor = Color.white;

	// Token: 0x04006E86 RID: 28294
	private bool thesisforsucks = true;

	// Token: 0x04006E87 RID: 28295
	private Vector3 position;
}
