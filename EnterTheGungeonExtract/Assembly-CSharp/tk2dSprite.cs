using System;
using UnityEngine;

// Token: 0x02000BB0 RID: 2992
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dSprite : tk2dBaseSprite
{
	// Token: 0x06003F4B RID: 16203 RVA: 0x00140814 File Offset: 0x0013EA14
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.MarkDynamic();
		this.mesh.hideFlags = HideFlags.DontSave;
		this.m_filter = base.GetComponent<MeshFilter>();
		this.m_filter.mesh = this.mesh;
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x001408A8 File Offset: 0x0013EAA8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
		if (this.meshColliderMesh)
		{
			UnityEngine.Object.Destroy(this.meshColliderMesh);
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x001408E8 File Offset: 0x0013EAE8
	public override void Build()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.meshVertices == null || this.meshVertices.Length != 4 || this.meshColors == null || this.meshColors.Length != 4)
		{
			this.meshVertices = new Vector3[4];
			this.meshColors = new Color32[4];
		}
		this.meshNormals = tk2dSprite.m_defaultNormalArray;
		this.meshTangents = tk2dSprite.m_defaultTangentArray;
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.MarkDynamic();
			this.mesh.hideFlags = HideFlags.DontSave;
			base.GetComponent<MeshFilter>().mesh = this.mesh;
		}
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		if (this.GenerateUV2)
		{
			if (this.LockUV2OnFrameOne)
			{
				if (!this.m_hasGeneratedLockedUV2)
				{
					this.m_hasGeneratedLockedUV2 = true;
					this.mesh.uv2 = tk2dSpriteDefinition.uvs;
				}
			}
			else if (base.spriteAnimator != null && base.spriteAnimator.IsFrameBlendedAnimation)
			{
				this.mesh.uv2 = base.spriteAnimator.GetNextFrameUVs();
			}
			else
			{
				this.mesh.uv2 = tk2dSprite.m_defaultUvs;
			}
		}
		this.mesh.triangles = tk2dSpriteDefinition.indices;
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
		this.UpdateMaterial();
		this.CreateCollider();
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x00140AF4 File Offset: 0x0013ECF4
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x00140B00 File Offset: 0x0013ED00
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x00140B0C File Offset: 0x0013ED0C
	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x00140B18 File Offset: 0x0013ED18
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x00140B20 File Offset: 0x0013ED20
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x00140B28 File Offset: 0x0013ED28
	protected override void UpdateVertices()
	{
		this.UpdateVerticesImpl();
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x00140B30 File Offset: 0x0013ED30
	protected void UpdateColorsImpl()
	{
		if (this.mesh == null || this.meshColors == null || this.meshColors.Length == 0)
		{
			return;
		}
		base.SetColors(this.meshColors);
		this.mesh.colors32 = this.meshColors;
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x00140B84 File Offset: 0x0013ED84
	protected void UpdateVerticesImpl()
	{
		if (this.mesh == null || this.meshVertices == null || this.meshVertices.Length == 0 || !this.collectionInst || this.collectionInst.spriteDefinitions == null)
		{
			return;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		this.meshNormals = tk2dSprite.m_defaultNormalArray;
		this.meshTangents = tk2dSprite.m_defaultTangentArray;
		if (!this.StaticPositions || !this.hasSetPositions)
		{
			base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
			this.hasSetPositions = true;
		}
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		if (this.GenerateUV2)
		{
			if (this.LockUV2OnFrameOne)
			{
				if (!this.m_hasGeneratedLockedUV2)
				{
					this.m_hasGeneratedLockedUV2 = true;
					this.mesh.uv2 = tk2dSpriteDefinition.uvs;
				}
			}
			else if (base.spriteAnimator && base.spriteAnimator.IsFrameBlendedAnimation)
			{
				this.mesh.uv2 = base.spriteAnimator.GetNextFrameUVs();
			}
			else
			{
				this.mesh.uv2 = tk2dSprite.m_defaultUvs;
			}
		}
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x00140D24 File Offset: 0x0013EF24
	protected void UpdateGeometryImpl()
	{
		if (this.mesh == null)
		{
			return;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.meshVertices == null || this.meshVertices.Length != 4)
		{
			this.meshVertices = new Vector3[4];
			this.meshNormals = tk2dSprite.m_defaultNormalArray;
			this.meshTangents = tk2dSprite.m_defaultTangentArray;
			this.meshColors = new Color32[4];
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		if (this.GenerateUV2)
		{
			if (this.LockUV2OnFrameOne)
			{
				if (!this.m_hasGeneratedLockedUV2)
				{
					this.m_hasGeneratedLockedUV2 = true;
					this.mesh.uv2 = tk2dSpriteDefinition.uvs;
				}
			}
			else if (base.spriteAnimator.IsFrameBlendedAnimation)
			{
				this.mesh.uv2 = base.spriteAnimator.GetNextFrameUVs();
			}
			else
			{
				this.mesh.uv2 = tk2dSprite.m_defaultUvs;
			}
		}
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(base.GetBounds(), this.renderLayer);
		this.mesh.triangles = tk2dSpriteDefinition.indices;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x00140EC8 File Offset: 0x0013F0C8
	protected void CopyPropertyBlock(Material source, Material dest)
	{
		if (dest.HasProperty(tk2dSprite.m_shaderEmissivePowerID) && source.HasProperty(tk2dSprite.m_shaderEmissivePowerID))
		{
			dest.SetFloat(tk2dSprite.m_shaderEmissivePowerID, source.GetFloat(tk2dSprite.m_shaderEmissivePowerID));
		}
		if (dest.HasProperty(tk2dSprite.m_shaderEmissiveColorPowerID) && source.HasProperty(tk2dSprite.m_shaderEmissiveColorPowerID))
		{
			dest.SetFloat(tk2dSprite.m_shaderEmissiveColorPowerID, source.GetFloat(tk2dSprite.m_shaderEmissiveColorPowerID));
		}
		if (dest.HasProperty(tk2dSprite.m_shaderEmissiveColorID) && source.HasProperty(tk2dSprite.m_shaderEmissiveColorID))
		{
			dest.SetColor(tk2dSprite.m_shaderEmissiveColorID, source.GetColor(tk2dSprite.m_shaderEmissiveColorID));
		}
		if (dest.HasProperty(tk2dSprite.m_shaderThresholdID) && source.HasProperty(tk2dSprite.m_shaderThresholdID))
		{
			dest.SetFloat(tk2dSprite.m_shaderThresholdID, source.GetFloat(tk2dSprite.m_shaderThresholdID));
		}
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x00140FB0 File Offset: 0x0013F1B0
	protected override void UpdateMaterial()
	{
		if (!base.renderer)
		{
			return;
		}
		if (tk2dSprite.m_shaderEmissiveColorID == -1)
		{
			tk2dSprite.m_shaderEmissivePowerID = Shader.PropertyToID("_EmissivePower");
			tk2dSprite.m_shaderEmissiveColorPowerID = Shader.PropertyToID("_EmissiveColorPower");
			tk2dSprite.m_shaderEmissiveColorID = Shader.PropertyToID("_EmissiveColor");
			tk2dSprite.m_shaderThresholdID = Shader.PropertyToID("_EmissiveThresholdSensitivity");
		}
		if (this.OverrideMaterialMode != tk2dBaseSprite.SpriteMaterialOverrideMode.NONE && base.renderer.sharedMaterial != null)
		{
			if (this.OverrideMaterialMode == tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE)
			{
				Material materialInst = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
				Material sharedMaterial = base.renderer.sharedMaterial;
				if (sharedMaterial != materialInst)
				{
					sharedMaterial.mainTexture = materialInst.mainTexture;
					if (this.ApplyEmissivePropertyBlock)
					{
						this.CopyPropertyBlock(materialInst, sharedMaterial);
					}
				}
				return;
			}
			if (this.OverrideMaterialMode == tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX)
			{
				return;
			}
		}
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x001410F0 File Offset: 0x0013F2F0
	protected override int GetCurrentVertexCount()
	{
		if (this.meshVertices == null)
		{
			return 0;
		}
		return this.meshVertices.Length;
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x00141108 File Offset: 0x0013F308
	public override void ForceBuild()
	{
		if (!this)
		{
			return;
		}
		base.ForceBuild();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x00141130 File Offset: 0x0013F330
	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 vector = Vector3.Scale(currentSprite.untrimmedBoundsDataCenter - 0.5f * currentSprite.untrimmedBoundsDataExtents, this._scale);
		Vector3 vector2 = Vector3.Scale(currentSprite.untrimmedBoundsDataExtents, this._scale);
		Vector3 vector3 = vector2 + dMax - dMin;
		vector3.x /= currentSprite.untrimmedBoundsDataExtents.x;
		vector3.y /= currentSprite.untrimmedBoundsDataExtents.y;
		Vector3 vector4 = new Vector3((!Mathf.Approximately(this._scale.x, 0f)) ? (vector.x * vector3.x / this._scale.x) : 0f, (!Mathf.Approximately(this._scale.y, 0f)) ? (vector.y * vector3.y / this._scale.y) : 0f);
		Vector3 vector5 = vector + dMin - vector4;
		vector5.z = 0f;
		base.transform.position = base.transform.TransformPoint(vector5);
		base.scale = new Vector3(vector3.x, vector3.y, this._scale.z);
	}

	// Token: 0x04003184 RID: 12676
	private Mesh mesh;

	// Token: 0x04003185 RID: 12677
	private Vector3[] meshVertices;

	// Token: 0x04003186 RID: 12678
	private Vector3[] meshNormals;

	// Token: 0x04003187 RID: 12679
	private Vector4[] meshTangents;

	// Token: 0x04003188 RID: 12680
	private Color32[] meshColors;

	// Token: 0x04003189 RID: 12681
	private MeshFilter m_filter;

	// Token: 0x0400318A RID: 12682
	public bool ApplyEmissivePropertyBlock;

	// Token: 0x0400318B RID: 12683
	public bool GenerateUV2;

	// Token: 0x0400318C RID: 12684
	public bool LockUV2OnFrameOne;

	// Token: 0x0400318D RID: 12685
	public bool StaticPositions;

	// Token: 0x0400318E RID: 12686
	[NonSerialized]
	private bool m_hasGeneratedLockedUV2;

	// Token: 0x0400318F RID: 12687
	private static Vector3[] m_defaultNormalArray = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, 1f, -1f)
	};

	// Token: 0x04003190 RID: 12688
	private static Vector4[] m_defaultTangentArray = new Vector4[]
	{
		new Vector4(1f, 0f, 0f, 1f),
		new Vector4(1f, 0f, 0f, 1f),
		new Vector4(1f, 0f, 0f, 1f),
		new Vector4(1f, 0f, 0f, 1f)
	};

	// Token: 0x04003191 RID: 12689
	private bool hasSetPositions;

	// Token: 0x04003192 RID: 12690
	private static Vector2[] m_defaultUvs = new Vector2[]
	{
		Vector2.zero,
		Vector2.right,
		Vector2.up,
		Vector2.one
	};

	// Token: 0x04003193 RID: 12691
	private static int m_shaderEmissivePowerID = -1;

	// Token: 0x04003194 RID: 12692
	private static int m_shaderEmissiveColorPowerID = -1;

	// Token: 0x04003195 RID: 12693
	private static int m_shaderEmissiveColorID = -1;

	// Token: 0x04003196 RID: 12694
	private static int m_shaderThresholdID = -1;
}
