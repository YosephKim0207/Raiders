using System;
using System.Text;
using UnityEngine;

// Token: 0x0200179B RID: 6043
public class HUDGC : MonoBehaviour
{
	// Token: 0x06008D75 RID: 36213 RVA: 0x003B8384 File Offset: 0x003B6584
	public void Start()
	{
		this.m_label = base.GetComponent<dfLabel>();
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x003B8394 File Offset: 0x003B6594
	public void Update()
	{
		if (HUDGC.ShowGcData)
		{
			if (!this.m_label.IsVisible)
			{
				this.m_label.IsVisible = true;
			}
			if (!this.m_showGcBarLastFrame)
			{
				this.m_cachedMemMin = ProfileUtils.GetMonoUsedHeapSize();
				this.m_cachedMemMax = ProfileUtils.GetMonoHeapSize();
				this.m_lastFrameMemSize = this.m_cachedMemMin;
			}
			uint monoUsedHeapSize = ProfileUtils.GetMonoUsedHeapSize();
			float num = Time.realtimeSinceStartup - this.m_lastFrameTime;
			if (monoUsedHeapSize < this.m_lastFrameMemSize)
			{
				this.m_cachedMemMin = monoUsedHeapSize;
				if (!HUDGC.SkipNextGC)
				{
					this.m_cachedMemMax = this.m_lastFrameMemSize;
				}
				float num2 = Time.realtimeSinceStartup - this.m_lastGcTime;
				if (!HUDGC.SkipNextGC)
				{
					this.m_lastGcDuration = num;
					this.m_avgDuration = BraveMathCollege.MovingAverage(this.m_avgDuration, this.m_lastGcDuration, 5);
					if (this.m_lastGcTime > 0f)
					{
						this.m_avgFrequency = BraveMathCollege.MovingAverage(this.m_avgFrequency, num2, 5);
					}
				}
				this.m_lastGcTime = Time.realtimeSinceStartup;
				this.m_avgMemDelta = 0f;
			}
			if (num > 0f && monoUsedHeapSize > this.m_lastFrameMemSize)
			{
				float num3 = (monoUsedHeapSize - this.m_lastFrameMemSize) / num;
				this.m_avgMemDelta = BraveMathCollege.MovingAverage(this.m_avgMemDelta, num3, 90);
			}
			float num4 = Mathf.InverseLerp(this.m_cachedMemMin, this.m_cachedMemMax, monoUsedHeapSize);
			this.stringBuilder.Length = 0;
			this.stringBuilder.AppendFormat("Memory Use: {0:00.000}%\n", num4 * 100f);
			this.stringBuilder.AppendFormat(" - {0:0.00} MB / {1:0.00} MB\n", monoUsedHeapSize * HUDGC.B2MB, this.m_cachedMemMax * HUDGC.B2MB);
			this.stringBuilder.AppendFormat(" - {0:+0.00} MB/sec\n", this.m_avgMemDelta * HUDGC.B2MB);
			this.stringBuilder.AppendFormat(" - {0:+0.00} kB/frame\n", this.m_avgMemDelta / 60f * HUDGC.B2kB);
			this.stringBuilder.AppendFormat("Last GC Duration: {0:00.00} ms\n", this.m_lastGcDuration * 1000f);
			this.stringBuilder.AppendFormat("Time Since Last GC: {0: 00.0} sec\n", Time.realtimeSinceStartup - this.m_lastGcTime);
			this.stringBuilder.AppendFormat("Avg Duration: {0:00.00} ms\n", this.m_avgDuration * 1000f);
			this.stringBuilder.AppendFormat("Avg Time Between: {0:00.0} sec\n", this.m_avgFrequency);
			this.stringBuilder.AppendFormat("Total Collections: {0}\n", ProfileUtils.GetMonoCollectionCount());
			this.m_label.Text = this.stringBuilder.ToString();
			this.m_lastFrameMemSize = monoUsedHeapSize;
			this.m_lastFrameTime = Time.realtimeSinceStartup;
			this.m_showGcBarLastFrame = true;
		}
		else
		{
			if (this.m_label.IsVisible)
			{
				this.m_label.IsVisible = false;
			}
			this.m_showGcBarLastFrame = false;
		}
		HUDGC.SkipNextGC = false;
	}

	// Token: 0x04009528 RID: 38184
	public static bool ShowGcData;

	// Token: 0x04009529 RID: 38185
	public static bool SkipNextGC;

	// Token: 0x0400952A RID: 38186
	private static float B2MB = 9.536743E-07f;

	// Token: 0x0400952B RID: 38187
	private static float B2kB = 0.0009765625f;

	// Token: 0x0400952C RID: 38188
	private dfLabel m_label;

	// Token: 0x0400952D RID: 38189
	private uint m_cachedMemMin;

	// Token: 0x0400952E RID: 38190
	private uint m_cachedMemMax;

	// Token: 0x0400952F RID: 38191
	private uint m_lastFrameMemSize;

	// Token: 0x04009530 RID: 38192
	private bool m_showGcBarLastFrame;

	// Token: 0x04009531 RID: 38193
	private float m_avgDuration;

	// Token: 0x04009532 RID: 38194
	private float m_avgFrequency;

	// Token: 0x04009533 RID: 38195
	private float m_avgMemDelta;

	// Token: 0x04009534 RID: 38196
	private float m_lastGcTime;

	// Token: 0x04009535 RID: 38197
	private float m_lastGcDuration;

	// Token: 0x04009536 RID: 38198
	private float m_lastFrameTime;

	// Token: 0x04009537 RID: 38199
	private StringBuilder stringBuilder = new StringBuilder();
}
