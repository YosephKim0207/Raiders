using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000420 RID: 1056
[AddComponentMenu("Daikon Forge/Input/Gestures/Pan")]
public class dfPanGesture : dfGestureBase
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06001815 RID: 6165 RVA: 0x000724A0 File Offset: 0x000706A0
	// (remove) Token: 0x06001816 RID: 6166 RVA: 0x000724D8 File Offset: 0x000706D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfPanGesture> PanGestureStart;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06001817 RID: 6167 RVA: 0x00072510 File Offset: 0x00070710
	// (remove) Token: 0x06001818 RID: 6168 RVA: 0x00072548 File Offset: 0x00070748
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfPanGesture> PanGestureMove;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06001819 RID: 6169 RVA: 0x00072580 File Offset: 0x00070780
	// (remove) Token: 0x0600181A RID: 6170 RVA: 0x000725B8 File Offset: 0x000707B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfPanGesture> PanGestureEnd;

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x0600181B RID: 6171 RVA: 0x000725F0 File Offset: 0x000707F0
	// (set) Token: 0x0600181C RID: 6172 RVA: 0x000725F8 File Offset: 0x000707F8
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

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x0600181D RID: 6173 RVA: 0x00072604 File Offset: 0x00070804
	// (set) Token: 0x0600181E RID: 6174 RVA: 0x0007260C File Offset: 0x0007080C
	public Vector2 Delta { get; protected set; }

	// Token: 0x0600181F RID: 6175 RVA: 0x00072618 File Offset: 0x00070818
	protected void Start()
	{
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x0007261C File Offset: 0x0007081C
	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 position = args.Position;
		base.CurrentPosition = position;
		base.StartPosition = position;
		base.State = dfGestureState.Possible;
		base.StartTime = Time.realtimeSinceStartup;
		this.Delta = Vector2.zero;
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x0007265C File Offset: 0x0007085C
	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (base.State == dfGestureState.Possible)
		{
			if (Vector2.Distance(args.Position, base.StartPosition) >= this.minDistance)
			{
				base.State = dfGestureState.Began;
				base.CurrentPosition = args.Position;
				this.Delta = args.Position - base.StartPosition;
				if (this.PanGestureStart != null)
				{
					this.PanGestureStart(this);
				}
				base.gameObject.Signal("OnPanGestureStart", new object[] { this });
			}
		}
		else if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			base.State = dfGestureState.Changed;
			this.Delta = args.Position - base.CurrentPosition;
			base.CurrentPosition = args.Position;
			if (this.PanGestureMove != null)
			{
				this.PanGestureMove(this);
			}
			base.gameObject.Signal("OnPanGestureMove", new object[] { this });
		}
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x00072768 File Offset: 0x00070968
	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		this.endPanGesture();
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00072770 File Offset: 0x00070970
	public void OnMultiTouchEnd()
	{
		this.endPanGesture();
		this.multiTouchMode = false;
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x00072780 File Offset: 0x00070980
	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		Vector2 center = this.getCenter(args.Touches);
		if (!this.multiTouchMode)
		{
			this.endPanGesture();
			this.multiTouchMode = true;
			base.State = dfGestureState.Possible;
			base.StartPosition = center;
		}
		else if (base.State == dfGestureState.Possible)
		{
			if (Vector2.Distance(center, base.StartPosition) >= this.minDistance)
			{
				base.State = dfGestureState.Began;
				base.CurrentPosition = center;
				this.Delta = base.CurrentPosition - base.StartPosition;
				if (this.PanGestureStart != null)
				{
					this.PanGestureStart(this);
				}
				base.gameObject.Signal("OnPanGestureStart", new object[] { this });
			}
		}
		else if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			base.State = dfGestureState.Changed;
			this.Delta = center - base.CurrentPosition;
			base.CurrentPosition = center;
			if (this.PanGestureMove != null)
			{
				this.PanGestureMove(this);
			}
			base.gameObject.Signal("OnPanGestureMove", new object[] { this });
		}
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x000728B0 File Offset: 0x00070AB0
	private Vector2 getCenter(List<dfTouchInfo> list)
	{
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < list.Count; i++)
		{
			vector += list[i].position;
		}
		return vector / (float)list.Count;
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x00072900 File Offset: 0x00070B00
	private void endPanGesture()
	{
		this.Delta = Vector2.zero;
		base.StartPosition = Vector2.one * float.MinValue;
		if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			base.State = dfGestureState.Ended;
			if (this.PanGestureEnd != null)
			{
				this.PanGestureEnd(this);
			}
			base.gameObject.Signal("OnPanGestureEnd", new object[] { this });
		}
		else if (base.State == dfGestureState.Possible)
		{
			base.State = dfGestureState.Cancelled;
		}
	}

	// Token: 0x04001349 RID: 4937
	[SerializeField]
	protected float minDistance = 25f;

	// Token: 0x0400134A RID: 4938
	private bool multiTouchMode;
}
