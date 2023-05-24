using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D14 RID: 3348
public class BuffEnemiesBehavior : BasicAttackBehavior
{
	// Token: 0x0600469C RID: 18076 RVA: 0x0016F124 File Offset: 0x0016D324
	public override void Start()
	{
		base.Start();
		this.m_aiActor.IsBuffEnemy = true;
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x0016F138 File Offset: 0x0016D338
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_searchTimer, false);
	}

	// Token: 0x0600469E RID: 18078 RVA: 0x0016F150 File Offset: 0x0016D350
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (this.m_searchTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref BuffEnemiesBehavior.s_activeEnemies);
		BuffEnemiesBehavior.s_activeEnemies.Remove(this.m_aiActor);
		for (int i = BuffEnemiesBehavior.s_activeEnemies.Count - 1; i >= 0; i--)
		{
			if (!this.IsGoodBuffTarget(BuffEnemiesBehavior.s_activeEnemies[i]))
			{
				BuffEnemiesBehavior.s_activeEnemies.RemoveAt(i);
			}
		}
		if (BuffEnemiesBehavior.s_activeEnemies.Count == 0)
		{
			return BehaviorResult.Continue;
		}
		while ((float)this.m_buffedEnemies.Count < this.EnemiesToBuff && BuffEnemiesBehavior.s_activeEnemies.Count > 0)
		{
			int num = UnityEngine.Random.Range(0, BuffEnemiesBehavior.s_activeEnemies.Count);
			this.m_buffedEnemies.Add(BuffEnemiesBehavior.s_activeEnemies[num]);
			BuffEnemiesBehavior.s_activeEnemies.RemoveAt(num);
		}
		for (int j = 0; j < this.m_buffedEnemies.Count; j++)
		{
			this.BuffEnemy(this.m_buffedEnemies[j]);
		}
		this.m_searchTimer = this.SearchInterval;
		if (!string.IsNullOrEmpty(this.BuffAnimation))
		{
			this.m_aiAnimator.PlayUntilCancelled(this.BuffAnimation, true, null, -1f, false);
		}
		if (!string.IsNullOrEmpty(this.BuffVfx))
		{
			this.m_aiAnimator.PlayVfx(this.BuffVfx, null, null, null);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "BuffEnemiesBehavior");
		}
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x0016F348 File Offset: 0x0016D548
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		for (int i = 0; i < this.m_buffedEnemies.Count; i++)
		{
			AIActor aiactor = this.m_buffedEnemies[i];
			if (!aiactor || aiactor.healthHaver.IsDead)
			{
				this.m_buffedEnemies.RemoveAt(i);
				i--;
			}
		}
		if (this.m_searchTimer <= 0f)
		{
			this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref BuffEnemiesBehavior.s_activeEnemies);
			BuffEnemiesBehavior.s_activeEnemies.Remove(this.m_aiActor);
			for (int j = 0; j < this.m_buffedEnemies.Count; j++)
			{
				BuffEnemiesBehavior.s_activeEnemies.Remove(this.m_buffedEnemies[j]);
			}
			for (int k = BuffEnemiesBehavior.s_activeEnemies.Count - 1; k >= 0; k--)
			{
				if (!this.IsGoodBuffTarget(BuffEnemiesBehavior.s_activeEnemies[k]))
				{
					BuffEnemiesBehavior.s_activeEnemies.RemoveAt(k);
				}
			}
			if (BuffEnemiesBehavior.s_activeEnemies.Count > 0)
			{
				while ((float)this.m_buffedEnemies.Count < this.EnemiesToBuff && BuffEnemiesBehavior.s_activeEnemies.Count > 0)
				{
					int num = UnityEngine.Random.Range(0, BuffEnemiesBehavior.s_activeEnemies.Count);
					AIActor aiactor2 = BuffEnemiesBehavior.s_activeEnemies[num];
					BuffEnemiesBehavior.s_activeEnemies.RemoveAt(num);
					this.m_buffedEnemies.Add(aiactor2);
					this.BuffEnemy(aiactor2);
				}
			}
			this.m_searchTimer = this.SearchInterval;
		}
		return (this.m_buffedEnemies.Count <= 0) ? ContinuousBehaviorResult.Finished : ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060046A0 RID: 18080 RVA: 0x0016F4F4 File Offset: 0x0016D6F4
	public override void EndContinuousUpdate()
	{
		for (int i = 0; i < this.m_buffedEnemies.Count; i++)
		{
			this.UnbuffEnemy(this.m_buffedEnemies[i]);
		}
		this.m_buffedEnemies.Clear();
		if (!string.IsNullOrEmpty(this.BuffAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.BuffAnimation);
		}
		if (!string.IsNullOrEmpty(this.BuffVfx))
		{
			this.m_aiAnimator.StopVfx(this.BuffVfx);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "BuffEnemiesBehavior");
		}
		this.UpdateCooldowns();
	}

	// Token: 0x060046A1 RID: 18081 RVA: 0x0016F5C0 File Offset: 0x0016D7C0
	public override void OnActorPreDeath()
	{
		if (this.m_buffedEnemies.Count > 0)
		{
			for (int i = 0; i < this.m_buffedEnemies.Count; i++)
			{
				this.UnbuffEnemy(this.m_buffedEnemies[i]);
			}
		}
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x0016F60C File Offset: 0x0016D80C
	protected virtual void BuffEnemy(AIActor enemy)
	{
		if (!enemy)
		{
			return;
		}
		if (this.JamEnemies)
		{
			if (enemy.specRigidbody)
			{
				if (enemy.IsSignatureEnemy)
				{
					enemy.PlaySmallExplosionsStyleEffect(this.LargeJamEffect, 8, 0.025f);
				}
				else
				{
					enemy.PlayEffectOnActor(this.SmallJamEffect, Vector3.zero, true, false, true);
				}
			}
			enemy.BecomeBlackPhantom();
		}
		if (this.UsesBuffEffect)
		{
			enemy.ApplyEffect(this.buffEffect, 1f, null);
		}
	}

	// Token: 0x060046A3 RID: 18083 RVA: 0x0016F69C File Offset: 0x0016D89C
	protected virtual void UnbuffEnemy(AIActor enemy)
	{
		if (!enemy)
		{
			return;
		}
		if (this.JamEnemies)
		{
			enemy.UnbecomeBlackPhantom();
		}
		if (this.UsesBuffEffect)
		{
			enemy.RemoveEffect(this.buffEffect);
		}
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x0016F6D4 File Offset: 0x0016D8D4
	private bool IsGoodBuffTarget(AIActor enemy)
	{
		return enemy && !enemy.IsBuffEnemy && !enemy.IsHarmlessEnemy && (!enemy.healthHaver || (enemy.healthHaver.IsVulnerable && !enemy.healthHaver.PreventAllDamage)) && (!this.JamEnemies || !enemy.IsBlackPhantom);
	}

	// Token: 0x04003930 RID: 14640
	public float SearchInterval = 1f;

	// Token: 0x04003931 RID: 14641
	public float EnemiesToBuff = 1f;

	// Token: 0x04003932 RID: 14642
	public bool UsesBuffEffect = true;

	// Token: 0x04003933 RID: 14643
	public AIActorBuffEffect buffEffect;

	// Token: 0x04003934 RID: 14644
	public bool JamEnemies;

	// Token: 0x04003935 RID: 14645
	public GameObject SmallJamEffect;

	// Token: 0x04003936 RID: 14646
	public GameObject LargeJamEffect;

	// Token: 0x04003937 RID: 14647
	[InspectorCategory("Visuals")]
	public string BuffAnimation;

	// Token: 0x04003938 RID: 14648
	[InspectorCategory("Visuals")]
	public string BuffVfx;

	// Token: 0x04003939 RID: 14649
	private float m_searchTimer;

	// Token: 0x0400393A RID: 14650
	private List<AIActor> m_buffedEnemies = new List<AIActor>();

	// Token: 0x0400393B RID: 14651
	private static List<AIActor> s_activeEnemies = new List<AIActor>();
}
