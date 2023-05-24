using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011B6 RID: 4534
public class MirrorController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x06006525 RID: 25893 RVA: 0x00275774 File Offset: 0x00273974
	private void Start()
	{
		this.PlayerReflection.TargetPlayer = GameManager.Instance.PrimaryPlayer;
		this.PlayerReflection.MirrorSprite = this.MirrorSprite;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.CoopPlayerReflection.TargetPlayer = GameManager.Instance.SecondaryPlayer;
			this.CoopPlayerReflection.MirrorSprite = this.MirrorSprite;
		}
		else
		{
			this.CoopPlayerReflection.gameObject.SetActive(false);
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		Chest chest = GameManager.Instance.RewardManager.GenerationSpawnRewardChestAt(base.transform.position.IntXY(VectorConversions.Round) + new IntVector2(0, -2) - absoluteRoom.area.basePosition, absoluteRoom, null, -1f);
		chest.PreventFuse = true;
		SpriteOutlineManager.RemoveOutlineFromSprite(chest.sprite, false);
		Transform transform = chest.gameObject.transform.Find("Shadow");
		if (transform)
		{
			chest.ShadowSprite = transform.GetComponent<tk2dSprite>();
		}
		chest.IsMirrorChest = true;
		chest.ConfigureOnPlacement(base.GetAbsoluteParentRoom());
		if (chest.majorBreakable)
		{
			chest.majorBreakable.TemporarilyInvulnerable = true;
		}
		this.ChestSprite = chest.sprite;
		this.ChestSprite.renderer.enabled = false;
		this.ChestReflection.TargetSprite = this.ChestSprite;
		this.ChestReflection.MirrorSprite = this.MirrorSprite;
		SpeculativeRigidbody specRigidbody = this.MirrorSprite.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollisionWithMirror));
		MinorBreakable componentInChildren = base.GetComponentInChildren<MinorBreakable>();
		componentInChildren.OnlyBrokenByCode = true;
		componentInChildren.heightOffGround = 4f;
	}

	// Token: 0x06006526 RID: 25894 RVA: 0x00275948 File Offset: 0x00273B48
	private void HandleRigidbodyCollisionWithMirror(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.projectile)
		{
			base.GetAbsoluteParentRoom().DeregisterInteractable(this);
			if (rigidbodyCollision.OtherRigidbody.projectile.Owner is PlayerController)
			{
				base.StartCoroutine(this.HandleShatter(rigidbodyCollision.OtherRigidbody.projectile.Owner as PlayerController, true));
			}
			else
			{
				base.StartCoroutine(this.HandleShatter(GameManager.Instance.PrimaryPlayer, true));
			}
		}
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x002759D0 File Offset: 0x00273BD0
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = this.ChestSprite.GetBounds();
		bounds.SetMinMax(bounds.min + this.ChestSprite.transform.position, bounds.max + this.ChestSprite.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006528 RID: 25896 RVA: 0x00275ABC File Offset: 0x00273CBC
	public void OnEnteredRange(PlayerController interactor)
	{
	}

	// Token: 0x06006529 RID: 25897 RVA: 0x00275AC0 File Offset: 0x00273CC0
	public void OnExitRange(PlayerController interactor)
	{
		MirrorDweller[] componentsInChildren = this.ChestReflection.GetComponentsInChildren<MirrorDweller>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].UsesOverrideTintColor)
			{
				componentsInChildren[i].renderer.enabled = false;
			}
		}
	}

	// Token: 0x0600652A RID: 25898 RVA: 0x00275B0C File Offset: 0x00273D0C
	public void Interact(PlayerController interactor)
	{
		this.ChestSprite.GetComponent<Chest>().ForceOpen(interactor);
		MirrorDweller[] componentsInChildren = this.ChestReflection.GetComponentsInChildren<MirrorDweller>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].UsesOverrideTintColor)
			{
				componentsInChildren[i].renderer.enabled = false;
			}
		}
		base.GetAbsoluteParentRoom().DeregisterInteractable(this);
		base.StartCoroutine(this.HandleShatter(interactor, false));
		for (int j = 0; j < interactor.passiveItems.Count; j++)
		{
			if (interactor.passiveItems[j] is YellowChamberItem)
			{
				break;
			}
		}
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x00275BB8 File Offset: 0x00273DB8
	private IEnumerator HandleShatter(PlayerController interactor, bool skipInitialWait = false)
	{
		if (!skipInitialWait)
		{
			yield return new WaitForSeconds(0.5f);
		}
		if (this)
		{
			AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_pot_shatter_01", base.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_glass_shatter_01", base.gameObject);
		}
		StatModifier curse = new StatModifier();
		curse.statToBoost = PlayerStats.StatType.Curse;
		curse.amount = this.CURSE_EXPOSED;
		curse.modifyType = StatModifier.ModifyMethod.ADDITIVE;
		if (!interactor)
		{
			interactor = GameManager.Instance.PrimaryPlayer;
		}
		if (interactor)
		{
			interactor.ownerlessStatModifiers.Add(curse);
			interactor.stats.RecalculateStats(interactor, false, false);
		}
		MinorBreakable childBreakable = base.GetComponentInChildren<MinorBreakable>();
		if (childBreakable)
		{
			childBreakable.Break();
			while (childBreakable)
			{
				yield return null;
			}
		}
		tk2dSpriteAnimator eyeBall = base.GetComponentInChildren<tk2dSpriteAnimator>();
		if (eyeBall)
		{
			eyeBall.Play("haunted_mirror_eye");
		}
		if (this.ShatterSystem)
		{
			this.ShatterSystem.SetActive(true);
		}
		yield return new WaitForSeconds(2.5f);
		if (this.ShatterSystem)
		{
			this.ShatterSystem.GetComponent<ParticleSystem>().Pause(false);
		}
		yield break;
	}

	// Token: 0x0600652C RID: 25900 RVA: 0x00275BE4 File Offset: 0x00273DE4
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600652D RID: 25901 RVA: 0x00275BF0 File Offset: 0x00273DF0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600652E RID: 25902 RVA: 0x00275BF8 File Offset: 0x00273DF8
	public void ConfigureOnPlacement(RoomHandler room)
	{
		room.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Purple_Lantern") as GameObject;
		if (!room.IsOnCriticalPath && room.connectedRooms.Count == 1)
		{
			room.ShouldAttemptProceduralLock = true;
			room.AttemptProceduralLockChance = Mathf.Max(room.AttemptProceduralLockChance, UnityEngine.Random.Range(0.3f, 0.5f));
		}
	}

	// Token: 0x040060E6 RID: 24806
	public MirrorDweller PlayerReflection;

	// Token: 0x040060E7 RID: 24807
	public MirrorDweller CoopPlayerReflection;

	// Token: 0x040060E8 RID: 24808
	public MirrorDweller ChestReflection;

	// Token: 0x040060E9 RID: 24809
	public tk2dBaseSprite ChestSprite;

	// Token: 0x040060EA RID: 24810
	public tk2dBaseSprite MirrorSprite;

	// Token: 0x040060EB RID: 24811
	public GameObject ShatterSystem;

	// Token: 0x040060EC RID: 24812
	public float CURSE_EXPOSED = 3f;
}
