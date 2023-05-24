using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D29 RID: 3369
public class GripMasterBehavior : BasicAttackBehavior
{
	// Token: 0x06004722 RID: 18210 RVA: 0x00174244 File Offset: 0x00172444
	public override void Start()
	{
		base.Start();
		this.m_posOffset = -(this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.transform.position.XY());
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		this.m_gripMasterController = this.m_aiActor.GetComponent<GripMasterController>();
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x001742CC File Offset: 0x001724CC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x001742E4 File Offset: 0x001724E4
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
		this.m_state = GripMasterBehavior.State.Tell;
		this.m_timer = this.TellTime;
		this.m_startPos = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
		this.m_aiAnimator.PlayUntilCancelled(this.TellAnim, false, null, -1f, false);
		this.m_aiActor.ClearPath();
		AkSoundEngine.PostEvent("Play_ENM_Grip_Master_Lockon_01", this.m_aiActor.gameObject);
		this.m_targetPosition = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
		this.m_gripMasterController.IsAttacking = true;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x001743B0 File Offset: 0x001725B0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == GripMasterBehavior.State.Tell)
		{
			if (this.m_state != GripMasterBehavior.State.Idle && this.m_aiActor.TargetRigidbody)
			{
				float num = this.TellTime - this.m_timer;
				this.UpdateTargetPosition();
				Vector2 vector = Vector2Extensions.SmoothStep(this.m_startPos, this.m_targetPosition, Mathf.Clamp01(num / this.TrackTime));
				this.m_aiActor.transform.position = vector + this.m_posOffset;
				this.m_aiActor.specRigidbody.Reinitialize();
			}
			if (this.m_timer <= 0f)
			{
				this.m_state = GripMasterBehavior.State.Grab;
				this.m_aiAnimator.PlayUntilFinished(this.GrabAnim, false, null, -1f, false);
				if (!string.IsNullOrEmpty(this.ShadowAnim))
				{
					this.m_aiActor.ShadowObject.GetComponent<tk2dSpriteAnimator>().Play(this.ShadowAnim);
				}
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == GripMasterBehavior.State.Grab)
		{
			if (this.m_state != GripMasterBehavior.State.Idle && this.m_aiActor.TargetRigidbody && !this.m_hasHit)
			{
				this.UpdateTargetPosition();
				this.m_aiActor.transform.position = this.m_targetPosition + this.m_posOffset;
				this.m_aiActor.specRigidbody.Reinitialize();
			}
			if (!this.m_aiAnimator.IsPlaying(this.GrabAnim))
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == GripMasterBehavior.State.Miss && !this.m_aiAnimator.IsPlaying(this.MissAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004726 RID: 18214 RVA: 0x0017456C File Offset: 0x0017276C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = GripMasterBehavior.State.Idle;
		this.m_hasHit = false;
		this.m_aiActor.sprite.HeightOffGround = 4f;
		if (!this.m_sentPlayerBack)
		{
			this.m_gripMasterController.IsAttacking = false;
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004727 RID: 18215 RVA: 0x001745C8 File Offset: 0x001727C8
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (!this.m_hasHit && this.m_state == GripMasterBehavior.State.Grab && frame.eventInfo == "hit")
		{
			this.m_aiActor.sprite.HeightOffGround = 0f;
			this.m_hasHit = true;
			if (this.m_gripMasterController)
			{
				this.m_gripMasterController.OnAttack();
			}
			this.ForceBlank(5f, 0.65f);
			bool flag = false;
			if (this.m_aiActor.TargetRigidbody)
			{
				PlayerController playerController = this.m_aiActor.TargetRigidbody.gameActor as PlayerController;
				if (playerController)
				{
					Vector2 unitCenter = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
					if (playerController.CanBeGrabbed && Vector2.Distance(unitCenter, this.m_targetPosition) < 1f)
					{
						this.BanishPlayer(playerController);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.m_state = GripMasterBehavior.State.Miss;
				this.m_aiAnimator.PlayUntilFinished(this.MissAnim, false, null, -1f, false);
			}
			this.m_aiActor.MoveToSafeSpot(0.5f);
		}
		if (frame.eventInfo == "lift")
		{
			this.m_aiActor.sprite.HeightOffGround = 4f;
		}
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x00174728 File Offset: 0x00172928
	private void UpdateTargetPosition()
	{
		if (this.m_behaviorSpeculator.TargetRigidbody)
		{
			Vector2 unitCenter = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
			this.m_targetPosition = Vector2.MoveTowards(this.m_targetPosition, unitCenter, 10f * this.m_deltaTime);
		}
	}

	// Token: 0x06004729 RID: 18217 RVA: 0x0017477C File Offset: 0x0017297C
	private void BanishPlayer(PlayerController player)
	{
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_HIT_WITH_THE_GRIPPY, 1f);
		int num = this.RoomsToSendBackward;
		if (this.m_gripMasterController && this.m_gripMasterController.Grip_OverrideRoomsToSendBackward > 0)
		{
			num = this.m_gripMasterController.Grip_OverrideRoomsToSendBackward;
		}
		if (num < 1)
		{
			num = 1;
		}
		List<RoomHandler> list = new List<RoomHandler>();
		List<RoomHandler> list2 = new List<RoomHandler>();
		list.Add(player.CurrentRoom);
		while (list.Count - 1 < this.RoomsToSendBackward)
		{
			RoomHandler roomHandler = list[list.Count - 1];
			list2.Clear();
			foreach (RoomHandler roomHandler2 in roomHandler.connectedRooms)
			{
				if (roomHandler2.hasEverBeenVisited && roomHandler2.distanceFromEntrance < roomHandler.distanceFromEntrance && !list.Contains(roomHandler2))
				{
					if (!roomHandler2.area.IsProceduralRoom || roomHandler2.area.proceduralCells == null)
					{
						list2.Add(roomHandler2);
					}
				}
			}
			if (list2.Count == 0)
			{
				break;
			}
			list.Add(BraveUtility.RandomElement<RoomHandler>(list2));
		}
		if (list.Count > 1)
		{
			player.RespawnInPreviousRoom(false, PlayerController.EscapeSealedRoomStyle.GRIP_MASTER, true, list[list.Count - 1]);
			for (int i = 1; i < list.Count - 1; i++)
			{
				list[i].ResetPredefinedRoomLikeDarkSouls();
			}
			Debug.LogFormat("Sending the player back {0} rooms (attempted {1})", new object[]
			{
				list.Count - 1,
				num
			});
		}
		else
		{
			player.RespawnInPreviousRoom(false, PlayerController.EscapeSealedRoomStyle.GRIP_MASTER, true, null);
			Debug.LogFormat("Sending the player back with RespawnInPreviousRoom (no valid \"backwards\" rooms found!)", new object[0]);
		}
		player.specRigidbody.Velocity = Vector2.zero;
		player.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
		this.m_aiActor.StartCoroutine(this.ForceAnimateCR(player));
		this.m_sentPlayerBack = true;
	}

	// Token: 0x0600472A RID: 18218 RVA: 0x001749AC File Offset: 0x00172BAC
	private IEnumerator ForceAnimateCR(PlayerController player)
	{
		tk2dSpriteAnimator shadowAnimator = null;
		if (this.m_aiActor.ShadowObject)
		{
			shadowAnimator = this.m_aiActor.ShadowObject.GetComponent<tk2dSpriteAnimator>();
		}
		float elapsed = 0f;
		float duration = 3.9f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			this.m_aiAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			if (shadowAnimator)
			{
				shadowAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			yield return null;
		}
		if (this.m_aiActor)
		{
			this.m_behaviorSpeculator.InterruptAndDisable();
			UnityEngine.Object.Destroy(this.m_aiActor.gameObject);
		}
		yield break;
	}

	// Token: 0x0600472B RID: 18219 RVA: 0x001749C8 File Offset: 0x00172BC8
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

	// Token: 0x04003A07 RID: 14855
	public float TrackTime;

	// Token: 0x04003A08 RID: 14856
	public float TellTime;

	// Token: 0x04003A09 RID: 14857
	public int RoomsToSendBackward = 1;

	// Token: 0x04003A0A RID: 14858
	[InspectorCategory("Visuals")]
	public string TellAnim;

	// Token: 0x04003A0B RID: 14859
	[InspectorCategory("Visuals")]
	public string GrabAnim;

	// Token: 0x04003A0C RID: 14860
	[InspectorCategory("Visuals")]
	public string MissAnim;

	// Token: 0x04003A0D RID: 14861
	[InspectorCategory("Visuals")]
	public string ShadowAnim;

	// Token: 0x04003A0E RID: 14862
	private GripMasterController m_gripMasterController;

	// Token: 0x04003A0F RID: 14863
	private Vector2 m_posOffset;

	// Token: 0x04003A10 RID: 14864
	private Vector2 m_targetPosition;

	// Token: 0x04003A11 RID: 14865
	private float m_timer;

	// Token: 0x04003A12 RID: 14866
	private Vector2 m_startPos;

	// Token: 0x04003A13 RID: 14867
	private bool m_hasHit;

	// Token: 0x04003A14 RID: 14868
	private bool m_sentPlayerBack;

	// Token: 0x04003A15 RID: 14869
	private GripMasterBehavior.State m_state;

	// Token: 0x02000D2A RID: 3370
	public enum State
	{
		// Token: 0x04003A17 RID: 14871
		Idle,
		// Token: 0x04003A18 RID: 14872
		Tell,
		// Token: 0x04003A19 RID: 14873
		Grab,
		// Token: 0x04003A1A RID: 14874
		Miss
	}
}
