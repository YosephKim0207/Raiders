using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CCB RID: 3275
	[Tooltip("Handles NPC teleportation.")]
	[ActionCategory(".NPCs")]
	public class Teleport : FsmStateAction
	{
		// Token: 0x06004598 RID: 17816 RVA: 0x00168D88 File Offset: 0x00166F88
		public override void Reset()
		{
			this.mode = Teleport.Mode.In;
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00168D94 File Offset: 0x00166F94
		public override string ErrorCheck()
		{
			string text = string.Empty;
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (component.spriteAnimator == null && component.aiAnimator == null)
			{
				return text + "Requires a 2D Toolkit animator or an AI Animator.\n";
			}
			if (component.aiAnimator != null)
			{
				if ((this.mode == Teleport.Mode.In || this.mode == Teleport.Mode.Both) && !component.aiAnimator.HasDirectionalAnimation(component.teleportInSettings.anim))
				{
					text = text + "Unknown animation " + component.teleportInSettings.anim + ".\n";
				}
				if ((this.mode == Teleport.Mode.Out || this.mode == Teleport.Mode.Both) && !component.aiAnimator.HasDirectionalAnimation(component.teleportOutSettings.anim))
				{
					text = text + "Unknown animation " + component.teleportOutSettings.anim + ".\n";
				}
			}
			else if (component.spriteAnimator != null)
			{
				if ((this.mode == Teleport.Mode.In || this.mode == Teleport.Mode.Both) && component.spriteAnimator.GetClipByName(component.teleportInSettings.anim) == null)
				{
					text = text + "Unknown animation " + component.teleportInSettings.anim + ".\n";
				}
				if ((this.mode == Teleport.Mode.Out || this.mode == Teleport.Mode.Both) && component.spriteAnimator.GetClipByName(component.teleportOutSettings.anim) == null)
				{
					text = text + "Unknown animation " + component.teleportOutSettings.anim + ".\n";
				}
			}
			return text;
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00168F48 File Offset: 0x00167148
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			this.m_coroutine = null;
			this.m_lightIntensityAction = null;
			if (this.mode == Teleport.Mode.In)
			{
				this.m_state = Teleport.State.TeleportIn;
				this.m_coroutine = this.HandleAnimAndVfx(this.m_talkDoer.teleportInSettings);
			}
			else if (this.mode == Teleport.Mode.Out || this.mode == Teleport.Mode.Both)
			{
				this.m_state = Teleport.State.TeleportOut;
				this.m_coroutine = this.HandleAnimAndVfx(this.m_talkDoer.teleportOutSettings);
			}
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00168FD8 File Offset: 0x001671D8
		public override void OnUpdate()
		{
			if (this.m_state == Teleport.State.TeleportOut)
			{
				if (!this.m_coroutine.MoveNext())
				{
					if (this.mode == Teleport.Mode.Both)
					{
						this.m_state = Teleport.State.MidStep;
						this.m_stateTimer = this.goneTime.Value;
					}
					else
					{
						base.Finish();
					}
				}
			}
			else if (this.m_state == Teleport.State.MidStep)
			{
				this.m_stateTimer -= BraveTime.DeltaTime;
				if (this.m_stateTimer <= 0f)
				{
					PathMover component = this.m_talkDoer.GetComponent<PathMover>();
					this.m_talkDoer.transform.position = ((!this.useEndOfPath) ? (this.m_talkDoer.transform.position + this.positionDelta.Value) : component.GetPositionOfNode(component.Path.nodes.Count - 1).ToVector3ZUp(0f));
					this.m_talkDoer.specRigidbody.Reinitialize();
					PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_talkDoer.specRigidbody, new int?(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider)), false);
					this.m_state = Teleport.State.TeleportIn;
					this.m_coroutine = this.HandleAnimAndVfx(this.m_talkDoer.teleportInSettings);
				}
			}
			else if (this.m_state == Teleport.State.TeleportIn && !this.m_coroutine.MoveNext())
			{
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_talkDoer.specRigidbody, new int?(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider)), false);
				base.Finish();
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0016916C File Offset: 0x0016736C
		private IEnumerator HandleAnimAndVfx(TalkDoerLite.TeleportSettings teleportSettings)
		{
			if (teleportSettings.timing == Teleport.Timing.Simultaneous)
			{
				GameObject vfx = this.SpawnVfx(teleportSettings.vfx, teleportSettings.vfxAnchor);
				this.PlayAnim(teleportSettings.anim);
				yield return null;
				while (vfx || this.IsPlaying(teleportSettings.anim))
				{
					yield return null;
				}
				this.FinishAnim();
			}
			else if (teleportSettings.timing == Teleport.Timing.VfxThenAnimation)
			{
				GameObject vfx2 = this.SpawnVfx(teleportSettings.vfx, teleportSettings.vfxAnchor);
				while (vfx2)
				{
					yield return null;
				}
				this.PlayAnim(teleportSettings.anim);
				yield return null;
				while (this.IsPlaying(teleportSettings.anim))
				{
					yield return null;
				}
				this.FinishAnim();
			}
			else if (teleportSettings.timing == Teleport.Timing.AnimationThenVfx)
			{
				this.PlayAnim(teleportSettings.anim);
				yield return null;
				while (this.IsPlaying(teleportSettings.anim))
				{
					yield return null;
				}
				this.FinishAnim();
				GameObject vfx3 = this.SpawnVfx(teleportSettings.vfx, teleportSettings.vfxAnchor);
				while (vfx3)
				{
					yield return null;
				}
			}
			else if (teleportSettings.timing == Teleport.Timing.Delays)
			{
				float animTimer = teleportSettings.animDelay;
				float vfxTimer = teleportSettings.vfxDelay;
				bool playedAnim = false;
				bool waitForAnimComplete = false;
				bool playedVfx = !teleportSettings.vfx;
				GameObject vfx4 = null;
				yield return null;
				while (!playedAnim || !playedVfx || vfx4 || this.IsPlaying(teleportSettings.anim))
				{
					if (this.m_lightIntensityAction != null && !this.m_lightIntensityAction.Finished)
					{
						this.m_lightIntensityAction.OnUpdate();
					}
					if (waitForAnimComplete && !this.IsPlaying(teleportSettings.anim))
					{
						this.FinishAnim();
						waitForAnimComplete = false;
					}
					if (!playedVfx && vfxTimer >= 0f)
					{
						vfxTimer -= BraveTime.DeltaTime;
						if (vfxTimer <= 0f)
						{
							playedVfx = true;
							vfx4 = this.SpawnVfx(teleportSettings.vfx, teleportSettings.vfxAnchor);
						}
					}
					if (!playedAnim && animTimer >= 0f)
					{
						animTimer -= BraveTime.DeltaTime;
						if (animTimer <= 0f)
						{
							playedAnim = true;
							this.PlayAnim(teleportSettings.anim);
							waitForAnimComplete = true;
						}
					}
					yield return null;
				}
				if (this.m_lightIntensityAction != null && !this.m_lightIntensityAction.Finished)
				{
					this.m_lightIntensityAction.OnUpdate();
					this.m_lightIntensityAction.OnExit();
					this.m_lightIntensityAction = null;
				}
				if (waitForAnimComplete && !this.IsPlaying(teleportSettings.anim))
				{
					this.FinishAnim();
				}
			}
			yield break;
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00169190 File Offset: 0x00167390
		private void PlayAnim(string anim)
		{
			if (this.m_state == Teleport.State.TeleportIn)
			{
				SetNpcVisibility.SetVisible(this.m_talkDoer, true);
				this.m_talkDoer.ShowOutlines = true;
			}
			if (this.m_talkDoer.aiAnimator)
			{
				this.m_talkDoer.aiAnimator.PlayUntilCancelled(anim, false, null, -1f, false);
			}
			else if (this.m_talkDoer.spriteAnimator)
			{
				this.m_talkDoer.spriteAnimator.Play(anim);
			}
			if (this.lerpLight.Value)
			{
				float num;
				if (this.mode == Teleport.Mode.Both)
				{
					num = ((this.m_state != Teleport.State.TeleportOut) ? this.m_cachedLightIntensity : 0f);
				}
				else
				{
					num = this.newLightIntensity.Value;
				}
				this.m_lightIntensityAction = new SetBraveLightIntensity();
				this.m_lightIntensityAction.specifyLights = new ShadowSystem[0];
				this.m_lightIntensityAction.intensity = num;
				this.m_lightIntensityAction.transitionTime = this.m_talkDoer.spriteAnimator.CurrentClip.BaseClipLength;
				this.m_lightIntensityAction.Owner = base.Owner;
				this.m_lightIntensityAction.IsKeptAction = true;
				this.m_lightIntensityAction.OnEnter();
				this.m_cachedLightIntensity = ((this.m_lightIntensityAction.specifyLights.Length <= 0) ? 0f : this.m_lightIntensityAction.specifyLights[0].uLightIntensity);
				this.m_lightIntensityAction.OnUpdate();
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00169320 File Offset: 0x00167520
		private bool IsPlaying(string anim)
		{
			if (this.m_talkDoer.aiAnimator)
			{
				return this.m_talkDoer.aiAnimator.IsPlaying(anim);
			}
			return this.m_talkDoer.spriteAnimator && this.m_talkDoer.spriteAnimator.IsPlaying(anim);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0016937C File Offset: 0x0016757C
		private void FinishAnim()
		{
			if (this.m_state == Teleport.State.TeleportOut)
			{
				SetNpcVisibility.SetVisible(this.m_talkDoer, false);
				this.m_talkDoer.ShowOutlines = false;
			}
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x001693A4 File Offset: 0x001675A4
		private GameObject SpawnVfx(GameObject vfxPrefab, GameObject anchor)
		{
			if (!vfxPrefab)
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(vfxPrefab, this.m_talkDoer.specRigidbody.GetUnitCenter(ColliderType.HitBox), Quaternion.identity);
			if (anchor)
			{
				gameObject.transform.parent = anchor.transform;
				gameObject.transform.localPosition = Vector3.zero;
			}
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			if (component && component.IsPerpendicular == this.m_talkDoer.sprite.IsPerpendicular)
			{
				this.m_talkDoer.sprite.AttachRenderer(component);
				component.HeightOffGround = 0.05f;
				component.UpdateZDepth();
			}
			return gameObject;
		}

		// Token: 0x040037C8 RID: 14280
		[Tooltip("Teleportation type; In and Out handle visibility and effects, Both also handles translation.")]
		public Teleport.Mode mode;

		// Token: 0x040037C9 RID: 14281
		[Tooltip("How long the NPC is completely gone (i.e. the delay between the In finishing and the Out starting).")]
		public FsmFloat goneTime;

		// Token: 0x040037CA RID: 14282
		[Tooltip("How far the NPC should move during the teleport, in Unity units (i.e. tiles).")]
		public FsmVector2 positionDelta;

		// Token: 0x040037CB RID: 14283
		[Tooltip("When true, will ignore positionDelta and teleport to the end of the attached path.")]
		public bool useEndOfPath;

		// Token: 0x040037CC RID: 14284
		[Tooltip("If true, lerps any Brent lights on this object while the the teleport animation is playing.")]
		public FsmBool lerpLight = false;

		// Token: 0x040037CD RID: 14285
		[Tooltip("The new light intensity to set to.")]
		public FsmFloat newLightIntensity;

		// Token: 0x040037CE RID: 14286
		private TalkDoerLite m_talkDoer;

		// Token: 0x040037CF RID: 14287
		private Teleport.State m_state;

		// Token: 0x040037D0 RID: 14288
		private Teleport.Mode m_submode;

		// Token: 0x040037D1 RID: 14289
		private float m_stateTimer;

		// Token: 0x040037D2 RID: 14290
		private IEnumerator m_coroutine;

		// Token: 0x040037D3 RID: 14291
		private SetBraveLightIntensity m_lightIntensityAction;

		// Token: 0x040037D4 RID: 14292
		private float m_cachedLightIntensity;

		// Token: 0x02000CCC RID: 3276
		public enum Mode
		{
			// Token: 0x040037D6 RID: 14294
			Out,
			// Token: 0x040037D7 RID: 14295
			In,
			// Token: 0x040037D8 RID: 14296
			Both
		}

		// Token: 0x02000CCD RID: 3277
		public new enum State
		{
			// Token: 0x040037DA RID: 14298
			TeleportOut,
			// Token: 0x040037DB RID: 14299
			MidStep,
			// Token: 0x040037DC RID: 14300
			TeleportIn
		}

		// Token: 0x02000CCE RID: 3278
		public enum Timing
		{
			// Token: 0x040037DE RID: 14302
			Simultaneous,
			// Token: 0x040037DF RID: 14303
			VfxThenAnimation,
			// Token: 0x040037E0 RID: 14304
			AnimationThenVfx,
			// Token: 0x040037E1 RID: 14305
			Delays
		}
	}
}
