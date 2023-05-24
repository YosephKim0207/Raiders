using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014BB RID: 5307
public class SpikedArmorItem : BasicStatPickup
{
	// Token: 0x060078A2 RID: 30882 RVA: 0x00303998 File Offset: 0x00301B98
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		if (this.HasIgniteSynergy)
		{
			SpeculativeRigidbody specRigidbody = player.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		base.Pickup(player);
	}

	// Token: 0x060078A3 RID: 30883 RVA: 0x00303A70 File Offset: 0x00301C70
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.HasIgniteSynergy && this.m_owner && this.m_owner.HasActiveBonusSynergy(this.RequiredSynergy, false) && rigidbodyCollision.OtherRigidbody && rigidbodyCollision.OtherRigidbody.aiActor)
		{
			AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;
			if (aiActor.IsNormalEnemy && !aiActor.IsHarmlessEnemy)
			{
				aiActor.ApplyEffect(this.IgniteEffect, 1f, null);
			}
		}
	}

	// Token: 0x060078A4 RID: 30884 RVA: 0x00303B08 File Offset: 0x00301D08
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		if (player && player.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = player.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		debrisObject.GetComponent<SpikedArmorItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060078A5 RID: 30885 RVA: 0x00303BF0 File Offset: 0x00301DF0
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		if (this.m_owner && this.m_owner.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = this.m_owner.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		base.OnDestroy();
	}

	// Token: 0x04007ADF RID: 31455
	public bool HasIgniteSynergy;

	// Token: 0x04007AE0 RID: 31456
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007AE1 RID: 31457
	public GameActorFireEffect IgniteEffect;
}
