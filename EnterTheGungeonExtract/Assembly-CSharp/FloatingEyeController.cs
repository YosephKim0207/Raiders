using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001026 RID: 4134
public class FloatingEyeController : BraveBehaviour
{
	// Token: 0x06005AA5 RID: 23205 RVA: 0x00229ECC File Offset: 0x002280CC
	public void Awake()
	{
		if (base.aiAnimator)
		{
			base.aiAnimator.OnSpawnCompleted += this.OnSpawnCompleted;
		}
		base.aiActor.PreventAutoKillOnBossDeath = true;
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x00229F04 File Offset: 0x00228104
	public void Start()
	{
		this.m_beholster = UnityEngine.Object.FindObjectOfType<BeholsterController>();
		if (this.m_beholster)
		{
			this.m_beholster.healthHaver.OnDamaged += this.OnBeholsterDamaged;
		}
	}

	// Token: 0x06005AA7 RID: 23207 RVA: 0x00229F40 File Offset: 0x00228140
	protected override void OnDestroy()
	{
		if (this.m_beholster)
		{
			this.m_beholster.healthHaver.OnDamaged -= this.OnBeholsterDamaged;
		}
		if (base.aiAnimator)
		{
			base.aiAnimator.OnSpawnCompleted -= this.OnSpawnCompleted;
		}
		base.OnDestroy();
	}

	// Token: 0x06005AA8 RID: 23208 RVA: 0x00229FA8 File Offset: 0x002281A8
	private void OnBeholsterDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (resultValue <= 0f)
		{
			this.m_beholsterKilled = true;
			this.StartCrying();
		}
	}

	// Token: 0x06005AA9 RID: 23209 RVA: 0x00229FC4 File Offset: 0x002281C4
	private void OnSpawnCompleted()
	{
		if (base.aiActor)
		{
			base.aiActor.PathableTiles |= CellTypes.PIT;
		}
		if (this.m_beholsterKilled || (this.m_beholster && this.m_beholster.healthHaver.IsDead))
		{
			this.StartCrying();
		}
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x0022A02C File Offset: 0x0022822C
	private void StartCrying()
	{
		base.aiActor.ClearPath();
		base.behaviorSpeculator.enabled = false;
		base.aiShooter.enabled = false;
		base.aiShooter.ToggleGunAndHandRenderers(false, "Cry");
		base.aiActor.IgnoreForRoomClear = true;
		base.aiAnimator.PlayUntilCancelled("cry", false, null, -1f, false);
	}

	// Token: 0x04005412 RID: 21522
	private BeholsterController m_beholster;

	// Token: 0x04005413 RID: 21523
	private bool m_beholsterKilled;
}
