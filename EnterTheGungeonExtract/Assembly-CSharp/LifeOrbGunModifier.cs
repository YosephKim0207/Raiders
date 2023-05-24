using System;
using UnityEngine;

// Token: 0x02001430 RID: 5168
public class LifeOrbGunModifier : MonoBehaviour
{
	// Token: 0x0600754F RID: 30031 RVA: 0x002EB728 File Offset: 0x002E9928
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
	}

	// Token: 0x06007550 RID: 30032 RVA: 0x002EB760 File Offset: 0x002E9960
	private void HandleReloadPressed(PlayerController owner, Gun source, bool reloadSomething)
	{
		if (this.m_storedSoulDamage > 0f)
		{
			if (this.OnBurstGunVFX)
			{
				SpawnManager.SpawnVFX(this.OnBurstGunVFX, owner.CurrentGun.barrelOffset.position, Quaternion.identity);
			}
			this.m_isDealingBurstDamage = true;
			owner.CurrentRoom.ApplyActionToNearbyEnemies(owner.transform.position.XY(), 100f, new Action<AIActor, float>(this.ProcessEnemy));
			this.m_isDealingBurstDamage = false;
			this.ClearSoul(false);
		}
	}

	// Token: 0x06007551 RID: 30033 RVA: 0x002EB7F0 File Offset: 0x002E99F0
	private void OnDisable()
	{
		this.ClearSoul(true);
	}

	// Token: 0x06007552 RID: 30034 RVA: 0x002EB7FC File Offset: 0x002E99FC
	private void ClearSoul(bool disabling)
	{
		this.m_storedSoulDamage = 0f;
		this.m_gun.idleAnimation = string.Empty;
		if (!disabling)
		{
			this.m_gun.PlayIdleAnimation();
		}
		if (this.m_overheadVFXInstance)
		{
			SpawnManager.Despawn(this.m_overheadVFXInstance.gameObject);
			this.m_overheadVFXInstance = null;
		}
	}

	// Token: 0x06007553 RID: 30035 RVA: 0x002EB860 File Offset: 0x002E9A60
	private void ProcessEnemy(AIActor a, float distance)
	{
		if (a && a.IsNormalEnemy && a.healthHaver && !a.IsGone)
		{
			if (this.m_lastOwner)
			{
				a.healthHaver.ApplyDamage(this.m_storedSoulDamage * this.damageFraction, Vector2.zero, this.m_lastOwner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			else
			{
				a.healthHaver.ApplyDamage(this.m_storedSoulDamage * this.damageFraction, Vector2.zero, "projectile", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			if (this.OnBurstDamageVFX)
			{
				a.PlayEffectOnActor(this.OnBurstDamageVFX, Vector3.zero, true, false, false);
			}
		}
	}

	// Token: 0x06007554 RID: 30036 RVA: 0x002EB92C File Offset: 0x002E9B2C
	private void Update()
	{
		if (!this.m_connected && this.m_gun.CurrentOwner)
		{
			this.m_connected = true;
			this.m_lastOwner = this.m_gun.CurrentOwner as PlayerController;
			this.m_lastOwner.OnDealtDamageContext += this.HandlePlayerDealtDamage;
		}
		else if (this.m_connected && !this.m_gun.CurrentOwner)
		{
			this.m_connected = false;
			this.m_lastOwner.OnDealtDamageContext -= this.HandlePlayerDealtDamage;
		}
	}

	// Token: 0x06007555 RID: 30037 RVA: 0x002EB9D0 File Offset: 0x002E9BD0
	private void HandlePlayerDealtDamage(PlayerController source, float damage, bool fatal, HealthHaver target)
	{
		if (source.CurrentGun != this.m_gun)
		{
			return;
		}
		if (this.m_isDealingBurstDamage)
		{
			return;
		}
		if (this.m_lastTargetDamaged != target)
		{
			this.m_lastTargetDamaged = target;
			this.m_totalDamageDealtToLastTarget = 0f;
		}
		this.m_totalDamageDealtToLastTarget += damage;
		if (fatal)
		{
			this.m_storedSoulDamage = this.m_totalDamageDealtToLastTarget;
			this.m_lastTargetDamaged = null;
			this.m_totalDamageDealtToLastTarget = 0f;
			if (this.OverheadVFX && !this.m_overheadVFXInstance)
			{
				this.m_overheadVFXInstance = source.PlayEffectOnActor(this.OverheadVFX, Vector3.up, true, false, false);
				this.m_overheadVFXInstance.transform.localPosition = this.m_overheadVFXInstance.transform.localPosition.Quantize(0.0625f);
				this.m_gun.idleAnimation = "life_orb_full_idle";
				this.m_gun.PlayIdleAnimation();
			}
			if (this.OnKilledEnemyVFX && target && target.aiActor)
			{
				target.aiActor.PlayEffectOnActor(this.OnKilledEnemyVFX, new Vector3(0f, 0.5f, 0f), false, false, false);
			}
		}
	}

	// Token: 0x04007734 RID: 30516
	public float damageFraction = 1f;

	// Token: 0x04007735 RID: 30517
	public GameObject OverheadVFX;

	// Token: 0x04007736 RID: 30518
	public GameObject OnKilledEnemyVFX;

	// Token: 0x04007737 RID: 30519
	public GameObject OnBurstGunVFX;

	// Token: 0x04007738 RID: 30520
	public GameObject OnBurstDamageVFX;

	// Token: 0x04007739 RID: 30521
	private GameObject m_overheadVFXInstance;

	// Token: 0x0400773A RID: 30522
	private Gun m_gun;

	// Token: 0x0400773B RID: 30523
	private bool m_connected;

	// Token: 0x0400773C RID: 30524
	private PlayerController m_lastOwner;

	// Token: 0x0400773D RID: 30525
	private HealthHaver m_lastTargetDamaged;

	// Token: 0x0400773E RID: 30526
	private float m_totalDamageDealtToLastTarget;

	// Token: 0x0400773F RID: 30527
	private float m_storedSoulDamage;

	// Token: 0x04007740 RID: 30528
	private bool m_isDealingBurstDamage;
}
