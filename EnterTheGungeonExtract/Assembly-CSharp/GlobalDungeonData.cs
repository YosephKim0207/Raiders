using System;
using UnityEngine;

// Token: 0x02000E9C RID: 3740
public static class GlobalDungeonData
{
	// Token: 0x06004F4B RID: 20299 RVA: 0x001B7E24 File Offset: 0x001B6024
	public static int GetBasePrice(PickupObject.ItemQuality quality)
	{
		switch (quality)
		{
		case PickupObject.ItemQuality.COMMON:
			return GlobalDungeonData.COMMON_BASE_PRICE;
		case PickupObject.ItemQuality.D:
			return GlobalDungeonData.D_BASE_PRICE;
		case PickupObject.ItemQuality.C:
			return GlobalDungeonData.C_BASE_PRICE;
		case PickupObject.ItemQuality.B:
			return GlobalDungeonData.B_BASE_PRICE;
		case PickupObject.ItemQuality.A:
			return GlobalDungeonData.A_BASE_PRICE;
		case PickupObject.ItemQuality.S:
			return GlobalDungeonData.S_BASE_PRICE;
		default:
			if (Application.isPlaying)
			{
				Debug.LogError("Invalid quality : " + quality + " in GetBasePrice");
			}
			return GlobalDungeonData.S_BASE_PRICE;
		}
	}

	// Token: 0x0400468B RID: 18059
	public static int COMMON_BASE_PRICE = 20;

	// Token: 0x0400468C RID: 18060
	public static int D_BASE_PRICE = 35;

	// Token: 0x0400468D RID: 18061
	public static int C_BASE_PRICE = 45;

	// Token: 0x0400468E RID: 18062
	public static int B_BASE_PRICE = 65;

	// Token: 0x0400468F RID: 18063
	public static int A_BASE_PRICE = 90;

	// Token: 0x04004690 RID: 18064
	public static int S_BASE_PRICE = 120;

	// Token: 0x04004691 RID: 18065
	public static int occlusionPartitionIndex = 0;

	// Token: 0x04004692 RID: 18066
	public static int pitLayerIndex = 1;

	// Token: 0x04004693 RID: 18067
	public static int floorLayerIndex = 2;

	// Token: 0x04004694 RID: 18068
	public static int patternLayerIndex = 3;

	// Token: 0x04004695 RID: 18069
	public static int decalLayerIndex = 4;

	// Token: 0x04004696 RID: 18070
	public static int actorCollisionLayerIndex = 5;

	// Token: 0x04004697 RID: 18071
	public static int collisionLayerIndex = 6;

	// Token: 0x04004698 RID: 18072
	public static int wallStampLayerIndex = 7;

	// Token: 0x04004699 RID: 18073
	public static int objectStampLayerIndex = 8;

	// Token: 0x0400469A RID: 18074
	public static int shadowLayerIndex = 9;

	// Token: 0x0400469B RID: 18075
	public static int killLayerIndex = 10;

	// Token: 0x0400469C RID: 18076
	public static int ceilingLayerIndex = 11;

	// Token: 0x0400469D RID: 18077
	public static int borderLayerIndex = 12;

	// Token: 0x0400469E RID: 18078
	public static int aboveBorderLayerIndex = 13;

	// Token: 0x0400469F RID: 18079
	public static bool GUNGEON_EXPERIMENTAL = false;

	// Token: 0x040046A0 RID: 18080
	public static readonly string[] TilesetPaths = new string[]
	{
		"Assets\\Sprites\\Collections\\ENV_Tileset_Gungeon.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Castle.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Sewer.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Cathedral.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Mines.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Catacombs.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_Forge.prefab",
		"Assets\\Sprites\\Collections\\ENV_Tileset_BulletHell.prefab",
		string.Empty,
		string.Empty,
		string.Empty,
		"Assets\\Sprites\\Collections\\ENV_Tileset_Nakatomi.prefab",
		string.Empty,
		string.Empty,
		string.Empty,
		"Assets\\Sprites\\Collections\\Dolphin Tilesets\\ENV_Tileset_Rat.prefab"
	};

	// Token: 0x02000E9D RID: 3741
	[Flags]
	public enum ValidTilesets
	{
		// Token: 0x040046A2 RID: 18082
		GUNGEON = 1,
		// Token: 0x040046A3 RID: 18083
		CASTLEGEON = 2,
		// Token: 0x040046A4 RID: 18084
		SEWERGEON = 4,
		// Token: 0x040046A5 RID: 18085
		CATHEDRALGEON = 8,
		// Token: 0x040046A6 RID: 18086
		MINEGEON = 16,
		// Token: 0x040046A7 RID: 18087
		CATACOMBGEON = 32,
		// Token: 0x040046A8 RID: 18088
		FORGEGEON = 64,
		// Token: 0x040046A9 RID: 18089
		HELLGEON = 128,
		// Token: 0x040046AA RID: 18090
		SPACEGEON = 256,
		// Token: 0x040046AB RID: 18091
		PHOBOSGEON = 512,
		// Token: 0x040046AC RID: 18092
		WESTGEON = 1024,
		// Token: 0x040046AD RID: 18093
		OFFICEGEON = 2048,
		// Token: 0x040046AE RID: 18094
		BELLYGEON = 4096,
		// Token: 0x040046AF RID: 18095
		JUNGLEGEON = 8192,
		// Token: 0x040046B0 RID: 18096
		FINALGEON = 16384,
		// Token: 0x040046B1 RID: 18097
		RATGEON = 32768
	}
}
