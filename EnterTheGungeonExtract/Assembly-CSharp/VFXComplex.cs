using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001849 RID: 6217
[Serializable]
public class VFXComplex
{
	// Token: 0x06009315 RID: 37653 RVA: 0x003E1290 File Offset: 0x003DF490
	public void SpawnAtPosition(float xPosition, float yPositionAtGround, float heightOffGround, float zRotation = 0f, Transform parent = null, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, bool ignoresPools = false)
	{
		Vector3 vector = new Vector3(xPosition, yPositionAtGround + heightOffGround, yPositionAtGround - heightOffGround);
		Action<VFXObject, tk2dSprite> action = delegate(VFXObject effect, tk2dSprite vfxSprite)
		{
			vfxSprite.HeightOffGround = 2f * heightOffGround;
			vfxSprite.UpdateZDepth();
		};
		this.InternalSpawnAtLocation(vector, zRotation, parent, sourceNormal, sourceVelocity, action, keepReferences, spawnMethod, ignoresPools);
	}

	// Token: 0x06009316 RID: 37654 RVA: 0x003E12E8 File Offset: 0x003DF4E8
	public void SpawnAtPosition(Vector3 position, float zRotation = 0f, Transform parent = null, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, float? heightOffGround = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, tk2dBaseSprite spriteParent = null, bool ignoresPools = false)
	{
		Action<VFXObject, tk2dSprite> action = delegate(VFXObject effect, tk2dSprite vfxSprite)
		{
			if (spriteParent != null)
			{
				spriteParent.AttachRenderer(vfxSprite);
				vfxSprite.HeightOffGround = 0.05f;
				vfxSprite.UpdateZDepth();
			}
			else if (vfxSprite.Collection != null)
			{
				DepthLookupManager.ProcessRenderer(vfxSprite.renderer);
				if (Mathf.Abs(zRotation) > 90f)
				{
					vfxSprite.FlipY = true;
				}
				if (heightOffGround != null)
				{
					vfxSprite.HeightOffGround = heightOffGround.Value;
				}
				else if (effect.usesZHeight)
				{
					vfxSprite.HeightOffGround = effect.zHeight;
				}
				else
				{
					vfxSprite.HeightOffGround = 0.9f;
				}
				vfxSprite.UpdateZDepth();
			}
		};
		this.InternalSpawnAtLocation(position, zRotation, parent, sourceNormal, sourceVelocity, action, keepReferences, spawnMethod, ignoresPools);
	}

	// Token: 0x06009317 RID: 37655 RVA: 0x003E1338 File Offset: 0x003DF538
	public void SpawnAtLocalPosition(Vector3 localPosition, float zRotation, Transform parent, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, bool keepReferences = false, VFXComplex.SpawnMethod spawnMethod = null, bool ignoresPools = false)
	{
		Vector3 vector = parent.transform.position + localPosition;
		Action<VFXObject, tk2dSprite> action = delegate(VFXObject effect, tk2dSprite vfxSprite)
		{
			if (effect.usesZHeight)
			{
				vfxSprite.HeightOffGround = effect.zHeight;
			}
			else if (effect.orphaned && !vfxSprite.IsPerpendicular)
			{
				vfxSprite.HeightOffGround = 0f;
			}
			else
			{
				vfxSprite.HeightOffGround = 0.9f;
			}
			vfxSprite.UpdateZDepth();
		};
		this.InternalSpawnAtLocation(vector, zRotation, parent, sourceNormal, sourceVelocity, action, keepReferences, spawnMethod, ignoresPools);
	}

	// Token: 0x06009318 RID: 37656 RVA: 0x003E138C File Offset: 0x003DF58C
	public void RemoveDespawnedVfx()
	{
		for (int i = this.m_spawnedObjects.Count - 1; i >= 0; i--)
		{
			if (!this.m_spawnedObjects[i] || !this.m_spawnedObjects[i].activeSelf)
			{
				this.m_spawnedObjects.RemoveAt(i);
			}
		}
	}

	// Token: 0x06009319 RID: 37657 RVA: 0x003E13F0 File Offset: 0x003DF5F0
	public void DestroyAll()
	{
		for (int i = 0; i < this.m_spawnedObjects.Count; i++)
		{
			if (this.m_spawnedObjects[i])
			{
				if (SpawnManager.Instance)
				{
					this.m_spawnedObjects[i].transform.parent = SpawnManager.Instance.VFX;
				}
				SpawnManager.Despawn(this.m_spawnedObjects[i]);
			}
		}
		this.m_spawnedObjects.Clear();
	}

	// Token: 0x0600931A RID: 37658 RVA: 0x003E147C File Offset: 0x003DF67C
	public void ForEach(Action<GameObject> action)
	{
		for (int i = 0; i < this.m_spawnedObjects.Count; i++)
		{
			if (this.m_spawnedObjects[i])
			{
				action(this.m_spawnedObjects[i]);
			}
		}
	}

	// Token: 0x0600931B RID: 37659 RVA: 0x003E14D0 File Offset: 0x003DF6D0
	public void ToggleRenderers(bool value)
	{
		for (int i = 0; i < this.m_spawnedObjects.Count; i++)
		{
			if (this.m_spawnedObjects[i])
			{
				foreach (Renderer renderer in this.m_spawnedObjects[i].GetComponentsInChildren<Renderer>())
				{
					renderer.enabled = value;
				}
			}
		}
	}

	// Token: 0x0600931C RID: 37660 RVA: 0x003E1540 File Offset: 0x003DF740
	public void SetHeightOffGround(float height)
	{
		for (int i = 0; i < this.m_spawnedObjects.Count; i++)
		{
			if (this.m_spawnedObjects[i])
			{
				foreach (tk2dBaseSprite tk2dBaseSprite in this.m_spawnedObjects[i].GetComponentsInChildren<tk2dBaseSprite>())
				{
					tk2dBaseSprite.HeightOffGround = height;
					if (tk2dBaseSprite.attachParent == null)
					{
						tk2dBaseSprite.UpdateZDepth();
					}
				}
			}
		}
	}

	// Token: 0x0600931D RID: 37661 RVA: 0x003E15C8 File Offset: 0x003DF7C8
	protected void HandleDebris(GameObject vfx, float heightOffGround, Vector2? sourceNormal, Vector2? sourceVelocity)
	{
		DebrisObject component = vfx.GetComponent<DebrisObject>();
		if (component != null && sourceNormal != null && sourceVelocity != null)
		{
			if (sourceNormal == null)
			{
				Debug.LogWarning("Trying to create debris for an effect with no normal.");
			}
			if (sourceVelocity == null)
			{
				Debug.LogWarning("Trying to create debris for an effect with no velocity.");
			}
			tk2dBaseSprite sprite = component.sprite;
			sprite.IsPerpendicular = false;
			sprite.usesOverrideMaterial = true;
			Bounds bounds = sprite.GetBounds();
			component.transform.position = component.transform.position + new Vector3(BraveMathCollege.ActualSign(sourceNormal.Value.x) * bounds.size.x, 0f, 0f);
			float num = Mathf.Atan2(sourceVelocity.Value.y, sourceVelocity.Value.x) * 57.29578f;
			vfx.transform.localRotation = Quaternion.Euler(0f, 0f, num);
			Vector2 vector = BraveMathCollege.ReflectVectorAcrossNormal(sourceVelocity.Value, sourceNormal.Value).normalized;
			float num2 = UnityEngine.Random.Range(-20f, 20f);
			vector = Quaternion.Euler(0f, 0f, num2) * vector;
			component.Trigger(vector.ToVector3ZUp(0.1f), heightOffGround + 1f, 1f);
		}
	}

	// Token: 0x0600931E RID: 37662 RVA: 0x003E1754 File Offset: 0x003DF954
	protected void HandleAttachment(tk2dSprite vfxSprite, Transform parent)
	{
		if (parent != null && parent.parent != null)
		{
			tk2dSprite componentInChildren = parent.parent.GetComponentInChildren<tk2dSprite>();
			if (vfxSprite != null && componentInChildren != null)
			{
				componentInChildren.AttachRenderer(vfxSprite);
			}
		}
	}

	// Token: 0x0600931F RID: 37663 RVA: 0x003E17AC File Offset: 0x003DF9AC
	protected void InternalSpawnAtLocation(Vector3 position, float zRotation, Transform parent, Vector2? sourceNormal, Vector2? sourceVelocity, Action<VFXObject, tk2dSprite> vfxSpriteManipulator, bool keepReferences, VFXComplex.SpawnMethod spawnMethod, bool ignoresPools)
	{
		if (spawnMethod == null)
		{
			spawnMethod = new VFXComplex.SpawnMethod(SpawnManager.SpawnVFX);
		}
		this.m_spawnedObjects.RemoveAll((GameObject go) => !go);
		for (int i = 0; i < this.effects.Length; i++)
		{
			if (!(this.effects[i].effect == null))
			{
				if (this.effects[i].alignment == VFXAlignment.NormalAligned && sourceNormal != null)
				{
					zRotation = Mathf.Atan2(sourceNormal.Value.y, sourceNormal.Value.x) * 57.29578f;
				}
				if (this.effects[i].alignment == VFXAlignment.VelocityAligned && sourceVelocity != null)
				{
					zRotation = Mathf.Atan2(sourceVelocity.Value.y, sourceVelocity.Value.x) * 57.29578f + 180f;
				}
				Vector3 vector = position.Quantize(0.0625f);
				GameObject gameObject = spawnMethod(this.effects[i].effect, vector, Quaternion.identity, ignoresPools);
				if (gameObject)
				{
					if (keepReferences && !this.effects[i].persistsOnDeath)
					{
						this.m_spawnedObjects.Add(gameObject);
					}
					tk2dSprite componentInChildren = gameObject.GetComponentInChildren<tk2dSprite>();
					if (componentInChildren != null)
					{
						vfxSpriteManipulator(this.effects[i], componentInChildren);
						if (this.effects[i].usesZHeight)
						{
							componentInChildren.HeightOffGround = this.effects[i].zHeight;
							componentInChildren.UpdateZDepth();
						}
					}
					if (gameObject.GetComponent<ParticleSystem>())
					{
						ParticleKiller component = gameObject.GetComponent<ParticleKiller>();
						if (component && component.overrideXRotation)
						{
							gameObject.transform.localRotation = Quaternion.Euler(component.xRotation, 0f, 0f);
						}
						else
						{
							gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
						}
						gameObject.transform.position = gameObject.transform.position.WithZ(gameObject.transform.position.y);
					}
					else
					{
						gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, zRotation);
					}
					this.HandleDebris(gameObject, 0.5f, sourceNormal, sourceVelocity);
					if (parent != null)
					{
						if (!this.effects[i].orphaned)
						{
							gameObject.transform.parent = parent;
							gameObject.transform.localScale = Vector3.one;
							PersistentVFXManagerBehaviour persistentVFXManagerBehaviour = parent.GetComponentInChildren<PersistentVFXManagerBehaviour>() ?? parent.GetComponentInParent<PersistentVFXManagerBehaviour>();
							if (persistentVFXManagerBehaviour != null && !gameObject.GetComponent<SpriteAnimatorKiller>())
							{
								if (this.effects[i].destructible)
								{
									persistentVFXManagerBehaviour.AttachDestructibleVFX(gameObject);
								}
								else
								{
									persistentVFXManagerBehaviour.AttachPersistentVFX(gameObject);
								}
							}
							if (this.effects[i].attached)
							{
								this.HandleAttachment(componentInChildren, parent);
							}
						}
						else
						{
							gameObject.transform.localScale = parent.localScale;
						}
					}
				}
			}
		}
	}

	// Token: 0x04009AA7 RID: 39591
	public VFXObject[] effects;

	// Token: 0x04009AA8 RID: 39592
	private List<GameObject> m_spawnedObjects = new List<GameObject>();

	// Token: 0x0200184A RID: 6218
	// (Invoke) Token: 0x06009323 RID: 37667
	public delegate GameObject SpawnMethod(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools);
}
