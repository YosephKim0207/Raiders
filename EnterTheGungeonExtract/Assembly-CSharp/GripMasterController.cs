using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001178 RID: 4472
public class GripMasterController : BraveBehaviour, IPlaceConfigurable, IHasDwarfConfigurables
{
	// Token: 0x17000EA1 RID: 3745
	// (set) Token: 0x0600633E RID: 25406 RVA: 0x00267B98 File Offset: 0x00265D98
	public bool IsAttacking
	{
		set
		{
			this.m_isAttacking = value;
			base.aiActor.IgnoreForRoomClear = this.Grip_EndOnEnemiesCleared && !this.m_shouldBecomeEnemy && !this.m_isAttacking;
		}
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x00267BD0 File Offset: 0x00265DD0
	public void Start()
	{
		base.specRigidbody.CollideWithOthers = false;
		base.aiActor.IsGone = true;
		if (this.Grip_StartAsEnemy)
		{
			this.m_shouldBecomeEnemy = true;
			this.End(true);
		}
		else
		{
			if (this.ShouldBecomeEnemy())
			{
				this.m_shouldBecomeEnemy = true;
			}
			base.aiActor.IgnoreForRoomClear = this.Grip_EndOnEnemiesCleared && !this.m_shouldBecomeEnemy;
		}
		if (this.Grip_EndAfterNumAttacks < 0 && !this.Grip_EndOnEnemiesCleared)
		{
			Debug.LogErrorFormat("Gripmaster was told to last forever! ({0})", new object[] { base.aiActor.ParentRoom.GetRoomName() });
			this.Grip_EndOnEnemiesCleared = true;
		}
		if (!this.m_isEnemy)
		{
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		}
	}

	// Token: 0x06006340 RID: 25408 RVA: 0x00267CA8 File Offset: 0x00265EA8
	public void Update()
	{
		if (!this.m_isEnemy)
		{
			if (base.healthHaver.IsAlive && base.aiAnimator.IsIdle())
			{
				if (this.Grip_EndAfterNumAttacks > 0 && this.m_numTimesAttacked >= this.Grip_EndAfterNumAttacks)
				{
					this.End(false);
				}
				if (this.Grip_EndOnEnemiesCleared)
				{
					if (this.m_shouldBecomeEnemy)
					{
						base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref this.m_activeEnemies);
						bool flag = false;
						for (int i = 0; i < this.m_activeEnemies.Count; i++)
						{
							if (this.m_activeEnemies[i] && !this.m_activeEnemies[i].healthHaver.PreventAllDamage)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.End(false);
						}
					}
					else if (base.aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) <= 0)
					{
						this.End(false);
					}
				}
			}
		}
		else
		{
			if (base.healthHaver.IsAlive && base.aiAnimator.IsIdle() && this.m_turnTimer <= 0f && base.aiActor.TargetRigidbody)
			{
				Vector2 vector = base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				DungeonData.Direction direction = DungeonData.GetCardinalFromVector2(vector);
				if (direction == DungeonData.Direction.NORTH)
				{
					direction = DungeonData.Direction.SOUTH;
				}
				if (this.m_facingDirection != direction)
				{
					string text;
					if (this.m_facingDirection == DungeonData.Direction.WEST)
					{
						text = ((direction != DungeonData.Direction.EAST) ? "red_trans_west_south" : "red_trans_west_east");
					}
					else if (this.m_facingDirection == DungeonData.Direction.SOUTH)
					{
						text = ((direction != DungeonData.Direction.WEST) ? "red_trans_south_east" : "red_trans_south_west");
					}
					else
					{
						text = ((direction != DungeonData.Direction.SOUTH) ? "red_trans_east_west" : "red_trans_east_south");
					}
					base.aiAnimator.PlayUntilFinished(text, false, null, -1f, false);
					base.aiAnimator.AnimatedFacingDirection = DungeonData.GetAngleFromDirection(direction);
					this.m_facingDirection = direction;
					this.m_turnTimer = 1f;
					base.behaviorSpeculator.AttackCooldown = Mathf.Max(base.behaviorSpeculator.AttackCooldown, base.aiAnimator.CurrentClipLength);
				}
			}
			this.m_turnTimer = Mathf.Max(0f, this.m_turnTimer - base.aiActor.LocalDeltaTime);
		}
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x00267F2C File Offset: 0x0026612C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006342 RID: 25410 RVA: 0x00267F34 File Offset: 0x00266134
	public void OnAttack()
	{
		this.m_numTimesAttacked++;
	}

	// Token: 0x06006343 RID: 25411 RVA: 0x00267F44 File Offset: 0x00266144
	public void ConfigureOnPlacement(RoomHandler room)
	{
		base.aiActor.IgnoreForRoomClear = this.Grip_EndOnEnemiesCleared;
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x00267F58 File Offset: 0x00266158
	public void End(bool skipAnim = false)
	{
		if (this.m_shouldBecomeEnemy)
		{
			base.healthHaver.PreventAllDamage = false;
			base.specRigidbody.CollideWithOthers = true;
			base.specRigidbody.PixelColliders[0].ManualOffsetY = 28;
			base.specRigidbody.ForceRegenerate(null, null);
			base.aiActor.IsGone = false;
			base.aiAnimator.IdleAnimation.Type = DirectionalAnimation.DirectionType.FourWayCardinal;
			base.aiAnimator.IdleAnimation.AnimNames = new string[] { "red_idle_south", "red_idle_east", "red_idle_south", "red_idle_west" };
			base.aiAnimator.IdleAnimation.Flipped = new DirectionalAnimation.FlipType[]
			{
				DirectionalAnimation.FlipType.None,
				DirectionalAnimation.FlipType.None,
				DirectionalAnimation.FlipType.None,
				DirectionalAnimation.FlipType.None
			};
			base.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "death").anim.Prefix = "red_die";
			base.aiAnimator.UseAnimatedFacingDirection = true;
			base.aiAnimator.FacingDirection = -90f;
			this.m_facingDirection = DungeonData.Direction.SOUTH;
			if (!skipAnim)
			{
				base.aiAnimator.PlayUntilFinished("transform", false, null, -1f, false);
				base.behaviorSpeculator.GlobalCooldown = Mathf.Max(base.behaviorSpeculator.AttackCooldown, base.aiAnimator.CurrentClipLength);
				this.m_turnTimer = base.aiAnimator.CurrentClipLength;
				base.aiActor.MoveToSafeSpot(base.aiAnimator.CurrentClipLength);
			}
			AttackBehaviorGroup attackBehaviorGroup = base.behaviorSpeculator.AttackBehaviorGroup;
			attackBehaviorGroup.AttackBehaviors[0].Probability = 0f;
			attackBehaviorGroup.AttackBehaviors[1].Probability = 1f;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
			this.m_isEnemy = true;
		}
		else
		{
			base.healthHaver.ApplyDamage(10000f, Vector2.zero, "Grip Master Finished", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
		}
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x00268178 File Offset: 0x00266378
	private bool ShouldBecomeEnemy()
	{
		if (!this.BecomeEnemeyPrereq.CheckConditionsFulfilled())
		{
			return false;
		}
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		return (parentRoom == null || (parentRoom.area.dimensions.x >= this.MinRoomWidth && parentRoom.area.dimensions.y >= this.MinRoomHeight)) && UnityEngine.Random.value < this.BecomeEnemyChance;
	}

	// Token: 0x04005EBA RID: 24250
	[DwarfConfigurable]
	public bool Grip_StartAsEnemy;

	// Token: 0x04005EBB RID: 24251
	[DwarfConfigurable]
	public bool Grip_EndOnEnemiesCleared = true;

	// Token: 0x04005EBC RID: 24252
	[DwarfConfigurable]
	public int Grip_EndAfterNumAttacks = -1;

	// Token: 0x04005EBD RID: 24253
	[DwarfConfigurable]
	public int Grip_OverrideRoomsToSendBackward = -1;

	// Token: 0x04005EBE RID: 24254
	[Header("Become Enemy Stuff")]
	public DungeonPrerequisite BecomeEnemeyPrereq;

	// Token: 0x04005EBF RID: 24255
	public float BecomeEnemyChance = 0.5f;

	// Token: 0x04005EC0 RID: 24256
	public int MinRoomWidth = 20;

	// Token: 0x04005EC1 RID: 24257
	public int MinRoomHeight = 15;

	// Token: 0x04005EC2 RID: 24258
	private bool m_isAttacking;

	// Token: 0x04005EC3 RID: 24259
	private bool m_shouldBecomeEnemy;

	// Token: 0x04005EC4 RID: 24260
	private int m_numTimesAttacked;

	// Token: 0x04005EC5 RID: 24261
	private bool m_isEnemy;

	// Token: 0x04005EC6 RID: 24262
	private DungeonData.Direction m_facingDirection;

	// Token: 0x04005EC7 RID: 24263
	private float m_turnTimer;

	// Token: 0x04005EC8 RID: 24264
	private List<AIActor> m_activeEnemies = new List<AIActor>();
}
