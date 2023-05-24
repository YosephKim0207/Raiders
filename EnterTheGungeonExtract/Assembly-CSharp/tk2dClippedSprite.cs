using System;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("2D Toolkit/Sprite/tk2dClippedSprite")]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dClippedSprite : tk2dBaseSprite
{
	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06003F02 RID: 16130 RVA: 0x0013E190 File Offset: 0x0013C390
	// (set) Token: 0x06003F03 RID: 16131 RVA: 0x0013E1F4 File Offset: 0x0013C3F4
	public Rect ClipRect
	{
		get
		{
			this._clipRect.Set(this._clipBottomLeft.x, this._clipBottomLeft.y, this._clipTopRight.x - this._clipBottomLeft.x, this._clipTopRight.y - this._clipBottomLeft.y);
			return this._clipRect;
		}
		set
		{
			Vector2 vector = new Vector2(value.x, value.y);
			this.clipBottomLeft = vector;
			vector.x += value.width;
			vector.y += value.height;
			this.clipTopRight = vector;
		}
	}

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06003F04 RID: 16132 RVA: 0x0013E250 File Offset: 0x0013C450
	// (set) Token: 0x06003F05 RID: 16133 RVA: 0x0013E258 File Offset: 0x0013C458
	public Vector2 clipBottomLeft
	{
		get
		{
			return this._clipBottomLeft;
		}
		set
		{
			if (value != this._clipBottomLeft)
			{
				this._clipBottomLeft = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06003F06 RID: 16134 RVA: 0x0013E290 File Offset: 0x0013C490
	// (set) Token: 0x06003F07 RID: 16135 RVA: 0x0013E298 File Offset: 0x0013C498
	public Vector2 clipTopRight
	{
		get
		{
			return this._clipTopRight;
		}
		set
		{
			if (value != this._clipTopRight)
			{
				this._clipTopRight = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06003F08 RID: 16136 RVA: 0x0013E2D0 File Offset: 0x0013C4D0
	// (set) Token: 0x06003F09 RID: 16137 RVA: 0x0013E2D8 File Offset: 0x0013C4D8
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

	// Token: 0x06003F0A RID: 16138 RVA: 0x0013E2F4 File Offset: 0x0013C4F4
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
		}
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x0013E370 File Offset: 0x0013C570
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0013E394 File Offset: 0x0013C594
	protected new void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 4, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0013E3B0 File Offset: 0x0013C5B0
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float num = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num2 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		tk2dSpriteGeomGen.SetClippedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this._clipBottomLeft, this._clipTopRight, num, num2);
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(this.meshVertices, this.meshVertices[0], this.meshVertices[3], currentSprite.normals, currentSprite.tangents, this.meshNormals, this.meshTangents);
		}
		if (currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0013E4F4 File Offset: 0x0013C6F4
	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		this.meshUvs = new Vector2[4];
		this.meshVertices = new Vector3[4];
		this.meshColors = new Color32[4];
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			this.meshNormals = new Vector3[4];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[4];
		}
		this.SetGeometry(this.meshVertices, this.meshUvs);
		this.SetColors(this.meshColors);
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
		int[] array = new int[6];
		tk2dSpriteGeomGen.SetClippedSpriteIndices(array, 0, 0, base.CurrentSprite);
		this.mesh.triangles = array;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x0013E6A4 File Offset: 0x0013C8A4
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0013E6AC File Offset: 0x0013C8AC
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0013E6B4 File Offset: 0x0013C8B4
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0013E6BC File Offset: 0x0013C8BC
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

	// Token: 0x06003F13 RID: 16147 RVA: 0x0013E70C File Offset: 0x0013C90C
	protected void UpdateGeometryImpl()
	{
		if (this.meshVertices == null || this.meshVertices.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetGeometry(this.meshVertices, this.meshUvs);
			this.mesh.vertices = this.meshVertices;
			this.mesh.uv = this.meshUvs;
			this.mesh.normals = this.meshNormals;
			this.mesh.tangents = this.meshTangents;
			this.mesh.RecalculateBounds();
			this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		}
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x0013E7C0 File Offset: 0x0013C9C0
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

	// Token: 0x06003F15 RID: 16149 RVA: 0x0013E884 File Offset: 0x0013CA84
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x0013E88C File Offset: 0x0013CA8C
	protected override void UpdateMaterial()
	{
		if (this.OverrideMaterialMode != tk2dBaseSprite.SpriteMaterialOverrideMode.NONE && base.renderer.sharedMaterial != null)
		{
			if (this.OverrideMaterialMode == tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE)
			{
				if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
				{
					base.renderer.sharedMaterial.mainTexture = this.collectionInst.spriteDefinitions[base.spriteId].materialInst.mainTexture;
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

	// Token: 0x06003F17 RID: 16151 RVA: 0x0013E97C File Offset: 0x0013CB7C
	protected override int GetCurrentVertexCount()
	{
		return 4;
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x0013E980 File Offset: 0x0013CB80
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

	// Token: 0x0400315B RID: 12635
	private Mesh mesh;

	// Token: 0x0400315C RID: 12636
	private Vector2[] meshUvs;

	// Token: 0x0400315D RID: 12637
	private Vector3[] meshVertices;

	// Token: 0x0400315E RID: 12638
	private Color32[] meshColors;

	// Token: 0x0400315F RID: 12639
	private Vector3[] meshNormals;

	// Token: 0x04003160 RID: 12640
	private Vector4[] meshTangents;

	// Token: 0x04003161 RID: 12641
	private int[] meshIndices;

	// Token: 0x04003162 RID: 12642
	public Vector2 _clipBottomLeft = new Vector2(0f, 0f);

	// Token: 0x04003163 RID: 12643
	public Vector2 _clipTopRight = new Vector2(1f, 1f);

	// Token: 0x04003164 RID: 12644
	private Rect _clipRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04003165 RID: 12645
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x04003166 RID: 12646
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x04003167 RID: 12647
	private Vector3 boundsExtents = Vector3.zero;
}
