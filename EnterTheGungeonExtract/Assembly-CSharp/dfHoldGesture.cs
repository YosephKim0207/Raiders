using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200041F RID: 1055
[AddComponentMenu("Daikon Forge/Input/Gestures/Hold")]
public class dfHoldGesture : dfGestureBase
{
	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06001805 RID: 6149 RVA: 0x0007214C File Offset: 0x0007034C
	// (remove) Token: 0x06001806 RID: 6150 RVA: 0x00072184 File Offset: 0x00070384
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfHoldGesture> HoldGestureStart;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06001807 RID: 6151 RVA: 0x000721BC File Offset: 0x000703BC
	// (remove) Token: 0x06001808 RID: 6152 RVA: 0x000721F4 File Offset: 0x000703F4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfHoldGesture> HoldGestureEnd;

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06001809 RID: 6153 RVA: 0x0007222C File Offset: 0x0007042C
	// (set) Token: 0x0600180A RID: 6154 RVA: 0x00072234 File Offset: 0x00070434
	public float MinimumTime
	{
		get
		{
			return this.minTime;
		}
		set
		{
			this.minTime = value;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x0600180B RID: 6155 RVA: 0x00072240 File Offset: 0x00070440
	// (set) Token: 0x0600180C RID: 6156 RVA: 0x00072248 File Offset: 0x00070448
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

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x0600180D RID: 6157 RVA: 0x00072254 File Offset: 0x00070454
	public float HoldLength
	{
		get
		{
			if (base.State == dfGestureState.Began)
			{
				return Time.realtimeSinceStartup - base.StartTime;
			}
			return 0f;
		}
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x00072274 File Offset: 0x00070474
	protected void Start()
	{
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x00072278 File Offset: 0x00070478
	protected void Update()
	{
		if (base.State == dfGestureState.Possible && Time.realtimeSinceStartup - base.StartTime >= this.minTime)
		{
			base.State = dfGestureState.Began;
			if (this.HoldGestureStart != null)
			{
				this.HoldGestureStart(this);
			}
			base.gameObject.Signal("OnHoldGestureStart", new object[] { this });
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000722E4 File Offset: 0x000704E4
	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		base.State = dfGestureState.Possible;
		Vector2 position = args.Position;
		base.CurrentPosition = position;
		base.StartPosition = position;
		base.StartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00072318 File Offset: 0x00070518
	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (base.State != dfGestureState.Possible && base.State != dfGestureState.Began)
		{
			return;
		}
		base.CurrentPosition = args.Position;
		if (Vector2.Distance(args.Position, base.StartPosition) > this.maxDistance)
		{
			if (base.State == dfGestureState.Possible)
			{
				base.State = dfGestureState.Failed;
			}
			else if (base.State == dfGestureState.Began)
			{
				base.State = dfGestureState.Cancelled;
				if (this.HoldGestureEnd != null)
				{
					this.HoldGestureEnd(this);
				}
				base.gameObject.Signal("OnHoldGestureEnd", new object[] { this });
			}
		}
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000723C4 File Offset: 0x000705C4
	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Began)
		{
			base.CurrentPosition = args.Position;
			base.State = dfGestureState.Ended;
			if (this.HoldGestureEnd != null)
			{
				this.HoldGestureEnd(this);
			}
			base.gameObject.Signal("OnHoldGestureEnd", new object[] { this });
		}
		base.State = dfGestureState.None;
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x0007242C File Offset: 0x0007062C
	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		if (base.State == dfGestureState.Began)
		{
			base.State = dfGestureState.Cancelled;
			if (this.HoldGestureEnd != null)
			{
				this.HoldGestureEnd(this);
			}
			base.gameObject.Signal("OnHoldGestureEnd", new object[] { this });
		}
		else
		{
			base.State = dfGestureState.Failed;
		}
	}

	// Token: 0x04001344 RID: 4932
	[SerializeField]
	private float minTime = 0.75f;

	// Token: 0x04001345 RID: 4933
	[SerializeField]
	private float maxDistance = 25f;
}
