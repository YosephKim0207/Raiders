using System;
using System.Collections.Generic;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000BE3 RID: 3043
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dStaticSpriteBatcher")]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dStaticSpriteBatcher : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x0600407D RID: 16509 RVA: 0x00149798 File Offset: 0x00147998
	public bool CheckFlag(tk2dStaticSpriteBatcher.Flags mask)
	{
		return (this.flags & mask) != tk2dStaticSpriteBatcher.Flags.None;
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x001497A8 File Offset: 0x001479A8
	public void SetFlag(tk2dStaticSpriteBatcher.Flags mask, bool value)
	{
		if (this.CheckFlag(mask) != value)
		{
			if (value)
			{
				this.flags |= mask;
			}
			else
			{
				this.flags &= ~mask;
			}
			this.Build();
		}
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x001497E8 File Offset: 0x001479E8
	private void Awake()
	{
		this.Build();
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x001497F0 File Offset: 0x001479F0
	private bool UpgradeData()
	{
		if (this.version == tk2dStaticSpriteBatcher.CURRENT_VERSION)
		{
			return false;
		}
		if (this._scale == Vector3.zero)
		{
			this._scale = Vector3.one;
		}
		if (this.version < 2 && this.batchedSprites != null)
		{
			foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
			{
				tk2dBatchedSprite.parentId = -1;
			}
		}
		if (this.version < 3)
		{
			if (this.batchedSprites != null)
			{
				foreach (tk2dBatchedSprite tk2dBatchedSprite2 in this.batchedSprites)
				{
					if (tk2dBatchedSprite2.spriteId == -1)
					{
						tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.EmptyGameObject;
					}
					else
					{
						tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.Sprite;
						if (tk2dBatchedSprite2.spriteCollection == null)
						{
							tk2dBatchedSprite2.spriteCollection = this.spriteCollection;
						}
					}
				}
				this.UpdateMatrices();
			}
			this.spriteCollection = null;
		}
		this.version = tk2dStaticSpriteBatcher.CURRENT_VERSION;
		return true;
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00149900 File Offset: 0x00147B00
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
		if (this.colliderMesh)
		{
			UnityEngine.Object.Destroy(this.colliderMesh);
		}
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x00149938 File Offset: 0x00147B38
	public void UpdateMatrices()
	{
		bool flag = false;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (tk2dBatchedSprite.parentId != -1)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>(this.batchedSprites);
			list.Sort((tk2dBatchedSprite a, tk2dBatchedSprite b) => a.parentId.CompareTo(b.parentId));
			foreach (tk2dBatchedSprite tk2dBatchedSprite2 in list)
			{
				matrix4x.SetTRS(tk2dBatchedSprite2.position, tk2dBatchedSprite2.rotation, tk2dBatchedSprite2.localScale);
				tk2dBatchedSprite2.relativeMatrix = ((tk2dBatchedSprite2.parentId != -1) ? this.batchedSprites[tk2dBatchedSprite2.parentId].relativeMatrix : Matrix4x4.identity) * matrix4x;
			}
		}
		else
		{
			foreach (tk2dBatchedSprite tk2dBatchedSprite3 in this.batchedSprites)
			{
				tk2dBatchedSprite3.relativeMatrix.SetTRS(tk2dBatchedSprite3.position, tk2dBatchedSprite3.rotation, tk2dBatchedSprite3.localScale);
			}
		}
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00149AA0 File Offset: 0x00147CA0
	public void Build()
	{
		this.UpgradeData();
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
			base.GetComponent<MeshFilter>().mesh = this.mesh;
		}
		else
		{
			this.mesh.Clear();
		}
		if (this.colliderMesh)
		{
			UnityEngine.Object.Destroy(this.colliderMesh);
			this.colliderMesh = null;
		}
		if (this.batchedSprites != null && this.batchedSprites.Length != 0)
		{
			this.SortBatchedSprites();
			this.BuildRenderMesh();
			this.BuildPhysicsMesh();
		}
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x00149B50 File Offset: 0x00147D50
	private void SortBatchedSprites()
	{
		List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list2 = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list3 = new List<tk2dBatchedSprite>();
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				list3.Add(tk2dBatchedSprite);
			}
			else
			{
				Material material = this.GetMaterial(tk2dBatchedSprite);
				if (material != null)
				{
					if (material.renderQueue == 2000)
					{
						list.Add(tk2dBatchedSprite);
					}
					else
					{
						list2.Add(tk2dBatchedSprite);
					}
				}
				else
				{
					list.Add(tk2dBatchedSprite);
				}
			}
		}
		List<tk2dBatchedSprite> list4 = new List<tk2dBatchedSprite>(list.Count + list2.Count + list3.Count);
		list4.AddRange(list);
		list4.AddRange(list2);
		list4.AddRange(list3);
		Dictionary<tk2dBatchedSprite, int> dictionary = new Dictionary<tk2dBatchedSprite, int>();
		int num = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in list4)
		{
			dictionary[tk2dBatchedSprite2] = num++;
		}
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in list4)
		{
			if (tk2dBatchedSprite3.parentId != -1)
			{
				tk2dBatchedSprite3.parentId = dictionary[this.batchedSprites[tk2dBatchedSprite3.parentId]];
			}
		}
		this.batchedSprites = list4.ToArray();
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x00149D04 File Offset: 0x00147F04
	private Material GetMaterial(tk2dBatchedSprite bs)
	{
		tk2dBatchedSprite.Type type = bs.type;
		if (type == tk2dBatchedSprite.Type.EmptyGameObject)
		{
			return null;
		}
		if (type != tk2dBatchedSprite.Type.TextMesh)
		{
			return bs.GetSpriteDefinition().materialInst;
		}
		return this.allTextMeshData[bs.xRefId].font.materialInst;
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x00149D50 File Offset: 0x00147F50
	private void BuildRenderMesh()
	{
		List<Material> list = new List<Material>();
		List<List<int>> list2 = new List<List<int>>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = this.CheckFlag(tk2dStaticSpriteBatcher.Flags.FlattenDepth);
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			if (spriteDefinition != null)
			{
				flag |= spriteDefinition.normals != null && spriteDefinition.normals.Length > 0;
				flag2 |= spriteDefinition.tangents != null && spriteDefinition.tangents.Length > 0;
			}
			if (tk2dBatchedSprite.type == tk2dBatchedSprite.Type.TextMesh)
			{
				tk2dTextMeshData tk2dTextMeshData = this.allTextMeshData[tk2dBatchedSprite.xRefId];
				if (tk2dTextMeshData.font != null && tk2dTextMeshData.font.inst.textureGradients)
				{
					flag3 = true;
				}
			}
		}
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		int num = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in this.batchedSprites)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition2 = tk2dBatchedSprite2.GetSpriteDefinition();
			int num2 = 0;
			int num3 = 0;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSpriteGeomDesc(out num2, out num3, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num2, out num3, spriteDefinition2, tk2dBatchedSprite2.Dimensions);
				}
				break;
			case tk2dBatchedSprite.Type.SlicedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSlicedSpriteGeomDesc(out num2, out num3, spriteDefinition2, tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly), false, Vector2.zero, Vector2.zero, Vector2.zero, 0f);
				}
				break;
			case tk2dBatchedSprite.Type.ClippedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetClippedSpriteGeomDesc(out num2, out num3, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TextMesh:
			{
				tk2dTextMeshData tk2dTextMeshData2 = this.allTextMeshData[tk2dBatchedSprite2.xRefId];
				tk2dTextGeomGen.GetTextMeshGeomDesc(out num2, out num3, tk2dTextGeomGen.Data(tk2dTextMeshData2, tk2dTextMeshData2.font.inst, tk2dBatchedSprite2.FormattedText));
				break;
			}
			}
			num += num2;
			list3.Add(num2);
			list4.Add(num3);
		}
		Vector3[] array3 = ((!flag) ? null : new Vector3[num]);
		Vector4[] array4 = ((!flag2) ? null : new Vector4[num]);
		Vector3[] array5 = new Vector3[num];
		Color32[] array6 = new Color32[num];
		Vector2[] array7 = new Vector2[num];
		Vector2[] array8 = ((!flag3) ? null : new Vector2[num]);
		int num4 = 0;
		Material material = null;
		List<int> list5 = null;
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = this._scale.x;
		identity.m11 = this._scale.y;
		identity.m22 = this._scale.z;
		int num5 = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in this.batchedSprites)
		{
			if (!tk2dBatchedSprite3.IsDrawn)
			{
				break;
			}
			if (tk2dBatchedSprite3.type == tk2dBatchedSprite.Type.EmptyGameObject)
			{
				num5++;
			}
			else
			{
				tk2dSpriteDefinition spriteDefinition3 = tk2dBatchedSprite3.GetSpriteDefinition();
				int num6 = list3[num5];
				int num7 = list4[num5];
				Material material2 = this.GetMaterial(tk2dBatchedSprite3);
				if (material2 != material)
				{
					if (material != null)
					{
						list.Add(material);
						list2.Add(list5);
					}
					material = material2;
					list5 = new List<int>();
				}
				Vector3[] array10 = new Vector3[num6];
				Vector2[] array11 = new Vector2[num6];
				Vector2[] array12 = ((!flag3) ? null : new Vector2[num6]);
				Color32[] array13 = new Color32[num6];
				Vector3[] array14 = ((!flag) ? null : new Vector3[num6]);
				Vector4[] array15 = ((!flag2) ? null : new Vector4[num6]);
				int[] array16 = new int[num7];
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				switch (tk2dBatchedSprite3.type)
				{
				case tk2dBatchedSprite.Type.Sprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetSpriteGeom(array10, array11, array14, array15, 0, spriteDefinition3, Vector3.one);
						tk2dSpriteGeomGen.SetSpriteIndices(array16, 0, num4, spriteDefinition3);
					}
					break;
				case tk2dBatchedSprite.Type.TiledSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetTiledSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, Vector3.one, tk2dBatchedSprite3.Dimensions, tk2dBatchedSprite3.anchor, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ);
						tk2dSpriteGeomGen.SetTiledSpriteIndices(array16, 0, num4, spriteDefinition3, tk2dBatchedSprite3.Dimensions, null);
					}
					break;
				case tk2dBatchedSprite.Type.SlicedSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetSlicedSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, tk2dBatchedSprite3.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly), Vector3.one, tk2dBatchedSprite3.Dimensions, tk2dBatchedSprite3.SlicedSpriteBorderBottomLeft, tk2dBatchedSprite3.SlicedSpriteBorderTopRight, 0f, tk2dBatchedSprite3.anchor, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ, Vector2.zero, false);
						tk2dSpriteGeomGen.SetSlicedSpriteIndices(array16, 0, num4, spriteDefinition3, tk2dBatchedSprite3.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly), false, Vector2.zero, Vector2.zero, Vector2.zero, 0f);
					}
					break;
				case tk2dBatchedSprite.Type.ClippedSprite:
					if (spriteDefinition3 != null)
					{
						tk2dSpriteGeomGen.SetClippedSpriteGeom(array10, array11, 0, out zero, out zero2, spriteDefinition3, Vector3.one, tk2dBatchedSprite3.ClippedSpriteRegionBottomLeft, tk2dBatchedSprite3.ClippedSpriteRegionTopRight, tk2dBatchedSprite3.BoxColliderOffsetZ, tk2dBatchedSprite3.BoxColliderExtentZ);
						tk2dSpriteGeomGen.SetClippedSpriteIndices(array16, 0, num4, spriteDefinition3);
					}
					break;
				case tk2dBatchedSprite.Type.TextMesh:
				{
					tk2dTextMeshData tk2dTextMeshData3 = this.allTextMeshData[tk2dBatchedSprite3.xRefId];
					tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(tk2dTextMeshData3, tk2dTextMeshData3.font.inst, tk2dBatchedSprite3.FormattedText);
					int num8 = tk2dTextGeomGen.SetTextMeshGeom(array10, array11, array12, array13, 0, geomData, int.MaxValue);
					if (!geomData.fontInst.isPacked)
					{
						Color32 color = tk2dTextMeshData3.color;
						Color32 color2 = ((!tk2dTextMeshData3.useGradient) ? tk2dTextMeshData3.color : tk2dTextMeshData3.color2);
						for (int l = 0; l < array13.Length; l++)
						{
							Color32 color3 = ((l % 4 >= 2) ? color2 : color);
							byte b = array13[l].r * color3.r / byte.MaxValue;
							byte b2 = array13[l].g * color3.g / byte.MaxValue;
							byte b3 = array13[l].b * color3.b / byte.MaxValue;
							byte b4 = array13[l].a * color3.a / byte.MaxValue;
							if (geomData.fontInst.premultipliedAlpha)
							{
								b = b * b4 / byte.MaxValue;
								b2 = b2 * b4 / byte.MaxValue;
								b3 = b3 * b4 / byte.MaxValue;
							}
							array13[l] = new Color32(b, b2, b3, b4);
						}
					}
					tk2dTextGeomGen.SetTextMeshIndices(array16, 0, num4, geomData, num8);
					break;
				}
				}
				tk2dBatchedSprite3.CachedBoundsCenter = zero;
				tk2dBatchedSprite3.CachedBoundsExtents = zero2;
				if (num6 > 0 && tk2dBatchedSprite3.type != tk2dBatchedSprite.Type.TextMesh)
				{
					bool flag5 = tk2dBatchedSprite3.spriteCollection != null && tk2dBatchedSprite3.spriteCollection.premultipliedAlpha;
					tk2dSpriteGeomGen.SetSpriteColors(array13, 0, num6, tk2dBatchedSprite3.color, flag5);
				}
				Matrix4x4 matrix4x = identity * tk2dBatchedSprite3.relativeMatrix;
				for (int m = 0; m < num6; m++)
				{
					Vector3 vector = Vector3.Scale(array10[m], tk2dBatchedSprite3.baseScale);
					vector = matrix4x.MultiplyPoint(vector);
					if (flag4)
					{
						vector.z = 0f;
					}
					array5[num4 + m] = vector;
					array7[num4 + m] = array11[m];
					if (flag3)
					{
						array8[num4 + m] = array12[m];
					}
					array6[num4 + m] = array13[m];
					if (flag)
					{
						array3[num4 + m] = tk2dBatchedSprite3.rotation * array14[m];
					}
					if (flag2)
					{
						Vector3 vector2 = new Vector3(array15[m].x, array15[m].y, array15[m].z);
						vector2 = tk2dBatchedSprite3.rotation * vector2;
						array4[num4 + m] = new Vector4(vector2.x, vector2.y, vector2.z, array15[m].w);
					}
				}
				list5.AddRange(array16);
				num4 += num6;
				num5++;
			}
		}
		if (list5 != null)
		{
			list.Add(material);
			list2.Add(list5);
		}
		if (this.mesh)
		{
			this.mesh.vertices = array5;
			this.mesh.uv = array7;
			if (flag3)
			{
				this.mesh.uv2 = array8;
			}
			this.mesh.colors32 = array6;
			if (flag)
			{
				this.mesh.normals = array3;
			}
			if (flag2)
			{
				this.mesh.tangents = array4;
			}
			this.mesh.subMeshCount = list2.Count;
			for (int n = 0; n < list2.Count; n++)
			{
				this.mesh.SetTriangles(list2[n].ToArray(), n);
			}
			this.mesh.RecalculateBounds();
		}
		base.GetComponent<Renderer>().sharedMaterials = list.ToArray();
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x0014A75C File Offset: 0x0014895C
	private void BuildPhysicsMesh()
	{
		MeshCollider component = base.GetComponent<MeshCollider>();
		if (component != null)
		{
			if (base.GetComponent<Collider>() != component)
			{
				return;
			}
			if (!this.CheckFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider))
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		EdgeCollider2D[] components = base.GetComponents<EdgeCollider2D>();
		if (!this.CheckFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider))
		{
			foreach (EdgeCollider2D edgeCollider2D in components)
			{
				UnityEngine.Object.Destroy(edgeCollider2D);
			}
		}
		if (!this.CheckFlag(tk2dStaticSpriteBatcher.Flags.GenerateCollider))
		{
			return;
		}
		bool flag = this.CheckFlag(tk2dStaticSpriteBatcher.Flags.FlattenDepth);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag2 = true;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			bool flag3 = false;
			bool flag4 = false;
			switch (tk2dBatchedSprite.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag3 = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag4 = true;
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag4 = tk2dBatchedSprite.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				break;
			}
			if (!flag3)
			{
				if (flag4)
				{
					num += 36;
					num2 += 8;
					num3++;
				}
			}
			if (spriteDefinition.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
			{
				flag2 = false;
			}
		}
		if ((flag2 && num == 0) || (!flag2 && num3 == 0))
		{
			if (this.colliderMesh != null)
			{
				UnityEngine.Object.Destroy(this.colliderMesh);
				this.colliderMesh = null;
			}
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			foreach (EdgeCollider2D edgeCollider2D2 in components)
			{
				UnityEngine.Object.Destroy(edgeCollider2D2);
			}
			return;
		}
		if (flag2)
		{
			foreach (EdgeCollider2D edgeCollider2D3 in components)
			{
				UnityEngine.Object.Destroy(edgeCollider2D3);
			}
		}
		else
		{
			if (this.colliderMesh != null)
			{
				UnityEngine.Object.Destroy(this.colliderMesh);
			}
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		if (flag2)
		{
			this.BuildPhysicsMesh3D(component, flag, num2, num);
		}
		else
		{
			this.BuildPhysicsMesh2D(components, num3);
		}
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x0014A9EC File Offset: 0x00148BEC
	private void BuildPhysicsMesh2D(EdgeCollider2D[] edgeColliders, int numEdgeColliders)
	{
		for (int i = numEdgeColliders; i < edgeColliders.Length; i++)
		{
			UnityEngine.Object.Destroy(edgeColliders[i]);
		}
		Vector2[] array = new Vector2[5];
		if (numEdgeColliders > edgeColliders.Length)
		{
			EdgeCollider2D[] array2 = new EdgeCollider2D[numEdgeColliders];
			int num = Mathf.Min(numEdgeColliders, edgeColliders.Length);
			for (int j = 0; j < num; j++)
			{
				array2[j] = edgeColliders[j];
			}
			for (int k = num; k < numEdgeColliders; k++)
			{
				array2[k] = base.gameObject.AddComponent<EdgeCollider2D>();
			}
			edgeColliders = array2;
		}
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = this._scale.x;
		identity.m11 = this._scale.y;
		identity.m22 = this._scale.z;
		int num2 = 0;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			bool flag = false;
			bool flag2 = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			switch (tk2dBatchedSprite.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag2 = true;
					vector = spriteDefinition.colliderVertices[0];
					vector2 = spriteDefinition.colliderVertices[1];
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag2 = tk2dBatchedSprite.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				if (flag2)
				{
					vector = tk2dBatchedSprite.CachedBoundsCenter;
					vector2 = tk2dBatchedSprite.CachedBoundsExtents;
				}
				break;
			}
			Matrix4x4 matrix4x = identity * tk2dBatchedSprite.relativeMatrix;
			if (!flag)
			{
				if (flag2)
				{
					Vector3 vector3 = vector - vector2;
					Vector3 vector4 = vector + vector2;
					array[0] = matrix4x.MultiplyPoint(new Vector2(vector3.x, vector3.y));
					array[1] = matrix4x.MultiplyPoint(new Vector2(vector4.x, vector3.y));
					array[2] = matrix4x.MultiplyPoint(new Vector2(vector4.x, vector4.y));
					array[3] = matrix4x.MultiplyPoint(new Vector2(vector3.x, vector4.y));
					array[4] = array[0];
					edgeColliders[num2].points = array;
					num2++;
				}
			}
		}
	}

	// Token: 0x06004089 RID: 16521 RVA: 0x0014ACE8 File Offset: 0x00148EE8
	private void BuildPhysicsMesh3D(MeshCollider meshCollider, bool flattenDepth, int numVertices, int numIndices)
	{
		if (meshCollider == null)
		{
			meshCollider = base.gameObject.AddComponent<MeshCollider>();
		}
		if (this.colliderMesh == null)
		{
			this.colliderMesh = new Mesh();
			this.colliderMesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this.colliderMesh.Clear();
		}
		int num = 0;
		Vector3[] array = new Vector3[numVertices];
		int num2 = 0;
		int[] array2 = new int[numIndices];
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = this._scale.x;
		identity.m11 = this._scale.y;
		identity.m22 = this._scale.z;
		foreach (tk2dBatchedSprite tk2dBatchedSprite in this.batchedSprites)
		{
			if (!tk2dBatchedSprite.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite.GetSpriteDefinition();
			bool flag = false;
			bool flag2 = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			switch (tk2dBatchedSprite.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag2 = true;
					vector = spriteDefinition.colliderVertices[0];
					vector2 = spriteDefinition.colliderVertices[1];
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag2 = tk2dBatchedSprite.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				if (flag2)
				{
					vector = tk2dBatchedSprite.CachedBoundsCenter;
					vector2 = tk2dBatchedSprite.CachedBoundsExtents;
				}
				break;
			}
			Matrix4x4 matrix4x = identity * tk2dBatchedSprite.relativeMatrix;
			if (flattenDepth)
			{
				matrix4x.m23 = 0f;
			}
			if (!flag)
			{
				if (flag2)
				{
					tk2dSpriteGeomGen.SetBoxMeshData(array, array2, num, num2, num, vector, vector2, matrix4x, tk2dBatchedSprite.baseScale);
					num += 8;
					num2 += 36;
				}
			}
		}
		this.colliderMesh.vertices = array;
		this.colliderMesh.triangles = array2;
		meshCollider.sharedMesh = this.colliderMesh;
	}

	// Token: 0x0600408A RID: 16522 RVA: 0x0014AF1C File Offset: 0x0014911C
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.spriteCollection == spriteCollection;
	}

	// Token: 0x0600408B RID: 16523 RVA: 0x0014AF2C File Offset: 0x0014912C
	public void ForceBuild()
	{
		this.Build();
	}

	// Token: 0x0400336D RID: 13165
	public static int CURRENT_VERSION = 3;

	// Token: 0x0400336E RID: 13166
	public int version;

	// Token: 0x0400336F RID: 13167
	public tk2dBatchedSprite[] batchedSprites;

	// Token: 0x04003370 RID: 13168
	public tk2dTextMeshData[] allTextMeshData;

	// Token: 0x04003371 RID: 13169
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003372 RID: 13170
	[SerializeField]
	private tk2dStaticSpriteBatcher.Flags flags = tk2dStaticSpriteBatcher.Flags.GenerateCollider;

	// Token: 0x04003373 RID: 13171
	private Mesh mesh;

	// Token: 0x04003374 RID: 13172
	private Mesh colliderMesh;

	// Token: 0x04003375 RID: 13173
	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x02000BE4 RID: 3044
	public enum Flags
	{
		// Token: 0x04003378 RID: 13176
		None,
		// Token: 0x04003379 RID: 13177
		GenerateCollider,
		// Token: 0x0400337A RID: 13178
		FlattenDepth,
		// Token: 0x0400337B RID: 13179
		SortToCamera = 4
	}
}
