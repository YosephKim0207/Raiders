using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FD0 RID: 4048
public class BlobulordDeathController : BraveBehaviour
{
	// Token: 0x06005847 RID: 22599 RVA: 0x0021C050 File Offset: 0x0021A250
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
	}

	// Token: 0x06005848 RID: 22600 RVA: 0x0021C078 File Offset: 0x0021A278
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005849 RID: 22601 RVA: 0x0021C080 File Offset: 0x0021A280
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.PlayUntilFinished("death", true, null, -1f, false);
		base.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x0600584A RID: 22602 RVA: 0x0021C0A8 File Offset: 0x0021A2A8
	private IEnumerator OnDeathExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		Vector2 colliderMinPos = collider.UnitBottomLeft;
		Vector2 colliderMaxPos = collider.UnitTopRight;
		Vector2 scalePoint = base.specRigidbody.UnitCenter;
		base.specRigidbody.enabled = false;
		GameObject scaleParent = new GameObject("Blobulord Scaler");
		scaleParent.transform.position = scalePoint;
		base.transform.parent = scaleParent.transform;
		float scale = 1f;
		float totalTime = 0f;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = scalePoint - (scalePoint - colliderMinPos) * scale;
			Vector2 maxPos = scalePoint - (scalePoint - colliderMaxPos) * scale;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.8f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			float timer = 0f;
			while (timer < this.explosionMidDelay)
			{
				yield return null;
				timer += BraveTime.DeltaTime;
				totalTime += BraveTime.DeltaTime;
				scale = BraveMathCollege.QuantizeFloat(Mathf.Lerp(1f, this.finalScale, totalTime / ((float)this.explosionCount * this.explosionMidDelay)), 0.04f);
				scaleParent.transform.localScale = new Vector3(scale, scale, 1f);
			}
		}
		GameObject spawnedExplosion = SpawnManager.SpawnVFX(this.bigExplosionVfx, scalePoint, Quaternion.identity);
		tk2dBaseSprite explosionSprite = spawnedExplosion.GetComponent<tk2dSprite>();
		explosionSprite.HeightOffGround = 0.8f;
		base.sprite.AttachRenderer(explosionSprite);
		base.sprite.UpdateZDepth();
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		yield return new WaitForSeconds(this.crawlerSpawnDelay);
		AIActor crawlerPrefab = EnemyDatabase.GetOrLoadByGuid(this.crawlerGuid);
		AIActor crawler = AIActor.Spawn(crawlerPrefab, base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), base.aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Default, true);
		if (crawler)
		{
			crawler.PreventAutoKillOnBossDeath = true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04005166 RID: 20838
	public List<GameObject> explosionVfx;

	// Token: 0x04005167 RID: 20839
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005168 RID: 20840
	public int explosionCount = 10;

	// Token: 0x04005169 RID: 20841
	public float finalScale = 0.1f;

	// Token: 0x0400516A RID: 20842
	public GameObject bigExplosionVfx;

	// Token: 0x0400516B RID: 20843
	public float crawlerSpawnDelay = 0.3f;

	// Token: 0x0400516C RID: 20844
	[EnemyIdentifier]
	public string crawlerGuid;
}
