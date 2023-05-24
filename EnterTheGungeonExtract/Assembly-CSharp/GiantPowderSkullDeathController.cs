using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001038 RID: 4152
public class GiantPowderSkullDeathController : BraveBehaviour
{
	// Token: 0x06005B1D RID: 23325 RVA: 0x0022ED80 File Offset: 0x0022CF80
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005B1E RID: 23326 RVA: 0x0022EDA8 File Offset: 0x0022CFA8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x0022EDB0 File Offset: 0x0022CFB0
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilFinished("death", true, null, -1f, false);
		base.StartCoroutine(this.OnDeathExplosionsCR());
		base.StartCoroutine(this.HandleParticleSystemsCR());
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x0022EDE8 File Offset: 0x0022CFE8
	private IEnumerator OnDeathExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			yield return new WaitForSeconds(this.explosionMidDelay);
		}
		SpawnManager.SpawnVFX(this.bigExplosionVfx, collider.UnitCenter, Quaternion.identity);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		base.specRigidbody.enabled = false;
		base.aiActor.ToggleRenderers(false);
		base.renderer.enabled = false;
		yield break;
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x0022EE04 File Offset: 0x0022D004
	private IEnumerator HandleParticleSystemsCR()
	{
		PowderSkullParticleController particleController = base.aiActor.GetComponentInChildren<PowderSkullParticleController>();
		ParticleSystem mainParticleSystem = particleController.GetComponent<ParticleSystem>();
		ParticleSystem trailParticleSystem = particleController.RotationChild.GetComponentInChildren<ParticleSystem>();
		float startRate = mainParticleSystem.emission.rate.constant;
		mainParticleSystem.transform.parent = null;
		BraveUtility.EnableEmission(trailParticleSystem, false);
		float t = 0f;
		float duration = 6f;
		while (t < duration)
		{
			t += BraveTime.DeltaTime;
			BraveUtility.SetEmissionRate(mainParticleSystem, Mathf.Lerp(startRate, 0f, t / duration));
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040054AD RID: 21677
	public List<GameObject> explosionVfx;

	// Token: 0x040054AE RID: 21678
	public float explosionMidDelay = 0.3f;

	// Token: 0x040054AF RID: 21679
	public int explosionCount = 10;

	// Token: 0x040054B0 RID: 21680
	public GameObject bigExplosionVfx;
}
