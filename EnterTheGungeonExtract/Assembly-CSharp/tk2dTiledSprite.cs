using System;
using UnityEngine;

// Token: 0x02000BE5 RID: 3045
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Sprite/tk2dTiledSprite")]
[RequireComponent(typeof(MeshFilter))]
public class tk2dTiledSprite : tk2dBaseSprite
{
	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x0600408F RID: 16527 RVA: 0x0014AF84 File Offset: 0x00149184
	// (set) Token: 0x06004090 RID: 16528 RVA: 0x0014AF8C File Offset: 0x0014918C
	public Vector2 dimensions
	{
		get
		{
			return this._dimensions;
		}
		set
		{
			if (value != this._dimensions)
			{
				this._dimensions = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06004091 RID: 16529 RVA: 0x0014AFB4 File Offset: 0x001491B4
	// (set) Token: 0x06004092 RID: 16530 RVA: 0x0014AFBC File Offset: 0x001491BC
	public tk2dBaseSprite.Anchor anchor
	{
		get
		{
			return this._anchor;
		}
		set
		{
			if (value != this._anchor)
			{
				this._anchor = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06004093 RID: 16531 RVA: 0x0014AFE0 File Offset: 0x001491E0
	// (set) Token: 0x06004094 RID: 16532 RVA: 0x0014AFE8 File Offset: 0x001491E8
	public bool CreateBoxCollider
	{
		get
		{
			return this._createBoxCollider;
		}
		set
		{
			if (this._createBoxCollider != value)
			{
				this._createBoxCollider = value;
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x0014B004 File Offset: 0x00149204
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
			if (this.boxCollider == null)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
			}
			if (this.boxCollider2D == null)
			{
				this.boxCollider2D = base.GetComponent<BoxCollider2D>();
			}
		}
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x0014B0BC File Offset: 0x001492BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x0014B0E0 File Offset: 0x001492E0
	protected new void SetColors(Color32[] dest)
	{
		int num;
		if (this.OverrideGetTiledSpriteGeomDesc != null)
		{
			int num2;
			this.OverrideGetTiledSpriteGeomDesc(out num, out num2, base.CurrentSprite, this.dimensions);
		}
		else
		{
			int num2;
			tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, base.CurrentSprite, this.dimensions);
		}
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, num, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x0014B148 File Offset: 0x00149348
	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		int num;
		int num2;
		if (this.OverrideGetTiledSpriteGeomDesc != null)
		{
			this.OverrideGetTiledSpriteGeomDesc(out num, out num2, currentSprite, this.dimensions);
		}
		else
		{
			tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, currentSprite, this.dimensions);
		}
		int num3 = num;
		if (this.meshUvs == null || this.meshUvs.Length < num)
		{
			num3 = BraveUtility.SmartListResizer((this.meshUvs != null) ? this.meshUvs.Length : 0, num, 100, 0);
			this.meshUvs = new Vector2[num3];
			this.meshVertices = new Vector3[num3];
			this.meshColors = new Color32[num3];
		}
		if (this.meshIndices == null || this.meshIndices.Length < num2)
		{
			int num4 = BraveUtility.SmartListResizer((this.meshIndices != null) ? this.meshIndices.Length : 0, num2, 100, 3);
			this.meshIndices = new int[num4];
		}
		int num5 = 0;
		if (currentSprite != null && currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			num5 = num3;
		}
		if (this.meshNormals == null || this.meshNormals.Length < num5)
		{
			this.meshNormals = new Vector3[num5];
		}
		int num6 = 0;
		if (currentSprite != null && currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			num6 = num3;
		}
		if (this.meshTangents == null || this.meshTangents.Length < num6)
		{
			this.meshTangents = new Vector4[num6];
		}
		float num7 = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num8 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		if (this.OverrideSetTiledSpriteGeom != null)
		{
			this.OverrideSetTiledSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, this.anchor, num7, num8);
		}
		else
		{
			tk2dSpriteGeomGen.SetTiledSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, this.anchor, num7, num8);
		}
		tk2dSpriteGeomGen.SetTiledSpriteIndices(this.meshIndices, 0, 0, currentSprite, this.dimensions, this.OverrideGetTiledSpriteGeomDesc);
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormalsFast(this.meshVertices, this.meshNormals, this.meshTangents);
		}
		this.SetColors(this.meshColors);
		if (base.ShouldDoTilt)
		{
			bool isPerpendicular = base.IsPerpendicular;
			for (int i = 0; i < num; i++)
			{
				float y = (this.m_transform.rotation * Vector3.Scale(this.meshVertices[i], this.m_transform.lossyScale)).y;
				if (isPerpendicular)
				{
					Vector3[] array = this.meshVertices;
					int num9 = i;
					array[num9].z = array[num9].z - y;
				}
				else
				{
					Vector3[] array2 = this.meshVertices;
					int num10 = i;
					array2[num10].z = array2[num10].z + y;
				}
			}
		}
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this.mesh.Clear();
		}
		this.mesh.vertices = this.meshVertices;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = this.meshUvs;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x0014B5A8 File Offset: 0x001497A8
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x0014B5B0 File Offset: 0x001497B0
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x0014B5B8 File Offset: 0x001497B8
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x0014B5C0 File Offset: 0x001497C0
	protected void UpdateColorsImpl()
	{
		if (this.meshColors == null || this.meshColors.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetColors(this.meshColors);
			this.mesh.colors32 = this.meshColors;
		}
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x0014B610 File Offset: 0x00149810
	protected void UpdateGeometryImpl()
	{
		this.Build();
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x0014B618 File Offset: 0x00149818
	protected override void UpdateCollider()
	{
		if (this.CreateBoxCollider)
		{
			if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D)
			{
				if (this.boxCollider != null)
				{
					this.boxCollider.size = 2f * this.boundsExtents;
					this.boxCollider.center = this.boundsCenter;
				}
			}
			else if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D && this.boxCollider2D != null)
			{
				this.boxCollider2D.size = 2f * this.boundsExtents;
				this.boxCollider2D.offset = this.boundsCenter;
			}
		}
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x0014B6DC File Offset: 0x001498DC
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x060040A0 RID: 16544 RVA: 0x0014B6E4 File Offset: 0x001498E4
	protected override void UpdateMaterial()
	{
		if (this.OverrideMaterialMode != tk2dBaseSprite.SpriteMaterialOverrideMode.NONE)
		{
			return;
		}
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x0014B74C File Offset: 0x0014994C
	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x0014B750 File Offset: 0x00149950
	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 vector = new Vector3(this._dimensions.x * currentSprite.texelSize.x * this._scale.x, this._dimensions.y * currentSprite.texelSize.y * this._scale.y);
		Vector3 vector2 = Vector3.zero;
		switch (this._anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			vector2.Set(0f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			vector2.Set(0.5f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			vector2.Set(1f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			vector2.Set(0f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			vector2.Set(0.5f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector2.Set(1f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			vector2.Set(0f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector2.Set(0.5f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.Set(1f, 1f, 0f);
			break;
		}
		vector2 = Vector3.Scale(vector2, vector) * -1f;
		Vector3 vector3 = vector + dMax - dMin;
		vector3.x /= currentSprite.texelSize.x * this._scale.x;
		vector3.y /= currentSprite.texelSize.y * this._scale.y;
		Vector3 vector4 = new Vector3((!Mathf.Approximately(this._dimensions.x, 0f)) ? (vector2.x * vector3.x / this._dimensions.x) : 0f, (!Mathf.Approximately(this._dimensions.y, 0f)) ? (vector2.y * vector3.y / this._dimensions.y) : 0f);
		Vector3 vector5 = vector2 + dMin - vector4;
		vector5.z = 0f;
		base.transform.position = base.transform.TransformPoint(vector5);
		this.dimensions = new Vector2(vector3.x, vector3.y);
	}

	// Token: 0x0400337C RID: 13180
	private Mesh mesh;

	// Token: 0x0400337D RID: 13181
	private Vector2[] meshUvs;

	// Token: 0x0400337E RID: 13182
	private Vector3[] meshVertices;

	// Token: 0x0400337F RID: 13183
	private Color32[] meshColors;

	// Token: 0x04003380 RID: 13184
	private Vector3[] meshNormals;

	// Token: 0x04003381 RID: 13185
	private Vector4[] meshTangents;

	// Token: 0x04003382 RID: 13186
	private int[] meshIndices;

	// Token: 0x04003383 RID: 13187
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x04003384 RID: 13188
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x04003385 RID: 13189
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x04003386 RID: 13190
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x04003387 RID: 13191
	private Vector3 boundsExtents = Vector3.zero;

	// Token: 0x04003388 RID: 13192
	public tk2dTiledSprite.OverrideGetTiledSpriteGeomDescDelegate OverrideGetTiledSpriteGeomDesc;

	// Token: 0x04003389 RID: 13193
	public tk2dTiledSprite.OverrideSetTiledSpriteGeomDelegate OverrideSetTiledSpriteGeom;

	// Token: 0x02000BE6 RID: 3046
	// (Invoke) Token: 0x060040A4 RID: 16548
	public delegate void OverrideGetTiledSpriteGeomDescDelegate(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions);

	// Token: 0x02000BE7 RID: 3047
	// (Invoke) Token: 0x060040A8 RID: 16552
	public delegate void OverrideSetTiledSpriteGeomDelegate(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ);
}
