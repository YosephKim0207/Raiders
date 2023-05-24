using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D5C RID: 3420
public class SpinAttackBehavior : BasicAttackBehavior
{
	// Token: 0x06004840 RID: 18496 RVA: 0x0017E2AC File Offset: 0x0017C4AC
	public override void Start()
	{
		base.Start();
		if (this.PreventConcurrentAttacks)
		{
			SpinAttackBehavior.ConcurrentAttackHappening = false;
		}
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x0017E2C8 File Offset: 0x0017C4C8
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
		this.m_state = SpinAttackBehavior.SpinState.Start;
		this.m_aiAnimator.PlayUntilFinished(this.spinBeginAnim, true, null, -1f, false);
		if (this.PreventConcurrentAttacks)
		{
			SpinAttackBehavior.ConcurrentAttackHappening = true;
		}
		this.m_vfxTimer = this.VfxMidDelay;
		this.m_aiActor.ClearPath();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x0017E344 File Offset: 0x0017C544
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == SpinAttackBehavior.SpinState.Start)
		{
			if (!this.m_aiAnimator.IsPlaying(this.spinBeginAnim))
			{
				this.m_state = SpinAttackBehavior.SpinState.Spin;
				this.m_aiAnimator.PlayUntilFinished(this.spinAnim, true, null, -1f, false);
				if (!string.IsNullOrEmpty(this.spinVfx))
				{
					this.m_aiAnimator.PlayVfx(this.spinVfx, null, null, null);
				}
				this.ShootBulletScript();
			}
		}
		else if (this.m_state == SpinAttackBehavior.SpinState.Spin)
		{
			if (this.SpawnMuzzleVfx)
			{
				this.m_vfxTimer -= this.m_deltaTime;
				if (this.VfxMidDelay > 0f && this.m_vfxTimer <= 0f)
				{
					VFXPool muzzleFlashEffects = this.m_aiActor.bulletBank.GetBullet("default").MuzzleFlashEffects;
					PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
					Vector2 unitBottomLeft = hitboxPixelCollider.UnitBottomLeft;
					Vector2 unitCenter = hitboxPixelCollider.UnitCenter;
					Vector2 unitTopRight = hitboxPixelCollider.UnitTopRight;
					while (this.m_vfxTimer <= 0f)
					{
						Vector2 vector = BraveUtility.RandomVector2(unitBottomLeft, unitTopRight, new Vector2(-1.5f, -1.5f));
						float num = (vector - unitCenter).ToAngle();
						muzzleFlashEffects.SpawnAtLocalPosition(vector - this.ShootPoint.transform.position.XY(), num, this.ShootPoint.transform, null, null, false, null, false);
						this.m_vfxTimer += this.VfxMidDelay;
					}
				}
			}
			if (this.m_bulletSource.IsEnded)
			{
				this.m_state = SpinAttackBehavior.SpinState.End;
				if (!string.IsNullOrEmpty(this.spinEndAnim))
				{
					this.m_aiAnimator.PlayUntilFinished(this.spinEndAnim, true, null, -1f, false);
				}
				else
				{
					this.m_aiAnimator.EndAnimationIf(this.spinAnim);
				}
				if (!string.IsNullOrEmpty(this.spinVfx))
				{
					this.m_aiAnimator.StopVfx(this.spinVfx);
				}
			}
		}
		else if (this.m_state == SpinAttackBehavior.SpinState.End && !this.m_aiAnimator.IsPlaying(this.spinEndAnim))
		{
			this.m_state = SpinAttackBehavior.SpinState.None;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x0017E5B4 File Offset: 0x0017C7B4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = SpinAttackBehavior.SpinState.None;
		if (this.m_bulletSource && !this.m_bulletSource.IsEnded)
		{
			this.m_bulletSource.ForceStop();
		}
		this.m_aiAnimator.EndAnimationIf(this.spinBeginAnim);
		this.m_aiAnimator.EndAnimationIf(this.spinAnim);
		this.m_aiAnimator.EndAnimationIf(this.spinEndAnim);
		this.m_aiAnimator.StopVfx(this.spinVfx);
		if (this.PreventConcurrentAttacks)
		{
			SpinAttackBehavior.ConcurrentAttackHappening = false;
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x0017E660 File Offset: 0x0017C860
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		if (this.PreventConcurrentAttacks)
		{
			SpinAttackBehavior.ConcurrentAttackHappening = false;
		}
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x0017E67C File Offset: 0x0017C87C
	public override bool IsReady()
	{
		return (!this.PreventConcurrentAttacks || !SpinAttackBehavior.ConcurrentAttackHappening) && base.IsReady();
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x0017E69C File Offset: 0x0017C89C
	private void ShootBulletScript()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x04003BD6 RID: 15318
	public static bool ConcurrentAttackHappening;

	// Token: 0x04003BD7 RID: 15319
	public GameObject ShootPoint;

	// Token: 0x04003BD8 RID: 15320
	public BulletScriptSelector BulletScript;

	// Token: 0x04003BD9 RID: 15321
	public bool PreventConcurrentAttacks;

	// Token: 0x04003BDA RID: 15322
	[InspectorCategory("Visuals")]
	public string spinBeginAnim = "spin_begin";

	// Token: 0x04003BDB RID: 15323
	[InspectorCategory("Visuals")]
	public string spinAnim = "spin";

	// Token: 0x04003BDC RID: 15324
	[InspectorCategory("Visuals")]
	public string spinEndAnim = "spin_end";

	// Token: 0x04003BDD RID: 15325
	[InspectorCategory("Visuals")]
	public bool SpawnMuzzleVfx;

	// Token: 0x04003BDE RID: 15326
	[InspectorShowIf("SpawnMuzzleVfx")]
	[InspectorCategory("Visuals")]
	public float VfxMidDelay = 0.1f;

	// Token: 0x04003BDF RID: 15327
	[InspectorCategory("Visuals")]
	public string spinVfx;

	// Token: 0x04003BE0 RID: 15328
	private SpinAttackBehavior.SpinState m_state;

	// Token: 0x04003BE1 RID: 15329
	private float m_vfxTimer;

	// Token: 0x04003BE2 RID: 15330
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000D5D RID: 3421
	private enum SpinState
	{
		// Token: 0x04003BE4 RID: 15332
		None,
		// Token: 0x04003BE5 RID: 15333
		Start,
		// Token: 0x04003BE6 RID: 15334
		Spin,
		// Token: 0x04003BE7 RID: 15335
		End
	}
}
