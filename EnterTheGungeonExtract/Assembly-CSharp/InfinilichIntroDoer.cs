using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001050 RID: 4176
[RequireComponent(typeof(GenericIntroDoer))]
public class InfinilichIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005BBE RID: 23486 RVA: 0x00232A44 File Offset: 0x00230C44
	public void Awake()
	{
		base.GetComponentInChildren<BulletLimbController>().HideBullets = true;
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			absoluteRoom.AdditionalRoomState = RoomHandler.CustomRoomState.LICH_PHASE_THREE;
		}
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x00232A7C File Offset: 0x00230C7C
	protected override void OnDestroy()
	{
		this.ModifyWorld(false);
		base.OnDestroy();
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x00232A8C File Offset: 0x00230C8C
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.PlayUntilCancelled("preintro", false, null, -1f, false);
		this.m_shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(0f);
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x00232AE8 File Offset: 0x00230CE8
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		Minimap.Instance.TemporarilyPreventMinimap = false;
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06005BC2 RID: 23490 RVA: 0x00232B04 File Offset: 0x00230D04
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.controllerCamera.isTransitioning = false;
			return null;
		}
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x00232B34 File Offset: 0x00230D34
	public IEnumerator DoIntro()
	{
		base.GetComponentInChildren<BulletLimbController>().HideBullets = false;
		base.aiAnimator.PlayUntilCancelled("intro", false, null, -1f, false);
		this.m_radiusSquared = this.radius * this.radius;
		while (base.aiAnimator.IsPlaying("intro"))
		{
			float clipProgress = Mathf.InverseLerp(0.3f, 1f, base.aiAnimator.CurrentClipProgress);
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(clipProgress);
			for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
			{
				this.AdjustDebrisVelocity(StaticReferenceManager.AllDebris[i]);
			}
			yield return null;
		}
		base.aiAnimator.EndAnimationIf("intro");
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x06005BC4 RID: 23492 RVA: 0x00232B50 File Offset: 0x00230D50
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		base.aiAnimator.EndAnimationIf("preintro");
		base.aiAnimator.EndAnimationIf("intro");
		base.GetComponentInChildren<BulletLimbController>().HideBullets = false;
		AkSoundEngine.PostEvent("Play_MUS_Lich_Phase_03", base.gameObject);
	}

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x06005BC5 RID: 23493 RVA: 0x00232BA4 File Offset: 0x00230DA4
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x00232BAC File Offset: 0x00230DAC
	public void ModifyWorld(bool value)
	{
		if (GameManager.HasInstance && value != this.m_isWorldModified)
		{
			if (value)
			{
				if (!this.m_endTimesNebulaController)
				{
					this.m_endTimesNebulaController = UnityEngine.Object.FindObjectOfType<EndTimesNebulaController>();
				}
				if (this.m_endTimesNebulaController)
				{
					this.m_endTimesNebulaController.BecomeActive();
				}
			}
			else if (this.m_endTimesNebulaController)
			{
				this.m_endTimesNebulaController.BecomeInactive(false);
			}
			this.m_isWorldModified = value;
		}
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00232C34 File Offset: 0x00230E34
	private bool AdjustDebrisVelocity(DebrisObject debris)
	{
		if (debris.IsPickupObject)
		{
			return false;
		}
		if (debris.GetComponent<BlackHoleDoer>() != null)
		{
			return false;
		}
		if (!debris.name.Contains("shell", true))
		{
			return false;
		}
		Vector2 vector = debris.sprite.WorldCenter - base.specRigidbody.UnitCenter;
		float num = Vector2.SqrMagnitude(vector);
		if (num > this.m_radiusSquared)
		{
			return false;
		}
		float num2 = Mathf.Sqrt(num);
		if (num2 < this.destroyRadius)
		{
			UnityEngine.Object.Destroy(debris.gameObject);
			return true;
		}
		Vector2 frameAccelerationForRigidbody = this.GetFrameAccelerationForRigidbody(debris.sprite.WorldCenter, num2, this.gravityForce);
		float num3 = Mathf.Clamp(GameManager.INVARIANT_DELTA_TIME, 0f, 0.02f);
		if (debris.HasBeenTriggered)
		{
			debris.ApplyVelocity(frameAccelerationForRigidbody * num3);
		}
		else if (num2 < this.radius / 2f)
		{
			debris.Trigger(frameAccelerationForRigidbody * num3, 0.5f, 1f);
		}
		return true;
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x00232D48 File Offset: 0x00230F48
	private Vector2 GetFrameAccelerationForRigidbody(Vector2 unitCenter, float currentDistance, float g)
	{
		float num = Mathf.Clamp01(1f - currentDistance / this.radius);
		float num2 = g * num * num;
		return (base.specRigidbody.UnitCenter - unitCenter).normalized * num2;
	}

	// Token: 0x04005557 RID: 21847
	[Header("Shell Sucking")]
	public float radius = 15f;

	// Token: 0x04005558 RID: 21848
	public float gravityForce = 200f;

	// Token: 0x04005559 RID: 21849
	public float destroyRadius = 1f;

	// Token: 0x0400555A RID: 21850
	private bool m_isFinished;

	// Token: 0x0400555B RID: 21851
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x0400555C RID: 21852
	private bool m_isWorldModified;

	// Token: 0x0400555D RID: 21853
	private EndTimesNebulaController m_endTimesNebulaController;

	// Token: 0x0400555E RID: 21854
	private float m_radiusSquared;
}
