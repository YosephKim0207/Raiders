using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013BF RID: 5055
public class KiPulseItem : PlayerItem
{
	// Token: 0x06007293 RID: 29331 RVA: 0x002D8D88 File Offset: 0x002D6F88
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.m_activated = 0;
		player.OnIsRolling += this.HandleRollFrame;
		player.OnDodgedBeam += this.HandleDodgedBeam;
	}

	// Token: 0x06007294 RID: 29332 RVA: 0x002D8DBC File Offset: 0x002D6FBC
	private void HandleDodgedBeam(BeamController beam, PlayerController player)
	{
		if (!base.IsOnCooldown && player.CurrentRollState == PlayerController.DodgeRollState.InAir)
		{
			base.StartCoroutine(this.Activate());
		}
	}

	// Token: 0x06007295 RID: 29333 RVA: 0x002D8DE4 File Offset: 0x002D6FE4
	private void HandleRollFrame(PlayerController obj)
	{
		if (!base.IsOnCooldown && this.m_activated <= 0 && obj.CurrentRollState == PlayerController.DodgeRollState.InAir)
		{
			Vector2 centerPosition = obj.CenterPosition;
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile = StaticReferenceManager.AllProjectiles[i];
				if (projectile && projectile.Owner is AIActor)
				{
					float sqrMagnitude = (projectile.transform.position.XY() - centerPosition).sqrMagnitude;
					if (sqrMagnitude < this.DetectionRadius)
					{
						base.StartCoroutine(this.Activate());
						break;
					}
				}
			}
		}
	}

	// Token: 0x06007296 RID: 29334 RVA: 0x002D8E9C File Offset: 0x002D709C
	public override bool CanBeUsed(PlayerController user)
	{
		return base.CanBeUsed(user) && this.m_activated > 0 && !base.IsOnCooldown;
	}

	// Token: 0x06007297 RID: 29335 RVA: 0x002D8EC4 File Offset: 0x002D70C4
	private void HandleDodgedProjectile(Projectile obj)
	{
		if (!base.IsOnCooldown)
		{
			base.StartCoroutine(this.Activate());
		}
	}

	// Token: 0x06007298 RID: 29336 RVA: 0x002D8EE0 File Offset: 0x002D70E0
	private IEnumerator Activate()
	{
		if (this.PreTriggerPeriod > 0f)
		{
			yield return new WaitForSeconds(this.PreTriggerPeriod);
		}
		this.m_activated++;
		float modifiedTriggerPeriod = ((!(this.LastOwner != null) || !this.LastOwner.HasActiveBonusSynergy(CustomSynergyType.GUON_UPGRADE_WHITE, false)) ? this.TriggerPeriod : this.SynergyTriggerPeriod);
		yield return new WaitForSeconds(modifiedTriggerPeriod);
		this.m_activated--;
		this.m_activated = Mathf.Max(this.m_activated, 0);
		yield break;
	}

	// Token: 0x06007299 RID: 29337 RVA: 0x002D8EFC File Offset: 0x002D70FC
	private void LateUpdate()
	{
		bool flag = false;
		if (!this.m_pickedUp)
		{
			if (!base.spriteAnimator.IsPlaying(this.IdleAnimation))
			{
				base.spriteAnimator.Play(this.IdleAnimation);
			}
		}
		else if (base.IsOnCooldown)
		{
			if (!base.spriteAnimator.IsPlaying(this.CooldownAnimation))
			{
				base.spriteAnimator.Play(this.CooldownAnimation);
			}
		}
		else if (this.m_activated <= 0)
		{
			if (!base.spriteAnimator.IsPlaying(this.InactiveAnimation))
			{
				base.spriteAnimator.Play(this.InactiveAnimation);
			}
		}
		else
		{
			flag = true;
			if (!base.spriteAnimator.IsPlaying(this.ActiveAnimation))
			{
				base.spriteAnimator.Play(this.ActiveAnimation);
			}
		}
		if (flag && this.LastOwner)
		{
			if (!this.m_extantVFX)
			{
				this.m_extantVFX = this.LastOwner.PlayEffectOnActor(this.KiOverheadVFX, new Vector3(-0.0625f, 1.25f, 0f), true, false, false);
			}
		}
		else if (!flag && this.m_extantVFX && !this.m_extantVFX.GetComponent<tk2dSpriteAnimator>().Playing)
		{
			UnityEngine.Object.Destroy(this.m_extantVFX);
			this.m_extantVFX = null;
		}
	}

	// Token: 0x0600729A RID: 29338 RVA: 0x002D9070 File Offset: 0x002D7270
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		user.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
		if (this.m_extantVFX)
		{
			this.m_extantVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject(string.Empty, null);
		}
	}

	// Token: 0x0600729B RID: 29339 RVA: 0x002D90CC File Offset: 0x002D72CC
	protected override void AfterCooldownApplied(PlayerController user)
	{
		base.AfterCooldownApplied(user);
		if (user.HasActiveBonusSynergy(CustomSynergyType.GUON_UPGRADE_WHITE, false))
		{
			base.CurrentDamageCooldown /= 2f;
		}
	}

	// Token: 0x0600729C RID: 29340 RVA: 0x002D90F8 File Offset: 0x002D72F8
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		user.OnIsRolling -= this.HandleRollFrame;
		user.OnDodgedProjectile -= this.HandleDodgedProjectile;
		user.OnDodgedBeam -= this.HandleDodgedBeam;
	}

	// Token: 0x0600729D RID: 29341 RVA: 0x002D9138 File Offset: 0x002D7338
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.LastOwner)
		{
			this.LastOwner.OnIsRolling -= this.HandleRollFrame;
			this.LastOwner.OnDodgedProjectile -= this.HandleDodgedProjectile;
			this.LastOwner.OnDodgedBeam -= this.HandleDodgedBeam;
		}
	}

	// Token: 0x040073E8 RID: 29672
	public string IdleAnimation;

	// Token: 0x040073E9 RID: 29673
	public string CooldownAnimation;

	// Token: 0x040073EA RID: 29674
	public string InactiveAnimation;

	// Token: 0x040073EB RID: 29675
	public string ActiveAnimation;

	// Token: 0x040073EC RID: 29676
	public float DetectionRadius = 0.75f;

	// Token: 0x040073ED RID: 29677
	public float PreTriggerPeriod;

	// Token: 0x040073EE RID: 29678
	public float TriggerPeriod = 0.25f;

	// Token: 0x040073EF RID: 29679
	public float SynergyTriggerPeriod;

	// Token: 0x040073F0 RID: 29680
	public GameObject KiOverheadVFX;

	// Token: 0x040073F1 RID: 29681
	private GameObject m_extantVFX;

	// Token: 0x040073F2 RID: 29682
	private int m_activated;
}
