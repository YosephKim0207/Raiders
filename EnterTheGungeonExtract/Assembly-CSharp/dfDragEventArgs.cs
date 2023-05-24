using System;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class dfDragEventArgs : dfControlEventArgs
{
	// Token: 0x060011FC RID: 4604 RVA: 0x00053080 File Offset: 0x00051280
	internal dfDragEventArgs(dfControl source)
		: base(source)
	{
		this.State = dfDragDropState.None;
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x00053090 File Offset: 0x00051290
	internal dfDragEventArgs(dfControl source, dfDragDropState state, object data, Ray ray, Vector2 position)
		: base(source)
	{
		this.Data = data;
		this.State = state;
		this.Position = position;
		this.Ray = ray;
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x000530B8 File Offset: 0x000512B8
	// (set) Token: 0x060011FF RID: 4607 RVA: 0x000530C0 File Offset: 0x000512C0
	public dfDragDropState State { get; set; }

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x000530CC File Offset: 0x000512CC
	// (set) Token: 0x06001201 RID: 4609 RVA: 0x000530D4 File Offset: 0x000512D4
	public object Data { get; set; }

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x000530E0 File Offset: 0x000512E0
	// (set) Token: 0x06001203 RID: 4611 RVA: 0x000530E8 File Offset: 0x000512E8
	public Vector2 Position { get; set; }

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x000530F4 File Offset: 0x000512F4
	// (set) Token: 0x06001205 RID: 4613 RVA: 0x000530FC File Offset: 0x000512FC
	public dfControl Target { get; set; }

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06001206 RID: 4614 RVA: 0x00053108 File Offset: 0x00051308
	// (set) Token: 0x06001207 RID: 4615 RVA: 0x00053110 File Offset: 0x00051310
	public Ray Ray { get; set; }
}
