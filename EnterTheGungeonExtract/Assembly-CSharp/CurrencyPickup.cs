using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E59 RID: 3673
public class CurrencyPickup : PickupObject
{
	// Token: 0x06004E35 RID: 20021 RVA: 0x001B01A0 File Offset: 0x001AE3A0
	private void Start()
	{
		this.m_transform = base.transform;
		this.m_transform.position = this.m_transform.position.Quantize(0.0625f);
		this.m_srb = base.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody srb = this.m_srb;
		srb.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(srb.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
		this.m_srb.Reinitialize();
		if (base.debris)
		{
			if (this.IsMetaCurrency)
			{
				base.debris.FlagAsPickup();
			}
			DebrisObject debris = base.debris;
			debris.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debris.OnGrounded, new Action<DebrisObject>(delegate(DebrisObject a)
			{
				base.specRigidbody.Reinitialize();
			}));
			base.debris.ForceUpdateIfDisabled = true;
		}
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x001B0270 File Offset: 0x001AE470
	private void Update()
	{
		if (!this.IsMetaCurrency)
		{
			if (base.spriteAnimator != null && base.spriteAnimator.DefaultClip != null)
			{
				if (base.spriteAnimator.IsPlaying(base.spriteAnimator.CurrentClip))
				{
					base.spriteAnimator.Stop();
				}
				base.spriteAnimator.SetFrame(Mathf.FloorToInt(Time.time * base.spriteAnimator.DefaultClip.fps % (float)base.spriteAnimator.DefaultClip.frames.Length));
			}
		}
		else if (!GameManager.Instance.IsLoadingLevel && !this.m_hasBeenPickedUp)
		{
			if (!this.m_hasBeenPickedUp && base.debris && base.debris.specRigidbody && base.debris.Static && PhysicsEngine.Instance.OverlapCast(base.debris.specRigidbody, null, true, true, new int?(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle)), null, false, null, null, new SpeculativeRigidbody[0]))
			{
				base.debris.ForceDestroyAndMaybeRespawn();
			}
			if (this && !GameManager.Instance.IsAnyPlayerInRoom(base.transform.position.GetAbsoluteRoom()))
			{
				PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
				if (bestActivePlayer && !bestActivePlayer.IsGhost && bestActivePlayer.AcceptingAnyInput)
				{
					this.m_hasBeenPickedUp = true;
					this.Pickup(bestActivePlayer);
				}
			}
			else if (GameManager.Instance.PrimaryPlayer && base.sprite)
			{
				CellData cellData = null;
				IntVector2 intVector = base.sprite.WorldCenter.ToIntVector2(VectorConversions.Floor);
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
				{
					cellData = GameManager.Instance.Dungeon.data[intVector];
				}
				if (cellData == null || cellData.type == CellType.WALL)
				{
					this.m_hasBeenPickedUp = true;
					this.Pickup(GameManager.Instance.PrimaryPlayer);
				}
			}
		}
	}

	// Token: 0x06004E37 RID: 20023 RVA: 0x001B04B4 File Offset: 0x001AE6B4
	private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (this.PreventPickup)
		{
			return;
		}
		if (this.m_hasBeenPickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.m_hasBeenPickedUp = true;
			this.Pickup(component);
		}
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x001B04FC File Offset: 0x001AE6FC
	private IEnumerator InitialBounce()
	{
		float startingY = this.m_transform.position.y;
		Vector2 currentVelocity = Vector2.up * 5f;
		while (this.m_srb.Velocity.y >= 0f || this.m_transform.position.y > startingY)
		{
			if (this.m_hasBeenPickedUp)
			{
				break;
			}
			currentVelocity += -10f * Vector2.up * BraveTime.DeltaTime;
			this.m_srb.Velocity = currentVelocity;
			yield return null;
		}
		this.m_srb.Velocity = Vector2.zero;
		yield break;
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001B0518 File Offset: 0x001AE718
	public void ForceSetPickedUp()
	{
		this.m_hasBeenPickedUp = true;
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001B0524 File Offset: 0x001AE724
	public override void Pickup(PlayerController player)
	{
		if (this.IsMetaCurrency)
		{
			base.spriteAnimator.StopAndResetFrame();
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, (float)this.currencyValue);
			tk2dBaseSprite targetAutomaticSprite = base.GetComponent<HologramDoer>().TargetAutomaticSprite;
			targetAutomaticSprite.spriteAnimator.StopAndResetFrame();
			player.BloopItemAboveHead(targetAutomaticSprite, this.overrideBloopSpriteName);
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Pickup");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
			component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
			component.UpdateZDepth();
			if (base.encounterTrackable)
			{
				base.encounterTrackable.DoNotificationOnEncounter = false;
				base.encounterTrackable.HandleEncounter();
				AkSoundEngine.PostEvent("Play_OBJ_metacoin_collect_01", base.gameObject);
			}
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) == 1f)
			{
				GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetItemsString("#HEGEMONYCREDIT_ENCNAME", -1), StringTableManager.GetItemsString("#HEGEMONYCREDIT_SHORTDESC", -1), targetAutomaticSprite.Collection, targetAutomaticSprite.spriteId, UINotificationController.NotificationColor.GOLD, false, false);
			}
		}
		else
		{
			base.HandleEncounterable(player);
			player.BloopItemAboveHead(base.sprite, this.overrideBloopSpriteName);
			if (base.sprite.name == "Coin_Copper(Clone)")
			{
				AkSoundEngine.PostEvent("Play_OBJ_coin_small_01", base.gameObject);
			}
			else if (base.sprite.name == "Coin_Silver(Clone)")
			{
				AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Play_OBJ_coin_large_01", base.gameObject);
			}
			GameManager.Instance.PrimaryPlayer.carriedConsumables.Currency += this.currencyValue;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001B06F8 File Offset: 0x001AE8F8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400448E RID: 17550
	public int currencyValue = 1;

	// Token: 0x0400448F RID: 17551
	public string overrideBloopSpriteName = string.Empty;

	// Token: 0x04004490 RID: 17552
	public bool IsMetaCurrency;

	// Token: 0x04004491 RID: 17553
	private bool m_hasBeenPickedUp;

	// Token: 0x04004492 RID: 17554
	private Transform m_transform;

	// Token: 0x04004493 RID: 17555
	private SpeculativeRigidbody m_srb;

	// Token: 0x04004494 RID: 17556
	[NonSerialized]
	public bool PreventPickup;
}
