using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DD0 RID: 3536
[InspectorDropdownName("Bosses/MetalGearRat/MoveBehavior")]
public class MetalGearRatMoveBehavior : BasicAttackBehavior
{
	// Token: 0x06004AF1 RID: 19185 RVA: 0x0019449C File Offset: 0x0019269C
	public override void Start()
	{
		this.m_moveDirection = Vector2.right;
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x001944AC File Offset: 0x001926AC
	public override bool IsOverridable()
	{
		return this.m_state == MetalGearRatMoveBehavior.State.Idle;
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x001944B8 File Offset: 0x001926B8
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
		this.m_moveDirection = this.UpdateMoveDirection();
		this.m_shadow = this.m_aiActor.ShadowObject;
		this.m_shadowStartingPos = this.m_shadow.transform.localPosition;
		this.m_cameraPoint = this.m_aiActor.gameObject.transform.Find("camera point").gameObject;
		this.m_cameraStartingPos = this.m_cameraPoint.transform.localPosition;
		if (this.m_moveDirection.x < 0f)
		{
			this.m_aiAnimator.PlayUntilFinished("move_left", false, null, -1f, false);
		}
		else
		{
			this.m_aiAnimator.PlayUntilFinished("move_right", false, null, -1f, false);
		}
		this.m_updateEveryFrame = true;
		this.m_state = MetalGearRatMoveBehavior.State.Moving;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001945B4 File Offset: 0x001927B4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == MetalGearRatMoveBehavior.State.Moving)
		{
			Vector2 vector = Vector2.Lerp(Vector2.zero, this.m_moveDirection * (this.HorizontalMovePixels / 16f), this.m_aiAnimator.CurrentClipProgress * 2.2f);
			this.m_shadow.transform.localPosition = this.m_shadowStartingPos + vector;
			this.m_cameraPoint.transform.localPosition = this.m_cameraStartingPos + vector;
			if (!this.m_aiAnimator.IsPlaying("move_left") && !this.m_aiAnimator.IsPlaying("move_right"))
			{
				this.m_aiActor.transform.position += this.m_moveDirection * (this.HorizontalMovePixels / 16f);
				this.m_shadow.transform.localPosition = this.m_shadowStartingPos;
				this.m_cameraPoint.transform.localPosition = this.m_cameraStartingPos;
				this.m_aiActor.specRigidbody.Reinitialize();
				this.m_state = MetalGearRatMoveBehavior.State.Done;
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == MetalGearRatMoveBehavior.State.Done)
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x00194710 File Offset: 0x00192910
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = MetalGearRatMoveBehavior.State.Idle;
		this.m_aiAnimator.EndAnimationIf("move_left");
		this.m_shadow.transform.localPosition = this.m_shadowStartingPos;
		this.m_cameraPoint.transform.localPosition = this.m_cameraStartingPos;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x00194780 File Offset: 0x00192980
	private Vector2 UpdateMoveDirection()
	{
		Vector2 unitCenter = this.m_aiActor.ParentRoom.area.UnitCenter;
		Vector2 unitCenter2 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (unitCenter2.x < unitCenter.x - 7f)
		{
			return Vector2.right;
		}
		if (unitCenter2.x > unitCenter.x + 7f)
		{
			return Vector2.left;
		}
		if (this.m_aiActor.TargetRigidbody)
		{
			float num = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox).x - unitCenter2.x;
			if (num > 0f)
			{
				return Vector2.right;
			}
			if (num < 0f)
			{
				return Vector2.left;
			}
		}
		return (!BraveUtility.RandomBool()) ? Vector2.right : Vector2.left;
	}

	// Token: 0x0400401A RID: 16410
	public float HorizontalMovePixels = 5f;

	// Token: 0x0400401B RID: 16411
	private MetalGearRatMoveBehavior.State m_state;

	// Token: 0x0400401C RID: 16412
	private Vector2 m_moveDirection;

	// Token: 0x0400401D RID: 16413
	private GameObject m_shadow;

	// Token: 0x0400401E RID: 16414
	private Vector2 m_shadowStartingPos;

	// Token: 0x0400401F RID: 16415
	private GameObject m_cameraPoint;

	// Token: 0x04004020 RID: 16416
	private Vector2 m_cameraStartingPos;

	// Token: 0x02000DD1 RID: 3537
	private enum State
	{
		// Token: 0x04004022 RID: 16418
		Idle,
		// Token: 0x04004023 RID: 16419
		Moving,
		// Token: 0x04004024 RID: 16420
		Done
	}
}
