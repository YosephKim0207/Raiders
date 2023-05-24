using System;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004D6 RID: 1238
	[InspectorGroupOrder(new string[] { "General", "Animation", "Looping", "Parameters" })]
	[AddComponentMenu("Daikon Forge/Tween/Camera Shake")]
	public class TweenCameraShake : TweenComponentBase
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x00088154 File Offset: 0x00086354
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x0008815C File Offset: 0x0008635C
		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x00088170 File Offset: 0x00086370
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x00088178 File Offset: 0x00086378
		public float ShakeMagnitude
		{
			get
			{
				return this.shakeMagnitude;
			}
			set
			{
				if (value != this.shakeMagnitude)
				{
					this.shakeMagnitude = value;
					this.Stop();
				}
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x00088194 File Offset: 0x00086394
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x0008819C File Offset: 0x0008639C
		public float ShakeSpeed
		{
			get
			{
				return this.shakeSpeed;
			}
			set
			{
				if (value != this.shakeSpeed)
				{
					this.shakeSpeed = value;
					this.Stop();
				}
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x000881B8 File Offset: 0x000863B8
		public override TweenBase BaseTween
		{
			get
			{
				this.configureTween();
				return this.tween;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x000881C8 File Offset: 0x000863C8
		public override TweenState State
		{
			get
			{
				if (this.tween == null)
				{
					return TweenState.Stopped;
				}
				return this.tween.State;
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000881E4 File Offset: 0x000863E4
		public virtual void OnApplicationQuit()
		{
			this.cleanup();
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000881EC File Offset: 0x000863EC
		public override void OnDisable()
		{
			base.OnDisable();
			this.cleanup();
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000881FC File Offset: 0x000863FC
		public override void Play()
		{
			if (this.State != TweenState.Stopped)
			{
				this.Stop();
			}
			this.configureTween();
			this.validateTweenConfiguration();
			this.tween.Play();
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00088228 File Offset: 0x00086428
		public override void Stop()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Stop();
			}
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00088248 File Offset: 0x00086448
		public override void Pause()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Pause();
			}
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x00088268 File Offset: 0x00086468
		public override void Resume()
		{
			if (base.IsPaused)
			{
				this.validateTweenConfiguration();
				this.tween.Resume();
			}
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00088288 File Offset: 0x00086488
		public override void Rewind()
		{
			this.validateTweenConfiguration();
			this.tween.Rewind();
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0008829C File Offset: 0x0008649C
		public override void FastForward()
		{
			this.validateTweenConfiguration();
			this.tween.FastForward();
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000882B0 File Offset: 0x000864B0
		protected void cleanup()
		{
			if (this.tween != null)
			{
				this.tween.Stop();
				this.tween.Release();
				this.tween = null;
			}
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000882DC File Offset: 0x000864DC
		protected void validateTweenConfiguration()
		{
			this.loopCount = Mathf.Max(0, this.loopCount);
			if (base.gameObject.GetComponent<Camera>() == null)
			{
				throw new InvalidOperationException("Camera not found");
			}
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00088314 File Offset: 0x00086514
		protected void configureTween()
		{
			Camera component = base.gameObject.GetComponent<Camera>();
			if (this.tween == null)
			{
				this.tween = (TweenShake)component.ShakePosition(true).OnStarted(delegate(TweenBase x)
				{
					this.onStarted();
				}).OnStopped(delegate(TweenBase x)
				{
					this.onStopped();
				})
					.OnPaused(delegate(TweenBase x)
					{
						this.onPaused();
					})
					.OnResumed(delegate(TweenBase x)
					{
						this.onResumed();
					})
					.OnLoopCompleted(delegate(TweenBase x)
					{
						this.onLoopCompleted();
					})
					.OnCompleted(delegate(TweenBase x)
					{
						this.onCompleted();
					});
			}
			this.tween.SetDelay(this.startDelay).SetDuration(this.Duration).SetShakeMagnitude(this.ShakeMagnitude)
				.SetShakeSpeed(this.ShakeSpeed);
		}

		// Token: 0x0400166B RID: 5739
		[SerializeField]
		[Inspector("Parameters", 0, Label = "Duration")]
		protected float duration = 1f;

		// Token: 0x0400166C RID: 5740
		[Inspector("Parameters", 0, Label = "Magnitude")]
		[SerializeField]
		protected float shakeMagnitude = 0.25f;

		// Token: 0x0400166D RID: 5741
		[Inspector("Parameters", 0, Label = "Speed")]
		[SerializeField]
		protected float shakeSpeed = 13f;

		// Token: 0x0400166E RID: 5742
		protected TweenShake tween;
	}
}
