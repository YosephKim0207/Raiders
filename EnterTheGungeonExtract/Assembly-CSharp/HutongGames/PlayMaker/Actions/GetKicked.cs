using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C59 RID: 3161
	[Tooltip("Kickable corpse.")]
	[ActionCategory(".Brave")]
	public class GetKicked : FsmStateAction
	{
		// Token: 0x06004412 RID: 17426 RVA: 0x0015F9BC File Offset: 0x0015DBBC
		public override void Reset()
		{
			base.Reset();
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0015F9C4 File Offset: 0x0015DBC4
		public override void Awake()
		{
			base.Awake();
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			if (ownerDefaultTarget)
			{
				SpeculativeRigidbody component = ownerDefaultTarget.GetComponent<SpeculativeRigidbody>();
				if (component && !this.m_hasInitializedSRB)
				{
					this.m_hasInitializedSRB = true;
					SpeculativeRigidbody speculativeRigidbody = component;
					speculativeRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(speculativeRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.HandlePostRigidbodyMotion));
					SpeculativeRigidbody speculativeRigidbody2 = component;
					speculativeRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
					SpeculativeRigidbody speculativeRigidbody3 = component;
					speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
				}
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0015FA80 File Offset: 0x0015DC80
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			tk2dSpriteAnimator component2 = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
			AIAnimator component3 = ownerDefaultTarget.GetComponent<AIAnimator>();
			PlayerController playerController = ((!component.TalkingPlayer) ? GameManager.Instance.PrimaryPlayer : component.TalkingPlayer);
			if (component3)
			{
				component3.PlayUntilCancelled("kick" + this.kickCount.ToString(), true, null, -1f, false);
				this.kickCount = this.kickCount % 8 + 1;
				if (component3.specRigidbody && playerController)
				{
					SpeculativeRigidbody specRigidbody = component3.specRigidbody;
					if (!this.m_hasInitializedSRB)
					{
						this.m_hasInitializedSRB = true;
						SpeculativeRigidbody speculativeRigidbody = specRigidbody;
						speculativeRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(speculativeRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.HandlePostRigidbodyMotion));
						SpeculativeRigidbody speculativeRigidbody2 = specRigidbody;
						speculativeRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
						SpeculativeRigidbody speculativeRigidbody3 = specRigidbody;
						speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
					}
					specRigidbody.Velocity += (specRigidbody.UnitCenter - playerController.CenterPosition).normalized * 3f;
					SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_DodgeRollHit", ".prefab"), specRigidbody.UnitCenter, Quaternion.identity, true);
				}
			}
			if (playerController)
			{
				this.SetAnimationState(playerController, component);
			}
			base.Finish();
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0015FC44 File Offset: 0x0015DE44
		private void HandleTileCollision(CollisionData tileCollision)
		{
			Vector2 velocity = tileCollision.MyRigidbody.Velocity;
			float num = (-velocity).ToAngle();
			float num2 = tileCollision.Normal.ToAngle();
			float num3 = BraveMathCollege.ClampAngle360(num + 2f * (num2 - num));
			PhysicsEngine.PostSliceVelocity = new Vector2?(BraveMathCollege.DegreesToVector(num3, 1f).normalized * velocity.magnitude);
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0015FCB0 File Offset: 0x0015DEB0
		private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
		{
			if (rigidbodyCollision.OtherRigidbody && rigidbodyCollision.OtherRigidbody.projectile)
			{
				Vector2 normalized = rigidbodyCollision.OtherRigidbody.projectile.LastVelocity.normalized;
				rigidbodyCollision.MyRigidbody.Velocity += normalized * 1.5f;
				AIAnimator aiAnimator = rigidbodyCollision.MyRigidbody.aiAnimator;
				if (aiAnimator && aiAnimator.CurrentClipProgress >= 1f)
				{
					aiAnimator.PlayUntilCancelled("kick" + this.kickCount.ToString(), true, null, -1f, false);
					this.kickCount = this.kickCount % 8 + 1;
				}
			}
			else
			{
				Vector2 velocity = rigidbodyCollision.MyRigidbody.Velocity;
				float num = (-velocity).ToAngle();
				float num2 = rigidbodyCollision.Normal.ToAngle();
				float num3 = BraveMathCollege.ClampAngle360(num + 2f * (num2 - num));
				PhysicsEngine.PostSliceVelocity = new Vector2?(BraveMathCollege.DegreesToVector(num3, 1f).normalized * velocity.magnitude);
			}
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0015FDE8 File Offset: 0x0015DFE8
		private void HandlePostRigidbodyMotion(SpeculativeRigidbody arg1, Vector2 arg2, IntVector2 arg3)
		{
			arg1.Velocity = Vector2.MoveTowards(arg1.Velocity, Vector2.zero, 5f * BraveTime.DeltaTime);
			if (!this.m_isFalling && GameManager.Instance.Dungeon.ShouldReallyFall(arg1.UnitTopLeft) && GameManager.Instance.Dungeon.ShouldReallyFall(arg1.UnitTopRight) && GameManager.Instance.Dungeon.ShouldReallyFall(arg1.UnitBottomLeft) && GameManager.Instance.Dungeon.ShouldReallyFall(arg1.UnitBottomRight))
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandlePitfall(arg1));
			}
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0015FEB4 File Offset: 0x0015E0B4
		private IEnumerator HandlePitfall(SpeculativeRigidbody srb)
		{
			this.m_isFalling = true;
			RoomHandler firstRoom = srb.UnitCenter.GetAbsoluteRoom();
			TalkDoerLite talkdoer = srb.GetComponent<TalkDoerLite>();
			firstRoom.DeregisterInteractable(talkdoer);
			srb.Velocity = srb.Velocity.normalized * 0.125f;
			AIAnimator anim = srb.GetComponent<AIAnimator>();
			anim.PlayUntilFinished("pitfall", false, null, -1f, false);
			while (anim.IsPlaying("pitfall"))
			{
				yield return null;
			}
			anim.PlayUntilCancelled("kick1", false, null, -1f, false);
			srb.Velocity = Vector2.zero;
			RoomHandler targetRoom = firstRoom.TargetPitfallRoom;
			Transform[] childTransforms = targetRoom.hierarchyParent.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < childTransforms.Length; i++)
			{
				if (childTransforms[i].name == "Arrival")
				{
					srb.transform.position = childTransforms[i].position + new Vector3(1f, 1f, 0f);
					srb.Reinitialize();
					RoomHandler.unassignedInteractableObjects.Add(talkdoer);
					break;
				}
			}
			yield break;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0015FED8 File Offset: 0x0015E0D8
		public void SetAnimationState(PlayerController interactor, TalkDoerLite owner)
		{
			string text = "tablekick_up";
			Vector2 vector = interactor.CenterPosition - owner.specRigidbody.UnitCenter;
			switch (BraveMathCollege.VectorToQuadrant(vector))
			{
			case 0:
				text = "tablekick_down";
				break;
			case 1:
				text = "tablekick_right";
				break;
			case 2:
				text = "tablekick_up";
				break;
			case 3:
				text = "tablekick_right";
				break;
			}
			interactor.QueueSpecificAnimation(text);
		}

		// Token: 0x04003624 RID: 13860
		public FsmOwnerDefault GameObject;

		// Token: 0x04003625 RID: 13861
		private int kickCount = 1;

		// Token: 0x04003626 RID: 13862
		private bool m_hasInitializedSRB;

		// Token: 0x04003627 RID: 13863
		private bool m_isFalling;
	}
}
