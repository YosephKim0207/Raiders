using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200010B RID: 267
[InspectorDropdownName("BulletManMagic/Astral2")]
public class BulletManMagicAstral2 : Script
{
	// Token: 0x060003FF RID: 1023 RVA: 0x000137C4 File Offset: 0x000119C4
	protected override IEnumerator Top()
	{
		base.Fire(new BulletManMagicAstral2.AstralBullet());
		return null;
	}

	// Token: 0x0200010C RID: 268
	public class AstralBullet : Bullet
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x000137D4 File Offset: 0x000119D4
		public AstralBullet()
			: base("astral", false, false, false)
		{
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000137E4 File Offset: 0x000119E4
		protected override IEnumerator Top()
		{
			yield return base.Wait(30);
			this.Projectile.specRigidbody.CollideWithOthers = true;
			this.Direction = base.AimDirection;
			this.Speed = 1.5f;
			for (int i = 0; i < 105; i++)
			{
				this.Direction = base.AimDirection;
				yield return base.Wait(1);
			}
			float startDirection = base.RandomAngle();
			float delta = 20f;
			for (int j = 0; j < 18; j++)
			{
				base.Fire(new Direction(startDirection + (float)j * delta, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040003FD RID: 1021
		private const int NumBullets = 18;
	}
}
