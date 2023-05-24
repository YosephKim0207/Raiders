using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200041B RID: 1051
[AddComponentMenu("Daikon Forge/Input/Gestures/Flick")]
public class dfFlickGesture : dfGestureBase
{
	// Token: 0x14000047 RID: 71
	// (add) Token: 0x060017E6 RID: 6118 RVA: 0x00071E58 File Offset: 0x00070058
	// (remove) Token: 0x060017E7 RID: 6119 RVA: 0x00071E90 File Offset: 0x00070090
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfFlickGesture> FlickGesture;

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00071EC8 File Offset: 0x000700C8
	// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00071ED0 File Offset: 0x000700D0
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

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x060017EA RID: 6122 RVA: 0x00071EDC File Offset: 0x000700DC
	// (set) Token: 0x060017EB RID: 6123 RVA: 0x00071EE4 File Offset: 0x000700E4
	public float MinimumDistance
	{
		get
		{
			return this.minDistance;
		}
		set
		{
			this.minDistance = value;
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x060017EC RID: 6124 RVA: 0x00071EF0 File Offset: 0x000700F0
	// (set) Token: 0x060017ED RID: 6125 RVA: 0x00071EF8 File Offset: 0x000700F8
	public float DeltaTime { get; protected set; }

	// Token: 0x060017EE RID: 6126 RVA: 0x00071F04 File Offset: 0x00070104
	protected void Start()
	{
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x00071F08 File Offset: 0x00070108
	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 position = args.Position;
		base.CurrentPosition = position;
		base.StartPosition = position;
		base.State = dfGestureState.Possible;
		base.StartTime = Time.realtimeSinceStartup;
		this.hoverTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x00071F48 File Offset: 0x00070148
	public void OnMouseHover(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Possible && Time.realtimeSinceStartup - this.hoverTime >= this.timeout)
		{
			Vector2 position = args.Position;
			base.CurrentPosition = position;
			base.StartPosition = position;
			base.StartTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x00071F98 File Offset: 0x00070198
	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		this.hoverTime = Time.realtimeSinceStartup;
		if (base.State == dfGestureState.Possible || base.State == dfGestureState.Began)
		{
			base.State = dfGestureState.Began;
			base.CurrentPosition = args.Position;
		}
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x00071FD0 File Offset: 0x000701D0
	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Began)
		{
			base.CurrentPosition = args.Position;
			if (Time.realtimeSinceStartup - base.StartTime <= this.timeout)
			{
				float num = Vector2.Distance(base.CurrentPosition, base.StartPosition);
				if (num >= this.minDistance)
				{
					base.State = dfGestureState.Ended;
					this.DeltaTime = Time.realtimeSinceStartup - base.StartTime;
					if (this.FlickGesture != null)
					{
						this.FlickGesture(this);
					}
					base.gameObject.Signal("OnFlickGesture", new object[] { this });
				}
				else
				{
					base.State = dfGestureState.Failed;
				}
			}
			else
			{
				base.State = dfGestureState.Failed;
			}
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x00072090 File Offset: 0x00070290
	public void OnMultiTouchEnd()
	{
		this.endGesture();
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x00072098 File Offset: 0x00070298
	public void OnMultiTouch()
	{
		this.endGesture();
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000720A0 File Offset: 0x000702A0
	private void endGesture()
	{
		base.State = dfGestureState.None;
	}

	// Token: 0x04001331 RID: 4913
	[SerializeField]
	private float timeout = 0.25f;

	// Token: 0x04001332 RID: 4914
	[SerializeField]
	private float minDistance = 25f;

	// Token: 0x04001334 RID: 4916
	private float hoverTime;
}
