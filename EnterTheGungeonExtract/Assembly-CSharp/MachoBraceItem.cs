using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013C4 RID: 5060
public class MachoBraceItem : PassiveItem
{
	// Token: 0x060072B9 RID: 29369 RVA: 0x002D9AFC File Offset: 0x002D7CFC
	private void Awake()
	{
		this.m_damageStat = new StatModifier();
		this.m_damageStat.statToBoost = PlayerStats.StatType.Damage;
		this.m_damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
		this.m_damageStat.amount = this.DamageMultiplier;
	}

	// Token: 0x060072BA RID: 29370 RVA: 0x002D9B34 File Offset: 0x002D7D34
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.OnRollStarted += this.HandleRollStarted;
		player.PostProcessProjectile += this.HandleProjectileFired;
		player.PostProcessBeamTick += this.HandleBeamTick;
		base.Pickup(player);
	}

	// Token: 0x060072BB RID: 29371 RVA: 0x002D9B8C File Offset: 0x002D7D8C
	private void HandleBeamTick(BeamController arg1, SpeculativeRigidbody arg2, float arg3)
	{
		if (!this.m_hasUsedShot)
		{
			this.m_beamTickElapsed += BraveTime.DeltaTime;
			if (this.m_beamTickElapsed > 1f)
			{
				this.m_hasUsedShot = true;
			}
		}
	}

	// Token: 0x060072BC RID: 29372 RVA: 0x002D9BC4 File Offset: 0x002D7DC4
	private void HandleProjectileFired(Projectile firedProjectile, float arg2)
	{
		if (this.m_destroyVFXSemaphore > 0)
		{
			firedProjectile.AdjustPlayerProjectileTint(new Color(1f, 0.9f, 0f), 50, 0f);
			if (!this.m_hasUsedShot)
			{
				this.m_hasUsedShot = true;
				if (base.Owner && this.DustUpVFX)
				{
					base.Owner.PlayEffectOnActor(this.DustUpVFX, new Vector3(0f, -0.625f, 0f), false, false, false);
					AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Trigger_01", base.gameObject);
				}
				if (base.Owner && this.BurstVFX)
				{
					base.Owner.PlayEffectOnActor(this.BurstVFX, new Vector3(0f, 0.375f, 0f), false, false, false);
				}
			}
		}
	}

	// Token: 0x060072BD RID: 29373 RVA: 0x002D9CB4 File Offset: 0x002D7EB4
	private void HandleRollStarted(PlayerController source, Vector2 arg2)
	{
		source.StartCoroutine(this.HandleDamageBoost(source));
	}

	// Token: 0x060072BE RID: 29374 RVA: 0x002D9CC4 File Offset: 0x002D7EC4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnRollStarted -= this.HandleRollStarted;
		return debrisObject;
	}

	// Token: 0x060072BF RID: 29375 RVA: 0x002D9CEC File Offset: 0x002D7EEC
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.OnRollStarted -= this.HandleRollStarted;
		}
		base.OnDestroy();
	}

	// Token: 0x060072C0 RID: 29376 RVA: 0x002D9D1C File Offset: 0x002D7F1C
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

	// Token: 0x060072C1 RID: 29377 RVA: 0x002D9DB4 File Offset: 0x002D7FB4
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

	// Token: 0x060072C2 RID: 29378 RVA: 0x002D9E34 File Offset: 0x002D8034
	public void ForceTrigger(PlayerController target)
	{
		target.StartCoroutine(this.HandleDamageBoost(target));
	}

	// Token: 0x060072C3 RID: 29379 RVA: 0x002D9E44 File Offset: 0x002D8044
	private IEnumerator HandleDamageBoost(PlayerController target)
	{
		this.EnableVFX(target);
		this.m_destroyVFXSemaphore++;
		if (this.m_destroyVFXSemaphore == 1)
		{
			AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Active_01", base.gameObject);
		}
		this.m_hasUsedShot = false;
		while (target.IsDodgeRolling)
		{
			yield return null;
		}
		this.m_beamTickElapsed = 0f;
		float elapsed = 0f;
		target.ownerlessStatModifiers.Add(this.m_damageStat);
		target.stats.RecalculateStats(target, false, false);
		while (elapsed < this.DamageBoostDuration && !this.m_hasUsedShot)
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
		if (this.m_hasUsedShot)
		{
			this.m_destroyVFXSemaphore = 0;
		}
		this.DisableVFX(target);
		yield break;
	}

	// Token: 0x04007410 RID: 29712
	public float DamageMultiplier = 1.25f;

	// Token: 0x04007411 RID: 29713
	public float DamageBoostDuration = 1.5f;

	// Token: 0x04007412 RID: 29714
	public GameObject DustUpVFX;

	// Token: 0x04007413 RID: 29715
	public GameObject BurstVFX;

	// Token: 0x04007414 RID: 29716
	public GameObject OverheadVFX;

	// Token: 0x04007415 RID: 29717
	private bool m_hasUsedShot;

	// Token: 0x04007416 RID: 29718
	private float m_beamTickElapsed;

	// Token: 0x04007417 RID: 29719
	private StatModifier m_damageStat;

	// Token: 0x04007418 RID: 29720
	private GameObject m_instanceVFX;

	// Token: 0x04007419 RID: 29721
	private int m_destroyVFXSemaphore;
}
