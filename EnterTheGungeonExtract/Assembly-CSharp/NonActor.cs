using System;
using UnityEngine;

// Token: 0x020012A5 RID: 4773
public class NonActor : GameActor
{
	// Token: 0x06006AC8 RID: 27336 RVA: 0x0029E004 File Offset: 0x0029C204
	public override void Awake()
	{
	}

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x06006AC9 RID: 27337 RVA: 0x0029E008 File Offset: 0x0029C208
	public override Gun CurrentGun
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x06006ACA RID: 27338 RVA: 0x0029E00C File Offset: 0x0029C20C
	public override Transform GunPivot
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x06006ACB RID: 27339 RVA: 0x0029E010 File Offset: 0x0029C210
	public override Vector3 SpriteDimensions
	{
		get
		{
			return Vector3.zero;
		}
	}

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x06006ACC RID: 27340 RVA: 0x0029E018 File Offset: 0x0029C218
	public override bool SpriteFlipped
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006ACD RID: 27341 RVA: 0x0029E01C File Offset: 0x0029C21C
	public override void Update()
	{
	}
}
