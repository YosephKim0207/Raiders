using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200119D RID: 4509
public class InteractableLock : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006450 RID: 25680 RVA: 0x0026DF14 File Offset: 0x0026C114
	private void Awake()
	{
		StaticReferenceManager.AllLocks.Add(this);
	}

	// Token: 0x06006451 RID: 25681 RVA: 0x0026DF24 File Offset: 0x0026C124
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (this.lockMode == InteractableLock.InteractableLockMode.NPC_JAIL)
		{
			List<PickupObject> list = new List<PickupObject>();
			PickupObject byId = PickupObjectDatabase.GetById(this.JailCellKeyId);
			list.Add(byId);
			MetaInjectionData.CellGeneratedForCurrentBlueprint = true;
			GameManager.Instance.Dungeon.data.DistributeComplexSecretPuzzleItems(list, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)), true, 0.5f);
		}
		yield break;
	}

	// Token: 0x06006452 RID: 25682 RVA: 0x0026DF40 File Offset: 0x0026C140
	private void Update()
	{
		if (!this.IsBusted)
		{
			if (this.IsLocked && !string.IsNullOrEmpty(this.SpitAnimName))
			{
				float num = Vector2.Distance(base.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter);
				if (!this.m_lockHasApproached && num < 2.5f)
				{
					base.spriteAnimator.Play(this.IdleAnimName);
					this.m_lockHasApproached = true;
				}
				else if (num > 2.5f)
				{
					if (this.m_lockHasLaughed)
					{
						base.spriteAnimator.Play(this.SpitAnimName);
					}
					this.m_lockHasLaughed = false;
					this.m_lockHasApproached = false;
				}
				if (!this.m_lockHasSpit && base.spriteAnimator != null && base.spriteAnimator.IsPlaying(this.SpitAnimName) && base.spriteAnimator.CurrentFrame == 3)
				{
					this.m_lockHasSpit = true;
					GameObject gameObject = SpawnManager.SpawnVFX(BraveResources.Load("Global VFX/VFX_Lock_Spit", ".prefab") as GameObject, false);
					tk2dSprite componentInChildren = gameObject.GetComponentInChildren<tk2dSprite>();
					componentInChildren.UpdateZDepth();
					componentInChildren.PlaceAtPositionByAnchor(base.spriteAnimator.sprite.WorldCenter, tk2dBaseSprite.Anchor.UpperCenter);
				}
			}
		}
	}

	// Token: 0x06006453 RID: 25683 RVA: 0x0026E094 File Offset: 0x0026C294
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (!this.Suppress)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x06006454 RID: 25684 RVA: 0x0026E0D4 File Offset: 0x0026C2D4
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (!this.Suppress)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x06006455 RID: 25685 RVA: 0x0026E104 File Offset: 0x0026C304
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.IsBusted || !this.IsLocked || this.Suppress)
		{
			return 10000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006456 RID: 25686 RVA: 0x0026E20C File Offset: 0x0026C40C
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006457 RID: 25687 RVA: 0x0026E214 File Offset: 0x0026C414
	public void BreakLock()
	{
		if (this.IsLocked && !this.IsBusted && this.lockMode == InteractableLock.InteractableLockMode.NORMAL)
		{
			this.IsBusted = true;
			if (!string.IsNullOrEmpty(this.BustedAnimName) && !base.spriteAnimator.IsPlaying(this.BustedAnimName))
			{
				base.spriteAnimator.Play(this.BustedAnimName);
			}
		}
	}

	// Token: 0x06006458 RID: 25688 RVA: 0x0026E280 File Offset: 0x0026C480
	public void ForceUnlock()
	{
		if (!this.IsLocked)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		base.sprite.UpdateZDepth();
		this.IsLocked = false;
		if (this.OnUnlocked != null)
		{
			this.OnUnlocked();
		}
		if (!string.IsNullOrEmpty(this.UnlockAnimName))
		{
			base.spriteAnimator.PlayAndDisableObject(this.UnlockAnimName, null);
		}
	}

	// Token: 0x06006459 RID: 25689 RVA: 0x0026E2F0 File Offset: 0x0026C4F0
	public void Interact(PlayerController player)
	{
		if (this.IsBusted)
		{
			return;
		}
		if (!this.IsLocked)
		{
			return;
		}
		bool flag = false;
		if (this.lockMode == InteractableLock.InteractableLockMode.NORMAL)
		{
			flag = player.carriedConsumables.InfiniteKeys || player.carriedConsumables.KeyBullets >= 1;
		}
		else if (this.lockMode == InteractableLock.InteractableLockMode.RESOURCEFUL_RAT)
		{
			for (int i = 0; i < player.passiveItems.Count; i++)
			{
				if (player.passiveItems[i] is SpecialKeyItem && (player.passiveItems[i] as SpecialKeyItem).keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR)
				{
					flag = true;
					int pickupObjectId = player.passiveItems[i].PickupObjectId;
					player.RemovePassiveItem(pickupObjectId);
					GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
				}
			}
		}
		else if (this.lockMode == InteractableLock.InteractableLockMode.NPC_JAIL)
		{
			for (int j = 0; j < player.additionalItems.Count; j++)
			{
				if (player.additionalItems[j] is NPCCellKeyItem)
				{
					flag = true;
					GameManager.BroadcastRoomFsmEvent("npcCellUnlocked", base.transform.position.GetAbsoluteRoom());
					UnityEngine.Object.Destroy(player.additionalItems[j].gameObject);
					player.additionalItems.RemoveAt(j);
					GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
				}
			}
		}
		else if (this.lockMode == InteractableLock.InteractableLockMode.RAT_REWARD && player.carriedConsumables.ResourcefulRatKeys > 0)
		{
			player.carriedConsumables.ResourcefulRatKeys--;
			flag = true;
			GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
		}
		if (flag)
		{
			this.OnExitRange(player);
			this.IsLocked = false;
			if (this.OnUnlocked != null)
			{
				this.OnUnlocked();
			}
			if (this.lockMode == InteractableLock.InteractableLockMode.NORMAL && !player.carriedConsumables.InfiniteKeys)
			{
				player.carriedConsumables.KeyBullets = player.carriedConsumables.KeyBullets - 1;
			}
			if (!string.IsNullOrEmpty(this.UnlockAnimName))
			{
				base.spriteAnimator.PlayAndDisableObject(this.UnlockAnimName, null);
			}
		}
		else if (!string.IsNullOrEmpty(this.NoKeyAnimName))
		{
			if (!string.IsNullOrEmpty(this.IdleAnimName) && base.spriteAnimator.GetClipByName(this.IdleAnimName) != null)
			{
				if (!string.IsNullOrEmpty(this.SpitAnimName))
				{
					base.spriteAnimator.Play(this.NoKeyAnimName);
				}
				else
				{
					base.spriteAnimator.PlayForDuration(this.NoKeyAnimName, 1f, this.IdleAnimName, false);
				}
				this.m_lockHasSpit = false;
				this.m_lockHasLaughed = true;
			}
			else
			{
				base.spriteAnimator.Play(this.NoKeyAnimName);
			}
		}
	}

	// Token: 0x0600645A RID: 25690 RVA: 0x0026E5CC File Offset: 0x0026C7CC
	private void ChangeToSpit(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		if (base.spriteAnimator)
		{
			base.spriteAnimator.PlayForDuration(this.SpitAnimName, -1f, this.IdleAnimName, false);
		}
	}

	// Token: 0x0600645B RID: 25691 RVA: 0x0026E5FC File Offset: 0x0026C7FC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600645C RID: 25692 RVA: 0x0026E608 File Offset: 0x0026C808
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllLocks.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x04005FDB RID: 24539
	public bool Suppress;

	// Token: 0x04005FDC RID: 24540
	[NonSerialized]
	public bool IsLocked = true;

	// Token: 0x04005FDD RID: 24541
	[NonSerialized]
	public bool HasBeenPicked;

	// Token: 0x04005FDE RID: 24542
	public InteractableLock.InteractableLockMode lockMode;

	// Token: 0x04005FDF RID: 24543
	[PickupIdentifier]
	public int JailCellKeyId = -1;

	// Token: 0x04005FE0 RID: 24544
	[CheckAnimation(null)]
	public string IdleAnimName;

	// Token: 0x04005FE1 RID: 24545
	[CheckAnimation(null)]
	public string UnlockAnimName;

	// Token: 0x04005FE2 RID: 24546
	[CheckAnimation(null)]
	public string NoKeyAnimName;

	// Token: 0x04005FE3 RID: 24547
	[CheckAnimation(null)]
	public string SpitAnimName;

	// Token: 0x04005FE4 RID: 24548
	[CheckAnimation(null)]
	public string BustedAnimName;

	// Token: 0x04005FE5 RID: 24549
	[NonSerialized]
	public bool IsBusted;

	// Token: 0x04005FE6 RID: 24550
	public Action OnUnlocked;

	// Token: 0x04005FE7 RID: 24551
	private bool m_lockHasApproached;

	// Token: 0x04005FE8 RID: 24552
	private bool m_lockHasLaughed;

	// Token: 0x04005FE9 RID: 24553
	private bool m_lockHasSpit;

	// Token: 0x0200119E RID: 4510
	public enum InteractableLockMode
	{
		// Token: 0x04005FEB RID: 24555
		NORMAL,
		// Token: 0x04005FEC RID: 24556
		RESOURCEFUL_RAT,
		// Token: 0x04005FED RID: 24557
		NPC_JAIL,
		// Token: 0x04005FEE RID: 24558
		RAT_REWARD
	}
}
