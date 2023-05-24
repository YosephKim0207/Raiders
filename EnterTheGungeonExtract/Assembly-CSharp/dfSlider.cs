using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
[AddComponentMenu("Daikon Forge/User Interface/Slider")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[dfTooltip("Allows the user to select any value from a specified range by moving an indicator along a horizontal or vertical track")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_slider.html")]
[Serializable]
public class dfSlider : dfControl
{
	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06001606 RID: 5638 RVA: 0x00068854 File Offset: 0x00066A54
	// (remove) Token: 0x06001607 RID: 5639 RVA: 0x0006888C File Offset: 0x00066A8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<float> ValueChanged;

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06001608 RID: 5640 RVA: 0x000688C4 File Offset: 0x00066AC4
	// (set) Token: 0x06001609 RID: 5641 RVA: 0x0006890C File Offset: 0x00066B0C
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

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x0600160A RID: 5642 RVA: 0x0006892C File Offset: 0x00066B2C
	// (set) Token: 0x0600160B RID: 5643 RVA: 0x00068934 File Offset: 0x00066B34
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
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x0600160C RID: 5644 RVA: 0x00068954 File Offset: 0x00066B54
	// (set) Token: 0x0600160D RID: 5645 RVA: 0x0006895C File Offset: 0x00066B5C
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
				this.updateValueIndicators(this.rawValue);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x0600160E RID: 5646 RVA: 0x00068998 File Offset: 0x00066B98
	// (set) Token: 0x0600160F RID: 5647 RVA: 0x000689A0 File Offset: 0x00066BA0
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
				this.updateValueIndicators(this.rawValue);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06001610 RID: 5648 RVA: 0x000689DC File Offset: 0x00066BDC
	// (set) Token: 0x06001611 RID: 5649 RVA: 0x000689E4 File Offset: 0x00066BE4
	public float StepSize
	{
		get
		{
			return this.stepSize;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (value != this.stepSize)
			{
				this.stepSize = value;
				this.Value = this.rawValue.Quantize(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x00068A20 File Offset: 0x00066C20
	// (set) Token: 0x06001613 RID: 5651 RVA: 0x00068A28 File Offset: 0x00066C28
	public float ScrollSize
	{
		get
		{
			return this.scrollSize;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (value != this.scrollSize)
			{
				this.scrollSize = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06001614 RID: 5652 RVA: 0x00068A50 File Offset: 0x00066C50
	// (set) Token: 0x06001615 RID: 5653 RVA: 0x00068A58 File Offset: 0x00066C58
	public dfControlOrientation Orientation
	{
		get
		{
			return this.orientation;
		}
		set
		{
			if (value != this.orientation)
			{
				this.orientation = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06001616 RID: 5654 RVA: 0x00068A80 File Offset: 0x00066C80
	// (set) Token: 0x06001617 RID: 5655 RVA: 0x00068A88 File Offset: 0x00066C88
	public float Value
	{
		get
		{
			return this.rawValue;
		}
		set
		{
			value = Mathf.Max(this.minValue, Mathf.Min(this.maxValue, value.RoundToNearest(this.stepSize)));
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06001618 RID: 5656 RVA: 0x00068AD8 File Offset: 0x00066CD8
	// (set) Token: 0x06001619 RID: 5657 RVA: 0x00068AE0 File Offset: 0x00066CE0
	public dfControl Thumb
	{
		get
		{
			return this.thumb;
		}
		set
		{
			if (value != this.thumb)
			{
				this.thumb = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x0600161A RID: 5658 RVA: 0x00068B0C File Offset: 0x00066D0C
	// (set) Token: 0x0600161B RID: 5659 RVA: 0x00068B14 File Offset: 0x00066D14
	public dfControl Progress
	{
		get
		{
			return this.fillIndicator;
		}
		set
		{
			if (value != this.fillIndicator)
			{
				this.fillIndicator = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x0600161C RID: 5660 RVA: 0x00068B40 File Offset: 0x00066D40
	// (set) Token: 0x0600161D RID: 5661 RVA: 0x00068B48 File Offset: 0x00066D48
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

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x0600161E RID: 5662 RVA: 0x00068B64 File Offset: 0x00066D64
	// (set) Token: 0x0600161F RID: 5663 RVA: 0x00068B84 File Offset: 0x00066D84
	public RectOffset FillPadding
	{
		get
		{
			if (this.fillPadding == null)
			{
				this.fillPadding = new RectOffset();
			}
			return this.fillPadding;
		}
		set
		{
			if (!object.Equals(value, this.fillPadding))
			{
				this.fillPadding = value;
				this.updateValueIndicators(this.rawValue);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06001620 RID: 5664 RVA: 0x00068BB0 File Offset: 0x00066DB0
	// (set) Token: 0x06001621 RID: 5665 RVA: 0x00068BB8 File Offset: 0x00066DB8
	public Vector2 ThumbOffset
	{
		get
		{
			return this.thumbOffset;
		}
		set
		{
			if (Vector2.Distance(value, this.thumbOffset) > 1E-45f)
			{
				this.thumbOffset = value;
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06001622 RID: 5666 RVA: 0x00068BE4 File Offset: 0x00066DE4
	// (set) Token: 0x06001623 RID: 5667 RVA: 0x00068BEC File Offset: 0x00066DEC
	public bool RightToLeft
	{
		get
		{
			return this.rightToLeft;
		}
		set
		{
			if (value != this.rightToLeft)
			{
				this.rightToLeft = value;
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00068C10 File Offset: 0x00066E10
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (args.Used)
		{
			return;
		}
		if (this.Orientation == dfControlOrientation.Horizontal)
		{
			if (args.KeyCode == KeyCode.LeftArrow)
			{
				this.Value -= ((!this.rightToLeft) ? this.scrollSize : (-this.scrollSize));
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow)
			{
				this.Value += ((!this.rightToLeft) ? this.scrollSize : (-this.scrollSize));
				args.Use();
				return;
			}
		}
		else
		{
			if (args.KeyCode == KeyCode.UpArrow)
			{
				this.Value += this.ScrollSize;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.DownArrow)
			{
				this.Value -= this.ScrollSize;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x00068D18 File Offset: 0x00066F18
	public override void Start()
	{
		base.Start();
		this.updateValueIndicators(this.rawValue);
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x00068D2C File Offset: 0x00066F2C
	public override void OnEnable()
	{
		if (this.size.magnitude < 1E-45f)
		{
			this.size = new Vector2(100f, 25f);
		}
		base.OnEnable();
		this.updateValueIndicators(this.rawValue);
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00068D6C File Offset: 0x00066F6C
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		int num = ((this.orientation != dfControlOrientation.Horizontal) ? 1 : (-1));
		this.Value += this.scrollSize * args.WheelDelta * (float)num;
		args.Use();
		base.Signal("OnMouseWheel", args);
		base.raiseMouseWheelEvent(args);
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00068DC4 File Offset: 0x00066FC4
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		this.Value = this.getValueFromMouseEvent(args);
		args.Use();
		base.Signal("OnMouseMove", this, args);
		base.raiseMouseMoveEvent(args);
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00068E14 File Offset: 0x00067014
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		base.Focus(true);
		this.Value = this.getValueFromMouseEvent(args);
		args.Use();
		base.Signal("OnMouseDown", this, args);
		base.raiseMouseDownEvent(args);
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x00068E6C File Offset: 0x0006706C
	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		this.updateValueIndicators(this.rawValue);
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x00068E80 File Offset: 0x00067080
	protected internal virtual void OnValueChanged()
	{
		this.Invalidate();
		this.updateValueIndicators(this.rawValue);
		base.SignalHierarchy("OnValueChanged", new object[] { this, this.Value });
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, this.Value);
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x0600162C RID: 5676 RVA: 0x00068EE0 File Offset: 0x000670E0
	public override bool CanFocus
	{
		get
		{
			return (base.IsEnabled && base.IsVisible) || base.CanFocus;
		}
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00068F00 File Offset: 0x00067100
	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		this.renderBackground();
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x00068F30 File Offset: 0x00067130
	protected internal virtual void renderBackground()
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
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
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

	// Token: 0x0600162F RID: 5679 RVA: 0x0006903C File Offset: 0x0006723C
	private void updateValueIndicators(float rawValue)
	{
		if (Mathf.Approximately(this.MinValue, this.MaxValue))
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("Slider Min and Max values cannot be the same", this);
			}
			if (this.thumb != null)
			{
				this.thumb.IsVisible = false;
			}
			if (this.fillIndicator != null)
			{
				this.fillIndicator.IsVisible = false;
			}
			return;
		}
		if (this.thumb != null)
		{
			this.thumb.IsVisible = true;
		}
		if (this.fillIndicator != null)
		{
			this.fillIndicator.IsVisible = true;
		}
		if (this.thumb != null)
		{
			Vector3[] endPoints = this.getEndPoints(true);
			Vector3 vector = endPoints[1] - endPoints[0];
			float num = this.maxValue - this.minValue;
			float num2 = (rawValue - this.minValue) / num * vector.magnitude;
			Vector3 vector2 = this.thumbOffset * base.PixelsToUnits();
			Vector3 vector3 = endPoints[0] + vector.normalized * num2 + vector2;
			if (this.orientation == dfControlOrientation.Vertical || this.rightToLeft)
			{
				vector3 = endPoints[1] + -vector.normalized * num2 + vector2;
			}
			this.thumb.transform.position = vector3;
			this.thumb.transform.localPosition = this.thumb.transform.localPosition.WithY(0f);
		}
		if (this.fillIndicator == null)
		{
			return;
		}
		RectOffset rectOffset = this.FillPadding;
		float num3 = (rawValue - this.minValue) / (this.maxValue - this.minValue);
		Vector3 vector4 = new Vector3((float)rectOffset.left, (float)rectOffset.top);
		Vector2 vector5 = this.size - new Vector2((float)rectOffset.horizontal, (float)rectOffset.vertical);
		dfSprite dfSprite = this.fillIndicator as dfSprite;
		if (dfSprite != null && this.fillMode == dfProgressFillMode.Fill)
		{
			dfSprite.FillAmount = num3;
			dfSprite.FillDirection = ((this.orientation != dfControlOrientation.Horizontal) ? dfFillDirection.Vertical : dfFillDirection.Horizontal);
			dfSprite.InvertFill = this.rightToLeft || this.orientation == dfControlOrientation.Vertical;
		}
		else if (this.orientation == dfControlOrientation.Horizontal)
		{
			vector5.x = base.Width * num3 - (float)rectOffset.horizontal;
		}
		else
		{
			vector5.y = base.Height * num3 - (float)rectOffset.vertical;
			vector4.y = base.Height - vector5.y;
		}
		this.fillIndicator.Size = vector5;
		this.fillIndicator.RelativePosition = vector4;
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00069348 File Offset: 0x00067548
	private float getValueFromMouseEvent(dfMouseEventArgs args)
	{
		Vector3[] endPoints = this.getEndPoints(true);
		Vector3 vector = endPoints[0];
		Vector3 vector2 = endPoints[1];
		if (this.orientation == dfControlOrientation.Vertical || this.rightToLeft)
		{
			vector = endPoints[1];
			vector2 = endPoints[0];
		}
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), vector);
		Ray ray = args.Ray;
		float num = 0f;
		if (!plane.Raycast(ray, out num))
		{
			return this.rawValue;
		}
		Vector3 point = ray.GetPoint(num);
		Vector3 vector3 = dfSlider.closestPoint(vector, vector2, point, true);
		float num2 = (vector3 - vector).magnitude / (vector2 - vector).magnitude;
		return this.minValue + (this.maxValue - this.minValue) * num2;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0006943C File Offset: 0x0006763C
	private Vector3[] getEndPoints()
	{
		return this.getEndPoints(false);
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x00069448 File Offset: 0x00067648
	private Vector3[] getEndPoints(bool convertToWorld)
	{
		Vector3 vector = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector2 = new Vector3(vector.x, vector.y - this.size.y * 0.5f);
		Vector3 vector3 = vector2 + new Vector3(this.size.x, 0f);
		if (this.orientation == dfControlOrientation.Vertical)
		{
			vector2 = new Vector3(vector.x + this.size.x * 0.5f, vector.y);
			vector3 = vector2 - new Vector3(0f, this.size.y);
		}
		if (convertToWorld)
		{
			float num = base.PixelsToUnits();
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			vector2 = localToWorldMatrix.MultiplyPoint(vector2 * num);
			vector3 = localToWorldMatrix.MultiplyPoint(vector3 * num);
		}
		return new Vector3[] { vector2, vector3 };
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00069554 File Offset: 0x00067754
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

	// Token: 0x0400126E RID: 4718
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x0400126F RID: 4719
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x04001270 RID: 4720
	[SerializeField]
	protected dfControlOrientation orientation;

	// Token: 0x04001271 RID: 4721
	[SerializeField]
	protected float rawValue = 10f;

	// Token: 0x04001272 RID: 4722
	[SerializeField]
	protected float minValue;

	// Token: 0x04001273 RID: 4723
	[SerializeField]
	protected float maxValue = 100f;

	// Token: 0x04001274 RID: 4724
	[SerializeField]
	protected float stepSize = 1f;

	// Token: 0x04001275 RID: 4725
	[SerializeField]
	protected float scrollSize = 1f;

	// Token: 0x04001276 RID: 4726
	[SerializeField]
	protected dfControl thumb;

	// Token: 0x04001277 RID: 4727
	[SerializeField]
	protected dfControl fillIndicator;

	// Token: 0x04001278 RID: 4728
	[SerializeField]
	protected dfProgressFillMode fillMode = dfProgressFillMode.Fill;

	// Token: 0x04001279 RID: 4729
	[SerializeField]
	protected RectOffset fillPadding = new RectOffset();

	// Token: 0x0400127A RID: 4730
	[SerializeField]
	protected Vector2 thumbOffset = Vector2.zero;

	// Token: 0x0400127B RID: 4731
	[SerializeField]
	protected bool rightToLeft;
}
