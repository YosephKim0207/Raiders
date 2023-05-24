using System;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
[dfCategory("Basic Controls")]
[ExecuteInEditMode]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_panel.html")]
[dfTooltip("Basic container control to facilitate user interface layout")]
[AddComponentMenu("Daikon Forge/User Interface/Containers/Panel")]
[Serializable]
public class dfPanel : dfControl
{
	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x060014F0 RID: 5360 RVA: 0x000610FC File Offset: 0x0005F2FC
	// (set) Token: 0x060014F1 RID: 5361 RVA: 0x00061144 File Offset: 0x0005F344
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

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00061164 File Offset: 0x0005F364
	// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0006116C File Offset: 0x0005F36C
	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			value = base.getLocalizedValue(value);
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x060014F4 RID: 5364 RVA: 0x00061198 File Offset: 0x0005F398
	// (set) Token: 0x060014F5 RID: 5365 RVA: 0x000611A0 File Offset: 0x0005F3A0
	public Color32 BackgroundColor
	{
		get
		{
			return this.backgroundColor;
		}
		set
		{
			if (!object.Equals(value, this.backgroundColor))
			{
				this.backgroundColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x060014F6 RID: 5366 RVA: 0x000611CC File Offset: 0x0005F3CC
	// (set) Token: 0x060014F7 RID: 5367 RVA: 0x000611EC File Offset: 0x0005F3EC
	public RectOffset Padding
	{
		get
		{
			if (this.padding == null)
			{
				this.padding = new RectOffset();
			}
			return this.padding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x00061214 File Offset: 0x0005F414
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.BackgroundSprite = base.getLocalizedValue(this.backgroundSprite);
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x00061230 File Offset: 0x0005F430
	protected internal override RectOffset GetClipPadding()
	{
		return this.padding ?? dfRectOffsetExtensions.Empty;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x00061244 File Offset: 0x0005F444
	protected internal override Plane[] GetClippingPlanes()
	{
		if (!base.ClipChildren)
		{
			return null;
		}
		Vector3[] corners = base.GetCorners();
		Vector3 vector = base.transform.TransformDirection(Vector3.right);
		Vector3 vector2 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector3 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector4 = base.transform.TransformDirection(Vector3.down);
		float num = base.PixelsToUnits();
		RectOffset rectOffset = this.Padding;
		corners[0] += vector * (float)rectOffset.left * num + vector4 * (float)rectOffset.top * num;
		corners[1] += vector2 * (float)rectOffset.right * num + vector4 * (float)rectOffset.top * num;
		corners[2] += vector * (float)rectOffset.left * num + vector3 * (float)rectOffset.bottom * num;
		return new Plane[]
		{
			new Plane(vector, corners[0]),
			new Plane(vector2, corners[1]),
			new Plane(vector3, corners[2]),
			new Plane(vector4, corners[0])
		};
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00061410 File Offset: 0x0005F610
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size == Vector2.zero)
		{
			base.SuspendLayout();
			Camera camera = base.GetCamera();
			base.Size = new Vector3((float)(camera.pixelWidth / 2), (float)(camera.pixelHeight / 2));
			base.ResumeLayout();
		}
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00061470 File Offset: 0x0005F670
	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null || string.IsNullOrEmpty(this.backgroundSprite))
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.backgroundSprite];
		if (itemInfo == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		Color32 color = base.ApplyOpacity(this.BackgroundColor);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = 1f,
			flip = dfSpriteFlip.None,
			offset = this.pivot.TransformToUpperLeft(base.Size),
			pixelsToUnits = base.PixelsToUnits(),
			size = base.Size,
			spriteInfo = itemInfo
		};
		if (itemInfo.border.horizontal == 0 && itemInfo.border.vertical == 0)
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x0006158C File Offset: 0x0005F78C
	public void FitToContents()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			Vector2 vector2 = dfControl.RelativePosition + dfControl.Size;
			vector = Vector2.Max(vector, vector2);
		}
		base.Size = vector + new Vector2((float)this.padding.right, (float)this.padding.bottom);
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00061624 File Offset: 0x0005F824
	public void CenterChildControls()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector = Vector2.one * float.MaxValue;
		Vector2 vector2 = Vector2.one * float.MinValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			Vector2 vector3 = dfControl.RelativePosition;
			Vector2 vector4 = vector3 + dfControl.Size;
			vector = Vector2.Min(vector, vector3);
			vector2 = Vector2.Max(vector2, vector4);
		}
		Vector2 vector5 = vector2 - vector;
		Vector2 vector6 = (base.Size - vector5) * 0.5f;
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl dfControl2 = this.controls[j];
			dfControl2.RelativePosition = dfControl2.RelativePosition - vector + vector6;
		}
	}

	// Token: 0x04001200 RID: 4608
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001201 RID: 4609
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x04001202 RID: 4610
	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	// Token: 0x04001203 RID: 4611
	[SerializeField]
	protected RectOffset padding = new RectOffset();
}
