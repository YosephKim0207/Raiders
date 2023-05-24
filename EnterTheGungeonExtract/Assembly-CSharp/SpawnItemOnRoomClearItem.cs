using System;
using UnityEngine;

// Token: 0x020014AF RID: 5295
public class SpawnItemOnRoomClearItem : PassiveItem
{
	// Token: 0x06007866 RID: 30822 RVA: 0x00302348 File Offset: 0x00300548
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.OnRoomClearEvent += this.RoomCleared;
		base.Pickup(player);
	}

	// Token: 0x06007867 RID: 30823 RVA: 0x00302370 File Offset: 0x00300570
	private void RoomCleared(PlayerController obj)
	{
		float value = UnityEngine.Random.value;
		if (this.requirePlayerDamaged && obj.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			return;
		}
		if (obj.CurrentRoom.PlayerHasTakenDamageInThisRoom)
		{
			return;
		}
		if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.THE_COIN_KING, false) && this.itemName == "Crown of the Coin King")
		{
			this.chanceToSpawn *= 2f;
		}
		if (value < this.chanceToSpawn)
		{
			PickupObject byId = PickupObjectDatabase.GetById(this.spawnItemId);
			LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
		}
	}

	// Token: 0x06007868 RID: 30824 RVA: 0x00302440 File Offset: 0x00300640
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnRoomClearEvent -= this.RoomCleared;
		debrisObject.GetComponent<SpawnItemOnRoomClearItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007869 RID: 30825 RVA: 0x00302474 File Offset: 0x00300674
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007A90 RID: 31376
	public float chanceToSpawn = 0.05f;

	// Token: 0x04007A91 RID: 31377
	[PickupIdentifier]
	public int spawnItemId = -1;

	// Token: 0x04007A92 RID: 31378
	public bool requirePlayerDamaged;
}
