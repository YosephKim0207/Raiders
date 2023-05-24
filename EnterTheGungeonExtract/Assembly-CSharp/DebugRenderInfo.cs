using System;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02000462 RID: 1122
[AddComponentMenu("Daikon Forge/Examples/General/Debug Render Info")]
public class DebugRenderInfo : MonoBehaviour
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x000790E4 File Offset: 0x000772E4
	private void Start()
	{
		this.info = base.GetComponent<dfLabel>();
		if (this.info == null)
		{
			base.enabled = false;
			throw new Exception("No Label component found");
		}
		this.info.Text = string.Empty;
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x00079130 File Offset: 0x00077330
	private void Update()
	{
		if (this.view == null)
		{
			this.view = this.info.GetManager();
		}
		this.frameCount++;
		float num = Time.realtimeSinceStartup - this.lastUpdate;
		if (num < this.interval)
		{
			return;
		}
		this.lastUpdate = Time.realtimeSinceStartup;
		float num2 = 1f / (num / (float)this.frameCount);
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		string text = string.Format("{0}x{1}", (int)vector.x, (int)vector.y);
		string text2 = "Screen : {0}, DrawCalls: {1}, Triangles: {2}, Mem: {3:F0}MB, FPS: {4:F0}";
		float num3 = ((!Profiler.supported) ? ((float)GC.GetTotalMemory(false) / 1048576f) : (Profiler.GetMonoUsedSize() / 1048576f));
		string text3 = string.Format(text2, new object[]
		{
			text,
			this.view.TotalDrawCalls,
			this.view.TotalTriangles,
			num3,
			num2
		});
		this.info.Text = text3.Trim();
		this.frameCount = 0;
	}

	// Token: 0x04001456 RID: 5206
	public float interval = 0.5f;

	// Token: 0x04001457 RID: 5207
	private dfLabel info;

	// Token: 0x04001458 RID: 5208
	private dfGUIManager view;

	// Token: 0x04001459 RID: 5209
	private float lastUpdate;

	// Token: 0x0400145A RID: 5210
	private int frameCount;
}
