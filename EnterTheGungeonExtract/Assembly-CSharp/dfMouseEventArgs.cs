using System;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class dfMouseEventArgs : dfControlEventArgs
{
	// Token: 0x06001214 RID: 4628 RVA: 0x00053200 File Offset: 0x00051400
	public dfMouseEventArgs(dfControl Source, dfMouseButtons button, int clicks, Ray ray, Vector2 location, float wheel)
		: base(Source)
	{
		this.Buttons = button;
		this.Clicks = clicks;
		this.Position = location;
		this.WheelDelta = wheel;
		this.Ray = ray;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00053230 File Offset: 0x00051430
	public dfMouseEventArgs(dfControl Source)
		: base(Source)
	{
		this.Buttons = dfMouseButtons.None;
		this.Clicks = 0;
		this.Position = Vector2.zero;
		this.WheelDelta = 0f;
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06001216 RID: 4630 RVA: 0x00053260 File Offset: 0x00051460
	// (set) Token: 0x06001217 RID: 4631 RVA: 0x00053268 File Offset: 0x00051468
	public dfMouseButtons Buttons { get; private set; }

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06001218 RID: 4632 RVA: 0x00053274 File Offset: 0x00051474
	// (set) Token: 0x06001219 RID: 4633 RVA: 0x0005327C File Offset: 0x0005147C
	public int Clicks { get; private set; }

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600121A RID: 4634 RVA: 0x00053288 File Offset: 0x00051488
	// (set) Token: 0x0600121B RID: 4635 RVA: 0x00053290 File Offset: 0x00051490
	public float WheelDelta { get; private set; }

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600121C RID: 4636 RVA: 0x0005329C File Offset: 0x0005149C
	// (set) Token: 0x0600121D RID: 4637 RVA: 0x000532A4 File Offset: 0x000514A4
	public Vector2 MoveDelta { get; set; }

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600121E RID: 4638 RVA: 0x000532B0 File Offset: 0x000514B0
	// (set) Token: 0x0600121F RID: 4639 RVA: 0x000532B8 File Offset: 0x000514B8
	public Vector2 Position { get; set; }

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06001220 RID: 4640 RVA: 0x000532C4 File Offset: 0x000514C4
	// (set) Token: 0x06001221 RID: 4641 RVA: 0x000532CC File Offset: 0x000514CC
	public Ray Ray { get; set; }
}
