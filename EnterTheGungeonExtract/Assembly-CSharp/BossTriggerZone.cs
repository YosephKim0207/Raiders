using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200107F RID: 4223
[RequireComponent(typeof(SpeculativeRigidbody))]
public class BossTriggerZone : BraveBehaviour
{
	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06005CEC RID: 23788 RVA: 0x00239498 File Offset: 0x00237698
	// (set) Token: 0x06005CED RID: 23789 RVA: 0x002394A0 File Offset: 0x002376A0
	public bool HasTriggered { get; set; }

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06005CEE RID: 23790 RVA: 0x002394AC File Offset: 0x002376AC
	// (set) Token: 0x06005CEF RID: 23791 RVA: 0x002394B4 File Offset: 0x002376B4
	public RoomHandler ParentRoom { get; set; }

	// Token: 0x06005CF0 RID: 23792 RVA: 0x002394C0 File Offset: 0x002376C0
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		this.ParentRoom = GameManager.Instance.Dungeon.GetRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		if (this.ParentRoom != null)
		{
			if (this.ParentRoom.bossTriggerZones == null)
			{
				this.ParentRoom.bossTriggerZones = new List<BossTriggerZone>();
			}
			this.ParentRoom.bossTriggerZones.Add(this);
		}
	}

	// Token: 0x06005CF1 RID: 23793 RVA: 0x00239558 File Offset: 0x00237758
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005CF2 RID: 23794 RVA: 0x00239560 File Offset: 0x00237760
	private void OnTriggerCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody myRigidbody, CollisionData collisionData)
	{
		if (this.HasTriggered)
		{
			return;
		}
		if (collisionData.OtherPixelCollider.CollisionLayer == CollisionLayer.PlayerCollider || collisionData.OtherPixelCollider.CollisionLayer == CollisionLayer.PlayerHitBox)
		{
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (!component)
			{
				return;
			}
			List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
			for (int i = 0; i < allHealthHavers.Count; i++)
			{
				if (allHealthHavers[i].IsBoss)
				{
					GenericIntroDoer component2 = allHealthHavers[i].GetComponent<GenericIntroDoer>();
					if (component2 && component2.triggerType == GenericIntroDoer.TriggerType.BossTriggerZone)
					{
						ObjectVisibilityManager component3 = component2.GetComponent<ObjectVisibilityManager>();
						component3.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
						component2.TriggerSequence(component);
						this.HasTriggered = true;
						return;
					}
				}
			}
		}
	}
}
