using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public abstract class dfGestureBase : MonoBehaviour
{
	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x060017FB RID: 6139 RVA: 0x000720B4 File Offset: 0x000702B4
	// (set) Token: 0x060017FC RID: 6140 RVA: 0x000720BC File Offset: 0x000702BC
	public dfGestureState State { get; protected set; }

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x060017FD RID: 6141 RVA: 0x000720C8 File Offset: 0x000702C8
	// (set) Token: 0x060017FE RID: 6142 RVA: 0x000720D0 File Offset: 0x000702D0
	public Vector2 StartPosition { get; protected set; }

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x060017FF RID: 6143 RVA: 0x000720DC File Offset: 0x000702DC
	// (set) Token: 0x06001800 RID: 6144 RVA: 0x000720E4 File Offset: 0x000702E4
	public Vector2 CurrentPosition { get; protected set; }

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06001801 RID: 6145 RVA: 0x000720F0 File Offset: 0x000702F0
	// (set) Token: 0x06001802 RID: 6146 RVA: 0x000720F8 File Offset: 0x000702F8
	public float StartTime { get; protected set; }

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06001803 RID: 6147 RVA: 0x00072104 File Offset: 0x00070304
	public dfControl Control
	{
		get
		{
			if (this.control == null)
			{
				this.control = base.GetComponent<dfControl>();
			}
			return this.control;
		}
	}

	// Token: 0x04001335 RID: 4917
	private dfControl control;
}
