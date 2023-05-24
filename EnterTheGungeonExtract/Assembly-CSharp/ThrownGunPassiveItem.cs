using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020014DB RID: 5339
public class ThrownGunPassiveItem : PassiveItem
{
	// Token: 0x06007961 RID: 31073 RVA: 0x0030915C File Offset: 0x0030735C
	private void Awake()
	{
		this.m_damageStat = new StatModifier();
		this.m_damageStat.statToBoost = PlayerStats.StatType.DodgeRollDamage;
		this.m_damageStat.modifyType = StatModifier.ModifyMethod.ADDITIVE;
		this.m_damageStat.amount = (float)this.AdditionalRollDamage;
	}

	// Token: 0x06007962 RID: 31074 RVA: 0x00309194 File Offset: 0x00307394
	public void EnableVFX(PlayerController target)
	{
		if (this.m_destroyVFXSemaphore == 0)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 0f, 99f));
			}
			if (this.OverheadVFX && !this.m_instanceVFX)
			{
				this.m_instanceVFX = target.PlayEffectOnActor(this.OverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
			}
		}
	}

	// Token: 0x06007963 RID: 31075 RVA: 0x0030922C File Offset: 0x0030742C
	public void DisableVFX(PlayerController target)
	{
		if (this.m_destroyVFXSemaphore == 0)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			}
			if (this.m_instanceVFX)
			{
				SpawnManager.Despawn(this.m_instanceVFX);
				this.m_instanceVFX = null;
			}
		}
	}

	// Token: 0x06007964 RID: 31076 RVA: 0x003092A0 File Offset: 0x003074A0
	protected override void Update()
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			this.m_motionTimer = 0f;
			this.m_hasUsedMomentum = true;
			return;
		}
		if (this.m_pickedUp && base.Owner && this.HasFlagContingentMomentum && this.m_cachedFlag)
		{
			base.Owner.ReceivesTouchDamage = false;
			if (this.m_destroyVFXSemaphore <= 0)
			{
				if (base.Owner.Velocity.magnitude > 0.05f)
				{
					this.m_motionTimer += BraveTime.DeltaTime;
					if (this.m_motionTimer > this.TimeInMotion)
					{
						this.ForceTrigger(base.Owner);
						this.m_motionTimer = 0f;
					}
				}
				else
				{
					this.m_hasUsedMomentum = true;
					this.m_motionTimer = 0f;
				}
			}
			else
			{
				if (base.Owner.Velocity.magnitude < 0.05f)
				{
					this.m_hasUsedMomentum = true;
				}
				this.m_motionTimer = 0f;
			}
		}
		else
		{
			this.m_motionTimer = 0f;
			this.m_hasUsedMomentum = true;
		}
		base.Update();
	}

	// Token: 0x06007965 RID: 31077 RVA: 0x003093FC File Offset: 0x003075FC
	public void ForceTrigger(PlayerController target)
	{
		target.StartCoroutine(this.HandleDamageBoost(target));
	}

	// Token: 0x06007966 RID: 31078 RVA: 0x0030940C File Offset: 0x0030760C
	private IEnumerator HandleDamageBoost(PlayerController target)
	{
		this.EnableVFX(target);
		if (this.m_destroyVFXSemaphore < 0)
		{
			this.m_destroyVFXSemaphore = 0;
		}
		this.m_destroyVFXSemaphore++;
		if (this.m_destroyVFXSemaphore == 1)
		{
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
		}
		this.m_hasUsedMomentum = false;
		while (target.IsDodgeRolling)
		{
			yield return null;
		}
		float elapsed = 0f;
		target.ownerlessStatModifiers.Add(this.m_damageStat);
		target.stats.RecalculateStats(target, false, false);
		while (!this.m_hasUsedMomentum)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		target.ownerlessStatModifiers.Remove(this.m_damageStat);
		if (this.m_destroyVFXSemaphore == 1)
		{
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Fade_01", base.gameObject);
		}
		target.stats.RecalculateStats(target, false, false);
		this.m_destroyVFXSemaphore--;
		if (this.m_hasUsedMomentum)
		{
			this.m_destroyVFXSemaphore = 0;
		}
		this.DisableVFX(target);
		yield break;
	}

	// Token: 0x06007967 RID: 31079 RVA: 0x00309430 File Offset: 0x00307630
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		if (this.HasFlagContingentMomentum)
		{
			this.m_cachedFlag = GameStatsManager.Instance.GetFlag(this.RequiredFlag);
		}
		player.PostProcessThrownGun += this.PostProcessThrownGun;
		player.OnRollStarted += this.HandleRollStarted;
		player.OnRolledIntoEnemy += this.HandleRolledIntoEnemy;
		player.OnReceivedDamage += this.HandleReceivedDamage;
	}

	// Token: 0x06007968 RID: 31080 RVA: 0x003094BC File Offset: 0x003076BC
	public void UpdateCachedFlag()
	{
		if (this.HasFlagContingentMomentum)
		{
			this.m_cachedFlag = GameStatsManager.Instance.GetFlag(this.RequiredFlag);
		}
	}

	// Token: 0x06007969 RID: 31081 RVA: 0x003094E0 File Offset: 0x003076E0
	private void HandleRolledIntoEnemy(PlayerController arg1, AIActor arg2)
	{
		if (!this.m_hasUsedMomentum)
		{
			if (arg2.knockbackDoer)
			{
				arg2.knockbackDoer.ApplyKnockback(arg1.specRigidbody.Velocity.normalized, this.MomentumKnockback, false);
			}
			if (this.MomentumVFX)
			{
				GameObject gameObject = arg2.PlayEffectOnActor(this.MomentumVFX, Vector3.zero, false, true, false);
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				if (component)
				{
					component.HeightOffGround = 3.5f;
				}
			}
		}
		this.m_hasUsedMomentum = true;
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x00309574 File Offset: 0x00307774
	private void HandleReceivedDamage(PlayerController obj)
	{
		this.m_hasUsedMomentum = true;
		this.m_motionTimer = 0f;
		Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(obj.sprite);
		obj.healthHaver.UpdateCachedOutlineColor(outlineMaterial, Color.black);
	}

	// Token: 0x0600796B RID: 31083 RVA: 0x003095B0 File Offset: 0x003077B0
	private void HandleRollStarted(PlayerController arg1, Vector2 arg2)
	{
		this.m_motionTimer = 0f;
	}

	// Token: 0x0600796C RID: 31084 RVA: 0x003095C0 File Offset: 0x003077C0
	private void PostProcessThrownGun(Projectile thrownGunProjectile)
	{
		if (this.MakeThrownGunsExplode)
		{
			ExplosiveModifier explosiveModifier = thrownGunProjectile.gameObject.AddComponent<ExplosiveModifier>();
			explosiveModifier.doExplosion = true;
			explosiveModifier.explosionData = this.ThrownGunExplosionData;
			explosiveModifier.explosionData.damageToPlayer = 0f;
			if (this.ThrownGunExplosionData.useDefaultExplosion)
			{
				explosiveModifier.explosionData = new ExplosionData();
				explosiveModifier.explosionData.CopyFrom(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData);
				explosiveModifier.explosionData.damageToPlayer = 0f;
			}
		}
		if (this.MakeThrownGunsReturnLikeBoomerangs)
		{
			thrownGunProjectile.OnBecameDebrisGrounded = (Action<DebrisObject>)Delegate.Combine(thrownGunProjectile.OnBecameDebrisGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
		}
	}

	// Token: 0x0600796D RID: 31085 RVA: 0x00309680 File Offset: 0x00307880
	private void HandleReturnLikeBoomerang(DebrisObject obj)
	{
		obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleReturnLikeBoomerang));
		PickupMover pickupMover = obj.gameObject.AddComponent<PickupMover>();
		if (pickupMover.specRigidbody)
		{
			pickupMover.specRigidbody.CollideWithTileMap = false;
		}
		pickupMover.minRadius = 1f;
		pickupMover.moveIfRoomUnclear = true;
		pickupMover.stopPathingOnContact = true;
	}

	// Token: 0x0600796E RID: 31086 RVA: 0x003096F0 File Offset: 0x003078F0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<ThrownGunPassiveItem>().m_pickedUpThisRun = true;
		if (player)
		{
			player.ReceivesTouchDamage = true;
			player.PostProcessThrownGun -= this.PostProcessThrownGun;
			player.OnRollStarted -= this.HandleRollStarted;
			player.OnReceivedDamage -= this.HandleReceivedDamage;
			player.OnRolledIntoEnemy -= this.HandleRolledIntoEnemy;
		}
		return debrisObject;
	}

	// Token: 0x0600796F RID: 31087 RVA: 0x0030976C File Offset: 0x0030796C
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp && base.Owner)
		{
			base.Owner.ReceivesTouchDamage = true;
			base.Owner.PostProcessThrownGun -= this.PostProcessThrownGun;
			base.Owner.OnRollStarted -= this.HandleRollStarted;
			base.Owner.OnReceivedDamage -= this.HandleReceivedDamage;
			base.Owner.OnRolledIntoEnemy -= this.HandleRolledIntoEnemy;
		}
		base.OnDestroy();
	}

	// Token: 0x04007BD9 RID: 31705
	public bool MakeThrownGunsExplode;

	// Token: 0x04007BDA RID: 31706
	[ShowInInspectorIf("MakeThrownGunsExplode", false)]
	public ExplosionData ThrownGunExplosionData;

	// Token: 0x04007BDB RID: 31707
	public bool MakeThrownGunsReturnLikeBoomerangs;

	// Token: 0x04007BDC RID: 31708
	[Header("Momentum")]
	public bool HasFlagContingentMomentum;

	// Token: 0x04007BDD RID: 31709
	[LongEnum]
	public GungeonFlags RequiredFlag;

	// Token: 0x04007BDE RID: 31710
	public GameObject OverheadVFX;

	// Token: 0x04007BDF RID: 31711
	public float TimeInMotion = 5f;

	// Token: 0x04007BE0 RID: 31712
	public int AdditionalRollDamage = 100;

	// Token: 0x04007BE1 RID: 31713
	public float MomentumKnockback = 100f;

	// Token: 0x04007BE2 RID: 31714
	public GameObject MomentumVFX;

	// Token: 0x04007BE3 RID: 31715
	private GameObject m_instanceVFX;

	// Token: 0x04007BE4 RID: 31716
	private int m_destroyVFXSemaphore;

	// Token: 0x04007BE5 RID: 31717
	private StatModifier m_damageStat;

	// Token: 0x04007BE6 RID: 31718
	private bool m_cachedFlag;

	// Token: 0x04007BE7 RID: 31719
	private float m_motionTimer;

	// Token: 0x04007BE8 RID: 31720
	private bool m_hasUsedMomentum;
}
