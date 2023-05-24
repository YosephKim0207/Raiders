using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200051D RID: 1309
	public class TweenManager : MonoBehaviour
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0008D4F0 File Offset: 0x0008B6F0
		public static TweenManager Instance
		{
			get
			{
				object typeFromHandle = typeof(TweenManager);
				TweenManager tweenManager;
				lock (typeFromHandle)
				{
					if (TweenManager.instance == null)
					{
						TweenManager.instance = new GameObject("_TweenManager_")
						{
							hideFlags = HideFlags.HideInHierarchy
						}.AddComponent<TweenManager>();
					}
					tweenManager = TweenManager.instance;
				}
				return tweenManager;
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0008D560 File Offset: 0x0008B760
		public void RegisterTween(ITweenUpdatable tween)
		{
			object obj = this.playingTweens;
			lock (obj)
			{
				if (!this.playingTweens.Contains(tween) || this.removeTweenQueue.Contains(tween))
				{
					object obj2 = this.addTweenQueue;
					lock (obj2)
					{
						this.addTweenQueue.Enqueue(tween);
					}
				}
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0008D5F0 File Offset: 0x0008B7F0
		public void UnregisterTween(ITweenUpdatable tween)
		{
			object obj = this.removeTweenQueue;
			lock (obj)
			{
				if (this.playingTweens.Contains(tween) && !this.removeTweenQueue.Contains(tween))
				{
					this.removeTweenQueue.Enqueue(tween);
				}
			}
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0008D65C File Offset: 0x0008B85C
		public void Clear()
		{
			object obj = this.playingTweens;
			lock (obj)
			{
				this.playingTweens.Clear();
				this.removeTweenQueue.Clear();
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0008D6A8 File Offset: 0x0008B8A8
		public virtual void OnDestroy()
		{
			TweenManager.instance = null;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0008D6B0 File Offset: 0x0008B8B0
		public virtual void Update()
		{
			this.realTimeSinceStartup = Time.realtimeSinceStartup;
			TweenManager.realDeltaTime = this.realTimeSinceStartup - TweenManager.lastFrameTime;
			TweenManager.lastFrameTime = this.realTimeSinceStartup;
			object obj = this.playingTweens;
			lock (obj)
			{
				object obj2 = this.addTweenQueue;
				lock (obj2)
				{
					while (this.addTweenQueue.Count > 0)
					{
						this.playingTweens.Add(this.addTweenQueue.Dequeue());
					}
				}
				object obj3 = this.removeTweenQueue;
				lock (obj3)
				{
					while (this.removeTweenQueue.Count > 0)
					{
						this.playingTweens.Remove(this.removeTweenQueue.Dequeue());
					}
				}
				int count = this.playingTweens.Count;
				for (int i = 0; i < count; i++)
				{
					ITweenUpdatable tweenUpdatable = this.playingTweens[i];
					TweenState state = tweenUpdatable.State;
					if (state == TweenState.Playing || state == TweenState.Started)
					{
						tweenUpdatable.Update();
					}
				}
			}
		}

		// Token: 0x0400173D RID: 5949
		private static TweenManager instance;

		// Token: 0x0400173E RID: 5950
		internal static float realDeltaTime = 0f;

		// Token: 0x0400173F RID: 5951
		private static float lastFrameTime = 0f;

		// Token: 0x04001740 RID: 5952
		internal float realTimeSinceStartup;

		// Token: 0x04001741 RID: 5953
		private List<ITweenUpdatable> playingTweens = new List<ITweenUpdatable>();

		// Token: 0x04001742 RID: 5954
		private Queue<ITweenUpdatable> addTweenQueue = new Queue<ITweenUpdatable>();

		// Token: 0x04001743 RID: 5955
		private Queue<ITweenUpdatable> removeTweenQueue = new Queue<ITweenUpdatable>();
	}
}
