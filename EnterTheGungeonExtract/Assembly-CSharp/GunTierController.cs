using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001416 RID: 5142
public class GunTierController : MonoBehaviour, IGunInheritable
{
	// Token: 0x17001181 RID: 4481
	// (get) Token: 0x060074A9 RID: 29865 RVA: 0x002E7094 File Offset: 0x002E5294
	private int KillsToNextTier
	{
		get
		{
			int currentStrengthTier = this.m_gun.CurrentStrengthTier;
			if (currentStrengthTier >= this.TierThresholds.Length)
			{
				return int.MaxValue;
			}
			return this.TierThresholds[currentStrengthTier];
		}
	}

	// Token: 0x060074AA RID: 29866 RVA: 0x002E70CC File Offset: 0x002E52CC
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun.OnInitializedWithOwner, new Action<GameActor>(this.OnGunInitialized));
		Gun gun2 = this.m_gun;
		gun2.OnDropped = (Action)Delegate.Combine(gun2.OnDropped, new Action(this.OnGunDroppedOrDestroyed));
		if (this.m_gun.CurrentOwner != null)
		{
			this.OnGunInitialized(this.m_gun.CurrentOwner);
		}
	}

	// Token: 0x060074AB RID: 29867 RVA: 0x002E715C File Offset: 0x002E535C
	private void OnGunInitialized(GameActor obj)
	{
		if (this.m_playerOwner != null)
		{
			this.OnGunDroppedOrDestroyed();
		}
		if (obj == null)
		{
			return;
		}
		if (obj is PlayerController)
		{
			this.m_playerOwner = obj as PlayerController;
			this.m_playerOwner.OnKilledEnemy += this.OnEnemyKilled;
			this.m_playerOwner.healthHaver.OnDamaged += this.OnPlayerDamaged;
		}
	}

	// Token: 0x060074AC RID: 29868 RVA: 0x002E71D8 File Offset: 0x002E53D8
	private void PlayTierVFX(PlayerController p)
	{
		if (this.m_gun.CurrentStrengthTier >= 0 && this.m_gun.CurrentStrengthTier < this.TierVFX.Length)
		{
			p.PlayEffectOnActor(this.TierVFX[this.m_gun.CurrentStrengthTier], Vector3.up, true, true, false);
		}
	}

	// Token: 0x060074AD RID: 29869 RVA: 0x002E7230 File Offset: 0x002E5430
	private void OnEnemyKilled(PlayerController obj)
	{
		if (obj.CurrentGun == this.m_gun)
		{
			this.m_kills++;
			if (this.m_kills > this.KillsToNextTier)
			{
				this.m_gun.CurrentStrengthTier = Mathf.Clamp(this.m_gun.CurrentStrengthTier + 1, 0, this.MaxTier - 1);
				this.PlayTierVFX(obj);
			}
		}
	}

	// Token: 0x060074AE RID: 29870 RVA: 0x002E72A0 File Offset: 0x002E54A0
	private void OnDestroy()
	{
		this.OnGunDroppedOrDestroyed();
	}

	// Token: 0x060074AF RID: 29871 RVA: 0x002E72A8 File Offset: 0x002E54A8
	private void OnGunDroppedOrDestroyed()
	{
		if (this.m_playerOwner != null)
		{
			this.m_playerOwner.healthHaver.OnDamaged -= this.OnPlayerDamaged;
			this.m_playerOwner.OnKilledEnemy -= this.OnEnemyKilled;
			this.m_playerOwner = null;
		}
	}

	// Token: 0x060074B0 RID: 29872 RVA: 0x002E7300 File Offset: 0x002E5500
	private void OnPlayerDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		int num = Mathf.Clamp(this.m_gun.CurrentStrengthTier - this.TiersToDropOnDamage, 0, this.MaxTier - 1);
		if (this.m_gun.CurrentStrengthTier != num)
		{
			this.m_gun.CurrentStrengthTier = num;
			this.PlayTierVFX(this.m_gun.CurrentOwner as PlayerController);
		}
		if (this.m_gun.CurrentStrengthTier == 0)
		{
			this.m_kills = 0;
		}
		else
		{
			this.m_kills = this.TierThresholds[this.m_gun.CurrentStrengthTier - 1];
		}
	}

	// Token: 0x060074B1 RID: 29873 RVA: 0x002E7398 File Offset: 0x002E5598
	public void InheritData(Gun sourceGun)
	{
		GunTierController component = sourceGun.GetComponent<GunTierController>();
		if (component)
		{
			this.m_kills = component.m_kills;
		}
	}

	// Token: 0x060074B2 RID: 29874 RVA: 0x002E73C4 File Offset: 0x002E55C4
	public void MidGameSerialize(List<object> data, int dataIndex)
	{
		data.Add(this.m_kills);
	}

	// Token: 0x060074B3 RID: 29875 RVA: 0x002E73D8 File Offset: 0x002E55D8
	public void MidGameDeserialize(List<object> data, ref int dataIndex)
	{
		this.m_kills = (int)data[dataIndex];
		if (this.m_gun == null)
		{
			this.m_gun = base.GetComponent<Gun>();
		}
		while (this.m_kills > this.KillsToNextTier)
		{
			this.m_gun.CurrentStrengthTier = Mathf.Clamp(this.m_gun.CurrentStrengthTier + 1, 0, this.MaxTier - 1);
		}
		dataIndex++;
	}

	// Token: 0x04007682 RID: 30338
	public int[] TierThresholds;

	// Token: 0x04007683 RID: 30339
	public int TiersToDropOnDamage = 100;

	// Token: 0x04007684 RID: 30340
	public int MaxTier = 3;

	// Token: 0x04007685 RID: 30341
	public GameObject[] TierVFX;

	// Token: 0x04007686 RID: 30342
	private Gun m_gun;

	// Token: 0x04007687 RID: 30343
	private PlayerController m_playerOwner;

	// Token: 0x04007688 RID: 30344
	private int m_kills;
}
