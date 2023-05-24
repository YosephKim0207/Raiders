using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200013B RID: 315
[InspectorDropdownName("Chancebulon/Dice1")]
public class ChancebulonDice1 : Script
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060004AB RID: 1195 RVA: 0x000165F4 File Offset: 0x000147F4
	// (set) Token: 0x060004AC RID: 1196 RVA: 0x000165FC File Offset: 0x000147FC
	public float aimDirection { get; private set; }

	// Token: 0x060004AD RID: 1197 RVA: 0x00016608 File Offset: 0x00014808
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		this.FireSquare();
		this.aimDirection = base.AimDirection;
		yield return base.Wait(15);
		float distanceToTarget = (this.BulletManager.PlayerPosition() - base.Position).magnitude;
		if (distanceToTarget > 4.5f)
		{
			this.aimDirection = base.GetAimDirection(1f, 10f);
		}
		yield break;
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00016624 File Offset: 0x00014824
	private void FireSquare()
	{
		Vector2 vector = new Vector2(2.2f, 0f).Rotate(45f);
		Vector2 vector2 = new Vector2(2.2f, 0f).Rotate(135f);
		Vector2 vector3 = new Vector2(2.2f, 0f).Rotate(225f);
		Vector2 vector4 = new Vector2(2.2f, 0f).Rotate(-45f);
		this.FireExpandingLine(vector, vector2, 5);
		this.FireExpandingLine(vector2, vector3, 5);
		this.FireExpandingLine(vector3, vector4, 5);
		this.FireExpandingLine(vector4, vector, 5);
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(0)));
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(1)));
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(2)));
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(3)));
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(4)));
		base.Fire(new ChancebulonDice1.ExpandingBullet(this, new Vector2(0f, 0f), new int?(5)));
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00016784 File Offset: 0x00014984
	private void FireExpandingLine(Vector2 start, Vector2 end, int numBullets)
	{
		for (int i = 0; i < numBullets; i++)
		{
			base.Fire(new ChancebulonDice1.ExpandingBullet(this, Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f)), null));
		}
	}

	// Token: 0x04000485 RID: 1157
	public const float Radius = 2f;

	// Token: 0x04000486 RID: 1158
	public const int GrowTime = 15;

	// Token: 0x04000487 RID: 1159
	public const float RotationSpeed = 180f;

	// Token: 0x04000488 RID: 1160
	public const float BulletSpeed = 10f;

	// Token: 0x0200013C RID: 316
	public class ExpandingBullet : Bullet
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x000167CC File Offset: 0x000149CC
		public ExpandingBullet(ChancebulonDice1 parent, Vector2 offset, int? numeralIndex = null)
			: base(null, false, false, false)
		{
			this.m_parent = parent;
			this.m_offset = offset;
			this.m_numeralIndex = numeralIndex;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000167F0 File Offset: 0x000149F0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			for (int i = 0; i < 15; i++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				Vector2 actualOffset = Vector2.Lerp(Vector2.zero, this.m_offset, (float)i / 14f);
				actualOffset = actualOffset.Rotate(3f * (float)i);
				base.Position = centerPosition + actualOffset;
				yield return base.Wait(1);
			}
			this.Direction = this.m_parent.aimDirection;
			this.Speed = 10f;
			for (int j = 0; j < 300; j++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				if (this.m_numeralIndex != null && j % 13 == 0 && j != 0)
				{
					this.m_currentNumeral = (this.m_currentNumeral + 1) % 6;
					switch (this.m_currentNumeral)
					{
					case 0:
						if (this.m_numeralIndex < 3)
						{
							this.m_offset = new Vector2(-0.7f, 0.7f);
						}
						else
						{
							this.m_offset = new Vector2(0.7f, -0.7f);
						}
						break;
					case 1:
						if (this.m_numeralIndex < 2)
						{
							this.m_offset = new Vector2(-0.7f, 0.7f);
						}
						else if (this.m_numeralIndex < 4)
						{
							this.m_offset = new Vector2(0f, 0f);
						}
						else
						{
							this.m_offset = new Vector2(0.7f, -0.7f);
						}
						break;
					case 2:
						if (this.m_numeralIndex < 1)
						{
							this.m_offset = new Vector2(-0.6f, -0.6f);
						}
						else if (this.m_numeralIndex < 2)
						{
							this.m_offset = new Vector2(-0.6f, 0.6f);
						}
						else if (this.m_numeralIndex < 3)
						{
							this.m_offset = new Vector2(0f, 0f);
						}
						else if (this.m_numeralIndex < 4)
						{
							this.m_offset = new Vector2(0.6f, -0.6f);
						}
						else
						{
							this.m_offset = new Vector2(0.6f, 0.6f);
						}
						break;
					case 3:
						if (this.m_numeralIndex < 2)
						{
							this.m_offset = new Vector2(-0.6f, -0.6f);
						}
						else if (this.m_numeralIndex < 3)
						{
							this.m_offset = new Vector2(-0.6f, 0.6f);
						}
						else if (this.m_numeralIndex < 4)
						{
							this.m_offset = new Vector2(0.6f, -0.6f);
						}
						else
						{
							this.m_offset = new Vector2(0.6f, 0.6f);
						}
						break;
					case 4:
						if (this.m_numeralIndex < 1)
						{
							this.m_offset = new Vector2(-0.6f, -0.6f);
						}
						else if (this.m_numeralIndex < 2)
						{
							this.m_offset = new Vector2(-0.6f, 0f);
						}
						else if (this.m_numeralIndex < 3)
						{
							this.m_offset = new Vector2(-0.6f, 0.6f);
						}
						else if (this.m_numeralIndex < 4)
						{
							this.m_offset = new Vector2(0.6f, -0.6f);
						}
						else if (this.m_numeralIndex < 5)
						{
							this.m_offset = new Vector2(0.6f, 0f);
						}
						else
						{
							this.m_offset = new Vector2(0.6f, 0.6f);
						}
						break;
					case 5:
						this.m_offset = new Vector2(0f, 0f);
						break;
					}
				}
				base.Position = centerPosition + this.m_offset.Rotate(3f * (float)(15 + j));
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400048A RID: 1162
		private const int SingleFaceShowTime = 13;

		// Token: 0x0400048B RID: 1163
		private ChancebulonDice1 m_parent;

		// Token: 0x0400048C RID: 1164
		private Vector2 m_offset;

		// Token: 0x0400048D RID: 1165
		private int? m_numeralIndex;

		// Token: 0x0400048E RID: 1166
		private int m_currentNumeral;
	}
}
