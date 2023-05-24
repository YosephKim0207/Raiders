using System;
using System.Collections.Generic;
using System.Linq;
using FullInspector;
using UnityEngine;

// Token: 0x02000D0F RID: 3343
public class AttackMoveBehavior : BasicAttackBehavior
{
	// Token: 0x06004682 RID: 18050 RVA: 0x0016DF84 File Offset: 0x0016C184
	private bool ShowN()
	{
		return this.selectType == AttackMoveBehavior.SelectType.RandomClosestN || this.selectType == AttackMoveBehavior.SelectType.RandomFurthestN;
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x0016DFA0 File Offset: 0x0016C1A0
	private bool ShowDisallowNearest()
	{
		return this.selectType == AttackMoveBehavior.SelectType.Random || this.selectType == AttackMoveBehavior.SelectType.RandomClosestN;
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x0016DFBC File Offset: 0x0016C1BC
	private bool ShowSubsequentMoveSpeed()
	{
		return this.selectType == AttackMoveBehavior.SelectType.InSequence;
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x0016DFC8 File Offset: 0x0016C1C8
	public override void Start()
	{
		base.Start();
		this.m_shadowTrail = this.m_aiActor.GetComponent<AfterImageTrailController>();
		if (this.bulletScript != null && !this.bulletScript.IsNull)
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x0016E034 File Offset: 0x0016C234
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_shadowSprite == null)
		{
			this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_aiAnimator.LockFacingDirection = true;
		this.m_aiAnimator.FacingDirection = -90f;
		if (this.HideGun && this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(false, "AttackMoveBehavior");
		}
		if (!string.IsNullOrEmpty(this.preMoveAnimation))
		{
			this.State = AttackMoveBehavior.MoveState.PreMove;
		}
		else
		{
			this.State = AttackMoveBehavior.MoveState.Move;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004687 RID: 18055 RVA: 0x0016E138 File Offset: 0x0016C338
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == AttackMoveBehavior.MoveState.PreMove)
		{
			if (!this.m_aiAnimator.IsPlaying(this.preMoveAnimation))
			{
				this.State = AttackMoveBehavior.MoveState.Move;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.State == AttackMoveBehavior.MoveState.Move)
		{
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			if (this.m_deltaTime <= 0f)
			{
				return ContinuousBehaviorResult.Continue;
			}
			Vector2 vector;
			if (this.SmoothStep)
			{
				vector = Vector2Extensions.SmoothStep(this.m_startPoint, this.m_targetPoint, this.m_timer / this.m_moveTime);
			}
			else
			{
				vector = Vector2.Lerp(this.m_startPoint, this.m_targetPoint, this.m_timer / this.m_moveTime);
			}
			if (this.animateShadow && this.m_moveTime - this.m_timer <= this.m_shadowOutTime)
			{
				this.m_shadowOutTime = -1f;
				this.m_shadowSprite.spriteAnimator.Play(this.shadowOutAnim);
			}
			if (this.m_timer > this.m_moveTime)
			{
				if (this.selectType != AttackMoveBehavior.SelectType.InSequence || this.m_sequenceIndex >= this.Positions.Length)
				{
					this.m_aiActor.BehaviorVelocity = Vector2.zero;
					return ContinuousBehaviorResult.Finished;
				}
				this.PlanNextMove();
			}
			this.m_aiActor.BehaviorVelocity = (vector - unitCenter) / this.m_deltaTime;
			if (this.updateFacingDirectionDuringMove)
			{
				this.UpdateFacingDirection(vector - unitCenter);
			}
			this.m_timer += this.m_deltaTime;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004688 RID: 18056 RVA: 0x0016E2D4 File Offset: 0x0016C4D4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.State = AttackMoveBehavior.MoveState.None;
		if (this.HideGun && this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "AttackMoveBehavior");
		}
		if (!string.IsNullOrEmpty(this.preMoveAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.preMoveAnimation);
		}
		if (!string.IsNullOrEmpty(this.moveAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.moveAnimation);
		}
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004689 RID: 18057 RVA: 0x0016E384 File Offset: 0x0016C584
	public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_state != AttackMoveBehavior.MoveState.None && this.bulletScript != null && !this.bulletScript.IsNull && clip.GetFrame(frame).eventInfo == "fire")
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
			this.m_bulletSource.BulletScript = this.bulletScript;
			this.m_bulletSource.Initialize();
		}
	}

	// Token: 0x0600468A RID: 18058 RVA: 0x0016E428 File Offset: 0x0016C628
	private void UpdateTargetPoint()
	{
		if (this.selectType == AttackMoveBehavior.SelectType.Random)
		{
			if (this.DisallowNearest && this.Positions.Length > 1)
			{
				Vector2 unitCenter = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				int num = -1;
				float num2 = -1f;
				for (int i = 0; i < this.Positions.Length; i++)
				{
					Vector2 position = this.GetPosition(i, null);
					float num3 = Vector2.Distance(unitCenter, position);
					if (i == 0 || num3 < num2)
					{
						num = i;
						num2 = num3;
					}
				}
				this.m_targetPoint = this.GetPosition(BraveUtility.SequentialRandomRange(0, this.Positions.Length, num, null, true), null);
			}
			else
			{
				this.m_targetPoint = this.GetPosition(UnityEngine.Random.Range(0, this.Positions.Length), null);
			}
		}
		else if (this.selectType == AttackMoveBehavior.SelectType.Closest)
		{
			Vector2 unitCenter2 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			int num4 = -1;
			float num5 = -1f;
			for (int j = 0; j < this.Positions.Length; j++)
			{
				Vector2 position2 = this.GetPosition(j, null);
				float num6 = Vector2.Distance(unitCenter2, position2);
				if (j == 0 || num6 < num5)
				{
					num4 = j;
					num5 = num6;
				}
			}
			this.m_targetPoint = this.GetPosition(num4, null);
		}
		else if (this.selectType == AttackMoveBehavior.SelectType.RandomClosestN)
		{
			Vector2 unitCenter3 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			List<Tuple<int, float>> list = new List<Tuple<int, float>>();
			for (int k = 0; k < this.Positions.Length; k++)
			{
				list.Add(Tuple.Create<int, float>(k, Vector2.Distance(unitCenter3, this.GetPosition(k, null))));
			}
			list = new List<Tuple<int, float>>(list.OrderBy((Tuple<int, float> t) => t.Second));
			if (this.DisallowNearest)
			{
				list.RemoveAt(0);
			}
			this.m_targetPoint = this.GetPosition(list[UnityEngine.Random.Range(0, Mathf.Min(this.N + 1, list.Count))].First, null);
		}
		else if (this.selectType == AttackMoveBehavior.SelectType.Furthest)
		{
			Vector2 unitCenter4 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			int num7 = -1;
			float num8 = float.MaxValue;
			for (int l = 0; l < this.Positions.Length; l++)
			{
				Vector2 position3 = this.GetPosition(l, null);
				float num9 = Vector2.Distance(unitCenter4, position3);
				if (l == 0 || num9 > num8)
				{
					num7 = l;
					num8 = num9;
				}
			}
			this.m_targetPoint = this.GetPosition(num7, null);
		}
		else if (this.selectType == AttackMoveBehavior.SelectType.RandomFurthestN)
		{
			Vector2 unitCenter5 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			List<Tuple<int, float>> list2 = new List<Tuple<int, float>>();
			for (int m = 0; m < this.Positions.Length; m++)
			{
				list2.Add(Tuple.Create<int, float>(m, Vector2.Distance(unitCenter5, this.GetPosition(m, null))));
			}
			list2 = new List<Tuple<int, float>>(list2.OrderByDescending((Tuple<int, float> t) => t.Second));
			this.m_targetPoint = this.GetPosition(list2[UnityEngine.Random.Range(0, Mathf.Min(this.N + 1, list2.Count))].First, null);
		}
		else
		{
			if (this.selectType != AttackMoveBehavior.SelectType.InSequence)
			{
				Debug.LogError("Unknown select type: " + this.selectType);
				return;
			}
			this.m_targetPoint = this.GetPosition(this.m_sequenceIndex++, null);
		}
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x0016E86C File Offset: 0x0016CA6C
	private Vector2 GetPosition(int i, bool? mirror = null)
	{
		if (mirror == null)
		{
			mirror = new bool?(this.m_mirrorPositions);
		}
		if (this.positionType != AttackMoveBehavior.PositionType.RelativeToRoomCenter && this.positionType != AttackMoveBehavior.PositionType.RelativeToHelicopterCenter)
		{
			Debug.LogError("Unknown position type: " + this.positionType);
			return Vector2.zero;
		}
		Vector2 center = this.m_aiActor.ParentRoom.area.Center;
		if (this.positionType == AttackMoveBehavior.PositionType.RelativeToHelicopterCenter)
		{
			float num = 0f;
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController.healthHaver.IsAlive)
				{
					num = Mathf.Max(num, playerController.specRigidbody.UnitCenter.y);
				}
			}
			if (num > 0f)
			{
				center.y = num;
			}
		}
		if (mirror.Value)
		{
			return center + Vector2.Scale(this.Positions[i], new Vector2(-1f, 1f));
		}
		return center + this.Positions[i];
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x0016E9B0 File Offset: 0x0016CBB0
	private void UpdateFacingDirection(Vector2 toTarget)
	{
		if (toTarget == Vector2.zero)
		{
			return;
		}
		toTarget.Normalize();
		if (this.biasFacingRoomCenter)
		{
			Vector2 vector = this.m_aiActor.ParentRoom.area.Center - this.m_aiActor.specRigidbody.UnitCenter;
			toTarget = (toTarget + 0.2f * vector).normalized;
		}
		if (this.faceBottomCenter)
		{
			Vector2 vector2 = new Vector2(this.m_aiActor.ParentRoom.area.UnitCenter.x, this.m_aiActor.specRigidbody.UnitCenter.y - 15f);
			toTarget = (vector2 - this.m_aiActor.specRigidbody.UnitCenter).normalized;
		}
		this.m_aiAnimator.FacingDirection = toTarget.ToAngle();
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x0600468D RID: 18061 RVA: 0x0016EAA8 File Offset: 0x0016CCA8
	// (set) Token: 0x0600468E RID: 18062 RVA: 0x0016EAB0 File Offset: 0x0016CCB0
	private AttackMoveBehavior.MoveState State
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

	// Token: 0x0600468F RID: 18063 RVA: 0x0016EAE0 File Offset: 0x0016CCE0
	private void BeginState(AttackMoveBehavior.MoveState state)
	{
		if (state == AttackMoveBehavior.MoveState.PreMove)
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilCancelled(this.preMoveAnimation, false, null, -1f, false);
		}
		else if (state == AttackMoveBehavior.MoveState.Move)
		{
			this.m_sequenceIndex = 0;
			if (this.MirrorIfCloser)
			{
				Vector2 position = this.GetPosition(0, new bool?(false));
				Vector2 position2 = this.GetPosition(0, new bool?(true));
				Vector2 unitCenter = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				this.m_mirrorPositions = Vector2.Distance(position2, unitCenter) < Vector2.Distance(position, unitCenter);
			}
			this.PlanNextMove();
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.PlayUntilCancelled(this.moveAnimation, false, null, -1f, false);
			if (this.DisableCollisionDuringMove)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
			if (this.disableGoops)
			{
				if (this.m_goopDoers == null)
				{
					this.m_goopDoers = this.m_aiActor.GetComponents<GoopDoer>();
				}
				for (int i = 0; i < this.m_goopDoers.Length; i++)
				{
					this.m_goopDoers[i].enabled = false;
				}
			}
			if (this.enableShadowTrail)
			{
				this.m_shadowTrail.spawnShadows = true;
			}
			if (this.animateShadow)
			{
				this.m_shadowSprite.spriteAnimator.Play(this.shadowInAnim);
				this.m_shadowOutTime = this.m_shadowSprite.spriteAnimator.GetClipByName(this.shadowOutAnim).BaseClipLength;
			}
		}
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x0016EC94 File Offset: 0x0016CE94
	private void PlanNextMove()
	{
		this.m_startPoint = this.m_aiActor.specRigidbody.UnitCenter;
		this.UpdateTargetPoint();
		Vector2 vector = this.m_targetPoint - this.m_startPoint;
		float magnitude = vector.magnitude;
		if (this.selectType == AttackMoveBehavior.SelectType.InSequence && this.m_sequenceIndex > 1 && this.SubsequentMoveSpeed > 0f)
		{
			this.m_moveTime = magnitude / this.SubsequentMoveSpeed;
		}
		else
		{
			this.m_moveTime = this.MoveTime;
			if (this.MinSpeed > 0f)
			{
				this.m_moveTime = Mathf.Min(this.m_moveTime, magnitude / this.MinSpeed);
			}
			if (this.MaxSpeed > 0f)
			{
				this.m_moveTime = Mathf.Max(this.m_moveTime, magnitude / this.MaxSpeed);
			}
		}
		this.UpdateFacingDirection(vector);
		this.m_timer = 0f;
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x0016ED84 File Offset: 0x0016CF84
	private void EndState(AttackMoveBehavior.MoveState state)
	{
		if (state == AttackMoveBehavior.MoveState.Move)
		{
			if (this.DisableCollisionDuringMove)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
				this.m_aiActor.IsGone = false;
			}
			if (this.disableGoops)
			{
				for (int i = 0; i < this.m_goopDoers.Length; i++)
				{
					this.m_goopDoers[i].enabled = true;
				}
			}
			if (this.enableShadowTrail)
			{
				this.m_shadowTrail.spawnShadows = false;
			}
		}
	}

	// Token: 0x040038F3 RID: 14579
	public AttackMoveBehavior.PositionType positionType = AttackMoveBehavior.PositionType.RelativeToRoomCenter;

	// Token: 0x040038F4 RID: 14580
	public Vector2[] Positions;

	// Token: 0x040038F5 RID: 14581
	public AttackMoveBehavior.SelectType selectType = AttackMoveBehavior.SelectType.Random;

	// Token: 0x040038F6 RID: 14582
	[InspectorShowIf("ShowN")]
	[InspectorIndent]
	public int N;

	// Token: 0x040038F7 RID: 14583
	[InspectorIndent]
	[InspectorShowIf("ShowDisallowNearest")]
	public bool DisallowNearest;

	// Token: 0x040038F8 RID: 14584
	public bool SmoothStep = true;

	// Token: 0x040038F9 RID: 14585
	public float MoveTime = 1f;

	// Token: 0x040038FA RID: 14586
	public float MinSpeed;

	// Token: 0x040038FB RID: 14587
	public float MaxSpeed;

	// Token: 0x040038FC RID: 14588
	[InspectorShowIf("ShowSubsequentMoveSpeed")]
	public float SubsequentMoveSpeed = -1f;

	// Token: 0x040038FD RID: 14589
	public bool MirrorIfCloser;

	// Token: 0x040038FE RID: 14590
	public bool DisableCollisionDuringMove;

	// Token: 0x040038FF RID: 14591
	[InspectorCategory("Attack")]
	public GameObject ShootPoint;

	// Token: 0x04003900 RID: 14592
	[InspectorCategory("Attack")]
	public BulletScriptSelector bulletScript;

	// Token: 0x04003901 RID: 14593
	[InspectorCategory("Visuals")]
	public string preMoveAnimation;

	// Token: 0x04003902 RID: 14594
	[InspectorCategory("Visuals")]
	public string moveAnimation;

	// Token: 0x04003903 RID: 14595
	[InspectorCategory("Visuals")]
	public bool disableGoops;

	// Token: 0x04003904 RID: 14596
	[InspectorCategory("Visuals")]
	public bool updateFacingDirectionDuringMove = true;

	// Token: 0x04003905 RID: 14597
	[InspectorCategory("Visuals")]
	public bool biasFacingRoomCenter;

	// Token: 0x04003906 RID: 14598
	[InspectorCategory("Visuals")]
	public bool faceBottomCenter;

	// Token: 0x04003907 RID: 14599
	[InspectorCategory("Visuals")]
	public bool enableShadowTrail;

	// Token: 0x04003908 RID: 14600
	[InspectorCategory("Visuals")]
	public bool HideGun;

	// Token: 0x04003909 RID: 14601
	[InspectorCategory("Visuals")]
	public bool animateShadow;

	// Token: 0x0400390A RID: 14602
	[InspectorShowIf("animateShadow")]
	[InspectorCategory("Visuals")]
	public string shadowInAnim;

	// Token: 0x0400390B RID: 14603
	[InspectorShowIf("animateShadow")]
	[InspectorCategory("Visuals")]
	public string shadowOutAnim;

	// Token: 0x0400390C RID: 14604
	private AttackMoveBehavior.MoveState m_state;

	// Token: 0x0400390D RID: 14605
	private Vector2 m_startPoint;

	// Token: 0x0400390E RID: 14606
	private Vector2 m_targetPoint;

	// Token: 0x0400390F RID: 14607
	private float m_moveTime;

	// Token: 0x04003910 RID: 14608
	private float m_timer;

	// Token: 0x04003911 RID: 14609
	private int m_sequenceIndex;

	// Token: 0x04003912 RID: 14610
	private bool m_mirrorPositions;

	// Token: 0x04003913 RID: 14611
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003914 RID: 14612
	private GoopDoer[] m_goopDoers;

	// Token: 0x04003915 RID: 14613
	private AfterImageTrailController m_shadowTrail;

	// Token: 0x04003916 RID: 14614
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003917 RID: 14615
	private float m_shadowOutTime;

	// Token: 0x02000D10 RID: 3344
	public enum PositionType
	{
		// Token: 0x0400391B RID: 14619
		RelativeToRoomCenter = 20,
		// Token: 0x0400391C RID: 14620
		RelativeToHelicopterCenter = 40
	}

	// Token: 0x02000D11 RID: 3345
	public enum SelectType
	{
		// Token: 0x0400391E RID: 14622
		Random = 10,
		// Token: 0x0400391F RID: 14623
		Closest = 20,
		// Token: 0x04003920 RID: 14624
		RandomClosestN = 30,
		// Token: 0x04003921 RID: 14625
		Furthest = 40,
		// Token: 0x04003922 RID: 14626
		RandomFurthestN = 50,
		// Token: 0x04003923 RID: 14627
		InSequence = 60
	}

	// Token: 0x02000D12 RID: 3346
	private enum MoveState
	{
		// Token: 0x04003925 RID: 14629
		None,
		// Token: 0x04003926 RID: 14630
		PreMove,
		// Token: 0x04003927 RID: 14631
		Move
	}
}
