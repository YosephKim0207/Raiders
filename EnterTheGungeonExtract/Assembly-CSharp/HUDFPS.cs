using System;
using UnityEngine;

// Token: 0x0200179A RID: 6042
public class HUDFPS : MonoBehaviour
{
	// Token: 0x06008D72 RID: 36210 RVA: 0x003B820C File Offset: 0x003B640C
	private void Start()
	{
		this.updateInterval = 0.5f;
		this.m_label = base.GetComponent<dfLabel>();
		if (!this.m_label)
		{
			Debug.Log("FramesPerSecond needs a dfLabel component!");
			base.enabled = false;
			return;
		}
		this.timeleft = this.updateInterval;
	}

	// Token: 0x06008D73 RID: 36211 RVA: 0x003B8260 File Offset: 0x003B6460
	private void Update()
	{
		if (this.m_label.IsVisible)
		{
			this.timeleft -= GameManager.INVARIANT_DELTA_TIME;
			this.accum += GameManager.INVARIANT_DELTA_TIME;
			this.frames++;
			if ((double)this.timeleft <= 0.0)
			{
				float num = (float)this.frames / this.accum;
				string text = string.Format("{0:F2} FPS", num);
				this.m_label.Text = text;
				if (num < 30f)
				{
					this.m_label.Color = Color.yellow;
				}
				else if (num < 10f)
				{
					this.m_label.Color = Color.red;
				}
				else
				{
					this.m_label.Color = Color.green;
				}
				this.timeleft = this.updateInterval;
				this.accum = 0f;
				this.frames = 0;
			}
		}
	}

	// Token: 0x04009523 RID: 38179
	[NonSerialized]
	public float updateInterval = 10f;

	// Token: 0x04009524 RID: 38180
	private dfLabel m_label;

	// Token: 0x04009525 RID: 38181
	private float accum;

	// Token: 0x04009526 RID: 38182
	private int frames;

	// Token: 0x04009527 RID: 38183
	private float timeleft;
}
