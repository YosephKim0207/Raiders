using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000C22 RID: 3106
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct tk2dUITouch
{
	// Token: 0x060042E1 RID: 17121 RVA: 0x0015ABD8 File Offset: 0x00158DD8
	public tk2dUITouch(TouchPhase _phase, int _fingerId, Vector2 _position, Vector2 _deltaPosition, float _deltaTime)
	{
		this = default(tk2dUITouch);
		this.phase = _phase;
		this.fingerId = _fingerId;
		this.position = _position;
		this.deltaPosition = _deltaPosition;
		this.deltaTime = _deltaTime;
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x0015AC08 File Offset: 0x00158E08
	public tk2dUITouch(Touch touch)
	{
		this = default(tk2dUITouch);
		this.phase = touch.phase;
		this.fingerId = touch.fingerId;
		this.position = touch.position;
		this.deltaPosition = this.deltaPosition;
		this.deltaTime = this.deltaTime;
	}

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060042E3 RID: 17123 RVA: 0x0015AC5C File Offset: 0x00158E5C
	// (set) Token: 0x060042E4 RID: 17124 RVA: 0x0015AC64 File Offset: 0x00158E64
	public TouchPhase phase { get; private set; }

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060042E5 RID: 17125 RVA: 0x0015AC70 File Offset: 0x00158E70
	// (set) Token: 0x060042E6 RID: 17126 RVA: 0x0015AC78 File Offset: 0x00158E78
	public int fingerId { get; private set; }

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060042E7 RID: 17127 RVA: 0x0015AC84 File Offset: 0x00158E84
	// (set) Token: 0x060042E8 RID: 17128 RVA: 0x0015AC8C File Offset: 0x00158E8C
	public Vector2 position { get; private set; }

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x060042E9 RID: 17129 RVA: 0x0015AC98 File Offset: 0x00158E98
	// (set) Token: 0x060042EA RID: 17130 RVA: 0x0015ACA0 File Offset: 0x00158EA0
	public Vector2 deltaPosition { get; private set; }

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x060042EB RID: 17131 RVA: 0x0015ACAC File Offset: 0x00158EAC
	// (set) Token: 0x060042EC RID: 17132 RVA: 0x0015ACB4 File Offset: 0x00158EB4
	public float deltaTime { get; private set; }

	// Token: 0x060042ED RID: 17133 RVA: 0x0015ACC0 File Offset: 0x00158EC0
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			this.phase.ToString(),
			",",
			this.fingerId,
			",",
			this.position,
			",",
			this.deltaPosition,
			",",
			this.deltaTime
		});
	}

	// Token: 0x04003521 RID: 13601
	public const int MOUSE_POINTER_FINGER_ID = 9999;
}
