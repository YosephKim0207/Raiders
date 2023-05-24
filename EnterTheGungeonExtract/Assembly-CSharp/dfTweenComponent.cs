using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
[Serializable]
public abstract class dfTweenComponent<T> : dfTweenComponentBase where T : struct
{
	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06001C8F RID: 7311 RVA: 0x0008663C File Offset: 0x0008483C
	// (remove) Token: 0x06001C90 RID: 7312 RVA: 0x00086674 File Offset: 0x00084874
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenStarted;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06001C91 RID: 7313 RVA: 0x000866AC File Offset: 0x000848AC
	// (remove) Token: 0x06001C92 RID: 7314 RVA: 0x000866E4 File Offset: 0x000848E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenStopped;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06001C93 RID: 7315 RVA: 0x0008671C File Offset: 0x0008491C
	// (remove) Token: 0x06001C94 RID: 7316 RVA: 0x00086754 File Offset: 0x00084954
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenPaused;

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06001C95 RID: 7317 RVA: 0x0008678C File Offset: 0x0008498C
	// (remove) Token: 0x06001C96 RID: 7318 RVA: 0x000867C4 File Offset: 0x000849C4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenResumed;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06001C97 RID: 7319 RVA: 0x000867FC File Offset: 0x000849FC
	// (remove) Token: 0x06001C98 RID: 7320 RVA: 0x00086834 File Offset: 0x00084A34
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenReset;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06001C99 RID: 7321 RVA: 0x0008686C File Offset: 0x00084A6C
	// (remove) Token: 0x06001C9A RID: 7322 RVA: 0x000868A4 File Offset: 0x00084AA4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification TweenCompleted;

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000868DC File Offset: 0x00084ADC
	// (set) Token: 0x06001C9C RID: 7324 RVA: 0x000868E4 File Offset: 0x00084AE4
	public T StartValue
	{
		get
		{
			return this.startValue;
		}
		set
		{
			this.startValue = value;
			if (this.state != dfTweenState.Stopped)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x06001C9D RID: 7325 RVA: 0x00086904 File Offset: 0x00084B04
	// (set) Token: 0x06001C9E RID: 7326 RVA: 0x0008690C File Offset: 0x00084B0C
	public T EndValue
	{
		get
		{
			return this.endValue;
		}
		set
		{
			this.endValue = value;
			if (this.state != dfTweenState.Stopped)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06001C9F RID: 7327 RVA: 0x0008692C File Offset: 0x00084B2C
	public dfTweenState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x00086934 File Offset: 0x00084B34
	public static dfTweenComponent<T> Create(Component target, string propertyName, T startValue, T endValue, float length)
	{
		return dfTweenComponent<T>.Create(target, propertyName, startValue, endValue, length, dfEasingType.Linear);
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x00086944 File Offset: 0x00084B44
	public static dfTweenComponent<T> Create(Component target, string propertyName, T startValue, T endValue, float length, dfEasingType func)
	{
		if (target == null || target.gameObject == null)
		{
			throw new ArgumentNullException("target");
		}
		if (string.IsNullOrEmpty(propertyName))
		{
			throw new ArgumentNullException("propertyName");
		}
		dfTweenComponent<T> dfTweenComponent = (dfTweenComponent<T>)target.gameObject.AddComponent(typeof(T));
		dfTweenComponent.autoRun = false;
		dfTweenComponent.target = new dfComponentMemberInfo
		{
			Component = target,
			MemberName = propertyName
		};
		dfTweenComponent.startValue = startValue;
		dfTweenComponent.endValue = endValue;
		dfTweenComponent.length = length;
		dfTweenComponent.easingType = func;
		return dfTweenComponent;
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x000869EC File Offset: 0x00084BEC
	public override void Play()
	{
		if (this.state != dfTweenState.Stopped)
		{
			this.Stop();
		}
		if (!base.enabled || !base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.target == null)
		{
			throw new NullReferenceException("Tween target is NULL");
		}
		if (!this.target.IsValid)
		{
			throw new InvalidOperationException(string.Concat(new object[]
			{
				"Invalid property binding configuration on ",
				this.getPath(base.gameObject.transform),
				" - ",
				this.target
			}));
		}
		this.boundProperty = this.target.GetProperty();
		this.easingFunction = dfEasingFunctions.GetFunction(this.easingType);
		this.onStarted();
		this.actualStartValue = this.startValue;
		this.actualEndValue = this.endValue;
		if (this.syncStartWhenRun)
		{
			this.actualStartValue = (T)((object)this.boundProperty.Value);
		}
		else if (this.startValueIsOffset)
		{
			this.actualStartValue = this.offset(this.startValue, (T)((object)this.boundProperty.Value));
		}
		if (this.syncEndWhenRun)
		{
			this.actualEndValue = (T)((object)this.boundProperty.Value);
		}
		else if (this.endValueIsOffset)
		{
			this.actualEndValue = this.offset(this.endValue, (T)((object)this.boundProperty.Value));
		}
		this.boundProperty.Value = this.actualStartValue;
		this.startTime = Time.realtimeSinceStartup;
		this.state = dfTweenState.Started;
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x00086BA8 File Offset: 0x00084DA8
	public override void Stop()
	{
		if (this.state == dfTweenState.Stopped)
		{
			return;
		}
		if (this.skipToEndOnStop)
		{
			this.boundProperty.Value = this.actualEndValue;
		}
		this.state = dfTweenState.Stopped;
		this.onStopped();
		this.easingFunction = null;
		this.boundProperty = null;
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x00086C00 File Offset: 0x00084E00
	public override void Reset()
	{
		if (this.boundProperty != null)
		{
			this.boundProperty.Value = this.actualStartValue;
		}
		this.state = dfTweenState.Stopped;
		this.onReset();
		this.easingFunction = null;
		this.boundProperty = null;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x00086C40 File Offset: 0x00084E40
	public void Pause()
	{
		base.IsPaused = true;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x00086C4C File Offset: 0x00084E4C
	public void Resume()
	{
		base.IsPaused = false;
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x00086C58 File Offset: 0x00084E58
	public void Update()
	{
		if (this.state == dfTweenState.Stopped || this.state == dfTweenState.Paused)
		{
			return;
		}
		if (this.state == dfTweenState.Started)
		{
			if (this.startTime + base.StartDelay >= Time.realtimeSinceStartup)
			{
				return;
			}
			this.state = dfTweenState.Playing;
			this.startTime = Time.realtimeSinceStartup;
			this.pingPongDirection = 0f;
		}
		float num = Mathf.Min(Time.realtimeSinceStartup - this.startTime, this.length);
		if (num >= this.length)
		{
			if (this.loopType == dfTweenLoopType.Once)
			{
				this.boundProperty.Value = this.actualEndValue;
				this.Stop();
				this.onCompleted();
			}
			else if (this.loopType == dfTweenLoopType.Loop)
			{
				this.startTime = Time.realtimeSinceStartup;
			}
			else
			{
				if (this.loopType != dfTweenLoopType.PingPong)
				{
					throw new NotImplementedException();
				}
				this.startTime = Time.realtimeSinceStartup;
				if (this.pingPongDirection == 0f)
				{
					this.pingPongDirection = 1f;
				}
				else
				{
					this.pingPongDirection = 0f;
				}
			}
			return;
		}
		float num2 = this.easingFunction(0f, 1f, Mathf.Abs(this.pingPongDirection - num / this.length));
		if (this.animCurve != null)
		{
			num2 = this.animCurve.Evaluate(num2);
		}
		this.boundProperty.Value = this.evaluate(this.actualStartValue, this.actualEndValue, num2);
	}

	// Token: 0x06001CA8 RID: 7336
	public abstract T evaluate(T startValue, T endValue, float time);

	// Token: 0x06001CA9 RID: 7337
	public abstract T offset(T value, T offset);

	// Token: 0x06001CAA RID: 7338 RVA: 0x00086DE4 File Offset: 0x00084FE4
	public override string ToString()
	{
		if (base.Target != null && base.Target.IsValid)
		{
			string name = this.target.Component.name;
			return string.Format("{0} ({1}.{2})", this.TweenName, name, this.target.MemberName);
		}
		return this.TweenName;
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x00086E40 File Offset: 0x00085040
	private string getPath(Transform obj)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (obj != null)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Insert(0, "\\");
				stringBuilder.Insert(0, obj.name);
			}
			else
			{
				stringBuilder.Append(obj.name);
			}
			obj = obj.parent;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00086EAC File Offset: 0x000850AC
	protected internal static float Lerp(float startValue, float endValue, float time)
	{
		return startValue + (endValue - startValue) * time;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x00086EB8 File Offset: 0x000850B8
	protected internal override void onPaused()
	{
		base.SendMessage("TweenPaused", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenPaused != null)
		{
			this.TweenPaused(this);
		}
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x00086EE0 File Offset: 0x000850E0
	protected internal override void onResumed()
	{
		base.SendMessage("TweenResumed", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenResumed != null)
		{
			this.TweenResumed(this);
		}
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x00086F08 File Offset: 0x00085108
	protected internal override void onStarted()
	{
		base.SendMessage("TweenStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStarted != null)
		{
			this.TweenStarted(this);
		}
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00086F30 File Offset: 0x00085130
	protected internal override void onStopped()
	{
		base.SendMessage("TweenStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStopped != null)
		{
			this.TweenStopped(this);
		}
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x00086F58 File Offset: 0x00085158
	protected internal override void onReset()
	{
		base.SendMessage("TweenReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenReset != null)
		{
			this.TweenReset(this);
		}
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00086F80 File Offset: 0x00085180
	protected internal override void onCompleted()
	{
		base.SendMessage("TweenCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenCompleted != null)
		{
			this.TweenCompleted(this);
		}
	}

	// Token: 0x04001626 RID: 5670
	[SerializeField]
	protected T startValue;

	// Token: 0x04001627 RID: 5671
	[SerializeField]
	protected T endValue;

	// Token: 0x04001628 RID: 5672
	[SerializeField]
	protected dfPlayDirection direction;

	// Token: 0x04001629 RID: 5673
	private T actualStartValue;

	// Token: 0x0400162A RID: 5674
	private T actualEndValue;

	// Token: 0x0400162B RID: 5675
	private float startTime;

	// Token: 0x0400162C RID: 5676
	private float pingPongDirection;
}
