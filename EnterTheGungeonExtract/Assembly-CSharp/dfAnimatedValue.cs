using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
public abstract class dfAnimatedValue<T> where T : struct
{
	// Token: 0x06001C57 RID: 7255 RVA: 0x000858D8 File Offset: 0x00083AD8
	protected internal dfAnimatedValue(T StartValue, T EndValue, float Time)
		: this()
	{
		this.startValue = StartValue;
		this.endValue = EndValue;
		this.animLength = Time;
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000858F8 File Offset: 0x00083AF8
	protected internal dfAnimatedValue()
	{
		this.startTime = Time.realtimeSinceStartup;
		this.easingFunction = dfEasingFunctions.GetFunction(this.easingType);
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06001C59 RID: 7257 RVA: 0x00085928 File Offset: 0x00083B28
	public bool IsDone
	{
		get
		{
			return this.isDone;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06001C5A RID: 7258 RVA: 0x00085930 File Offset: 0x00083B30
	// (set) Token: 0x06001C5B RID: 7259 RVA: 0x00085938 File Offset: 0x00083B38
	public float Length
	{
		get
		{
			return this.animLength;
		}
		set
		{
			this.animLength = value;
			this.startTime = Time.realtimeSinceStartup;
			this.isDone = false;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06001C5C RID: 7260 RVA: 0x00085954 File Offset: 0x00083B54
	// (set) Token: 0x06001C5D RID: 7261 RVA: 0x0008595C File Offset: 0x00083B5C
	public T StartValue
	{
		get
		{
			return this.startValue;
		}
		set
		{
			this.startValue = value;
			this.startTime = Time.realtimeSinceStartup;
			this.isDone = false;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06001C5E RID: 7262 RVA: 0x00085978 File Offset: 0x00083B78
	// (set) Token: 0x06001C5F RID: 7263 RVA: 0x00085980 File Offset: 0x00083B80
	public T EndValue
	{
		get
		{
			return this.endValue;
		}
		set
		{
			this.endValue = value;
			this.startTime = Time.realtimeSinceStartup;
			this.isDone = false;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06001C60 RID: 7264 RVA: 0x0008599C File Offset: 0x00083B9C
	public T Value
	{
		get
		{
			float num = Time.realtimeSinceStartup - this.startTime;
			if (num >= this.animLength)
			{
				this.isDone = true;
				return this.endValue;
			}
			float num2 = Mathf.Clamp01(num / this.animLength);
			num2 = this.easingFunction(0f, 1f, num2);
			return this.Lerp(this.startValue, this.endValue, num2);
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06001C61 RID: 7265 RVA: 0x00085A08 File Offset: 0x00083C08
	// (set) Token: 0x06001C62 RID: 7266 RVA: 0x00085A10 File Offset: 0x00083C10
	public dfEasingType EasingType
	{
		get
		{
			return this.easingType;
		}
		set
		{
			this.easingType = value;
			this.easingFunction = dfEasingFunctions.GetFunction(this.easingType);
		}
	}

	// Token: 0x06001C63 RID: 7267
	protected abstract T Lerp(T start, T end, float time);

	// Token: 0x06001C64 RID: 7268 RVA: 0x00085A2C File Offset: 0x00083C2C
	public static implicit operator T(dfAnimatedValue<T> animated)
	{
		return animated.Value;
	}

	// Token: 0x040015E2 RID: 5602
	private T startValue;

	// Token: 0x040015E3 RID: 5603
	private T endValue;

	// Token: 0x040015E4 RID: 5604
	private float animLength = 1f;

	// Token: 0x040015E5 RID: 5605
	private float startTime;

	// Token: 0x040015E6 RID: 5606
	private bool isDone;

	// Token: 0x040015E7 RID: 5607
	private dfEasingType easingType;

	// Token: 0x040015E8 RID: 5608
	private dfEasingFunctions.EasingFunction easingFunction;
}
