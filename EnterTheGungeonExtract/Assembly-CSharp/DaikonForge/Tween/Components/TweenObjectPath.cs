using System;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E5 RID: 1253
	[InspectorGroupOrder(new string[] { "General", "Path", "Animation", "Looping" })]
	[AddComponentMenu("Daikon Forge/Tween/Move Along Path")]
	public class TweenObjectPath : TweenComponentBase
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x000894E8 File Offset: 0x000876E8
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x000894F0 File Offset: 0x000876F0
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

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x00089504 File Offset: 0x00087704
		public override TweenBase BaseTween
		{
			get
			{
				this.configureTween();
				return this.tween;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x00089514 File Offset: 0x00087714
		// (set) Token: 0x06001DDC RID: 7644 RVA: 0x0008951C File Offset: 0x0008771C
		public SplineObject Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.cleanup();
				this.path = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0008952C File Offset: 0x0008772C
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

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x00089548 File Offset: 0x00087748
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x00089550 File Offset: 0x00087750
		public TweenDirection PlayDirection
		{
			get
			{
				return this.playDirection;
			}
			set
			{
				this.playDirection = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x00089570 File Offset: 0x00087770
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x00089578 File Offset: 0x00087778
		public bool OrientToPath
		{
			get
			{
				return this.orientToPath;
			}
			set
			{
				this.orientToPath = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.tween.Release();
					this.tween = null;
					this.Play();
				}
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000895AC File Offset: 0x000877AC
		public virtual void OnApplicationQuit()
		{
			this.cleanup();
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000895B4 File Offset: 0x000877B4
		public override void OnDisable()
		{
			base.OnDisable();
			this.cleanup();
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000895C4 File Offset: 0x000877C4
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

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000895F0 File Offset: 0x000877F0
		public override void Stop()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Stop();
			}
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x00089610 File Offset: 0x00087810
		public override void Pause()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Pause();
			}
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x00089630 File Offset: 0x00087830
		public override void Resume()
		{
			if (base.IsPaused)
			{
				this.validateTweenConfiguration();
				this.tween.Resume();
			}
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00089650 File Offset: 0x00087850
		public override void Rewind()
		{
			this.validateTweenConfiguration();
			this.tween.Rewind();
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x00089664 File Offset: 0x00087864
		public override void FastForward()
		{
			this.validateTweenConfiguration();
			this.tween.FastForward();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x00089678 File Offset: 0x00087878
		protected void cleanup()
		{
			if (this.tween != null)
			{
				this.tween.Stop();
				this.tween.Release();
				this.tween = null;
			}
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000896A4 File Offset: 0x000878A4
		protected void validateTweenConfiguration()
		{
			this.loopCount = Mathf.Max(0, this.loopCount);
			if (this.Path == null)
			{
				throw new InvalidOperationException("The Path property cannot be NULL");
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000896D4 File Offset: 0x000878D4
		protected void configureTween()
		{
			this.Path.CalculateSpline();
			if (this.tween == null)
			{
				this.tween = (Tween<float>)base.transform.TweenPath(this.Path.Spline, this.orientToPath).OnStarted(delegate(TweenBase x)
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
			this.Path.CalculateSpline();
			this.tween.SetDelay(this.startDelay).SetDuration(this.duration).SetLoopType(this.loopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x04001698 RID: 5784
		[Inspector("Animation", 0, Label = "Duration", Tooltip = "How long the Tween should take to complete the animation")]
		[SerializeField]
		protected float duration = 1f;

		// Token: 0x04001699 RID: 5785
		[Inspector("Path", 0, Label = "Path", Tooltip = "The path for the object to follow")]
		[SerializeField]
		protected SplineObject path;

		// Token: 0x0400169A RID: 5786
		[SerializeField]
		[Inspector("Animation", 1, Label = "Orient To Path", Tooltip = "If set to TRUE, will orient the object to face the direction of the path")]
		protected bool orientToPath = true;

		// Token: 0x0400169B RID: 5787
		[SerializeField]
		protected TweenDirection playDirection;

		// Token: 0x0400169C RID: 5788
		protected Tween<float> tween;
	}
}
