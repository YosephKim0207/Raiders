using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
[AddComponentMenu("Daikon Forge/Examples/General/FPS Counter")]
[RequireComponent(typeof(dfLabel))]
public class FPSCounter : MonoBehaviour
{
	// Token: 0x06001A2C RID: 6700 RVA: 0x0007A2B4 File Offset: 0x000784B4
	private void Start()
	{
		this.label = base.GetComponent<dfLabel>();
		if (this.label == null)
		{
			Debug.LogError("FPS Counter needs a Label component!");
		}
		this.timeleft = this.updateInterval;
		this.label.Text = string.Empty;
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x0007A304 File Offset: 0x00078504
	private void Update()
	{
		if (this.label == null)
		{
			return;
		}
		this.timeleft -= BraveTime.DeltaTime;
		this.accum += Time.timeScale / BraveTime.DeltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS", num);
			this.label.Text = text;
			if (num < 30f)
			{
				this.label.Color = Color.yellow;
			}
			else if (num < 10f)
			{
				this.label.Color = Color.red;
			}
			else
			{
				this.label.Color = Color.green;
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x04001485 RID: 5253
	public float updateInterval = 0.1f;

	// Token: 0x04001486 RID: 5254
	private float accum;

	// Token: 0x04001487 RID: 5255
	private int frames;

	// Token: 0x04001488 RID: 5256
	private float timeleft;

	// Token: 0x04001489 RID: 5257
	private dfLabel label;
}
