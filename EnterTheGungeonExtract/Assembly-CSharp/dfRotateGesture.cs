using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000422 RID: 1058
[AddComponentMenu("Daikon Forge/Input/Gestures/Rotate")]
public class dfRotateGesture : dfGestureBase
{
	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06001837 RID: 6199 RVA: 0x00072E7C File Offset: 0x0007107C
	// (remove) Token: 0x06001838 RID: 6200 RVA: 0x00072EB4 File Offset: 0x000710B4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfRotateGesture> RotateGestureStart;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06001839 RID: 6201 RVA: 0x00072EEC File Offset: 0x000710EC
	// (remove) Token: 0x0600183A RID: 6202 RVA: 0x00072F24 File Offset: 0x00071124
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfRotateGesture> RotateGestureUpdate;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x0600183B RID: 6203 RVA: 0x00072F5C File Offset: 0x0007115C
	// (remove) Token: 0x0600183C RID: 6204 RVA: 0x00072F94 File Offset: 0x00071194
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfRotateGesture> RotateGestureEnd;

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x00072FCC File Offset: 0x000711CC
	// (set) Token: 0x0600183E RID: 6206 RVA: 0x00072FD4 File Offset: 0x000711D4
	public float AngleDelta { get; protected set; }

	// Token: 0x0600183F RID: 6207 RVA: 0x00072FE0 File Offset: 0x000711E0
	protected void Start()
	{
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x00072FE4 File Offset: 0x000711E4
	public void OnMultiTouchEnd()
	{
		this.endGesture();
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x00072FEC File Offset: 0x000711EC
	public void OnMultiTouch(dfControl sender, dfTouchEventArgs args)
	{
		List<dfTouchInfo> touches = args.Touches;
		if (base.State == dfGestureState.None || base.State == dfGestureState.Cancelled || base.State == dfGestureState.Ended)
		{
			base.State = dfGestureState.Possible;
			this.accumulatedDelta = 0f;
		}
		else if (base.State == dfGestureState.Possible)
		{
			if (this.isRotateMovement(args.Touches))
			{
				float num = this.getAngleDelta(touches) + this.accumulatedDelta;
				if (Mathf.Abs(num) < this.thresholdAngle)
				{
					this.accumulatedDelta = num;
					return;
				}
				base.State = dfGestureState.Began;
				Vector2 center = this.getCenter(touches);
				base.CurrentPosition = center;
				base.StartPosition = center;
				this.AngleDelta = num;
				if (this.RotateGestureStart != null)
				{
					this.RotateGestureStart(this);
				}
				base.gameObject.Signal("OnRotateGestureStart", new object[] { this });
			}
		}
		else if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			float angleDelta = this.getAngleDelta(touches);
			if (Mathf.Abs(angleDelta) <= 1E-45f || Mathf.Abs(angleDelta) > 22.5f)
			{
				return;
			}
			base.State = dfGestureState.Changed;
			this.AngleDelta = angleDelta;
			base.CurrentPosition = this.getCenter(touches);
			if (this.RotateGestureUpdate != null)
			{
				this.RotateGestureUpdate(this);
			}
			base.gameObject.Signal("OnRotateGestureUpdate", new object[] { this });
		}
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x00073168 File Offset: 0x00071368
	private float getAngleDelta(List<dfTouchInfo> touches)
	{
		if (touches.Count < 2)
		{
			return 0f;
		}
		dfTouchInfo dfTouchInfo = touches[0];
		dfTouchInfo dfTouchInfo2 = touches[1];
		if (Vector2.Distance(dfTouchInfo.deltaPosition, dfTouchInfo2.deltaPosition) <= 1E-45f)
		{
			return 0f;
		}
		Vector2 vector = dfTouchInfo.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo.deltaTime);
		Vector2 vector2 = dfTouchInfo2.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo2.deltaTime);
		Vector2 vector3 = dfTouchInfo.position - vector - (dfTouchInfo2.position - vector2);
		Vector2 vector4 = dfTouchInfo.position - dfTouchInfo2.position;
		float num = this.deltaAngle(vector3.normalized, vector4.normalized);
		if (float.IsNaN(num))
		{
			return 0f;
		}
		if (dfTouchInfo.phase == TouchPhase.Stationary || dfTouchInfo2.phase == TouchPhase.Stationary)
		{
			num *= 0.5f;
		}
		return num;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x00073274 File Offset: 0x00071474
	private float deltaAngle(Vector2 start, Vector2 end)
	{
		float num = start.x * end.y - start.y * end.x;
		return 57.29578f * Mathf.Atan2(num, Vector2.Dot(start, end));
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000732B4 File Offset: 0x000714B4
	private Vector2 getCenter(List<dfTouchInfo> list)
	{
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < list.Count; i++)
		{
			vector += list[i].position;
		}
		return vector / (float)list.Count;
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x00073304 File Offset: 0x00071504
	private bool isRotateMovement(List<dfTouchInfo> list)
	{
		return Mathf.Abs(this.getAngleDelta(list)) >= 0.1f;
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0007331C File Offset: 0x0007151C
	private void endGesture()
	{
		this.AngleDelta = 0f;
		this.accumulatedDelta = 0f;
		if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			base.State = dfGestureState.Ended;
			if (this.RotateGestureEnd != null)
			{
				this.RotateGestureEnd(this);
			}
			base.gameObject.Signal("OnRotateGestureEnd", new object[] { this });
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

	// Token: 0x04001354 RID: 4948
	[SerializeField]
	protected float thresholdAngle = 10f;

	// Token: 0x04001356 RID: 4950
	private float accumulatedDelta;
}
