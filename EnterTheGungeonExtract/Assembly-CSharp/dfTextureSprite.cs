using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000406 RID: 1030
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_texture_sprite.html")]
[dfCategory("Basic Controls")]
[dfTooltip("Implements a Sprite that allows the user to use any Texture and Material they wish without having to use a Texture Atlas")]
[AddComponentMenu("Daikon Forge/User Interface/Sprite/Texture")]
[ExecuteInEditMode]
[Serializable]
public class dfTextureSprite : dfControl
{
	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06001745 RID: 5957 RVA: 0x0006E7F4 File Offset: 0x0006C9F4
	// (remove) Token: 0x06001746 RID: 5958 RVA: 0x0006E82C File Offset: 0x0006CA2C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Texture> TextureChanged;

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06001747 RID: 5959 RVA: 0x0006E864 File Offset: 0x0006CA64
	// (set) Token: 0x06001748 RID: 5960 RVA: 0x0006E86C File Offset: 0x0006CA6C
	public bool CropTexture
	{
		get
		{
			return this.cropImage;
		}
		set
		{
			if (value != this.cropImage)
			{
				this.cropImage = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06001749 RID: 5961 RVA: 0x0006E888 File Offset: 0x0006CA88
	// (set) Token: 0x0600174A RID: 5962 RVA: 0x0006E890 File Offset: 0x0006CA90
	public Rect CropRect
	{
		get
		{
			return this.cropRect;
		}
		set
		{
			value = this.validateCropRect(value);
			if (value != this.cropRect)
			{
				this.cropRect = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x0600174B RID: 5963 RVA: 0x0006E8BC File Offset: 0x0006CABC
	// (set) Token: 0x0600174C RID: 5964 RVA: 0x0006E8C4 File Offset: 0x0006CAC4
	public Texture Texture
	{
		get
		{
			return this.texture;
		}
		set
		{
			if (value != this.texture)
			{
				this.texture = value;
				this.Invalidate();
				if (value != null && this.size.sqrMagnitude <= 1E-45f)
				{
					this.size = new Vector2((float)value.width, (float)value.height);
				}
				this.OnTextureChanged(value);
			}
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x0600174D RID: 5965 RVA: 0x0006E930 File Offset: 0x0006CB30
	// (set) Token: 0x0600174E RID: 5966 RVA: 0x0006E938 File Offset: 0x0006CB38
	public Material Material
	{
		get
		{
			return this.material;
		}
		set
		{
			if (value != this.material)
			{
				this.disposeCreatedMaterial();
				this.renderMaterial = null;
				this.material = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x0600174F RID: 5967 RVA: 0x0006E968 File Offset: 0x0006CB68
	// (set) Token: 0x06001750 RID: 5968 RVA: 0x0006E970 File Offset: 0x0006CB70
	public dfSpriteFlip Flip
	{
		get
		{
			return this.flip;
		}
		set
		{
			if (value != this.flip)
			{
				this.flip = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x0006E98C File Offset: 0x0006CB8C
	// (set) Token: 0x06001752 RID: 5970 RVA: 0x0006E994 File Offset: 0x0006CB94
	public dfFillDirection FillDirection
	{
		get
		{
			return this.fillDirection;
		}
		set
		{
			if (value != this.fillDirection)
			{
				this.fillDirection = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06001753 RID: 5971 RVA: 0x0006E9B0 File Offset: 0x0006CBB0
	// (set) Token: 0x06001754 RID: 5972 RVA: 0x0006E9B8 File Offset: 0x0006CBB8
	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			if (!Mathf.Approximately(value, this.fillAmount))
			{
				this.fillAmount = Mathf.Max(0f, Mathf.Min(1f, value));
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06001755 RID: 5973 RVA: 0x0006E9EC File Offset: 0x0006CBEC
	// (set) Token: 0x06001756 RID: 5974 RVA: 0x0006E9F4 File Offset: 0x0006CBF4
	public bool InvertFill
	{
		get
		{
			return this.invertFill;
		}
		set
		{
			if (value != this.invertFill)
			{
				this.invertFill = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06001757 RID: 5975 RVA: 0x0006EA10 File Offset: 0x0006CC10
	public Material RenderMaterial
	{
		get
		{
			return this.renderMaterial;
		}
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0006EA18 File Offset: 0x0006CC18
	public override void OnEnable()
	{
		base.OnEnable();
		this.renderMaterial = null;
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0006EA28 File Offset: 0x0006CC28
	public override void OnDestroy()
	{
		this.disposeCreatedMaterial();
		base.OnDestroy();
		if (this.renderMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.renderMaterial);
			this.renderMaterial = null;
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0006EA5C File Offset: 0x0006CC5C
	public override void OnDisable()
	{
		base.OnDisable();
		if (Application.isPlaying && this.renderMaterial != null)
		{
			this.disposeCreatedMaterial();
			UnityEngine.Object.DestroyImmediate(this.renderMaterial);
			this.renderMaterial = null;
		}
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0006EA98 File Offset: 0x0006CC98
	protected override void OnRebuildRenderData()
	{
		base.OnRebuildRenderData();
		if (this.texture == null)
		{
			return;
		}
		this.ensureMaterial();
		if (this.material == null)
		{
			return;
		}
		if (this.renderMaterial == null)
		{
			this.renderMaterial = new Material(this.material)
			{
				hideFlags = HideFlags.DontSave,
				name = this.material.name + " (copy)"
			};
		}
		this.renderMaterial.mainTexture = this.texture;
		this.renderData.Material = this.OverrideMaterial ?? this.renderMaterial;
		float num = base.PixelsToUnits();
		float num2 = 0f;
		float num3 = 0f;
		float num4 = this.size.x * num;
		float num5 = -this.size.y * num;
		Vector3 vector = this.pivot.TransformToUpperLeft(this.size).RoundToInt() * num;
		this.renderData.Vertices.Add(new Vector3(num2, num3, 0f) + vector);
		this.renderData.Vertices.Add(new Vector3(num4, num3, 0f) + vector);
		this.renderData.Vertices.Add(new Vector3(num4, num5, 0f) + vector);
		this.renderData.Vertices.Add(new Vector3(num2, num5, 0f) + vector);
		this.renderData.Triangles.AddRange(dfTextureSprite.TRIANGLE_INDICES);
		this.rebuildUV(this.renderData);
		Color32 color = base.ApplyOpacity(this.color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		if (this.fillAmount < 1f)
		{
			this.doFill(this.renderData);
		}
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x0006ECC0 File Offset: 0x0006CEC0
	protected virtual void disposeCreatedMaterial()
	{
		if (this.createdRuntimeMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
			this.material = null;
			this.createdRuntimeMaterial = false;
		}
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x0006ECE8 File Offset: 0x0006CEE8
	protected virtual void rebuildUV(dfRenderData renderBuffer)
	{
		dfList<Vector2> uv = renderBuffer.UV;
		if (this.cropImage)
		{
			int width = this.texture.width;
			int height = this.texture.height;
			float num = Mathf.Max(0f, Mathf.Min(this.cropRect.x, (float)width));
			float num2 = Mathf.Max(0f, Mathf.Min(this.cropRect.xMax, (float)width));
			float num3 = Mathf.Max(0f, Mathf.Min(this.cropRect.y, (float)height));
			float num4 = Mathf.Max(0f, Mathf.Min(this.cropRect.yMax, (float)height));
			uv.Add(new Vector2(num / (float)width, num4 / (float)height));
			uv.Add(new Vector2(num2 / (float)width, num4 / (float)height));
			uv.Add(new Vector2(num2 / (float)width, num3 / (float)height));
			uv.Add(new Vector2(num / (float)width, num3 / (float)height));
		}
		else
		{
			uv.Add(new Vector2(0f, 1f));
			uv.Add(new Vector2(1f, 1f));
			uv.Add(new Vector2(1f, 0f));
			uv.Add(new Vector2(0f, 0f));
		}
		Vector2 vector = Vector2.zero;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			vector = uv[1];
			uv[1] = uv[0];
			uv[0] = vector;
			vector = uv[3];
			uv[3] = uv[2];
			uv[2] = vector;
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			vector = uv[0];
			uv[0] = uv[3];
			uv[3] = vector;
			vector = uv[1];
			uv[1] = uv[2];
			uv[2] = vector;
		}
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0006EEEC File Offset: 0x0006D0EC
	protected virtual void doFill(dfRenderData renderData)
	{
		dfList<Vector3> vertices = renderData.Vertices;
		dfList<Vector2> uv = renderData.UV;
		int num = 0;
		int num2 = 1;
		int num3 = 3;
		int num4 = 2;
		if (this.invertFill)
		{
			if (this.fillDirection == dfFillDirection.Horizontal)
			{
				num = 1;
				num2 = 0;
				num3 = 2;
				num4 = 3;
			}
			else
			{
				num = 3;
				num2 = 2;
				num3 = 0;
				num4 = 1;
			}
		}
		if (this.fillDirection == dfFillDirection.Horizontal)
		{
			vertices[num2] = Vector3.Lerp(vertices[num2], vertices[num], 1f - this.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num3], 1f - this.fillAmount);
			uv[num2] = Vector2.Lerp(uv[num2], uv[num], 1f - this.fillAmount);
			uv[num4] = Vector2.Lerp(uv[num4], uv[num3], 1f - this.fillAmount);
		}
		else
		{
			vertices[num3] = Vector3.Lerp(vertices[num3], vertices[num], 1f - this.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num2], 1f - this.fillAmount);
			uv[num3] = Vector2.Lerp(uv[num3], uv[num], 1f - this.fillAmount);
			uv[num4] = Vector2.Lerp(uv[num4], uv[num2], 1f - this.fillAmount);
		}
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x0006F090 File Offset: 0x0006D290
	private Rect validateCropRect(Rect rect)
	{
		if (this.texture == null)
		{
			return default(Rect);
		}
		int width = this.texture.width;
		int height = this.texture.height;
		float num = Mathf.Max(0f, Mathf.Min(rect.x, (float)width));
		float num2 = Mathf.Max(0f, Mathf.Min(rect.y, (float)height));
		float num3 = Mathf.Max(0f, Mathf.Min(rect.width, (float)width));
		float num4 = Mathf.Max(0f, Mathf.Min(rect.height, (float)height));
		Rect rect2 = new Rect(num, num2, num3, num4);
		return rect2;
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0006F148 File Offset: 0x0006D348
	protected internal virtual void OnTextureChanged(Texture value)
	{
		base.SignalHierarchy("OnTextureChanged", new object[] { this, value });
		if (this.TextureChanged != null)
		{
			this.TextureChanged(this, value);
		}
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x0006F17C File Offset: 0x0006D37C
	private void ensureMaterial()
	{
		if (this.material != null || this.texture == null)
		{
			return;
		}
		Shader shader = Shader.Find("Daikon Forge/Default UI Shader");
		if (shader == null)
		{
			UnityEngine.Debug.LogError("Failed to find default shader");
			return;
		}
		this.material = new Material(shader)
		{
			name = "Default Texture Shader",
			hideFlags = HideFlags.DontSave,
			mainTexture = this.texture
		};
		this.createdRuntimeMaterial = true;
	}

	// Token: 0x040012D7 RID: 4823
	private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };

	// Token: 0x040012D9 RID: 4825
	[SerializeField]
	protected Texture texture;

	// Token: 0x040012DA RID: 4826
	[SerializeField]
	protected Material material;

	// Token: 0x040012DB RID: 4827
	[SerializeField]
	protected dfSpriteFlip flip;

	// Token: 0x040012DC RID: 4828
	[SerializeField]
	protected dfFillDirection fillDirection;

	// Token: 0x040012DD RID: 4829
	[SerializeField]
	protected float fillAmount = 1f;

	// Token: 0x040012DE RID: 4830
	[SerializeField]
	protected bool invertFill;

	// Token: 0x040012DF RID: 4831
	[SerializeField]
	protected Rect cropRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x040012E0 RID: 4832
	[SerializeField]
	protected bool cropImage;

	// Token: 0x040012E1 RID: 4833
	private bool createdRuntimeMaterial;

	// Token: 0x040012E2 RID: 4834
	private Material renderMaterial;

	// Token: 0x040012E3 RID: 4835
	[NonSerialized]
	public Material OverrideMaterial;
}
