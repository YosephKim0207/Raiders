using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200041A RID: 1050
[AddComponentMenu("Daikon Forge/Input/Gestures/Double Tap")]
public class dfDoubleTapGesture : dfGestureBase
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x060017D9 RID: 6105 RVA: 0x00071C68 File Offset: 0x0006FE68
	// (remove) Token: 0x060017DA RID: 6106 RVA: 0x00071CA0 File Offset: 0x0006FEA0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfDoubleTapGesture> DoubleTapGesture;

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x060017DB RID: 6107 RVA: 0x00071CD8 File Offset: 0x0006FED8
	// (set) Token: 0x060017DC RID: 6108 RVA: 0x00071CE0 File Offset: 0x0006FEE0
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

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x060017DD RID: 6109 RVA: 0x00071CEC File Offset: 0x0006FEEC
	// (set) Token: 0x060017DE RID: 6110 RVA: 0x00071CF4 File Offset: 0x0006FEF4
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

	// Token: 0x060017DF RID: 6111 RVA: 0x00071D00 File Offset: 0x0006FF00
	protected void Start()
	{
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x00071D04 File Offset: 0x0006FF04
	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 vector;
		if (base.State == dfGestureState.Possible)
		{
			float num = Time.realtimeSinceStartup - base.StartTime;
			if (num <= this.timeout && Vector2.Distance(args.Position, base.StartPosition) <= this.maxDistance)
			{
				vector = args.Position;
				base.CurrentPosition = vector;
				base.StartPosition = vector;
				base.State = dfGestureState.Began;
				if (this.DoubleTapGesture != null)
				{
					this.DoubleTapGesture(this);
				}
				base.gameObject.Signal("OnDoubleTapGesture", new object[] { this });
				this.endGesture();
				return;
			}
		}
		vector = args.Position;
		base.CurrentPosition = vector;
		base.StartPosition = vector;
		base.State = dfGestureState.Possible;
		base.StartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00071DD0 File Offset: 0x0006FFD0
	public void OnMouseLeave()
	{
		this.endGesture();
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00071DD8 File Offset: 0x0006FFD8
	public void OnMultiTouchEnd()
	{
		this.endGesture();
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00071DE0 File Offset: 0x0006FFE0
	public void OnMultiTouch()
	{
		this.endGesture();
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00071DE8 File Offset: 0x0006FFE8
	private void endGesture()
	{
		if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			base.State = dfGestureState.Ended;
		}
		else if (base.State == dfGestureState.Possible)
		{
			base.State = dfGestureState.Cancelled;
		}
		else
		{
			base.State = dfGestureState.None;
		}
	}

	// Token: 0x0400132E RID: 4910
	[SerializeField]
	private float timeout = 0.5f;

	// Token: 0x0400132F RID: 4911
	[SerializeField]
	private float maxDistance = 35f;
}
