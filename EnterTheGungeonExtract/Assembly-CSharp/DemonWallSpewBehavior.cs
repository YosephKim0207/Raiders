using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DA1 RID: 3489
[InspectorDropdownName("Bosses/DemonWall/SpewBehavior")]
public class DemonWallSpewBehavior : BasicAttackBehavior
{
	// Token: 0x060049E5 RID: 18917 RVA: 0x0018B648 File Offset: 0x00189848
	private bool ShowArcParams()
	{
		return this.goopType == DemonWallSpewBehavior.GoopType.Arc;
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x0018B654 File Offset: 0x00189854
	private bool ShowLineParams()
	{
		return this.goopType == DemonWallSpewBehavior.GoopType.Line;
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x0018B660 File Offset: 0x00189860
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x0018B694 File Offset: 0x00189894
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_goopTimer, false);
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x0018B6AC File Offset: 0x001898AC
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
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_aiAnimator.PlayUntilFinished(this.spewAnimation, false, null, -1f, false);
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x0018B714 File Offset: 0x00189914
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.igniteGoop && this.m_igniteTimer > 0f)
		{
			this.m_igniteTimer -= this.m_deltaTime;
			if (this.m_igniteTimer <= 0f)
			{
				DeadlyDeadlyGoopManager.IgniteGoopsCircle(this.goopPoint.transform.position + new Vector3(0f, -0.5f), 0.5f);
			}
		}
		if (this.m_goopTimer > 0f || this.m_aiAnimator.IsPlaying(this.spewAnimation))
		{
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_bulletSource != null && !this.m_bulletSource.IsEnded)
		{
			return ContinuousBehaviorResult.Continue;
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x0018B7E8 File Offset: 0x001899E8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_bulletSource && !this.m_bulletSource.IsEnded)
		{
			this.m_bulletSource.ForceStop();
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x0018B828 File Offset: 0x00189A28
	private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (!this.m_updateEveryFrame)
		{
			return;
		}
		if (clip.GetFrame(frame).eventInfo == "spew")
		{
			this.spewSprite.SetActive(true);
			this.spewSprite.GetComponent<SpriteAnimatorKiller>().Restart();
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToUse);
			if (this.goopType == DemonWallSpewBehavior.GoopType.Arc)
			{
				goopManagerForGoopType.TimedAddGoopArc(this.goopPoint.transform.position, this.goopConeLength, this.goopConeArc, -Vector2.up, this.goopDuration, this.goopCurve);
			}
			else
			{
				Vector2 vector = this.goopPoint.transform.position;
				goopManagerForGoopType.TimedAddGoopLine(vector, vector + new Vector2(0f, -this.goopLength), this.goopRadius, this.goopDuration);
			}
			this.m_goopTimer = this.goopDuration;
			this.m_igniteTimer = this.igniteDelay;
		}
		if (clip.GetFrame(frame).eventInfo == "fire")
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
			this.m_bulletSource.BulletScript = this.BulletScript;
			this.m_bulletSource.Initialize();
		}
	}

	// Token: 0x04003E58 RID: 15960
	public Transform goopPoint;

	// Token: 0x04003E59 RID: 15961
	public GoopDefinition goopToUse;

	// Token: 0x04003E5A RID: 15962
	public DemonWallSpewBehavior.GoopType goopType;

	// Token: 0x04003E5B RID: 15963
	[InspectorShowIf("ShowArcParams")]
	public float goopConeLength = 5f;

	// Token: 0x04003E5C RID: 15964
	[InspectorShowIf("ShowArcParams")]
	public float goopConeArc = 45f;

	// Token: 0x04003E5D RID: 15965
	[InspectorShowIf("ShowArcParams")]
	public AnimationCurve goopCurve;

	// Token: 0x04003E5E RID: 15966
	[InspectorShowIf("ShowLineParams")]
	public float goopLength = 5f;

	// Token: 0x04003E5F RID: 15967
	[InspectorShowIf("ShowLineParams")]
	public float goopRadius = 5f;

	// Token: 0x04003E60 RID: 15968
	public float goopDuration = 0.5f;

	// Token: 0x04003E61 RID: 15969
	public bool igniteGoop;

	// Token: 0x04003E62 RID: 15970
	[InspectorShowIf("igniteGoop")]
	public float igniteDelay = 1f;

	// Token: 0x04003E63 RID: 15971
	[InspectorCategory("Attack")]
	public GameObject ShootPoint;

	// Token: 0x04003E64 RID: 15972
	[InspectorCategory("Attack")]
	public BulletScriptSelector BulletScript;

	// Token: 0x04003E65 RID: 15973
	[InspectorCategory("Visuals")]
	public string spewAnimation;

	// Token: 0x04003E66 RID: 15974
	[InspectorCategory("Visuals")]
	public GameObject spewSprite;

	// Token: 0x04003E67 RID: 15975
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003E68 RID: 15976
	private float m_goopTimer;

	// Token: 0x04003E69 RID: 15977
	private float m_igniteTimer;

	// Token: 0x02000DA2 RID: 3490
	public enum GoopType
	{
		// Token: 0x04003E6B RID: 15979
		Arc,
		// Token: 0x04003E6C RID: 15980
		Line
	}
}
