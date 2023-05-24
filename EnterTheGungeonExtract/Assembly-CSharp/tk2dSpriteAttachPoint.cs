using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BBB RID: 3003
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAttachPoint")]
[ExecuteInEditMode]
public class tk2dSpriteAttachPoint : MonoBehaviour
{
	// Token: 0x06003FD9 RID: 16345 RVA: 0x00143A64 File Offset: 0x00141C64
	public Transform GetAttachPointByName(string name)
	{
		if (this.attachPoints.Count != this.attachPointNames.Count)
		{
			this.ReinitAttachPointNames();
		}
		for (int i = 0; i < this.attachPoints.Count; i++)
		{
			if (this.attachPoints[i].name.ToLowerInvariant() == name.ToLowerInvariant())
			{
				return this.attachPoints[i];
			}
		}
		return null;
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x00143AE4 File Offset: 0x00141CE4
	private void ReinitAttachPointNames()
	{
		this.attachPointNames.Clear();
		for (int i = 0; i < this.attachPoints.Count; i++)
		{
			this.attachPointNames.Add((!this.attachPoints[i]) ? null : this.attachPoints[i].name);
		}
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x00143B50 File Offset: 0x00141D50
	private void Awake()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<tk2dBaseSprite>();
			if (this.sprite != null)
			{
				this.HandleSpriteChanged(this.sprite);
			}
		}
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x00143B8C File Offset: 0x00141D8C
	private void OnEnable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged += this.HandleSpriteChanged;
		}
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x00143BB8 File Offset: 0x00141DB8
	private void OnDisable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged -= this.HandleSpriteChanged;
		}
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x00143BE4 File Offset: 0x00141DE4
	private void UpdateAttachPointTransform(tk2dSpriteDefinition.AttachPoint attachPoint, Transform t)
	{
		if (!this.ignorePosition)
		{
			t.localPosition = Vector3.Scale(attachPoint.position, this.sprite.scale);
		}
		if (!this.ignoreScale)
		{
			t.localScale = this.sprite.scale;
		}
		if (!this.ignoreRotation)
		{
			float num = Mathf.Sign(this.sprite.scale.x) * Mathf.Sign(this.sprite.scale.y);
			t.localEulerAngles = new Vector3(0f, 0f, attachPoint.angle * num);
		}
		if (this.disableEmissionOnUnusedParticleSystems)
		{
			ParticleSystem component = t.GetComponent<ParticleSystem>();
			if (component)
			{
				BraveUtility.EnableEmission(component, true);
			}
		}
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x00143CB4 File Offset: 0x00141EB4
	public void ForceAddAttachPoint(string apname)
	{
		GameObject gameObject = new GameObject(apname);
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		if (this.deactivateUnusedAttachPoints)
		{
			gameObject.SetActive(false);
		}
		this.attachPoints.Add(transform);
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x00143CFC File Offset: 0x00141EFC
	private void HandleSpriteChanged(tk2dBaseSprite spr)
	{
		tk2dSpriteDefinition.AttachPoint[] array = spr.Collection.GetAttachPoints(spr.spriteId);
		if (tk2dSpriteAttachPoint.emptyAttachPointArray == null)
		{
			tk2dSpriteAttachPoint.emptyAttachPointArray = new tk2dSpriteDefinition.AttachPoint[0];
		}
		if (array == null)
		{
			array = tk2dSpriteAttachPoint.emptyAttachPointArray;
		}
		int num = Mathf.Max(array.Length, this.attachPoints.Count);
		if (num > tk2dSpriteAttachPoint.attachPointUpdated.Length)
		{
			tk2dSpriteAttachPoint.attachPointUpdated = new bool[num];
		}
		if (this.attachPoints.Count != this.attachPointNames.Count)
		{
			this.ReinitAttachPointNames();
		}
		for (int i = 0; i < tk2dSpriteAttachPoint.attachPointUpdated.Length; i++)
		{
			tk2dSpriteAttachPoint.attachPointUpdated[i] = false;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (this.attachPoints.Count != this.attachPointNames.Count)
			{
				this.ReinitAttachPointNames();
			}
			tk2dSpriteDefinition.AttachPoint attachPoint = array[j];
			bool flag = false;
			for (int k = 0; k < this.attachPoints.Count; k++)
			{
				Transform transform = this.attachPoints[k];
				int num2 = this.attachPoints.IndexOf(transform);
				if (transform != null && this.attachPointNames[k] == attachPoint.name)
				{
					if (this.deactivateUnusedAttachPoints || this.centerUnusedAttachPoints || this.disableEmissionOnUnusedParticleSystems)
					{
						tk2dSpriteAttachPoint.attachPointUpdated[num2] = true;
					}
					this.UpdateAttachPointTransform(attachPoint, transform);
					flag = true;
				}
			}
			if (!flag)
			{
				GameObject gameObject = new GameObject(attachPoint.name);
				Transform transform2 = gameObject.transform;
				transform2.parent = base.transform;
				this.UpdateAttachPointTransform(attachPoint, transform2);
				this.attachPoints.Add(transform2);
			}
		}
		if (this.centerUnusedAttachPoints)
		{
			for (int l = 0; l < tk2dSpriteAttachPoint.attachPointUpdated.Length; l++)
			{
				if (l < this.attachPoints.Count)
				{
					if (this.attachPoints[l] != null)
					{
						GameObject gameObject2 = this.attachPoints[l].gameObject;
						if (!tk2dSpriteAttachPoint.attachPointUpdated[l] && gameObject2.activeSelf)
						{
							gameObject2.transform.position = spr.WorldCenter.ToVector3ZUp(gameObject2.transform.position.z);
						}
					}
				}
			}
		}
		if (this.disableEmissionOnUnusedParticleSystems)
		{
			for (int m = 0; m < tk2dSpriteAttachPoint.attachPointUpdated.Length; m++)
			{
				if (m < this.attachPoints.Count)
				{
					if (!tk2dSpriteAttachPoint.attachPointUpdated[m] && this.attachPoints[m] != null && this.attachPoints[m].gameObject)
					{
						ParticleSystem component = this.attachPoints[m].gameObject.GetComponent<ParticleSystem>();
						if (component)
						{
							BraveUtility.EnableEmission(component, false);
						}
					}
				}
			}
		}
		if (this.deactivateUnusedAttachPoints)
		{
			for (int n = 0; n < this.attachPoints.Count; n++)
			{
				if (this.attachPoints[n] != null)
				{
					GameObject gameObject3 = this.attachPoints[n].gameObject;
					if (tk2dSpriteAttachPoint.attachPointUpdated[n] && !gameObject3.activeSelf)
					{
						gameObject3.SetActive(true);
					}
					else if (!tk2dSpriteAttachPoint.attachPointUpdated[n] && gameObject3.activeSelf)
					{
						gameObject3.SetActive(false);
					}
				}
				tk2dSpriteAttachPoint.attachPointUpdated[n] = false;
			}
		}
	}

	// Token: 0x04003201 RID: 12801
	private tk2dBaseSprite sprite;

	// Token: 0x04003202 RID: 12802
	public List<Transform> attachPoints = new List<Transform>();

	// Token: 0x04003203 RID: 12803
	private static bool[] attachPointUpdated = new bool[32];

	// Token: 0x04003204 RID: 12804
	public bool deactivateUnusedAttachPoints;

	// Token: 0x04003205 RID: 12805
	public bool disableEmissionOnUnusedParticleSystems;

	// Token: 0x04003206 RID: 12806
	public bool ignorePosition;

	// Token: 0x04003207 RID: 12807
	public bool ignoreScale;

	// Token: 0x04003208 RID: 12808
	public bool ignoreRotation;

	// Token: 0x04003209 RID: 12809
	public bool centerUnusedAttachPoints;

	// Token: 0x0400320A RID: 12810
	private List<string> attachPointNames = new List<string>();

	// Token: 0x0400320B RID: 12811
	private static tk2dSpriteDefinition.AttachPoint[] emptyAttachPointArray = null;
}
