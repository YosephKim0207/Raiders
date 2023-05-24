using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001003 RID: 4099
public class BulletKingDeathController : BraveBehaviour
{
	// Token: 0x060059AD RID: 22957 RVA: 0x00223F94 File Offset: 0x00222194
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x00223FBC File Offset: 0x002221BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x00223FC4 File Offset: 0x002221C4
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilFinished("death", true, null, -1f, false);
		base.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x00223FEC File Offset: 0x002221EC
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
		GameObject spawnedExplosion = SpawnManager.SpawnVFX(this.bigExplosionVfx, collider.UnitCenter, Quaternion.identity);
		tk2dBaseSprite explosionSprite = spawnedExplosion.GetComponent<tk2dSprite>();
		explosionSprite.HeightOffGround = 0.8f;
		base.sprite.AttachRenderer(explosionSprite);
		base.sprite.UpdateZDepth();
		base.aiAnimator.ChildAnimator.gameObject.SetActive(false);
		yield return new WaitForSeconds(this.throneFallDelay);
		base.aiAnimator.PlayUntilFinished("throne_fall", false, null, -1f, false);
		while (base.aiAnimator.IsPlaying("throne_fall"))
		{
			yield return null;
		}
		foreach (AdditionalBraveLight additionalBraveLight in base.GetComponentsInChildren<AdditionalBraveLight>())
		{
			AdditionalBraveLight additionalBraveLight2 = new GameObject("bullet king light")
			{
				transform = 
				{
					position = base.sprite.WorldCenter
				}
			}.AddComponent<AdditionalBraveLight>();
			additionalBraveLight2.LightColor = additionalBraveLight.LightColor;
			additionalBraveLight2.LightIntensity = additionalBraveLight.LightIntensity;
			additionalBraveLight2.LightRadius = additionalBraveLight.LightRadius;
			additionalBraveLight2.Initialize();
		}
		GameObject throne = UnityEngine.Object.Instantiate<GameObject>(this.thronePrefab, base.transform.position, Quaternion.identity);
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(throne.GetComponent<SpeculativeRigidbody>(), null, false);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040052FE RID: 21246
	public List<GameObject> explosionVfx;

	// Token: 0x040052FF RID: 21247
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005300 RID: 21248
	public int explosionCount = 10;

	// Token: 0x04005301 RID: 21249
	public GameObject bigExplosionVfx;

	// Token: 0x04005302 RID: 21250
	public float throneFallDelay = 1f;

	// Token: 0x04005303 RID: 21251
	public GameObject thronePrefab;
}
