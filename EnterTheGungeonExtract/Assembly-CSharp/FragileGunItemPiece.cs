using System;
using UnityEngine;

// Token: 0x020013B1 RID: 5041
public class FragileGunItemPiece : PickupObject
{
	// Token: 0x06007239 RID: 29241 RVA: 0x002D6494 File Offset: 0x002D4694
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.TriggerWasEntered));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		this.IgnoredByRat = true;
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x0600723A RID: 29242 RVA: 0x002D6508 File Offset: 0x002D4708
	public void AssignGun(Gun sourceGun)
	{
		this.AssignedGunId = sourceGun.PickupObjectId;
		if (sourceGun.sprite)
		{
			base.sprite.SetSprite(sourceGun.sprite.Collection, sourceGun.sprite.spriteId);
		}
	}

	// Token: 0x0600723B RID: 29243 RVA: 0x002D6548 File Offset: 0x002D4748
	private void TriggerWasEntered(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (otherRigidbody.GetComponent<PlayerController>() != null)
		{
			this.PrePickupLogic(otherRigidbody, selfRigidbody);
		}
		else if (otherRigidbody.GetComponent<PickupObject>() != null && base.debris)
		{
			base.debris.ApplyVelocity((selfRigidbody.UnitCenter - otherRigidbody.UnitCenter).normalized);
			selfRigidbody.RegisterGhostCollisionException(otherRigidbody);
		}
	}

	// Token: 0x0600723C RID: 29244 RVA: 0x002D65CC File Offset: 0x002D47CC
	public void OnTrigger(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (otherRigidbody.GetComponent<PlayerController>() != null)
		{
			this.PrePickupLogic(otherRigidbody, selfRigidbody);
		}
	}

	// Token: 0x0600723D RID: 29245 RVA: 0x002D65F4 File Offset: 0x002D47F4
	private void PrePickupLogic(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody)
	{
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component.IsGhost)
		{
			return;
		}
		if (!this.CheckPlayerForItem(component))
		{
			return;
		}
		this.Pickup(component);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600723E RID: 29246 RVA: 0x002D6634 File Offset: 0x002D4834
	private bool CheckPlayerForItem(PlayerController player)
	{
		if (player)
		{
			for (int i = 0; i < player.passiveItems.Count; i++)
			{
				if (player.passiveItems[i] is FragileGunItem)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600723F RID: 29247 RVA: 0x002D6684 File Offset: 0x002D4884
	public override void Pickup(PlayerController player)
	{
		if (player.IsGhost)
		{
			return;
		}
		this.m_pickedUp = true;
		FragileGunItem fragileGunItem = null;
		for (int i = 0; i < player.passiveItems.Count; i++)
		{
			if (player.passiveItems[i] is FragileGunItem)
			{
				fragileGunItem = player.passiveItems[i] as FragileGunItem;
				break;
			}
		}
		if (fragileGunItem)
		{
			fragileGunItem.AcquirePiece(this);
		}
	}

	// Token: 0x0400739B RID: 29595
	[NonSerialized]
	public int AssignedGunId = -1;

	// Token: 0x0400739C RID: 29596
	private bool m_pickedUp;
}
