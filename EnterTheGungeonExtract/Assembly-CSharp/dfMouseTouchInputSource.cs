using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class dfMouseTouchInputSource : IDFTouchInputSource
{
	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06001860 RID: 6240 RVA: 0x00073704 File Offset: 0x00071904
	// (set) Token: 0x06001861 RID: 6241 RVA: 0x0007370C File Offset: 0x0007190C
	public bool MirrorAlt { get; set; }

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06001862 RID: 6242 RVA: 0x00073718 File Offset: 0x00071918
	// (set) Token: 0x06001863 RID: 6243 RVA: 0x00073720 File Offset: 0x00071920
	public bool ParallelAlt { get; set; }

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06001864 RID: 6244 RVA: 0x0007372C File Offset: 0x0007192C
	public int TouchCount
	{
		get
		{
			int num = 0;
			if (this.touch != null)
			{
				num++;
			}
			if (this.altTouch != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06001865 RID: 6245 RVA: 0x0007375C File Offset: 0x0007195C
	public IList<dfTouchInfo> Touches
	{
		get
		{
			this.activeTouches.Clear();
			if (this.touch != null)
			{
				this.activeTouches.Add(this.touch);
			}
			if (this.altTouch != null)
			{
				this.activeTouches.Add(this.altTouch);
			}
			return this.activeTouches;
		}
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000737BC File Offset: 0x000719BC
	public void Update()
	{
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(1))
		{
			if (this.altTouch != null)
			{
				this.altTouch.Phase = TouchPhase.Ended;
			}
			else
			{
				this.altTouch = new dfTouchTrackingInfo
				{
					Phase = TouchPhase.Began,
					FingerID = 1,
					Position = Input.mousePosition
				};
			}
			return;
		}
		if (Input.GetKeyUp(KeyCode.LeftAlt))
		{
			if (this.altTouch != null)
			{
				this.altTouch.Phase = TouchPhase.Ended;
				return;
			}
		}
		else if (this.altTouch != null)
		{
			if (this.altTouch.Phase == TouchPhase.Ended)
			{
				this.altTouch = null;
			}
			else if (this.altTouch.Phase == TouchPhase.Began || this.altTouch.Phase == TouchPhase.Moved)
			{
				this.altTouch.Phase = TouchPhase.Stationary;
			}
		}
		if (this.touch != null)
		{
			this.touch.IsActive = false;
		}
		if (this.touch != null && Input.GetKeyDown(KeyCode.Escape))
		{
			this.touch.Phase = TouchPhase.Canceled;
		}
		else if (this.touch == null || this.touch.Phase != TouchPhase.Canceled)
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (this.touch != null)
				{
					this.touch.Phase = TouchPhase.Ended;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.touch = new dfTouchTrackingInfo
				{
					FingerID = 0,
					Phase = TouchPhase.Began,
					Position = Input.mousePosition
				};
			}
			else if (this.touch != null && this.touch.Phase != TouchPhase.Ended)
			{
				Vector2 vector = Input.mousePosition - this.touch.Position;
				bool flag = Vector2.Distance(Input.mousePosition, this.touch.Position) > float.Epsilon;
				this.touch.Position = Input.mousePosition;
				this.touch.Phase = ((!flag) ? TouchPhase.Stationary : TouchPhase.Moved);
				if (flag && this.altTouch != null && (this.MirrorAlt || this.ParallelAlt))
				{
					if (this.MirrorAlt)
					{
						this.altTouch.Position -= vector;
					}
					else
					{
						this.altTouch.Position += vector;
					}
					this.altTouch.Phase = TouchPhase.Moved;
				}
			}
		}
		if (this.touch != null && !this.touch.IsActive)
		{
			this.touch = null;
		}
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x00073A84 File Offset: 0x00071C84
	public dfTouchInfo GetTouch(int index)
	{
		return this.Touches[index];
	}

	// Token: 0x04001361 RID: 4961
	private List<dfTouchInfo> activeTouches = new List<dfTouchInfo>();

	// Token: 0x04001362 RID: 4962
	private dfTouchTrackingInfo touch;

	// Token: 0x04001363 RID: 4963
	private dfTouchTrackingInfo altTouch;
}
