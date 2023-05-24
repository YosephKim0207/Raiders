using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FDD RID: 4061
public class BossFinalMarineDeathController : BraveBehaviour
{
	// Token: 0x06005895 RID: 22677 RVA: 0x0021DCF0 File Offset: 0x0021BEF0
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(5f);
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x0021DD2C File Offset: 0x0021BF2C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x0021DD34 File Offset: 0x0021BF34
	private void OnBossDeath(Vector2 dir)
	{
		base.behaviorSpeculator.enabled = false;
		base.aiActor.BehaviorOverridesVelocity = true;
		base.aiActor.BehaviorVelocity = Vector2.zero;
		base.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
		GameManager.Instance.Dungeon.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x0021DD98 File Offset: 0x0021BF98
	private IEnumerator OnDeathExplosionsCR()
	{
		PastLabMarineController plmc = UnityEngine.Object.FindObjectOfType<PastLabMarineController>();
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 3f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			if (i < this.explosionCount - 1)
			{
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
		}
		for (int j = 0; j < this.bigExplosionCount; j++)
		{
			Vector2 minPos2 = collider.UnitBottomLeft;
			Vector2 maxPos2 = collider.UnitTopRight;
			GameObject vfxPrefab2 = BraveUtility.RandomElement<GameObject>(this.bigExplosionVfx);
			Vector2 pos2 = BraveUtility.RandomVector2(minPos2, maxPos2, new Vector2(1f, 1f));
			GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab2, pos2, Quaternion.identity);
			tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
			vfxSprite2.HeightOffGround = 3f;
			base.sprite.AttachRenderer(vfxSprite2);
			base.sprite.UpdateZDepth();
			if (j < this.bigExplosionCount - 1)
			{
				yield return new WaitForSeconds(this.bigExplosionMidDelay);
			}
		}
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return new WaitForSeconds(2f);
		Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
		plmc.OnBossKilled();
		yield break;
	}

	// Token: 0x040051B6 RID: 20918
	public List<GameObject> explosionVfx;

	// Token: 0x040051B7 RID: 20919
	public float explosionMidDelay = 0.3f;

	// Token: 0x040051B8 RID: 20920
	public int explosionCount = 10;

	// Token: 0x040051B9 RID: 20921
	[Space(12f)]
	public List<GameObject> bigExplosionVfx;

	// Token: 0x040051BA RID: 20922
	public float bigExplosionMidDelay = 0.3f;

	// Token: 0x040051BB RID: 20923
	public int bigExplosionCount = 10;
}
