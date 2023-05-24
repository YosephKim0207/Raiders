using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000418 RID: 1048
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct dfTouchInfo
{
	// Token: 0x060017C7 RID: 6087 RVA: 0x00071A30 File Offset: 0x0006FC30
	public dfTouchInfo(int fingerID, TouchPhase phase, int tapCount, Vector2 position, Vector2 positionDelta, float timeDelta)
	{
		this.m_FingerId = fingerID;
		this.m_Phase = phase;
		this.m_Position = position;
		this.m_PositionDelta = positionDelta;
		this.m_TapCount = tapCount;
		this.m_TimeDelta = timeDelta;
		this.m_RawPosition = position;
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00071A68 File Offset: 0x0006FC68
	public int fingerId
	{
		get
		{
			return this.m_FingerId;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060017C9 RID: 6089 RVA: 0x00071A70 File Offset: 0x0006FC70
	public Vector2 position
	{
		get
		{
			return this.m_Position;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060017CA RID: 6090 RVA: 0x00071A78 File Offset: 0x0006FC78
	public Vector2 rawPosition
	{
		get
		{
			return this.m_RawPosition;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060017CB RID: 6091 RVA: 0x00071A80 File Offset: 0x0006FC80
	public Vector2 deltaPosition
	{
		get
		{
			return this.m_PositionDelta;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x060017CC RID: 6092 RVA: 0x00071A88 File Offset: 0x0006FC88
	public float deltaTime
	{
		get
		{
			return this.m_TimeDelta;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x00071A90 File Offset: 0x0006FC90
	public int tapCount
	{
		get
		{
			return this.m_TapCount;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060017CE RID: 6094 RVA: 0x00071A98 File Offset: 0x0006FC98
	public TouchPhase phase
	{
		get
		{
			return this.m_Phase;
		}
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00071AA0 File Offset: 0x0006FCA0
	public static implicit operator dfTouchInfo(Touch touch)
	{
		return new dfTouchInfo
		{
			m_PositionDelta = touch.deltaPosition,
			m_TimeDelta = touch.deltaTime,
			m_FingerId = touch.fingerId,
			m_Phase = touch.phase,
			m_Position = touch.position,
			m_TapCount = touch.tapCount
		};
	}

	// Token: 0x0400131F RID: 4895
	private int m_FingerId;

	// Token: 0x04001320 RID: 4896
	private Vector2 m_Position;

	// Token: 0x04001321 RID: 4897
	private Vector2 m_RawPosition;

	// Token: 0x04001322 RID: 4898
	private Vector2 m_PositionDelta;

	// Token: 0x04001323 RID: 4899
	private float m_TimeDelta;

	// Token: 0x04001324 RID: 4900
	private int m_TapCount;

	// Token: 0x04001325 RID: 4901
	private TouchPhase m_Phase;
}
