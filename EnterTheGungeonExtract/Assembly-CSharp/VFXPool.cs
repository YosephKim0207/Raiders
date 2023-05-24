using System;
using UnityEngine;

// Token: 0x02001848 RID: 6216
[Serializable]
public class VFXPool
{
	// Token: 0x0600930A RID: 37642 RVA: 0x003E0FD8 File Offset: 0x003DF1D8
	public VFXComplex GetEffect()
	{
		if (this.effects == null || this.effects.Length == 0)
		{
			return null;
		}
		switch (this.type)
		{
		case VFXPoolType.None:
			return null;
		case VFXPoolType.All:
			return this.effects[0];
		case VFXPoolType.SequentialGroups:
		{
			VFXComplex vfxcomplex = this.effects[this.m_iterator];
			this.m_iterator = (this.m_iterator + 1) % this.effects.Length;
			return vfxcomplex;
		}
		case VFXPoolType.RandomGroups:
			return this.effects[UnityEngine.Random.Range(0, this.effects.Length)];
		case VFXPoolType.Single:
			return this.effects[0];
		default:
			Debug.LogWarning("Unknown VFXPoolType " + this.type);
			return null;
		}
	}

	// Token: 0x0600930B RID: 37643 RVA: 0x003E1094 File Offset: 0x003DF294
	public void SpawnAtPosition(float xPosition, float yPositionAtGround, float heightOffGround, float zRotation, Transform parent = null, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, bool ignoresPools = false)
	{
		VFXComplex effect = this.GetEffect();
		if (effect != null)
		{
			effect.SpawnAtPosition(xPosition, yPositionAtGround, heightOffGround, zRotation, parent, sourceNormal, sourceVelocity, keepReferences, spawnMethod, ignoresPools);
		}
	}

	// Token: 0x0600930C RID: 37644 RVA: 0x003E10C8 File Offset: 0x003DF2C8
	public void SpawnAtPosition(Vector3 position, float zRotation = 0f, Transform parent = null, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, float? heightOffGround = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, tk2dBaseSprite spriteParent = null, bool ignoresPools = false)
	{
		VFXComplex effect = this.GetEffect();
		if (effect != null)
		{
			effect.SpawnAtPosition(position, zRotation, parent, sourceNormal, sourceVelocity, heightOffGround, keepReferences, spawnMethod, spriteParent, ignoresPools);
		}
	}

	// Token: 0x0600930D RID: 37645 RVA: 0x003E10FC File Offset: 0x003DF2FC
	public void SpawnAtTilemapPosition(Vector3 position, float yPositionAtGround, float zRotation, Vector2 sourceNormal, Vector2 sourceVelocity, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, bool ignoresPools = false)
	{
		VFXComplex effect = this.GetEffect();
		if (effect != null)
		{
			float num = position.y - yPositionAtGround;
			effect.SpawnAtPosition(position.x, yPositionAtGround, num, zRotation, null, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), keepReferences, spawnMethod, ignoresPools);
		}
	}

	// Token: 0x0600930E RID: 37646 RVA: 0x003E1148 File Offset: 0x003DF348
	public void SpawnAtLocalPosition(Vector3 localPosition, float zRotation, Transform parent, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, bool ignoresPools = false)
	{
		VFXComplex effect = this.GetEffect();
		if (effect != null)
		{
			effect.SpawnAtLocalPosition(localPosition, zRotation, parent, sourceNormal, sourceVelocity, keepReferences, spawnMethod, ignoresPools);
		}
	}

	// Token: 0x0600930F RID: 37647 RVA: 0x003E1178 File Offset: 0x003DF378
	public void RemoveDespawnedVfx()
	{
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i].RemoveDespawnedVfx();
		}
	}

	// Token: 0x06009310 RID: 37648 RVA: 0x003E11AC File Offset: 0x003DF3AC
	public void DestroyAll()
	{
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i].DestroyAll();
		}
	}

	// Token: 0x06009311 RID: 37649 RVA: 0x003E11E0 File Offset: 0x003DF3E0
	public void ForEach(Action<GameObject> action)
	{
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i].ForEach(action);
		}
	}

	// Token: 0x06009312 RID: 37650 RVA: 0x003E1214 File Offset: 0x003DF414
	public void ToggleRenderers(bool value)
	{
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i].ToggleRenderers(value);
		}
	}

	// Token: 0x06009313 RID: 37651 RVA: 0x003E1248 File Offset: 0x003DF448
	public void SetHeightOffGround(float height)
	{
		for (int i = 0; i < this.effects.Length; i++)
		{
			this.effects[i].SetHeightOffGround(height);
		}
	}

	// Token: 0x04009AA4 RID: 39588
	public VFXPoolType type;

	// Token: 0x04009AA5 RID: 39589
	public VFXComplex[] effects;

	// Token: 0x04009AA6 RID: 39590
	private int m_iterator;
}
