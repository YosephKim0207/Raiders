using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class dfTouchEventArgs : dfMouseEventArgs
{
	// Token: 0x06001222 RID: 4642 RVA: 0x000532D8 File Offset: 0x000514D8
	public dfTouchEventArgs(dfControl Source, dfTouchInfo touch, Ray ray)
		: base(Source, dfMouseButtons.Left, touch.tapCount, ray, touch.position, 0f)
	{
		this.Touch = touch;
		this.Touches = new List<dfTouchInfo> { touch };
		float deltaTime = BraveTime.DeltaTime;
		if (touch.deltaTime > 1E-45f && deltaTime > 1E-45f)
		{
			base.MoveDelta = touch.deltaPosition * (deltaTime / touch.deltaTime);
		}
		else
		{
			base.MoveDelta = touch.deltaPosition;
		}
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x0005336C File Offset: 0x0005156C
	public dfTouchEventArgs(dfControl source, List<dfTouchInfo> touches, Ray ray)
		: this(source, touches.First<dfTouchInfo>(), ray)
	{
		this.Touches = touches;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00053384 File Offset: 0x00051584
	public dfTouchEventArgs(dfControl Source)
		: base(Source)
	{
		base.Position = Vector2.zero;
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06001225 RID: 4645 RVA: 0x00053398 File Offset: 0x00051598
	// (set) Token: 0x06001226 RID: 4646 RVA: 0x000533A0 File Offset: 0x000515A0
	public dfTouchInfo Touch { get; private set; }

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06001227 RID: 4647 RVA: 0x000533AC File Offset: 0x000515AC
	// (set) Token: 0x06001228 RID: 4648 RVA: 0x000533B4 File Offset: 0x000515B4
	public List<dfTouchInfo> Touches { get; private set; }

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06001229 RID: 4649 RVA: 0x000533C0 File Offset: 0x000515C0
	public bool IsMultiTouch
	{
		get
		{
			return this.Touches.Count > 1;
		}
	}
}
