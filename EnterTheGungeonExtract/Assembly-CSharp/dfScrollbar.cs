using System;
using System.Diagnostics;
using InControl;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
[AddComponentMenu("Daikon Forge/User Interface/Scrollbar")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[dfTooltip("Implements a common Scrollbar control")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_scrollbar.html")]
[Serializable]
public class dfScrollbar : dfControl
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06001560 RID: 5472 RVA: 0x00063ADC File Offset: 0x00061CDC
	// (remove) Token: 0x06001561 RID: 5473 RVA: 0x00063B14 File Offset: 0x00061D14
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<float> ValueChanged;

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06001562 RID: 5474 RVA: 0x00063B4C File Offset: 0x00061D4C
	// (set) Token: 0x06001563 RID: 5475 RVA: 0x00063B94 File Offset: 0x00061D94
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

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06001564 RID: 5476 RVA: 0x00063BB4 File Offset: 0x00061DB4
	// (set) Token: 0x06001565 RID: 5477 RVA: 0x00063BBC File Offset: 0x00061DBC
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
			}
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06001566 RID: 5478 RVA: 0x00063BEC File Offset: 0x00061DEC
	// (set) Token: 0x06001567 RID: 5479 RVA: 0x00063BF4 File Offset: 0x00061DF4
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
			}
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06001568 RID: 5480 RVA: 0x00063C24 File Offset: 0x00061E24
	// (set) Token: 0x06001569 RID: 5481 RVA: 0x00063C2C File Offset: 0x00061E2C
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
				this.Value = this.Value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x0600156A RID: 5482 RVA: 0x00063C60 File Offset: 0x00061E60
	// (set) Token: 0x0600156B RID: 5483 RVA: 0x00063C68 File Offset: 0x00061E68
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
			}
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x0600156C RID: 5484 RVA: 0x00063CA4 File Offset: 0x00061EA4
	// (set) Token: 0x0600156D RID: 5485 RVA: 0x00063CAC File Offset: 0x00061EAC
	public float IncrementAmount
	{
		get
		{
			return this.increment;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (!Mathf.Approximately(value, this.increment))
			{
				this.increment = value;
			}
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x0600156E RID: 5486 RVA: 0x00063CD4 File Offset: 0x00061ED4
	// (set) Token: 0x0600156F RID: 5487 RVA: 0x00063CDC File Offset: 0x00061EDC
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
			}
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06001570 RID: 5488 RVA: 0x00063CF8 File Offset: 0x00061EF8
	// (set) Token: 0x06001571 RID: 5489 RVA: 0x00063D00 File Offset: 0x00061F00
	public float Value
	{
		get
		{
			return this.rawValue;
		}
		set
		{
			value = this.adjustValue(value);
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
			this.updateThumb(this.rawValue);
			this.doAutoHide();
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06001572 RID: 5490 RVA: 0x00063D3C File Offset: 0x00061F3C
	// (set) Token: 0x06001573 RID: 5491 RVA: 0x00063D44 File Offset: 0x00061F44
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
			}
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06001574 RID: 5492 RVA: 0x00063D64 File Offset: 0x00061F64
	// (set) Token: 0x06001575 RID: 5493 RVA: 0x00063D6C File Offset: 0x00061F6C
	public dfControl Track
	{
		get
		{
			return this.track;
		}
		set
		{
			if (value != this.track)
			{
				this.track = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06001576 RID: 5494 RVA: 0x00063D8C File Offset: 0x00061F8C
	// (set) Token: 0x06001577 RID: 5495 RVA: 0x00063D94 File Offset: 0x00061F94
	public dfControl IncButton
	{
		get
		{
			return this.incButton;
		}
		set
		{
			if (value != this.incButton)
			{
				this.incButton = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06001578 RID: 5496 RVA: 0x00063DB4 File Offset: 0x00061FB4
	// (set) Token: 0x06001579 RID: 5497 RVA: 0x00063DBC File Offset: 0x00061FBC
	public dfControl DecButton
	{
		get
		{
			return this.decButton;
		}
		set
		{
			if (value != this.decButton)
			{
				this.decButton = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x0600157A RID: 5498 RVA: 0x00063DDC File Offset: 0x00061FDC
	// (set) Token: 0x0600157B RID: 5499 RVA: 0x00063DFC File Offset: 0x00061FFC
	public RectOffset ThumbPadding
	{
		get
		{
			if (this.thumbPadding == null)
			{
				this.thumbPadding = new RectOffset();
			}
			return this.thumbPadding;
		}
		set
		{
			if (this.orientation == dfControlOrientation.Horizontal)
			{
				int num = 0;
				value.bottom = num;
				value.top = num;
			}
			else
			{
				int num = 0;
				value.right = num;
				value.left = num;
			}
			if (!object.Equals(value, this.thumbPadding))
			{
				this.thumbPadding = value;
				this.updateThumb(this.rawValue);
			}
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x0600157C RID: 5500 RVA: 0x00063E60 File Offset: 0x00062060
	// (set) Token: 0x0600157D RID: 5501 RVA: 0x00063E68 File Offset: 0x00062068
	public bool AutoHide
	{
		get
		{
			return this.autoHide;
		}
		set
		{
			if (value != this.autoHide)
			{
				this.autoHide = value;
				this.Invalidate();
				this.doAutoHide();
			}
		}
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x00063E8C File Offset: 0x0006208C
	public override Vector2 CalculateMinimumSize()
	{
		Vector2[] array = new Vector2[3];
		if (this.decButton != null)
		{
			array[0] = this.decButton.CalculateMinimumSize();
		}
		if (this.incButton != null)
		{
			array[1] = this.incButton.CalculateMinimumSize();
		}
		if (this.thumb != null)
		{
			array[2] = this.thumb.CalculateMinimumSize();
		}
		Vector2 zero = Vector2.zero;
		if (this.orientation == dfControlOrientation.Horizontal)
		{
			zero.x = array[0].x + array[1].x + array[2].x;
			zero.y = Mathf.Max(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y
			});
		}
		else
		{
			zero.x = Mathf.Max(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x
			});
			zero.y = array[0].y + array[1].y + array[2].y;
		}
		return Vector2.Max(zero, base.CalculateMinimumSize());
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x0600157F RID: 5503 RVA: 0x00064014 File Offset: 0x00062214
	public override bool CanFocus
	{
		get
		{
			return (base.IsEnabled && base.IsVisible) || base.CanFocus;
		}
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x00064034 File Offset: 0x00062234
	protected override void OnRebuildRenderData()
	{
		this.updateThumb(this.rawValue);
		base.OnRebuildRenderData();
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00064048 File Offset: 0x00062248
	public override void Start()
	{
		base.Start();
		this.attachEvents();
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x00064058 File Offset: 0x00062258
	public override void Update()
	{
		base.Update();
		if (this.ControlledByRightStick && InputManager.ActiveDevice != null)
		{
			float y = InputManager.ActiveDevice.RightStick.Y;
			if (Mathf.Abs(y) > 0.1f)
			{
				this.IsBeingMovedByRightStick = true;
				this.Value += this.IncrementAmount * y * -6f * GameManager.INVARIANT_DELTA_TIME;
				this.IsBeingMovedByRightStick = false;
			}
		}
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x000640D0 File Offset: 0x000622D0
	public override void OnDisable()
	{
		base.OnDisable();
		this.detachEvents();
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x000640E0 File Offset: 0x000622E0
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.detachEvents();
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000640F0 File Offset: 0x000622F0
	private void attachEvents()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.IncButton != null)
		{
			this.IncButton.MouseDown += this.incrementPressed;
			this.IncButton.MouseHover += this.incrementPressed;
		}
		if (this.DecButton != null)
		{
			this.DecButton.MouseDown += this.decrementPressed;
			this.DecButton.MouseHover += this.decrementPressed;
		}
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x00064188 File Offset: 0x00062388
	private void detachEvents()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.IncButton != null)
		{
			this.IncButton.MouseDown -= this.incrementPressed;
			this.IncButton.MouseHover -= this.incrementPressed;
		}
		if (this.DecButton != null)
		{
			this.DecButton.MouseDown -= this.decrementPressed;
			this.DecButton.MouseHover -= this.decrementPressed;
		}
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x00064220 File Offset: 0x00062420
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.Orientation == dfControlOrientation.Horizontal)
		{
			if (args.KeyCode == KeyCode.LeftArrow)
			{
				this.Value -= this.IncrementAmount;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow)
			{
				this.Value += this.IncrementAmount;
				args.Use();
				return;
			}
		}
		else
		{
			if (args.KeyCode == KeyCode.UpArrow)
			{
				this.Value -= this.IncrementAmount;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.DownArrow)
			{
				this.Value += this.IncrementAmount;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x000642EC File Offset: 0x000624EC
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		if (args.Used)
		{
			return;
		}
		this.Value += this.IncrementAmount * -args.WheelDelta;
		args.Use();
		base.Signal("OnMouseWheel", this, args);
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x0006432C File Offset: 0x0006252C
	protected internal override void OnMouseHover(dfMouseEventArgs args)
	{
		bool flag = args.Source == this.incButton || args.Source == this.decButton || args.Source == this.thumb;
		if (flag)
		{
			return;
		}
		if (args.Source != this.track || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseHover(args);
			return;
		}
		this.updateFromTrackClick(args);
		args.Use();
		base.Signal("OnMouseHover", this, args);
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000643CC File Offset: 0x000625CC
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (args.Source == this.incButton || args.Source == this.decButton)
		{
			return;
		}
		if ((args.Source != this.track && args.Source != this.thumb) || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		float valueFromMouseEvent = this.getValueFromMouseEvent(args);
		float num = this.thumb.Height / 2f;
		float num2 = Mathf.InverseLerp(this.minValue, this.maxValue, valueFromMouseEvent);
		float num3 = Mathf.Lerp(this.minValue - num, this.maxValue - this.scrollSize + num, num2);
		this.Value = num3;
		args.Use();
		base.Signal("OnMouseMove", this, args);
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000644B4 File Offset: 0x000626B4
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.Focus(true);
		}
		if (args.Source == this.incButton || args.Source == this.decButton)
		{
			return;
		}
		if ((args.Source != this.track && args.Source != this.thumb) || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseDown(args);
			return;
		}
		if (args.Source == this.thumb)
		{
			RaycastHit raycastHit;
			this.thumb.GetComponent<Collider>().Raycast(args.Ray, out raycastHit, 1000f);
			Vector3 vector = this.thumb.transform.position + this.thumb.Pivot.TransformToCenter(this.thumb.Size * base.PixelsToUnits());
			this.thumbMouseOffset = vector - raycastHit.point;
		}
		else
		{
			this.updateFromTrackClick(args);
		}
		args.Use();
		base.Signal("OnMouseDown", this, args);
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x000645F0 File Offset: 0x000627F0
	protected internal virtual void OnValueChanged()
	{
		this.doAutoHide();
		this.Invalidate();
		base.SignalHierarchy("OnValueChanged", new object[] { this, this.Value });
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, this.Value);
		}
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x0006464C File Offset: 0x0006284C
	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		this.updateThumb(this.rawValue);
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x00064660 File Offset: 0x00062860
	private void doAutoHide()
	{
		if (!this.autoHide || !Application.isPlaying)
		{
			return;
		}
		if (Mathf.CeilToInt(this.ScrollSize) >= Mathf.CeilToInt(this.maxValue - this.minValue))
		{
			base.Hide();
		}
		else
		{
			base.Show();
		}
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x000646B8 File Offset: 0x000628B8
	private void incrementPressed(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			this.Value += this.IncrementAmount;
			args.Use();
		}
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x000646E4 File Offset: 0x000628E4
	private void decrementPressed(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			this.Value -= this.IncrementAmount;
			args.Use();
		}
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x00064710 File Offset: 0x00062910
	private void updateFromTrackClick(dfMouseEventArgs args)
	{
		float valueFromMouseEvent = this.getValueFromMouseEvent(args);
		if (valueFromMouseEvent > this.rawValue + this.scrollSize)
		{
			this.Value += this.scrollSize;
		}
		else if (valueFromMouseEvent < this.rawValue)
		{
			this.Value -= this.scrollSize;
		}
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00064770 File Offset: 0x00062970
	private float adjustValue(float value)
	{
		float num = Mathf.Max(this.maxValue - this.minValue, 0f);
		float num2 = Mathf.Max(num - this.scrollSize, 0f) + this.minValue;
		float num3 = Mathf.Max(Mathf.Min(num2, value), this.minValue);
		return num3.Quantize(this.stepSize);
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x000647D0 File Offset: 0x000629D0
	private void updateThumb(float rawValue)
	{
		if (this.controls.Count == 0 || this.thumb == null || this.track == null || !base.IsVisible)
		{
			return;
		}
		float num = this.maxValue - this.minValue;
		if (num <= 0f || num <= this.scrollSize)
		{
			this.thumb.IsVisible = false;
			return;
		}
		this.thumb.IsVisible = true;
		float num2 = ((this.orientation != dfControlOrientation.Horizontal) ? this.track.Height : this.track.Width);
		float num3 = ((this.orientation != dfControlOrientation.Horizontal) ? Mathf.Min(this.thumb.MaximumSize.y, Mathf.Max(this.scrollSize / num * num2, this.thumb.MinimumSize.y)) : Mathf.Min(this.thumb.MaximumSize.x, Mathf.Max(this.scrollSize / num * num2, this.thumb.MinimumSize.x)));
		Vector2 vector = ((this.orientation != dfControlOrientation.Horizontal) ? new Vector2(this.thumb.Width, num3) : new Vector2(num3, this.thumb.Height));
		if (this.Orientation == dfControlOrientation.Horizontal)
		{
			vector.x -= (float)this.thumbPadding.horizontal;
		}
		else
		{
			vector.y -= (float)this.thumbPadding.vertical;
		}
		this.thumb.Size = vector;
		float num4 = (rawValue - this.minValue) / (num - this.scrollSize);
		float num5 = num4 * (num2 - num3);
		Vector3 vector2 = ((this.orientation != dfControlOrientation.Horizontal) ? Vector3.up : Vector3.right);
		Vector3 vector3 = ((this.Orientation != dfControlOrientation.Horizontal) ? new Vector3((this.track.Width - this.thumb.Width) * 0.5f, 0f) : new Vector3(0f, (this.track.Height - this.thumb.Height) * 0.5f));
		if (this.Orientation == dfControlOrientation.Horizontal)
		{
			vector3.x = (float)this.thumbPadding.left;
		}
		else
		{
			vector3.y = (float)this.thumbPadding.top;
		}
		if (this.thumb.Parent == this)
		{
			this.thumb.RelativePosition = this.track.RelativePosition + vector3 + vector2 * num5;
		}
		else
		{
			this.thumb.RelativePosition = vector2 * num5 + vector3;
		}
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x00064ABC File Offset: 0x00062CBC
	private float getValueFromMouseEvent(dfMouseEventArgs args)
	{
		Vector3[] corners = this.track.GetCorners();
		Vector3 vector = corners[0];
		Vector3 vector2 = corners[(this.orientation != dfControlOrientation.Horizontal) ? 2 : 1];
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), vector);
		Ray ray = args.Ray;
		float num = 0f;
		if (!plane.Raycast(ray, out num))
		{
			return this.rawValue;
		}
		Vector3 vector3 = ray.origin + ray.direction * num;
		if (args.Source == this.thumb)
		{
			vector3 += this.thumbMouseOffset;
		}
		Vector3 vector4 = dfScrollbar.closestPoint(vector, vector2, vector3, true);
		float num2 = (vector4 - vector).magnitude / (vector2 - vector).magnitude;
		return this.minValue + (this.maxValue - this.minValue) * num2;
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x00064BCC File Offset: 0x00062DCC
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

	// Token: 0x04001235 RID: 4661
	[SerializeField]
	public bool ControlledByRightStick;

	// Token: 0x04001236 RID: 4662
	[NonSerialized]
	public bool IsBeingMovedByRightStick;

	// Token: 0x04001237 RID: 4663
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001238 RID: 4664
	[SerializeField]
	protected dfControlOrientation orientation;

	// Token: 0x04001239 RID: 4665
	[SerializeField]
	protected float rawValue = 1f;

	// Token: 0x0400123A RID: 4666
	[SerializeField]
	protected float minValue;

	// Token: 0x0400123B RID: 4667
	[SerializeField]
	protected float maxValue = 100f;

	// Token: 0x0400123C RID: 4668
	[SerializeField]
	protected float stepSize = 1f;

	// Token: 0x0400123D RID: 4669
	[SerializeField]
	protected float scrollSize = 1f;

	// Token: 0x0400123E RID: 4670
	[SerializeField]
	protected float increment = 1f;

	// Token: 0x0400123F RID: 4671
	[SerializeField]
	protected dfControl thumb;

	// Token: 0x04001240 RID: 4672
	[SerializeField]
	protected dfControl track;

	// Token: 0x04001241 RID: 4673
	[SerializeField]
	protected dfControl incButton;

	// Token: 0x04001242 RID: 4674
	[SerializeField]
	protected dfControl decButton;

	// Token: 0x04001243 RID: 4675
	[SerializeField]
	protected RectOffset thumbPadding = new RectOffset();

	// Token: 0x04001244 RID: 4676
	[SerializeField]
	protected bool autoHide;

	// Token: 0x04001245 RID: 4677
	private Vector3 thumbMouseOffset = Vector3.zero;
}
