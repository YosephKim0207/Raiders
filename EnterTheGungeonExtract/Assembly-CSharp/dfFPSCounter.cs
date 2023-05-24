using System;
using UnityEngine;

// Token: 0x02000467 RID: 1127
[AddComponentMenu("Daikon Forge/Examples/General/FPS Counter")]
public class dfFPSCounter : MonoBehaviour
{
	// Token: 0x06001A12 RID: 6674 RVA: 0x00079B34 File Offset: 0x00077D34
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

	// Token: 0x06001A13 RID: 6675 RVA: 0x00079B84 File Offset: 0x00077D84
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
			string text = string.Format("{0:F0} FPS", num);
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

	// Token: 0x04001471 RID: 5233
	public float updateInterval = 0.5f;

	// Token: 0x04001472 RID: 5234
	private float accum;

	// Token: 0x04001473 RID: 5235
	private int frames;

	// Token: 0x04001474 RID: 5236
	private float timeleft;

	// Token: 0x04001475 RID: 5237
	private dfLabel label;
}
