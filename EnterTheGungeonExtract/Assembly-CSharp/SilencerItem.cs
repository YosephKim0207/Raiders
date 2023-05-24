using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020014A9 RID: 5289
public class SilencerItem : PlayerItem
{
	// Token: 0x06007847 RID: 30791 RVA: 0x003017BC File Offset: 0x002FF9BC
	protected override void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.TriggerWasEntered));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		base.Start();
	}

	// Token: 0x06007848 RID: 30792 RVA: 0x00301820 File Offset: 0x002FFA20
	private void OnTrigger(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.Pickup(component);
		}
	}

	// Token: 0x06007849 RID: 30793 RVA: 0x00301854 File Offset: 0x002FFA54
	private void TriggerWasEntered(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.Pickup(component);
		}
	}

	// Token: 0x0600784A RID: 30794 RVA: 0x00301888 File Offset: 0x002FFA88
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerAcquiredPlayerItem");
		}
		this.m_pickedUp = true;
		if (!GameManager.Instance.InTutorial && GameStatsManager.Instance.QueryEncounterable(base.encounterTrackable) < 3)
		{
			base.HandleEncounterable(player);
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		base.GetRidOfMinimapIcon();
		AkSoundEngine.PostEvent("Play_OBJ_item_pickup_01", base.gameObject);
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Pickup");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
		component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
		component.UpdateZDepth();
		DebrisObject component2 = base.GetComponent<DebrisObject>();
		if (this.ForceAsExtant || component2 != null)
		{
			player.Blanks++;
			UnityEngine.Object.Destroy(base.gameObject);
			this.m_pickedUp = true;
			this.m_pickedUpThisRun = true;
		}
		else
		{
			player.Blanks++;
			this.m_pickedUp = true;
			this.m_pickedUpThisRun = true;
		}
		base.GetRidOfMinimapIcon();
	}

	// Token: 0x0600784B RID: 30795 RVA: 0x003019C8 File Offset: 0x002FFBC8
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", base.gameObject);
		AkSoundEngine.PostEvent("Stop_ENM_attack_cancel_01", base.gameObject);
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		silencerInstance.TriggerSilencer(user.CenterPosition, this.silencerSpeed, this.silencerRadius, this.silencerVFXPrefab, this.distortionIntensity, this.distortionRadius, this.pushForce, this.pushRadius, this.knockbackForce, this.knockbackRadius, this.additionalTimeAtMaxRadius, user, true, false);
	}

	// Token: 0x0600784C RID: 30796 RVA: 0x00301A54 File Offset: 0x002FFC54
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007A77 RID: 31351
	public bool destroysEnemyBullets = true;

	// Token: 0x04007A78 RID: 31352
	public bool destroysPlayerBullets;

	// Token: 0x04007A79 RID: 31353
	public float silencerRadius = 25f;

	// Token: 0x04007A7A RID: 31354
	public float silencerSpeed = 50f;

	// Token: 0x04007A7B RID: 31355
	public float additionalTimeAtMaxRadius = 1f;

	// Token: 0x04007A7C RID: 31356
	public float distortionIntensity = 0.1f;

	// Token: 0x04007A7D RID: 31357
	public float distortionRadius = 0.1f;

	// Token: 0x04007A7E RID: 31358
	public float pushForce = 12f;

	// Token: 0x04007A7F RID: 31359
	public float pushRadius = 10f;

	// Token: 0x04007A80 RID: 31360
	public float knockbackForce = 12f;

	// Token: 0x04007A81 RID: 31361
	public float knockbackRadius = 7f;

	// Token: 0x04007A82 RID: 31362
	public GameObject silencerVFXPrefab;
}
