using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class dfMobileTouchInputSource : IDFTouchInputSource
{
	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x0600185A RID: 6234 RVA: 0x00073670 File Offset: 0x00071870
	public static dfMobileTouchInputSource Instance
	{
		get
		{
			if (dfMobileTouchInputSource.instance == null)
			{
				dfMobileTouchInputSource.instance = new dfMobileTouchInputSource();
			}
			return dfMobileTouchInputSource.instance;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x0600185B RID: 6235 RVA: 0x0007368C File Offset: 0x0007188C
	public int TouchCount
	{
		get
		{
			return Input.touchCount;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x0600185C RID: 6236 RVA: 0x00073694 File Offset: 0x00071894
	public IList<dfTouchInfo> Touches
	{
		get
		{
			return this.activeTouches;
		}
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x0007369C File Offset: 0x0007189C
	public dfTouchInfo GetTouch(int index)
	{
		return Input.GetTouch(index);
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x000736AC File Offset: 0x000718AC
	public void Update()
	{
		this.activeTouches.Clear();
		for (int i = 0; i < this.TouchCount; i++)
		{
			this.activeTouches.Add(this.GetTouch(i));
		}
	}

	// Token: 0x0400135D RID: 4957
	private static dfMobileTouchInputSource instance;

	// Token: 0x0400135E RID: 4958
	private List<dfTouchInfo> activeTouches = new List<dfTouchInfo>();
}
