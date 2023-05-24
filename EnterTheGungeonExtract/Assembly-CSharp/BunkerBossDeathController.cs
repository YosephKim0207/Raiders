using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001007 RID: 4103
public class BunkerBossDeathController : BraveBehaviour
{
	// Token: 0x060059C8 RID: 22984 RVA: 0x00224B54 File Offset: 0x00222D54
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x060059C9 RID: 22985 RVA: 0x00224B7C File Offset: 0x00222D7C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060059CA RID: 22986 RVA: 0x00224B84 File Offset: 0x00222D84
	private void OnBossDeath(Vector2 dir)
	{
		base.StartCoroutine(this.OnDeathExplosionsCR());
		base.StartCoroutine(this.OnDeathDebrisCR());
		base.StartCoroutine(this.OnDeathAnimationCR());
	}

	// Token: 0x060059CB RID: 22987 RVA: 0x00224BB0 File Offset: 0x00222DB0
	private IEnumerator OnDeathExplosionsCR()
	{
		Vector2 minPos = base.specRigidbody.UnitBottomLeft;
		Vector2 maxPos = base.specRigidbody.UnitTopRight;
		for (int i = 0; i < this.explosionCount; i++)
		{
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(1f, 1.5f));
			SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		yield break;
	}

	// Token: 0x060059CC RID: 22988 RVA: 0x00224BCC File Offset: 0x00222DCC
	private IEnumerator OnDeathDebrisCR()
	{
		Vector2 minPos = base.specRigidbody.UnitBottomLeft;
		Vector2 centerPos = base.specRigidbody.UnitCenter;
		Vector2 maxPos = base.specRigidbody.UnitTopRight;
		for (int i = 0; i < this.debrisCount; i++)
		{
			GameObject shardPrefab = BraveUtility.RandomElement<GameObject>(this.debrisObjects);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(-1.5f, -1.5f));
			GameObject shardObj = SpawnManager.SpawnVFX(shardPrefab, pos, Quaternion.identity);
			if (shardObj)
			{
				shardObj.transform.parent = SpawnManager.Instance.VFX;
				DebrisObject orAddComponent = shardObj.GetOrAddComponent<DebrisObject>();
				if (base.aiActor)
				{
					base.aiActor.sprite.AttachRenderer(orAddComponent.sprite);
				}
				orAddComponent.angularVelocity = Mathf.Sign(UnityEngine.Random.value - 0.5f) * 125f;
				orAddComponent.angularVelocityVariance = 60f;
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				float num = (pos - centerPos).ToAngle() + UnityEngine.Random.Range(-this.debrisAngleVariance, this.debrisAngleVariance);
				Vector2 vector = BraveMathCollege.DegreesToVector(num, 1f) * (float)UnityEngine.Random.Range(this.debrisMinForce, this.debrisMaxForce);
				Vector3 vector2 = new Vector3(vector.x, (vector.y >= 0f) ? 0f : vector.y, (vector.y <= 0f) ? 0f : vector.y);
				if (orAddComponent.minorBreakable)
				{
					orAddComponent.minorBreakable.enabled = true;
				}
				orAddComponent.Trigger(vector2, 1f, 1f);
			}
			yield return new WaitForSeconds(this.debrisMidDelay);
		}
		yield break;
	}

	// Token: 0x060059CD RID: 22989 RVA: 0x00224BE8 File Offset: 0x00222DE8
	private IEnumerator OnDeathAnimationCR()
	{
		Vector2 minPos = base.specRigidbody.UnitBottomLeft + this.dustOffset;
		Vector2 maxPos = base.specRigidbody.UnitBottomLeft + this.dustOffset + this.dustDimensions;
		yield return new WaitForSeconds(this.deathAnimationDelay);
		base.aiAnimator.PlayUntilFinished(this.deathAnimation, false, null, -1f, false);
		float timer = this.dustTime;
		float intraTimer = 0f;
		float shakeTimer = 0f;
		IntVector2 shakeDir = IntVector2.Right;
		while (timer > 0f)
		{
			while (intraTimer <= 0f)
			{
				GameObject gameObject = BraveUtility.RandomElement<GameObject>(this.dustVfx);
				Vector2 vector = BraveUtility.RandomVector2(minPos, maxPos);
				GameObject gameObject2 = SpawnManager.SpawnVFX(gameObject, vector, Quaternion.identity);
				tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
				if (component)
				{
					base.sprite.AttachRenderer(component);
					component.HeightOffGround = 0.1f;
					component.UpdateZDepth();
				}
				intraTimer += this.dustMidDelay;
			}
			while (shakeTimer <= 0f)
			{
				base.transform.position += PhysicsEngine.PixelToUnit(shakeDir);
				shakeDir *= -1;
				shakeTimer += this.shakeMidDelay;
				if (shakeTimer > 0f)
				{
					base.specRigidbody.Reinitialize();
				}
			}
			yield return null;
			timer -= BraveTime.DeltaTime;
			intraTimer -= BraveTime.DeltaTime;
			shakeTimer -= BraveTime.DeltaTime;
		}
		if (shakeDir.x < 0)
		{
			base.transform.position += PhysicsEngine.PixelToUnit(shakeDir);
		}
		base.aiAnimator.PlayUntilFinished(this.flagAnimation, false, null, -1f, false);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		base.specRigidbody.PixelColliders.RemoveAt(1);
		base.specRigidbody.PixelColliders[0].ManualHeight -= 22;
		base.specRigidbody.RegenerateColliders = true;
		base.specRigidbody.Reinitialize();
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

	// Token: 0x0400531F RID: 21279
	public List<GameObject> explosionVfx;

	// Token: 0x04005320 RID: 21280
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005321 RID: 21281
	public int explosionCount = 10;

	// Token: 0x04005322 RID: 21282
	public List<GameObject> debrisObjects;

	// Token: 0x04005323 RID: 21283
	public float debrisMidDelay;

	// Token: 0x04005324 RID: 21284
	public int debrisCount;

	// Token: 0x04005325 RID: 21285
	public int debrisMinForce = 5;

	// Token: 0x04005326 RID: 21286
	public int debrisMaxForce = 5;

	// Token: 0x04005327 RID: 21287
	public float debrisAngleVariance = 15f;

	// Token: 0x04005328 RID: 21288
	public string deathAnimation;

	// Token: 0x04005329 RID: 21289
	public float deathAnimationDelay;

	// Token: 0x0400532A RID: 21290
	public List<GameObject> dustVfx;

	// Token: 0x0400532B RID: 21291
	public float dustTime = 1f;

	// Token: 0x0400532C RID: 21292
	public float dustMidDelay = 0.05f;

	// Token: 0x0400532D RID: 21293
	public Vector2 dustOffset;

	// Token: 0x0400532E RID: 21294
	public Vector2 dustDimensions;

	// Token: 0x0400532F RID: 21295
	public float shakeMidDelay = 0.1f;

	// Token: 0x04005330 RID: 21296
	public string flagAnimation;
}
