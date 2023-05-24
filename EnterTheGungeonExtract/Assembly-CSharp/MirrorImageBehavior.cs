using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D32 RID: 3378
public class MirrorImageBehavior : BasicAttackBehavior
{
	// Token: 0x06004759 RID: 18265 RVA: 0x00175774 File Offset: 0x00173974
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x0017577C File Offset: 0x0017397C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
		for (int i = this.m_allImages.Count - 1; i >= 0; i--)
		{
			if (!this.m_allImages[i] || !this.m_allImages[i].healthHaver || this.m_allImages[i].healthHaver.IsDead)
			{
				this.m_allImages.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x00175814 File Offset: 0x00173A14
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_enemyPrefab = EnemyDatabase.GetOrLoadByGuid(this.m_aiActor.EnemyGuid);
		this.m_aiAnimator.PlayUntilFinished(this.Anim, true, null, -1f, false);
		if (this.AnimRequiresTransparency)
		{
			this.m_cachedShader = this.m_aiActor.renderer.material.shader;
			this.m_aiActor.sprite.usesOverrideMaterial = true;
			this.m_aiActor.SetOutlines(false);
			this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
		}
		this.m_aiActor.ClearPath();
		this.m_timer = this.SpawnDelay;
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "MirrorImageBehavior");
		}
		this.m_aiActor.IsGone = true;
		this.m_aiActor.specRigidbody.CollideWithOthers = false;
		this.m_actorsToSplit.Clear();
		this.m_actorsToSplit.Add(this.m_aiActor);
		this.m_state = MirrorImageBehavior.State.Summoning;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x0017596C File Offset: 0x00173B6C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == MirrorImageBehavior.State.Summoning)
		{
			if (this.m_timer <= 0f)
			{
				int num = Mathf.Min(this.NumImages, this.MaxImages - this.m_allImages.Count);
				for (int i = 0; i < num; i++)
				{
					AIActor aiactor = AIActor.Spawn(this.m_enemyPrefab, this.m_aiActor.specRigidbody.UnitBottomLeft, this.m_aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Spawn, true);
					aiactor.transform.position = this.m_aiActor.transform.position;
					aiactor.specRigidbody.Reinitialize();
					aiactor.IsGone = true;
					aiactor.specRigidbody.CollideWithOthers = false;
					if (!string.IsNullOrEmpty(this.MirrorDeathAnim))
					{
						aiactor.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "death").anim.Prefix = this.MirrorDeathAnim;
					}
					aiactor.PreventBlackPhantom = true;
					if (aiactor.IsBlackPhantom)
					{
						aiactor.UnbecomeBlackPhantom();
					}
					this.m_actorsToSplit.Add(aiactor);
					this.m_allImages.Add(aiactor);
					aiactor.aiAnimator.healthHaver.SetHealthMaximum(this.MirrorHealth * AIActor.BaseLevelHealthModifier, null, false);
					MirrorImageController mirrorImageController = aiactor.gameObject.AddComponent<MirrorImageController>();
					mirrorImageController.SetHost(this.m_aiActor);
					for (int j = 0; j < this.MirroredAnims.Length; j++)
					{
						mirrorImageController.MirrorAnimations.Add(this.MirroredAnims[j]);
					}
					if (this.AnimRequiresTransparency)
					{
						aiactor.sprite.usesOverrideMaterial = true;
						aiactor.procedurallyOutlined = false;
						aiactor.SetOutlines(false);
						aiactor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
					}
				}
				this.m_startAngle = UnityEngine.Random.Range(0f, 360f);
				this.m_state = MirrorImageBehavior.State.Splitting;
				this.m_timer = this.SplitDelay;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == MirrorImageBehavior.State.Splitting)
		{
			float num2 = 360f / (float)this.m_actorsToSplit.Count;
			for (int k = 0; k < this.m_actorsToSplit.Count; k++)
			{
				this.m_actorsToSplit[k].BehaviorOverridesVelocity = true;
				this.m_actorsToSplit[k].BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startAngle + num2 * (float)k, this.SplitDistance / this.SplitDelay);
			}
			if (this.m_timer <= 0f)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x00175C1C File Offset: 0x00173E1C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.AnimRequiresTransparency && this.m_cachedShader)
		{
			for (int i = 0; i < this.m_actorsToSplit.Count; i++)
			{
				AIActor aiactor = this.m_actorsToSplit[i];
				if (aiactor)
				{
					aiactor.sprite.usesOverrideMaterial = false;
					aiactor.procedurallyOutlined = true;
					aiactor.SetOutlines(true);
					aiactor.renderer.material.shader = this.m_cachedShader;
				}
			}
			this.m_cachedShader = null;
		}
		if (!string.IsNullOrEmpty(this.Anim))
		{
			this.m_aiAnimator.EndAnimationIf(this.Anim);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "MirrorImageBehavior");
		}
		for (int j = 0; j < this.m_actorsToSplit.Count; j++)
		{
			AIActor aiactor2 = this.m_actorsToSplit[j];
			if (aiactor2)
			{
				aiactor2.BehaviorOverridesVelocity = false;
				aiactor2.IsGone = false;
				aiactor2.specRigidbody.CollideWithOthers = true;
				if (aiactor2 != this.m_aiActor)
				{
					aiactor2.PreventBlackPhantom = false;
					if (this.m_aiActor.IsBlackPhantom)
					{
						aiactor2.BecomeBlackPhantom();
					}
				}
			}
		}
		this.m_actorsToSplit.Clear();
		this.m_state = MirrorImageBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x00175DB8 File Offset: 0x00173FB8
	public override bool IsReady()
	{
		return (this.MaxImages <= 0 || this.m_allImages.Count < this.MaxImages) && base.IsReady();
	}

	// Token: 0x04003A43 RID: 14915
	public int NumImages = 2;

	// Token: 0x04003A44 RID: 14916
	public int MaxImages = 5;

	// Token: 0x04003A45 RID: 14917
	public float MirrorHealth = 15f;

	// Token: 0x04003A46 RID: 14918
	public float SpawnDelay = 0.5f;

	// Token: 0x04003A47 RID: 14919
	public float SplitDelay = 1f;

	// Token: 0x04003A48 RID: 14920
	public float SplitDistance = 1f;

	// Token: 0x04003A49 RID: 14921
	[InspectorCategory("Visuals")]
	public string Anim;

	// Token: 0x04003A4A RID: 14922
	[InspectorCategory("Visuals")]
	public bool AnimRequiresTransparency;

	// Token: 0x04003A4B RID: 14923
	[InspectorCategory("Visuals")]
	public string MirrorDeathAnim;

	// Token: 0x04003A4C RID: 14924
	[InspectorCategory("Visuals")]
	public string[] MirroredAnims;

	// Token: 0x04003A4D RID: 14925
	private MirrorImageBehavior.State m_state;

	// Token: 0x04003A4E RID: 14926
	private Shader m_cachedShader;

	// Token: 0x04003A4F RID: 14927
	private AIActor m_enemyPrefab;

	// Token: 0x04003A50 RID: 14928
	private float m_timer;

	// Token: 0x04003A51 RID: 14929
	private float m_startAngle;

	// Token: 0x04003A52 RID: 14930
	private List<AIActor> m_actorsToSplit = new List<AIActor>();

	// Token: 0x04003A53 RID: 14931
	private List<AIActor> m_allImages = new List<AIActor>();

	// Token: 0x02000D33 RID: 3379
	private enum State
	{
		// Token: 0x04003A56 RID: 14934
		Idle,
		// Token: 0x04003A57 RID: 14935
		Summoning,
		// Token: 0x04003A58 RID: 14936
		Splitting
	}
}
