using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FF4 RID: 4084
public class BossStatueDeathController : BraveBehaviour
{
	// Token: 0x06005936 RID: 22838 RVA: 0x00220E54 File Offset: 0x0021F054
	public void Start()
	{
		this.m_statueController = base.GetComponent<BossStatueController>();
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x00220E88 File Offset: 0x0021F088
	protected override void OnDestroy()
	{
		if (this)
		{
			UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
		base.OnDestroy();
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x00220EB0 File Offset: 0x0021F0B0
	private void OnBossDeath(Vector2 dir)
	{
		base.StartCoroutine(this.OnDeathAnimationCR());
	}

	// Token: 0x06005939 RID: 22841 RVA: 0x00220EC0 File Offset: 0x0021F0C0
	private IEnumerator OnDeathAnimationCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		while (!this.m_statueController.IsGrounded)
		{
			this.m_statueController.State = BossStatueController.StatueState.StandStill;
			yield return null;
		}
		string deathAnim = this.m_statueController.CurrentLevel.deathAnim;
		base.spriteAnimator.Play(deathAnim);
		tk2dSpriteAnimationClip deathClip = base.spriteAnimator.CurrentClip;
		float explosionsDelay = Mathf.Max(0f, (float)(deathClip.frames.Length - 1) / deathClip.fps - this.explosionMidDelay * (float)this.explosionCount);
		yield return new WaitForSeconds(explosionsDelay);
		base.StartCoroutine(this.DeathFlashCR());
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			float yStep = (maxPos.y - minPos.y) / (float)this.explosionCount;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos.WithY(minPos.y + yStep * (float)i), maxPos.WithY(minPos.y + yStep * (float)(i + 1)), new Vector2(0.4f, 0f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		if (base.spriteAnimator.IsPlaying(deathAnim))
		{
			while (base.spriteAnimator.IsPlaying(deathAnim) && base.spriteAnimator.CurrentFrame < deathClip.frames.Length - 1)
			{
				yield return null;
			}
		}
		if (this.bigExplosionVfx)
		{
			Vector2 vector = base.specRigidbody.HitboxPixelCollider.UnitCenter;
			if (this.bigExplosionTransform)
			{
				vector = this.bigExplosionTransform.position;
			}
			GameObject gameObject = SpawnManager.SpawnVFX(this.bigExplosionVfx, vector, Quaternion.identity);
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			component.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(component);
			base.sprite.UpdateZDepth();
		}
		Vector2 unitBottomLeft = collider.UnitBottomLeft;
		Vector2 unitCenter = collider.UnitCenter;
		Vector2 unitTopRight = collider.UnitTopRight;
		for (int j = 0; j < this.debrisCount; j++)
		{
			GameObject gameObject2 = BraveUtility.RandomElement<GameObject>(this.debrisObjects);
			Vector2 vector2 = BraveUtility.RandomVector2(unitBottomLeft, unitTopRight, new Vector2(-1.5f, -1.5f));
			GameObject gameObject3 = SpawnManager.SpawnVFX(gameObject2, vector2, Quaternion.identity);
			if (gameObject3)
			{
				gameObject3.transform.parent = SpawnManager.Instance.VFX;
				DebrisObject orAddComponent = gameObject3.GetOrAddComponent<DebrisObject>();
				if (base.aiActor)
				{
					base.aiActor.sprite.AttachRenderer(orAddComponent.sprite);
				}
				orAddComponent.angularVelocity = Mathf.Sign(UnityEngine.Random.value - 0.5f) * 125f;
				orAddComponent.angularVelocityVariance = 60f;
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				float num = (vector2 - unitCenter).ToAngle() + UnityEngine.Random.Range(-this.debrisAngleVariance, this.debrisAngleVariance);
				Vector2 vector3 = BraveMathCollege.DegreesToVector(num, 1f) * (float)UnityEngine.Random.Range(this.debrisMinForce, this.debrisMaxForce);
				Vector3 vector4 = new Vector3(vector3.x, (vector3.y >= 0f) ? 0f : vector3.y, (vector3.y <= 0f) ? 0f : vector3.y);
				if (orAddComponent.minorBreakable)
				{
					orAddComponent.minorBreakable.enabled = true;
				}
				orAddComponent.Trigger(vector4, 1f, 1f);
			}
		}
		base.sprite.renderer.enabled = false;
		this.m_statueController.shadowSprite.renderer.enabled = false;
		if (this.m_statueController.IsKali)
		{
			if (GameStatsManager.Instance.huntProgress != null)
			{
				GameStatsManager.Instance.huntProgress.ProcessStatuesKill();
			}
			UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
		base.specRigidbody.PixelColliders[0].Enabled = false;
		base.specRigidbody.PixelColliders[1].Enabled = false;
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		this.m_isReallyDead = true;
		if (this.m_statueController)
		{
			UnityEngine.Object.Destroy(this.m_statueController);
		}
		if (base.aiActor)
		{
			UnityEngine.Object.Destroy(base.aiActor);
		}
		if (base.healthHaver)
		{
			UnityEngine.Object.Destroy(base.healthHaver);
		}
		if (base.behaviorSpeculator)
		{
			UnityEngine.Object.Destroy(base.behaviorSpeculator);
		}
		base.RegenerateCache();
		yield break;
	}

	// Token: 0x0600593A RID: 22842 RVA: 0x00220EDC File Offset: 0x0021F0DC
	private IEnumerator DeathFlashCR()
	{
		Color startingColor = base.renderer.material.GetColor("_OverrideColor");
		while (!this.m_isReallyDead)
		{
			base.renderer.material.SetColor("_OverrideColor", Color.white);
			yield return new WaitForSeconds(this.deathFlashInterval);
			if (this.m_isReallyDead)
			{
				break;
			}
			base.renderer.material.SetColor("_OverrideColor", startingColor);
			yield return new WaitForSeconds(this.deathFlashInterval);
		}
		yield break;
	}

	// Token: 0x04005278 RID: 21112
	public float deathFlashInterval = 0.1f;

	// Token: 0x04005279 RID: 21113
	public List<GameObject> explosionVfx;

	// Token: 0x0400527A RID: 21114
	public float explosionMidDelay = 0.3f;

	// Token: 0x0400527B RID: 21115
	public int explosionCount = 10;

	// Token: 0x0400527C RID: 21116
	public Transform bigExplosionTransform;

	// Token: 0x0400527D RID: 21117
	public GameObject bigExplosionVfx;

	// Token: 0x0400527E RID: 21118
	public List<GameObject> debrisObjects;

	// Token: 0x0400527F RID: 21119
	public int debrisCount;

	// Token: 0x04005280 RID: 21120
	public int debrisMinForce = 5;

	// Token: 0x04005281 RID: 21121
	public int debrisMaxForce = 5;

	// Token: 0x04005282 RID: 21122
	public float debrisAngleVariance = 15f;

	// Token: 0x04005283 RID: 21123
	private BossStatueController m_statueController;

	// Token: 0x04005284 RID: 21124
	private bool m_isReallyDead;
}
