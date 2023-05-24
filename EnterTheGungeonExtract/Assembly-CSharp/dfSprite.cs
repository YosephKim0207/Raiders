using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[AddComponentMenu("Daikon Forge/User Interface/Sprite/Basic")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_sprite.html")]
[dfTooltip("Used to render a sprite from a Texture Atlas on the screen")]
[Serializable]
public class dfSprite : dfControl
{
	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06001635 RID: 5685 RVA: 0x000695D8 File Offset: 0x000677D8
	// (remove) Token: 0x06001636 RID: 5686 RVA: 0x00069610 File Offset: 0x00067810
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> SpriteNameChanged;

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06001637 RID: 5687 RVA: 0x00069648 File Offset: 0x00067848
	// (set) Token: 0x06001638 RID: 5688 RVA: 0x00069690 File Offset: 0x00067890
	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					return this.atlas = manager.DefaultAtlas;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06001639 RID: 5689 RVA: 0x000696B0 File Offset: 0x000678B0
	// (set) Token: 0x0600163A RID: 5690 RVA: 0x000696B8 File Offset: 0x000678B8
	public string SpriteName
	{
		get
		{
			return this.spriteName;
		}
		set
		{
			value = base.getLocalizedValue(value);
			if (value != this.spriteName)
			{
				this.spriteName = value;
				dfAtlas.ItemInfo spriteInfo = this.SpriteInfo;
				if (this.size == Vector2.zero && spriteInfo != null)
				{
					this.size = spriteInfo.sizeInPixels;
					base.updateCollider();
				}
				this.Invalidate();
				this.OnSpriteNameChanged(value);
			}
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x0600163B RID: 5691 RVA: 0x00069730 File Offset: 0x00067930
	public dfAtlas.ItemInfo SpriteInfo
	{
		get
		{
			if (this.Atlas == null)
			{
				return null;
			}
			return this.Atlas[this.spriteName];
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x0600163C RID: 5692 RVA: 0x00069768 File Offset: 0x00067968
	// (set) Token: 0x0600163D RID: 5693 RVA: 0x00069770 File Offset: 0x00067970
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

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x0600163E RID: 5694 RVA: 0x0006978C File Offset: 0x0006798C
	// (set) Token: 0x0600163F RID: 5695 RVA: 0x00069794 File Offset: 0x00067994
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

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06001640 RID: 5696 RVA: 0x000697B0 File Offset: 0x000679B0
	// (set) Token: 0x06001641 RID: 5697 RVA: 0x000697B8 File Offset: 0x000679B8
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

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06001642 RID: 5698 RVA: 0x000697EC File Offset: 0x000679EC
	// (set) Token: 0x06001643 RID: 5699 RVA: 0x000697F4 File Offset: 0x000679F4
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

	// Token: 0x06001644 RID: 5700 RVA: 0x00069810 File Offset: 0x00067A10
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.SpriteName = base.getLocalizedValue(this.spriteName);
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x0006982C File Offset: 0x00067A2C
	protected internal virtual void OnSpriteNameChanged(string value)
	{
		base.Signal("OnSpriteNameChanged", this, value);
		if (this.SpriteNameChanged != null)
		{
			this.SpriteNameChanged(this, value);
		}
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x00069854 File Offset: 0x00067A54
	public override Vector2 CalculateMinimumSize()
	{
		dfAtlas.ItemInfo spriteInfo = this.SpriteInfo;
		if (spriteInfo == null)
		{
			return Vector2.zero;
		}
		RectOffset border = spriteInfo.border;
		if (border != null && border.horizontal > 0 && border.vertical > 0)
		{
			return Vector2.Max(base.CalculateMinimumSize(), new Vector2((float)border.horizontal, (float)border.vertical));
		}
		return base.CalculateMinimumSize();
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x000698C4 File Offset: 0x00067AC4
	protected override void OnRebuildRenderData()
	{
		if (!(this.Atlas != null) || !(this.Atlas.Material != null))
		{
			return;
		}
		if (this.SpriteInfo == null)
		{
			return;
		}
		this.renderData.Material = this.OverrideMaterial ?? this.Atlas.Material;
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.Atlas,
			color = color,
			fillAmount = this.fillAmount,
			fillDirection = this.fillDirection,
			flip = this.flip,
			invertFill = this.invertFill,
			offset = this.pivot.TransformToUpperLeft(base.Size),
			pixelsToUnits = base.PixelsToUnits(),
			size = base.Size,
			spriteInfo = this.SpriteInfo
		};
		dfSprite.renderSprite(this.renderData, renderOptions);
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x000699F4 File Offset: 0x00067BF4
	internal static void renderSprite(dfRenderData data, dfSprite.RenderOptions options)
	{
		if (options.fillAmount <= 1E-45f)
		{
			return;
		}
		options.baseIndex = data.Vertices.Count;
		dfSprite.rebuildTriangles(data, options);
		dfSprite.rebuildVertices(data, options);
		dfSprite.rebuildUV(data, options);
		dfSprite.rebuildColors(data, options);
		if (options.fillAmount < 1f)
		{
			dfSprite.doFill(data, options);
		}
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x00069A5C File Offset: 0x00067C5C
	private static void rebuildTriangles(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int baseIndex = options.baseIndex;
		dfList<int> triangles = renderData.Triangles;
		triangles.EnsureCapacity(triangles.Count + dfSprite.TRIANGLE_INDICES.Length);
		for (int i = 0; i < dfSprite.TRIANGLE_INDICES.Length; i++)
		{
			triangles.Add(baseIndex + dfSprite.TRIANGLE_INDICES[i]);
		}
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x00069AB4 File Offset: 0x00067CB4
	private static void rebuildVertices(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		dfList<Vector3> vertices = renderData.Vertices;
		int baseIndex = options.baseIndex;
		float num = 0f;
		float num2 = 0f;
		float num3 = Mathf.Ceil(options.size.x);
		float num4 = Mathf.Ceil(-options.size.y);
		vertices.Add(new Vector3(num, num2, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(num3, num2, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(num3, num4, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(num, num4, 0f) * options.pixelsToUnits);
		Vector3 vector = options.offset.RoundToInt() * options.pixelsToUnits;
		Vector3[] items = vertices.Items;
		for (int i = baseIndex; i < baseIndex + 4; i++)
		{
			items[i] = (items[i] + vector).Quantize(options.pixelsToUnits);
		}
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x00069BE8 File Offset: 0x00067DE8
	private static void rebuildColors(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		dfList<Color32> colors = renderData.Colors;
		colors.Add(options.color);
		colors.Add(options.color);
		colors.Add(options.color);
		colors.Add(options.color);
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x00069C30 File Offset: 0x00067E30
	private static void rebuildUV(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		Rect region = options.spriteInfo.region;
		dfList<Vector2> uv = renderData.UV;
		uv.Add(new Vector2(region.x, region.yMax));
		uv.Add(new Vector2(region.xMax, region.yMax));
		uv.Add(new Vector2(region.xMax, region.y));
		uv.Add(new Vector2(region.x, region.y));
		Vector2 vector = Vector2.zero;
		if (options.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			vector = uv[1];
			uv[1] = uv[0];
			uv[0] = vector;
			vector = uv[3];
			uv[3] = uv[2];
			uv[2] = vector;
		}
		if (options.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			vector = uv[0];
			uv[0] = uv[3];
			uv[3] = vector;
			vector = uv[1];
			uv[1] = uv[2];
			uv[2] = vector;
		}
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x00069D58 File Offset: 0x00067F58
	private static void doFill(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int baseIndex = options.baseIndex;
		dfList<Vector3> vertices = renderData.Vertices;
		dfList<Vector2> uv = renderData.UV;
		int num = baseIndex;
		int num2 = baseIndex + 1;
		int num3 = baseIndex + 3;
		int num4 = baseIndex + 2;
		if (options.invertFill)
		{
			if (options.fillDirection == dfFillDirection.Horizontal)
			{
				num = baseIndex + 1;
				num2 = baseIndex;
				num3 = baseIndex + 2;
				num4 = baseIndex + 3;
			}
			else
			{
				num = baseIndex + 3;
				num2 = baseIndex + 2;
				num3 = baseIndex;
				num4 = baseIndex + 1;
			}
		}
		if (options.fillDirection == dfFillDirection.Horizontal)
		{
			vertices[num2] = Vector3.Lerp(vertices[num2], vertices[num], 1f - options.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num3], 1f - options.fillAmount);
			uv[num2] = Vector2.Lerp(uv[num2], uv[num], 1f - options.fillAmount);
			uv[num4] = Vector2.Lerp(uv[num4], uv[num3], 1f - options.fillAmount);
		}
		else
		{
			vertices[num3] = Vector3.Lerp(vertices[num3], vertices[num], 1f - options.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num2], 1f - options.fillAmount);
			uv[num3] = Vector2.Lerp(uv[num3], uv[num], 1f - options.fillAmount);
			uv[num4] = Vector2.Lerp(uv[num4], uv[num2], 1f - options.fillAmount);
		}
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00069F28 File Offset: 0x00068128
	public override string ToString()
	{
		if (!string.IsNullOrEmpty(this.spriteName))
		{
			return string.Format("{0} ({1})", base.name, this.spriteName);
		}
		return base.ToString();
	}

	// Token: 0x0400127C RID: 4732
	private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };

	// Token: 0x0400127E RID: 4734
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x0400127F RID: 4735
	[SerializeField]
	protected string spriteName;

	// Token: 0x04001280 RID: 4736
	[SerializeField]
	protected dfSpriteFlip flip;

	// Token: 0x04001281 RID: 4737
	[SerializeField]
	protected dfFillDirection fillDirection;

	// Token: 0x04001282 RID: 4738
	[SerializeField]
	protected float fillAmount = 1f;

	// Token: 0x04001283 RID: 4739
	[SerializeField]
	protected bool invertFill;

	// Token: 0x04001284 RID: 4740
	[NonSerialized]
	public Material OverrideMaterial;

	// Token: 0x020003F7 RID: 1015
	internal struct RenderOptions
	{
		// Token: 0x04001285 RID: 4741
		public dfAtlas atlas;

		// Token: 0x04001286 RID: 4742
		public dfAtlas.ItemInfo spriteInfo;

		// Token: 0x04001287 RID: 4743
		public Color32 color;

		// Token: 0x04001288 RID: 4744
		public float pixelsToUnits;

		// Token: 0x04001289 RID: 4745
		public Vector2 size;

		// Token: 0x0400128A RID: 4746
		public dfSpriteFlip flip;

		// Token: 0x0400128B RID: 4747
		public bool invertFill;

		// Token: 0x0400128C RID: 4748
		public dfFillDirection fillDirection;

		// Token: 0x0400128D RID: 4749
		public float fillAmount;

		// Token: 0x0400128E RID: 4750
		public Vector3 offset;

		// Token: 0x0400128F RID: 4751
		public int baseIndex;
	}
}
