using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000107 RID: 263
[InspectorDropdownName("BulletManMagic/Astral1")]
public class BulletManMagicAstral1 : Script
{
	// Token: 0x060003EE RID: 1006 RVA: 0x00013560 File Offset: 0x00011760
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new BulletManMagicAstral1.AstralBullet(this));
		yield return base.Wait(180);
		yield break;
	}

	// Token: 0x040003F1 RID: 1009
	private const int AirTime = 180;

	// Token: 0x02000108 RID: 264
	public class AstralBullet : Bullet
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x0001357C File Offset: 0x0001177C
		public AstralBullet(Script parentScript)
			: base("astral", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00013594 File Offset: 0x00011794
		protected override IEnumerator Top()
		{
			HealthHaver owner = base.BulletBank.healthHaver;
			for (int i = 0; i < 180; i++)
			{
				base.ChangeDirection(new Direction(0f, DirectionType.Aim, 3f), 1);
				if (!owner || owner.IsDead)
				{
					base.Vanish(false);
					yield break;
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000135B0 File Offset: 0x000117B0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.m_parentScript != null)
			{
				this.m_parentScript.ForceEnd();
			}
		}

		// Token: 0x040003F2 RID: 1010
		private Script m_parentScript;
	}
}
