using System;
using UnityEngine;

// Token: 0x020014AC RID: 5292
public class SimplePuzzleItem : PickupObject
{
	// Token: 0x06007859 RID: 30809 RVA: 0x00301F10 File Offset: 0x00300110
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
	}

	// Token: 0x0600785A RID: 30810 RVA: 0x00301F3C File Offset: 0x0030013C
	private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.Pickup(component);
			AkSoundEngine.PostEvent("Play_OBJ_item_pickup_01", base.gameObject);
		}
	}

	// Token: 0x0600785B RID: 30811 RVA: 0x00301F80 File Offset: 0x00300180
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_pickedUp = true;
		base.specRigidbody.enabled = false;
		base.renderer.enabled = false;
		DebrisObject component = base.GetComponent<DebrisObject>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
			UnityEngine.Object.Destroy(base.specRigidbody);
			player.AcquirePuzzleItem(this);
		}
		else
		{
			UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			player.AcquirePuzzleItem(this);
		}
	}

	// Token: 0x0600785C RID: 30812 RVA: 0x00301FFC File Offset: 0x003001FC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007A8F RID: 31375
	private bool m_pickedUp;
}
