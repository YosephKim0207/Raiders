using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EB7 RID: 3767
	[Serializable]
	public class TileIndices
	{
		// Token: 0x06004FA7 RID: 20391 RVA: 0x001BAA08 File Offset: 0x001B8C08
		public bool PitAtPositionIsWater(Vector2 point)
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				return false;
			}
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(point.ToIntVector2(VectorConversions.Round));
			if (absoluteRoomFromPosition.RoomFallValidForMaintenance())
			{
				return false;
			}
			DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[absoluteRoomFromPosition.RoomVisualSubtype];
			return !(dungeonMaterial == null) && !(dungeonMaterial.pitfallVFXPrefab == null) && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && !GameManager.PVP_ENABLED && dungeonMaterial.pitfallVFXPrefab.name.Contains("Splash");
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x001BAAC0 File Offset: 0x001B8CC0
		public GameObject DoSplashAtPosition(Vector2 point)
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				return null;
			}
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(point.ToIntVector2(VectorConversions.Round));
			if (absoluteRoomFromPosition.RoomFallValidForMaintenance())
			{
				return null;
			}
			DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[absoluteRoomFromPosition.RoomVisualSubtype];
			if (dungeonMaterial == null || dungeonMaterial.pitfallVFXPrefab == null)
			{
				return null;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.PVP_ENABLED)
			{
				return null;
			}
			IntVector2 intVector = point.ToIntVector2(VectorConversions.Floor);
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			if (cellData == null)
			{
				return null;
			}
			if (Time.realtimeSinceStartup - cellData.lastSplashTime < 0.25f)
			{
				return null;
			}
			cellData.lastSplashTime = Time.realtimeSinceStartup;
			GameObject pitfallVFXPrefab = dungeonMaterial.pitfallVFXPrefab;
			GameObject gameObject = SpawnManager.SpawnVFX(pitfallVFXPrefab, point.ToVector3ZUp(0f), Quaternion.identity);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			component.HeightOffGround = -4.0625f;
			component.PlaceAtPositionByAnchor(point, tk2dBaseSprite.Anchor.MiddleCenter);
			component.transform.position = component.transform.position.Quantize(1f / (float)PhysicsEngine.Instance.PixelsPerUnit);
			component.UpdateZDepth();
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && dungeonMaterial.usesFacewallGrids)
			{
				if (cellData.type != CellType.FLOOR)
				{
					GlobalSparksDoer.DoRandomParticleBurst(30, component.transform.position + new Vector3(-0.75f, -0.75f, 0f), component.transform.position + new Vector3(0.75f, 0.75f, 0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
				}
			}
			return gameObject;
		}

		// Token: 0x04004797 RID: 18327
		public GlobalDungeonData.ValidTilesets tilesetId;

		// Token: 0x04004798 RID: 18328
		public tk2dSpriteCollectionData dungeonCollection;

		// Token: 0x04004799 RID: 18329
		public bool dungeonCollectionSupportsDiagonalWalls;

		// Token: 0x0400479A RID: 18330
		public AOTileIndices aoTileIndices;

		// Token: 0x0400479B RID: 18331
		public bool placeBorders = true;

		// Token: 0x0400479C RID: 18332
		public bool placePits = true;

		// Token: 0x0400479D RID: 18333
		public List<TileIndexVariant> chestHighWallIndices;

		// Token: 0x0400479E RID: 18334
		public TileIndexGrid decalIndexGrid;

		// Token: 0x0400479F RID: 18335
		public TileIndexGrid patternIndexGrid;

		// Token: 0x040047A0 RID: 18336
		public List<int> globalSecondBorderTiles;

		// Token: 0x040047A1 RID: 18337
		public TileIndexGrid edgeDecorationTiles;
	}
}
