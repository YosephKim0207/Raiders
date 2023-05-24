using System;
using System.Collections.Generic;
using System.Diagnostics;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000BA7 RID: 2983
[AddComponentMenu("2D Toolkit/Backend/tk2dBaseSprite")]
public abstract class tk2dBaseSprite : PersistentVFXBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x06003EAD RID: 16045 RVA: 0x0013C42C File Offset: 0x0013A62C
	// (set) Token: 0x06003EAE RID: 16046 RVA: 0x0013C43C File Offset: 0x0013A63C
	public bool usesOverrideMaterial
	{
		get
		{
			return this.OverrideMaterialMode != tk2dBaseSprite.SpriteMaterialOverrideMode.NONE;
		}
		set
		{
			if (value)
			{
				if (this.OverrideMaterialMode == tk2dBaseSprite.SpriteMaterialOverrideMode.NONE)
				{
					this.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
				}
			}
			else
			{
				this.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.NONE;
			}
		}
	}

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0013C464 File Offset: 0x0013A664
	// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x0013C488 File Offset: 0x0013A688
	public tk2dSpriteCollectionData Collection
	{
		get
		{
			if (this.m_cachedAnimator != null)
			{
				this.m_cachedAnimator.ForceInvisibleSpriteUpdate();
			}
			return this.collection;
		}
		set
		{
			this.collection = value;
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x14000086 RID: 134
	// (add) Token: 0x06003EB1 RID: 16049 RVA: 0x0013C4A4 File Offset: 0x0013A6A4
	// (remove) Token: 0x06003EB2 RID: 16050 RVA: 0x0013C4DC File Offset: 0x0013A6DC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dBaseSprite> SpriteChanged;

	// Token: 0x06003EB3 RID: 16051 RVA: 0x0013C514 File Offset: 0x0013A714
	private void InitInstance()
	{
		if (this.collectionInst == null && this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x0013C54C File Offset: 0x0013A74C
	// (set) Token: 0x06003EB5 RID: 16053 RVA: 0x0013C554 File Offset: 0x0013A754
	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (value != this._color)
			{
				this._color = value;
				this.InitInstance();
				this.UpdateColors();
			}
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x0013C57C File Offset: 0x0013A77C
	// (set) Token: 0x06003EB7 RID: 16055 RVA: 0x0013C584 File Offset: 0x0013A784
	public Vector3 scale
	{
		get
		{
			return this._scale;
		}
		set
		{
			if (value != this._scale)
			{
				this._scale = value;
				this.InitInstance();
				this.UpdateVertices();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x0013C5D4 File Offset: 0x0013A7D4
	private Renderer CachedRenderer
	{
		get
		{
			if (this._cachedRenderer == null)
			{
				this._cachedRenderer = base.renderer;
			}
			return this._cachedRenderer;
		}
	}

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x0013C5FC File Offset: 0x0013A7FC
	// (set) Token: 0x06003EBA RID: 16058 RVA: 0x0013C664 File Offset: 0x0013A864
	public bool ShouldDoTilt
	{
		get
		{
			return !this.m_forceNoTilt && (this.CachedPerpState != tk2dBaseSprite.PerpendicularState.UNDEFINED || (base.renderer != null && base.renderer.sharedMaterial != null && base.renderer.sharedMaterial.HasProperty("_Perpendicular")));
		}
		set
		{
			this.m_forceNoTilt = !value;
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06003EBB RID: 16059 RVA: 0x0013C670 File Offset: 0x0013A870
	// (set) Token: 0x06003EBC RID: 16060 RVA: 0x0013C768 File Offset: 0x0013A968
	public bool IsPerpendicular
	{
		get
		{
			if (base.renderer == null || base.renderer.sharedMaterial == null)
			{
				return false;
			}
			if (this.CachedPerpState != tk2dBaseSprite.PerpendicularState.UNDEFINED)
			{
				return this.CachedPerpState == tk2dBaseSprite.PerpendicularState.PERPENDICULAR;
			}
			if (!base.renderer.sharedMaterial.HasProperty("_Perpendicular"))
			{
				UnityEngine.Debug.LogWarning(base.name + " <- failed to get perp");
				return true;
			}
			if (Application.isPlaying)
			{
				this.CachedPerpState = ((base.renderer.sharedMaterial.GetFloat("_Perpendicular") != 1f) ? tk2dBaseSprite.PerpendicularState.FLAT : tk2dBaseSprite.PerpendicularState.PERPENDICULAR);
				return this.CachedPerpState == tk2dBaseSprite.PerpendicularState.PERPENDICULAR;
			}
			return base.renderer.sharedMaterial.GetFloat("_Perpendicular") == 1f;
		}
		set
		{
			this.CachedPerpState = ((!value) ? tk2dBaseSprite.PerpendicularState.FLAT : tk2dBaseSprite.PerpendicularState.PERPENDICULAR);
			this.ForceBuild();
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06003EBD RID: 16061 RVA: 0x0013C784 File Offset: 0x0013A984
	// (set) Token: 0x06003EBE RID: 16062 RVA: 0x0013C78C File Offset: 0x0013A98C
	public float HeightOffGround
	{
		get
		{
			return this.m_heightOffGround;
		}
		set
		{
			this.m_heightOffGround = value;
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06003EBF RID: 16063 RVA: 0x0013C798 File Offset: 0x0013A998
	// (set) Token: 0x06003EC0 RID: 16064 RVA: 0x0013C7A8 File Offset: 0x0013A9A8
	public int SortingOrder
	{
		get
		{
			return this.CachedRenderer.sortingOrder;
		}
		set
		{
			if (this.CachedRenderer.sortingOrder != value)
			{
				this.renderLayer = value;
				this.CachedRenderer.sortingOrder = value;
			}
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x0013C7D0 File Offset: 0x0013A9D0
	// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x0013C7E4 File Offset: 0x0013A9E4
	public bool FlipX
	{
		get
		{
			return this._scale.x < 0f;
		}
		set
		{
			this.scale = new Vector3(Mathf.Abs(this._scale.x) * (float)((!value) ? 1 : (-1)), this._scale.y, this._scale.z);
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x0013C834 File Offset: 0x0013AA34
	// (set) Token: 0x06003EC4 RID: 16068 RVA: 0x0013C848 File Offset: 0x0013AA48
	public bool FlipY
	{
		get
		{
			return this._scale.y < 0f;
		}
		set
		{
			this.scale = new Vector3(this._scale.x, Mathf.Abs(this._scale.y) * (float)((!value) ? 1 : (-1)), this._scale.z);
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x0013C898 File Offset: 0x0013AA98
	// (set) Token: 0x06003EC6 RID: 16070 RVA: 0x0013C8BC File Offset: 0x0013AABC
	public int spriteId
	{
		get
		{
			if (this.m_cachedAnimator != null)
			{
				this.m_cachedAnimator.ForceInvisibleSpriteUpdate();
			}
			return this._spriteId;
		}
		set
		{
			this.hasOffScreenCachedUpdate = false;
			this.offScreenCachedCollection = null;
			this.offScreenCachedID = -1;
			if (value != this._spriteId)
			{
				this.InitInstance();
				value = Mathf.Clamp(value, 0, this.collectionInst.spriteDefinitions.Length - 1);
				if (this._spriteId < 0 || this._spriteId >= this.collectionInst.spriteDefinitions.Length || this.GetCurrentVertexCount() != 4 || this.collectionInst.spriteDefinitions[this._spriteId].complexGeometry != this.collectionInst.spriteDefinitions[value].complexGeometry)
				{
					this._spriteId = value;
					this.UpdateGeometry();
				}
				else
				{
					this._spriteId = value;
					this.UpdateVertices();
				}
				this.UpdateMaterial();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0013C9A8 File Offset: 0x0013ABA8
	public void SetSprite(int newSpriteId)
	{
		this.spriteId = newSpriteId;
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0013C9B4 File Offset: 0x0013ABB4
	public bool SetSprite(string spriteName)
	{
		int spriteIdByName = this.collection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(spriteIdByName);
		}
		else
		{
			UnityEngine.Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0013C9FC File Offset: 0x0013ABFC
	public void SetSprite(tk2dSpriteCollectionData newCollection, int newSpriteId)
	{
		bool flag = false;
		if (this.Collection != newCollection)
		{
			this.collection = newCollection;
			this.collectionInst = this.collection.inst;
			this._spriteId = -1;
			flag = true;
		}
		this.spriteId = newSpriteId;
		if (flag)
		{
			this.UpdateMaterial();
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0013CA50 File Offset: 0x0013AC50
	public bool SetSprite(tk2dSpriteCollectionData newCollection, string spriteName)
	{
		int spriteIdByName = newCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(newCollection, spriteIdByName);
		}
		else
		{
			UnityEngine.Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0013CA94 File Offset: 0x0013AC94
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this.Collection.version < 2)
			{
				UnityEngine.Debug.LogError("Need to rebuild sprite collection.");
			}
			float num2 = base.transform.position.z - tk2dCamera.transform.position.z;
			float num3 = this.Collection.invOrthoSize * this.Collection.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(num2) * num3;
		}
		else if (Camera.main)
		{
			if (Camera.main.orthographic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float num4 = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, num4);
			}
			num *= this.Collection.invOrthoSize;
		}
		else
		{
			UnityEngine.Debug.LogError("Main camera not found.");
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0013CC0C File Offset: 0x0013AE0C
	public void ForceRotationRebuild()
	{
		if (this.m_transform && this.m_transform.rotation != this.m_cachedRotation)
		{
			this.Build();
			this.m_cachedRotation = this.m_transform.rotation;
		}
		if (this.attachedRenderers != null)
		{
			for (int i = 0; i < this.attachedRenderers.Count; i++)
			{
				this.attachedRenderers[i].ForceRotationRebuild();
			}
		}
	}

	// Token: 0x06003ECD RID: 16077
	protected abstract void UpdateMaterial();

	// Token: 0x06003ECE RID: 16078
	protected abstract void UpdateColors();

	// Token: 0x06003ECF RID: 16079
	protected abstract void UpdateVertices();

	// Token: 0x06003ED0 RID: 16080
	protected abstract void UpdateGeometry();

	// Token: 0x06003ED1 RID: 16081
	protected abstract int GetCurrentVertexCount();

	// Token: 0x06003ED2 RID: 16082 RVA: 0x0013CC94 File Offset: 0x0013AE94
	public void ForceUpdateMaterial()
	{
		if (base.renderer == null || this.collectionInst == null)
		{
			return;
		}
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[this.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[this.spriteId].materialInst;
		}
	}

	// Token: 0x06003ED3 RID: 16083
	public abstract void Build();

	// Token: 0x06003ED4 RID: 16084 RVA: 0x0013CD14 File Offset: 0x0013AF14
	public int GetSpriteIdByName(string name)
	{
		this.InitInstance();
		return this.collectionInst.GetSpriteIdByName(name);
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0013CD28 File Offset: 0x0013AF28
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId) where T : tk2dBaseSprite
	{
		T t = go.AddComponent<T>();
		t._spriteId = -1;
		t.SetSprite(spriteCollection, spriteId);
		t.Build();
		return t;
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0013CD68 File Offset: 0x0013AF68
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName) where T : tk2dBaseSprite
	{
		int spriteIdByName = spriteCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName == -1)
		{
			UnityEngine.Debug.LogError(string.Format("Unable to find sprite named {0} in sprite collection {1}", spriteName, spriteCollection.spriteCollectionName));
			return (T)((object)null);
		}
		return tk2dBaseSprite.AddComponent<T>(go, spriteCollection, spriteIdByName);
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0013CDAC File Offset: 0x0013AFAC
	protected int GetNumVertices()
	{
		this.InitInstance();
		return 4;
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0013CDB8 File Offset: 0x0013AFB8
	protected int GetNumIndices()
	{
		this.InitInstance();
		return this.collectionInst.spriteDefinitions[this.spriteId].indices.Length;
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0013CDDC File Offset: 0x0013AFDC
	protected void SetPositions(Vector3[] positions, Vector3[] normals, Vector4[] tangents)
	{
		if (this.m_transform == null)
		{
			this.m_transform = base.transform;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this.spriteId];
		int numVertices = this.GetNumVertices();
		positions[0] = Vector3.Scale(tk2dSpriteDefinition.position0, this._scale);
		positions[1] = Vector3.Scale(tk2dSpriteDefinition.position1, this._scale);
		positions[2] = Vector3.Scale(tk2dSpriteDefinition.position2, this._scale);
		positions[3] = Vector3.Scale(tk2dSpriteDefinition.position3, this._scale);
		if (this.ShouldDoTilt)
		{
			float num = 0f;
			for (int i = 0; i < numVertices; i++)
			{
				Vector3 vector = positions[i];
				float y = (this.m_transform.rotation * Vector3.Scale(vector, this.m_transform.lossyScale)).y;
				if (this.IsPerpendicular)
				{
					int num2 = i;
					positions[num2].z = positions[num2].z - y;
					if (this.AdditionalPerpForwardPercentage > 0f)
					{
						int num3 = i;
						positions[num3].z = positions[num3].z - y * this.AdditionalPerpForwardPercentage;
					}
				}
				else
				{
					int num4 = i;
					positions[num4].z = positions[num4].z + y;
					if (this.AdditionalFlatForwardPercentage > 0f)
					{
						num = Mathf.Max(y * this.AdditionalFlatForwardPercentage, num);
						int num5 = i;
						positions[num5].z = positions[num5].z - y * this.AdditionalFlatForwardPercentage;
					}
				}
			}
			if (this.AdditionalFlatForwardPercentage > 0f)
			{
				for (int j = 0; j < numVertices; j++)
				{
					positions[j] += new Vector3(0f, 0f, num);
				}
			}
		}
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x0013CFEC File Offset: 0x0013B1EC
	protected void SetColors(Color32[] dest)
	{
		Color color = this._color;
		if (this.collectionInst.premultipliedAlpha)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
		}
		Color32 color2 = color;
		int numVertices = this.GetNumVertices();
		for (int i = 0; i < numVertices; i++)
		{
			dest[i] = color2;
		}
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x0013D07C File Offset: 0x0013B27C
	public Bounds GetBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.boundsDataCenter.x * this._scale.x, tk2dSpriteDefinition.boundsDataCenter.y * this._scale.y, tk2dSpriteDefinition.boundsDataCenter.z * this._scale.z), new Vector3(tk2dSpriteDefinition.boundsDataExtents.x * Mathf.Abs(this._scale.x), tk2dSpriteDefinition.boundsDataExtents.y * Mathf.Abs(this._scale.y), tk2dSpriteDefinition.boundsDataExtents.z * Mathf.Abs(this._scale.z)));
	}

	// Token: 0x06003EDC RID: 16092 RVA: 0x0013D14C File Offset: 0x0013B34C
	public Bounds GetUntrimmedBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.untrimmedBoundsDataCenter.x * this._scale.x, tk2dSpriteDefinition.untrimmedBoundsDataCenter.y * this._scale.y, tk2dSpriteDefinition.untrimmedBoundsDataCenter.z * this._scale.z), new Vector3(tk2dSpriteDefinition.untrimmedBoundsDataExtents.x * Mathf.Abs(this._scale.x), tk2dSpriteDefinition.untrimmedBoundsDataExtents.y * Mathf.Abs(this._scale.y), tk2dSpriteDefinition.untrimmedBoundsDataExtents.z * Mathf.Abs(this._scale.z)));
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x0013D21C File Offset: 0x0013B41C
	public static Bounds AdjustedMeshBounds(Bounds bounds, int renderLayer)
	{
		Vector3 center = bounds.center;
		center.z = (float)(-(float)renderLayer) * 0.01f;
		bounds.center = center;
		return bounds;
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x0013D24C File Offset: 0x0013B44C
	public tk2dSpriteDefinition GetCurrentSpriteDef()
	{
		this.InitInstance();
		return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x0013D280 File Offset: 0x0013B480
	public tk2dSpriteDefinition GetTrueCurrentSpriteDef()
	{
		if (this.hasOffScreenCachedUpdate)
		{
			return this.offScreenCachedCollection.spriteDefinitions[this.offScreenCachedID];
		}
		return this.GetCurrentSpriteDef();
	}

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x0013D2A8 File Offset: 0x0013B4A8
	public tk2dSpriteDefinition CurrentSprite
	{
		get
		{
			this.InitInstance();
			return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0013D2DC File Offset: 0x0013B4DC
	public virtual void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0013D2E0 File Offset: 0x0013B4E0
	protected virtual bool NeedBoxCollider()
	{
		return false;
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0013D2E4 File Offset: 0x0013B4E4
	protected virtual void UpdateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.physicsEngine != tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			if (tk2dSpriteDefinition.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
			{
				if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					if (this.boxCollider2D == null)
					{
						this.boxCollider2D = base.gameObject.GetComponent<BoxCollider2D>();
						if (this.boxCollider2D == null)
						{
							this.boxCollider2D = base.gameObject.AddComponent<BoxCollider2D>();
						}
					}
					if (!this.boxCollider2D.enabled)
					{
						this.boxCollider2D.enabled = true;
					}
					this.boxCollider2D.offset = new Vector2(tk2dSpriteDefinition.colliderVertices[0].x * this._scale.x, tk2dSpriteDefinition.colliderVertices[0].y * this._scale.y);
					this.boxCollider2D.size = new Vector2(Mathf.Abs(2f * tk2dSpriteDefinition.colliderVertices[1].x * this._scale.x), Mathf.Abs(2f * tk2dSpriteDefinition.colliderVertices[1].y * this._scale.y));
				}
				else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					UnityEngine.Debug.LogError("BraveTK2D does not support mesh colliders.");
				}
				else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.None && this.boxCollider2D != null && this.boxCollider2D.enabled)
				{
					this.boxCollider2D.enabled = false;
				}
			}
		}
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0013D490 File Offset: 0x0013B690
	protected virtual void CreateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Unset)
		{
			return;
		}
		if (tk2dSpriteDefinition.physicsEngine != tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			if (tk2dSpriteDefinition.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
			{
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0013D4E0 File Offset: 0x0013B6E0
	protected void Awake()
	{
		if (this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
		this.CachedRenderer.sortingOrder = this.renderLayer;
		this.m_cachedStartingSpriteID = this._spriteId;
		this.m_transform = base.transform;
		this.m_renderer = base.GetComponent<MeshRenderer>();
		this.m_cachedYPosition = this.m_transform.position.y;
		this.m_cachedAnimator = base.GetComponent<tk2dSpriteAnimator>();
		if (this.attachParent != null)
		{
			this.automaticallyManagesDepth = false;
			this.attachParent.AttachRenderer(this);
		}
		bool flag = base.gameObject.layer == 28;
		if (!this.allowDefaultLayer)
		{
			if (this.m_renderer.sortingLayerName == "Default" || this.m_renderer.sortingLayerID == 0)
			{
				this.renderLayer = 2;
				DepthLookupManager.ProcessRenderer(this.m_renderer);
			}
			if (base.gameObject.layer < 13 || base.gameObject.layer > 26)
			{
				base.gameObject.layer = 22;
			}
		}
		if (flag || Pixelator.IsValidReflectionObject(this))
		{
			base.gameObject.layer = 28;
		}
		this.m_cachedScale = this.scale;
		if (this.automaticallyManagesDepth)
		{
			this.UpdateZDepth();
		}
		this.m_cachedRotation = this.m_transform.rotation;
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0013D668 File Offset: 0x0013B868
	public void OnSpawned()
	{
		this.m_transform = base.transform;
		this.m_cachedYPosition = this.m_transform.position.y;
		if (this.attachParent != null)
		{
			this.automaticallyManagesDepth = false;
			this.attachParent.AttachRenderer(this);
		}
		if (this.automaticallyManagesDepth)
		{
			this.UpdateZDepth();
		}
		this.m_cachedRotation = this.m_transform.rotation;
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0013D6E0 File Offset: 0x0013B8E0
	public void OnDespawned()
	{
		this.scale = this.m_cachedScale;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x0013D6F4 File Offset: 0x0013B8F4
	public void CreateSimpleBoxCollider()
	{
		if (this.CurrentSprite == null)
		{
			return;
		}
		if (this.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			this.boxCollider2D = base.GetComponent<BoxCollider2D>();
			if (this.boxCollider2D != null)
			{
				UnityEngine.Object.DestroyImmediate(this.boxCollider2D, true);
			}
			this.boxCollider = base.GetComponent<BoxCollider>();
			if (this.boxCollider == null)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		else if (this.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
			if (this.boxCollider != null)
			{
				UnityEngine.Object.DestroyImmediate(this.boxCollider, true);
			}
			this.boxCollider2D = base.GetComponent<BoxCollider2D>();
			if (this.boxCollider2D == null)
			{
				this.boxCollider2D = base.gameObject.AddComponent<BoxCollider2D>();
			}
		}
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0013D7E4 File Offset: 0x0013B9E4
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.Collection == spriteCollection;
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0013D7F4 File Offset: 0x0013B9F4
	public virtual void ForceBuild()
	{
		if (!this)
		{
			return;
		}
		if (this.collection == null)
		{
			return;
		}
		this.collectionInst = this.collection.inst;
		if (this.spriteId < 0 || this.spriteId >= this.collectionInst.spriteDefinitions.Length)
		{
			this.spriteId = 0;
		}
		this.Build();
		if (this.SpriteChanged != null)
		{
			this.SpriteChanged(this);
		}
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0013D878 File Offset: 0x0013BA78
	public static GameObject CreateFromTexture<T>(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor) where T : tk2dBaseSprite
	{
		tk2dSpriteCollectionData tk2dSpriteCollectionData = SpriteCollectionGenerator.CreateFromTexture(texture, size, region, anchor);
		if (tk2dSpriteCollectionData == null)
		{
			return null;
		}
		GameObject gameObject = new GameObject();
		tk2dBaseSprite.AddComponent<T>(gameObject, tk2dSpriteCollectionData, 0);
		return gameObject;
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0013D8B0 File Offset: 0x0013BAB0
	public IntVector2 GetAnchorPixelOffset()
	{
		return -PhysicsEngine.UnitToPixel(this.GetUntrimmedBounds().min);
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0013D8DC File Offset: 0x0013BADC
	public Vector2 GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor anchor)
	{
		Bounds bounds = this.GetBounds();
		Vector2 vector = bounds.min;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector.x += bounds.extents.x;
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector.x += bounds.extents.x * 2f;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector.y += bounds.extents.y;
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector.y += bounds.extents.y * 2f;
			break;
		}
		return vector;
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06003EEE RID: 16110 RVA: 0x0013D9FC File Offset: 0x0013BBFC
	public Vector2 WorldCenter
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06003EEF RID: 16111 RVA: 0x0013DA40 File Offset: 0x0013BC40
	public Vector2 WorldTopCenter
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.UpperCenter).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x0013DA84 File Offset: 0x0013BC84
	public Vector2 WorldTopLeft
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.UpperLeft).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x0013DAC8 File Offset: 0x0013BCC8
	public Vector2 WorldTopRight
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.UpperRight).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x0013DB0C File Offset: 0x0013BD0C
	public Vector2 WorldBottomLeft
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.LowerLeft).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x0013DB50 File Offset: 0x0013BD50
	public Vector2 WorldBottomCenter
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.LowerCenter).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06003EF4 RID: 16116 RVA: 0x0013DB94 File Offset: 0x0013BD94
	public Vector2 WorldBottomRight
	{
		get
		{
			return base.transform.position.XY() + this.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.LowerRight).Rotate(base.transform.eulerAngles.z);
		}
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0013DBD8 File Offset: 0x0013BDD8
	public void PlayEffectOnSprite(GameObject effect, Vector3 offset, bool attached = true)
	{
		if (effect == null)
		{
			return;
		}
		GameObject gameObject = SpawnManager.SpawnVFX(effect, false);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.PlaceAtPositionByAnchor(this.WorldCenter.ToVector3ZUp(0f) + offset, tk2dBaseSprite.Anchor.MiddleCenter);
		if (attached)
		{
			gameObject.transform.parent = base.transform;
			component.HeightOffGround = 0.2f;
			this.AttachRenderer(component);
		}
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0013DC48 File Offset: 0x0013BE48
	public void PlaceAtPositionByAnchor(Vector3 position, tk2dBaseSprite.Anchor anchor)
	{
		Vector2 relativePositionFromAnchor = this.GetRelativePositionFromAnchor(anchor);
		this.m_transform.position = position - relativePositionFromAnchor.ToVector3ZUp(0f);
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0013DC7C File Offset: 0x0013BE7C
	public void PlaceAtLocalPositionByAnchor(Vector3 position, tk2dBaseSprite.Anchor anchor)
	{
		Vector2 relativePositionFromAnchor = this.GetRelativePositionFromAnchor(anchor);
		this.m_transform.localPosition = position - relativePositionFromAnchor.ToVector3ZUp(0f);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0013DCB0 File Offset: 0x0013BEB0
	public void AttachRenderer(tk2dBaseSprite attachment)
	{
		if (this.attachedRenderers == null)
		{
			this.attachedRenderers = new List<tk2dBaseSprite>();
		}
		if (this.attachedRenderers.Contains(attachment))
		{
			return;
		}
		attachment.attachParent = this;
		if (!attachment.independentOrientation)
		{
			attachment.IsPerpendicular = this.IsPerpendicular;
		}
		this.attachedRenderers.Add(attachment);
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0013DD10 File Offset: 0x0013BF10
	public void DetachRenderer(tk2dBaseSprite attachment)
	{
		if (this.attachedRenderers == null)
		{
			return;
		}
		if (!this.attachedRenderers.Contains(attachment))
		{
			return;
		}
		if (attachment is tk2dSprite)
		{
			(attachment as tk2dSprite).attachParent = null;
		}
		this.attachedRenderers.Remove(attachment);
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0013DD60 File Offset: 0x0013BF60
	public void ForceBuildWithAttached()
	{
		this.ForceBuild();
		if (this.attachedRenderers != null && this.attachedRenderers.Count > 0)
		{
			for (int i = 0; i < this.attachedRenderers.Count; i++)
			{
				if (this.attachedRenderers[i] is tk2dSprite)
				{
					(this.attachedRenderers[i] as tk2dSprite).ForceBuildWithAttached();
				}
				else
				{
					this.attachedRenderers[i].ForceBuild();
				}
			}
		}
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0013DDF0 File Offset: 0x0013BFF0
	public void UpdateZDepthAttached(float parentDepth, float parentWorldY, bool parentPerpendicular)
	{
		float num = parentDepth - this.HeightOffGround;
		float num2 = this.m_transform.position.y - parentWorldY;
		num = ((!parentPerpendicular) ? (num + num2) : (num - num2));
		this.UpdateZDepthInternal(num, this.m_transform.position.y);
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x0013DE48 File Offset: 0x0013C048
	public void StackTraceAttachment()
	{
		if (this.attachedRenderers == null)
		{
			UnityEngine.Debug.Log(base.name + " has no children.");
			return;
		}
		string text = base.name + " parent of: ";
		for (int i = 0; i < this.attachedRenderers.Count; i++)
		{
			text = text + this.attachedRenderers[i].name + " ";
		}
		UnityEngine.Debug.Log(text);
		for (int j = 0; j < this.attachedRenderers.Count; j++)
		{
			this.attachedRenderers[j].StackTraceAttachment();
		}
	}

	// Token: 0x06003EFD RID: 16125 RVA: 0x0013DEF4 File Offset: 0x0013C0F4
	public void UpdateZDepthLater()
	{
		this.IsZDepthDirty = true;
	}

	// Token: 0x06003EFE RID: 16126 RVA: 0x0013DF00 File Offset: 0x0013C100
	public void UpdateZDepth()
	{
		this.IsZDepthDirty = false;
		if (this.ignoresTiltworldDepth)
		{
			return;
		}
		if (this.attachParent != null)
		{
			this.attachParent.UpdateZDepth();
			return;
		}
		if (this.m_transform == null && this)
		{
			this.m_transform = base.transform;
		}
		if (this.collectionInst == null || this.collectionInst.spriteDefinitions == null || !this.m_transform)
		{
			return;
		}
		float y = this.m_transform.position.y;
		float num;
		if (this.depthUsesTrimmedBounds)
		{
			float y2 = this.GetBounds().min.y;
			num = y + y2 + ((!this.IsPerpendicular) ? (-y2) : y2);
		}
		else
		{
			num = y;
		}
		float num2 = num - this.HeightOffGround;
		this.UpdateZDepthInternal(num2, y);
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x0013E004 File Offset: 0x0013C204
	protected void UpdateZDepthInternal(float targetZValue, float currentYValue)
	{
		this.IsZDepthDirty = false;
		Vector3 position = this.m_transform.position;
		if (position.z != targetZValue)
		{
			position.z = targetZValue;
			this.m_transform.position = position;
		}
		if (this.attachedRenderers != null && this.attachedRenderers.Count > 0)
		{
			for (int i = 0; i < this.attachedRenderers.Count; i++)
			{
				tk2dBaseSprite tk2dBaseSprite = this.attachedRenderers[i];
				if (!tk2dBaseSprite || tk2dBaseSprite.attachParent != this)
				{
					this.attachedRenderers.RemoveAt(i);
					i--;
				}
				else
				{
					if (tk2dBaseSprite != null)
					{
						tk2dBaseSprite.UpdateZDepthAttached(targetZValue, currentYValue, this.IsPerpendicular);
					}
					if (!tk2dBaseSprite.independentOrientation)
					{
						bool isPerpendicular = this.IsPerpendicular;
						if (isPerpendicular && !tk2dBaseSprite.IsPerpendicular)
						{
							tk2dBaseSprite.IsPerpendicular = true;
						}
						if (!isPerpendicular && tk2dBaseSprite.IsPerpendicular)
						{
							tk2dBaseSprite.IsPerpendicular = false;
						}
					}
				}
			}
		}
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0013E114 File Offset: 0x0013C314
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400311F RID: 12575
	public bool automaticallyManagesDepth = true;

	// Token: 0x04003120 RID: 12576
	public bool ignoresTiltworldDepth;

	// Token: 0x04003121 RID: 12577
	public bool depthUsesTrimmedBounds;

	// Token: 0x04003122 RID: 12578
	public bool allowDefaultLayer;

	// Token: 0x04003123 RID: 12579
	public tk2dBaseSprite attachParent;

	// Token: 0x04003124 RID: 12580
	public tk2dBaseSprite.SpriteMaterialOverrideMode OverrideMaterialMode;

	// Token: 0x04003125 RID: 12581
	[HideInInspector]
	public bool independentOrientation;

	// Token: 0x04003126 RID: 12582
	[Header("Decorator Data")]
	public bool autodetectFootprint = true;

	// Token: 0x04003127 RID: 12583
	public IntVector2 customFootprintOrigin;

	// Token: 0x04003128 RID: 12584
	public IntVector2 customFootprint;

	// Token: 0x04003129 RID: 12585
	protected List<tk2dBaseSprite> attachedRenderers;

	// Token: 0x0400312A RID: 12586
	protected MeshRenderer m_renderer;

	// Token: 0x0400312B RID: 12587
	private Quaternion m_cachedRotation;

	// Token: 0x0400312C RID: 12588
	protected float m_cachedYPosition;

	// Token: 0x0400312D RID: 12589
	protected int m_cachedStartingSpriteID;

	// Token: 0x0400312E RID: 12590
	public bool hasOffScreenCachedUpdate;

	// Token: 0x0400312F RID: 12591
	public tk2dSpriteCollectionData offScreenCachedCollection;

	// Token: 0x04003130 RID: 12592
	public int offScreenCachedID = -1;

	// Token: 0x04003131 RID: 12593
	[SerializeField]
	private tk2dSpriteCollectionData collection;

	// Token: 0x04003132 RID: 12594
	protected tk2dSpriteCollectionData collectionInst;

	// Token: 0x04003133 RID: 12595
	[SerializeField]
	protected Color _color = Color.white;

	// Token: 0x04003134 RID: 12596
	[SerializeField]
	protected Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04003135 RID: 12597
	[SerializeField]
	protected int _spriteId;

	// Token: 0x04003136 RID: 12598
	public BoxCollider2D boxCollider2D;

	// Token: 0x04003137 RID: 12599
	public BoxCollider boxCollider;

	// Token: 0x04003138 RID: 12600
	public MeshCollider meshCollider;

	// Token: 0x04003139 RID: 12601
	public Vector3[] meshColliderPositions;

	// Token: 0x0400313A RID: 12602
	public Mesh meshColliderMesh;

	// Token: 0x0400313C RID: 12604
	private Renderer _cachedRenderer;

	// Token: 0x0400313D RID: 12605
	protected tk2dSpriteAnimator m_cachedAnimator;

	// Token: 0x0400313E RID: 12606
	protected Transform m_transform;

	// Token: 0x0400313F RID: 12607
	protected bool m_forceNoTilt;

	// Token: 0x04003140 RID: 12608
	[NonSerialized]
	public float AdditionalFlatForwardPercentage;

	// Token: 0x04003141 RID: 12609
	[NonSerialized]
	public float AdditionalPerpForwardPercentage;

	// Token: 0x04003142 RID: 12610
	public tk2dBaseSprite.PerpendicularState CachedPerpState;

	// Token: 0x04003143 RID: 12611
	[HideInInspector]
	[SerializeField]
	protected float m_heightOffGround;

	// Token: 0x04003144 RID: 12612
	[SerializeField]
	protected int renderLayer;

	// Token: 0x04003145 RID: 12613
	[NonSerialized]
	public bool IsOutlineSprite;

	// Token: 0x04003146 RID: 12614
	public bool IsBraveOutlineSprite;

	// Token: 0x04003147 RID: 12615
	private Vector2 m_cachedScale;

	// Token: 0x04003148 RID: 12616
	public bool IsZDepthDirty;

	// Token: 0x02000BA8 RID: 2984
	public enum SpriteMaterialOverrideMode
	{
		// Token: 0x0400314A RID: 12618
		NONE,
		// Token: 0x0400314B RID: 12619
		OVERRIDE_MATERIAL_SIMPLE,
		// Token: 0x0400314C RID: 12620
		OVERRIDE_MATERIAL_COMPLEX
	}

	// Token: 0x02000BA9 RID: 2985
	public enum Anchor
	{
		// Token: 0x0400314E RID: 12622
		LowerLeft,
		// Token: 0x0400314F RID: 12623
		LowerCenter,
		// Token: 0x04003150 RID: 12624
		LowerRight,
		// Token: 0x04003151 RID: 12625
		MiddleLeft,
		// Token: 0x04003152 RID: 12626
		MiddleCenter,
		// Token: 0x04003153 RID: 12627
		MiddleRight,
		// Token: 0x04003154 RID: 12628
		UpperLeft,
		// Token: 0x04003155 RID: 12629
		UpperCenter,
		// Token: 0x04003156 RID: 12630
		UpperRight
	}

	// Token: 0x02000BAA RID: 2986
	public enum PerpendicularState
	{
		// Token: 0x04003158 RID: 12632
		UNDEFINED,
		// Token: 0x04003159 RID: 12633
		PERPENDICULAR,
		// Token: 0x0400315A RID: 12634
		FLAT
	}
}
