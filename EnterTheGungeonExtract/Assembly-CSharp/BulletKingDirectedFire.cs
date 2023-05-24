using System;
using Brave.BulletScript;

// Token: 0x020000E8 RID: 232
public abstract class BulletKingDirectedFire : Script
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000382 RID: 898 RVA: 0x0001177C File Offset: 0x0000F97C
	public bool IsHard
	{
		get
		{
			return this is BulletKingDirectedFireHard;
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00011788 File Offset: 0x0000F988
	protected void DirectedShots(float x, float y, float direction)
	{
		direction -= 90f;
		if (this.IsHard)
		{
			direction += 15f;
		}
		base.Fire(new Offset(x, y, 0f, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed((float)((!this.IsHard) ? 12 : 16), SpeedType.Absolute), new Bullet("directedfire", false, false, false));
		if (this.IsHard)
		{
			direction += 30f;
			base.Fire(new Offset(x, y, 0f, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed((float)((!this.IsHard) ? 12 : 16), SpeedType.Absolute), new Bullet("directedfire", false, false, false));
		}
	}
}
