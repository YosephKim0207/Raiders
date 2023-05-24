using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013B0 RID: 5040
public class FragileGunItem : PassiveItem
{
	// Token: 0x06007232 RID: 29234 RVA: 0x002D6228 File Offset: 0x002D4428
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.OnReceivedDamage += this.HandleTookDamage;
	}

	// Token: 0x06007233 RID: 29235 RVA: 0x002D6258 File Offset: 0x002D4458
	private void HandleTookDamage(PlayerController obj)
	{
		if (obj && obj.CurrentGun && !obj.CurrentGun.InfiniteAmmo)
		{
			this.BreakGun(obj, obj.CurrentGun);
		}
	}

	// Token: 0x06007234 RID: 29236 RVA: 0x002D6294 File Offset: 0x002D4494
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<FragileGunItem>().m_pickedUpThisRun = true;
		player.OnReceivedDamage -= this.HandleTookDamage;
		return debrisObject;
	}

	// Token: 0x06007235 RID: 29237 RVA: 0x002D62D0 File Offset: 0x002D44D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.OnReceivedDamage -= this.HandleTookDamage;
		}
	}

	// Token: 0x06007236 RID: 29238 RVA: 0x002D6300 File Offset: 0x002D4500
	private void BreakGun(PlayerController sourcePlayer, Gun sourceGun)
	{
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			DebrisObject debrisObject = LootEngine.SpawnItem(this.GunPiecePrefab, sourcePlayer.CenterPosition, UnityEngine.Random.insideUnitCircle.normalized, 10f, true, false, false);
			FragileGunItemPiece componentInChildren = debrisObject.GetComponentInChildren<FragileGunItemPiece>();
			componentInChildren.AssignGun(sourceGun);
		}
		this.m_workingDictionary.Add(sourceGun.PickupObjectId, num);
		this.m_gunToAmmoDictionary.Add(sourceGun.PickupObjectId, sourceGun.ammo);
		sourcePlayer.inventory.RemoveGunFromInventory(sourceGun);
	}

	// Token: 0x06007237 RID: 29239 RVA: 0x002D6394 File Offset: 0x002D4594
	public void AcquirePiece(FragileGunItemPiece piece)
	{
		if (piece.AssignedGunId != -1 && this.m_workingDictionary.ContainsKey(piece.AssignedGunId))
		{
			this.m_workingDictionary[piece.AssignedGunId] = this.m_workingDictionary[piece.AssignedGunId] - 1;
			if (this.m_workingDictionary[piece.AssignedGunId] <= 0)
			{
				this.m_workingDictionary.Remove(piece.AssignedGunId);
				PickupObject byId = PickupObjectDatabase.GetById(piece.AssignedGunId);
				if (byId)
				{
					Gun gun = LootEngine.TryGiveGunToPlayer(byId.gameObject, this.m_owner, false);
					if (this.m_gunToAmmoDictionary.ContainsKey(piece.AssignedGunId) && gun)
					{
						gun.ammo = this.m_gunToAmmoDictionary[piece.AssignedGunId];
						this.m_gunToAmmoDictionary.Remove(piece.AssignedGunId);
					}
				}
			}
		}
	}

	// Token: 0x04007397 RID: 29591
	public GameObject GunPiecePrefab;

	// Token: 0x04007398 RID: 29592
	private PlayerController m_player;

	// Token: 0x04007399 RID: 29593
	private Dictionary<int, int> m_workingDictionary = new Dictionary<int, int>();

	// Token: 0x0400739A RID: 29594
	private Dictionary<int, int> m_gunToAmmoDictionary = new Dictionary<int, int>();
}
