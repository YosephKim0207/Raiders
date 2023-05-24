using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D76 RID: 3446
public abstract class BasicAttackBehavior : AttackBehaviorBase
{
	// Token: 0x060048CB RID: 18635 RVA: 0x001843B4 File Offset: 0x001825B4
	private bool ShowGroupCooldown()
	{
		return this.GroupCooldown > 0f || !string.IsNullOrEmpty(this.GroupName);
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x001843D8 File Offset: 0x001825D8
	private bool ShowMinHealthThreshold()
	{
		return this.MinHealthThreshold != 0f;
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x001843EC File Offset: 0x001825EC
	private bool ShowMaxHealthThreshold()
	{
		return this.MaxHealthThreshold != 1f;
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x00184400 File Offset: 0x00182600
	private bool ShowHealthThresholds()
	{
		return this.HealthThresholds.Length > 0;
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x00184410 File Offset: 0x00182610
	private bool ShowTargetArea()
	{
		return this.targetAreaStyle != null;
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x00184420 File Offset: 0x00182620
	private bool ShowResetCooldownOnDamage()
	{
		return this.resetCooldownOnDamage != null;
	}

	// Token: 0x060048D1 RID: 18641 RVA: 0x00184430 File Offset: 0x00182630
	public override void Start()
	{
		base.Start();
		this.m_cooldownTimer = this.InitialCooldown;
		if (this.InitialCooldownVariance > 0f)
		{
			this.m_cooldownTimer += UnityEngine.Random.Range(-this.InitialCooldownVariance, this.InitialCooldownVariance);
		}
	}

	// Token: 0x060048D2 RID: 18642 RVA: 0x00184480 File Offset: 0x00182680
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_cooldownTimer, true);
		base.DecrementTimer(ref this.m_resetCooldownOnDamageCooldown, true);
		if (this.HealthThresholds.Length > 0)
		{
			float currentHealthPercentage = this.m_aiActor.healthHaver.GetCurrentHealthPercentage();
			if (currentHealthPercentage < this.m_lowestRecordedHealthPercentage)
			{
				for (int i = 0; i < this.HealthThresholds.Length; i++)
				{
					if (this.HealthThresholds[i] >= currentHealthPercentage && this.HealthThresholds[i] < this.m_lowestRecordedHealthPercentage)
					{
						this.m_healthThresholdCredits++;
					}
				}
				this.m_lowestRecordedHealthPercentage = currentHealthPercentage;
			}
		}
		if (BasicAttackBehavior.DrawDebugFiringArea)
		{
			if (Time.frameCount != BasicAttackBehavior.m_lastFrame)
			{
				BasicAttackBehavior.m_arcCount = 0;
				BasicAttackBehavior.m_lastFrame = Time.frameCount;
			}
			if (this.m_aiActor.TargetRigidbody && this.targetAreaStyle != null)
			{
				this.targetAreaStyle.DrawDebugLines(this.GetOrigin(this.targetAreaStyle.targetAreaOrigin), this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox), this.m_aiActor);
			}
		}
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x001845A4 File Offset: 0x001827A4
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x001845C4 File Offset: 0x001827C4
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		this.m_behaviorSpeculator = gameObject.GetComponent<BehaviorSpeculator>();
		if (this.resetCooldownOnDamage != null)
		{
			this.m_aiActor.healthHaver.OnDamaged += this.OnDamaged;
		}
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x00184604 File Offset: 0x00182804
	public override bool IsReady()
	{
		if (this.MinHealthThreshold > 0f && this.m_aiActor.healthHaver.GetCurrentHealthPercentage() < this.MinHealthThreshold)
		{
			return false;
		}
		if (this.MaxHealthThreshold < 1f && this.m_aiActor.healthHaver.GetCurrentHealthPercentage() > this.MaxHealthThreshold)
		{
			return false;
		}
		if (this.HealthThresholds.Length > 0 && this.m_healthThresholdCredits <= 0)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(this.GroupName) && this.m_behaviorSpeculator.GetGroupCooldownTimer(this.GroupName) > 0f)
		{
			return false;
		}
		if (this.IsBlackPhantom && !this.m_aiActor.IsBlackPhantom)
		{
			return false;
		}
		if (this.MinRange > 0f)
		{
			if (!this.m_aiActor.TargetRigidbody)
			{
				return false;
			}
			Vector2 unitCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, unitCenter);
			if (num < this.MinRange)
			{
				return false;
			}
		}
		if (this.Range > 0f)
		{
			if (!this.m_aiActor.TargetRigidbody)
			{
				return false;
			}
			Vector2 unitCenter2 = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			float num2 = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, unitCenter2);
			if (num2 > this.Range)
			{
				return false;
			}
		}
		if (this.MinWallDistance > 0f)
		{
			PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
			CellArea area = this.m_aiActor.ParentRoom.area;
			if (hitboxPixelCollider.UnitLeft - area.UnitLeft < this.MinWallDistance)
			{
				return false;
			}
			if (area.UnitRight - hitboxPixelCollider.UnitRight < this.MinWallDistance)
			{
				return false;
			}
			if (hitboxPixelCollider.UnitBottom - area.UnitBottom < this.MinWallDistance)
			{
				return false;
			}
			if (area.UnitTop - hitboxPixelCollider.UnitTop < this.MinWallDistance)
			{
				return false;
			}
		}
		return (this.MaxEnemiesInRoom <= 0f || (float)this.m_aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= this.MaxEnemiesInRoom) && this.TargetInFiringArea() && (!this.RequiresLineOfSight || this.TargetInLineOfSight()) && (this.MaxUsages <= 0 || this.m_numTimesUsed < this.MaxUsages) && this.m_cooldownTimer <= 0f;
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x001848B4 File Offset: 0x00182AB4
	public override float GetMinReadyRange()
	{
		if (this.Range > 0f)
		{
			return (!this.IsReady()) ? (-1f) : this.Range;
		}
		return -1f;
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x001848E8 File Offset: 0x00182AE8
	public override float GetMaxRange()
	{
		if (this.Range > 0f)
		{
			return this.Range;
		}
		return -1f;
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x00184908 File Offset: 0x00182B08
	public override void OnActorPreDeath()
	{
		if (this.resetCooldownOnDamage != null)
		{
			this.m_aiActor.healthHaver.OnDamaged -= this.OnDamaged;
		}
		base.OnActorPreDeath();
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x00184938 File Offset: 0x00182B38
	protected virtual void UpdateCooldowns()
	{
		this.m_cooldownTimer = this.Cooldown;
		if (this.CooldownVariance > 0f)
		{
			this.m_cooldownTimer += UnityEngine.Random.Range(-this.CooldownVariance, this.CooldownVariance);
		}
		if (this.AttackCooldown > 0f)
		{
			this.m_behaviorSpeculator.AttackCooldown = this.AttackCooldown;
		}
		if (this.GlobalCooldown > 0f)
		{
			this.m_behaviorSpeculator.GlobalCooldown = this.GlobalCooldown;
		}
		if (this.GroupCooldown > 0f)
		{
			this.m_behaviorSpeculator.SetGroupCooldown(this.GroupName, this.GroupCooldown);
		}
		if (this.HealthThresholds.Length > 0 && this.m_healthThresholdCredits > 0)
		{
			this.m_healthThresholdCredits = ((!this.AccumulateHealthThresholds) ? 0 : (this.m_healthThresholdCredits - 1));
		}
		this.m_numTimesUsed++;
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x00184A30 File Offset: 0x00182C30
	protected virtual Vector2 GetOrigin(ShootBehavior.TargetAreaOrigin origin)
	{
		if (origin == ShootBehavior.TargetAreaOrigin.ShootPoint)
		{
			Debug.LogWarning("ColliderType.ShootPoint is not supported for base BasicAttackBehaviors!");
		}
		return this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x00184A54 File Offset: 0x00182C54
	protected bool TargetInLineOfSight()
	{
		return this.m_aiActor.HasLineOfSightToTarget;
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x00184A64 File Offset: 0x00182C64
	protected bool TargetInFiringArea()
	{
		return this.targetAreaStyle == null || (this.m_aiActor.TargetRigidbody && this.targetAreaStyle.TargetInFiringArea(this.GetOrigin(this.targetAreaStyle.targetAreaOrigin), this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox)));
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x00184AC4 File Offset: 0x00182CC4
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.resetCooldownOnDamage != null && this.m_resetCooldownOnDamageCooldown <= 0f)
		{
			bool flag = false;
			if (this.resetCooldownOnDamage.Cooldown && this.m_cooldownTimer > 0f)
			{
				this.m_cooldownTimer = 0f;
				flag = true;
			}
			if (this.resetCooldownOnDamage.AttackCooldown && this.m_behaviorSpeculator.AttackCooldown > 0f)
			{
				this.m_behaviorSpeculator.AttackCooldown = 0f;
				flag = true;
			}
			if (this.resetCooldownOnDamage.GlobalCooldown && this.m_behaviorSpeculator.GlobalCooldown > 0f)
			{
				this.m_behaviorSpeculator.GlobalCooldown = 0f;
				flag = true;
			}
			if (this.resetCooldownOnDamage.GroupCooldown && this.m_behaviorSpeculator.GetGroupCooldownTimer(this.GroupName) > 0f)
			{
				this.m_behaviorSpeculator.SetGroupCooldown(this.GroupName, 0f);
				flag = true;
			}
			if (flag && this.resetCooldownOnDamage.ResetCooldown > 0f)
			{
				this.m_resetCooldownOnDamageCooldown = this.resetCooldownOnDamage.ResetCooldown;
			}
		}
	}

	// Token: 0x04003CEA RID: 15594
	public static bool DrawDebugFiringArea;

	// Token: 0x04003CEB RID: 15595
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Time before THIS behavior may be run again.")]
	public float Cooldown = 1f;

	// Token: 0x04003CEC RID: 15596
	[InspectorTooltip("Time variance added to the base cooldown.")]
	[InspectorCategory("Conditions")]
	public float CooldownVariance;

	// Token: 0x04003CED RID: 15597
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Time before ATTACK behaviors may be run again.")]
	public float AttackCooldown;

	// Token: 0x04003CEE RID: 15598
	[InspectorTooltip("Time before ANY behavior may be run again.")]
	[InspectorCategory("Conditions")]
	public float GlobalCooldown;

	// Token: 0x04003CEF RID: 15599
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Time after the enemy becomes active before this attack can be used for the first time.")]
	public float InitialCooldown;

	// Token: 0x04003CF0 RID: 15600
	[InspectorTooltip("Time variance added to the initial cooldown.")]
	[InspectorCategory("Conditions")]
	public float InitialCooldownVariance;

	// Token: 0x04003CF1 RID: 15601
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Name of the cooldown group to use; all behaviors on this BehaviorSpeculator with a matching group will use this cooldown value.")]
	[InspectorShowIf("ShowGroupCooldown")]
	public string GroupName;

	// Token: 0x04003CF2 RID: 15602
	[InspectorTooltip("Time before any behaviors with a matching group name may be run again.")]
	[InspectorShowIf("ShowGroupCooldown")]
	[InspectorCategory("Conditions")]
	public float GroupCooldown;

	// Token: 0x04003CF3 RID: 15603
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Minimum range")]
	public float MinRange;

	// Token: 0x04003CF4 RID: 15604
	[InspectorTooltip("Range")]
	[InspectorCategory("Conditions")]
	public float Range;

	// Token: 0x04003CF5 RID: 15605
	[InspectorTooltip("Minimum distance from a wall")]
	[InspectorCategory("Conditions")]
	public float MinWallDistance;

	// Token: 0x04003CF6 RID: 15606
	[InspectorCategory("Conditions")]
	[InspectorTooltip("If the room contains more than this number of enemies, this attack wont be used.")]
	public float MaxEnemiesInRoom;

	// Token: 0x04003CF7 RID: 15607
	[InspectorShowIf("ShowMinHealthThreshold")]
	[InspectorCategory("Conditions")]
	[InspectorTooltip("The minimum amount of health an enemy can have and still use this attack.\n(Raising this means the enemy wont use this attack at low health)")]
	public float MinHealthThreshold;

	// Token: 0x04003CF8 RID: 15608
	[InspectorTooltip("The maximum amount of health an enemy can have and still use this attack.\n(Lowering this means the enemy wont use this attack until they lose health)")]
	[InspectorShowIf("ShowMaxHealthThreshold")]
	[InspectorCategory("Conditions")]
	public float MaxHealthThreshold = 1f;

	// Token: 0x04003CF9 RID: 15609
	[InspectorCategory("Conditions")]
	[InspectorTooltip("The attack can only be used once each time a new health threshold is met")]
	[InspectorShowIf("ShowHealthThresholds")]
	public float[] HealthThresholds = new float[0];

	// Token: 0x04003CFA RID: 15610
	[InspectorTooltip("If true, the attack can build up multiple uses by passing multiple thresholds in quick succession")]
	[InspectorCategory("Conditions")]
	[InspectorShowIf("ShowHealthThresholds")]
	public bool AccumulateHealthThresholds = true;

	// Token: 0x04003CFB RID: 15611
	[InspectorCategory("Conditions")]
	[InspectorShowIf("ShowTargetArea")]
	public ShootBehavior.FiringAreaStyle targetAreaStyle;

	// Token: 0x04003CFC RID: 15612
	[InspectorCategory("Conditions")]
	[InspectorTooltip("The attack can only be used for Black Phantom versions of this enemy")]
	public bool IsBlackPhantom;

	// Token: 0x04003CFD RID: 15613
	[InspectorShowIf("ShowResetCooldownOnDamage")]
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Resets the appropriate cooldowns when the actor takes damage.")]
	[InspectorNullable]
	public BasicAttackBehavior.ResetCooldownOnDamage resetCooldownOnDamage;

	// Token: 0x04003CFE RID: 15614
	[InspectorCategory("Conditions")]
	[InspectorTooltip("Require line of sight to target. Expensive! Use for companions.")]
	public bool RequiresLineOfSight;

	// Token: 0x04003CFF RID: 15615
	[InspectorTooltip("This attack can only be used this number of times.")]
	[InspectorCategory("Conditions")]
	public int MaxUsages;

	// Token: 0x04003D00 RID: 15616
	protected float m_cooldownTimer;

	// Token: 0x04003D01 RID: 15617
	protected float m_resetCooldownOnDamageCooldown;

	// Token: 0x04003D02 RID: 15618
	protected BehaviorSpeculator m_behaviorSpeculator;

	// Token: 0x04003D03 RID: 15619
	protected int m_healthThresholdCredits;

	// Token: 0x04003D04 RID: 15620
	protected float m_lowestRecordedHealthPercentage = float.MaxValue;

	// Token: 0x04003D05 RID: 15621
	protected int m_numTimesUsed;

	// Token: 0x04003D06 RID: 15622
	protected static int m_arcCount;

	// Token: 0x04003D07 RID: 15623
	protected static int m_lastFrame;

	// Token: 0x02000D77 RID: 3447
	public class ResetCooldownOnDamage
	{
		// Token: 0x04003D08 RID: 15624
		public bool Cooldown = true;

		// Token: 0x04003D09 RID: 15625
		public bool AttackCooldown;

		// Token: 0x04003D0A RID: 15626
		public bool GlobalCooldown;

		// Token: 0x04003D0B RID: 15627
		public bool GroupCooldown;

		// Token: 0x04003D0C RID: 15628
		[InspectorTooltip("If set, cooldowns can not be reset again for this amount of time after taking damage.")]
		public float ResetCooldown;
	}
}
