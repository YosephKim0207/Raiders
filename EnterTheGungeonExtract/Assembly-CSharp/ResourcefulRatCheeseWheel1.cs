using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020002C7 RID: 711
[InspectorDropdownName("Bosses/ResourcefulRat/CheeseWheel1")]
public class ResourcefulRatCheeseWheel1 : Script
{
	// Token: 0x06000AEB RID: 2795 RVA: 0x000343CC File Offset: 0x000325CC
	protected override IEnumerator Top()
	{
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 roomLowerLeft = area.UnitBottomLeft;
		Vector2 roomUpperRight = area.UnitTopRight - new Vector2(0f, 3f);
		Vector2 roomCenter = area.UnitCenter - new Vector2(0f, 2.5f);
		base.PostWwiseEvent("Play_BOSS_Rat_Cheese_Summon_01", null);
		for (int i = 0; i < 3; i++)
		{
			int misfireIndex = UnityEngine.Random.Range(0, 15);
			for (int j = 0; j < 20; j++)
			{
				Vector2 vector = new Vector2(roomLowerLeft.x, base.SubdivideRange(roomLowerLeft.y, roomUpperRight.y, 21, j, true));
				vector += new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f));
				vector.x -= 1.25f;
				bool flag = j >= misfireIndex && j < misfireIndex + 5;
				this.FireWallBullet(0f, vector, roomCenter, flag);
			}
			misfireIndex = UnityEngine.Random.Range(0, 15);
			for (int k = 0; k < 20; k++)
			{
				Vector2 vector2 = new Vector2(base.SubdivideRange(roomLowerLeft.x, roomUpperRight.x, 21, k, true), roomUpperRight.y);
				vector2 += new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f));
				vector2.y += 3.25f;
				bool flag2 = k >= misfireIndex && k < misfireIndex + 5;
				this.FireWallBullet(-90f, vector2, roomCenter, flag2);
			}
			misfireIndex = UnityEngine.Random.Range(0, 15);
			for (int l = 0; l < 20; l++)
			{
				Vector2 vector3 = new Vector2(roomUpperRight.x, base.SubdivideRange(roomLowerLeft.y, roomUpperRight.y, 21, l, true));
				vector3 += new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f));
				vector3.x += 1.25f;
				bool flag3 = l >= misfireIndex && l < misfireIndex + 5;
				this.FireWallBullet(180f, vector3, roomCenter, flag3);
			}
			misfireIndex = UnityEngine.Random.Range(0, 15);
			for (int m = 0; m < 20; m++)
			{
				Vector2 vector4 = new Vector2(base.SubdivideRange(roomLowerLeft.x, roomUpperRight.x, 21, m, true), roomLowerLeft.y);
				vector4 += new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f));
				vector4.y -= 1.25f;
				bool flag4 = m >= misfireIndex && m < misfireIndex + 5;
				this.FireWallBullet(90f, vector4, roomCenter, flag4);
			}
			if (i == 2)
			{
				base.EndOnBlank = true;
			}
			yield return base.Wait(75);
		}
		yield return base.Wait(125);
		AIActor aiActor = base.BulletBank.aiActor;
		aiActor.aiAnimator.PlayUntilFinished("cheese_wheel_out", false, null, -1f, false);
		aiActor.IsGone = true;
		aiActor.specRigidbody.CollideWithOthers = false;
		base.Fire(Offset.OverridePosition(roomCenter), new Speed(0f, SpeedType.Absolute), new ResourcefulRatCheeseWheel1.CheeseWheelBullet());
		yield return base.Wait(65);
		aiActor.IsGone = false;
		aiActor.specRigidbody.CollideWithOthers = true;
		yield return base.Wait(105);
		yield break;
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000343E8 File Offset: 0x000325E8
	public override void OnForceEnded()
	{
		AIActor aiActor = base.BulletBank.aiActor;
		aiActor.IsGone = false;
		aiActor.specRigidbody.CollideWithOthers = true;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00034414 File Offset: 0x00032614
	private void FireWallBullet(float facingDir, Vector2 spawnPos, Vector2 roomCenter, bool isMisfire)
	{
		float num = (spawnPos - roomCenter).ToAngle();
		int num2 = Mathf.RoundToInt(BraveMathCollege.ClampAngle360(num) / 45f) % 8;
		float num3 = (float)num2 * 45f;
		Vector2 vector = (roomCenter + BraveMathCollege.DegreesToVector(num3, 0.875f) + ResourcefulRatCheeseWheel1.TargetOffsets[num2]).Quantize(0.0625f);
		base.Fire(Offset.OverridePosition(spawnPos), new Direction(facingDir, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new ResourcefulRatCheeseWheel1.CheeseWedgeBullet(this, ResourcefulRatCheeseWheel1.TargetNames[num2], ResourcefulRatCheeseWheel1.RampHeights[num2], vector, num3 + 180f, isMisfire));
	}

	// Token: 0x04000B72 RID: 2930
	private const float WallInset = 1.25f;

	// Token: 0x04000B73 RID: 2931
	private const int WallInsetTime = 40;

	// Token: 0x04000B74 RID: 2932
	private const int WallInsetTimeVariation = 20;

	// Token: 0x04000B75 RID: 2933
	private const int NumVerticalWallBullets = 20;

	// Token: 0x04000B76 RID: 2934
	private const int NumHorizontalWallBullets = 20;

	// Token: 0x04000B77 RID: 2935
	private const int MisfireBullets = 5;

	// Token: 0x04000B78 RID: 2936
	private const int TargetPoints = 8;

	// Token: 0x04000B79 RID: 2937
	private const float TargetAngleDelta = 45f;

	// Token: 0x04000B7A RID: 2938
	private const float TargetOffset = 0.875f;

	// Token: 0x04000B7B RID: 2939
	private const int MinTravelTime = 90;

	// Token: 0x04000B7C RID: 2940
	private const int MaxTravelTime = 135;

	// Token: 0x04000B7D RID: 2941
	private const int WaveDelay = 75;

	// Token: 0x04000B7E RID: 2942
	private const int NumWaves = 3;

	// Token: 0x04000B7F RID: 2943
	private static string[] TargetNames = new string[] { "cheeseWedge0", "cheeseWedge1", "cheeseWedge2", "cheeseWedge3", "cheeseWedge4", "cheeseWedge5", "cheeseWedge6", "cheeseWedge7" };

	// Token: 0x04000B80 RID: 2944
	private static float[] RampHeights = new float[] { 2f, 1f, 0f, 1f, 2f, 3f, 4f, 2f };

	// Token: 0x04000B81 RID: 2945
	private static Vector2[] TargetOffsets = new Vector2[]
	{
		new Vector2(0f, 0.0625f),
		new Vector2(0.0625f, -0.0625f),
		new Vector2(0.0625f, 0f),
		new Vector2(0.0625f, -0.0625f),
		new Vector2(0.0625f, 0.0625f),
		new Vector2(0f, 0f),
		new Vector2(0.0625f, 0f),
		new Vector2(0.125f, -0.125f)
	};

	// Token: 0x020002C8 RID: 712
	public class CheeseWedgeBullet : Bullet
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x00034614 File Offset: 0x00032814
		public CheeseWedgeBullet(ResourcefulRatCheeseWheel1 parent, string bulletName, float additionalRampHeight, Vector2 targetPos, float endingAngle, bool isMisfire)
			: base(bulletName, true, false, false)
		{
			this.m_parent = parent;
			this.m_targetPos = targetPos;
			this.m_endingAngle = endingAngle;
			this.m_isMisfire = isMisfire;
			this.m_additionalRampHeight = additionalRampHeight;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00034648 File Offset: 0x00032848
		protected override IEnumerator Top()
		{
			int travelTime = UnityEngine.Random.RandomRange(90, 136);
			this.Projectile.IgnoreTileCollisionsFor(90f);
			this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle));
			this.Projectile.sprite.HeightOffGround = 10f + this.m_additionalRampHeight + UnityEngine.Random.value / 2f;
			this.Projectile.sprite.ForceRotationRebuild();
			this.Projectile.sprite.UpdateZDepth();
			int r = UnityEngine.Random.Range(0, 20);
			yield return base.Wait(15 + r);
			this.Speed = 2.5f;
			yield return base.Wait(50 - r);
			this.Speed = 0f;
			if (this.m_isMisfire)
			{
				this.Direction += 180f;
				this.Speed = 2.5f;
				yield return base.Wait(180);
				base.Vanish(true);
				yield break;
			}
			this.Direction = (this.m_targetPos - base.Position).ToAngle();
			base.ChangeSpeed(new Speed((this.m_targetPos - base.Position).magnitude / ((float)(travelTime - 15) / 60f), SpeedType.Absolute), 30);
			yield return base.Wait(travelTime);
			this.Speed = 0f;
			base.Position = this.m_targetPos;
			this.Direction = this.m_endingAngle;
			if (this.Projectile && this.Projectile.sprite)
			{
				this.Projectile.sprite.HeightOffGround -= 1f;
				this.Projectile.sprite.UpdateZDepth();
			}
			int totalTime = 350;
			yield return base.Wait(totalTime - this.m_parent.Tick);
			base.Vanish(true);
			yield break;
		}

		// Token: 0x04000B82 RID: 2946
		private ResourcefulRatCheeseWheel1 m_parent;

		// Token: 0x04000B83 RID: 2947
		private Vector2 m_targetPos;

		// Token: 0x04000B84 RID: 2948
		private float m_endingAngle;

		// Token: 0x04000B85 RID: 2949
		private bool m_isMisfire;

		// Token: 0x04000B86 RID: 2950
		private float m_additionalRampHeight;
	}

	// Token: 0x020002CA RID: 714
	public class CheeseWheelBullet : Bullet
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x00034A24 File Offset: 0x00032C24
		public CheeseWheelBullet()
			: base("cheeseWheel", true, false, false)
		{
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00034A34 File Offset: 0x00032C34
		protected override IEnumerator Top()
		{
			this.Projectile.spriteAnimator.Play("cheese_wheel_burst");
			this.Projectile.ImmuneToSustainedBlanks = true;
			yield return base.Wait(45);
			this.Projectile.Ramp(-1.5f, 100f);
			yield return base.Wait(80);
			for (int i = 0; i < 80; i++)
			{
				Bullet bullet = new Bullet("cheese", true, false, false);
				base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(12f, 33f), SpeedType.Absolute), bullet);
				bullet.Projectile.ImmuneToSustainedBlanks = true;
			}
			if (base.BulletBank)
			{
				ResourcefulRatController component = base.BulletBank.GetComponent<ResourcefulRatController>();
				if (component)
				{
					GameManager.Instance.MainCameraController.DoScreenShake(component.cheeseSlamScreenShake, null, false);
				}
			}
			yield return base.Wait(25);
			base.Vanish(true);
			yield break;
		}
	}
}
