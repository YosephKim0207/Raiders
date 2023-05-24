using System;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004DC RID: 1244
	[InspectorGroupOrder(new string[] { "General", "Animation", "Looping", "Values" })]
	public abstract class TweenComponent<T> : TweenComponentBase
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x00088A70 File Offset: 0x00086C70
		public override TweenBase BaseTween
		{
			get
			{
				this.configureTween();
				return this.tween;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00088A80 File Offset: 0x00086C80
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x00088A88 File Offset: 0x00086C88
		public AnimationCurve AnimationCurve
		{
			get
			{
				return this.animCurve;
			}
			set
			{
				this.animCurve = value;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x00088A94 File Offset: 0x00086C94
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x00088A9C File Offset: 0x00086C9C
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

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x00088AB0 File Offset: 0x00086CB0
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x00088AB8 File Offset: 0x00086CB8
		public T StartValue
		{
			get
			{
				return this.startValue;
			}
			set
			{
				this.startValue = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x00088AD8 File Offset: 0x00086CD8
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x00088AE0 File Offset: 0x00086CE0
		public TweenStartValueType StartValueType
		{
			get
			{
				return this.startValueType;
			}
			set
			{
				this.startValueType = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x00088B00 File Offset: 0x00086D00
		// (set) Token: 0x06001D91 RID: 7569 RVA: 0x00088B08 File Offset: 0x00086D08
		public T EndValue
		{
			get
			{
				return this.endValue;
			}
			set
			{
				this.endValue = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x00088B28 File Offset: 0x00086D28
		// (set) Token: 0x06001D93 RID: 7571 RVA: 0x00088B30 File Offset: 0x00086D30
		public TweenEndValueType EndValueType
		{
			get
			{
				return this.endValueType;
			}
			set
			{
				this.endValueType = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x00088B50 File Offset: 0x00086D50
		// (set) Token: 0x06001D95 RID: 7573 RVA: 0x00088B58 File Offset: 0x00086D58
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

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x00088B78 File Offset: 0x00086D78
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

		// Token: 0x06001D97 RID: 7575 RVA: 0x00088B94 File Offset: 0x00086D94
		public virtual void OnApplicationQuit()
		{
			this.cleanup();
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00088B9C File Offset: 0x00086D9C
		public override void OnDisable()
		{
			base.OnDisable();
			this.cleanup();
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x00088BAC File Offset: 0x00086DAC
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

		// Token: 0x06001D9A RID: 7578 RVA: 0x00088BD8 File Offset: 0x00086DD8
		public override void Stop()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Stop();
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00088BF8 File Offset: 0x00086DF8
		public override void Pause()
		{
			if (base.IsPlaying)
			{
				this.validateTweenConfiguration();
				this.tween.Pause();
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x00088C18 File Offset: 0x00086E18
		public override void Resume()
		{
			if (base.IsPaused)
			{
				this.validateTweenConfiguration();
				this.tween.Resume();
			}
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00088C38 File Offset: 0x00086E38
		public override void Rewind()
		{
			this.validateTweenConfiguration();
			this.tween.Rewind();
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00088C4C File Offset: 0x00086E4C
		public override void FastForward()
		{
			this.validateTweenConfiguration();
			this.tween.FastForward();
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00088C60 File Offset: 0x00086E60
		protected virtual void cleanup()
		{
			if (this.tween != null)
			{
				this.tween.Stop();
				this.tween.Release();
				this.tween = null;
			}
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00088C8C File Offset: 0x00086E8C
		protected virtual void validateTweenConfiguration()
		{
			this.loopCount = Mathf.Max(0, this.loopCount);
			if (this.tween == null)
			{
				throw new InvalidOperationException("The tween has not been properly configured");
			}
		}

		// Token: 0x06001DA1 RID: 7585
		protected abstract void configureTween();

		// Token: 0x04001681 RID: 5761
		[Inspector("Animation", Order = 4, Tooltip = "How long the Tween should take to complete the animation")]
		[SerializeField]
		protected float duration = 1f;

		// Token: 0x04001682 RID: 5762
		[Inspector("Animation", Order = 2, Tooltip = "The type of easing, if any, to apply to the animation")]
		[SerializeField]
		protected EasingType easingType;

		// Token: 0x04001683 RID: 5763
		[Inspector("Animation", Order = 3, Label = "Curve", Tooltip = "An animation curve can be used to modify the animation timeline")]
		[SerializeField]
		protected AnimationCurve animCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});

		// Token: 0x04001684 RID: 5764
		[SerializeField]
		[Inspector("Animation", Order = 5, Label = "Direction")]
		protected TweenDirection playDirection;

		// Token: 0x04001685 RID: 5765
		[Inspector("Values", Order = 0)]
		[SerializeField]
		protected TweenStartValueType startValueType;

		// Token: 0x04001686 RID: 5766
		[Inspector("Values", Order = 1)]
		[SerializeField]
		protected T startValue;

		// Token: 0x04001687 RID: 5767
		[SerializeField]
		[Inspector("Values", Order = 2)]
		protected TweenEndValueType endValueType;

		// Token: 0x04001688 RID: 5768
		[SerializeField]
		[Inspector("Values", Order = 3)]
		protected T endValue;

		// Token: 0x04001689 RID: 5769
		protected Tween<T> tween;
	}
}
