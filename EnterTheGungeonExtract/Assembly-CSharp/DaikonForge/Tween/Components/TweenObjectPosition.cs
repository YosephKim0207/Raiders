using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E6 RID: 1254
	[AddComponentMenu("Daikon Forge/Tween/Object Position")]
	public class TweenObjectPosition : TweenComponent<Vector3>
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x00089808 File Offset: 0x00087A08
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x00089810 File Offset: 0x00087A10
		public bool UseLocalPosition
		{
			get
			{
				return this.useLocalPosition;
			}
			set
			{
				this.useLocalPosition = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x00089830 File Offset: 0x00087A30
		protected override void configureTween()
		{
			if (this.tween == null)
			{
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<Vector3>)base.transform.TweenPosition(this.useLocalPosition).SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
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
			Vector3 vector = ((!this.useLocalPosition) ? base.transform.position : base.transform.localPosition);
			Vector3 vector2 = this.startValue;
			if (this.startValueType == TweenStartValueType.SyncOnRun)
			{
				vector2 = vector;
			}
			Vector3 vector3 = this.endValue;
			if (this.endValueType == TweenEndValueType.SyncOnRun)
			{
				vector3 = vector;
			}
			else if (this.endValueType == TweenEndValueType.Relative)
			{
				vector3 += vector2;
			}
			this.tween.SetStartValue(vector2).SetEndValue(vector3).SetDelay(this.startDelay, this.assignStartValueBeforeDelay)
				.SetDuration(this.duration)
				.SetLoopType(base.LoopType)
				.SetLoopCount(this.loopCount)
				.SetPlayDirection(this.playDirection);
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000899A8 File Offset: 0x00087BA8
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x0400169D RID: 5789
		[SerializeField]
		protected bool useLocalPosition;

		// Token: 0x0400169E RID: 5790
		private TweenEasingCallback easingFunc;
	}
}
