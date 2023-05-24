using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002D7 RID: 727
[InspectorDropdownName("Bosses/ResourcefulRat/Tail1")]
public class ResourcefulRatTail1 : Script
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0003677C File Offset: 0x0003497C
	// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00036784 File Offset: 0x00034984
	public bool ShouldTell { get; set; }

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00036790 File Offset: 0x00034990
	// (set) Token: 0x06000B38 RID: 2872 RVA: 0x00036798 File Offset: 0x00034998
	public bool Done { get; set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x000367A4 File Offset: 0x000349A4
	// (set) Token: 0x06000B3A RID: 2874 RVA: 0x000367AC File Offset: 0x000349AC
	public float FireAngle { get; set; }

	// Token: 0x06000B3B RID: 2875 RVA: 0x000367B8 File Offset: 0x000349B8
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		ResourcefulRatTail1.TailBullet[] bullets = new ResourcefulRatTail1.TailBullet[33];
		yield return base.Wait(10);
		for (int i = 0; i < 33; i++)
		{
			Vector2 pos = this.GetPosition(i, base.Tick + 1);
			ResourcefulRatTail1.TailBullet bullet = new ResourcefulRatTail1.TailBullet(this, i);
			base.Fire(Offset.OverridePosition(pos), bullet);
			bullets[i] = bullet;
			if (i % 2 == 0)
			{
				yield return base.Wait(3);
			}
		}
		int spinTime = 167;
		float currentAngle = 0f;
		for (int j = 0; j < spinTime + 60; j++)
		{
			currentAngle = (this.GetPosition(16, base.Tick) - base.Position).ToAngle();
			if (j > spinTime && BraveMathCollege.AbsAngleBetween(currentAngle, base.AimDirection + 45f) < 10f)
			{
				break;
			}
			if (j == spinTime - 1)
			{
				this.ShouldTell = true;
			}
			yield return base.Wait(1);
		}
		this.Done = true;
		this.FireAngle = currentAngle - 90f;
		yield break;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x000367D4 File Offset: 0x000349D4
	public Vector2 GetPosition(int index, int tick)
	{
		float num;
		if (base.Tick <= 120)
		{
			num = -90f + -90f * ((float)tick / 60f) * ((float)tick / 60f);
		}
		else
		{
			num = -450f + (float)(tick - 120) / 60f * -360f;
		}
		float num2 = BraveMathCollege.AbsAngleBetween(num, -90f);
		float num3 = Mathf.Lerp(0.5f, 0.75f, num2 / 70f);
		float num4 = 1f + (float)index * num3;
		float num5 = (float)index * Mathf.Lerp(0f, 3f, num2 / 120f);
		return BraveMathCollege.DegreesToVector(num + num5, num4) + base.Position;
	}

	// Token: 0x04000BE6 RID: 3046
	public const int NumBullets = 33;

	// Token: 0x04000BE7 RID: 3047
	public const int SpawnDelay = 3;

	// Token: 0x04000BE8 RID: 3048
	public const float RotationSpeed = -360f;

	// Token: 0x04000BE9 RID: 3049
	public const int RotationTime = 266;

	// Token: 0x04000BEA RID: 3050
	public const int FlashTime = 45;

	// Token: 0x020002D8 RID: 728
	public class TailBullet : Bullet
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x00036888 File Offset: 0x00034A88
		public TailBullet(ResourcefulRatTail1 parentScript, int index)
			: base("tail", true, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_index = index;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000368B0 File Offset: 0x00034AB0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			bool hasTold = false;
			while (!this.m_parentScript.Destroyed && !this.m_parentScript.IsEnded && !this.m_parentScript.Done)
			{
				base.Position = this.m_parentScript.GetPosition(this.m_index, this.m_parentScript.Tick);
				if (this.m_parentScript.ShouldTell)
				{
					if (!hasTold)
					{
						this.Projectile.sprite.spriteAnimator.Play();
					}
				}
				else
				{
					this.m_spawnCountdown--;
					if (this.m_spawnCountdown == 0)
					{
						base.Fire(new ResourcefulRatTail1.SubtailBullet(this.m_parentScript));
						this.Projectile.sprite.spriteAnimator.StopAndResetFrameToDefault();
						this.m_spawnCountdown = -1;
					}
				}
				yield return base.Wait(1);
			}
			this.Speed = 20f + UnityEngine.Random.Range(-2f, 2f);
			this.Direction = this.m_parentScript.FireAngle + UnityEngine.Random.Range(-15f, 15f);
			this.Projectile.sprite.spriteAnimator.StopAndResetFrameToDefault();
			AkSoundEngine.PostEvent("Play_BOSS_Rat_Tail_Whip_01", GameManager.Instance.gameObject);
			base.ManualControl = false;
			this.Projectile.specRigidbody.CollideWithTileMap = true;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = false;
			yield break;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x000368CC File Offset: 0x00034ACC
		public void SpawnBullet()
		{
			if (this.m_spawnCountdown >= 0 || !this.Projectile)
			{
				Debug.Log("skipped");
				return;
			}
			Debug.Log(this.Projectile);
			Debug.Log(this.Projectile.sprite);
			Debug.Log(this.Projectile.sprite.spriteAnimator);
			this.m_spawnCountdown = 45;
			this.Projectile.sprite.spriteAnimator.Play();
			Debug.LogWarning("STARTING SOME SHIT");
		}

		// Token: 0x04000BEE RID: 3054
		private ResourcefulRatTail1 m_parentScript;

		// Token: 0x04000BEF RID: 3055
		private int m_index;

		// Token: 0x04000BF0 RID: 3056
		private int m_spawnCountdown = -1;
	}

	// Token: 0x020002DA RID: 730
	public class SubtailBullet : Bullet
	{
		// Token: 0x06000B46 RID: 2886 RVA: 0x00036BFC File Offset: 0x00034DFC
		public SubtailBullet(ResourcefulRatTail1 parentScript)
			: base(null, true, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00036C10 File Offset: 0x00034E10
		protected override IEnumerator Top()
		{
			while (!this.m_parentScript.Destroyed && !this.m_parentScript.IsEnded && !this.m_parentScript.Done)
			{
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000BF6 RID: 3062
		private ResourcefulRatTail1 m_parentScript;
	}
}
