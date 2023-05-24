using System;
using System.Collections;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x020004FC RID: 1276
	public abstract class TweenBase : ITweenUpdatable
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x0008BB74 File Offset: 0x00089D74
		public float ElapsedTime
		{
			get
			{
				return this.getCurrentTime() - this.startTime;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x0008BB84 File Offset: 0x00089D84
		// (set) Token: 0x06001EB1 RID: 7857 RVA: 0x0008BB8C File Offset: 0x00089D8C
		public TweenState State { get; protected set; }

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0008BB98 File Offset: 0x00089D98
		public virtual TweenBase Play()
		{
			this.State = TweenState.Started;
			this.CurrentTime = 0f;
			this.startTime = this.getCurrentTime();
			this.registerWithTweenManager();
			this.raiseStarted();
			return this;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0008BBC8 File Offset: 0x00089DC8
		public virtual TweenBase Pause()
		{
			if (this.State != TweenState.Playing && this.State != TweenState.Started)
			{
				return this;
			}
			this.State = TweenState.Paused;
			this.raisePaused();
			return this;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0008BBF4 File Offset: 0x00089DF4
		public virtual TweenBase Resume()
		{
			if (this.State != TweenState.Paused)
			{
				return this;
			}
			this.State = TweenState.Playing;
			this.raiseResumed();
			return this;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0008BC14 File Offset: 0x00089E14
		public virtual TweenBase Stop()
		{
			if (this.State == TweenState.Stopped)
			{
				return this;
			}
			this.unregisterWithTweenManager();
			this.State = TweenState.Stopped;
			this.raiseStopped();
			return this;
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0008BC38 File Offset: 0x00089E38
		public virtual TweenBase Rewind()
		{
			this.CurrentTime = 0f;
			this.startTime = this.getCurrentTime();
			return this;
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0008BC54 File Offset: 0x00089E54
		public virtual TweenBase FastForward()
		{
			this.CurrentTime = 1f;
			return this;
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0008BC64 File Offset: 0x00089E64
		public virtual IEnumerator WaitForCompletion()
		{
			do
			{
				yield return null;
			}
			while (this.State != TweenState.Stopped);
			yield break;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0008BC80 File Offset: 0x00089E80
		public virtual TweenBase Chain(TweenBase tween)
		{
			return this.Chain(tween, null);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0008BC8C File Offset: 0x00089E8C
		public virtual TweenBase Chain(TweenBase tween, Action initFunction)
		{
			if (tween == null)
			{
				throw new ArgumentNullException("tween");
			}
			TweenCallback completedCallback = this.TweenCompleted;
			this.TweenCompleted = delegate(TweenBase sender)
			{
				if (completedCallback != null)
				{
					completedCallback(sender);
				}
				if (initFunction != null)
				{
					initFunction();
				}
				tween.Play();
			};
			return tween;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0008BCE8 File Offset: 0x00089EE8
		internal virtual float CalculateTotalDuration()
		{
			float num = this.Delay + this.Duration;
			if (this.LoopCount > 0)
			{
				num *= (float)this.LoopCount;
			}
			else if (this.LoopType != TweenLoopType.None)
			{
				num = float.PositiveInfinity;
			}
			return num;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0008BD30 File Offset: 0x00089F30
		public virtual TweenBase SetIsTimeScaleIndependent(bool isTimeScaleIndependent)
		{
			this.IsTimeScaleIndependent = isTimeScaleIndependent;
			return this;
		}

		// Token: 0x06001EBD RID: 7869
		public abstract void Update();

		// Token: 0x06001EBE RID: 7870 RVA: 0x0008BD3C File Offset: 0x00089F3C
		protected virtual void Reset()
		{
			this.Easing = new TweenEasingCallback(TweenEasingFunctions.Linear);
			this.LoopType = TweenLoopType.None;
			this.CurrentTime = 0f;
			this.Delay = 0f;
			this.AutoCleanup = false;
			this.IsTimeScaleIndependent = false;
			this.startTime = 0f;
			this.TweenLoopCompleted = null;
			this.TweenCompleted = null;
			this.TweenPaused = null;
			this.TweenResumed = null;
			this.TweenStarted = null;
			this.TweenStopped = null;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0008BDCC File Offset: 0x00089FCC
		protected void registerWithTweenManager()
		{
			if (!this.registered)
			{
				TweenManager.Instance.RegisterTween(this);
				this.registered = true;
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0008BDEC File Offset: 0x00089FEC
		protected void unregisterWithTweenManager()
		{
			if (this.registered)
			{
				TweenManager.Instance.UnregisterTween(this);
				this.registered = false;
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0008BE0C File Offset: 0x0008A00C
		protected float getTimeElapsed()
		{
			if (this.State == TweenState.Playing || this.State == TweenState.Started)
			{
				return Mathf.Min(this.getCurrentTime() - this.startTime, this.Duration);
			}
			return 0f;
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0008BE54 File Offset: 0x0008A054
		protected float getCurrentTime()
		{
			if (this.IsTimeScaleIndependent)
			{
				return TweenManager.Instance.realTimeSinceStartup;
			}
			return Time.time;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0008BE74 File Offset: 0x0008A074
		protected float getDeltaTime()
		{
			if (this.IsTimeScaleIndependent)
			{
				return TweenManager.realDeltaTime;
			}
			return BraveTime.DeltaTime;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0008BE8C File Offset: 0x0008A08C
		public TweenBase OnLoopCompleted(TweenCallback function)
		{
			this.TweenLoopCompleted = function;
			return this;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0008BE98 File Offset: 0x0008A098
		public TweenBase OnCompleted(TweenCallback function)
		{
			this.TweenCompleted = function;
			return this;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0008BEA4 File Offset: 0x0008A0A4
		public TweenBase OnPaused(TweenCallback function)
		{
			this.TweenPaused = function;
			return this;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0008BEB0 File Offset: 0x0008A0B0
		public TweenBase OnResumed(TweenCallback function)
		{
			this.TweenResumed = function;
			return this;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0008BEBC File Offset: 0x0008A0BC
		public TweenBase OnStarted(TweenCallback function)
		{
			this.TweenStarted = function;
			return this;
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x0008BEC8 File Offset: 0x0008A0C8
		public TweenBase OnStopped(TweenCallback function)
		{
			this.TweenStopped = function;
			return this;
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0008BED4 File Offset: 0x0008A0D4
		public virtual TweenBase Wait(float seconds)
		{
			return this.Chain(new TweenWait(seconds));
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0008BEE4 File Offset: 0x0008A0E4
		protected virtual void raisePaused()
		{
			if (this.TweenPaused != null)
			{
				this.TweenPaused(this);
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0008BF00 File Offset: 0x0008A100
		protected virtual void raiseResumed()
		{
			if (this.TweenResumed != null)
			{
				this.TweenResumed(this);
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0008BF1C File Offset: 0x0008A11C
		protected virtual void raiseStarted()
		{
			if (this.TweenStarted != null)
			{
				this.TweenStarted(this);
			}
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0008BF38 File Offset: 0x0008A138
		protected virtual void raiseStopped()
		{
			if (this.TweenStopped != null)
			{
				this.TweenStopped(this);
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0008BF54 File Offset: 0x0008A154
		protected virtual void raiseCompleted()
		{
			if (this.TweenCompleted != null)
			{
				this.TweenCompleted(this);
			}
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0008BF70 File Offset: 0x0008A170
		public override string ToString()
		{
			return string.IsNullOrEmpty(this.Name) ? base.ToString() : this.Name;
		}

		// Token: 0x040016E2 RID: 5858
		public string Name;

		// Token: 0x040016E3 RID: 5859
		public float CurrentTime;

		// Token: 0x040016E4 RID: 5860
		public float Duration;

		// Token: 0x040016E5 RID: 5861
		public float Delay;

		// Token: 0x040016E6 RID: 5862
		public TweenLoopType LoopType;

		// Token: 0x040016E7 RID: 5863
		public int LoopCount;

		// Token: 0x040016E8 RID: 5864
		public TweenEasingCallback Easing;

		// Token: 0x040016E9 RID: 5865
		public bool AutoCleanup;

		// Token: 0x040016EA RID: 5866
		public bool IsTimeScaleIndependent;

		// Token: 0x040016EC RID: 5868
		public TweenCallback TweenStarted;

		// Token: 0x040016ED RID: 5869
		public TweenCallback TweenStopped;

		// Token: 0x040016EE RID: 5870
		public TweenCallback TweenPaused;

		// Token: 0x040016EF RID: 5871
		public TweenCallback TweenResumed;

		// Token: 0x040016F0 RID: 5872
		public TweenCallback TweenCompleted;

		// Token: 0x040016F1 RID: 5873
		public TweenCallback TweenLoopCompleted;

		// Token: 0x040016F2 RID: 5874
		protected float startTime;

		// Token: 0x040016F3 RID: 5875
		protected bool registered;
	}
}
