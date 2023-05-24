using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001189 RID: 4489
public class HeartDispenser : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x060063C1 RID: 25537 RVA: 0x0026BC28 File Offset: 0x00269E28
	public static void ClearPerLevelData()
	{
		HeartDispenser.CurrentHalfHeartsStored = 0;
		HeartDispenser.DispenserOnFloor = false;
	}

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x060063C2 RID: 25538 RVA: 0x0026BC38 File Offset: 0x00269E38
	// (set) Token: 0x060063C3 RID: 25539 RVA: 0x0026BC40 File Offset: 0x00269E40
	public static int CurrentHalfHeartsStored
	{
		get
		{
			return HeartDispenser.m_currentHalfHeartsStored;
		}
		set
		{
			HeartDispenser.m_currentHalfHeartsStored = value;
		}
	}

	// Token: 0x060063C4 RID: 25540 RVA: 0x0026BC48 File Offset: 0x00269E48
	private void UpdateVisuals()
	{
		if (HeartDispenser.CurrentHalfHeartsStored > 0)
		{
			this.m_currentBaseAnimation = "heart_dispenser_idle_full";
		}
		else
		{
			this.m_currentBaseAnimation = "heart_dispenser_idle_empty";
		}
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x0026BC70 File Offset: 0x00269E70
	public void Awake()
	{
		HeartDispenser.DispenserOnFloor = true;
	}

	// Token: 0x060063C6 RID: 25542 RVA: 0x0026BC78 File Offset: 0x00269E78
	private void Start()
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
	}

	// Token: 0x060063C7 RID: 25543 RVA: 0x0026BC98 File Offset: 0x00269E98
	public void Update()
	{
		if (this.m_cachedStored != HeartDispenser.CurrentHalfHeartsStored)
		{
			this.m_cachedStored = HeartDispenser.CurrentHalfHeartsStored;
			this.UpdateVisuals();
		}
		if ((base.spriteAnimator.IsPlaying("heart_dispenser_idle_empty") || base.spriteAnimator.IsPlaying("heart_dispenser_idle_full")) && base.spriteAnimator.CurrentClip.name != this.m_currentBaseAnimation)
		{
			base.spriteAnimator.Play(this.m_currentBaseAnimation);
		}
		if (this.m_isVisible && !this.m_hasEverBeenRevealed && HeartDispenser.CurrentHalfHeartsStored == 0)
		{
			this.m_isVisible = false;
			this.ToggleRenderers(false);
		}
		else if (!this.m_isVisible && (this.m_hasEverBeenRevealed || HeartDispenser.CurrentHalfHeartsStored > 0))
		{
			this.m_hasEverBeenRevealed = true;
			this.m_isVisible = true;
			this.ToggleRenderers(true);
		}
	}

	// Token: 0x060063C8 RID: 25544 RVA: 0x0026BD8C File Offset: 0x00269F8C
	private void ToggleRenderers(bool state)
	{
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, state);
		base.renderer.enabled = state;
		base.transform.Find("shadow").GetComponent<MeshRenderer>().enabled = state;
		base.specRigidbody.enabled = state;
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x0026BDD8 File Offset: 0x00269FD8
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(base.specRigidbody.UnitBottomCenter, point);
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x0026BDEC File Offset: 0x00269FEC
	public void OnEnteredRange(PlayerController interactor)
	{
		if (interactor.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			return;
		}
		if (HeartDispenser.CurrentHalfHeartsStored > 0)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060063CB RID: 25547 RVA: 0x0026BE4C File Offset: 0x0026A04C
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x0026BE78 File Offset: 0x0026A078
	public void Interact(PlayerController interactor)
	{
		if (HeartDispenser.CurrentHalfHeartsStored > 0 && interactor.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			base.spriteAnimator.PlayForDuration("heart_dispenser_no", -1f, this.m_currentBaseAnimation, false);
		}
		else if (HeartDispenser.CurrentHalfHeartsStored > 0)
		{
			HeartDispenser.CurrentHalfHeartsStored--;
			base.spriteAnimator.PlayForDuration("heart_dispenser_dispense", -1f, this.m_currentBaseAnimation, false);
			UnityEngine.Object.Instantiate<GameObject>(this.dustVFX, base.transform.position, Quaternion.identity);
			base.StartCoroutine(this.DelayedSpawnHalfHeart());
		}
		else
		{
			base.spriteAnimator.PlayForDuration("heart_dispenser_empty", -1f, this.m_currentBaseAnimation, false);
		}
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x0026BF44 File Offset: 0x0026A144
	private IEnumerator DelayedSpawnHalfHeart()
	{
		yield return new WaitForSeconds(1.125f);
		PickupObject halfHeart = PickupObjectDatabase.GetById(this.halfHeartId);
		LootEngine.SpawnItem(halfHeart.gameObject, base.specRigidbody.PrimaryPixelCollider.UnitCenter - halfHeart.sprite.GetBounds().extents.XY(), Vector2.down, 1f, true, false, false);
		yield break;
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x0026BF60 File Offset: 0x0026A160
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060063CF RID: 25551 RVA: 0x0026BF6C File Offset: 0x0026A16C
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x04005F62 RID: 24418
	[PickupIdentifier]
	public int halfHeartId = -1;

	// Token: 0x04005F63 RID: 24419
	public GameObject dustVFX;

	// Token: 0x04005F64 RID: 24420
	private bool m_isVisible = true;

	// Token: 0x04005F65 RID: 24421
	private bool m_hasEverBeenRevealed;

	// Token: 0x04005F66 RID: 24422
	public static bool DispenserOnFloor;

	// Token: 0x04005F67 RID: 24423
	private static int m_currentHalfHeartsStored;

	// Token: 0x04005F68 RID: 24424
	private int m_cachedStored;

	// Token: 0x04005F69 RID: 24425
	private string m_currentBaseAnimation = "heart_dispenser_idle_empty";
}
