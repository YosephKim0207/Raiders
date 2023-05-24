using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000762 RID: 1890
	public class Touch
	{
		// Token: 0x06002A21 RID: 10785 RVA: 0x000BFBC4 File Offset: 0x000BDDC4
		internal Touch()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000BFBE0 File Offset: 0x000BDDE0
		internal void Reset()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
			this.tapCount = 0;
			this.position = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
			this.deltaTime = 0f;
			this.updateTick = 0UL;
			this.type = TouchType.Direct;
			this.altitudeAngle = 0f;
			this.azimuthAngle = 0f;
			this.maximumPossiblePressure = 1f;
			this.pressure = 0f;
			this.radius = 0f;
			this.radiusVariance = 0f;
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000BFC84 File Offset: 0x000BDE84
		public float normalizedPressure
		{
			get
			{
				return Mathf.Clamp(this.pressure / this.maximumPossiblePressure, 0.001f, 1f);
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000BFCA4 File Offset: 0x000BDEA4
		internal void SetWithTouchData(Touch touch, ulong updateTick, float deltaTime)
		{
			this.phase = touch.phase;
			this.tapCount = touch.tapCount;
			this.altitudeAngle = touch.altitudeAngle;
			this.azimuthAngle = touch.azimuthAngle;
			this.maximumPossiblePressure = touch.maximumPossiblePressure;
			this.pressure = touch.pressure;
			this.radius = touch.radius;
			this.radiusVariance = touch.radiusVariance;
			Vector2 vector = touch.position;
			if (vector.x < 0f)
			{
				vector.x = (float)Screen.width + vector.x;
			}
			if (this.phase == TouchPhase.Began)
			{
				this.deltaPosition = Vector2.zero;
				this.lastPosition = vector;
				this.position = vector;
			}
			else
			{
				if (this.phase == TouchPhase.Stationary)
				{
					this.phase = TouchPhase.Moved;
				}
				this.deltaPosition = vector - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector;
			}
			this.deltaTime = deltaTime;
			this.updateTick = updateTick;
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000BFDB8 File Offset: 0x000BDFB8
		internal bool SetWithMouseData(ulong updateTick, float deltaTime)
		{
			if (Input.touchCount > 0)
			{
				return false;
			}
			Vector2 vector = new Vector2(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y));
			if (Input.GetMouseButtonDown(0))
			{
				this.phase = TouchPhase.Began;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = Vector2.zero;
				this.lastPosition = vector;
				this.position = vector;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.phase = TouchPhase.Ended;
				this.pressure = 0f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = vector - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButton(0))
			{
				this.phase = TouchPhase.Moved;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = vector - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			return false;
		}

		// Token: 0x04001CF6 RID: 7414
		public static readonly int FingerID_None = -1;

		// Token: 0x04001CF7 RID: 7415
		public static readonly int FingerID_Mouse = -2;

		// Token: 0x04001CF8 RID: 7416
		public int fingerId;

		// Token: 0x04001CF9 RID: 7417
		public TouchPhase phase;

		// Token: 0x04001CFA RID: 7418
		public int tapCount;

		// Token: 0x04001CFB RID: 7419
		public Vector2 position;

		// Token: 0x04001CFC RID: 7420
		public Vector2 deltaPosition;

		// Token: 0x04001CFD RID: 7421
		public Vector2 lastPosition;

		// Token: 0x04001CFE RID: 7422
		public float deltaTime;

		// Token: 0x04001CFF RID: 7423
		public ulong updateTick;

		// Token: 0x04001D00 RID: 7424
		public TouchType type;

		// Token: 0x04001D01 RID: 7425
		public float altitudeAngle;

		// Token: 0x04001D02 RID: 7426
		public float azimuthAngle;

		// Token: 0x04001D03 RID: 7427
		public float maximumPossiblePressure;

		// Token: 0x04001D04 RID: 7428
		public float pressure;

		// Token: 0x04001D05 RID: 7429
		public float radius;

		// Token: 0x04001D06 RID: 7430
		public float radiusVariance;
	}
}
