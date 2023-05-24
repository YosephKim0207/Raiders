using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x020001BB RID: 443
[InspectorDropdownName("GunNut/ChainSpin1")]
public class GunNutSpin1 : Script
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001FC70 File Offset: 0x0001DE70
	// (set) Token: 0x06000696 RID: 1686 RVA: 0x0001FC78 File Offset: 0x0001DE78
	public bool IsTellingBolas { get; set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001FC84 File Offset: 0x0001DE84
	// (set) Token: 0x06000698 RID: 1688 RVA: 0x0001FC8C File Offset: 0x0001DE8C
	public bool ShouldThrowBolas { get; set; }

	// Token: 0x06000699 RID: 1689 RVA: 0x0001FC98 File Offset: 0x0001DE98
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		base.BulletBank.aiAnimator.ChildAnimator.OverrideIdleAnimation = "twirl";
		base.BulletBank.aiAnimator.ChildAnimator.OverrideMoveAnimation = "twirl_move";
		base.BulletBank.aiAnimator.ChildAnimator.renderer.enabled = false;
		float turnSign = (float)((BraveMathCollege.AbsAngleBetween(base.BulletBank.aiAnimator.FacingDirection, 0f) <= 90f) ? (-1) : 1);
		this.TurnSpeed = 540f * turnSign;
		this.bullets = new List<GunNutSpin1.SpinBullet>(9);
		for (int i = 0; i < 9; i++)
		{
			float num = ((float)i + 0.5f) / 8.5f;
			int num2 = Mathf.CeilToInt(Mathf.Lerp((float)(GunNutSpin1.Transforms.Length - 1), 0f, num));
			GunNutSpin1.SpinBullet spinBullet = new GunNutSpin1.SpinBullet(this, num * 6f, i == 8);
			base.Fire(new Offset(GunNutSpin1.Transforms[num2]), new Speed(0f, SpeedType.Absolute), spinBullet);
			this.bullets.Add(spinBullet);
		}
		this.TicksRemaining = 192;
		int respawnCooldown = 0;
		while (this.TicksRemaining > 0)
		{
			if (base.Tick == 30 && this.CanThrowBolas())
			{
				this.StartBolasTell();
			}
			if (base.Tick == 120 && this.IsTellingBolas)
			{
				this.ShouldThrowBolas = true;
			}
			respawnCooldown--;
			for (int j = 0; j < this.bullets.Count; j++)
			{
				GunNutSpin1.SpinBullet spinBullet2 = this.bullets[j];
				if ((spinBullet2.Destroyed || (spinBullet2.Projectile && !spinBullet2.Projectile.isActiveAndEnabled)) && respawnCooldown <= 0)
				{
					float num3 = ((float)j + 1f) / 9f;
					float num4 = 90f + this.TurnSpeed / 60f * (float)(base.Tick + 1 + 3);
					float num5;
					if (this.TicksRemaining < 60)
					{
						num5 = Mathf.Lerp(0f, num3 * 6f, (float)this.TicksRemaining / 45f);
					}
					else
					{
						num5 = Mathf.Lerp(0f, num3 * 6f, (float)base.Tick / 30f);
					}
					Vector2 vector = base.Position + BraveMathCollege.DegreesToVector(num4, num5);
					GunNutSpin1.SpinBullet spinBullet3 = new GunNutSpin1.SpinBullet(this, num3 * 6f, j == 8);
					base.Fire(Offset.OverridePosition(vector), new Speed(0f, SpeedType.Absolute), spinBullet3);
					this.bullets[j] = spinBullet3;
					respawnCooldown = 4;
					break;
				}
			}
			yield return base.Wait(1);
			this.TicksRemaining--;
		}
		for (int k = 0; k < this.bullets.Count; k++)
		{
			if (this.bullets[k] != null)
			{
				this.bullets[k].Vanish(k < 8);
			}
		}
		this.bullets = null;
		base.BulletBank.aiAnimator.ChildAnimator.OverrideIdleAnimation = null;
		base.BulletBank.aiAnimator.ChildAnimator.OverrideMoveAnimation = null;
		base.BulletBank.aiAnimator.ChildAnimator.renderer.enabled = false;
		yield break;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0001FCB4 File Offset: 0x0001DEB4
	public override void OnForceEnded()
	{
		base.OnForceEnded();
		this.bullets = null;
		base.BulletBank.aiAnimator.ChildAnimator.OverrideIdleAnimation = null;
		base.BulletBank.aiAnimator.ChildAnimator.OverrideMoveAnimation = null;
		base.BulletBank.aiAnimator.ChildAnimator.renderer.enabled = false;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001FD18 File Offset: 0x0001DF18
	private bool CanThrowBolas()
	{
		return (!GameManager.HasInstance || GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.CHARACTER_PAST || GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Bullet) && (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_CATACOMBS) > 0f || GameStatsManager.Instance.QueryEncounterable(base.BulletBank.encounterTrackable) >= 15) && (base.BulletBank && base.BulletBank.aiActor && base.BulletBank.aiActor.ParentRoom != null) && base.BulletBank.aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) < 2;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
	public void StartBolasTell()
	{
		this.IsTellingBolas = true;
		base.PostWwiseEvent("Play_ENM_CannonArmor_Charge_01", null);
		for (int i = 0; i < this.bullets.Count; i++)
		{
			GunNutSpin1.SpinBullet spinBullet = this.bullets[i];
			if (spinBullet != null && spinBullet.Projectile)
			{
				spinBullet.Projectile.spriteAnimator.Play();
			}
		}
		base.BulletBank.aiAnimator.ChildAnimator.renderer.enabled = true;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0001FE6C File Offset: 0x0001E06C
	public void WasThrown()
	{
		this.IsTellingBolas = false;
		base.PostWwiseEvent("Play_OBJ_Chainpot_Drop_01", null);
		for (int i = this.bullets.Count - 1; i > 2; i--)
		{
			this.bullets[i].Vanish(true);
			this.bullets.RemoveAt(i);
		}
		for (int j = 0; j < this.bullets.Count; j++)
		{
			GunNutSpin1.SpinBullet spinBullet = this.bullets[j];
			if (spinBullet != null && spinBullet.Projectile)
			{
				spinBullet.Projectile.spriteAnimator.StopAndResetFrameToDefault();
			}
		}
		base.BulletBank.aiAnimator.ChildAnimator.renderer.enabled = false;
	}

	// Token: 0x0400066A RID: 1642
	public static string[] Transforms = new string[] { "bullet hand", "bullet limb 1", "bullet limb 2", "bullet limb 3" };

	// Token: 0x0400066B RID: 1643
	public const int NumBullets = 9;

	// Token: 0x0400066C RID: 1644
	public const int BaseTurnSpeed = 540;

	// Token: 0x0400066D RID: 1645
	public const float MaxDist = 6f;

	// Token: 0x0400066E RID: 1646
	public const int ExtendTime = 30;

	// Token: 0x0400066F RID: 1647
	public const int Lifetime = 120;

	// Token: 0x04000670 RID: 1648
	public const int ContractTime = 45;

	// Token: 0x04000671 RID: 1649
	public const int TellTime = 30;

	// Token: 0x04000672 RID: 1650
	public const int BolasThrowTime = 120;

	// Token: 0x04000673 RID: 1651
	public float TurnSpeed;

	// Token: 0x04000674 RID: 1652
	public int TicksRemaining;

	// Token: 0x04000677 RID: 1655
	private List<GunNutSpin1.SpinBullet> bullets;

	// Token: 0x020001BC RID: 444
	private class SpinBullet : Bullet
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x0001FF64 File Offset: 0x0001E164
		public SpinBullet(GunNutSpin1 parentScript, float maxDist, bool isBall)
			: base((!isBall) ? "link" : "ball", false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_maxDist = maxDist;
			this.m_isBall = isBall;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001FF9C File Offset: 0x0001E19C
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			float startDist = Vector2.Distance(base.Position, this.m_parentScript.Position);
			while (!this.m_parentScript.Destroyed && !this.m_parentScript.IsEnded)
			{
				if (this.m_parentScript.BulletBank.healthHaver.IsDead)
				{
					base.Vanish(false);
					yield break;
				}
				float angle = 90f + this.m_parentScript.TurnSpeed / 60f * (float)(this.m_parentScript.Tick + 3);
				if (this.m_isBall && this.m_parentScript.ShouldThrowBolas)
				{
					float num = BraveMathCollege.ClampAngle180(base.AimDirection - (angle - 90f));
					if (num >= 0f && num < 45f && !base.IsPointInTile(base.Position))
					{
						float aimDirection = base.AimDirection;
						base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new GunNutSpin1.BolasBullet(true, -2f));
						base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new GunNutSpin1.BolasBullet(false, -1f));
						base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new GunNutSpin1.BolasBullet(false, 0f));
						base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new GunNutSpin1.BolasBullet(false, 1f));
						base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new GunNutSpin1.BolasBullet(true, 2f));
						this.m_parentScript.WasThrown();
						yield break;
					}
				}
				float dist;
				if (this.m_parentScript.TicksRemaining < 60)
				{
					dist = Mathf.Lerp(0f, this.m_maxDist, (float)this.m_parentScript.TicksRemaining / 45f);
				}
				else
				{
					dist = Mathf.Lerp(startDist, this.m_maxDist, (float)this.m_parentScript.Tick / 30f);
				}
				base.Position = this.m_parentScript.Position + BraveMathCollege.DegreesToVector(angle, dist);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000678 RID: 1656
		private GunNutSpin1 m_parentScript;

		// Token: 0x04000679 RID: 1657
		private float m_maxDist;

		// Token: 0x0400067A RID: 1658
		private bool m_isBall;
	}

	// Token: 0x020001BE RID: 446
	public class BolasBullet : Bullet
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x00020374 File Offset: 0x0001E574
		public BolasBullet(bool isBall, float offset)
			: base((!isBall) ? "link" : "ball_trail", false, false, false)
		{
			this.m_offset = offset;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002039C File Offset: 0x0001E59C
		protected override IEnumerator Top()
		{
			Vector2 truePosition = base.Position;
			base.ManualControl = true;
			this.Projectile.IgnoreTileCollisionsFor(0.2f);
			for (;;)
			{
				base.UpdateVelocity();
				truePosition += this.Velocity / 60f;
				Vector2 offset = new Vector2(this.m_offset * Mathf.Lerp(0f, 1f, (float)base.Tick / 60f), 0f);
				offset = offset.Rotate((float)base.Tick / 60f * -360f);
				base.Position = truePosition + offset;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000682 RID: 1666
		public const float ExpandTime = 60f;

		// Token: 0x04000683 RID: 1667
		public const float RotationTime = 60f;

		// Token: 0x04000684 RID: 1668
		private float m_offset;
	}
}
