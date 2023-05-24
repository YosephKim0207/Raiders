using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class WizardYellowSlam1 : Script
{
	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0003CD90 File Offset: 0x0003AF90
	// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x0003CD98 File Offset: 0x0003AF98
	public float aimDirection { get; private set; }

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			this.FireX();
			break;
		case 1:
			this.FireSquare();
			break;
		case 2:
			this.FireTriangle();
			break;
		case 3:
			this.FireCircle();
			break;
		}
		this.aimDirection = base.AimDirection;
		yield return base.Wait(15);
		float distanceToTarget = (this.BulletManager.PlayerPosition() - base.Position).magnitude;
		if (distanceToTarget > 4.5f)
		{
			this.aimDirection = base.GetAimDirection(1f, 10f);
		}
		yield break;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0003CDC0 File Offset: 0x0003AFC0
	private void FireX()
	{
		Vector2 vector = new Vector2(2f, 0f).Rotate(45f);
		Vector2 vector2 = new Vector2(2f, 0f).Rotate(135f);
		Vector2 vector3 = new Vector2(2f, 0f).Rotate(225f);
		Vector2 vector4 = new Vector2(2f, 0f).Rotate(-45f);
		this.FireExpandingLine(vector, vector3, 11);
		this.FireExpandingLine(vector2, vector4, 11);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0003CE4C File Offset: 0x0003B04C
	private void FireSquare()
	{
		Vector2 vector = new Vector2(2f, 0f).Rotate(45f);
		Vector2 vector2 = new Vector2(2f, 0f).Rotate(135f);
		Vector2 vector3 = new Vector2(2f, 0f).Rotate(225f);
		Vector2 vector4 = new Vector2(2f, 0f).Rotate(-45f);
		this.FireExpandingLine(vector, vector2, 9);
		this.FireExpandingLine(vector2, vector3, 9);
		this.FireExpandingLine(vector3, vector4, 9);
		this.FireExpandingLine(vector4, vector, 9);
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0003CEEC File Offset: 0x0003B0EC
	private void FireTriangle()
	{
		Vector2 vector = new Vector2(2f, 0f).Rotate(90f);
		Vector2 vector2 = new Vector2(2f, 0f).Rotate(210f);
		Vector2 vector3 = new Vector2(2f, 0f).Rotate(330f);
		this.FireExpandingLine(vector, vector2, 10);
		this.FireExpandingLine(vector2, vector3, 10);
		this.FireExpandingLine(vector3, vector, 10);
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0003CF68 File Offset: 0x0003B168
	private void FireCircle()
	{
		for (int i = 0; i < 36; i++)
		{
			base.Fire(new WizardYellowSlam1.ExpandingBullet(this, new Vector2(2f, 0f).Rotate((float)i / 35f * 360f)));
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0003CFB8 File Offset: 0x0003B1B8
	private void FireExpandingLine(Vector2 start, Vector2 end, int numBullets)
	{
		for (int i = 0; i < numBullets; i++)
		{
			base.Fire(new WizardYellowSlam1.ExpandingBullet(this, Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f))));
		}
	}

	// Token: 0x04000D4F RID: 3407
	public const float Radius = 2f;

	// Token: 0x04000D50 RID: 3408
	public const int GrowTime = 15;

	// Token: 0x04000D51 RID: 3409
	public const float RotationSpeed = 180f;

	// Token: 0x04000D52 RID: 3410
	public const float BulletSpeed = 10f;

	// Token: 0x02000332 RID: 818
	public class ExpandingBullet : Bullet
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x0003CFF8 File Offset: 0x0003B1F8
		public ExpandingBullet(WizardYellowSlam1 parent, Vector2 offset)
			: base(null, false, false, false)
		{
			this.m_parent = parent;
			this.m_offset = offset;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003D014 File Offset: 0x0003B214
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
				base.Position = centerPosition + this.m_offset.Rotate(3f * (float)(15 + j));
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000D54 RID: 3412
		private WizardYellowSlam1 m_parent;

		// Token: 0x04000D55 RID: 3413
		private Vector2 m_offset;
	}
}
