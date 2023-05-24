using System;
using UnityEngine;

// Token: 0x02000CE1 RID: 3297
public class TowerBossBatteryController : DungeonPlaceableBehaviour
{
	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x060045FE RID: 17918 RVA: 0x0016C554 File Offset: 0x0016A754
	// (set) Token: 0x060045FF RID: 17919 RVA: 0x0016C564 File Offset: 0x0016A764
	public bool IsVulnerable
	{
		get
		{
			return base.healthHaver.IsVulnerable;
		}
		set
		{
			if (this.m_sprite == null)
			{
				this.m_sprite = base.GetComponentInChildren<tk2dSprite>();
			}
			base.healthHaver.IsVulnerable = value;
			if (value)
			{
				this.m_sprite.renderer.enabled = true;
			}
			else
			{
				this.m_sprite.renderer.enabled = false;
			}
		}
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x0016C5C8 File Offset: 0x0016A7C8
	private void Start()
	{
		this.m_sprite = base.GetComponentInChildren<tk2dSprite>();
		base.healthHaver.IsVulnerable = false;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.OnDamaged += this.Damaged;
		base.healthHaver.OnDeath += this.Die;
	}

	// Token: 0x06004601 RID: 17921 RVA: 0x0016C628 File Offset: 0x0016A828
	private void Damaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
	}

	// Token: 0x06004602 RID: 17922 RVA: 0x0016C62C File Offset: 0x0016A82C
	private void Die(Vector2 finalDamageDirection)
	{
		this.linkedIris.Open();
		base.healthHaver.FullHeal();
		base.healthHaver.IsVulnerable = false;
	}

	// Token: 0x06004603 RID: 17923 RVA: 0x0016C650 File Offset: 0x0016A850
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04003885 RID: 14469
	public TowerBossController tower;

	// Token: 0x04003886 RID: 14470
	public TowerBossIrisController linkedIris;

	// Token: 0x04003887 RID: 14471
	public float cycleTime = 5f;

	// Token: 0x04003888 RID: 14472
	private tk2dSprite m_sprite;
}
