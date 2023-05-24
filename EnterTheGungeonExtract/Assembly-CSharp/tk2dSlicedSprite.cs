using System;
using UnityEngine;

// Token: 0x02000BAF RID: 2991
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSlicedSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class tk2dSlicedSprite : tk2dBaseSprite
{
	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x06003F2A RID: 16170 RVA: 0x0013F7AC File Offset: 0x0013D9AC
	// (set) Token: 0x06003F2B RID: 16171 RVA: 0x0013F7B4 File Offset: 0x0013D9B4
	public bool BorderOnly
	{
		get
		{
			return this._borderOnly;
		}
		set
		{
			if (value != this._borderOnly)
			{
				this._borderOnly = value;
				if (this._tileStretchedSprites)
				{
					this.UpdateGeometryImpl();
				}
				else
				{
					this.UpdateIndices();
				}
			}
		}
	}

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06003F2C RID: 16172 RVA: 0x0013F7E8 File Offset: 0x0013D9E8
	// (set) Token: 0x06003F2D RID: 16173 RVA: 0x0013F7F0 File Offset: 0x0013D9F0
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

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06003F2E RID: 16174 RVA: 0x0013F818 File Offset: 0x0013DA18
	// (set) Token: 0x06003F2F RID: 16175 RVA: 0x0013F820 File Offset: 0x0013DA20
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

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06003F30 RID: 16176 RVA: 0x0013F844 File Offset: 0x0013DA44
	// (set) Token: 0x06003F31 RID: 16177 RVA: 0x0013F84C File Offset: 0x0013DA4C
	public Vector2 anchorOffset
	{
		get
		{
			return this._anchorOffset;
		}
		set
		{
			if (value != this._anchorOffset)
			{
				this._anchorOffset = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06003F32 RID: 16178 RVA: 0x0013F874 File Offset: 0x0013DA74
	// (set) Token: 0x06003F33 RID: 16179 RVA: 0x0013F87C File Offset: 0x0013DA7C
	public bool TileStretchedSprites
	{
		get
		{
			return this._tileStretchedSprites;
		}
		set
		{
			if (value != this._tileStretchedSprites)
			{
				this._tileStretchedSprites = value;
				this.meshVertices = null;
				this.UpdateGeometryImpl();
			}
		}
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x0013F8A0 File Offset: 0x0013DAA0
	public void SetBorder(float left, float bottom, float right, float top)
	{
		if (this.borderLeft != left || this.borderBottom != bottom || this.borderRight != right || this.borderTop != top)
		{
			this.borderLeft = left;
			this.borderBottom = bottom;
			this.borderRight = right;
			this.borderTop = top;
			this.UpdateVertices();
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06003F35 RID: 16181 RVA: 0x0013F904 File Offset: 0x0013DB04
	// (set) Token: 0x06003F36 RID: 16182 RVA: 0x0013F90C File Offset: 0x0013DB0C
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

	// Token: 0x06003F37 RID: 16183 RVA: 0x0013F928 File Offset: 0x0013DB28
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		if (this.boxCollider2D == null)
		{
			this.boxCollider2D = base.GetComponent<BoxCollider2D>();
		}
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x0013F9E0 File Offset: 0x0013DBE0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x0013FA04 File Offset: 0x0013DC04
	protected new void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, this.meshVertices.Length, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x0013FA28 File Offset: 0x0013DC28
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float num = ((!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z);
		float num2 = ((!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f));
		tk2dSpriteGeomGen.SetSlicedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._borderOnly, this._scale, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.borderCornerBottom, this.anchor, num, num2, this._anchorOffset, this._tileStretchedSprites);
		if (base.ShouldDoTilt)
		{
			for (int i = 0; i < this.meshVertices.Length; i++)
			{
				float y = (this.m_transform.rotation * Vector3.Scale(this.meshVertices[i], this.m_transform.lossyScale)).y;
				if (base.IsPerpendicular)
				{
					this.meshVertices[i].z = this.meshVertices[i].z - y;
				}
				else
				{
					this.meshVertices[i].z = this.meshVertices[i].z + y;
				}
			}
		}
		if (this.meshNormals.Length > 0 || this.meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(this.meshVertices, this.meshVertices[0], this.meshVertices[this.meshVertices.Length - 1], currentSprite.normals, currentSprite.tangents, this.meshNormals, this.meshTangents);
		}
		if (currentSprite.complexGeometry)
		{
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] = Vector3.zero;
			}
		}
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x0013FC70 File Offset: 0x0013DE70
	private void SetIndices()
	{
		int num;
		int num2;
		tk2dSpriteGeomGen.GetSlicedSpriteGeomDesc(out num, out num2, base.CurrentSprite, this._borderOnly, this._tileStretchedSprites, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.borderCornerBottom);
		if (this.meshIndices == null || this.meshIndices.Length != num2)
		{
			this.meshIndices = new int[num2];
		}
		tk2dSpriteGeomGen.SetSlicedSpriteIndices(this.meshIndices, 0, 0, base.CurrentSprite, this._borderOnly, this._tileStretchedSprites, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.borderCornerBottom);
	}

	// Token: 0x06003F3C RID: 16188 RVA: 0x0013FD38 File Offset: 0x0013DF38
	private bool NearEnough(float value, float compValue, float scale)
	{
		float num = Mathf.Abs(value - compValue);
		return Mathf.Abs(num / scale) < 0.01f;
	}

	// Token: 0x06003F3D RID: 16189 RVA: 0x0013FD60 File Offset: 0x0013DF60
	private void PermanentUpgradeLegacyMode()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float x = currentSprite.untrimmedBoundsDataCenter.x;
		float y = currentSprite.untrimmedBoundsDataCenter.y;
		float x2 = currentSprite.untrimmedBoundsDataExtents.x;
		float y2 = currentSprite.untrimmedBoundsDataExtents.y;
		if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerCenter;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerRight;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerLeft;
		}
		else
		{
			Debug.LogError("tk2dSlicedSprite (" + base.name + ") error - Unable to determine anchor upgrading from legacy mode. Please fix this manually.");
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		float num = x2 / currentSprite.texelSize.x;
		float num2 = y2 / currentSprite.texelSize.y;
		this._dimensions.x = this._scale.x * num;
		this._dimensions.y = this._scale.y * num2;
		this._scale.Set(1f, 1f, 1f);
		this.legacyMode = false;
	}

	// Token: 0x06003F3E RID: 16190 RVA: 0x00140018 File Offset: 0x0013E218
	public override void Build()
	{
		if (this.legacyMode)
		{
			this.PermanentUpgradeLegacyMode();
		}
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		int num;
		int num2;
		tk2dSpriteGeomGen.GetSlicedSpriteGeomDesc(out num, out num2, currentSprite, this._borderOnly, this._tileStretchedSprites, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.borderCornerBottom);
		int num3 = 0;
		int num4 = 0;
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			num3 = num;
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			num4 = num;
		}
		if (this.meshUvs == null || this.meshUvs.Length != num)
		{
			this.meshUvs = new Vector2[num];
		}
		if (this.meshVertices == null || this.meshVertices.Length != num)
		{
			this.meshVertices = new Vector3[num];
		}
		if (this.meshColors == null || this.meshColors.Length != num)
		{
			this.meshColors = new Color32[num];
		}
		if (this.meshNormals == null || this.meshNormals.Length != num3)
		{
			this.meshNormals = new Vector3[num3];
		}
		if (this.meshTangents == null || this.meshTangents.Length != num4)
		{
			this.meshTangents = new Vector4[num4];
		}
		this.SetIndices();
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
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.renderLayer);
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x06003F3F RID: 16191 RVA: 0x00140284 File Offset: 0x0013E484
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06003F40 RID: 16192 RVA: 0x0014028C File Offset: 0x0013E48C
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06003F41 RID: 16193 RVA: 0x00140294 File Offset: 0x0013E494
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06003F42 RID: 16194 RVA: 0x0014029C File Offset: 0x0013E49C
	private void UpdateIndices()
	{
		if (this.mesh != null)
		{
			this.SetIndices();
			this.mesh.triangles = this.meshIndices;
		}
	}

	// Token: 0x06003F43 RID: 16195 RVA: 0x001402C8 File Offset: 0x0013E4C8
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

	// Token: 0x06003F44 RID: 16196 RVA: 0x00140318 File Offset: 0x0013E518
	protected void UpdateGeometryImpl()
	{
		if (this.meshVertices == null || this.meshVertices.Length == 0 || this.TileStretchedSprites)
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
			this.UpdateCollider();
		}
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x001403DC File Offset: 0x0013E5DC
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

	// Token: 0x06003F46 RID: 16198 RVA: 0x001404A0 File Offset: 0x0013E6A0
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x001404A8 File Offset: 0x0013E6A8
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x00140504 File Offset: 0x0013E704
	protected override int GetCurrentVertexCount()
	{
		if (!this.TileStretchedSprites)
		{
			return 16;
		}
		if (this._spriteId == -1 || this._spriteId >= this.collectionInst.spriteDefinitions.Length)
		{
			return 16;
		}
		return 4;
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x0014053C File Offset: 0x0013E73C
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

	// Token: 0x0400316F RID: 12655
	private Mesh mesh;

	// Token: 0x04003170 RID: 12656
	private Vector2[] meshUvs;

	// Token: 0x04003171 RID: 12657
	private Vector3[] meshVertices;

	// Token: 0x04003172 RID: 12658
	private Color32[] meshColors;

	// Token: 0x04003173 RID: 12659
	private Vector3[] meshNormals;

	// Token: 0x04003174 RID: 12660
	private Vector4[] meshTangents;

	// Token: 0x04003175 RID: 12661
	private int[] meshIndices;

	// Token: 0x04003176 RID: 12662
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x04003177 RID: 12663
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x04003178 RID: 12664
	[SerializeField]
	private bool _borderOnly;

	// Token: 0x04003179 RID: 12665
	[SerializeField]
	private bool legacyMode;

	// Token: 0x0400317A RID: 12666
	[SerializeField]
	private Vector2 _anchorOffset;

	// Token: 0x0400317B RID: 12667
	[SerializeField]
	private bool _tileStretchedSprites;

	// Token: 0x0400317C RID: 12668
	public float borderTop = 0.2f;

	// Token: 0x0400317D RID: 12669
	public float borderBottom = 0.2f;

	// Token: 0x0400317E RID: 12670
	public float borderLeft = 0.2f;

	// Token: 0x0400317F RID: 12671
	public float borderRight = 0.2f;

	// Token: 0x04003180 RID: 12672
	public float borderCornerBottom;

	// Token: 0x04003181 RID: 12673
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x04003182 RID: 12674
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x04003183 RID: 12675
	private Vector3 boundsExtents = Vector3.zero;
}
