using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D25 RID: 3365
public class DisplaceBehavior : BasicAttackBehavior
{
	// Token: 0x0600470B RID: 18187 RVA: 0x00173B10 File Offset: 0x00171D10
	public override void Start()
	{
		base.Start();
		this.m_aiAnimator.ChildAnimator.renderer.enabled = false;
		this.m_limbControllers = this.m_aiActor.GetComponentsInChildren<BulletLimbController>();
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x00173B40 File Offset: 0x00171D40
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiAnimator.ChildAnimator.renderer.enabled = false;
		base.DecrementTimer(ref this.m_timer, false);
		if (this.m_image && this.m_image.healthHaver.IsDead)
		{
			this.m_image = null;
			this.UpdateCooldowns();
		}
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x00173BA8 File Offset: 0x00171DA8
	public override BehaviorResult Update()
	{
		if (!this.m_aiActor.GetComponent<DisplacedImageController>() && !this.m_hasInstantSpawned)
		{
			this.SpawnImage();
			this.m_hasInstantSpawned = true;
		}
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_aiAnimator.PlayUntilFinished(this.Anim, true, null, -1f, false);
		this.m_aiActor.ClearPath();
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "DisplaceBehavior");
		}
		for (int i = 0; i < this.m_limbControllers.Length; i++)
		{
			this.m_limbControllers[i].enabled = true;
			this.m_limbControllers[i].HideBullets = false;
		}
		this.m_state = DisplaceBehavior.State.Summoning;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x00173CA4 File Offset: 0x00171EA4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == DisplaceBehavior.State.Summoning && !this.m_aiAnimator.IsPlaying(this.Anim))
		{
			this.SpawnImage();
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x00173CD4 File Offset: 0x00171ED4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.Anim))
		{
			this.m_aiAnimator.EndAnimationIf(this.Anim);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "DisplaceBehavior");
		}
		for (int i = 0; i < this.m_limbControllers.Length; i++)
		{
			this.m_limbControllers[i].enabled = false;
			this.m_limbControllers[i].HideBullets = true;
		}
		this.m_state = DisplaceBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00173D90 File Offset: 0x00171F90
	public override bool IsReady()
	{
		return (!this.m_image || !this.m_image.healthHaver.IsAlive) && base.IsReady();
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00173DC0 File Offset: 0x00171FC0
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x00173DC4 File Offset: 0x00171FC4
	private void SpawnImage()
	{
		if (this.m_behaviorSpeculator && this.m_behaviorSpeculator.MovementBehaviors.Count == 0)
		{
			return;
		}
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.m_aiActor.EnemyGuid);
		this.m_image = AIActor.Spawn(orLoadByGuid, this.m_aiActor.specRigidbody.UnitBottomLeft, this.m_aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Spawn, true);
		this.m_image.transform.position = this.m_aiActor.transform.position;
		this.m_image.specRigidbody.Reinitialize();
		this.m_image.aiAnimator.healthHaver.SetHealthMaximum(this.ImageHealthMultiplier * this.m_aiActor.healthHaver.GetMaxHealth(), null, false);
		DisplacedImageController displacedImageController = this.m_image.gameObject.AddComponent<DisplacedImageController>();
		displacedImageController.Init();
		displacedImageController.SetHost(this.m_aiActor);
		if (this.m_behaviorSpeculator && this.m_behaviorSpeculator.MovementBehaviors != null && this.m_behaviorSpeculator.MovementBehaviors.Count > 0)
		{
			FleeTargetBehavior fleeTargetBehavior = this.m_behaviorSpeculator.MovementBehaviors[0] as FleeTargetBehavior;
			if (fleeTargetBehavior != null)
			{
				fleeTargetBehavior.ForceRun = true;
			}
		}
		if (!this.m_hasInstantSpawned)
		{
			this.m_image.behaviorSpeculator.GlobalCooldown = this.InitialImageAttackDelay;
		}
	}

	// Token: 0x040039F0 RID: 14832
	public float ImageHealthMultiplier = 1f;

	// Token: 0x040039F1 RID: 14833
	public float InitialImageAttackDelay = 0.5f;

	// Token: 0x040039F2 RID: 14834
	[InspectorCategory("Visuals")]
	public string Anim;

	// Token: 0x040039F3 RID: 14835
	private DisplaceBehavior.State m_state;

	// Token: 0x040039F4 RID: 14836
	private Shader m_cachedShader;

	// Token: 0x040039F5 RID: 14837
	private float m_timer;

	// Token: 0x040039F6 RID: 14838
	private bool m_hasInstantSpawned;

	// Token: 0x040039F7 RID: 14839
	private BulletLimbController[] m_limbControllers;

	// Token: 0x040039F8 RID: 14840
	private AIActor m_image;

	// Token: 0x02000D26 RID: 3366
	private enum State
	{
		// Token: 0x040039FA RID: 14842
		Idle,
		// Token: 0x040039FB RID: 14843
		Summoning
	}
}
