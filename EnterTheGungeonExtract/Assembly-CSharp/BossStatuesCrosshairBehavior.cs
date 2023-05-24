using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D94 RID: 3476
[InspectorDropdownName("Bosses/BossStatues/CrosshairBehavior")]
public class BossStatuesCrosshairBehavior : BossStatuesPatternBehavior
{
	// Token: 0x0600499D RID: 18845 RVA: 0x0018969C File Offset: 0x0018789C
	public override void Start()
	{
		base.Start();
		this.m_cachedStatueAngle = 0.5f * (360f / (float)this.m_statuesController.allStatues.Count);
		if (TurboModeController.IsActive)
		{
			this.InitialJumpDelay /= TurboModeController.sEnemyBulletSpeedMultiplier;
			this.SequentialJumpDelays /= TurboModeController.sEnemyBulletSpeedMultiplier;
		}
	}

	// Token: 0x0600499E RID: 18846 RVA: 0x00189700 File Offset: 0x00187900
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			if (!this.m_hasStarted)
			{
				if (!this.m_hasPlayedAttackVfx)
				{
					float num = this.m_statuesController.attackHopTime - this.m_timeElapsed;
					if (num < this.AttackVfxPreTimer)
					{
						this.m_statuesController.GetComponent<VfxController>().PlayVfx(this.AttackVfx, null, null);
						this.m_hasPlayedAttackVfx = true;
					}
				}
				if (this.m_timeElapsed > 0.1f)
				{
					base.SetActiveState(BossStatueController.StatueState.StandStill);
					if (base.AreAllGrounded())
					{
						this.m_hasStarted = true;
						this.ShootBulletScript();
						this.m_hasPlayedAttackVfx = false;
						this.m_isGrounded = true;
						this.m_jumpTimer = this.InitialJumpDelay;
					}
				}
			}
			else
			{
				this.m_jumpTimer -= this.m_deltaTime;
				if (!this.m_hasPlayedAttackVfx)
				{
					float jumpTimer = this.m_jumpTimer;
					if (jumpTimer < this.AttackVfxPreTimer)
					{
						this.m_statuesController.GetComponent<VfxController>().PlayVfx(this.AttackVfx, null, null);
						this.m_hasPlayedAttackVfx = true;
					}
				}
				if (this.m_isGrounded)
				{
					if (this.m_jumpTimer <= this.m_statuesController.attackHopTime)
					{
						float num2 = -1f;
						for (int i = 0; i < this.m_activeStatueCount; i++)
						{
							if (this.m_activeStatues[i] && this.m_activeStatues[i].healthHaver.IsAlive)
							{
								this.m_activeStatues[i].QueuedBulletScript.Add(null);
								this.m_activeStatues[i].State = BossStatueController.StatueState.HopToTarget;
								num2 = Math.Max(this.m_activeStatues[i].DistancetoTarget, num2);
							}
						}
						if (num2 > 0f)
						{
							this.m_statuesController.OverrideMoveSpeed = new float?(Mathf.Max(this.m_statuesController.moveSpeed, 1.5f * num2 / this.m_statuesController.attackHopTime));
						}
						this.m_jumpTimer += this.SequentialJumpDelays;
						this.m_isGrounded = false;
					}
				}
				else
				{
					this.m_isGrounded = true;
					for (int j = 0; j < this.m_activeStatueCount; j++)
					{
						if (this.m_activeStatues[j] && this.m_activeStatues[j].healthHaver.IsAlive)
						{
							this.m_activeStatues[j].State = BossStatueController.StatueState.StandStill;
							this.m_isGrounded &= this.m_activeStatues[j].IsGrounded;
						}
					}
					if (this.m_isGrounded)
					{
						this.m_hasPlayedAttackVfx = false;
						this.m_statuesController.OverrideMoveSpeed = null;
					}
				}
			}
		}
		return base.ContinuousUpdate();
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x001899F8 File Offset: 0x00187BF8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_cachedStatueAngle = BraveMathCollege.ClampAngle360(this.m_statueAngles[0]);
	}

	// Token: 0x060049A0 RID: 18848 RVA: 0x00189A14 File Offset: 0x00187C14
	protected override void InitPositions()
	{
		this.m_statueAngles = new float[this.m_activeStatueCount];
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_statueAngles[i] = this.m_cachedStatueAngle + (float)i * (360f / (float)this.m_activeStatueCount);
		}
		Vector2[] array = new Vector2[this.m_activeStatueCount];
		for (int j = 0; j < this.m_activeStatueCount; j++)
		{
			array[j] = this.GetTargetPoint(this.m_statueAngles[j]);
		}
		base.ReorderStatues(array);
		for (int k = 0; k < array.Length; k++)
		{
			this.m_activeStatues[k].Target = new Vector2?(this.GetTargetPoint(this.m_statueAngles[k]));
		}
		this.m_hasStarted = false;
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x00189AEC File Offset: 0x00187CEC
	protected override void UpdatePositions()
	{
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x00189AF0 File Offset: 0x00187CF0
	protected override bool IsFinished()
	{
		return this.m_hasStarted && this.m_bulletSource.IsEnded;
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x00189B0C File Offset: 0x00187D0C
	protected override void OnStatueDeath()
	{
		if (this.m_bulletSource)
		{
			this.m_statuesController.ClearBullets(this.m_bulletSource.transform.position);
			UnityEngine.Object.Destroy(this.m_bulletSource);
			this.m_bulletSource = null;
			AkSoundEngine.PostEvent("Stop_ENM_statue_ring_01", this.m_statuesController.bulletBank.gameObject);
		}
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x00189B78 File Offset: 0x00187D78
	protected override void BeginState(BossStatuesPatternBehavior.PatternState state)
	{
		base.BeginState(state);
		if (state == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			this.m_hasStarted = false;
			this.m_hasPlayedAttackVfx = false;
			for (int i = 0; i < this.m_activeStatueCount; i++)
			{
				BossStatueController bossStatueController = this.m_activeStatues[i];
				if (bossStatueController && bossStatueController.healthHaver.IsAlive)
				{
					bossStatueController.knockbackDoer.SetImmobile(true, "CrosshairBehavior");
					bossStatueController.healthHaver.AllDamageMultiplier *= 0.5f;
					bossStatueController.QueuedBulletScript.Add(null);
					bossStatueController.State = BossStatueController.StatueState.HopToTarget;
					bossStatueController.SuppressShootVfx = true;
				}
			}
		}
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x00189C24 File Offset: 0x00187E24
	protected override void EndState(BossStatuesPatternBehavior.PatternState state)
	{
		if (state != BossStatuesPatternBehavior.PatternState.MovingToStartingPosition)
		{
			if (state == BossStatuesPatternBehavior.PatternState.InProgress)
			{
				if (this.OverrideMoveSpeed > 0f)
				{
					this.m_statuesController.OverrideMoveSpeed = null;
				}
				for (int i = 0; i < this.m_activeStatueCount; i++)
				{
					BossStatueController bossStatueController = this.m_activeStatues[i];
					if (bossStatueController && bossStatueController.healthHaver.IsAlive)
					{
						bossStatueController.knockbackDoer.SetImmobile(false, "CrosshairBehavior");
						bossStatueController.healthHaver.AllDamageMultiplier *= 2f;
						bossStatueController.SuppressShootVfx = true;
					}
				}
			}
		}
		else
		{
			this.m_statuesController.IsTransitioning = false;
		}
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x00189CEC File Offset: 0x00187EEC
	private Vector2 GetTargetPoint(float angle)
	{
		return this.m_statuesController.PatternCenter + BraveMathCollege.DegreesToVector(angle, this.CircleRadius);
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x00189D0C File Offset: 0x00187F0C
	private void ShootBulletScript()
	{
		if (!this.m_bulletSource)
		{
			Transform transform = new GameObject("crazy shoot point").transform;
			transform.position = this.m_statuesController.PatternCenter;
			this.m_bulletSource = transform.gameObject.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_statuesController.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x04003E05 RID: 15877
	public float CircleRadius;

	// Token: 0x04003E06 RID: 15878
	public float InitialJumpDelay;

	// Token: 0x04003E07 RID: 15879
	public float SequentialJumpDelays;

	// Token: 0x04003E08 RID: 15880
	public string AttackVfx;

	// Token: 0x04003E09 RID: 15881
	public float AttackVfxPreTimer;

	// Token: 0x04003E0A RID: 15882
	public BulletScriptSelector BulletScript;

	// Token: 0x04003E0B RID: 15883
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003E0C RID: 15884
	private float[] m_statueAngles;

	// Token: 0x04003E0D RID: 15885
	private float m_cachedStatueAngle;

	// Token: 0x04003E0E RID: 15886
	private float m_jumpTimer;

	// Token: 0x04003E0F RID: 15887
	private bool m_hasStarted;

	// Token: 0x04003E10 RID: 15888
	private bool m_isGrounded;

	// Token: 0x04003E11 RID: 15889
	private bool m_hasPlayedAttackVfx;
}
