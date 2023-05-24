using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
internal class dfTouchTrackingInfo
{
	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00071B40 File Offset: 0x0006FD40
	// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00071B48 File Offset: 0x0006FD48
	public int FingerID { get; set; }

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00071B54 File Offset: 0x0006FD54
	// (set) Token: 0x060017D4 RID: 6100 RVA: 0x00071B5C File Offset: 0x0006FD5C
	public TouchPhase Phase
	{
		get
		{
			return this.phase;
		}
		set
		{
			this.IsActive = true;
			this.phase = value;
			if (value == TouchPhase.Stationary)
			{
				this.deltaTime = float.Epsilon;
				this.deltaPosition = Vector2.zero;
				this.lastUpdateTime = Time.realtimeSinceStartup;
			}
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00071B94 File Offset: 0x0006FD94
	// (set) Token: 0x060017D6 RID: 6102 RVA: 0x00071B9C File Offset: 0x0006FD9C
	public Vector2 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.IsActive = true;
			if (this.Phase == TouchPhase.Began)
			{
				this.deltaPosition = Vector2.zero;
			}
			else
			{
				this.deltaPosition = value - this.position;
			}
			this.position = value;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			this.deltaTime = realtimeSinceStartup - this.lastUpdateTime;
			this.lastUpdateTime = realtimeSinceStartup;
		}
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00071C00 File Offset: 0x0006FE00
	public static implicit operator dfTouchInfo(dfTouchTrackingInfo info)
	{
		dfTouchInfo dfTouchInfo = new dfTouchInfo(info.FingerID, info.phase, (info.phase != TouchPhase.Began) ? 0 : 1, info.position, info.deltaPosition, info.deltaTime);
		return dfTouchInfo;
	}

	// Token: 0x04001326 RID: 4902
	private TouchPhase phase;

	// Token: 0x04001327 RID: 4903
	private Vector2 position = Vector2.one * float.MinValue;

	// Token: 0x04001328 RID: 4904
	private Vector2 deltaPosition = Vector2.zero;

	// Token: 0x04001329 RID: 4905
	private float deltaTime;

	// Token: 0x0400132A RID: 4906
	private float lastUpdateTime = Time.realtimeSinceStartup;

	// Token: 0x0400132B RID: 4907
	public bool IsActive;
}
