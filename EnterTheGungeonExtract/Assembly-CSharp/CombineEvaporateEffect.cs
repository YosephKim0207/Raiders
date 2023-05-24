using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200128B RID: 4747
public class CombineEvaporateEffect : MonoBehaviour
{
	// Token: 0x06006A42 RID: 27202 RVA: 0x0029A900 File Offset: 0x00298B00
	private void Start()
	{
		Projectile component = base.GetComponent<Projectile>();
		Projectile projectile = component;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x06006A43 RID: 27203 RVA: 0x0029A938 File Offset: 0x00298B38
	private void HandleHitEnemy(Projectile proj, SpeculativeRigidbody hitRigidbody, bool fatal)
	{
		if (fatal)
		{
			AIActor aiActor = hitRigidbody.aiActor;
			if (aiActor && aiActor.IsNormalEnemy && (!aiActor.healthHaver || !aiActor.healthHaver.IsBoss))
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemyDeath(aiActor, proj.LastVelocity));
			}
		}
	}

	// Token: 0x06006A44 RID: 27204 RVA: 0x0029A9A8 File Offset: 0x00298BA8
	private IEnumerator HandleEnemyDeath(AIActor target, Vector2 motionDirection)
	{
		target.EraseFromExistenceWithRewards(false);
		Transform copyTransform = this.CreateEmptySprite(target);
		tk2dSprite copySprite = copyTransform.GetComponentInChildren<tk2dSprite>();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleSystemToSpawn, copySprite.WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
		ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
		gameObject.transform.parent = copyTransform;
		if (copySprite)
		{
			gameObject.transform.position = copySprite.WorldCenter;
			Bounds bounds = copySprite.GetBounds();
			component.shape.scale = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, 0.125f);
		}
		float elapsed = 0f;
		float duration = 2.5f;
		copySprite.renderer.material.DisableKeyword("TINTING_OFF");
		copySprite.renderer.material.EnableKeyword("TINTING_ON");
		copySprite.renderer.material.DisableKeyword("EMISSIVE_OFF");
		copySprite.renderer.material.EnableKeyword("EMISSIVE_ON");
		copySprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
		copySprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
		copySprite.renderer.material.SetFloat("_EmissiveThresholdSensitivity", 5f);
		copySprite.renderer.material.SetFloat("_EmissiveColorPower", 1f);
		int emId = Shader.PropertyToID("_EmissivePower");
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			copySprite.renderer.material.SetFloat(emId, Mathf.Lerp(1f, 10f, t));
			copySprite.renderer.material.SetFloat("_BurnAmount", t);
			copyTransform.position += motionDirection.ToVector3ZisY(0f).normalized * BraveTime.DeltaTime * 1f;
			yield return null;
		}
		UnityEngine.Object.Destroy(copyTransform.gameObject);
		yield break;
	}

	// Token: 0x06006A45 RID: 27205 RVA: 0x0029A9D4 File Offset: 0x00298BD4
	private Transform CreateEmptySprite(AIActor target)
	{
		GameObject gameObject = new GameObject("suck image");
		gameObject.layer = target.gameObject.layer;
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
		tk2dSprite.transform.position = target.sprite.transform.position;
		GameObject gameObject2 = new GameObject("image parent");
		gameObject2.transform.position = tk2dSprite.WorldCenter;
		tk2dSprite.transform.parent = gameObject2.transform;
		tk2dSprite.usesOverrideMaterial = true;
		if (target.optionalPalette != null)
		{
			tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
		}
		if (tk2dSprite.renderer.material.shader.name.Contains("ColorEmissive"))
		{
		}
		return gameObject2.transform;
	}

	// Token: 0x040066BB RID: 26299
	public GameObject ParticleSystemToSpawn;

	// Token: 0x040066BC RID: 26300
	public Shader FallbackShader;
}
