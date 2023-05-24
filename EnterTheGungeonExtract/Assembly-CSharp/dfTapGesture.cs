using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000423 RID: 1059
[AddComponentMenu("Daikon Forge/Input/Gestures/Tap")]
public class dfTapGesture : dfGestureBase
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06001848 RID: 6216 RVA: 0x000733D4 File Offset: 0x000715D4
	// (remove) Token: 0x06001849 RID: 6217 RVA: 0x0007340C File Offset: 0x0007160C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfTapGesture> TapGesture;

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x0600184A RID: 6218 RVA: 0x00073444 File Offset: 0x00071644
	// (set) Token: 0x0600184B RID: 6219 RVA: 0x0007344C File Offset: 0x0007164C
	public float Timeout
	{
		get
		{
			return this.timeout;
		}
		set
		{
			this.timeout = value;
		}
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x0600184C RID: 6220 RVA: 0x00073458 File Offset: 0x00071658
	// (set) Token: 0x0600184D RID: 6221 RVA: 0x00073460 File Offset: 0x00071660
	public float MaximumDistance
	{
		get
		{
			return this.maxDistance;
		}
		set
		{
			this.maxDistance = value;
		}
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0007346C File Offset: 0x0007166C
	protected void Start()
	{
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x00073470 File Offset: 0x00071670
	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 position = args.Position;
		base.CurrentPosition = position;
		base.StartPosition = position;
		base.State = dfGestureState.Possible;
		base.StartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000734A4 File Offset: 0x000716A4
	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Possible || base.State == dfGestureState.Began)
		{
			base.CurrentPosition = args.Position;
			if (Vector2.Distance(args.Position, base.StartPosition) > this.maxDistance)
			{
				base.State = dfGestureState.Failed;
			}
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000734F8 File Offset: 0x000716F8
	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Possible)
		{
			if (Time.realtimeSinceStartup - base.StartTime <= this.timeout)
			{
				base.CurrentPosition = args.Position;
				base.State = dfGestureState.Ended;
				if (this.TapGesture != null)
				{
					this.TapGesture(this);
				}
				base.gameObject.Signal("OnTapGesture", new object[] { this });
			}
			else
			{
				base.State = dfGestureState.Failed;
			}
		}
		else
		{
			base.State = dfGestureState.None;
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x00073588 File Offset: 0x00071788
	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		base.State = dfGestureState.Failed;
	}

	// Token: 0x04001358 RID: 4952
	[SerializeField]
	private float timeout = 0.25f;

	// Token: 0x04001359 RID: 4953
	[SerializeField]
	private float maxDistance = 25f;
}
