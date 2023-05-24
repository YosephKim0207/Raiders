using System;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D1B RID: 3355
public class ConsumeTargetBehavior : BasicAttackBehavior
{
	// Token: 0x060046CE RID: 18126 RVA: 0x00170DB0 File Offset: 0x0016EFB0
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		this.m_startMoveSpeed = this.m_aiActor.MovementSpeed;
		this.m_posOffset = -(this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.transform.position.XY());
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x00170E38 File Offset: 0x0016F038
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x00170E5C File Offset: 0x0016F05C
	public override BehaviorResult Update()
	{
		base.Update();
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (this.m_behaviorSpeculator.TargetRigidbody == null)
		{
			return BehaviorResult.Continue;
		}
		this.m_state = ConsumeTargetBehavior.State.Tell;
		this.m_aiAnimator.PlayUntilCancelled(this.TellAnim, false, null, -1f, false);
		this.m_aiActor.ClearPath();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x00170ECC File Offset: 0x0016F0CC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == ConsumeTargetBehavior.State.Tell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.TellAnim))
			{
				AkSoundEngine.PostEvent("Play_ENM_Tarnisher_Seeking_01", this.m_aiActor.gameObject);
				this.m_state = ConsumeTargetBehavior.State.Path;
				this.m_aiAnimator.PlayUntilCancelled(this.MoveAnim, false, null, -1f, false);
				this.m_aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "pitfall_attack";
				this.m_timer = this.MaxPathTime;
				this.m_repathTimer = 0f;
				this.m_aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(this.PathMoveSpeed);
				this.SetPlayerCollision(false);
				this.Flatten(true);
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_state == ConsumeTargetBehavior.State.Path)
		{
			float num = this.PathToTarget();
			if (num < 1.5f)
			{
				this.m_state = ConsumeTargetBehavior.State.Track;
				this.m_timer = this.TrackTime;
				if (this.m_behaviorSpeculator.TargetRigidbody)
				{
					this.m_trackOffset = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
					this.m_targetPosition = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
				}
				else
				{
					this.m_trackOffset = Vector2.zero;
					this.m_targetPosition = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
				}
			}
			else if (this.m_timer <= 0f)
			{
				this.m_state = ConsumeTargetBehavior.State.GrabBegin;
				this.m_aiActor.ClearPath();
				this.m_aiAnimator.PlayUntilFinished(this.GrabAnim, false, null, -1f, false);
				this.m_trackDuringGrab = false;
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_state == ConsumeTargetBehavior.State.Track)
		{
			this.TrackToTarget(Vector2.Lerp(Vector2.zero, this.m_trackOffset, this.m_timer / this.TrackTime));
			if (this.m_timer <= 0f)
			{
				this.m_state = ConsumeTargetBehavior.State.GrabBegin;
				this.m_aiActor.ClearPath();
				this.m_aiAnimator.PlayUntilFinished(this.GrabAnim, false, null, -1f, false);
				this.m_aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "pitfall";
				this.m_trackDuringGrab = true;
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_state == ConsumeTargetBehavior.State.GrabBegin)
		{
			if (this.m_trackDuringGrab)
			{
				this.TrackToTarget(Vector2.zero);
			}
			if (!this.m_aiAnimator.IsPlaying(this.GrabAnim))
			{
				this.GetSafeEndPoint();
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == ConsumeTargetBehavior.State.GrabSuccess)
		{
			if (!this.m_aiAnimator.IsPlaying(this.GrabAnim))
			{
				this.UnconsumePlayer(true);
				return ContinuousBehaviorResult.Finished;
			}
			this.TrackToSafeEndPoint();
		}
		else if (this.m_state == ConsumeTargetBehavior.State.Miss)
		{
			if (!this.m_aiAnimator.IsPlaying(this.MissAnim))
			{
				return ContinuousBehaviorResult.Finished;
			}
			this.TrackToSafeEndPoint();
		}
		else if (this.m_state == ConsumeTargetBehavior.State.WaitingForFinish)
		{
			if (!this.m_behaviorSpeculator.TargetRigidbody || (!this.m_behaviorSpeculator.TargetRigidbody.GroundPixelCollider.Overlaps(this.m_aiActor.specRigidbody.GroundPixelCollider) && !this.m_behaviorSpeculator.TargetRigidbody.GroundPixelCollider.Overlaps(this.m_aiActor.specRigidbody.HitboxPixelCollider)))
			{
				this.m_aiAnimator.PlayUntilFinished(this.GrabFinishAnim, false, null, -1f, false);
				this.m_state = ConsumeTargetBehavior.State.GrabFinish;
				this.Flatten(false);
			}
		}
		else if (this.m_state == ConsumeTargetBehavior.State.GrabFinish && !this.m_aiAnimator.IsPlaying(this.GrabFinishAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x001712D0 File Offset: 0x0016F4D0
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_affectedPlayer != null)
		{
			this.UnconsumePlayer(false);
		}
		this.m_state = ConsumeTargetBehavior.State.Idle;
		this.SetPlayerCollision(true);
		if (this.m_aiActor)
		{
			this.m_aiActor.specRigidbody.ClearSpecificCollisionExceptions();
		}
		this.m_aiActor.MovementSpeed = this.m_startMoveSpeed;
		this.Flatten(false);
		this.m_aiActor.knockbackDoer.SetImmobile(false, "ConsumeTargetBehavior");
		Vector2? resetEndPos = this.m_resetEndPos;
		if (resetEndPos != null)
		{
			this.m_aiActor.transform.position = this.m_resetEndPos.Value;
			this.m_aiActor.specRigidbody.Reinitialize();
		}
		this.m_resetStartPos = null;
		this.m_resetEndPos = null;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x001713C8 File Offset: 0x0016F5C8
	public override void Destroy()
	{
		if (this.m_affectedPlayer)
		{
			this.UnconsumePlayer(false);
		}
		base.Destroy();
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x001713E8 File Offset: 0x0016F5E8
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (this.m_state == ConsumeTargetBehavior.State.GrabBegin && frame.eventInfo == "hit")
		{
			this.ForceBlank(5f, 0.65f);
			if (this.m_behaviorSpeculator.TargetRigidbody)
			{
				PlayerController playerController = this.m_behaviorSpeculator.TargetRigidbody.gameActor as PlayerController;
				if (playerController)
				{
					this.m_aiActor.specRigidbody.RegisterSpecificCollisionException(playerController.specRigidbody);
					if (playerController.CanBeGrabbed && playerController.specRigidbody.HitboxPixelCollider.Overlaps(this.m_aiActor.specRigidbody.HitboxPixelCollider))
					{
						this.ConsumePlayer(playerController);
						this.m_state = ConsumeTargetBehavior.State.GrabSuccess;
					}
				}
			}
			this.Flatten(false);
			this.m_aiActor.knockbackDoer.SetImmobile(true, "ConsumeTargetBehavior");
			if (this.m_state != ConsumeTargetBehavior.State.GrabSuccess)
			{
				this.m_state = ConsumeTargetBehavior.State.Miss;
				this.m_aiAnimator.PlayUntilFinished(this.MissAnim, false, null, -1f, false);
			}
			this.GetSafeEndPoint();
		}
		if ((this.m_state == ConsumeTargetBehavior.State.GrabSuccess || this.m_state == ConsumeTargetBehavior.State.Miss) && frame.eventInfo == "enable_colliders")
		{
			this.SetPlayerCollision(true);
			this.Flatten(false);
		}
		if (this.m_state == ConsumeTargetBehavior.State.GrabSuccess && frame.eventInfo == "release")
		{
			this.UnconsumePlayer(true);
			this.Flatten(true);
		}
		if (this.m_state == ConsumeTargetBehavior.State.GrabSuccess && frame.eventInfo == "static")
		{
			this.m_state = ConsumeTargetBehavior.State.WaitingForFinish;
		}
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x001715A0 File Offset: 0x0016F7A0
	private void SetPlayerCollision(bool collision)
	{
		if (collision)
		{
			this.m_aiActor.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox));
		}
		else
		{
			this.m_aiActor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox));
		}
	}

	// Token: 0x060046D6 RID: 18134 RVA: 0x001715DC File Offset: 0x0016F7DC
	private void Flatten(bool flatten)
	{
		this.m_aiActor.sprite.IsPerpendicular = !flatten;
		this.m_aiActor.specRigidbody.CollideWithOthers = !flatten;
		this.m_aiActor.IsGone = flatten;
		if (flatten)
		{
			this.m_startingHeightOffGround = this.m_aiActor.sprite.HeightOffGround;
			this.m_aiActor.sprite.HeightOffGround = -1.5f;
			this.m_aiActor.sprite.UpdateZDepth();
		}
		else
		{
			this.m_aiActor.sprite.HeightOffGround = this.m_startingHeightOffGround;
			this.m_aiActor.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x0017168C File Offset: 0x0016F88C
	private float PathToTarget()
	{
		if (this.m_behaviorSpeculator.TargetRigidbody)
		{
			if (this.m_repathTimer <= 0f)
			{
				this.m_aiActor.PathfindToPosition(this.m_behaviorSpeculator.TargetRigidbody.UnitCenter, null, true, null, null, null, true);
				this.m_repathTimer = this.PathInterval;
			}
			return Vector2.Distance(this.m_behaviorSpeculator.TargetRigidbody.HitboxPixelCollider.UnitCenter, this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter);
		}
		return -1f;
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x00171734 File Offset: 0x0016F934
	private void TrackToTarget(Vector2 additionalOffset)
	{
		if (this.m_behaviorSpeculator.TargetRigidbody)
		{
			Vector2 unitCenter = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
			this.m_targetPosition = Vector2.MoveTowards(this.m_targetPosition, unitCenter, 10f * this.m_deltaTime);
			this.m_aiActor.transform.position = this.m_targetPosition + this.m_posOffset + additionalOffset;
			this.m_aiActor.specRigidbody.Reinitialize();
		}
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x001717C4 File Offset: 0x0016F9C4
	private void TrackToSafeEndPoint()
	{
		Vector2? resetStartPos = this.m_resetStartPos;
		if (resetStartPos != null)
		{
			Vector2? resetEndPos = this.m_resetEndPos;
			if (resetEndPos != null)
			{
				this.m_aiActor.transform.position = Vector2.Lerp(this.m_resetStartPos.Value, this.m_resetEndPos.Value, this.m_aiAnimator.CurrentClipProgress);
				this.m_aiActor.specRigidbody.Reinitialize();
			}
		}
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x00171844 File Offset: 0x0016FA44
	private void GetSafeEndPoint()
	{
		if (!GameManager.HasInstance || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		Vector2[] array = new Vector2[] { specRigidbody.UnitBottomLeft, specRigidbody.UnitBottomCenter, specRigidbody.UnitBottomRight, specRigidbody.UnitTopLeft, specRigidbody.UnitTopCenter, specRigidbody.UnitTopRight };
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			IntVector2 intVector = array[i].ToIntVector2(VectorConversions.Floor);
			if (!data.CheckInBoundsAndValid(intVector) || data.isWall(intVector) || data.isTopWall(intVector.x, intVector.y) || data[intVector].isOccupied)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int j = 0; j < this.m_aiActor.Clearance.x; j++)
			{
				int num = c.x + j;
				for (int k = 0; k < this.m_aiActor.Clearance.y; k++)
				{
					int num2 = c.y + k;
					if (GameManager.Instance.Dungeon.data.isTopWall(num, num2))
					{
						return false;
					}
				}
			}
			return true;
		};
		Vector2 vector = this.m_aiActor.specRigidbody.UnitBottomCenter - this.m_aiActor.transform.position.XY();
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		IntVector2? nearestAvailableCell = absoluteRoomFromPosition.GetNearestAvailableCell(this.m_aiActor.specRigidbody.UnitCenter, new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		if (nearestAvailableCell != null)
		{
			this.m_resetStartPos = new Vector2?(this.m_aiActor.transform.position);
			this.m_resetEndPos = new Vector2?(Pathfinder.GetClearanceOffset(nearestAvailableCell.Value, this.m_aiActor.Clearance).WithY((float)nearestAvailableCell.Value.y) - vector);
		}
		else
		{
			this.m_resetStartPos = null;
			this.m_resetEndPos = null;
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x00171AB4 File Offset: 0x0016FCB4
	private void ConsumePlayer(PlayerController player)
	{
		player.specRigidbody.Velocity = Vector2.zero;
		player.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
		player.ToggleRenderer(false, "consumed");
		player.ToggleHandRenderers(false, "consumed");
		player.ToggleGunRenderers(false, "consumed");
		player.CurrentInputState = PlayerInputState.NoInput;
		player.healthHaver.IsVulnerable = false;
		this.m_affectedPlayer = player;
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x00171B20 File Offset: 0x0016FD20
	private void UnconsumePlayer(bool punishPlayer)
	{
		if (this.m_affectedPlayer)
		{
			this.m_affectedPlayer.healthHaver.IsVulnerable = true;
			if (punishPlayer)
			{
				this.PunishPlayer();
			}
		}
		if (this.m_affectedPlayer)
		{
			this.m_affectedPlayer.ToggleRenderer(true, "consumed");
			this.m_affectedPlayer.ToggleHandRenderers(true, "consumed");
			this.m_affectedPlayer.ToggleGunRenderers(true, "consumed");
			this.m_affectedPlayer.CurrentInputState = PlayerInputState.AllInput;
			this.m_affectedPlayer.DoSpitOut();
		}
		this.m_affectedPlayer = null;
		if (this.m_aiActor)
		{
			this.m_aiActor.specRigidbody.ClearSpecificCollisionExceptions();
		}
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x00171BDC File Offset: 0x0016FDDC
	private void PunishPlayer()
	{
		if (!this.m_affectedPlayer || !this.m_aiActor)
		{
			return;
		}
		if (this.m_affectedPlayer.HasActiveItem(GlobalItemIds.EitrShield) && PickupObjectDatabase.HasInstance && this.m_aiActor.AdditionalSafeItemDrops != null)
		{
			this.m_affectedPlayer.RemoveActiveItem(GlobalItemIds.EitrShield);
			this.m_aiActor.AdditionalSafeItemDrops.Add(PickupObjectDatabase.Instance.InternalGetById(GlobalItemIds.EitrShield));
			return;
		}
		if (this.m_affectedPlayer.healthHaver)
		{
			this.m_affectedPlayer.healthHaver.ApplyDamage((!this.m_aiActor.IsBlackPhantom) ? 0.5f : 1f, Vector2.zero, this.m_aiActor.GetActorName(), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
		}
		if (this.m_affectedPlayer && this.m_affectedPlayer.ownerlessStatModifiers != null && this.m_affectedPlayer.stats != null)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.TarnisherClipCapacityMultiplier;
			statModifier.amount = -this.PlayerClipSizePenalty;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			this.m_affectedPlayer.ownerlessStatModifiers.Add(statModifier);
			this.m_affectedPlayer.stats.RecalculateStats(this.m_affectedPlayer, false, false);
		}
		if (this.m_affectedPlayer && this.m_affectedPlayer.CurrentGun && this.m_affectedPlayer.CurrentGun.ammo > 0)
		{
			this.m_affectedPlayer.CurrentGun.ammo = Mathf.RoundToInt((float)this.m_affectedPlayer.CurrentGun.ammo * 0.85f);
		}
		if (GameStatsManager.HasInstance)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_TARNISHED, 1f);
		}
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x00171DC8 File Offset: 0x0016FFC8
	public void ForceBlank(float overrideRadius = 5f, float overrideTimeAtMaxRadius = 0.65f)
	{
		if (!this.m_aiActor || !this.m_aiActor.specRigidbody)
		{
			return;
		}
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		silencerInstance.ForceNoDamage = true;
		silencerInstance.TriggerSilencer(this.m_aiActor.specRigidbody.UnitCenter, 50f, overrideRadius, null, 0f, 0f, 0f, 0f, 0f, 0f, overrideTimeAtMaxRadius, null, false, true);
	}

	// Token: 0x04003987 RID: 14727
	public float PathInterval = 0.2f;

	// Token: 0x04003988 RID: 14728
	public float PathMoveSpeed = 7f;

	// Token: 0x04003989 RID: 14729
	public float MaxPathTime = 3f;

	// Token: 0x0400398A RID: 14730
	public float TrackTime = 0.25f;

	// Token: 0x0400398B RID: 14731
	public float PlayerClipSizePenalty = 0.15f;

	// Token: 0x0400398C RID: 14732
	[InspectorCategory("Visuals")]
	public string TellAnim;

	// Token: 0x0400398D RID: 14733
	[InspectorCategory("Visuals")]
	public string MoveAnim;

	// Token: 0x0400398E RID: 14734
	[InspectorCategory("Visuals")]
	public string GrabAnim;

	// Token: 0x0400398F RID: 14735
	[InspectorCategory("Visuals")]
	public string MissAnim;

	// Token: 0x04003990 RID: 14736
	[InspectorCategory("Visuals")]
	public string GrabFinishAnim;

	// Token: 0x04003991 RID: 14737
	private float m_startingHeightOffGround;

	// Token: 0x04003992 RID: 14738
	private float m_startMoveSpeed;

	// Token: 0x04003993 RID: 14739
	private Vector2 m_posOffset;

	// Token: 0x04003994 RID: 14740
	private Vector2 m_targetPosition;

	// Token: 0x04003995 RID: 14741
	private float m_repathTimer;

	// Token: 0x04003996 RID: 14742
	private float m_timer;

	// Token: 0x04003997 RID: 14743
	private Vector2 m_trackOffset;

	// Token: 0x04003998 RID: 14744
	private bool m_trackDuringGrab;

	// Token: 0x04003999 RID: 14745
	private Vector2? m_resetStartPos;

	// Token: 0x0400399A RID: 14746
	private Vector2? m_resetEndPos;

	// Token: 0x0400399B RID: 14747
	private PlayerController m_affectedPlayer;

	// Token: 0x0400399C RID: 14748
	private ConsumeTargetBehavior.State m_state;

	// Token: 0x02000D1C RID: 3356
	public enum State
	{
		// Token: 0x040039A0 RID: 14752
		Idle,
		// Token: 0x040039A1 RID: 14753
		Tell,
		// Token: 0x040039A2 RID: 14754
		Path,
		// Token: 0x040039A3 RID: 14755
		Track,
		// Token: 0x040039A4 RID: 14756
		GrabBegin,
		// Token: 0x040039A5 RID: 14757
		GrabSuccess,
		// Token: 0x040039A6 RID: 14758
		Miss,
		// Token: 0x040039A7 RID: 14759
		WaitingForFinish,
		// Token: 0x040039A8 RID: 14760
		GrabFinish
	}
}
