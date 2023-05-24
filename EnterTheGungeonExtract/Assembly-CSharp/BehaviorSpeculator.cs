using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D7B RID: 3451
public class BehaviorSpeculator : BaseBehavior<FullSerializerSerializer>
{
	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x060048F0 RID: 18672 RVA: 0x00184D20 File Offset: 0x00182F20
	// (set) Token: 0x060048F1 RID: 18673 RVA: 0x00184D44 File Offset: 0x00182F44
	public float LocalTimeScale
	{
		get
		{
			if (this.m_aiActor)
			{
				return this.m_aiActor.LocalTimeScale;
			}
			return this.m_localTimeScale;
		}
		set
		{
			if (this.m_aiActor)
			{
				this.m_aiActor.LocalTimeScale = value;
			}
			else
			{
				this.m_localTimeScale = value;
			}
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x060048F2 RID: 18674 RVA: 0x00184D70 File Offset: 0x00182F70
	public float LocalDeltaTime
	{
		get
		{
			if (this.m_aiActor != null)
			{
				return this.m_aiActor.LocalDeltaTime;
			}
			return BraveTime.DeltaTime * this.LocalTimeScale;
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x060048F3 RID: 18675 RVA: 0x00184D9C File Offset: 0x00182F9C
	// (set) Token: 0x060048F4 RID: 18676 RVA: 0x00184DA4 File Offset: 0x00182FA4
	public float AttackCooldown
	{
		get
		{
			return this.m_attackCooldownTimer;
		}
		set
		{
			this.m_attackCooldownTimer = value;
		}
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x060048F5 RID: 18677 RVA: 0x00184DB0 File Offset: 0x00182FB0
	// (set) Token: 0x060048F6 RID: 18678 RVA: 0x00184DB8 File Offset: 0x00182FB8
	public float GlobalCooldown
	{
		get
		{
			return this.m_globalCooldownTimer;
		}
		set
		{
			this.m_globalCooldownTimer = value;
		}
	}

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x060048F7 RID: 18679 RVA: 0x00184DC4 File Offset: 0x00182FC4
	// (set) Token: 0x060048F8 RID: 18680 RVA: 0x00184DCC File Offset: 0x00182FCC
	public float CooldownScale
	{
		get
		{
			return this.m_cooldownScale;
		}
		set
		{
			this.m_cooldownScale = value;
		}
	}

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x060048F9 RID: 18681 RVA: 0x00184DD8 File Offset: 0x00182FD8
	public BehaviorBase ActiveContinuousAttackBehavior
	{
		get
		{
			if (this.m_activeContinuousBehavior is AttackBehaviorBase)
			{
				return this.m_activeContinuousBehavior;
			}
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.AttackBehaviors))
			{
				return this.m_classSpecificContinuousBehavior[this.AttackBehaviors];
			}
			return null;
		}
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x060048FA RID: 18682 RVA: 0x00184E28 File Offset: 0x00183028
	public bool IsInterruptable
	{
		get
		{
			bool flag = true;
			if (this.m_activeContinuousBehavior != null)
			{
				flag &= this.m_activeContinuousBehavior.IsOverridable();
			}
			if (this.m_classSpecificContinuousBehavior.Count > 0)
			{
				if (this.m_classSpecificContinuousBehavior.ContainsKey(this.OverrideBehaviors))
				{
					flag &= this.m_classSpecificContinuousBehavior[this.OverrideBehaviors].IsOverridable();
				}
				if (this.m_classSpecificContinuousBehavior.ContainsKey(this.TargetBehaviors))
				{
					flag &= this.m_classSpecificContinuousBehavior[this.TargetBehaviors].IsOverridable();
				}
				if (this.m_classSpecificContinuousBehavior.ContainsKey(this.MovementBehaviors))
				{
					flag &= this.m_classSpecificContinuousBehavior[this.MovementBehaviors].IsOverridable();
				}
				if (this.m_classSpecificContinuousBehavior.ContainsKey(this.AttackBehaviors))
				{
					flag &= this.m_classSpecificContinuousBehavior[this.AttackBehaviors].IsOverridable();
				}
				if (this.m_classSpecificContinuousBehavior.ContainsKey(this.OtherBehaviors))
				{
					flag &= this.m_classSpecificContinuousBehavior[this.OtherBehaviors].IsOverridable();
				}
				return flag;
			}
			return flag;
		}
	}

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x060048FB RID: 18683 RVA: 0x00184F50 File Offset: 0x00183150
	// (set) Token: 0x060048FC RID: 18684 RVA: 0x00184F74 File Offset: 0x00183174
	public GameActor PlayerTarget
	{
		get
		{
			if (this.m_aiActor)
			{
				return this.m_aiActor.PlayerTarget;
			}
			return this.m_playerTarget;
		}
		set
		{
			if (this.m_aiActor)
			{
				this.m_aiActor.PlayerTarget = value;
			}
			else
			{
				this.m_playerTarget = value;
			}
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x060048FD RID: 18685 RVA: 0x00184FA0 File Offset: 0x001831A0
	public SpeculativeRigidbody TargetRigidbody
	{
		get
		{
			if (this.m_aiActor)
			{
				return this.m_aiActor.TargetRigidbody;
			}
			if (this.m_playerTarget)
			{
				return this.m_playerTarget.specRigidbody;
			}
			return null;
		}
	}

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x060048FE RID: 18686 RVA: 0x00184FDC File Offset: 0x001831DC
	public Vector2 TargetVelocity
	{
		get
		{
			if (this.m_aiActor)
			{
				return this.m_aiActor.TargetVelocity;
			}
			if (this.m_playerTarget)
			{
				return this.m_playerTarget.specRigidbody.Velocity;
			}
			return Vector2.zero;
		}
	}

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x0018502C File Offset: 0x0018322C
	// (set) Token: 0x06004900 RID: 18688 RVA: 0x00185034 File Offset: 0x00183234
	public bool PreventMovement { get; set; }

	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x00185040 File Offset: 0x00183240
	// (set) Token: 0x06004902 RID: 18690 RVA: 0x00185048 File Offset: 0x00183248
	public FleePlayerData FleePlayerData { get; set; }

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x00185054 File Offset: 0x00183254
	public AttackBehaviorGroup AttackBehaviorGroup
	{
		get
		{
			if (this.AttackBehaviors == null)
			{
				return null;
			}
			for (int i = 0; i < this.AttackBehaviors.Count; i++)
			{
				if (this.AttackBehaviors[i] is AttackBehaviorGroup)
				{
					return this.AttackBehaviors[i] as AttackBehaviorGroup;
				}
			}
			return null;
		}
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x001850B4 File Offset: 0x001832B4
	private void Start()
	{
		this.m_aiActor = base.GetComponent<AIActor>();
		if (this.m_aiActor)
		{
			this.m_aiActor.healthHaver.OnPreDeath += this.OnPreDeath;
		}
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged += this.OnDamaged;
		}
		if (this.OverrideStartingFacingDirection && base.aiAnimator)
		{
			base.aiAnimator.FacingDirection = this.StartingFacingDirection;
		}
		if (this.m_aiActor)
		{
			this.m_aiActor.specRigidbody.Initialize();
		}
		this.RegisterBehaviors(this.OverrideBehaviors);
		this.RegisterBehaviors(this.TargetBehaviors);
		this.RegisterBehaviors(this.MovementBehaviors);
		this.RegisterBehaviors(this.AttackBehaviors);
		this.RegisterBehaviors(this.OtherBehaviors);
		this.StartBehaviors();
		if (this.InstantFirstTick)
		{
			this.m_tickTimer = this.TickInterval;
		}
		this.m_postAwakenDelay = this.PostAwakenDelay;
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001851D4 File Offset: 0x001833D4
	private void Update()
	{
		if (this.m_aiActor)
		{
			if (!this.m_aiActor.enabled)
			{
				return;
			}
			if (this.m_aiActor.healthHaver.IsDead)
			{
				return;
			}
			if (!this.m_aiActor.HasBeenAwoken)
			{
				return;
			}
			if (this.m_postAwakenDelay > 0f && (!this.RemoveDelayOnReinforce || !base.aiActor.IsInReinforcementLayer))
			{
				this.m_postAwakenDelay = Mathf.Max(0f, this.m_postAwakenDelay - this.LocalDeltaTime);
				return;
			}
			if (this.m_aiActor.SpeculatorDelayTime > 0f)
			{
				this.m_aiActor.SpeculatorDelayTime = Mathf.Max(0f, this.m_aiActor.SpeculatorDelayTime - this.LocalDeltaTime);
				return;
			}
		}
		if (!this.m_isFirstUpdate)
		{
			this.FirstUpdate();
			this.m_isFirstUpdate = true;
		}
		this.m_tickTimer += this.LocalDeltaTime;
		this.m_globalCooldownTimer = Mathf.Max(0f, this.m_globalCooldownTimer - this.LocalDeltaTime);
		this.m_attackCooldownTimer = Mathf.Max(0f, this.m_attackCooldownTimer - this.LocalDeltaTime);
		this.m_stunTimer = Mathf.Max(0f, this.m_stunTimer - this.LocalDeltaTime);
		this.UpdateStunVFX();
		if (this.m_groupCooldownTimers != null)
		{
			for (int i = 0; i < this.m_groupCooldownTimers.Count; i++)
			{
				this.m_groupCooldownTimers.Values[i] = Mathf.Max(0f, this.m_groupCooldownTimers.Values[i] - this.LocalDeltaTime);
			}
		}
		bool flag = this.m_tickTimer > this.TickInterval;
		bool flag2 = this.m_globalCooldownTimer > 0f;
		this.UpkeepBehaviors(flag);
		if (!this.IsStunned)
		{
			this.UpdateBehaviors(flag, flag2);
		}
		if (flag)
		{
			this.m_tickTimer = 0f;
		}
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001853DC File Offset: 0x001835DC
	protected virtual void UpdateStunVFX()
	{
		if (this.m_stunTimer <= 0f && this.m_extantStunVFX != null)
		{
			SpawnManager.Despawn(this.m_extantStunVFX.gameObject);
			this.m_extantStunVFX = null;
		}
		else if (this.m_stunTimer > 0f && this.m_extantStunVFX != null)
		{
			this.m_extantStunVFX.transform.position = base.aiActor.sprite.WorldTopCenter.ToVector3ZUp(this.m_extantStunVFX.transform.position.z);
		}
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x00185488 File Offset: 0x00183688
	private void OnPreDeath(Vector2 dir)
	{
		if (this.IsStunned)
		{
			this.m_stunTimer = 0f;
			this.UpdateStunVFX();
		}
		for (int i = 0; i < this.m_behaviors.Count; i++)
		{
			this.m_behaviors[i].OnActorPreDeath();
		}
		this.Interrupt();
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x001854E4 File Offset: 0x001836E4
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.m_postAwakenDelay = 0f;
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged -= this.OnDamaged;
		}
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x00185518 File Offset: 0x00183718
	protected override void OnDestroy()
	{
		for (int i = 0; i < this.m_behaviors.Count; i++)
		{
			this.m_behaviors[i].Destroy();
		}
		if (this.m_aiActor)
		{
			this.m_aiActor.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged -= this.OnDamaged;
		}
		base.OnDestroy();
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x001855AC File Offset: 0x001837AC
	public void Stun(float duration, bool createVFX = true)
	{
		if (base.aiActor && base.aiActor.healthHaver && base.aiActor.healthHaver.IsBoss)
		{
			return;
		}
		if (base.healthHaver && !base.healthHaver.IsVulnerable)
		{
			return;
		}
		if (this.ImmuneToStun)
		{
			return;
		}
		this.m_stunTimer = Mathf.Max(this.m_stunTimer, duration);
		if (this.m_stunTimer > 0f)
		{
			if (this.IsInterruptable)
			{
				this.Interrupt();
			}
			if (this.m_extantStunVFX == null && createVFX)
			{
				this.m_extantStunVFX = base.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Stun") as GameObject, (base.aiActor.sprite.WorldTopCenter - base.aiActor.CenterPosition).WithX(0f), true, true, false).GetComponent<tk2dSpriteAnimator>();
			}
			base.aiActor.ClearPath();
		}
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001856D0 File Offset: 0x001838D0
	public void UpdateStun(float maxStunTime)
	{
		if (!this.IsStunned)
		{
			return;
		}
		if (base.aiActor && base.aiActor.healthHaver && base.aiActor.healthHaver.IsBoss)
		{
			return;
		}
		if (base.healthHaver && !base.healthHaver.IsVulnerable)
		{
			return;
		}
		if (this.ImmuneToStun)
		{
			return;
		}
		this.m_stunTimer = maxStunTime;
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x00185758 File Offset: 0x00183958
	public void EndStun()
	{
		this.m_stunTimer = 0f;
		this.UpdateStunVFX();
	}

	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x0600490D RID: 18701 RVA: 0x0018576C File Offset: 0x0018396C
	public bool IsStunned
	{
		get
		{
			return this.m_stunTimer > 0f;
		}
	}

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x0600490E RID: 18702 RVA: 0x0018577C File Offset: 0x0018397C
	// (set) Token: 0x0600490F RID: 18703 RVA: 0x00185784 File Offset: 0x00183984
	public bool ImmuneToStun { get; set; }

	// Token: 0x06004910 RID: 18704 RVA: 0x00185790 File Offset: 0x00183990
	public float GetDesiredCombatDistance()
	{
		float num = -1f;
		for (int i = 0; i < this.MovementBehaviors.Count; i++)
		{
			float desiredCombatDistance = this.MovementBehaviors[i].DesiredCombatDistance;
			if (desiredCombatDistance > -1f)
			{
				if (num < 0f)
				{
					num = this.MovementBehaviors[i].DesiredCombatDistance;
				}
				else
				{
					num = Mathf.Min(num, desiredCombatDistance);
				}
			}
		}
		float num2 = num;
		float num3 = float.MinValue;
		for (int j = 0; j < this.AttackBehaviors.Count; j++)
		{
			float minReadyRange = this.AttackBehaviors[j].GetMinReadyRange();
			if (minReadyRange >= 0f)
			{
				num2 = Mathf.Min(num2, minReadyRange);
			}
			num3 = Mathf.Max(num3, this.AttackBehaviors[j].GetMaxRange());
		}
		if (num2 < 2.1474836E+09f)
		{
			return num2;
		}
		if (num3 > -2.1474836E+09f)
		{
			return num3;
		}
		return -1f;
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x00185894 File Offset: 0x00183A94
	public void Interrupt()
	{
		if (this.m_activeContinuousBehavior != null)
		{
			this.EndContinuousBehavior();
		}
		if (this.m_classSpecificContinuousBehavior.Count > 0)
		{
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.OverrideBehaviors))
			{
				this.EndClassSpecificContinuousBehavior(this.OverrideBehaviors);
			}
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.TargetBehaviors))
			{
				this.EndClassSpecificContinuousBehavior(this.TargetBehaviors);
			}
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.MovementBehaviors))
			{
				this.EndClassSpecificContinuousBehavior(this.MovementBehaviors);
			}
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.AttackBehaviors))
			{
				this.EndClassSpecificContinuousBehavior(this.AttackBehaviors);
			}
			if (this.m_classSpecificContinuousBehavior.ContainsKey(this.OtherBehaviors))
			{
				this.EndClassSpecificContinuousBehavior(this.OtherBehaviors);
			}
		}
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x00185970 File Offset: 0x00183B70
	public void InterruptAndDisable()
	{
		this.Interrupt();
		base.enabled = false;
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x00185980 File Offset: 0x00183B80
	public void RefreshBehaviors()
	{
		List<BehaviorBase> behaviors = this.m_behaviors;
		this.m_behaviors = new List<BehaviorBase>();
		this.RefreshBehaviors(this.OverrideBehaviors, behaviors);
		this.RefreshBehaviors(this.TargetBehaviors, behaviors);
		this.RefreshBehaviors(this.MovementBehaviors, behaviors);
		this.RefreshBehaviors(this.AttackBehaviors, behaviors);
		this.RefreshBehaviors(this.OtherBehaviors, behaviors);
	}

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x06004914 RID: 18708 RVA: 0x001859E0 File Offset: 0x00183BE0
	// (remove) Token: 0x06004915 RID: 18709 RVA: 0x00185A18 File Offset: 0x00183C18
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<string> AnimationEventTriggered;

	// Token: 0x06004916 RID: 18710 RVA: 0x00185A50 File Offset: 0x00183C50
	public void TriggerAnimationEvent(string eventInfo)
	{
		if (this.AnimationEventTriggered != null)
		{
			this.AnimationEventTriggered(eventInfo);
		}
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x00185A6C File Offset: 0x00183C6C
	public void SetGroupCooldown(string groupName, float newCooldown)
	{
		if (this.m_groupCooldownTimers == null)
		{
			this.m_groupCooldownTimers = new BraveDictionary<string, float>();
		}
		float num;
		if (this.m_groupCooldownTimers.TryGetValue(groupName, out num))
		{
			if (num < newCooldown)
			{
				this.m_groupCooldownTimers[groupName] = newCooldown;
			}
		}
		else
		{
			this.m_groupCooldownTimers[groupName] = newCooldown;
		}
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x00185AC8 File Offset: 0x00183CC8
	public float GetGroupCooldownTimer(string groupName)
	{
		if (this.m_groupCooldownTimers == null)
		{
			return 0f;
		}
		float num;
		if (this.m_groupCooldownTimers.TryGetValue(groupName, out num))
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x00185B00 File Offset: 0x00183D00
	private void RegisterBehaviors(IList behaviors)
	{
		if (behaviors == null)
		{
			behaviors = new BehaviorBase[0];
		}
		for (int i = 0; i < behaviors.Count; i++)
		{
			this.m_behaviors.Add(behaviors[i] as BehaviorBase);
		}
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x00185B4C File Offset: 0x00183D4C
	private void StartBehaviors()
	{
		for (int i = 0; i < this.m_behaviors.Count; i++)
		{
			this.m_behaviors[i].Init(base.gameObject, this.m_aiActor, base.aiShooter);
			this.m_behaviors[i].Start();
		}
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x00185BAC File Offset: 0x00183DAC
	private void UpkeepBehaviors(bool isTick)
	{
		for (int i = 0; i < this.m_behaviors.Count; i++)
		{
			if (isTick || this.m_behaviors[i].UpdateEveryFrame())
			{
				this.m_behaviors[i].SetDeltaTime((!this.m_behaviors[i].UpdateEveryFrame()) ? this.m_tickTimer : this.LocalDeltaTime);
				this.m_behaviors[i].Upkeep();
			}
		}
		if (this.m_activeContinuousBehavior != null && this.m_activeContinuousBehavior.IsOverridable())
		{
			for (int j = 0; j < this.m_behaviors.Count; j++)
			{
				if ((isTick || this.m_behaviors[j].UpdateEveryFrame()) && this.m_behaviors[j] != this.m_activeContinuousBehavior && this.m_behaviors[j].OverrideOtherBehaviors())
				{
					this.EndContinuousBehavior();
					break;
				}
			}
		}
		else if (this.m_classSpecificContinuousBehavior.Count > 0)
		{
			KeyValuePair<IList, BehaviorBase> keyValuePair = this.m_classSpecificContinuousBehavior.First<KeyValuePair<IList, BehaviorBase>>();
			IList key = keyValuePair.Key;
			BehaviorBase value = keyValuePair.Value;
			if (value.IsOverridable())
			{
				for (int k = 0; k < key.Count; k++)
				{
					BehaviorBase behaviorBase = key[k] as BehaviorBase;
					if ((isTick || behaviorBase.UpdateEveryFrame()) && key[k] != value && behaviorBase.OverrideOtherBehaviors())
					{
						this.EndClassSpecificContinuousBehavior(key);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x00185D68 File Offset: 0x00183F68
	private void UpdateBehaviors(bool isTick, bool onGlobalCooldown)
	{
		if (this.m_activeContinuousBehavior != null && this.m_classSpecificContinuousBehavior.Count > 1)
		{
			BraveUtility.Log("Trying to activate a class specific continuous behavior at the same time as a global continuous behavior; this isn't supported.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
		}
		if (this.m_activeContinuousBehavior != null)
		{
			if ((isTick || this.m_activeContinuousBehavior.UpdateEveryFrame()) && (!onGlobalCooldown || this.m_activeContinuousBehavior.IgnoreGlobalCooldown()))
			{
				ContinuousBehaviorResult continuousBehaviorResult = this.m_activeContinuousBehavior.ContinuousUpdate();
				if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
				{
					this.EndContinuousBehavior();
				}
			}
			return;
		}
		if (this.UpdateBehaviorClass(this.OverrideBehaviors, isTick, onGlobalCooldown) == BehaviorResult.SkipAllRemainingBehaviors)
		{
			return;
		}
		if (this.UpdateBehaviorClass(this.TargetBehaviors, isTick, onGlobalCooldown) == BehaviorResult.SkipAllRemainingBehaviors)
		{
			return;
		}
		if (!this.PreventMovement && this.UpdateBehaviorClass(this.MovementBehaviors, isTick, onGlobalCooldown) == BehaviorResult.SkipAllRemainingBehaviors)
		{
			return;
		}
		if (this.m_attackCooldownTimer <= 0f && this.UpdateBehaviorClass(this.AttackBehaviors, isTick, onGlobalCooldown) == BehaviorResult.SkipAllRemainingBehaviors)
		{
			return;
		}
		if (this.UpdateBehaviorClass(this.OtherBehaviors, isTick, onGlobalCooldown) == BehaviorResult.SkipAllRemainingBehaviors)
		{
			return;
		}
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x00185E78 File Offset: 0x00184078
	private BehaviorResult UpdateBehaviorClass(IList behaviors, bool isTick, bool onGlobalCooldown)
	{
		if (behaviors == null)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_classSpecificContinuousBehavior.ContainsKey(behaviors))
		{
			BehaviorBase behaviorBase = this.m_classSpecificContinuousBehavior[behaviors];
			if ((isTick || behaviorBase.UpdateEveryFrame()) && (!onGlobalCooldown || behaviorBase.IgnoreGlobalCooldown()))
			{
				ContinuousBehaviorResult continuousBehaviorResult = behaviorBase.ContinuousUpdate();
				if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
				{
					this.EndClassSpecificContinuousBehavior(behaviors);
				}
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		for (int i = 0; i < behaviors.Count; i++)
		{
			BehaviorBase behaviorBase2 = behaviors[i] as BehaviorBase;
			if ((isTick || behaviorBase2.UpdateEveryFrame()) && (!onGlobalCooldown || behaviorBase2.IgnoreGlobalCooldown()))
			{
				BehaviorResult behaviorResult = behaviorBase2.Update();
				if (behaviorResult == BehaviorResult.RunContinuous)
				{
					if (this.m_activeContinuousBehavior != null)
					{
						BraveUtility.Log("Trying to overwrite the current continuous behaviors; this shouldn't happen.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
					}
					this.m_activeContinuousBehavior = behaviorBase2;
					return BehaviorResult.SkipAllRemainingBehaviors;
				}
				if (behaviorResult == BehaviorResult.RunContinuousInClass)
				{
					if (this.m_classSpecificContinuousBehavior.ContainsKey(behaviors))
					{
						BraveUtility.Log("Trying to overwrite the current class continuous behaviors; this shouldn't happen.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
					}
					this.m_classSpecificContinuousBehavior[behaviors] = behaviorBase2;
					return BehaviorResult.SkipRemainingClassBehaviors;
				}
				if (behaviorResult == BehaviorResult.SkipAllRemainingBehaviors)
				{
					return BehaviorResult.SkipAllRemainingBehaviors;
				}
				if (behaviorResult == BehaviorResult.SkipRemainingClassBehaviors)
				{
					return BehaviorResult.SkipRemainingClassBehaviors;
				}
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x00185FAC File Offset: 0x001841AC
	private void EndContinuousBehavior()
	{
		if (this.m_activeContinuousBehavior != null)
		{
			BehaviorBase activeContinuousBehavior = this.m_activeContinuousBehavior;
			this.m_activeContinuousBehavior = null;
			activeContinuousBehavior.EndContinuousUpdate();
		}
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x00185FD8 File Offset: 0x001841D8
	private void EndClassSpecificContinuousBehavior(IList key)
	{
		BehaviorBase behaviorBase;
		if (this.m_classSpecificContinuousBehavior.TryGetValue(key, out behaviorBase))
		{
			this.m_classSpecificContinuousBehavior.Remove(key);
			behaviorBase.EndContinuousUpdate();
		}
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x0018600C File Offset: 0x0018420C
	private void RefreshBehaviors(IList behaviors, List<BehaviorBase> oldBehaviors)
	{
		for (int i = 0; i < behaviors.Count; i++)
		{
			if (!oldBehaviors.Contains(behaviors[i] as BehaviorBase))
			{
				BehaviorBase behaviorBase = behaviors[i] as BehaviorBase;
				behaviorBase.Init(base.gameObject, this.m_aiActor, base.aiShooter);
				behaviorBase.Start();
			}
			this.m_behaviors.Add(behaviors[i] as BehaviorBase);
		}
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x0018608C File Offset: 0x0018428C
	private void FirstUpdate()
	{
		if (!this.SkipTimingDifferentiator && base.aiActor && (!base.healthHaver || !base.healthHaver.IsBoss) && base.aiActor.ParentRoom != null)
		{
			int num = 0;
			List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null)
			{
				return;
			}
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i] && activeEnemies[i].EnemyGuid == base.aiActor.EnemyGuid)
				{
					num++;
					if (num == 1 && activeEnemies[i] == base.aiActor)
					{
						return;
					}
				}
			}
			if (num > 1)
			{
				float quickestCooldown = float.MaxValue;
				this.ProcessAttacks(delegate(AttackBehaviorBase attackBase)
				{
					BasicAttackBehavior basicAttackBehavior = attackBase as BasicAttackBehavior;
					if (attackBase is SequentialAttackBehaviorGroup)
					{
						SequentialAttackBehaviorGroup sequentialAttackBehaviorGroup = attackBase as SequentialAttackBehaviorGroup;
						basicAttackBehavior = sequentialAttackBehaviorGroup.AttackBehaviors[sequentialAttackBehaviorGroup.AttackBehaviors.Count - 1] as BasicAttackBehavior;
					}
					if (basicAttackBehavior != null)
					{
						if (basicAttackBehavior.CooldownVariance < 0.2f)
						{
							basicAttackBehavior.CooldownVariance = 0.2f;
						}
						float num2 = Mathf.Max(new float[] { basicAttackBehavior.Cooldown, basicAttackBehavior.GlobalCooldown, basicAttackBehavior.GroupCooldown, basicAttackBehavior.InitialCooldown });
						quickestCooldown = Mathf.Min(quickestCooldown, num2);
					}
				}, true);
				if (quickestCooldown < 3.4028235E+38f && !this.InstantFirstTick)
				{
					this.AttackCooldown = UnityEngine.Random.Range(0f, Mathf.Max(quickestCooldown, 4f));
				}
			}
		}
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x001861D4 File Offset: 0x001843D4
	private void ProcessAttacks(Action<AttackBehaviorBase> func, bool skipSimultaneous = false)
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.ProcessAttacksRecursive(this.AttackBehaviors[i], func, skipSimultaneous);
		}
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x00186214 File Offset: 0x00184414
	private void ProcessAttacksRecursive(AttackBehaviorBase attack, Action<AttackBehaviorBase> func, bool skipSimultaneous)
	{
		if (!(attack is IAttackBehaviorGroup))
		{
			func(attack);
			return;
		}
		IAttackBehaviorGroup attackBehaviorGroup = attack as IAttackBehaviorGroup;
		if (skipSimultaneous && attack is SimultaneousAttackBehaviorGroup)
		{
			return;
		}
		for (int i = 0; i < attackBehaviorGroup.Count; i++)
		{
			this.ProcessAttacksRecursive(attackBehaviorGroup.GetAttackBehavior(i), func, skipSimultaneous);
		}
	}

	// Token: 0x04003D1D RID: 15645
	public bool InstantFirstTick;

	// Token: 0x04003D1E RID: 15646
	public float TickInterval = 0.1f;

	// Token: 0x04003D1F RID: 15647
	public float PostAwakenDelay;

	// Token: 0x04003D20 RID: 15648
	public bool RemoveDelayOnReinforce;

	// Token: 0x04003D21 RID: 15649
	public bool OverrideStartingFacingDirection;

	// Token: 0x04003D22 RID: 15650
	[ShowInInspectorIf("OverrideStartingFacingDirection", false)]
	public float StartingFacingDirection = -90f;

	// Token: 0x04003D23 RID: 15651
	public bool SkipTimingDifferentiator;

	// Token: 0x04003D24 RID: 15652
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	[InspectorHeader("Behaviors")]
	public List<OverrideBehaviorBase> OverrideBehaviors;

	// Token: 0x04003D25 RID: 15653
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<TargetBehaviorBase> TargetBehaviors;

	// Token: 0x04003D26 RID: 15654
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<MovementBehaviorBase> MovementBehaviors;

	// Token: 0x04003D27 RID: 15655
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<AttackBehaviorBase> AttackBehaviors;

	// Token: 0x04003D28 RID: 15656
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<BehaviorBase> OtherBehaviors;

	// Token: 0x04003D29 RID: 15657
	private float m_localTimeScale = 1f;

	// Token: 0x04003D2A RID: 15658
	private GameActor m_playerTarget;

	// Token: 0x04003D2F RID: 15663
	private bool m_isFirstUpdate;

	// Token: 0x04003D30 RID: 15664
	private float m_postAwakenDelay;

	// Token: 0x04003D31 RID: 15665
	private float m_tickTimer;

	// Token: 0x04003D32 RID: 15666
	private float m_attackCooldownTimer;

	// Token: 0x04003D33 RID: 15667
	private float m_globalCooldownTimer;

	// Token: 0x04003D34 RID: 15668
	private float m_stunTimer;

	// Token: 0x04003D35 RID: 15669
	private tk2dSpriteAnimator m_extantStunVFX;

	// Token: 0x04003D36 RID: 15670
	private BraveDictionary<string, float> m_groupCooldownTimers;

	// Token: 0x04003D37 RID: 15671
	private float m_cooldownScale = 1f;

	// Token: 0x04003D38 RID: 15672
	private List<BehaviorBase> m_behaviors = new List<BehaviorBase>();

	// Token: 0x04003D39 RID: 15673
	private BehaviorBase m_activeContinuousBehavior;

	// Token: 0x04003D3A RID: 15674
	private Dictionary<IList, BehaviorBase> m_classSpecificContinuousBehavior = new Dictionary<IList, BehaviorBase>();

	// Token: 0x04003D3B RID: 15675
	private AIActor m_aiActor;
}
