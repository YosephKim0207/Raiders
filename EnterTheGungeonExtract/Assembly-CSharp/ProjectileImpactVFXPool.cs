using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001610 RID: 5648
[Serializable]
public class ProjectileImpactVFXPool
{
	// Token: 0x060083AF RID: 33711 RVA: 0x0035F4FC File Offset: 0x0035D6FC
	public GameObject SpawnVFXEnemy(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(prefab, position, rotation, ignoresPools);
		if (gameObject.CompareTag("DefaultEnemyHitVFX"))
		{
			tk2dSpriteAnimator component = gameObject.GetComponent<tk2dSpriteAnimator>();
			component.deferNextStartClip = true;
			component.Play("Dust_Impact_Enemy");
		}
		return gameObject;
	}

	// Token: 0x060083B0 RID: 33712 RVA: 0x0035F540 File Offset: 0x0035D740
	public void HandleProjectileDeathVFX(Vector3 position, float rotation, Transform enemyTransform, Vector2 sourceNormal, Vector2 sourceVelocity, bool isObject = false)
	{
		if (this.suppressHitEffectsIfOffscreen && !GameManager.Instance.MainCameraController.PointIsVisible(position, 0.05f))
		{
			return;
		}
		if (isObject)
		{
			this.deathAny.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), null, false, new VFXComplex.SpawnMethod(this.SpawnVFXEnemy), null, false);
		}
		else
		{
			this.deathAny.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), null, false, null, null, false);
		}
	}

	// Token: 0x060083B1 RID: 33713 RVA: 0x0035F5E4 File Offset: 0x0035D7E4
	public void HandleEnemyImpact(Vector3 position, float rotation, Transform enemyTransform, Vector2 sourceNormal, Vector2 sourceVelocity, bool playProjectileDeathVfx, bool isObject = false)
	{
		if (this.suppressHitEffectsIfOffscreen && !GameManager.Instance.MainCameraController.PointIsVisible(position, 0.05f))
		{
			return;
		}
		float? num = null;
		if (Projectile.CurrentProjectileDepth != 0.8f)
		{
			num = new float?(Projectile.CurrentProjectileDepth);
		}
		if (isObject)
		{
			this.enemy.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), num, false, new VFXComplex.SpawnMethod(this.SpawnVFXEnemy), null, false);
			if (playProjectileDeathVfx && this.HasProjectileDeathVFX)
			{
				this.deathEnemy.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), num, false, new VFXComplex.SpawnMethod(this.SpawnVFXEnemy), null, false);
			}
		}
		else
		{
			this.enemy.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), num, false, null, null, false);
			if (playProjectileDeathVfx && this.HasProjectileDeathVFX)
			{
				this.deathEnemy.SpawnAtPosition(position, rotation, enemyTransform, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), num, false, new VFXComplex.SpawnMethod(this.SpawnVFXEnemy), null, false);
			}
		}
	}

	// Token: 0x060083B2 RID: 33714 RVA: 0x0035F718 File Offset: 0x0035D918
	public void HandleTileMapImpactVertical(Vector3 position, float heightOffGroundOffset, float rotation, Vector2 sourceNormal, Vector2 sourceVelocity, bool playProjectileDeathVfx, Transform parent = null, VFXComplex.SpawnMethod overrideSpawnMethod = null, VFXComplex.SpawnMethod overrideDeathSpawnMethod = null)
	{
		if (this.suppressHitEffectsIfOffscreen && !GameManager.Instance.MainCameraController.PointIsVisible(position, 0.05f))
		{
			return;
		}
		float num = rotation + 90f;
		if (!this.HasTileMapVerticalEffects)
		{
			int roomVisualTypeAtPosition = GameManager.Instance.Dungeon.data.GetRoomVisualTypeAtPosition(position.XY());
			GameManager.Instance.Dungeon.roomMaterialDefinitions[roomVisualTypeAtPosition].SpawnRandomVertical(position, num, parent, sourceNormal, sourceVelocity);
			return;
		}
		float num2 = (float)Mathf.FloorToInt(position.y) - heightOffGroundOffset;
		this.tileMapVertical.SpawnAtPosition(position.x, num2, position.y - num2, num, parent, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), false, overrideSpawnMethod, false);
		if (playProjectileDeathVfx && this.HasProjectileDeathVFX)
		{
			VFXPool vfxpool = this.deathTileMapVertical;
			float x = position.x;
			float num3 = num2;
			float num4 = position.y - num2;
			float num5 = num;
			Vector2? vector = new Vector2?(sourceNormal);
			Vector2? vector2 = new Vector2?(sourceVelocity);
			vfxpool.SpawnAtPosition(x, num3, num4, num5, parent, vector, vector2, false, overrideDeathSpawnMethod, false);
		}
	}

	// Token: 0x060083B3 RID: 33715 RVA: 0x0035F840 File Offset: 0x0035DA40
	public void HandleTileMapImpactHorizontal(Vector3 position, float rotation, Vector2 sourceNormal, Vector2 sourceVelocity, bool playProjectileDeathVfx, Transform parent = null, VFXComplex.SpawnMethod overrideSpawnMethod = null, VFXComplex.SpawnMethod overrideDeathSpawnMethod = null)
	{
		if (this.suppressHitEffectsIfOffscreen && !GameManager.Instance.MainCameraController.PointIsVisible(position, 0.05f))
		{
			return;
		}
		if (!this.HasTileMapHorizontalEffects)
		{
			int roomVisualTypeAtPosition = GameManager.Instance.Dungeon.data.GetRoomVisualTypeAtPosition(position.XY());
			GameManager.Instance.Dungeon.roomMaterialDefinitions[roomVisualTypeAtPosition].SpawnRandomHorizontal(position, rotation, parent, sourceNormal, sourceVelocity);
			return;
		}
		this.tileMapHorizontal.SpawnAtPosition(position, rotation, parent, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), null, false, overrideSpawnMethod, null, false);
		if (playProjectileDeathVfx && this.HasProjectileDeathVFX)
		{
			VFXPool vfxpool = this.deathTileMapHorizontal;
			Vector2? vector = new Vector2?(sourceNormal);
			Vector2? vector2 = new Vector2?(sourceVelocity);
			vfxpool.SpawnAtPosition(position, rotation, parent, vector, vector2, null, false, overrideDeathSpawnMethod, null, false);
		}
	}

	// Token: 0x170013BB RID: 5051
	// (get) Token: 0x060083B4 RID: 33716 RVA: 0x0035F938 File Offset: 0x0035DB38
	public bool HasTileMapVerticalEffects
	{
		get
		{
			return this.tileMapVertical.type != VFXPoolType.None || (this.HasProjectileDeathVFX && this.deathTileMapVertical.type != VFXPoolType.None);
		}
	}

	// Token: 0x170013BC RID: 5052
	// (get) Token: 0x060083B5 RID: 33717 RVA: 0x0035F96C File Offset: 0x0035DB6C
	public bool HasTileMapHorizontalEffects
	{
		get
		{
			return this.tileMapHorizontal.type != VFXPoolType.None || (this.HasProjectileDeathVFX && this.deathTileMapHorizontal.type != VFXPoolType.None);
		}
	}

	// Token: 0x040086F5 RID: 34549
	public bool alwaysUseMidair;

	// Token: 0x040086F6 RID: 34550
	[HideInInspectorIf("alwaysUseMidair", false)]
	public VFXPool tileMapVertical;

	// Token: 0x040086F7 RID: 34551
	[HideInInspectorIf("alwaysUseMidair", false)]
	public VFXPool tileMapHorizontal;

	// Token: 0x040086F8 RID: 34552
	[HideInInspectorIf("alwaysUseMidair", false)]
	public VFXPool enemy;

	// Token: 0x040086F9 RID: 34553
	public bool suppressMidairDeathVfx;

	// Token: 0x040086FA RID: 34554
	public GameObject overrideMidairDeathVFX;

	// Token: 0x040086FB RID: 34555
	[ShowInInspectorIf("overrideMidairDeathVFX", false)]
	public bool midairInheritsRotation;

	// Token: 0x040086FC RID: 34556
	[ShowInInspectorIf("overrideMidairDeathVFX", false)]
	public bool midairInheritsVelocity;

	// Token: 0x040086FD RID: 34557
	[ShowInInspectorIf("overrideMidairDeathVFX", false)]
	public bool midairInheritsFlip;

	// Token: 0x040086FE RID: 34558
	[ShowInInspectorIf("overrideMidairDeathVFX", false)]
	public int overrideMidairZHeight = -1;

	// Token: 0x040086FF RID: 34559
	public GameObject overrideEarlyDeathVfx;

	// Token: 0x04008700 RID: 34560
	public bool HasProjectileDeathVFX;

	// Token: 0x04008701 RID: 34561
	[ShowInInspectorIf("HasProjectileDeathVFX", true)]
	public VFXPool deathTileMapVertical;

	// Token: 0x04008702 RID: 34562
	[ShowInInspectorIf("HasProjectileDeathVFX", true)]
	public VFXPool deathTileMapHorizontal;

	// Token: 0x04008703 RID: 34563
	[ShowInInspectorIf("HasProjectileDeathVFX", true)]
	public VFXPool deathEnemy;

	// Token: 0x04008704 RID: 34564
	[ShowInInspectorIf("HasProjectileDeathVFX", true)]
	[FormerlySerializedAs("ProjectileDeathVFX")]
	public VFXPool deathAny;

	// Token: 0x04008705 RID: 34565
	[ShowInInspectorIf("HasProjectileDeathVFX", true)]
	public bool CenterDeathVFXOnProjectile;

	// Token: 0x04008706 RID: 34566
	[NonSerialized]
	public bool suppressHitEffectsIfOffscreen;
}
