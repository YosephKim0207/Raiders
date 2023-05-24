using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200009D RID: 157
[InspectorDropdownName("Bosses/BossFinalRogue/BarGauntlet1")]
public class BossFinalRogueBarGauntlet1 : Script
{
	// Token: 0x06000269 RID: 617 RVA: 0x0000C200 File Offset: 0x0000A400
	protected override IEnumerator Top()
	{
		yield return base.Wait(180);
		base.EndOnBlank = true;
		int num = UnityEngine.Random.Range(0, 3);
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					float bulletSeparation = 6f;
					for (int i = 0; i < 5; i++)
					{
						for (int l = 1; l <= 6; l++)
						{
							int num2 = l;
							if (l > 3)
							{
								num2--;
							}
							this.Fire(num2, l, bulletSeparation, 6);
						}
						if (i != 4)
						{
							yield return base.Wait(80);
						}
					}
				}
			}
			else
			{
				int skipIndex = 3;
				float bulletSeparation = 6f;
				for (int j = 0; j < 9; j++)
				{
					skipIndex = BraveUtility.SequentialRandomRange(1, 6, skipIndex, new int?(2), true);
					for (int m = 1; m <= 6; m++)
					{
						if (m < skipIndex || m > skipIndex + 1)
						{
							int num3 = m;
							if (m > 3)
							{
								num3--;
							}
							this.Fire(num3, m, bulletSeparation, 6);
						}
					}
					if (j != 8)
					{
						yield return base.Wait(40);
					}
				}
			}
		}
		else
		{
			float bulletSeparation = 6f;
			this.Fire(2, 3, bulletSeparation, 6);
			yield return base.Wait(5);
			this.Fire(3, 4, bulletSeparation, 6);
			yield return base.Wait(5);
			this.Fire(4, 5, bulletSeparation, 6);
			yield return base.Wait(5);
			this.Fire(5, 6, bulletSeparation, 6);
			yield return base.Wait(5);
			yield return base.Wait(14);
			for (int k = 0; k < 3; k++)
			{
				this.Fire(1, 1, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(2, 2, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(3, 3, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(4, 4, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(3, 3, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(2, 2, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(1, 1, bulletSeparation, 6);
				yield return base.Wait(5);
				yield return base.Wait(14);
				this.Fire(5, 6, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(4, 5, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(3, 4, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(2, 3, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(3, 4, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(4, 5, bulletSeparation, 6);
				yield return base.Wait(5);
				this.Fire(5, 6, bulletSeparation, 6);
				yield return base.Wait(5);
				yield return base.Wait(14);
			}
		}
		yield break;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000C21C File Offset: 0x0000A41C
	private void Fire(int gunNum, float lineWidth, int numBullets)
	{
		this.Fire(gunNum, gunNum, lineWidth, numBullets);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000C228 File Offset: 0x0000A428
	private void Fire(int shootPointNum, int gunNum, float bulletSeparation, int numBullets)
	{
		base.Fire(new Offset("bar gun " + shootPointNum), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BossFinalRogueBarGauntlet1.BarBullet(gunNum, base.Position.x, bulletSeparation, numBullets));
	}

	// Token: 0x0200009E RID: 158
	public class BarBullet : Bullet
	{
		// Token: 0x0600026C RID: 620 RVA: 0x0000C284 File Offset: 0x0000A484
		public BarBullet(int gunNum, float centerX, float lineWidth, int numBullets)
			: base("bar", false, false, false)
		{
			this.m_gunNum = gunNum;
			this.m_centerX = centerX;
			this.m_lineWidth = lineWidth;
			this.m_numBullets = numBullets;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			float startingX = base.Position.x;
			float desiredX = this.m_centerX + base.SubdivideRange(0f, this.m_lineWidth, this.m_numBullets, this.m_gunNum - 1, false) - this.m_lineWidth / 2f;
			for (int i = 0; i < 240; i++)
			{
				base.UpdateVelocity();
				if (i < 30)
				{
					base.Position = new Vector2(Mathf.Lerp(startingX, desiredX, (float)i / 29f), base.Position.y + this.Velocity.y / 60f);
				}
				else
				{
					base.UpdatePosition();
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000299 RID: 665
		private int m_gunNum;

		// Token: 0x0400029A RID: 666
		private float m_centerX;

		// Token: 0x0400029B RID: 667
		private float m_lineWidth;

		// Token: 0x0400029C RID: 668
		private int m_numBullets;
	}
}
