using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB5 RID: 3509
[InspectorDropdownName("Bosses/GatlingGull/LeapBehaviour")]
public class GatlingGullLeapBehavior : BasicAttackBehavior
{
	// Token: 0x06004A5A RID: 19034 RVA: 0x0018E5BC File Offset: 0x0018C7BC
	public override void Start()
	{
		base.Start();
		this.m_sprite = this.m_gameObject.GetComponent<tk2dSprite>();
		this.m_specRigidbody = this.m_gameObject.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody specRigidbody = this.m_specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleMajorBreakableDestruction));
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0018E618 File Offset: 0x0018C818
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0018E620 File Offset: 0x0018C820
	protected void HandleMajorBreakableDestruction(CollisionData rigidbodyCollision)
	{
		MajorBreakable majorBreakable = rigidbodyCollision.OtherRigidbody.GetComponent<MajorBreakable>();
		if (majorBreakable == null)
		{
			majorBreakable = rigidbodyCollision.OtherRigidbody.GetComponentInParent<MajorBreakable>();
		}
		if (rigidbodyCollision.Overlap && majorBreakable != null)
		{
			majorBreakable.Break(Vector2.zero);
		}
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x0018E674 File Offset: 0x0018C874
	public override BehaviorResult Update()
	{
		base.Update();
		if (!this.m_animator)
		{
			this.m_animator = this.m_aiActor.spriteAnimator;
		}
		if (!this.m_shadowAnimator)
		{
			this.m_shadowAnimator = this.m_aiActor.ShadowObject.GetComponent<tk2dSpriteAnimator>();
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_startPosition = this.m_gameObject.transform.position;
		this.m_aiActor.ClearPath();
		this.m_state = GatlingGullLeapBehavior.LeapState.Jump;
		this.m_aiAnimator.enabled = false;
		tk2dSpriteAnimationClip clipByName = this.m_animator.GetClipByName("jump");
		this.m_animator.Play(clipByName, 0f, clipByName.fps * this.SpeedMultiplier, false);
		this.m_updateEveryFrame = true;
		this.m_offset = (this.m_aiActor.specRigidbody.UnitCenter.ToVector3ZUp(0f) - this.m_aiActor.transform.position).WithZ(0f);
		tk2dSpriteAnimator animator = this.m_animator;
		animator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(animator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		this.UpdateTargetPosition();
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0018E7C0 File Offset: 0x0018C9C0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		this.m_timer -= this.m_deltaTime * this.SpeedMultiplier;
		if (this.m_state == GatlingGullLeapBehavior.LeapState.Jump)
		{
			if (!this.m_animator.IsPlaying("jump"))
			{
				this.UpdateTargetPosition();
				this.m_totalAirTime = Mathf.Max(this.MinAirtime / this.SpeedMultiplier, (this.m_targetLandPosition - this.m_startPosition).magnitude / this.AirSpeed);
				this.m_timer = this.m_totalAirTime;
				this.m_state = GatlingGullLeapBehavior.LeapState.TrackFromAbove;
				this.m_specRigidbody.enabled = false;
				this.m_sprite.renderer.enabled = false;
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_sprite, false);
			}
		}
		else if (this.m_state == GatlingGullLeapBehavior.LeapState.TrackFromAbove)
		{
			this.UpdateTargetPosition();
			this.m_gameObject.transform.position = Vector3.Lerp(this.m_targetLandPosition, this.m_startPosition, Mathf.Clamp01(this.m_timer / this.m_totalAirTime));
			if (this.m_timer <= 0f)
			{
				this.m_state = GatlingGullLeapBehavior.LeapState.ShadowFall;
				AkSoundEngine.PostEvent("Play_ANM_gull_descend_01", this.m_gameObject);
				tk2dSpriteAnimationClip clipByName = this.m_shadowAnimator.GetClipByName("shadow_out");
				this.m_shadowAnimator.Play(clipByName, 0f, clipByName.fps * this.SpeedMultiplier, false);
			}
		}
		else if (this.m_state == GatlingGullLeapBehavior.LeapState.ShadowFall)
		{
			if (!this.m_shadowAnimator.IsPlaying("shadow_out"))
			{
				this.m_state = GatlingGullLeapBehavior.LeapState.Fall;
				this.m_gameObject.transform.position = this.m_targetLandPosition;
				this.m_specRigidbody.enabled = true;
				this.m_specRigidbody.Reinitialize();
				this.m_sprite.renderer.enabled = true;
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_sprite, true);
				tk2dSpriteAnimationClip clipByName2 = this.m_animator.GetClipByName("land");
				this.m_animator.Play(clipByName2, 0f, clipByName2.fps * this.SpeedMultiplier, false);
			}
		}
		else if (this.m_state == GatlingGullLeapBehavior.LeapState.Fall)
		{
			if (!this.m_animator.IsPlaying("land"))
			{
				this.m_state = GatlingGullLeapBehavior.LeapState.Smug;
				this.m_aiAnimator.enabled = true;
				if (this.ShouldSmug)
				{
					this.m_aiAnimator.PlayForDuration("smug", this.SmugTime, true, null, -1f, false);
					this.m_timer = this.SmugTime;
				}
			}
		}
		else if (this.m_state == GatlingGullLeapBehavior.LeapState.Smug && (this.m_timer <= 0f || !this.ShouldSmug))
		{
			this.ShouldSmug = false;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A5F RID: 19039 RVA: 0x0018EA78 File Offset: 0x0018CC78
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		tk2dSpriteAnimator animator = this.m_animator;
		animator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(animator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		this.UpdateCooldowns();
	}

	// Token: 0x06004A60 RID: 19040 RVA: 0x0018EAB4 File Offset: 0x0018CCB4
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A61 RID: 19041 RVA: 0x0018EAB8 File Offset: 0x0018CCB8
	private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventInfo == "start_shadow_animation")
		{
			this.m_shadowAnimator.Play("shadow_in");
		}
		else if (frame.eventInfo == "land_impact")
		{
			if (this.ImpactDustUp)
			{
				tk2dSprite component = SpawnManager.SpawnVFX(this.ImpactDustUp, false).GetComponent<tk2dSprite>();
				tk2dSprite component2 = this.m_aiActor.ShadowObject.GetComponent<tk2dSprite>();
				component.transform.position = this.m_targetLandPosition + this.m_offset;
				component2.AttachRenderer(component);
				component.HeightOffGround = 0.01f;
			}
			bool flag = false;
			SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
			if (targetRigidbody)
			{
				Vector2 vector = this.m_aiActor.TargetRigidbody.UnitCenter - this.m_aiActor.specRigidbody.UnitCenter;
				if (vector.magnitude < this.DamageRadius)
				{
					if (Mathf.Approximately(vector.magnitude, 0f))
					{
						vector = UnityEngine.Random.insideUnitCircle;
					}
					if (targetRigidbody.healthHaver)
					{
						targetRigidbody.healthHaver.ApplyDamage(this.Damage, vector.normalized, this.m_aiActor.GetActorName(), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
					}
					if (targetRigidbody.knockbackDoer)
					{
						targetRigidbody.knockbackDoer.ApplyKnockback(vector, this.Force, false);
					}
					targetRigidbody.RegisterGhostCollisionException(this.m_aiActor.specRigidbody);
					flag = true;
					this.ShouldSmug = true;
					GameManager.Instance.MainCameraController.DoScreenShake(this.HitScreenShake, new Vector2?(this.m_aiActor.specRigidbody.UnitCenter), false);
				}
			}
			if (!flag)
			{
				GameManager.Instance.MainCameraController.DoScreenShake(this.MissScreenShake, new Vector2?(this.m_aiActor.specRigidbody.UnitCenter), false);
			}
		}
	}

	// Token: 0x06004A62 RID: 19042 RVA: 0x0018ECB8 File Offset: 0x0018CEB8
	private void UpdateTargetPosition()
	{
		if (!this.m_aiActor.TargetRigidbody)
		{
			return;
		}
		Vector2 vector = ((this.OverridePosition == null) ? this.m_aiActor.TargetRigidbody.UnitCenter : this.OverridePosition.Value);
		Vector2 vector2 = this.m_aiActor.specRigidbody.UnitDimensions / 2f;
		Dungeon dungeon = GameManager.Instance.Dungeon;
		RoomHandler roomFromPosition = dungeon.data.GetRoomFromPosition(vector.ToIntVector2(VectorConversions.Floor));
		if (roomFromPosition != null)
		{
			Vector2 vector3 = roomFromPosition.area.basePosition.ToVector2() + vector2 + Vector2.one * PhysicsEngine.Instance.HalfPixelUnitWidth;
			Vector2 vector4 = (roomFromPosition.area.basePosition + roomFromPosition.area.dimensions).ToVector2() - vector2 - Vector2.one * PhysicsEngine.Instance.HalfPixelUnitWidth;
			vector = Vector2Extensions.Clamp(vector, vector3, vector4);
		}
		Vector2 vector5 = vector + new Vector2(-vector2.x, vector2.y);
		Vector2 vector6 = vector + new Vector2(vector2.x, vector2.y);
		Vector2 vector7 = vector + new Vector2(-vector2.x, -vector2.y);
		Vector2 vector8 = vector + new Vector2(vector2.x, -vector2.y);
		CellData cellData = dungeon.data[vector5.ToIntVector2(VectorConversions.Floor)];
		CellData cellData2 = dungeon.data[vector6.ToIntVector2(VectorConversions.Floor)];
		CellData cellData3 = dungeon.data[vector7.ToIntVector2(VectorConversions.Floor)];
		CellData cellData4 = dungeon.data[vector8.ToIntVector2(VectorConversions.Floor)];
		bool flag = cellData.type != CellType.FLOOR;
		bool flag2 = cellData2.type != CellType.FLOOR;
		bool flag3 = cellData3.type != CellType.FLOOR || cellData3.IsTopWall();
		bool flag4 = cellData4.type != CellType.FLOOR || cellData4.IsTopWall();
		int num = 0;
		if (flag)
		{
			num++;
		}
		if (flag2)
		{
			num++;
		}
		if (flag3)
		{
			num++;
		}
		if (flag4)
		{
			num++;
		}
		if (num == 1)
		{
			if (flag)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Down,
					IntVector2.Right
				});
			}
			if (flag2)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Down,
					IntVector2.Left
				});
			}
			if (flag3)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Up,
					IntVector2.Right
				});
			}
			if (flag4)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Up,
					IntVector2.Left
				});
			}
		}
		else if (num == 2)
		{
			if (flag3 && flag4)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[] { IntVector2.Up });
			}
			if (flag && flag2)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[] { IntVector2.Down });
			}
			if (flag2 && flag4)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[] { IntVector2.Left });
			}
			if (flag && flag3)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[] { IntVector2.Right });
			}
		}
		else if (num == 3)
		{
			if (!flag4)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Down,
					IntVector2.Right
				});
			}
			if (!flag3)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Down,
					IntVector2.Left
				});
			}
			if (!flag2)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Up,
					IntVector2.Right
				});
			}
			if (!flag)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Up,
					IntVector2.Left
				});
			}
		}
		else if (num == 4)
		{
			if (dungeon.data[vector7.ToIntVector2(VectorConversions.Floor) + new IntVector2(0, 2)].type == CellType.FLOOR)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Up,
					IntVector2.Up
				});
			}
			else if (dungeon.data[vector5.ToIntVector2(VectorConversions.Floor) + new IntVector2(2, 0)].type == CellType.FLOOR)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Right,
					IntVector2.Right
				});
			}
			else if (dungeon.data[vector6.ToIntVector2(VectorConversions.Floor) + new IntVector2(0, -2)].type == CellType.FLOOR)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Down,
					IntVector2.Down
				});
			}
			else if (dungeon.data[vector8.ToIntVector2(VectorConversions.Floor) + new IntVector2(-2, 0)].type == CellType.FLOOR)
			{
				this.AdjustTarget(ref vector, vector2, new IntVector2[]
				{
					IntVector2.Left,
					IntVector2.Left
				});
			}
		}
		this.m_targetLandPosition = vector.ToVector3ZUp(0f) - this.m_offset;
		this.m_targetLandPosition.z = this.m_targetLandPosition.y;
	}

	// Token: 0x06004A63 RID: 19043 RVA: 0x0018F370 File Offset: 0x0018D570
	private void AdjustTarget(ref Vector2 target, Vector2 extents, params IntVector2[] dir)
	{
		for (int i = 0; i < dir.Length; i++)
		{
			if (dir[i] == IntVector2.Up)
			{
				target.y = (float)((int)(target.y - extents.y) + 1) + extents.y + PhysicsEngine.Instance.PixelUnitWidth;
			}
			if (dir[i] == IntVector2.Down)
			{
				target.y = (float)((int)(target.y + extents.y)) - extents.y - PhysicsEngine.Instance.PixelUnitWidth;
			}
			if (dir[i] == IntVector2.Left)
			{
				target.x = (float)((int)(target.x + extents.x)) - extents.x - PhysicsEngine.Instance.PixelUnitWidth;
			}
			if (dir[i] == IntVector2.Right)
			{
				target.x = (float)((int)(target.x - extents.x) + 1) + extents.x + PhysicsEngine.Instance.PixelUnitWidth;
			}
		}
	}

	// Token: 0x04003F0D RID: 16141
	public float AirSpeed = 1f;

	// Token: 0x04003F0E RID: 16142
	public float MinAirtime = 0.8f;

	// Token: 0x04003F0F RID: 16143
	public float DamageRadius = 3f;

	// Token: 0x04003F10 RID: 16144
	public float Damage = 1f;

	// Token: 0x04003F11 RID: 16145
	public float Force = 1f;

	// Token: 0x04003F12 RID: 16146
	public ScreenShakeSettings HitScreenShake;

	// Token: 0x04003F13 RID: 16147
	public ScreenShakeSettings MissScreenShake;

	// Token: 0x04003F14 RID: 16148
	public GameObject ImpactDustUp;

	// Token: 0x04003F15 RID: 16149
	public float SmugTime = 1f;

	// Token: 0x04003F16 RID: 16150
	[HideInInspector]
	[NonSerialized]
	public bool ShouldSmug = true;

	// Token: 0x04003F17 RID: 16151
	[HideInInspector]
	[NonSerialized]
	public Vector2? OverridePosition;

	// Token: 0x04003F18 RID: 16152
	[HideInInspector]
	[NonSerialized]
	public float SpeedMultiplier = 1f;

	// Token: 0x04003F19 RID: 16153
	private Vector3 m_startPosition;

	// Token: 0x04003F1A RID: 16154
	private Vector3 m_targetLandPosition;

	// Token: 0x04003F1B RID: 16155
	private tk2dSprite m_sprite;

	// Token: 0x04003F1C RID: 16156
	private SpeculativeRigidbody m_specRigidbody;

	// Token: 0x04003F1D RID: 16157
	private Vector3 m_offset;

	// Token: 0x04003F1E RID: 16158
	private float m_timer;

	// Token: 0x04003F1F RID: 16159
	private float m_totalAirTime;

	// Token: 0x04003F20 RID: 16160
	private tk2dSpriteAnimator m_animator;

	// Token: 0x04003F21 RID: 16161
	private tk2dSpriteAnimator m_shadowAnimator;

	// Token: 0x04003F22 RID: 16162
	private GatlingGullLeapBehavior.LeapState m_state;

	// Token: 0x02000DB6 RID: 3510
	public enum LeapState
	{
		// Token: 0x04003F24 RID: 16164
		Jump,
		// Token: 0x04003F25 RID: 16165
		TrackFromAbove,
		// Token: 0x04003F26 RID: 16166
		ShadowFall,
		// Token: 0x04003F27 RID: 16167
		Fall,
		// Token: 0x04003F28 RID: 16168
		Smug
	}
}
