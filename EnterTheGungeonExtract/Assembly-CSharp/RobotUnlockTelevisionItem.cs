using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001497 RID: 5271
public class RobotUnlockTelevisionItem : PlayerItem
{
	// Token: 0x060077F2 RID: 30706 RVA: 0x002FE2D4 File Offset: 0x002FC4D4
	public override void Pickup(PlayerController player)
	{
		this.m_owner = player;
		player.OnRollStarted += this.HandleRoll;
		base.Pickup(player);
	}

	// Token: 0x060077F3 RID: 30707 RVA: 0x002FE2F8 File Offset: 0x002FC4F8
	protected override void OnPreDrop(PlayerController user)
	{
		user.OnRollStarted -= this.HandleRoll;
		this.m_owner = null;
		base.OnPreDrop(user);
	}

	// Token: 0x060077F4 RID: 30708 RVA: 0x002FE31C File Offset: 0x002FC51C
	private void HandleRoll(PlayerController arg1, Vector2 arg2)
	{
		DebrisObject debrisObject = this.m_owner.DropActiveItem(this, 0f, false);
		debrisObject.inertialMass = 10000000f;
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", GameManager.Instance.gameObject);
	}

	// Token: 0x060077F5 RID: 30709 RVA: 0x002FE35C File Offset: 0x002FC55C
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.HandleRoll;
		}
		base.OnDestroy();
	}

	// Token: 0x060077F6 RID: 30710 RVA: 0x002FE38C File Offset: 0x002FC58C
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", GameManager.Instance.gameObject);
		DebrisObject debrisObject = this.m_owner.DropActiveItem(this, 7f, false);
		GameObject gameObject = debrisObject.gameObject;
		UnityEngine.Object.Destroy(debrisObject);
		SpeculativeRigidbody speculativeRigidbody = gameObject.AddComponent<SpeculativeRigidbody>();
		speculativeRigidbody.transform.position = user.specRigidbody.UnitBottomLeft.ToVector3ZisY(0f);
		speculativeRigidbody.PixelColliders = new List<PixelCollider>();
		PixelCollider pixelCollider = new PixelCollider();
		pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
		pixelCollider.ManualOffsetX = 2;
		pixelCollider.ManualOffsetY = 3;
		pixelCollider.ManualWidth = 11;
		pixelCollider.ManualHeight = 10;
		pixelCollider.CollisionLayer = CollisionLayer.LowObstacle;
		pixelCollider.Enabled = true;
		pixelCollider.IsTrigger = false;
		speculativeRigidbody.PixelColliders.Add(pixelCollider);
		speculativeRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(speculativeRigidbody, null, false);
		speculativeRigidbody.RegisterSpecificCollisionException(user.specRigidbody);
		user.StartCoroutine(this.HandleDodgeRoll(speculativeRigidbody, user.unadjustedAimPoint.XY() - user.sprite.WorldCenter));
	}

	// Token: 0x060077F7 RID: 30711 RVA: 0x002FE4A0 File Offset: 0x002FC6A0
	private bool HandlePitfall(SpeculativeRigidbody targetRigidbody)
	{
		if (GameManager.Instance.Dungeon.ShouldReallyFall(targetRigidbody.UnitCenter))
		{
			DebrisObject debrisObject = targetRigidbody.gameObject.AddComponent<DebrisObject>();
			debrisObject.canRotate = false;
			debrisObject.Trigger(Vector3.zero, 0.01f, 1f);
			UnityEngine.Object.Destroy(targetRigidbody);
			return true;
		}
		return false;
	}

	// Token: 0x060077F8 RID: 30712 RVA: 0x002FE500 File Offset: 0x002FC700
	private IEnumerator HandleDodgeRoll(SpeculativeRigidbody targetRigidbody, Vector2 direction)
	{
		float elapsed = 0f;
		targetRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(targetRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RollPitMovementRestrictor));
		bool hasGrounded = false;
		targetRigidbody.spriteAnimator.PlayAndForceTime(targetRigidbody.spriteAnimator.DefaultClip, this.RollStats.time);
		while (elapsed < this.RollStats.time)
		{
			elapsed += BraveTime.DeltaTime;
			float drSpeed = this.GetDodgeRollSpeed(elapsed, this.RollStats.speed, this.RollStats.time, this.RollStats.distance);
			targetRigidbody.Velocity = direction.normalized * drSpeed;
			this.m_dodgeRollState = ((elapsed <= 0.39f) ? PlayerController.DodgeRollState.InAir : PlayerController.DodgeRollState.OnGround);
			if (!hasGrounded && this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround)
			{
				hasGrounded = true;
				GameManager.Instance.Dungeon.dungeonDustups.InstantiateLandDustup(targetRigidbody.UnitBottomCenter.ToVector3ZisY(0f));
				this.OnGrounded(targetRigidbody);
			}
			if (this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround && this.HandlePitfall(targetRigidbody))
			{
				yield break;
			}
			yield return null;
		}
		targetRigidbody.spriteAnimator.SetFrame(0);
		targetRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(targetRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RollPitMovementRestrictor));
		this.m_dodgeRollState = PlayerController.DodgeRollState.None;
		targetRigidbody.Velocity = Vector2.zero;
		UnityEngine.Object.Destroy(targetRigidbody);
		yield break;
	}

	// Token: 0x060077F9 RID: 30713 RVA: 0x002FE52C File Offset: 0x002FC72C
	private void OnGrounded(SpeculativeRigidbody targetRigidbody)
	{
		PlayerItem component = targetRigidbody.GetComponent<PlayerItem>();
		if (component != null)
		{
			component.ForceAsExtant = true;
			if (!RoomHandler.unassignedInteractableObjects.Contains(component))
			{
				RoomHandler.unassignedInteractableObjects.Add(component);
			}
		}
	}

	// Token: 0x060077FA RID: 30714 RVA: 0x002FE570 File Offset: 0x002FC770
	private float GetDodgeRollSpeed(float dodgeRollTimer, AnimationCurve speedCurve, float rollTime, float rollDistance)
	{
		float num = Mathf.Clamp01((dodgeRollTimer - BraveTime.DeltaTime) / rollTime);
		float num2 = Mathf.Clamp01(dodgeRollTimer / rollTime);
		float num3 = (Mathf.Clamp01(speedCurve.Evaluate(num2)) - Mathf.Clamp01(speedCurve.Evaluate(num))) * rollDistance;
		return num3 / BraveTime.DeltaTime;
	}

	// Token: 0x060077FB RID: 30715 RVA: 0x002FE5BC File Offset: 0x002FC7BC
	private void RollPitMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround)
		{
			Func<IntVector2, bool> func = delegate(IntVector2 pixel)
			{
				Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
				if (!GameManager.Instance.Dungeon.CellSupportsFalling(vector))
				{
					return false;
				}
				List<SpeculativeRigidbody> platformsAt = GameManager.Instance.Dungeon.GetPlatformsAt(vector);
				if (platformsAt != null)
				{
					for (int i = 0; i < platformsAt.Count; i++)
					{
						if (platformsAt[i].PrimaryPixelCollider.ContainsPixel(pixel))
						{
							return false;
						}
					}
				}
				IntVector2 intVector2 = vector.ToIntVector2(VectorConversions.Floor);
				return !GameManager.Instance.Dungeon.data.isTopWall(intVector2.x, intVector2.y) || true;
			};
			PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
			if (primaryPixelCollider != null)
			{
				IntVector2 intVector = pixelOffset - prevPixelOffset;
				if (intVector == IntVector2.Down && func(primaryPixelCollider.LowerLeft + pixelOffset) && func(primaryPixelCollider.LowerRight + pixelOffset) && (!func(primaryPixelCollider.UpperRight + prevPixelOffset) || !func(primaryPixelCollider.UpperLeft + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Right && func(primaryPixelCollider.LowerRight + pixelOffset) && func(primaryPixelCollider.UpperRight + pixelOffset) && (!func(primaryPixelCollider.UpperLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerLeft + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Up && func(primaryPixelCollider.UpperRight + pixelOffset) && func(primaryPixelCollider.UpperLeft + pixelOffset) && (!func(primaryPixelCollider.LowerLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerRight + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Left && func(primaryPixelCollider.UpperLeft + pixelOffset) && func(primaryPixelCollider.LowerLeft + pixelOffset) && (!func(primaryPixelCollider.LowerRight + prevPixelOffset) || !func(primaryPixelCollider.UpperRight + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
			}
		}
	}

	// Token: 0x04007A0E RID: 31246
	public DodgeRollStats RollStats;

	// Token: 0x04007A0F RID: 31247
	protected PlayerController m_owner;

	// Token: 0x04007A10 RID: 31248
	protected PlayerController.DodgeRollState m_dodgeRollState;
}
