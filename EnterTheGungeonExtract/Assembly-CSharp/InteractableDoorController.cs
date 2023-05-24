using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200119C RID: 4508
public class InteractableDoorController : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x06006442 RID: 25666 RVA: 0x0026DB44 File Offset: 0x0026BD44
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x06006443 RID: 25667 RVA: 0x0026DB50 File Offset: 0x0026BD50
	private void Start()
	{
		if (this.WorldLocks.Count > 0 && this.WorldLocks[0].lockMode == InteractableLock.InteractableLockMode.NPC_JAIL)
		{
			GameStatsManager.Instance.NumberRunsValidCellWithoutSpawn = 0;
		}
		RoomHandler absoluteParentRoom = base.GetAbsoluteParentRoom();
		for (int i = 0; i < this.WorldLocks.Count; i++)
		{
			absoluteParentRoom.RegisterInteractable(this.WorldLocks[i]);
		}
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
	}

	// Token: 0x06006444 RID: 25668 RVA: 0x0026DBFC File Offset: 0x0026BDFC
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision != null && rigidbodyCollision.OtherRigidbody && rigidbodyCollision.OtherRigidbody.GetComponent<KeyProjModifier>())
		{
			for (int i = 0; i < this.WorldLocks.Count; i++)
			{
				if (this.WorldLocks[i] && this.WorldLocks[i].IsLocked && this.WorldLocks[i].lockMode == InteractableLock.InteractableLockMode.NORMAL)
				{
					this.WorldLocks[i].ForceUnlock();
				}
			}
		}
	}

	// Token: 0x06006445 RID: 25669 RVA: 0x0026DCA4 File Offset: 0x0026BEA4
	private void Update()
	{
		if (!this.m_hasOpened && this.OpensAutomaticallyOnUnlocked && this.IsValidForUse())
		{
			this.Open();
		}
	}

	// Token: 0x06006446 RID: 25670 RVA: 0x0026DCD0 File Offset: 0x0026BED0
	private bool IsValidForUse()
	{
		if (this.m_hasOpened)
		{
			return false;
		}
		bool flag = true;
		for (int i = 0; i < this.WorldLocks.Count; i++)
		{
			if (this.WorldLocks[i].IsLocked || this.WorldLocks[i].spriteAnimator.IsPlaying(this.WorldLocks[i].spriteAnimator.CurrentClip))
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x06006447 RID: 25671 RVA: 0x0026DD54 File Offset: 0x0026BF54
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006448 RID: 25672 RVA: 0x0026DD88 File Offset: 0x0026BF88
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x0026DDB0 File Offset: 0x0026BFB0
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.OpensAutomaticallyOnUnlocked || !this.IsValidForUse())
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600644A RID: 25674 RVA: 0x0026DEAC File Offset: 0x0026C0AC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600644B RID: 25675 RVA: 0x0026DEB4 File Offset: 0x0026C0B4
	private void Open()
	{
		this.m_hasOpened = true;
		base.spriteAnimator.Play();
		base.specRigidbody.enabled = false;
	}

	// Token: 0x0600644C RID: 25676 RVA: 0x0026DED4 File Offset: 0x0026C0D4
	public void Interact(PlayerController player)
	{
		if (this.IsValidForUse())
		{
			this.Open();
		}
	}

	// Token: 0x0600644D RID: 25677 RVA: 0x0026DEE8 File Offset: 0x0026C0E8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600644E RID: 25678 RVA: 0x0026DEF4 File Offset: 0x0026C0F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005FD8 RID: 24536
	public List<InteractableLock> WorldLocks;

	// Token: 0x04005FD9 RID: 24537
	public bool OpensAutomaticallyOnUnlocked;

	// Token: 0x04005FDA RID: 24538
	private bool m_hasOpened;
}
