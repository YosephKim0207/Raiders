using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C0A RID: 3082
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollableArea")]
public class tk2dUIScrollableArea : MonoBehaviour
{
	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00153D5C File Offset: 0x00151F5C
	// (set) Token: 0x060041B2 RID: 16818 RVA: 0x00153D64 File Offset: 0x00151F64
	public float ContentLength
	{
		get
		{
			return this.contentLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, value, this.visibleAreaLength, this.visibleAreaLength);
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x060041B3 RID: 16819 RVA: 0x00153D80 File Offset: 0x00151F80
	// (set) Token: 0x060041B4 RID: 16820 RVA: 0x00153D88 File Offset: 0x00151F88
	public float VisibleAreaLength
	{
		get
		{
			return this.visibleAreaLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, this.contentLength, this.visibleAreaLength, value);
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00153DA4 File Offset: 0x00151FA4
	// (set) Token: 0x060041B6 RID: 16822 RVA: 0x00153DAC File Offset: 0x00151FAC
	public tk2dUILayout BackgroundLayoutItem
	{
		get
		{
			return this.backgroundLayoutItem;
		}
		set
		{
			if (this.backgroundLayoutItem != value)
			{
				if (this.backgroundLayoutItem != null)
				{
					this.backgroundLayoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.backgroundLayoutItem = value;
				if (this.backgroundLayoutItem != null)
				{
					this.backgroundLayoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00153E24 File Offset: 0x00152024
	// (set) Token: 0x060041B8 RID: 16824 RVA: 0x00153E2C File Offset: 0x0015202C
	public tk2dUILayoutContainer ContentLayoutContainer
	{
		get
		{
			return this.contentLayoutContainer;
		}
		set
		{
			if (this.contentLayoutContainer != value)
			{
				if (this.contentLayoutContainer != null)
				{
					this.contentLayoutContainer.OnChangeContent -= this.ContentLayoutChangeCallback;
				}
				this.contentLayoutContainer = value;
				if (this.contentLayoutContainer != null)
				{
					this.contentLayoutContainer.OnChangeContent += this.ContentLayoutChangeCallback;
				}
			}
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x060041B9 RID: 16825 RVA: 0x00153EA4 File Offset: 0x001520A4
	// (set) Token: 0x060041BA RID: 16826 RVA: 0x00153EC4 File Offset: 0x001520C4
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.backgroundUIItem != null)
			{
				return this.backgroundUIItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.backgroundUIItem != null && this.backgroundUIItem.sendMessageTarget != value)
			{
				this.backgroundUIItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x1400008C RID: 140
	// (add) Token: 0x060041BB RID: 16827 RVA: 0x00153EFC File Offset: 0x001520FC
	// (remove) Token: 0x060041BC RID: 16828 RVA: 0x00153F34 File Offset: 0x00152134
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIScrollableArea> OnScroll;

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x060041BD RID: 16829 RVA: 0x00153F6C File Offset: 0x0015216C
	// (set) Token: 0x060041BE RID: 16830 RVA: 0x00153F7C File Offset: 0x0015217C
	public float Value
	{
		get
		{
			return Mathf.Clamp01(this.percent);
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			if (value != this.percent)
			{
				this.UnpressAllUIItemChildren();
				this.percent = value;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
				if (this.isBackgroundButtonDown || this.isSwipeScrollingInProgress)
				{
					if (tk2dUIManager.Instance__NoCreate != null)
					{
						tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
					}
					this.isBackgroundButtonDown = false;
					this.isSwipeScrollingInProgress = false;
				}
				this.TargetOnScrollCallback();
			}
			if (this.scrollBar != null)
			{
				this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
			}
			this.SetContentPosition();
		}
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x00154044 File Offset: 0x00152244
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.UnpressAllUIItemChildren();
		if (this.scrollBar != null)
		{
			this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
		}
		this.SetContentPosition();
	}

	// Token: 0x060041C0 RID: 16832 RVA: 0x00154098 File Offset: 0x00152298
	public float MeasureContentLength()
	{
		Vector3 vector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Vector3 vector2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3[] array = new Vector3[] { vector2, vector };
		Transform transform = this.contentContainer.transform;
		tk2dUIScrollableArea.GetRendererBoundsInChildren(transform.worldToLocalMatrix, array, transform);
		if (array[0] != vector2 && array[1] != vector)
		{
			array[0] = Vector3.Min(array[0], Vector3.zero);
			array[1] = Vector3.Max(array[1], Vector3.zero);
			return (this.scrollAxes != tk2dUIScrollableArea.Axes.YAxis) ? (array[1].x - array[0].x) : (array[1].y - array[0].y);
		}
		UnityEngine.Debug.LogError("Unable to measure content length");
		return this.VisibleAreaLength * 0.9f;
	}

	// Token: 0x060041C1 RID: 16833 RVA: 0x001541D8 File Offset: 0x001523D8
	private void OnEnable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll += this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown += this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease += this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver += this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut += this.BackgroundButtonHoverOut;
		}
		if (this.backgroundLayoutItem != null)
		{
			this.backgroundLayoutItem.OnReshape += this.LayoutReshaped;
		}
		if (this.contentLayoutContainer != null)
		{
			this.contentLayoutContainer.OnChangeContent += this.ContentLayoutChangeCallback;
		}
	}

	// Token: 0x060041C2 RID: 16834 RVA: 0x001542CC File Offset: 0x001524CC
	private void OnDisable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll -= this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown -= this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease -= this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver -= this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut -= this.BackgroundButtonHoverOut;
		}
		if (this.isBackgroundButtonOver)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = false;
		}
		if (this.isBackgroundButtonDown || this.isSwipeScrollingInProgress)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
			this.isSwipeScrollingInProgress = false;
		}
		if (this.backgroundLayoutItem != null)
		{
			this.backgroundLayoutItem.OnReshape -= this.LayoutReshaped;
		}
		if (this.contentLayoutContainer != null)
		{
			this.contentLayoutContainer.OnChangeContent -= this.ContentLayoutChangeCallback;
		}
		this.swipeCurrVelocity = 0f;
	}

	// Token: 0x060041C3 RID: 16835 RVA: 0x0015444C File Offset: 0x0015264C
	private void Start()
	{
		this.UpdateScrollbarActiveState();
	}

	// Token: 0x060041C4 RID: 16836 RVA: 0x00154454 File Offset: 0x00152654
	private void BackgroundHoverOverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollUpFixed();
			}
			else
			{
				this.Value -= 0.1f;
			}
		}
		else if (mouseWheelChange < 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollDownFixed();
			}
			else
			{
				this.Value += 0.1f;
			}
		}
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x001544E0 File Offset: 0x001526E0
	private void ScrollBarMove(tk2dUIScrollbar scrollBar)
	{
		this.Value = scrollBar.Value;
		this.isSwipeScrollingInProgress = false;
		if (this.isBackgroundButtonDown)
		{
			this.BackgroundButtonRelease();
		}
	}

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x060041C6 RID: 16838 RVA: 0x00154508 File Offset: 0x00152708
	// (set) Token: 0x060041C7 RID: 16839 RVA: 0x00154534 File Offset: 0x00152734
	private Vector3 ContentContainerOffset
	{
		get
		{
			return Vector3.Scale(new Vector3(-1f, 1f, 1f), this.contentContainer.transform.localPosition);
		}
		set
		{
			this.contentContainer.transform.localPosition = Vector3.Scale(new Vector3(-1f, 1f, 1f), value);
		}
	}

	// Token: 0x060041C8 RID: 16840 RVA: 0x00154560 File Offset: 0x00152760
	private void SetContentPosition()
	{
		Vector3 contentContainerOffset = this.ContentContainerOffset;
		float num = (this.contentLength - this.visibleAreaLength) * this.Value;
		if (num < 0f)
		{
			num = 0f;
		}
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			contentContainerOffset.x = num;
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			contentContainerOffset.y = num;
		}
		this.ContentContainerOffset = contentContainerOffset;
	}

	// Token: 0x060041C9 RID: 16841 RVA: 0x001545D0 File Offset: 0x001527D0
	private void BackgroundButtonDown()
	{
		if (this.allowSwipeScrolling && this.contentLength > this.visibleAreaLength)
		{
			if (!this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate += this.BackgroundOverUpdate;
			}
			this.swipeScrollingPressDownStartLocalPos = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
			this.swipePrevScrollingContentPressLocalPos = this.swipeScrollingPressDownStartLocalPos;
			this.swipeScrollingContentStartLocalPos = this.ContentContainerOffset;
			this.swipeScrollingContentDestLocalPos = this.swipeScrollingContentStartLocalPos;
			this.isBackgroundButtonDown = true;
			this.swipeCurrVelocity = 0f;
		}
	}

	// Token: 0x060041CA RID: 16842 RVA: 0x00154678 File Offset: 0x00152878
	private void BackgroundOverUpdate()
	{
		if (this.isBackgroundButtonDown)
		{
			this.UpdateSwipeScrollDestintationPosition();
		}
		if (this.isSwipeScrollingInProgress)
		{
			float num = this.percent;
			float num2 = 0f;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.x;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.y;
			}
			float num3 = 0f;
			float num4 = this.contentLength - this.visibleAreaLength;
			if (this.isBackgroundButtonDown)
			{
				if (num2 < num3)
				{
					num2 += -num2 / this.visibleAreaLength / 2f;
					if (num2 > num3)
					{
						num2 = num3;
					}
				}
				else if (num2 > num4)
				{
					num2 -= (num2 - num4) / this.visibleAreaLength / 2f;
					if (num2 < num4)
					{
						num2 = num4;
					}
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			else
			{
				float num5 = this.visibleAreaLength * 0.001f;
				if (num2 < num3 || num2 > num4)
				{
					float num6 = ((num2 >= num3) ? num4 : num3);
					num2 = Mathf.SmoothDamp(num2, num6, ref this.snapBackVelocity, 0.05f, float.PositiveInfinity, tk2dUITime.deltaTime);
					if (Mathf.Abs(this.snapBackVelocity) < num5)
					{
						num2 = num6;
						this.snapBackVelocity = 0f;
					}
					this.swipeCurrVelocity = 0f;
				}
				else if (this.swipeCurrVelocity != 0f)
				{
					num2 += this.swipeCurrVelocity * tk2dUITime.deltaTime * 20f;
					if (this.swipeCurrVelocity > num5 || this.swipeCurrVelocity < -num5)
					{
						this.swipeCurrVelocity = Mathf.Lerp(this.swipeCurrVelocity, 0f, tk2dUITime.deltaTime * 2.5f);
					}
					else
					{
						this.swipeCurrVelocity = 0f;
					}
				}
				else
				{
					this.isSwipeScrollingInProgress = false;
					tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			if (num != this.percent)
			{
				this.percent = num;
				this.ContentContainerOffset = this.swipeScrollingContentDestLocalPos;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
				this.TargetOnScrollCallback();
			}
			if (this.scrollBar != null)
			{
				float num7 = this.percent;
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					num7 = this.ContentContainerOffset.x / (this.contentLength - this.visibleAreaLength);
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					num7 = this.ContentContainerOffset.y / (this.contentLength - this.visibleAreaLength);
				}
				this.scrollBar.SetScrollPercentWithoutEvent(num7);
			}
		}
	}

	// Token: 0x060041CB RID: 16843 RVA: 0x001549A4 File Offset: 0x00152BA4
	private void UpdateSwipeScrollDestintationPosition()
	{
		Vector3 vector = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
		Vector3 vector2 = vector - this.swipeScrollingPressDownStartLocalPos;
		vector2.x *= -1f;
		float num = 0f;
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			num = vector2.x;
			this.swipeCurrVelocity = -(vector.x - this.swipePrevScrollingContentPressLocalPos.x);
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			num = vector2.y;
			this.swipeCurrVelocity = vector.y - this.swipePrevScrollingContentPressLocalPos.y;
		}
		if (!this.isSwipeScrollingInProgress && Mathf.Abs(num) > 0.02f)
		{
			this.isSwipeScrollingInProgress = true;
			tk2dUIManager.Instance.OverrideClearAllChildrenPresses(this.backgroundUIItem);
		}
		if (this.isSwipeScrollingInProgress)
		{
			Vector3 vector3 = this.swipeScrollingContentStartLocalPos + vector2;
			vector3.z = this.ContentContainerOffset.z;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				vector3.y = this.ContentContainerOffset.y;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				vector3.x = this.ContentContainerOffset.x;
			}
			vector3.z = this.ContentContainerOffset.z;
			this.swipeScrollingContentDestLocalPos = vector3;
			this.swipePrevScrollingContentPressLocalPos = vector;
		}
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x00154B1C File Offset: 0x00152D1C
	private void BackgroundButtonRelease()
	{
		if (this.allowSwipeScrolling)
		{
			if (this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
		}
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x00154B5C File Offset: 0x00152D5C
	private void BackgroundButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isBackgroundButtonOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = true;
		}
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x00154B94 File Offset: 0x00152D94
	private void BackgroundButtonHoverOut()
	{
		if (this.isBackgroundButtonOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
		}
		this.isBackgroundButtonOver = false;
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x00154BC0 File Offset: 0x00152DC0
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Vector2 position = btn.Touch.position;
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = btn.transform.position.z;
		return vector;
	}

	// Token: 0x060041D0 RID: 16848 RVA: 0x00154C4C File Offset: 0x00152E4C
	private void UpdateScrollbarActiveState()
	{
		bool flag = this.contentLength > this.visibleAreaLength;
		if (this.scrollBar != null && this.scrollBar.gameObject.activeSelf != flag)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.scrollBar.gameObject, flag);
		}
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x00154CA0 File Offset: 0x00152EA0
	private void ContentLengthVisibleAreaLengthChange(float prevContentLength, float newContentLength, float prevVisibleAreaLength, float newVisibleAreaLength)
	{
		float num;
		if (newContentLength - this.visibleAreaLength != 0f)
		{
			num = (prevContentLength - prevVisibleAreaLength) * this.Value / (newContentLength - newVisibleAreaLength);
		}
		else
		{
			num = 0f;
		}
		this.contentLength = newContentLength;
		this.visibleAreaLength = newVisibleAreaLength;
		this.UpdateScrollbarActiveState();
		this.Value = num;
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x00154CF8 File Offset: 0x00152EF8
	private void UnpressAllUIItemChildren()
	{
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x00154CFC File Offset: 0x00152EFC
	private void TargetOnScrollCallback()
	{
		if (this.SendMessageTarget != null && this.SendMessageOnScrollMethodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(this.SendMessageOnScrollMethodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x00154D34 File Offset: 0x00152F34
	private static void GetRendererBoundsInChildren(Matrix4x4 rootWorldToLocal, Vector3[] minMax, Transform t)
	{
		MeshFilter component = t.GetComponent<MeshFilter>();
		if (component != null && component.sharedMesh != null)
		{
			Bounds bounds = component.sharedMesh.bounds;
			Matrix4x4 matrix4x = rootWorldToLocal * t.localToWorldMatrix;
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector = bounds.center + Vector3.Scale(bounds.extents, tk2dUIScrollableArea.boxExtents[i]);
				Vector3 vector2 = matrix4x.MultiplyPoint(vector);
				minMax[0] = Vector3.Min(minMax[0], vector2);
				minMax[1] = Vector3.Max(minMax[1], vector2);
			}
		}
		int childCount = t.childCount;
		for (int j = 0; j < childCount; j++)
		{
			Transform child = t.GetChild(j);
			if (t.gameObject.activeSelf)
			{
				tk2dUIScrollableArea.GetRendererBoundsInChildren(rootWorldToLocal, minMax, child);
			}
		}
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x00154E48 File Offset: 0x00153048
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.VisibleAreaLength += ((this.scrollAxes != tk2dUIScrollableArea.Axes.XAxis) ? (dMax.y - dMin.y) : (dMax.x - dMin.x));
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x00154E88 File Offset: 0x00153088
	private void ContentLayoutChangeCallback()
	{
		if (this.contentLayoutContainer != null)
		{
			Vector2 innerSize = this.contentLayoutContainer.GetInnerSize();
			this.ContentLength = ((this.scrollAxes != tk2dUIScrollableArea.Axes.XAxis) ? innerSize.y : innerSize.x);
		}
	}

	// Token: 0x04003447 RID: 13383
	[SerializeField]
	private float contentLength = 1f;

	// Token: 0x04003448 RID: 13384
	[SerializeField]
	private float visibleAreaLength = 1f;

	// Token: 0x04003449 RID: 13385
	public GameObject contentContainer;

	// Token: 0x0400344A RID: 13386
	public tk2dUIScrollbar scrollBar;

	// Token: 0x0400344B RID: 13387
	public tk2dUIItem backgroundUIItem;

	// Token: 0x0400344C RID: 13388
	public tk2dUIScrollableArea.Axes scrollAxes = tk2dUIScrollableArea.Axes.YAxis;

	// Token: 0x0400344D RID: 13389
	public bool allowSwipeScrolling = true;

	// Token: 0x0400344E RID: 13390
	public bool allowScrollWheel = true;

	// Token: 0x0400344F RID: 13391
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout backgroundLayoutItem;

	// Token: 0x04003450 RID: 13392
	[HideInInspector]
	[SerializeField]
	private tk2dUILayoutContainer contentLayoutContainer;

	// Token: 0x04003451 RID: 13393
	private bool isBackgroundButtonDown;

	// Token: 0x04003452 RID: 13394
	private bool isBackgroundButtonOver;

	// Token: 0x04003453 RID: 13395
	private Vector3 swipeScrollingPressDownStartLocalPos = Vector3.zero;

	// Token: 0x04003454 RID: 13396
	private Vector3 swipeScrollingContentStartLocalPos = Vector3.zero;

	// Token: 0x04003455 RID: 13397
	private Vector3 swipeScrollingContentDestLocalPos = Vector3.zero;

	// Token: 0x04003456 RID: 13398
	private bool isSwipeScrollingInProgress;

	// Token: 0x04003457 RID: 13399
	private const float SWIPE_SCROLLING_FIRST_SCROLL_THRESHOLD = 0.02f;

	// Token: 0x04003458 RID: 13400
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04003459 RID: 13401
	private Vector3 swipePrevScrollingContentPressLocalPos = Vector3.zero;

	// Token: 0x0400345A RID: 13402
	private float swipeCurrVelocity;

	// Token: 0x0400345B RID: 13403
	private float snapBackVelocity;

	// Token: 0x0400345D RID: 13405
	public string SendMessageOnScrollMethodName = string.Empty;

	// Token: 0x0400345E RID: 13406
	private float percent;

	// Token: 0x0400345F RID: 13407
	private static readonly Vector3[] boxExtents = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x02000C0B RID: 3083
	public enum Axes
	{
		// Token: 0x04003461 RID: 13409
		XAxis,
		// Token: 0x04003462 RID: 13410
		YAxis
	}
}
