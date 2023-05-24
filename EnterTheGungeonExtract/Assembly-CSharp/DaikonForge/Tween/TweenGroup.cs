using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200051A RID: 1306
	public class TweenGroup : TweenBase, IPoolableObject, IEnumerable<TweenBase>, IEnumerable
	{
		// Token: 0x06001F51 RID: 8017 RVA: 0x0008CC78 File Offset: 0x0008AE78
		public static TweenGroup Obtain()
		{
			if (TweenGroup.pool.Count > 0)
			{
				TweenGroup tweenGroup = TweenGroup.pool[TweenGroup.pool.Count - 1];
				TweenGroup.pool.RemoveAt(TweenGroup.pool.Count - 1);
				return tweenGroup;
			}
			return new TweenGroup();
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0008CCCC File Offset: 0x0008AECC
		public void Release()
		{
			this.Stop();
			if (!TweenGroup.pool.Contains(this))
			{
				this.Reset();
				TweenGroup.pool.Add(this);
			}
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0008CCF8 File Offset: 0x0008AEF8
		public TweenGroup SetAutoCleanup(bool autoCleanup)
		{
			this.AutoCleanup = true;
			return this;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0008CD04 File Offset: 0x0008AF04
		public override TweenBase SetIsTimeScaleIndependent(bool isTimeScaleIndependent)
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				this.tweenList[i].SetIsTimeScaleIndependent(isTimeScaleIndependent);
			}
			return base.SetIsTimeScaleIndependent(isTimeScaleIndependent);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0008CD48 File Offset: 0x0008AF48
		public TweenGroup SetMode(TweenGroupMode mode)
		{
			this.Mode = mode;
			return this;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0008CD54 File Offset: 0x0008AF54
		public TweenGroup SetDelay(float seconds)
		{
			this.Delay = seconds;
			return this;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0008CD60 File Offset: 0x0008AF60
		public TweenGroup SetLoopType(TweenLoopType loopType)
		{
			if (loopType != TweenLoopType.None && loopType != TweenLoopType.Loop)
			{
				throw new ArgumentException("LoopType may only be one of the following values: TweenLoopType.None, TweenLoopType.Loop");
			}
			this.LoopType = loopType;
			return this;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0008CD84 File Offset: 0x0008AF84
		public TweenGroup SetLoopCount(int loopCount)
		{
			this.LoopCount = loopCount;
			return this;
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0008CD90 File Offset: 0x0008AF90
		public TweenGroup AppendTween(params TweenBase[] tweens)
		{
			if (tweens == null || tweens.Length == 0)
			{
				throw new ArgumentException("You must provide at least one Tween");
			}
			this.tweenList.AddRange(tweens);
			return this;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0008CDB8 File Offset: 0x0008AFB8
		public TweenGroup AppendDelay(float seconds)
		{
			this.tweenList.Add(TweenWait.Obtain(seconds));
			return this;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0008CDCC File Offset: 0x0008AFCC
		public TweenGroup ClearTweens()
		{
			this.tweenList.Clear();
			return this;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0008CDDC File Offset: 0x0008AFDC
		public override TweenBase Play()
		{
			if (this.LoopType != TweenLoopType.None && this.LoopType != TweenLoopType.Loop)
			{
				throw new ArgumentException("LoopType may only be one of the following values: TweenLoopType.None, TweenLoopType.Loop");
			}
			this.currentTween = null;
			this.currentIndex = -1;
			base.Play();
			return this;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0008CE18 File Offset: 0x0008B018
		public override TweenBase Stop()
		{
			if (base.State != TweenState.Stopped)
			{
				for (int i = 0; i < this.tweenList.Count; i++)
				{
					this.tweenList[i].Stop();
				}
				this.currentTween = null;
				this.currentIndex = -1;
			}
			return base.Stop();
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0008CE74 File Offset: 0x0008B074
		public override TweenBase Pause()
		{
			if (base.State == TweenState.Playing || base.State == TweenState.Started)
			{
				if (this.Mode == TweenGroupMode.Concurrent)
				{
					this.pauseAllTweens();
				}
				else if (this.currentTween != null)
				{
					this.currentTween.Pause();
				}
			}
			return base.Pause();
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0008CED0 File Offset: 0x0008B0D0
		public override TweenBase Resume()
		{
			if (base.State == TweenState.Paused)
			{
				if (this.Mode == TweenGroupMode.Concurrent)
				{
					this.resumeAllTweens();
				}
				else if (this.currentTween != null)
				{
					this.currentTween.Resume();
				}
			}
			base.Resume();
			return this;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0008CF20 File Offset: 0x0008B120
		public override TweenBase Rewind()
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				this.tweenList[i].Rewind();
			}
			this.currentTween = null;
			this.currentIndex = -1;
			return base.Rewind();
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0008CF70 File Offset: 0x0008B170
		public override void Update()
		{
			if (this.tweenList.Count == 0)
			{
				return;
			}
			if (base.State == TweenState.Started)
			{
				float currentTime = base.getCurrentTime();
				if (currentTime < this.startTime + this.Delay)
				{
					return;
				}
				if (this.Mode == TweenGroupMode.Concurrent)
				{
					this.startAllTweens();
				}
				else if (!this.nextTween())
				{
					return;
				}
				this.startTime = currentTime;
				this.CurrentTime = 0f;
				base.State = TweenState.Playing;
			}
			else if (base.State != TweenState.Playing)
			{
				return;
			}
			if (this.Mode == TweenGroupMode.Concurrent)
			{
				if (this.allTweensComplete())
				{
					if (this.LoopType == TweenLoopType.Loop && --this.LoopCount != 0)
					{
						if (this.TweenLoopCompleted != null)
						{
							this.TweenLoopCompleted(this);
						}
						if (base.State == TweenState.Playing)
						{
							this.Rewind();
							this.Play();
						}
					}
					else
					{
						this.onGroupComplete();
					}
				}
				return;
			}
			if (this.currentTween.State == TweenState.Stopped)
			{
				bool flag = !this.nextTween();
				if (flag)
				{
					this.Stop();
					this.raiseCompleted();
					return;
				}
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0008D0A8 File Offset: 0x0008B2A8
		protected override void Reset()
		{
			this.Stop();
			if (this.AutoCleanup)
			{
				this.cleanUp();
			}
			base.Reset();
			this.Mode = TweenGroupMode.Sequential;
			this.AutoCleanup = false;
			this.tweenList.Clear();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0008D0E4 File Offset: 0x0008B2E4
		internal override float CalculateTotalDuration()
		{
			float num = 0f;
			if (this.Mode == TweenGroupMode.Sequential)
			{
				for (int i = 0; i < this.tweenList.Count; i++)
				{
					TweenBase tweenBase = this.tweenList[i];
					if (tweenBase != null)
					{
						num += tweenBase.CalculateTotalDuration();
					}
				}
			}
			else
			{
				for (int j = 0; j < this.tweenList.Count; j++)
				{
					TweenBase tweenBase2 = this.tweenList[j];
					if (tweenBase2 != null)
					{
						num = Mathf.Max(num, tweenBase2.CalculateTotalDuration());
					}
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

		// Token: 0x06001F64 RID: 8036 RVA: 0x0008D1BC File Offset: 0x0008B3BC
		private void onGroupComplete()
		{
			this.Stop();
			this.raiseCompleted();
			if (this.autoCleanup)
			{
				this.cleanUp();
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0008D1DC File Offset: 0x0008B3DC
		private void startAllTweens()
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				TweenBase tweenBase = this.tweenList[i];
				if (tweenBase != null)
				{
					tweenBase.Play();
				}
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0008D220 File Offset: 0x0008B420
		private void pauseAllTweens()
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				TweenBase tweenBase = this.tweenList[i];
				if (tweenBase != null)
				{
					tweenBase.Pause();
				}
			}
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0008D264 File Offset: 0x0008B464
		private void resumeAllTweens()
		{
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				TweenBase tweenBase = this.tweenList[i];
				if (tweenBase != null)
				{
					tweenBase.Resume();
				}
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0008D2A8 File Offset: 0x0008B4A8
		private bool nextTween()
		{
			if (this.Mode == TweenGroupMode.Concurrent)
			{
				return true;
			}
			if (base.State == TweenState.Started)
			{
				this.currentIndex = 0;
				this.currentTween = this.tweenList[this.currentIndex];
				this.currentTween.Play();
				return true;
			}
			if (this.currentIndex == this.tweenList.Count - 1)
			{
				if (this.LoopType != TweenLoopType.Loop || --this.LoopCount == 0)
				{
					return false;
				}
				if (this.TweenLoopCompleted != null)
				{
					this.TweenLoopCompleted(this);
				}
				this.currentIndex = 0;
				if (base.State == TweenState.Stopped)
				{
					return false;
				}
			}
			else
			{
				this.currentIndex++;
			}
			this.currentTween = this.tweenList[this.currentIndex];
			this.currentTween.Play();
			return true;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0008D3A0 File Offset: 0x0008B5A0
		private bool allTweensComplete()
		{
			if (this.Mode == TweenGroupMode.Sequential && this.currentTween != null)
			{
				return this.currentTween.State == TweenState.Stopped;
			}
			for (int i = 0; i < this.tweenList.Count; i++)
			{
				if (this.tweenList[i].State != TweenState.Stopped)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0008D408 File Offset: 0x0008B608
		private void cleanUp()
		{
			int i = 0;
			while (i < this.tweenList.Count)
			{
				TweenBase tweenBase = this.tweenList[i];
				if (tweenBase != null && tweenBase.AutoCleanup)
				{
					if (tweenBase is IPoolableObject)
					{
						((IPoolableObject)tweenBase).Release();
					}
					this.tweenList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0008D478 File Offset: 0x0008B678
		public IEnumerator<TweenBase> GetEnumerator()
		{
			return this.tweenList.GetEnumerator();
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0008D48C File Offset: 0x0008B68C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.tweenList.GetEnumerator();
		}

		// Token: 0x04001737 RID: 5943
		private static List<TweenGroup> pool = new List<TweenGroup>();

		// Token: 0x04001738 RID: 5944
		public TweenGroupMode Mode;

		// Token: 0x04001739 RID: 5945
		protected List<TweenBase> tweenList = new List<TweenBase>();

		// Token: 0x0400173A RID: 5946
		protected TweenBase currentTween;

		// Token: 0x0400173B RID: 5947
		protected int currentIndex;

		// Token: 0x0400173C RID: 5948
		protected bool autoCleanup;
	}
}
