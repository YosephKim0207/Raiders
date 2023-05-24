using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E71 RID: 3697
[Serializable]
public class DungeonMaterial : ScriptableObject
{
	// Token: 0x06004EAE RID: 20142 RVA: 0x001B36BC File Offset: 0x001B18BC
	public GameObject GetSecretRoomWallShardCollection()
	{
		if (this.secretRoomWallShardCollections.Count > 0)
		{
			return this.secretRoomWallShardCollections[UnityEngine.Random.Range(0, this.secretRoomWallShardCollections.Count)];
		}
		return null;
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001B36F0 File Offset: 0x001B18F0
	public TileIndexGrid GetRandomGridFromArray(TileIndexGrid[] grids)
	{
		if (grids == null)
		{
			return null;
		}
		if (grids.Length == 0)
		{
			return null;
		}
		return grids[UnityEngine.Random.Range(0, grids.Length)];
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x001B3710 File Offset: 0x001B1910
	public void SpawnRandomVertical(Vector3 position, float rotation, Transform enemy, Vector2 sourceNormal, Vector2 sourceVelocity)
	{
		VFXComplex vfxcomplex = this.fallbackVerticalTileMapEffects[UnityEngine.Random.Range(0, this.fallbackVerticalTileMapEffects.Length)];
		float num = (float)Mathf.FloorToInt(position.y);
		if (sourceNormal.y > 0.1f)
		{
			num += 0.25f;
		}
		vfxcomplex.SpawnAtPosition(position.x, num, position.y - num, rotation, enemy, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), false, null, false);
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001B3784 File Offset: 0x001B1984
	public void SpawnRandomHorizontal(Vector3 position, float rotation, Transform enemy, Vector2 sourceNormal, Vector2 sourceVelocity)
	{
		VFXComplex vfxcomplex = this.fallbackHorizontalTileMapEffects[UnityEngine.Random.Range(0, this.fallbackHorizontalTileMapEffects.Length)];
		vfxcomplex.SpawnAtPosition(position, rotation, enemy, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), null, false, null, null, false);
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001B37CC File Offset: 0x001B19CC
	public void SpawnRandomShard(Vector3 position, Vector2 collisionNormal)
	{
		GameObject gameObject = this.wallShards.SelectByWeight();
		this.InternalSpawnShard(gameObject, position, collisionNormal);
	}

	// Token: 0x06004EB3 RID: 20147 RVA: 0x001B37F0 File Offset: 0x001B19F0
	public void SpawnRandomShard(Vector3 position, Vector2 collisionNormal, float damage)
	{
		GameObject gameObject;
		if (damage > this.bigWallShardDamageThreshold && this.bigWallShards.elements.Count > 0)
		{
			gameObject = this.bigWallShards.SelectByWeight();
		}
		else
		{
			gameObject = this.wallShards.SelectByWeight();
		}
		this.InternalSpawnShard(gameObject, position, collisionNormal);
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001B3848 File Offset: 0x001B1A48
	private void InternalSpawnShard(GameObject shardToSpawn, Vector3 position, Vector2 collisionNormal)
	{
		if (shardToSpawn != null)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(shardToSpawn, position, Quaternion.identity);
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			component.angularVelocity = UnityEngine.Random.Range(0.5f, 1.5f) * component.angularVelocity;
			float num = ((Mathf.Abs(collisionNormal.y) <= 0.1f) ? 0f : 0.25f);
			component.Trigger(Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-30, 30)) * collisionNormal.ToVector3ZUp(0f) * UnityEngine.Random.Range(0f, 4f), UnityEngine.Random.Range(0.1f, 0.5f) + num, 1f);
		}
	}

	// Token: 0x04004527 RID: 17703
	public WeightedGameObjectCollection wallShards;

	// Token: 0x04004528 RID: 17704
	public WeightedGameObjectCollection bigWallShards;

	// Token: 0x04004529 RID: 17705
	public float bigWallShardDamageThreshold = 10f;

	// Token: 0x0400452A RID: 17706
	public VFXComplex[] fallbackVerticalTileMapEffects;

	// Token: 0x0400452B RID: 17707
	public VFXComplex[] fallbackHorizontalTileMapEffects;

	// Token: 0x0400452C RID: 17708
	public GameObject pitfallVFXPrefab;

	// Token: 0x0400452D RID: 17709
	public bool UsePitAmbientVFX;

	// Token: 0x0400452E RID: 17710
	public List<GameObject> AmbientPitVFX;

	// Token: 0x0400452F RID: 17711
	public float PitVFXMinCooldown = 5f;

	// Token: 0x04004530 RID: 17712
	public float PitVFXMaxCooldown = 30f;

	// Token: 0x04004531 RID: 17713
	public float ChanceToSpawnPitVFXOnCooldown = 1f;

	// Token: 0x04004532 RID: 17714
	public bool UseChannelAmbientVFX;

	// Token: 0x04004533 RID: 17715
	public float ChannelVFXMinCooldown = 1f;

	// Token: 0x04004534 RID: 17716
	public float ChannelVFXMaxCooldown = 15f;

	// Token: 0x04004535 RID: 17717
	public List<GameObject> AmbientChannelVFX;

	// Token: 0x04004536 RID: 17718
	[Header("Stamp Overrides")]
	public float stampFailChance = 0.2f;

	// Token: 0x04004537 RID: 17719
	public GenericLootTable overrideTableTable;

	// Token: 0x04004538 RID: 17720
	[Header("Weirdo Tilemap Stuff")]
	public bool supportsPits = true;

	// Token: 0x04004539 RID: 17721
	public bool doPitAO = true;

	// Token: 0x0400453A RID: 17722
	[ShowInInspectorIf("doPitAO", false)]
	public bool pitsAreOneDeep;

	// Token: 0x0400453B RID: 17723
	public bool supportsDiagonalWalls = true;

	// Token: 0x0400453C RID: 17724
	public bool supportsUpholstery;

	// Token: 0x0400453D RID: 17725
	public bool carpetIsMainFloor;

	// Token: 0x0400453E RID: 17726
	public TileIndexGrid[] carpetGrids;

	// Token: 0x0400453F RID: 17727
	public bool supportsChannels;

	// Token: 0x04004540 RID: 17728
	public int minChannelPools;

	// Token: 0x04004541 RID: 17729
	public int maxChannelPools = 3;

	// Token: 0x04004542 RID: 17730
	public float channelTenacity = 0.75f;

	// Token: 0x04004543 RID: 17731
	public TileIndexGrid[] channelGrids;

	// Token: 0x04004544 RID: 17732
	public bool supportsLavaOrLavalikeSquares;

	// Token: 0x04004545 RID: 17733
	public TileIndexGrid[] lavaGrids;

	// Token: 0x04004546 RID: 17734
	public bool supportsIceSquares;

	// Token: 0x04004547 RID: 17735
	public TileIndexGrid[] iceGrids;

	// Token: 0x04004548 RID: 17736
	public TileIndexGrid roomFloorBorderGrid;

	// Token: 0x04004549 RID: 17737
	public TileIndexGrid roomCeilingBorderGrid;

	// Token: 0x0400454A RID: 17738
	public TileIndexGrid pitLayoutGrid;

	// Token: 0x0400454B RID: 17739
	public TileIndexGrid pitBorderFlatGrid;

	// Token: 0x0400454C RID: 17740
	public TileIndexGrid pitBorderRaisedGrid;

	// Token: 0x0400454D RID: 17741
	public TileIndexGrid additionalPitBorderFlatGrid;

	// Token: 0x0400454E RID: 17742
	public TileIndexGrid outerCeilingBorderGrid;

	// Token: 0x0400454F RID: 17743
	public float floorSquareDensity = 0.05f;

	// Token: 0x04004550 RID: 17744
	public TileIndexGrid[] floorSquares;

	// Token: 0x04004551 RID: 17745
	public bool usesFacewallGrids;

	// Token: 0x04004552 RID: 17746
	public FacewallIndexGridDefinition[] facewallGrids;

	// Token: 0x04004553 RID: 17747
	public bool usesInternalMaterialTransitions;

	// Token: 0x04004554 RID: 17748
	public bool usesProceduralMaterialTransitions;

	// Token: 0x04004555 RID: 17749
	public RoomInternalMaterialTransition[] internalMaterialTransitions;

	// Token: 0x04004556 RID: 17750
	public List<GameObject> secretRoomWallShardCollections;

	// Token: 0x04004557 RID: 17751
	public bool overrideStoneFloorType;

	// Token: 0x04004558 RID: 17752
	[ShowInInspectorIf("overrideStoneFloorType", true)]
	public CellVisualData.CellFloorType overrideFloorType;

	// Token: 0x04004559 RID: 17753
	[Header("Lighting Data")]
	public bool useLighting = true;

	// Token: 0x0400455A RID: 17754
	public WeightedGameObjectCollection lightPrefabs;

	// Token: 0x0400455B RID: 17755
	public List<LightStampData> facewallLightStamps;

	// Token: 0x0400455C RID: 17756
	public List<LightStampData> sidewallLightStamps;

	// Token: 0x0400455D RID: 17757
	[Header("Deco Overrides")]
	public bool usesDecalLayer;

	// Token: 0x0400455E RID: 17758
	public TileIndexGrid decalIndexGrid;

	// Token: 0x0400455F RID: 17759
	public TilemapDecoSettings.DecoStyle decalLayerStyle;

	// Token: 0x04004560 RID: 17760
	public int decalSize = 1;

	// Token: 0x04004561 RID: 17761
	public int decalSpacing = 1;

	// Token: 0x04004562 RID: 17762
	public bool usesPatternLayer;

	// Token: 0x04004563 RID: 17763
	public TileIndexGrid patternIndexGrid;

	// Token: 0x04004564 RID: 17764
	public TilemapDecoSettings.DecoStyle patternLayerStyle;

	// Token: 0x04004565 RID: 17765
	public int patternSize = 1;

	// Token: 0x04004566 RID: 17766
	public int patternSpacing = 1;

	// Token: 0x04004567 RID: 17767
	[Header("The Wild West")]
	public bool forceEdgesDiagonal;

	// Token: 0x04004568 RID: 17768
	public TileIndexGrid exteriorFacadeBorderGrid;

	// Token: 0x04004569 RID: 17769
	public TileIndexGrid facadeTopGrid;

	// Token: 0x0400456A RID: 17770
	[Header("The Sewers")]
	public TileIndexGrid bridgeGrid;
}
