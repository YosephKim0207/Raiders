using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020014BC RID: 5308
public class SprenOrbitalItem : PlayerOrbitalItem
{
	// Token: 0x060078A7 RID: 30887 RVA: 0x00303D44 File Offset: 0x00301F44
	private void Start()
	{
		this.AssignTrigger();
	}

	// Token: 0x060078A8 RID: 30888 RVA: 0x00303D4C File Offset: 0x00301F4C
	private void AssignTrigger()
	{
		if (this.m_trigger == SprenOrbitalItem.SprenTrigger.UNASSIGNED)
		{
			this.m_trigger = (SprenOrbitalItem.SprenTrigger)UnityEngine.Random.Range(1, 11);
		}
		if (this.m_secondaryTrigger == SprenOrbitalItem.SprenTrigger.UNASSIGNED)
		{
			while (this.m_secondaryTrigger == SprenOrbitalItem.SprenTrigger.UNASSIGNED || this.m_secondaryTrigger == this.m_trigger)
			{
				this.m_secondaryTrigger = (SprenOrbitalItem.SprenTrigger)UnityEngine.Random.Range(1, 11);
			}
		}
	}

	// Token: 0x060078A9 RID: 30889 RVA: 0x00303DAC File Offset: 0x00301FAC
	private bool CheckTrigger(SprenOrbitalItem.SprenTrigger target, bool force = false)
	{
		return force || (this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.SHARDBLADE, false) && this.m_secondaryTrigger == target) || this.m_trigger == target;
	}

	// Token: 0x060078AA RID: 30890 RVA: 0x00303DFC File Offset: 0x00301FFC
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.m_player = player;
		this.AssignTrigger();
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.USED_LAST_BLANK, true))
		{
			player.OnUsedBlank += this.HandleBlank;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.LOST_LAST_ARMOR, true))
		{
			player.LostArmor = (Action)Delegate.Combine(player.LostArmor, new Action(this.HandleLostArmor));
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.ELECTROCUTED_OR_POISONED, true) || this.CheckTrigger(SprenOrbitalItem.SprenTrigger.TOOK_ANY_HEART_DAMAGE, true) || this.CheckTrigger(SprenOrbitalItem.SprenTrigger.REDUCED_TO_ONE_HEALTH, true))
		{
			player.healthHaver.OnDamaged += this.HandleDamaged;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.ACTIVE_ITEM_USED, true))
		{
			player.OnUsedPlayerItem += this.HandleActiveItemUsed;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.FLIPPED_TABLE, true))
		{
			player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.HandleTableFlipped));
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.FELL_IN_PIT, true))
		{
			player.OnPitfall += this.HandlePitfall;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.SET_ON_FIRE, true))
		{
			player.OnIgnited = (Action<PlayerController>)Delegate.Combine(player.OnIgnited, new Action<PlayerController>(this.HandleIgnited));
		}
	}

	// Token: 0x060078AB RID: 30891 RVA: 0x00303F48 File Offset: 0x00302148
	protected override void Update()
	{
		if (this.m_transformation == SprenOrbitalItem.SprenTransformationState.TRANSFORMED && (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating || (this.m_player && this.m_player.CurrentRoom != null && this.m_player.CurrentRoom.IsWinchesterArcadeRoom)))
		{
			this.DetransformSpren();
		}
		if (this.m_transformation == SprenOrbitalItem.SprenTransformationState.NORMAL && this.CheckTrigger(SprenOrbitalItem.SprenTrigger.GUN_OUT_OF_AMMO, false) && this.m_player && this.m_player.CurrentGun)
		{
			if (!this.m_player.CurrentGun.InfiniteAmmo && this.m_player.CurrentGun.ammo <= 0 && this.m_player.CurrentGun.PickupObjectId == this.m_lastEquippedGunID && this.m_lastEquippedGunAmmo > 0)
			{
				this.TransformSpren();
			}
			this.m_lastEquippedGunID = this.m_player.CurrentGun.PickupObjectId;
			this.m_lastEquippedGunAmmo = this.m_player.CurrentGun.ammo;
		}
		base.Update();
	}

	// Token: 0x060078AC RID: 30892 RVA: 0x0030407C File Offset: 0x0030227C
	private void HandleIgnited(PlayerController obj)
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.SET_ON_FIRE, false))
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078AD RID: 30893 RVA: 0x003040A0 File Offset: 0x003022A0
	private void HandlePitfall()
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.FELL_IN_PIT, false))
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078AE RID: 30894 RVA: 0x003040C4 File Offset: 0x003022C4
	private void HandleTableFlipped(FlippableCover obj)
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.FLIPPED_TABLE, false))
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078AF RID: 30895 RVA: 0x003040E8 File Offset: 0x003022E8
	private void HandleActiveItemUsed(PlayerController arg1, PlayerItem arg2)
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.ACTIVE_ITEM_USED, false))
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078B0 RID: 30896 RVA: 0x0030410C File Offset: 0x0030230C
	private void HandleLostArmor()
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.LOST_LAST_ARMOR, false) && ((!this.m_player.ForceZeroHealthState && this.m_player.healthHaver.Armor == 0f) || (this.m_player.ForceZeroHealthState && this.m_player.healthHaver.Armor == 1f)))
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078B1 RID: 30897 RVA: 0x0030418C File Offset: 0x0030238C
	private void HandleDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.ELECTROCUTED_OR_POISONED, false) && ((damageTypes | CoreDamageTypes.Electric) == damageTypes || (damageTypes | CoreDamageTypes.Poison) == damageTypes))
		{
			this.TransformSpren();
		}
		else if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.TOOK_ANY_HEART_DAMAGE, false) && this.m_player.healthHaver.Armor == 0f)
		{
			this.TransformSpren();
		}
		else if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.REDUCED_TO_ONE_HEALTH, false) && this.m_player.healthHaver.GetCurrentHealth() <= 0.5f)
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078B2 RID: 30898 RVA: 0x00304230 File Offset: 0x00302430
	private void HandleBlank(PlayerController arg1, int BlanksRemaining)
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.CheckTrigger(SprenOrbitalItem.SprenTrigger.USED_LAST_BLANK, false) && BlanksRemaining == 0)
		{
			this.TransformSpren();
		}
	}

	// Token: 0x060078B3 RID: 30899 RVA: 0x00304258 File Offset: 0x00302458
	private void Disconnect(PlayerController player)
	{
		player.OnUsedBlank -= this.HandleBlank;
		player.LostArmor = (Action)Delegate.Remove(player.LostArmor, new Action(this.HandleLostArmor));
		player.healthHaver.OnDamaged -= this.HandleDamaged;
		player.OnUsedPlayerItem -= this.HandleActiveItemUsed;
		player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.HandleTableFlipped));
		player.OnPitfall -= this.HandlePitfall;
		player.OnIgnited = (Action<PlayerController>)Delegate.Remove(player.OnIgnited, new Action<PlayerController>(this.HandleIgnited));
	}

	// Token: 0x060078B4 RID: 30900 RVA: 0x00304318 File Offset: 0x00302518
	public override DebrisObject Drop(PlayerController player)
	{
		this.Disconnect(player);
		return base.Drop(player);
	}

	// Token: 0x060078B5 RID: 30901 RVA: 0x00304328 File Offset: 0x00302528
	protected void TransformSpren()
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.NORMAL)
		{
			return;
		}
		if (this.m_player && this.m_player.CurrentRoom != null && this.m_player.CurrentRoom.IsWinchesterArcadeRoom)
		{
			return;
		}
		this.m_transformation = SprenOrbitalItem.SprenTransformationState.PRE_TRANSFORM;
		if (this.m_player && !this.m_player.IsGhost)
		{
			this.m_player.StartCoroutine(this.HandleTransformationDuration());
		}
	}

	// Token: 0x060078B6 RID: 30902 RVA: 0x003043B0 File Offset: 0x003025B0
	private IEnumerator HandleTransformationDuration()
	{
		tk2dSpriteAnimator extantAnimator = this.m_extantOrbital.GetComponentInChildren<tk2dSpriteAnimator>();
		extantAnimator.Play(this.GunChangeAnimation);
		PlayerOrbitalFollower follower = this.m_extantOrbital.GetComponent<PlayerOrbitalFollower>();
		if (follower)
		{
			follower.OverridePosition = true;
		}
		float elapsed = 0f;
		extantAnimator.sprite.HeightOffGround = 5f;
		while (elapsed < 1f)
		{
			elapsed += BraveTime.DeltaTime;
			if (follower && this.m_player)
			{
				follower.OverrideTargetPosition = this.m_player.CenterPosition;
			}
			yield return null;
		}
		extantAnimator.Play(this.GunChangeMoreAnimation);
		while (extantAnimator.IsPlaying(this.GunChangeMoreAnimation))
		{
			if (follower && this.m_player)
			{
				follower.OverrideTargetPosition = this.m_player.CenterPosition;
			}
			yield return null;
		}
		if (follower)
		{
			follower.ToggleRenderer(false);
		}
		this.m_player.inventory.GunChangeForgiveness = true;
		this.m_transformation = SprenOrbitalItem.SprenTransformationState.TRANSFORMED;
		Gun limitGun = PickupObjectDatabase.GetById(this.LimitGunId) as Gun;
		this.m_extantGun = this.m_player.inventory.AddGunToInventory(limitGun, true);
		this.m_extantGun.CanBeDropped = false;
		this.m_extantGun.CanBeSold = false;
		this.m_player.inventory.GunLocked.SetOverride("spren gun", true, null);
		elapsed = 0f;
		while (elapsed < this.LimitDuration)
		{
			if (follower && this.m_player)
			{
				follower.OverrideTargetPosition = this.m_player.CenterPosition;
			}
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (follower)
		{
			follower.ToggleRenderer(true);
		}
		if (extantAnimator)
		{
			extantAnimator.PlayForDuration(this.BackchangeAnimation, -1f, this.IdleAnimation, false);
		}
		while (extantAnimator.IsPlaying(this.BackchangeAnimation))
		{
			if (follower && this.m_player)
			{
				follower.OverrideTargetPosition = this.m_player.CenterPosition;
			}
			yield return null;
		}
		follower.OverridePosition = false;
		this.DetransformSpren();
		yield break;
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x003043CC File Offset: 0x003025CC
	protected void DetransformSpren()
	{
		if (this.m_transformation != SprenOrbitalItem.SprenTransformationState.TRANSFORMED)
		{
			return;
		}
		if (!this || !this.m_player || !this.m_extantGun)
		{
			return;
		}
		this.m_transformation = SprenOrbitalItem.SprenTransformationState.NORMAL;
		if (this.m_player)
		{
			if (!GameManager.Instance.IsLoadingLevel && !Dungeon.IsGenerating)
			{
				Minimap.Instance.ToggleMinimap(false, false);
			}
			this.m_player.inventory.GunLocked.RemoveOverride("spren gun");
			this.m_player.inventory.DestroyGun(this.m_extantGun);
			this.m_extantGun = null;
		}
		this.m_player.inventory.GunChangeForgiveness = false;
	}

	// Token: 0x060078B8 RID: 30904 RVA: 0x00304498 File Offset: 0x00302698
	protected override void OnDestroy()
	{
		if (this.m_player)
		{
			this.Disconnect(this.m_player);
		}
		this.m_player = null;
		base.OnDestroy();
	}

	// Token: 0x04007AE2 RID: 31458
	[PickupIdentifier]
	public int LimitGunId = -1;

	// Token: 0x04007AE3 RID: 31459
	public float LimitDuration = 15f;

	// Token: 0x04007AE4 RID: 31460
	public string IdleAnimation;

	// Token: 0x04007AE5 RID: 31461
	public string GunChangeAnimation;

	// Token: 0x04007AE6 RID: 31462
	public string GunChangeMoreAnimation;

	// Token: 0x04007AE7 RID: 31463
	public string BackchangeAnimation;

	// Token: 0x04007AE8 RID: 31464
	private SprenOrbitalItem.SprenTrigger m_trigger;

	// Token: 0x04007AE9 RID: 31465
	private SprenOrbitalItem.SprenTrigger m_secondaryTrigger;

	// Token: 0x04007AEA RID: 31466
	private PlayerController m_player;

	// Token: 0x04007AEB RID: 31467
	private Gun m_extantGun;

	// Token: 0x04007AEC RID: 31468
	private SprenOrbitalItem.SprenTransformationState m_transformation;

	// Token: 0x04007AED RID: 31469
	private int m_lastEquippedGunID = -1;

	// Token: 0x04007AEE RID: 31470
	private int m_lastEquippedGunAmmo = -1;

	// Token: 0x020014BD RID: 5309
	public enum SprenTrigger
	{
		// Token: 0x04007AF0 RID: 31472
		UNASSIGNED,
		// Token: 0x04007AF1 RID: 31473
		USED_LAST_BLANK,
		// Token: 0x04007AF2 RID: 31474
		LOST_LAST_ARMOR,
		// Token: 0x04007AF3 RID: 31475
		REDUCED_TO_ONE_HEALTH,
		// Token: 0x04007AF4 RID: 31476
		GUN_OUT_OF_AMMO,
		// Token: 0x04007AF5 RID: 31477
		SET_ON_FIRE,
		// Token: 0x04007AF6 RID: 31478
		ELECTROCUTED_OR_POISONED,
		// Token: 0x04007AF7 RID: 31479
		FELL_IN_PIT,
		// Token: 0x04007AF8 RID: 31480
		TOOK_ANY_HEART_DAMAGE,
		// Token: 0x04007AF9 RID: 31481
		FLIPPED_TABLE,
		// Token: 0x04007AFA RID: 31482
		ACTIVE_ITEM_USED
	}

	// Token: 0x020014BE RID: 5310
	private enum SprenTransformationState
	{
		// Token: 0x04007AFC RID: 31484
		NORMAL,
		// Token: 0x04007AFD RID: 31485
		PRE_TRANSFORM,
		// Token: 0x04007AFE RID: 31486
		TRANSFORMED
	}
}
