using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020016FD RID: 5885
public class MachoBraceSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088C9 RID: 35017 RVA: 0x0038B2B8 File Offset: 0x003894B8
	private void Awake()
	{
		this.m_damageStat = new StatModifier();
		this.m_damageStat.statToBoost = PlayerStats.StatType.Damage;
		this.m_damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
		this.m_damageStat.amount = this.DamageMultiplier;
		this.m_item = base.GetComponent<PassiveItem>();
	}

	// Token: 0x060088CA RID: 35018 RVA: 0x0038B308 File Offset: 0x00389508
	private void Update()
	{
		if (Dungeon.IsGenerating || !PhysicsEngine.HasInstance)
		{
			return;
		}
		bool flag = this.m_item.Owner && this.m_item.Owner.HasActiveBonusSynergy(this.RequiredSynergy, false);
		if (flag && !this.m_initialized)
		{
			this.Initialize(this.m_item.Owner);
		}
		else if (this.m_initialized && !flag)
		{
			if (this.m_lastOwner)
			{
				this.m_lastOwner.PostProcessProjectile -= this.HandleProjectileFired;
				this.m_lastOwner.PostProcessBeamTick -= this.HandleBeamTick;
				PlayerController lastOwner = this.m_lastOwner;
				lastOwner.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(lastOwner.OnTableFlipped, new Action<FlippableCover>(this.HandleTableFlip));
			}
			this.m_initialized = false;
			this.m_lastOwner = null;
		}
		else if (this.m_initialized && flag)
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				this.m_hasUsedShot = true;
				return;
			}
			if (this.TriggersOnStandingStill && this.m_destroyVFXSemaphore <= 0 && this.m_item.Owner.Velocity.magnitude < 0.05f)
			{
				this.m_standStillTimer += BraveTime.DeltaTime;
				if (this.m_standStillTimer > this.StandStillTimer)
				{
					this.ForceTrigger(this.m_item.Owner);
					this.m_standStillTimer = 0f;
				}
			}
			if (this.TriggersOnAimRotation && this.m_item.Owner.CurrentGun)
			{
				float num = Vector2.SignedAngle(BraveMathCollege.DegreesToVector((this.m_item.Owner.unadjustedAimPoint.XY() - this.m_item.Owner.CenterPosition).ToAngle(), 1f), BraveMathCollege.DegreesToVector(this.m_lastGunAngle, 1f));
				num = Mathf.Clamp(num, -90f, 90f);
				if (Mathf.Abs(num) < 120f * BraveTime.DeltaTime)
				{
					this.m_zeroRotationTime += Time.deltaTime;
					if (this.m_zeroRotationTime < 0.0333f)
					{
						return;
					}
					num = 0f;
					this.m_cumulativeGunRotation = 0f;
				}
				else
				{
					this.m_zeroRotationTime = 0f;
				}
				this.m_lastGunAngle = (this.m_item.Owner.unadjustedAimPoint.XY() - this.m_item.Owner.CenterPosition).ToAngle();
				this.m_cumulativeGunRotation += num;
				if (this.m_cumulativeGunRotation > 360f)
				{
					this.m_cumulativeGunRotation = 0f;
					this.ForceTrigger(this.m_item.Owner);
				}
				else if (this.m_cumulativeGunRotation < -360f)
				{
					this.m_cumulativeGunRotation = 0f;
					this.ForceTrigger(this.m_item.Owner);
				}
			}
		}
	}

	// Token: 0x060088CB RID: 35019 RVA: 0x0038B62C File Offset: 0x0038982C
	public void Initialize(PlayerController player)
	{
		this.m_initialized = true;
		player.PostProcessProjectile += this.HandleProjectileFired;
		player.PostProcessBeamTick += this.HandleBeamTick;
		player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.HandleTableFlip));
		this.m_lastOwner = player;
		if (player.CurrentGun)
		{
			this.m_lastGunAngle = player.CurrentGun.CurrentAngle;
		}
	}

	// Token: 0x060088CC RID: 35020 RVA: 0x0038B6B0 File Offset: 0x003898B0
	private void HandleTableFlip(FlippableCover obj)
	{
		if (this.TriggersOnTableFlip && this.m_item.Owner)
		{
			this.ForceTrigger(this.m_item.Owner);
		}
	}

	// Token: 0x060088CD RID: 35021 RVA: 0x0038B6E4 File Offset: 0x003898E4
	private void HandleBeamTick(BeamController arg1, SpeculativeRigidbody arg2, float arg3)
	{
		if (!this.m_item.Owner)
		{
			return;
		}
		if (!this.m_hasUsedShot)
		{
			this.m_beamTickElapsed += BraveTime.DeltaTime;
			if (this.m_beamTickElapsed > 1f)
			{
				this.m_hasUsedShot = true;
			}
		}
	}

	// Token: 0x060088CE RID: 35022 RVA: 0x0038B73C File Offset: 0x0038993C
	private void HandleProjectileFired(Projectile firedProjectile, float arg2)
	{
		if (!this.m_item.Owner)
		{
			return;
		}
		if (this.m_destroyVFXSemaphore > 0)
		{
			firedProjectile.AdjustPlayerProjectileTint(new Color(1f, 0.9f, 0f), 50, 0f);
			if (!this.m_hasUsedShot)
			{
				this.m_hasUsedShot = true;
				if (this.m_item.Owner && this.DustUpVFX)
				{
					this.m_item.Owner.PlayEffectOnActor(this.DustUpVFX, new Vector3(0f, -0.625f, 0f), false, false, false);
					AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Trigger_01", base.gameObject);
				}
				if (this.m_item.Owner && this.BurstVFX)
				{
					this.m_item.Owner.PlayEffectOnActor(this.BurstVFX, new Vector3(0f, 0.375f, 0f), false, false, false);
				}
			}
		}
	}

	// Token: 0x060088CF RID: 35023 RVA: 0x0038B858 File Offset: 0x00389A58
	public void EnableVFX(PlayerController target)
	{
		if (this.m_destroyVFXSemaphore == 0)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 0f));
			}
			if (this.OverheadVFX && !this.m_instanceVFX)
			{
				this.m_instanceVFX = target.PlayEffectOnActor(this.OverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
			}
		}
	}

	// Token: 0x060088D0 RID: 35024 RVA: 0x0038B8F0 File Offset: 0x00389AF0
	public void DisableVFX(PlayerController target)
	{
		if (this.m_destroyVFXSemaphore == 0)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			}
			if (!this.m_hasUsedShot)
			{
			}
			if (this.m_instanceVFX)
			{
				SpawnManager.Despawn(this.m_instanceVFX);
				this.m_instanceVFX = null;
			}
		}
	}

	// Token: 0x060088D1 RID: 35025 RVA: 0x0038B970 File Offset: 0x00389B70
	public void ForceTrigger(PlayerController target)
	{
		target.StartCoroutine(this.HandleDamageBoost(target));
	}

	// Token: 0x060088D2 RID: 35026 RVA: 0x0038B980 File Offset: 0x00389B80
	private IEnumerator HandleDamageBoost(PlayerController target)
	{
		if (this.m_destroyVFXSemaphore > 5)
		{
			yield break;
		}
		this.EnableVFX(target);
		this.m_destroyVFXSemaphore++;
		if (this.m_destroyVFXSemaphore == 1)
		{
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
		}
		this.m_hasUsedShot = false;
		this.m_beamTickElapsed = 0f;
		float elapsed = 0f;
		if (target)
		{
			target.ownerlessStatModifiers.Add(this.m_damageStat);
			target.stats.RecalculateStats(target, false, false);
		}
		if (this.TriggersOnStandingStill)
		{
			while (target && target.specRigidbody.Velocity.magnitude < 0.05f && !this.m_hasUsedShot)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
		}
		else if (this.TriggersOnTableFlip || this.TriggersOnAimRotation)
		{
			while (target && elapsed < this.FlipDuration && !this.m_hasUsedShot)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
		}
		if (target)
		{
			target.ownerlessStatModifiers.Remove(this.m_damageStat);
		}
		if (this.m_destroyVFXSemaphore == 1)
		{
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Fade_01", base.gameObject);
		}
		if (target)
		{
			target.stats.RecalculateStats(target, false, false);
		}
		this.m_destroyVFXSemaphore--;
		if (this.m_hasUsedShot)
		{
			this.m_destroyVFXSemaphore = 0;
		}
		if (target)
		{
			this.DisableVFX(target);
		}
		yield break;
	}

	// Token: 0x04008E4A RID: 36426
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008E4B RID: 36427
	public float DamageMultiplier = 1.25f;

	// Token: 0x04008E4C RID: 36428
	public GameObject DustUpVFX;

	// Token: 0x04008E4D RID: 36429
	public GameObject BurstVFX;

	// Token: 0x04008E4E RID: 36430
	public GameObject OverheadVFX;

	// Token: 0x04008E4F RID: 36431
	public bool TriggersOnStandingStill;

	// Token: 0x04008E50 RID: 36432
	public float StandStillTimer = 3f;

	// Token: 0x04008E51 RID: 36433
	public bool TriggersOnTableFlip;

	// Token: 0x04008E52 RID: 36434
	public float FlipDuration = 3f;

	// Token: 0x04008E53 RID: 36435
	public bool TriggersOnAimRotation;

	// Token: 0x04008E54 RID: 36436
	private float m_lastGunAngle;

	// Token: 0x04008E55 RID: 36437
	private float m_cumulativeGunRotation;

	// Token: 0x04008E56 RID: 36438
	private float m_zeroRotationTime;

	// Token: 0x04008E57 RID: 36439
	private float m_standStillTimer;

	// Token: 0x04008E58 RID: 36440
	private PassiveItem m_item;

	// Token: 0x04008E59 RID: 36441
	private bool m_initialized;

	// Token: 0x04008E5A RID: 36442
	private PlayerController m_lastOwner;

	// Token: 0x04008E5B RID: 36443
	private bool m_hasUsedShot;

	// Token: 0x04008E5C RID: 36444
	private float m_beamTickElapsed;

	// Token: 0x04008E5D RID: 36445
	private StatModifier m_damageStat;

	// Token: 0x04008E5E RID: 36446
	private GameObject m_instanceVFX;

	// Token: 0x04008E5F RID: 36447
	private int m_destroyVFXSemaphore;
}
