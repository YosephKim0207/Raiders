using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001540 RID: 5440
public class SpriteShadow
{
	// Token: 0x06007C85 RID: 31877 RVA: 0x00322224 File Offset: 0x00320424
	public SpriteShadow(tk2dSprite sprite, SpriteShadowCaster caster)
	{
		this.shadowedSprite = sprite;
		this.m_caster = caster;
		this.m_casterTransform = caster.transform;
		this.shadowDepth = caster.shadowDepth;
		this.vertices = new List<Vector3>();
		this.m_spriteMesh = sprite.GetComponent<MeshFilter>().sharedMesh;
		this.spriteTransform = sprite.transform;
		this.spriteAnimator = sprite.GetComponent<tk2dSpriteAnimator>();
		this.cachedPosition = this.spriteTransform.position;
		if (this.spriteAnimator != null && this.spriteAnimator.CurrentClip != null)
		{
			this.cachedClip = this.spriteAnimator.CurrentClip;
			this.cachedFrame = this.spriteAnimator.CurrentFrame;
		}
		this.m_shadowObject = new GameObject("Shadow");
		this.m_shadowFilter = this.m_shadowObject.AddComponent<MeshFilter>();
		this.m_shadowRenderer = this.m_shadowObject.AddComponent<MeshRenderer>();
		this.m_shadowRenderer.sharedMaterial = this.m_caster.GetMaterialInstance();
		Texture mainTexture = this.shadowedSprite.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
		this.m_shadowRenderer.sharedMaterial.mainTexture = mainTexture;
		this.m_shadowMesh = new Mesh();
		this.m_shadowMesh.vertices = new Vector3[10];
		this.m_shadowMesh.triangles = new int[]
		{
			0, 3, 1, 2, 3, 0, 2, 5, 3, 4,
			5, 2, 4, 7, 5, 6, 7, 4, 6, 9,
			7, 8, 9, 6
		};
		this.m_shadowMesh.uv = new Vector2[10];
		this.UpdateShadow(true);
	}

	// Token: 0x06007C86 RID: 31878 RVA: 0x003223A8 File Offset: 0x003205A8
	public void Destroy()
	{
		UnityEngine.Object.Destroy(this.m_shadowObject);
	}

	// Token: 0x06007C87 RID: 31879 RVA: 0x003223B8 File Offset: 0x003205B8
	private Vector3 CollapseDepth(Vector3 input)
	{
		return new Vector3(input.x, input.y, this.shadowDepth);
	}

	// Token: 0x06007C88 RID: 31880 RVA: 0x003223D4 File Offset: 0x003205D4
	public void UpdateShadow(bool force = false)
	{
		if (!force)
		{
			bool flag = this.cachedPosition != this.spriteTransform.position;
			bool flag2 = false;
			if (this.spriteAnimator != null && this.spriteAnimator.CurrentClip != null)
			{
				if (this.cachedClip != this.spriteAnimator.CurrentClip)
				{
					flag2 = true;
				}
				if (this.cachedFrame != this.spriteAnimator.CurrentFrame)
				{
					flag2 = true;
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
		}
		float x = this.shadowedSprite.GetBounds().size.x;
		float y = this.shadowedSprite.GetBounds().size.y;
		Vector3 vector = this.CollapseDepth(this.m_casterTransform.position);
		Vector3 vector2 = this.CollapseDepth(this.spriteTransform.position);
		Vector3 vector3 = vector2 + new Vector3(x / 2f, 0f, 0f);
		float magnitude = (vector3 - vector).magnitude;
		Vector3 vector4 = vector2;
		Vector3 vector5 = vector4 - vector;
		Vector3 vector6 = vector2 + new Vector3(x, 0f, 0f);
		Vector3 vector7 = vector6 - vector;
		Ray ray = new Ray(vector, vector5);
		Ray ray2 = new Ray(vector, vector7);
		Vector3 point = ray.GetPoint(magnitude);
		Vector3 point2 = ray2.GetPoint(magnitude);
		Vector3 point3 = ray.GetPoint(magnitude + y * (magnitude / this.m_caster.radius * 4f));
		Vector3 point4 = ray2.GetPoint(magnitude + y * (magnitude / this.m_caster.radius * 4f));
		this.vertices.Clear();
		this.vertices.Add(point);
		this.vertices.Add(point2);
		this.vertices.Add(point + (point3 - point) / 4f);
		this.vertices.Add(point2 + (point4 - point2) / 4f);
		this.vertices.Add((point + point3) / 2f);
		this.vertices.Add((point2 + point4) / 2f);
		this.vertices.Add(point * 0.25f + point3 * 0.75f);
		this.vertices.Add(point2 * 0.25f + point4 * 0.75f);
		this.vertices.Add(point3);
		this.vertices.Add(point4);
		this.hasChanged = true;
		if (vector.y > vector2.y)
		{
			this.m_shadowMesh.triangles = new int[]
			{
				0, 1, 3, 2, 0, 3, 2, 3, 5, 4,
				2, 5, 4, 5, 7, 6, 4, 7, 6, 7,
				9, 8, 6, 9
			};
		}
		else
		{
			this.m_shadowMesh.triangles = new int[]
			{
				0, 3, 1, 2, 3, 0, 2, 5, 3, 4,
				5, 2, 4, 7, 5, 6, 7, 4, 6, 9,
				7, 8, 9, 6
			};
		}
		this.RebuildMesh();
		this.cachedPosition = this.spriteTransform.position;
		if (this.spriteAnimator != null && this.spriteAnimator.CurrentClip != null)
		{
			this.cachedClip = this.spriteAnimator.CurrentClip;
			this.cachedFrame = this.spriteAnimator.CurrentFrame;
		}
	}

	// Token: 0x06007C89 RID: 31881 RVA: 0x00322760 File Offset: 0x00320960
	private void RebuildMesh()
	{
		this.m_shadowMesh.vertices = this.vertices.ToArray();
		Vector2[] uv = this.m_spriteMesh.uv;
		Vector2[] array = new Vector2[10];
		array[0] = uv[0];
		array[1] = uv[1];
		array[4] = (uv[0] + uv[2]) / 2f;
		array[5] = (uv[1] + uv[3]) / 2f;
		array[2] = (uv[0] + array[4]) / 2f;
		array[3] = (uv[1] + array[5]) / 2f;
		array[6] = uv[0] + (uv[2] - uv[0]) * 0.75f;
		array[7] = uv[1] + (uv[3] - uv[1]) * 0.75f;
		array[8] = uv[2];
		array[9] = uv[3];
		this.m_shadowMesh.uv = array;
		this.m_shadowMesh.RecalculateBounds();
		this.m_shadowFilter.sharedMesh = this.m_shadowMesh;
	}

	// Token: 0x04007F74 RID: 32628
	public bool hasChanged;

	// Token: 0x04007F75 RID: 32629
	public List<Vector3> vertices;

	// Token: 0x04007F76 RID: 32630
	public tk2dSprite shadowedSprite;

	// Token: 0x04007F77 RID: 32631
	private SpriteShadowCaster m_caster;

	// Token: 0x04007F78 RID: 32632
	private Transform m_casterTransform;

	// Token: 0x04007F79 RID: 32633
	private tk2dSpriteAnimator spriteAnimator;

	// Token: 0x04007F7A RID: 32634
	private Transform spriteTransform;

	// Token: 0x04007F7B RID: 32635
	private Vector3 cachedPosition;

	// Token: 0x04007F7C RID: 32636
	private tk2dSpriteAnimationClip cachedClip;

	// Token: 0x04007F7D RID: 32637
	private int cachedFrame;

	// Token: 0x04007F7E RID: 32638
	private float shadowDepth;

	// Token: 0x04007F7F RID: 32639
	private GameObject m_shadowObject;

	// Token: 0x04007F80 RID: 32640
	private MeshFilter m_shadowFilter;

	// Token: 0x04007F81 RID: 32641
	private MeshRenderer m_shadowRenderer;

	// Token: 0x04007F82 RID: 32642
	private Mesh m_shadowMesh;

	// Token: 0x04007F83 RID: 32643
	private Mesh m_spriteMesh;
}
