using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001076 RID: 4214
public class TankTreaderController : BraveBehaviour
{
	// Token: 0x06005CBB RID: 23739 RVA: 0x002382C4 File Offset: 0x002364C4
	public void Start()
	{
		this.m_miniTurrets = base.GetComponentsInChildren<TankTreaderMiniTurretController>();
		this.m_exhaustParticleSystems = base.GetComponentsInChildren<ParticleSystem>();
		base.aiActor.OverrideHitEnemies = true;
		tk2dSpriteAnimator tk2dSpriteAnimator = this.guy;
		tk2dSpriteAnimator.OnPlayAnimationCalled = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.OnPlayAnimationCalled, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnGuyAnimation));
		base.healthHaver.bodySprites.Add(this.hatch.sprite);
		base.healthHaver.bodySprites.Add(this.guy.sprite);
	}

	// Token: 0x06005CBC RID: 23740 RVA: 0x00238354 File Offset: 0x00236554
	public void Update()
	{
		if (base.aiActor.TargetRigidbody)
		{
			Vector2 unitCenter = base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			float num = Vector2.Distance(this.mainGun.transform.position, unitCenter);
			float num2 = (this.mainGun.transform.position.XY() - unitCenter).ToAngle();
			for (int i = 0; i < this.m_miniTurrets.Length; i++)
			{
				TankTreaderMiniTurretController tankTreaderMiniTurretController = this.m_miniTurrets[i];
				float num3 = Vector2.Distance(tankTreaderMiniTurretController.transform.position, unitCenter);
				if (num3 < num)
				{
					tankTreaderMiniTurretController.aimMode = TankTreaderMiniTurretController.AimMode.AtPlayer;
					tankTreaderMiniTurretController.OverrideAngle = null;
				}
				else
				{
					tankTreaderMiniTurretController.aimMode = TankTreaderMiniTurretController.AimMode.Away;
					float num4 = (tankTreaderMiniTurretController.transform.position.XY() - unitCenter).ToAngle();
					float num5 = (float)((BraveMathCollege.ClampAngle180(num4 - num2) >= 0f) ? (-1) : 1);
					tankTreaderMiniTurretController.OverrideAngle = new float?((unitCenter - tankTreaderMiniTurretController.transform.position.XY()).ToAngle() + num5 * this.backTurretOffset);
				}
			}
		}
		bool flag = true;
		if (base.aiActor.BehaviorVelocity != Vector2.zero)
		{
			float num6 = base.aiActor.BehaviorVelocity.ToAngle();
			float facingDirection = base.aiAnimator.FacingDirection;
			if (BraveMathCollege.AbsAngleBetween(num6, facingDirection) > 170f)
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (this.m_exhaustFrameCount++ < 5)
			{
				flag = false;
			}
		}
		else
		{
			this.m_exhaustFrameCount = 0;
		}
		for (int j = 0; j < this.m_exhaustParticleSystems.Length; j++)
		{
			BraveUtility.EnableEmission(this.m_exhaustParticleSystems[j], flag);
		}
	}

	// Token: 0x06005CBD RID: 23741 RVA: 0x0023854C File Offset: 0x0023674C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		AkSoundEngine.PostEvent("Stop_BOSS_tank_idle_01", base.gameObject);
	}

	// Token: 0x06005CBE RID: 23742 RVA: 0x00238568 File Offset: 0x00236768
	private void OnGuyAnimation(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (!this.m_hasPoppedHatch && (clip.name == "guy_in_gun" || clip.name.StartsWith("guy_fire")))
		{
			base.StartCoroutine(this.PopHatchCR());
			this.m_hasPoppedHatch = true;
		}
	}

	// Token: 0x06005CBF RID: 23743 RVA: 0x002385C0 File Offset: 0x002367C0
	private IEnumerator PopHatchCR()
	{
		this.hatch.Play("hatch_open");
		this.hatchPopVfx.SpawnAtLocalPosition(Vector3.zero, 0f, this.hatch.transform, null, null, false, null, false);
		tk2dSprite flyingHatch = UnityEngine.Object.Instantiate<GameObject>(this.hatchPopObject, this.hatch.transform.position, Quaternion.identity).GetComponent<tk2dSprite>();
		flyingHatch.HeightOffGround = 7f;
		float elapsed = 0f;
		float duration = this.hatchFlyTime;
		while (elapsed < duration)
		{
			flyingHatch.transform.position += this.hatchFlySpeed * BraveTime.DeltaTime;
			flyingHatch.transform.localScale = Vector3.one * (1f - elapsed / duration);
			flyingHatch.UpdateZDepth();
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		UnityEngine.Object.Destroy(flyingHatch);
		yield break;
	}

	// Token: 0x0400565E RID: 22110
	public GameObject mainGun;

	// Token: 0x0400565F RID: 22111
	public float backTurretOffset = 30f;

	// Token: 0x04005660 RID: 22112
	public tk2dSpriteAnimator guy;

	// Token: 0x04005661 RID: 22113
	public tk2dSpriteAnimator hatch;

	// Token: 0x04005662 RID: 22114
	public GameObject hatchPopObject;

	// Token: 0x04005663 RID: 22115
	public VFXPool hatchPopVfx;

	// Token: 0x04005664 RID: 22116
	public float hatchFlyTime = 1f;

	// Token: 0x04005665 RID: 22117
	public Vector2 hatchFlySpeed = new Vector3(8f, 20f);

	// Token: 0x04005666 RID: 22118
	private TankTreaderMiniTurretController[] m_miniTurrets;

	// Token: 0x04005667 RID: 22119
	private ParticleSystem[] m_exhaustParticleSystems;

	// Token: 0x04005668 RID: 22120
	private int m_exhaustFrameCount;

	// Token: 0x04005669 RID: 22121
	private bool m_hasPoppedHatch;
}
