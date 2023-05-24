using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x020004F8 RID: 1272
	public class TweenTimeline : TweenBase, IEnumerable<TweenBase>, IEnumerable
	{
		// Token: 0x06001E74 RID: 7796 RVA: 0x0008AF5C File Offset: 0x0008915C
		public static TweenTimeline Obtain()
		{
			if (TweenTimeline.pool.Count > 0)
			{
				TweenTimeline tweenTimeline = (TweenTimeline)TweenTimeline.pool[TweenTimeline.pool.Count - 1];
				TweenTimeline.pool.RemoveAt(TweenTimeline.pool.Count - 1);
				return tweenTimeline;
			}
			return new TweenTimeline();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0008AFB4 File Offset: 0x000891B4
		public void Release()
		{
			this.Stop();
			this.Reset();
			TweenTimeline.pool.Add(this);
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0008AFD0 File Offset: 0x000891D0
		public TweenTimeline Add(float time, params TweenBase[] tweens)
		{
			foreach (TweenBase tweenBase in tweens)
			{
				this.Duration = Mathf.Max(this.Delay + this.Duration, time + tweenBase.Delay + tweenBase.Duration + this.Delay);
				this.tweenList.Add(new TweenTimeline.Entry
				{
					Time = time,
					Tween = tweenBase
				});
			}
			return this;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0008B048 File Offset: 0x00089248
		public override TweenBase Play()
		{
			this.pending.AddRange(this.tweenList);
			this.pending.Sort();
			this.triggered.Clear();
			if (this.Delay > 0f)
			{
				for (int i = 0; i < this.pending.Count; i++)
				{
					this.pending[i] = new TweenTimeline.Entry
					{
						Time = this.pending[i].Time + this.Delay,
						Tween = this.pending[i].Tween
					};
				}
			}
			base.State = TweenState.Playing;
			this.CurrentTime = 0f;
			this.startTime = base.getCurrentTime();
			base.registerWithTweenManager();
			this.raiseStarted();
			return this;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0008B128 File Offset: 0x00089328
		public override TweenBase Stop()
		{
			if (base.State == TweenState.Stopped)
			{
				return this;
			}
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				this.tweenList[i].Tween.Stop();
			}
			this.pending.Clear();
			this.triggered.Clear();
			return base.Stop();
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0008B194 File Offset: 0x00089394
		public override TweenBase Pause()
		{
			if (base.State != TweenState.Playing && base.State != TweenState.Started)
			{
				return this;
			}
			for (int i = 0; i < this.triggered.Count; i++)
			{
				this.triggered[i].Tween.Pause();
			}
			return base.Pause();
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0008B1F8 File Offset: 0x000893F8
		public override TweenBase Resume()
		{
			if (base.State != TweenState.Paused)
			{
				return this;
			}
			for (int i = 0; i < this.triggered.Count; i++)
			{
				this.triggered[i].Tween.Resume();
			}
			return base.Resume();
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0008B250 File Offset: 0x00089450
		public override TweenBase SetIsTimeScaleIndependent(bool isTimeScaleIndependent)
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				TweenBase tween = this.tweenList[i].Tween;
				tween.SetIsTimeScaleIndependent(isTimeScaleIndependent);
			}
			return base.SetIsTimeScaleIndependent(isTimeScaleIndependent);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0008B2A0 File Offset: 0x000894A0
		public TweenTimeline SetLoopType(TweenLoopType loopType)
		{
			if (loopType != TweenLoopType.None && loopType != TweenLoopType.Loop)
			{
				throw new ArgumentException("LoopType may only be one of the following values: TweenLoopType.None, TweenLoopType.Loop");
			}
			this.LoopType = loopType;
			return this;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0008B2C4 File Offset: 0x000894C4
		public TweenTimeline SetLoopCount(int loopCount)
		{
			this.LoopCount = loopCount;
			return this;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0008B2D0 File Offset: 0x000894D0
		internal override float CalculateTotalDuration()
		{
			float num = 0f;
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				TweenTimeline.Entry entry = this.tweenList[i];
				if (entry.Tween != null)
				{
					num = Mathf.Max(num, entry.Time + entry.Tween.CalculateTotalDuration());
				}
			}
			if (this.LoopCount > 0)
			{
				num *= (float)this.LoopCount;
			}
			else if (this.LoopType != TweenLoopType.None)
			{
				num = float.PositiveInfinity;
			}
			return this.Delay + num;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0008B36C File Offset: 0x0008956C
		protected override void Reset()
		{
			this.tweenList.Clear();
			this.pending.Clear();
			this.triggered.Clear();
			base.Reset();
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0008B398 File Offset: 0x00089598
		public override void Update()
		{
			if (base.State != TweenState.Started && base.State != TweenState.Playing)
			{
				return;
			}
			float num = base.getCurrentTime() - this.startTime;
			while (this.pending.Count > 0)
			{
				TweenTimeline.Entry entry = this.pending[0];
				if (entry.Time > num)
				{
					break;
				}
				this.pending.RemoveAt(0);
				this.triggered.Add(entry);
				entry.Tween.Play();
			}
			if (this.allTweensComplete())
			{
				if (this.LoopType == TweenLoopType.Loop && --this.LoopCount != 0)
				{
					if (this.TweenLoopCompleted != null)
					{
						this.TweenLoopCompleted(this);
					}
					this.Rewind();
					this.Play();
				}
				else
				{
					this.Stop();
					this.raiseCompleted();
					if (this.AutoCleanup)
					{
						this.Release();
					}
				}
				return;
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0008B49C File Offset: 0x0008969C
		private bool allTweensComplete()
		{
			if (this.pending.Count > 0)
			{
				return false;
			}
			for (int i = 0; i < this.triggered.Count; i++)
			{
				if (this.triggered[i].Tween.State != TweenState.Stopped)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0008B4FC File Offset: 0x000896FC
		public IEnumerator<TweenBase> GetEnumerator()
		{
			return this.enumerateTweens();
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0008B504 File Offset: 0x00089704
		private IEnumerator<TweenBase> enumerateTweens()
		{
			int index = 0;
			while (index < this.tweenList.Count)
			{
				List<TweenTimeline.Entry> list = this.tweenList;
				int num;
				index = (num = index) + 1;
				yield return list[num].Tween;
			}
			yield break;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0008B520 File Offset: 0x00089720
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.enumerateTweens();
		}

		// Token: 0x040016CC RID: 5836
		private List<TweenTimeline.Entry> tweenList = new List<TweenTimeline.Entry>();

		// Token: 0x040016CD RID: 5837
		private List<TweenTimeline.Entry> pending = new List<TweenTimeline.Entry>();

		// Token: 0x040016CE RID: 5838
		private List<TweenTimeline.Entry> triggered = new List<TweenTimeline.Entry>();

		// Token: 0x040016CF RID: 5839
		private static List<object> pool = new List<object>();

		// Token: 0x020004F9 RID: 1273
		private struct Entry : IComparable<TweenTimeline.Entry>
		{
			// Token: 0x06001E86 RID: 7814 RVA: 0x0008B534 File Offset: 0x00089734
			public int CompareTo(TweenTimeline.Entry other)
			{
				return this.Time.CompareTo(other.Time);
			}

			// Token: 0x040016D0 RID: 5840
			public float Time;

			// Token: 0x040016D1 RID: 5841
			public TweenBase Tween;
		}
	}
}
