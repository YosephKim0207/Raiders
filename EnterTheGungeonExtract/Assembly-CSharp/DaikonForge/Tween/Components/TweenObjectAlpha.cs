using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E3 RID: 1251
	[AddComponentMenu("Daikon Forge/Tween/Object Alpha")]
	public class TweenObjectAlpha : TweenComponent<float>
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x00088FD4 File Offset: 0x000871D4
		// (set) Token: 0x06001DC1 RID: 7617 RVA: 0x00088FDC File Offset: 0x000871DC
		public Component Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
				this.Stop();
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00088FEC File Offset: 0x000871EC
		protected override void validateTweenConfiguration()
		{
			if (this.target == null)
			{
				throw new InvalidOperationException("The Target cannot be NULL");
			}
			if (this.startValue < 0f || this.startValue > 1f)
			{
				throw new InvalidOperationException("The Start Value must be between 0 and 1");
			}
			if (this.endValue < 0f || this.endValue > 1f)
			{
				throw new InvalidOperationException("The End Value must be between 0 and 1");
			}
			base.validateTweenConfiguration();
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00089074 File Offset: 0x00087274
		protected override void configureTween()
		{
			if (this.target == null)
			{
				this.target = base.gameObject.GetComponent<Renderer>();
				if (this.target == null)
				{
					if (this.tween != null)
					{
						this.tween.Stop();
						this.tween.Release();
						this.tween = null;
					}
					return;
				}
			}
			if (this.tween == null)
			{
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<float>)this.Target.TweenAlpha().SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
				{
					this.onStarted();
				})
					.OnStopped(delegate(TweenBase x)
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
			float currentValue = this.tween.CurrentValue;
			float num = this.startValue;
			if (this.startValueType == TweenStartValueType.SyncOnRun)
			{
				num = currentValue;
			}
			float num2 = this.endValue;
			if (this.endValueType == TweenEndValueType.SyncOnRun)
			{
				num2 = currentValue;
			}
			else if (this.endValueType == TweenEndValueType.Relative)
			{
				num2 += num;
			}
			this.tween.SetStartValue(num).SetEndValue(num2).SetDelay(this.startDelay, this.assignStartValueBeforeDelay)
				.SetDuration(this.duration)
				.SetLoopType(base.LoopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00089224 File Offset: 0x00087424
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x04001694 RID: 5780
		[SerializeField]
		protected Component target;

		// Token: 0x04001695 RID: 5781
		private TweenEasingCallback easingFunc;
	}
}
