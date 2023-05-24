using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003EB RID: 1003
[dfCategory("Basic Controls")]
[dfTooltip("Implements a progress bar that can be used to display the progress from a start value toward an end value, such as the amount of work completed or a player's progress toward some goal, etc.")]
[AddComponentMenu("Daikon Forge/User Interface/Progress Bar")]
[ExecuteInEditMode]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_progress_bar.html")]
[Serializable]
public class dfProgressBar : dfControl
{
	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06001515 RID: 5397 RVA: 0x00061CE0 File Offset: 0x0005FEE0
	// (remove) Token: 0x06001516 RID: 5398 RVA: 0x00061D18 File Offset: 0x0005FF18
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<float> ValueChanged;

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06001517 RID: 5399 RVA: 0x00061D50 File Offset: 0x0005FF50
	// (set) Token: 0x06001518 RID: 5400 RVA: 0x00061D98 File Offset: 0x0005FF98
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

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06001519 RID: 5401 RVA: 0x00061DB8 File Offset: 0x0005FFB8
	// (set) Token: 0x0600151A RID: 5402 RVA: 0x00061DC0 File Offset: 0x0005FFC0
	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.setDefaultSize(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x0600151B RID: 5403 RVA: 0x00061DE8 File Offset: 0x0005FFE8
	// (set) Token: 0x0600151C RID: 5404 RVA: 0x00061DF0 File Offset: 0x0005FFF0
	public string ProgressSprite
	{
		get
		{
			return this.progressSprite;
		}
		set
		{
			if (value != this.progressSprite)
			{
				this.progressSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x0600151D RID: 5405 RVA: 0x00061E10 File Offset: 0x00060010
	// (set) Token: 0x0600151E RID: 5406 RVA: 0x00061E18 File Offset: 0x00060018
	public Color32 ProgressColor
	{
		get
		{
			return this.progressColor;
		}
		set
		{
			if (!object.Equals(value, this.progressColor))
			{
				this.progressColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x00061E44 File Offset: 0x00060044
	// (set) Token: 0x06001520 RID: 5408 RVA: 0x00061E4C File Offset: 0x0006004C
	public float MinValue
	{
		get
		{
			return this.minValue;
		}
		set
		{
			if (value != this.minValue)
			{
				this.minValue = value;
				if (this.rawValue < value)
				{
					this.Value = value;
				}
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06001521 RID: 5409 RVA: 0x00061E7C File Offset: 0x0006007C
	// (set) Token: 0x06001522 RID: 5410 RVA: 0x00061E84 File Offset: 0x00060084
	public float MaxValue
	{
		get
		{
			return this.maxValue;
		}
		set
		{
			if (value != this.maxValue)
			{
				this.maxValue = value;
				if (this.rawValue > value)
				{
					this.Value = value;
				}
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06001523 RID: 5411 RVA: 0x00061EB4 File Offset: 0x000600B4
	// (set) Token: 0x06001524 RID: 5412 RVA: 0x00061EBC File Offset: 0x000600BC
	public float Value
	{
		get
		{
			return this.rawValue;
		}
		set
		{
			value = Mathf.Max(this.minValue, Mathf.Min(this.maxValue, value));
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x00061EF8 File Offset: 0x000600F8
	// (set) Token: 0x06001526 RID: 5414 RVA: 0x00061F00 File Offset: 0x00060100
	public dfProgressFillMode FillMode
	{
		get
		{
			return this.fillMode;
		}
		set
		{
			if (value != this.fillMode)
			{
				this.fillMode = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x00061F1C File Offset: 0x0006011C
	// (set) Token: 0x06001528 RID: 5416 RVA: 0x00061F24 File Offset: 0x00060124
	public dfFillDirection ProgressFillDirection
	{
		get
		{
			return this.progressFillDirection;
		}
		set
		{
			if (value != this.progressFillDirection)
			{
				this.progressFillDirection = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06001529 RID: 5417 RVA: 0x00061F40 File Offset: 0x00060140
	// (set) Token: 0x0600152A RID: 5418 RVA: 0x00061F60 File Offset: 0x00060160
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
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x0600152B RID: 5419 RVA: 0x00061F80 File Offset: 0x00060180
	// (set) Token: 0x0600152C RID: 5420 RVA: 0x00061F88 File Offset: 0x00060188
	public bool ActAsSlider
	{
		get
		{
			return this.actAsSlider;
		}
		set
		{
			this.actAsSlider = value;
		}
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00061F94 File Offset: 0x00060194
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				float num = (this.maxValue - this.minValue) * 0.1f;
				this.Value += num * (float)Mathf.RoundToInt(-args.WheelDelta);
				args.Use();
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x00062004 File Offset: 0x00060204
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				if (args.Buttons.IsSet(dfMouseButtons.Left))
				{
					this.Value = this.getValueFromMouseEvent(args);
					args.Use();
				}
			}
		}
		finally
		{
			base.OnMouseMove(args);
		}
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x00062068 File Offset: 0x00060268
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				if (args.Buttons.IsSet(dfMouseButtons.Left))
				{
					base.Focus(true);
					this.Value = this.getValueFromMouseEvent(args);
					args.Use();
				}
			}
		}
		finally
		{
			base.OnMouseDown(args);
		}
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x000620D4 File Offset: 0x000602D4
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				float num = (this.maxValue - this.minValue) * 0.1f;
				if (args.KeyCode == KeyCode.LeftArrow)
				{
					this.Value -= num;
					args.Use();
				}
				else if (args.KeyCode == KeyCode.RightArrow)
				{
					this.Value += num;
					args.Use();
				}
			}
		}
		finally
		{
			base.OnKeyDown(args);
		}
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x00062174 File Offset: 0x00060374
	protected internal virtual void OnValueChanged()
	{
		this.Invalidate();
		base.SignalHierarchy("OnValueChanged", new object[] { this, this.Value });
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, this.Value);
		}
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x000621C8 File Offset: 0x000603C8
	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		this.renderBackground();
		this.renderProgressFill();
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00062200 File Offset: 0x00060400
	private void renderProgressFill()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.progressSprite];
		if (itemInfo == null)
		{
			return;
		}
		Vector3 vector = new Vector3((float)this.padding.left, (float)(-(float)this.padding.top));
		Vector2 vector2 = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		float num = 1f;
		float num2 = this.maxValue - this.minValue;
		float num3 = (this.rawValue - this.minValue) / num2;
		dfProgressFillMode dfProgressFillMode = this.fillMode;
		if (dfProgressFillMode != dfProgressFillMode.Stretch || vector2.x * num3 < (float)itemInfo.border.horizontal)
		{
		}
		if (dfProgressFillMode == dfProgressFillMode.Fill)
		{
			num = num3;
		}
		else
		{
			vector2.x = Mathf.Max((float)itemInfo.border.horizontal, vector2.x * num3);
		}
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? base.DisabledColor : this.ProgressColor);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = num,
			flip = dfSpriteFlip.None,
			offset = this.pivot.TransformToUpperLeft(base.Size) + vector,
			pixelsToUnits = base.PixelsToUnits(),
			size = vector2,
			spriteInfo = itemInfo
		};
		if (this.progressFillDirection == dfFillDirection.Vertical)
		{
			renderOptions.invertFill = true;
		}
		renderOptions.fillDirection = this.progressFillDirection;
		if (itemInfo.border.horizontal == 0 && itemInfo.border.vertical == 0)
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x0006240C File Offset: 0x0006060C
	private void renderBackground()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.backgroundSprite];
		if (itemInfo == null)
		{
			return;
		}
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? base.DisabledColor : base.Color);
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

	// Token: 0x06001535 RID: 5429 RVA: 0x00062518 File Offset: 0x00060718
	private float getValueFromMouseEvent(dfMouseEventArgs args)
	{
		Vector3[] endPoints = this.getEndPoints(true);
		Vector3 vector = endPoints[0];
		Vector3 vector2 = endPoints[1];
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), vector);
		Ray ray = args.Ray;
		float num = 0f;
		if (!plane.Raycast(ray, out num))
		{
			return this.rawValue;
		}
		Vector3 vector3 = ray.origin + ray.direction * num;
		Vector3 vector4 = dfProgressBar.closestPoint(vector, vector2, vector3, true);
		float num2 = (vector4 - vector).magnitude / (vector2 - vector).magnitude;
		return this.minValue + (this.maxValue - this.minValue) * num2;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000625EC File Offset: 0x000607EC
	private Vector3[] getEndPoints()
	{
		return this.getEndPoints(false);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000625F8 File Offset: 0x000607F8
	private Vector3[] getEndPoints(bool convertToWorld)
	{
		Vector3 vector = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector2 = new Vector3(vector.x + (float)this.padding.left, vector.y - this.size.y * 0.5f);
		Vector3 vector3 = vector2 + new Vector3(this.size.x - (float)this.padding.right, 0f);
		if (convertToWorld)
		{
			float num = base.PixelsToUnits();
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			vector2 = localToWorldMatrix.MultiplyPoint(vector2 * num);
			vector3 = localToWorldMatrix.MultiplyPoint(vector3 * num);
		}
		return new Vector3[] { vector2, vector3 };
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000626CC File Offset: 0x000608CC
	private static Vector3 closestPoint(Vector3 start, Vector3 end, Vector3 test, bool clamp)
	{
		Vector3 vector = test - start;
		Vector3 vector2 = (end - start).normalized;
		float magnitude = (end - start).magnitude;
		float num = Vector3.Dot(vector2, vector);
		if (clamp)
		{
			if (num < 0f)
			{
				return start;
			}
			if (num > magnitude)
			{
				return end;
			}
		}
		vector2 *= num;
		return start + vector2;
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x0006273C File Offset: 0x0006093C
	private void setDefaultSize(string spriteName)
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[spriteName];
		if (this.size == Vector2.zero && itemInfo != null)
		{
			base.Size = itemInfo.sizeInPixels;
		}
	}

	// Token: 0x0400120C RID: 4620
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x0400120D RID: 4621
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x0400120E RID: 4622
	[SerializeField]
	protected string progressSprite;

	// Token: 0x0400120F RID: 4623
	[SerializeField]
	protected Color32 progressColor = UnityEngine.Color.white;

	// Token: 0x04001210 RID: 4624
	[SerializeField]
	protected float rawValue = 0.25f;

	// Token: 0x04001211 RID: 4625
	[SerializeField]
	protected float minValue;

	// Token: 0x04001212 RID: 4626
	[SerializeField]
	protected float maxValue = 1f;

	// Token: 0x04001213 RID: 4627
	[SerializeField]
	protected dfProgressFillMode fillMode;

	// Token: 0x04001214 RID: 4628
	[SerializeField]
	protected dfFillDirection progressFillDirection;

	// Token: 0x04001215 RID: 4629
	[SerializeField]
	protected RectOffset padding = new RectOffset();

	// Token: 0x04001216 RID: 4630
	[SerializeField]
	protected bool actAsSlider;
}
