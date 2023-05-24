using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E7 RID: 1255
	[AddComponentMenu("Daikon Forge/Tween/Object Rotation")]
	public class TweenObjectRotation : TweenComponent<Vector3>
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001DFF RID: 7679 RVA: 0x00089A10 File Offset: 0x00087C10
		// (set) Token: 0x06001E00 RID: 7680 RVA: 0x00089A18 File Offset: 0x00087C18
		public bool UseLocalRotation
		{
			get
			{
				return this.useLocalRotation;
			}
			set
			{
				this.useLocalRotation = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x00089A38 File Offset: 0x00087C38
		// (set) Token: 0x06001E02 RID: 7682 RVA: 0x00089A40 File Offset: 0x00087C40
		public bool UseShortestPath
		{
			get
			{
				return this.useShortestPath;
			}
			set
			{
				this.useShortestPath = value;
				if (this.State != TweenState.Stopped)
				{
					this.Stop();
					this.Play();
				}
			}
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x00089A60 File Offset: 0x00087C60
		protected override void configureTween()
		{
			if (this.tween == null)
			{
				this.easingFunc = TweenEasingFunctions.GetFunction(this.easingType);
				this.tween = (Tween<Vector3>)base.transform.TweenRotation(this.useShortestPath, this.useLocalRotation).SetEasing(new TweenEasingCallback(this.modifyEasing)).OnStarted(delegate(TweenBase x)
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
			Vector3 vector = ((!this.useLocalRotation) ? base.transform.eulerAngles : base.transform.localEulerAngles);
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

		// Token: 0x06001E04 RID: 7684 RVA: 0x00089BDC File Offset: 0x00087DDC
		private float modifyEasing(float time)
		{
			if (this.animCurve != null)
			{
				time = this.animCurve.Evaluate(time);
			}
			return this.easingFunc(time);
		}

		// Token: 0x0400169F RID: 5791
		[SerializeField]
		protected bool useLocalRotation;

		// Token: 0x040016A0 RID: 5792
		[SerializeField]
		protected bool useShortestPath = true;

		// Token: 0x040016A1 RID: 5793
		private TweenEasingCallback easingFunc;
	}
}
