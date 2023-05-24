using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D97 RID: 3479
public abstract class BossStatuesPatternBehavior : BasicAttackBehavior
{
	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x060049B0 RID: 18864 RVA: 0x0018A1C8 File Offset: 0x001883C8
	// (set) Token: 0x060049B1 RID: 18865 RVA: 0x0018A1D0 File Offset: 0x001883D0
	protected BossStatuesPatternBehavior.PatternState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x0018A200 File Offset: 0x00188400
	public override void Start()
	{
		base.Start();
		this.m_statuesController = this.m_gameObject.GetComponent<BossStatuesController>();
		this.m_activeStatues = new List<BossStatueController>(this.m_statuesController.allStatues);
		this.UpdateNumStatuesArray();
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x0018A238 File Offset: 0x00188438
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_stateTimer, false);
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x0018A250 File Offset: 0x00188450
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
		this.m_updateEveryFrame = true;
		this.RefreshActiveStatues();
		this.InitPositions();
		if (this.attackType != null)
		{
			this.attackType.Start(this.m_activeStatues);
		}
		this.State = ((!this.waitForStartingPositions) ? BossStatuesPatternBehavior.PatternState.InProgress : BossStatuesPatternBehavior.PatternState.MovingToStartingPosition);
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x0018A2C4 File Offset: 0x001884C4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State != BossStatuesPatternBehavior.PatternState.Ending && this.AnyStatuesHaveDied())
		{
			for (int i = 0; i < this.m_activeStatueCount; i++)
			{
				BossStatueController bossStatueController = this.m_activeStatues[i];
				if (bossStatueController)
				{
					bossStatueController.ForceStopBulletScript();
				}
			}
			this.OnStatueDeath();
			this.State = BossStatuesPatternBehavior.PatternState.Ending;
			return ContinuousBehaviorResult.Continue;
		}
		if (this.State == BossStatuesPatternBehavior.PatternState.MovingToStartingPosition)
		{
			if (this.m_stateTimer <= 0f)
			{
				this.SetActiveState(BossStatueController.StatueState.StandStill);
				if (this.AreAllGroundedAndReadyToJump())
				{
					this.State = BossStatuesPatternBehavior.PatternState.InProgress;
				}
			}
		}
		else if (this.State == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			float timeElapsed = this.m_timeElapsed;
			this.m_timeElapsed += this.m_deltaTime;
			this.UpdatePositions();
			if (this.attackType != null)
			{
				this.attackType.Update(timeElapsed, this.m_timeElapsed, this.m_activeStatues);
			}
			if (this.IsFinished())
			{
				this.State = BossStatuesPatternBehavior.PatternState.Ending;
			}
		}
		if (this.State == BossStatuesPatternBehavior.PatternState.Ending && this.AreAllGroundedAndReadyToJump())
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x0018A3E8 File Offset: 0x001885E8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			this.m_activeStatues[i].ClearQueuedAttacks();
			this.m_activeStatues[i].State = BossStatueController.StatueState.WaitForAttack;
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
		this.State = BossStatuesPatternBehavior.PatternState.Idle;
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x0018A44C File Offset: 0x0018864C
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x0018A450 File Offset: 0x00188650
	public override bool IsReady()
	{
		if (!base.IsReady())
		{
			return false;
		}
		if (Array.IndexOf<int>(this.numStatuesArray, this.m_statuesController.NumLivingStatues) < 0)
		{
			return false;
		}
		for (int i = 0; i < this.m_activeStatues.Count; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive && bossStatueController.IsTransforming)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049B9 RID: 18873 RVA: 0x0018A4DC File Offset: 0x001886DC
	protected virtual void BeginState(BossStatuesPatternBehavior.PatternState state)
	{
		if (state != BossStatuesPatternBehavior.PatternState.MovingToStartingPosition)
		{
			if (state != BossStatuesPatternBehavior.PatternState.InProgress)
			{
				if (state == BossStatuesPatternBehavior.PatternState.Ending)
				{
					this.SetActiveState(BossStatueController.StatueState.StandStill);
				}
			}
			else
			{
				this.m_timeElapsed = 0f;
				this.SetActiveState(BossStatueController.StatueState.HopToTarget);
				if (this.OverrideMoveSpeed > 0f)
				{
					this.m_statuesController.OverrideMoveSpeed = new float?(this.OverrideMoveSpeed);
				}
				if (this.attackType != null)
				{
					this.attackType.Update(-0.02f, 0f, this.m_activeStatues);
				}
			}
		}
		else
		{
			this.m_statuesController.IsTransitioning = true;
			this.m_stateTimer = 0f;
			this.SetActiveState(BossStatueController.StatueState.HopToTarget);
			float effectiveMoveSpeed = this.m_statuesController.GetEffectiveMoveSpeed(this.m_statuesController.transitionMoveSpeed);
			for (int i = 0; i < this.m_activeStatueCount; i++)
			{
				float num = this.m_activeStatues[i].DistancetoTarget / effectiveMoveSpeed;
				this.m_stateTimer = Mathf.Max(this.m_stateTimer, num);
			}
		}
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x0018A5EC File Offset: 0x001887EC
	protected virtual void EndState(BossStatuesPatternBehavior.PatternState state)
	{
		if (state != BossStatuesPatternBehavior.PatternState.MovingToStartingPosition)
		{
			if (state == BossStatuesPatternBehavior.PatternState.InProgress)
			{
				if (this.OverrideMoveSpeed > 0f)
				{
					this.m_statuesController.OverrideMoveSpeed = null;
				}
			}
		}
		else
		{
			this.m_statuesController.IsTransitioning = false;
		}
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x0018A648 File Offset: 0x00188848
	protected void SetActiveState(BossStatueController.StatueState newState)
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive)
			{
				bossStatueController.State = newState;
			}
		}
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x0018A69C File Offset: 0x0018889C
	protected bool AreAllGrounded()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive && !bossStatueController.IsGrounded)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049BD RID: 18877 RVA: 0x0018A6F8 File Offset: 0x001888F8
	protected bool AreAllGroundedAndReadyToJump()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive && (!bossStatueController.IsGrounded || !bossStatueController.ReadyToJump))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049BE RID: 18878 RVA: 0x0018A760 File Offset: 0x00188960
	private bool AnyStatuesHaveDied()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (!bossStatueController || bossStatueController.healthHaver.IsDead)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x0018A7B0 File Offset: 0x001889B0
	private void RefreshActiveStatues()
	{
		for (int i = this.m_activeStatues.Count - 1; i >= 0; i--)
		{
			if (!this.m_activeStatues[i] || this.m_activeStatues[i].healthHaver.IsDead)
			{
				this.m_activeStatues.RemoveAt(i);
			}
		}
		this.m_activeStatueCount = this.m_activeStatues.Count;
	}

	// Token: 0x060049C0 RID: 18880 RVA: 0x0018A82C File Offset: 0x00188A2C
	private void UpdateNumStatuesArray()
	{
		this.numStatuesArray = BraveUtility.ParsePageNums(this.numStatues);
	}

	// Token: 0x060049C1 RID: 18881
	protected abstract void InitPositions();

	// Token: 0x060049C2 RID: 18882
	protected abstract void UpdatePositions();

	// Token: 0x060049C3 RID: 18883
	protected abstract bool IsFinished();

	// Token: 0x060049C4 RID: 18884 RVA: 0x0018A840 File Offset: 0x00188A40
	protected virtual void OnStatueDeath()
	{
	}

	// Token: 0x060049C5 RID: 18885 RVA: 0x0018A844 File Offset: 0x00188A44
	protected void ReorderStatues(Vector2[] positions)
	{
		int[] array = new int[this.m_activeStatueCount];
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			array[i] = i;
		}
		float num = float.MaxValue;
		int[] array2 = new int[this.m_activeStatueCount];
		do
		{
			int num2 = 0;
			float num3 = 0f;
			for (int j = 0; j < this.m_activeStatueCount; j++)
			{
				if (this.m_activeStatues[num2])
				{
					num3 += Vector2.Distance(this.m_activeStatues[num2].GroundPosition, positions[array[j]]);
				}
				num2++;
			}
			if (num3 < num)
			{
				num = num3;
				Array.Copy(array, array2, this.m_activeStatueCount);
			}
		}
		while (BraveMathCollege.NextPermutation(ref array));
		List<BossStatueController> list = new List<BossStatueController>(this.m_activeStatues);
		for (int k = 0; k < this.m_activeStatueCount; k++)
		{
			this.m_activeStatues[k] = list[array2[k]];
		}
	}

	// Token: 0x04003E1F RID: 15903
	public string numStatues = "2-4";

	// Token: 0x04003E20 RID: 15904
	public float OverrideMoveSpeed = -1f;

	// Token: 0x04003E21 RID: 15905
	public bool waitForStartingPositions = true;

	// Token: 0x04003E22 RID: 15906
	public BossStatuesPatternBehavior.StatueAttack attackType;

	// Token: 0x04003E23 RID: 15907
	protected BossStatuesPatternBehavior.PatternState m_state;

	// Token: 0x04003E24 RID: 15908
	protected int[] numStatuesArray;

	// Token: 0x04003E25 RID: 15909
	protected BossStatuesController m_statuesController;

	// Token: 0x04003E26 RID: 15910
	protected List<BossStatueController> m_activeStatues;

	// Token: 0x04003E27 RID: 15911
	protected int m_activeStatueCount;

	// Token: 0x04003E28 RID: 15912
	protected float m_stateTimer;

	// Token: 0x04003E29 RID: 15913
	protected float m_timeElapsed;

	// Token: 0x02000D98 RID: 3480
	protected enum PatternState
	{
		// Token: 0x04003E2B RID: 15915
		Idle,
		// Token: 0x04003E2C RID: 15916
		MovingToStartingPosition,
		// Token: 0x04003E2D RID: 15917
		InProgress,
		// Token: 0x04003E2E RID: 15918
		Ending
	}

	// Token: 0x02000D99 RID: 3481
	[Serializable]
	public abstract class StatueAttack
	{
		// Token: 0x060049C7 RID: 18887 RVA: 0x0018A964 File Offset: 0x00188B64
		public virtual void Start(List<BossStatueController> statues)
		{
		}

		// Token: 0x060049C8 RID: 18888
		public abstract void Update(float prevTimeElapsed, float timeElapsed, List<BossStatueController> statues);
	}

	// Token: 0x02000D9A RID: 3482
	[Serializable]
	public class TimedAttacks : BossStatuesPatternBehavior.StatueAttack
	{
		// Token: 0x060049CA RID: 18890 RVA: 0x0018A970 File Offset: 0x00188B70
		public override void Update(float prevTimeElapsed, float timeElapsed, List<BossStatueController> statues)
		{
			for (int i = 0; i < this.attacks.Count; i++)
			{
				BossStatuesPatternBehavior.TimedAttacks.TimedAttack timedAttack = this.attacks[i];
				if (prevTimeElapsed < timedAttack.delay && timeElapsed >= timedAttack.delay && timedAttack.index < statues.Count)
				{
					statues[timedAttack.index].QueuedBulletScript.Add(timedAttack.bulletScript);
				}
			}
		}

		// Token: 0x04003E2F RID: 15919
		public List<BossStatuesPatternBehavior.TimedAttacks.TimedAttack> attacks;

		// Token: 0x02000D9B RID: 3483
		[Serializable]
		public class TimedAttack
		{
			// Token: 0x04003E30 RID: 15920
			public int index;

			// Token: 0x04003E31 RID: 15921
			public float delay;

			// Token: 0x04003E32 RID: 15922
			public BulletScriptSelector bulletScript;
		}
	}

	// Token: 0x02000D9C RID: 3484
	[Serializable]
	public class ConstantAttacks : BossStatuesPatternBehavior.StatueAttack
	{
		// Token: 0x060049CD RID: 18893 RVA: 0x0018A9FC File Offset: 0x00188BFC
		public override void Start(List<BossStatueController> statues)
		{
			this.m_bulletScriptIndices = new int[statues.Count];
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0018AA10 File Offset: 0x00188C10
		public override void Update(float prevTimeElapsed, float timeElapsed, List<BossStatueController> statues)
		{
			for (int i = 0; i < this.attacks.Count; i++)
			{
				BossStatuesPatternBehavior.ConstantAttacks.ConstantAttackGroup constantAttackGroup = this.attacks[i];
				int index = constantAttackGroup.index;
				if (index < statues.Count)
				{
					if (statues[index].QueuedBulletScript.Count == 0)
					{
						int num = this.m_bulletScriptIndices[index];
						num = (num + 1) % constantAttackGroup.bulletScript.Count;
						this.m_bulletScriptIndices[index] = num;
						BulletScriptSelector bulletScriptSelector = constantAttackGroup.bulletScript[num];
						statues[index].QueuedBulletScript.Add(bulletScriptSelector);
					}
				}
			}
		}

		// Token: 0x04003E33 RID: 15923
		public List<BossStatuesPatternBehavior.ConstantAttacks.ConstantAttackGroup> attacks;

		// Token: 0x04003E34 RID: 15924
		[NonSerialized]
		private int[] m_bulletScriptIndices;

		// Token: 0x02000D9D RID: 3485
		[Serializable]
		public class ConstantAttackGroup
		{
			// Token: 0x04003E35 RID: 15925
			public int index;

			// Token: 0x04003E36 RID: 15926
			public List<BulletScriptSelector> bulletScript;
		}
	}
}
