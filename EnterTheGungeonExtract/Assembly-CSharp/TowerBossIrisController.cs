using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CE7 RID: 3303
public class TowerBossIrisController : BraveBehaviour
{
	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x0600461D RID: 17949 RVA: 0x0016CDEC File Offset: 0x0016AFEC
	public bool IsOpen
	{
		get
		{
			return base.healthHaver.IsVulnerable;
		}
	}

	// Token: 0x0600461E RID: 17950 RVA: 0x0016CDFC File Offset: 0x0016AFFC
	private void Start()
	{
		this.m_sprite = base.GetComponentInChildren<tk2dSprite>();
		this.m_sprite.IsPerpendicular = false;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.IsVulnerable = false;
		base.healthHaver.OnDamaged += this.Damaged;
		base.healthHaver.OnDeath += this.Die;
	}

	// Token: 0x0600461F RID: 17951 RVA: 0x0016CE68 File Offset: 0x0016B068
	private void Update()
	{
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x0016CE6C File Offset: 0x0016B06C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x0016CE74 File Offset: 0x0016B074
	public void Open()
	{
		base.healthHaver.IsVulnerable = true;
		base.spriteAnimator.Play("tower_boss_leftPanel_open");
		base.StartCoroutine(this.TimedClose());
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x0016CEA0 File Offset: 0x0016B0A0
	private IEnumerator TimedClose()
	{
		float elapsed = 0f;
		while (elapsed < this.openDuration)
		{
			if (!this.fuseAlive)
			{
				break;
			}
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.fuseAlive)
		{
			this.Close();
		}
		yield break;
	}

	// Token: 0x06004623 RID: 17955 RVA: 0x0016CEBC File Offset: 0x0016B0BC
	public void Close()
	{
		base.healthHaver.IsVulnerable = false;
		base.spriteAnimator.Play("tower_boss_rightPanel_open");
	}

	// Token: 0x06004624 RID: 17956 RVA: 0x0016CEDC File Offset: 0x0016B0DC
	private void Damaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
	}

	// Token: 0x06004625 RID: 17957 RVA: 0x0016CEE0 File Offset: 0x0016B0E0
	private void Die(Vector2 finalDamageDirection)
	{
		this.fuseAlive = false;
		if (this.tower.currentPhase == TowerBossController.TowerBossPhase.PHASE_ONE)
		{
			this.tower.NotifyFuseDestruction(this);
			base.healthHaver.FullHeal();
			base.healthHaver.IsVulnerable = false;
		}
		else
		{
			this.tower.NotifyFuseDestruction(this);
			base.healthHaver.IsVulnerable = false;
		}
	}

	// Token: 0x040038A9 RID: 14505
	public TowerBossController tower;

	// Token: 0x040038AA RID: 14506
	public bool fuseAlive = true;

	// Token: 0x040038AB RID: 14507
	public float openDuration = 10f;

	// Token: 0x040038AC RID: 14508
	private tk2dSprite m_sprite;
}
