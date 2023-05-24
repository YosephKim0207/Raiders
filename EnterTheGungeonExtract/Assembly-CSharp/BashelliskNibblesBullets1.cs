using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200001B RID: 27
[InspectorDropdownName("Bosses/Bashellisk/NibblesBullets1")]
public class BashelliskNibblesBullets1 : Script
{
	// Token: 0x06000066 RID: 102 RVA: 0x000036A4 File Offset: 0x000018A4
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.QuantizeFloat(base.GetAimDirection(1f, 12f), 90f);
		BashelliskNibblesBullets1.NibblesBullet nibblesBullet = null;
		for (int i = 0; i < 8; i++)
		{
			BashelliskNibblesBullets1.NibblesBullet nibblesBullet2 = new BashelliskNibblesBullets1.NibblesBullet(i, nibblesBullet);
			base.Fire(new Direction(BraveMathCollege.QuantizeFloat(num, 90f), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), nibblesBullet2);
			nibblesBullet = nibblesBullet2;
		}
		return null;
	}

	// Token: 0x04000066 RID: 102
	private const int NumBullets = 8;

	// Token: 0x04000067 RID: 103
	private const int BulletSpeed = 12;

	// Token: 0x04000068 RID: 104
	private const int NibblesTickTime = 3;

	// Token: 0x04000069 RID: 105
	private const int NibblesTurnCooldown = 200;

	// Token: 0x0400006A RID: 106
	private const float NibblesTurnChance = 0.07f;

	// Token: 0x0200001C RID: 28
	public class NibblesBullet : Bullet
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003714 File Offset: 0x00001914
		public NibblesBullet(int delay, BashelliskNibblesBullets1.NibblesBullet parent)
			: base("nibblesBullet", false, false, false)
		{
			this.delay = delay;
			this.parent = parent;
			if (parent != null)
			{
				parent.child = this;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003740 File Offset: 0x00001940
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			if (this.parent != null)
			{
				while (this.parent != null && !this.parent.Destroyed)
				{
					yield return base.Wait(1);
				}
				base.Vanish(false);
				yield break;
			}
			while (this.child == null)
			{
				yield return base.Wait(1);
			}
			if (this.delay > 0)
			{
				yield return base.Wait(this.delay * 3);
			}
			int preTurnTime = -1;
			this.turnCooldown = 8 - this.delay;
			for (int i = 0; i < 120; i++)
			{
				if (this.turnCooldown == 0 && preTurnTime < 0 && UnityEngine.Random.value < 0.07f)
				{
					preTurnTime = 20;
					this.Projectile.spriteAnimator.Play();
				}
				if (preTurnTime >= 0)
				{
					preTurnTime--;
					if (preTurnTime <= 0)
					{
						this.prevDirection = this.Direction;
						this.Direction += BraveUtility.RandomSign() * 90f;
						this.turnCooldown = 201;
						preTurnTime = -1;
						this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
					}
				}
				this.prevPosition = base.Position;
				base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f * 3f);
				BashelliskNibblesBullets1.NibblesBullet ptr = this;
				while (ptr.child != null)
				{
					if (ptr.delay > i)
					{
						break;
					}
					ptr.child.prevDirection = ptr.child.Direction;
					ptr.child.Direction = ptr.prevDirection;
					ptr.child.prevPosition = ptr.child.Position;
					ptr.child.Position = ptr.prevPosition;
					ptr = ptr.child;
				}
				if (this.turnCooldown > 0)
				{
					this.turnCooldown--;
				}
				yield return base.Wait(3);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400006B RID: 107
		private int delay;

		// Token: 0x0400006C RID: 108
		private BashelliskNibblesBullets1.NibblesBullet parent;

		// Token: 0x0400006D RID: 109
		private BashelliskNibblesBullets1.NibblesBullet child;

		// Token: 0x0400006E RID: 110
		private float prevDirection;

		// Token: 0x0400006F RID: 111
		private Vector2 prevPosition;

		// Token: 0x04000070 RID: 112
		private int turnCooldown;
	}
}
