using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012DE RID: 4830
public class FloorChestPlacer : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06006C3F RID: 27711 RVA: 0x002A9C24 File Offset: 0x002A7E24
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round) - room.area.basePosition;
		Chest chest;
		if (this.UseOverrideChest && this.OverrideChestPrereq.CheckConditionsFulfilled())
		{
			chest = Chest.Spawn(this.OverrideChestPrefab, base.transform.position.IntXY(VectorConversions.Round));
		}
		else
		{
			chest = GameManager.Instance.RewardManager.GenerationSpawnRewardChestAt(intVector, room, (!this.OverrideItemQuality) ? null : new PickupObject.ItemQuality?(this.ItemQuality), this.OverrideMimicChance);
		}
		if (this.CenterChestInRegion && chest)
		{
			SpeculativeRigidbody component = chest.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				Vector2 vector = component.UnitCenter - chest.transform.position.XY();
				Vector2 vector2 = base.transform.position.XY() + new Vector2((float)this.xPixelOffset / 16f, (float)this.yPixelOffset / 16f) + new Vector2((float)this.placeableWidth / 2f, (float)this.placeableHeight / 2f);
				Vector2 vector3 = vector2 - vector;
				chest.transform.position = vector3.ToVector3ZisY(0f).Quantize(0.0625f);
				component.Reinitialize();
			}
		}
		if (this.OverrideLockChance && chest)
		{
			if (UnityEngine.Random.value < this.LockChance || (this.ForceUnlockedIfWooden && chest.lootTable.D_Chance == 1f))
			{
				chest.ForceUnlock();
			}
			else
			{
				chest.IsLocked = true;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006C40 RID: 27712 RVA: 0x002A9E04 File Offset: 0x002A8004
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400693D RID: 26941
	public bool OverrideItemQuality;

	// Token: 0x0400693E RID: 26942
	[ShowInInspectorIf("OverrideItemQuality", false)]
	public PickupObject.ItemQuality ItemQuality;

	// Token: 0x0400693F RID: 26943
	public float OverrideMimicChance = -1f;

	// Token: 0x04006940 RID: 26944
	[DwarfConfigurable]
	public int xPixelOffset;

	// Token: 0x04006941 RID: 26945
	[DwarfConfigurable]
	public int yPixelOffset;

	// Token: 0x04006942 RID: 26946
	public bool CenterChestInRegion;

	// Token: 0x04006943 RID: 26947
	public bool OverrideLockChance;

	// Token: 0x04006944 RID: 26948
	public bool ForceUnlockedIfWooden;

	// Token: 0x04006945 RID: 26949
	public float LockChance = 0.5f;

	// Token: 0x04006946 RID: 26950
	public bool UseOverrideChest;

	// Token: 0x04006947 RID: 26951
	public DungeonPrerequisite OverrideChestPrereq;

	// Token: 0x04006948 RID: 26952
	public Chest OverrideChestPrefab;
}
