using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000421 RID: 1057
[AddComponentMenu("Daikon Forge/Input/Gestures/Resize")]
public class dfResizeGesture : dfGestureBase
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06001828 RID: 6184 RVA: 0x000729A0 File Offset: 0x00070BA0
	// (remove) Token: 0x06001829 RID: 6185 RVA: 0x000729D8 File Offset: 0x00070BD8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureStart;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x0600182A RID: 6186 RVA: 0x00072A10 File Offset: 0x00070C10
	// (remove) Token: 0x0600182B RID: 6187 RVA: 0x00072A48 File Offset: 0x00070C48
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureUpdate;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x0600182C RID: 6188 RVA: 0x00072A80 File Offset: 0x00070C80
	// (remove) Token: 0x0600182D RID: 6189 RVA: 0x00072AB8 File Offset: 0x00070CB8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureEnd;

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x0600182E RID: 6190 RVA: 0x00072AF0 File Offset: 0x00070CF0
	// (set) Token: 0x0600182F RID: 6191 RVA: 0x00072AF8 File Offset: 0x00070CF8
	public float SizeDelta { get; protected set; }

	// Token: 0x06001830 RID: 6192 RVA: 0x00072B04 File Offset: 0x00070D04
	protected void Start()
	{
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x00072B08 File Offset: 0x00070D08
	public void OnMultiTouchEnd()
	{
		this.endGesture();
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x00072B10 File Offset: 0x00070D10
	public void OnMultiTouch(dfControl sender, dfTouchEventArgs args)
	{
		List<dfTouchInfo> touches = args.Touches;
		if (base.State == dfGestureState.None || base.State == dfGestureState.Cancelled || base.State == dfGestureState.Ended)
		{
			base.State = dfGestureState.Possible;
		}
		else if (base.State == dfGestureState.Possible)
		{
			if (this.isResizeMovement(args.Touches))
			{
				base.State = dfGestureState.Began;
				Vector2 center = this.getCenter(touches);
				base.CurrentPosition = center;
				base.StartPosition = center;
				this.lastDistance = Vector2.Distance(touches[0].position, touches[1].position);
				this.SizeDelta = 0f;
				if (this.ResizeGestureStart != null)
				{
					this.ResizeGestureStart(this);
				}
				base.gameObject.Signal("OnResizeGestureStart", new object[] { this });
			}
		}
		else if ((base.State == dfGestureState.Began || base.State == dfGestureState.Changed) && this.isResizeMovement(touches))
		{
			base.State = dfGestureState.Changed;
			base.CurrentPosition = this.getCenter(touches);
			float num = Vector2.Distance(touches[0].position, touches[1].position);
			this.SizeDelta = num - this.lastDistance;
			this.lastDistance = num;
			if (this.ResizeGestureUpdate != null)
			{
				this.ResizeGestureUpdate(this);
			}
			base.gameObject.Signal("OnResizeGestureUpdate", new object[] { this });
		}
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x00072CA0 File Offset: 0x00070EA0
	private Vector2 getCenter(List<dfTouchInfo> list)
	{
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < list.Count; i++)
		{
			vector += list[i].position;
		}
		return vector / (float)list.Count;
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x00072CF0 File Offset: 0x00070EF0
	private bool isResizeMovement(List<dfTouchInfo> list)
	{
		if (list.Count < 2)
		{
			return false;
		}
		dfTouchInfo dfTouchInfo = list[0];
		Vector2 normalized = (dfTouchInfo.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo.deltaTime)).normalized;
		dfTouchInfo dfTouchInfo2 = list[1];
		Vector2 normalized2 = (dfTouchInfo2.deltaPosition * (BraveTime.DeltaTime / dfTouchInfo2.deltaTime)).normalized;
		float num = Vector2.Dot(normalized, (dfTouchInfo.position - dfTouchInfo2.position).normalized);
		float num2 = Vector2.Dot(normalized2, (dfTouchInfo2.position - dfTouchInfo.position).normalized);
		return Mathf.Abs(num) >= 0.21460184f || Mathf.Abs(num2) >= 0.21460184f;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x00072DD0 File Offset: 0x00070FD0
	private void endGesture()
	{
		if (base.State == dfGestureState.Began || base.State == dfGestureState.Changed)
		{
			if (base.State == dfGestureState.Began)
			{
				base.State = dfGestureState.Cancelled;
			}
			else
			{
				base.State = dfGestureState.Ended;
			}
			float num = 0f;
			this.SizeDelta = num;
			this.lastDistance = num;
			if (this.ResizeGestureEnd != null)
			{
				this.ResizeGestureEnd(this);
			}
			base.gameObject.Signal("OnResizeGestureEnd", new object[] { this });
		}
		else
		{
			base.State = dfGestureState.None;
		}
	}

	// Token: 0x04001350 RID: 4944
	private float lastDistance;
}
