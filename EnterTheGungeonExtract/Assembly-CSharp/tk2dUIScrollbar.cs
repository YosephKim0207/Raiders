using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C0C RID: 3084
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollbar")]
public class tk2dUIScrollbar : MonoBehaviour
{
	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x060041D9 RID: 16857 RVA: 0x00155028 File Offset: 0x00153228
	// (set) Token: 0x060041DA RID: 16858 RVA: 0x00155030 File Offset: 0x00153230
	public tk2dUILayout BarLayoutItem
	{
		get
		{
			return this.barLayoutItem;
		}
		set
		{
			if (this.barLayoutItem != value)
			{
				if (this.barLayoutItem != null)
				{
					this.barLayoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.barLayoutItem = value;
				if (this.barLayoutItem != null)
				{
					this.barLayoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x1400008D RID: 141
	// (add) Token: 0x060041DB RID: 16859 RVA: 0x001550A8 File Offset: 0x001532A8
	// (remove) Token: 0x060041DC RID: 16860 RVA: 0x001550E0 File Offset: 0x001532E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIScrollbar> OnScroll;

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x060041DD RID: 16861 RVA: 0x00155118 File Offset: 0x00153318
	// (set) Token: 0x060041DE RID: 16862 RVA: 0x00155138 File Offset: 0x00153338
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.barUIItem != null)
			{
				return this.barUIItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.barUIItem != null && this.barUIItem.sendMessageTarget != value)
			{
				this.barUIItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x060041DF RID: 16863 RVA: 0x00155170 File Offset: 0x00153370
	// (set) Token: 0x060041E0 RID: 16864 RVA: 0x00155178 File Offset: 0x00153378
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (this.OnScroll != null)
			{
				this.OnScroll(this);
			}
			this.SetScrollThumbPosition();
			if (this.SendMessageTarget != null && this.SendMessageOnScrollMethodName.Length > 0)
			{
				this.SendMessageTarget.SendMessage(this.SendMessageOnScrollMethodName, this, SendMessageOptions.RequireReceiver);
			}
		}
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x001551F0 File Offset: 0x001533F0
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.SetScrollThumbPosition();
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x00155210 File Offset: 0x00153410
	private void OnEnable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown += this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver += this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut += this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown += this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease += this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown += this.ScrollUpButtonDown;
			this.upButton.OnUp += this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown += this.ScrollDownButtonDown;
			this.downButton.OnUp += this.ScrollDownButtonUp;
		}
		if (this.barLayoutItem != null)
		{
			this.barLayoutItem.OnReshape += this.LayoutReshaped;
		}
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x00155358 File Offset: 0x00153558
	private void OnDisable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown -= this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver -= this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut -= this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown -= this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease -= this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown -= this.ScrollUpButtonDown;
			this.upButton.OnUp -= this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown -= this.ScrollDownButtonDown;
			this.downButton.OnUp -= this.ScrollDownButtonUp;
		}
		if (this.isScrollThumbButtonDown)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
			}
			this.isScrollThumbButtonDown = false;
		}
		if (this.isTrackHoverOver)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = false;
		}
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
			this.scrollUpDownButtonState = 0;
		}
		if (this.barLayoutItem != null)
		{
			this.barLayoutItem.OnReshape -= this.LayoutReshaped;
		}
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x00155538 File Offset: 0x00153738
	private void Awake()
	{
		if (this.upButton != null)
		{
			this.hoverUpButton = this.upButton.GetComponent<tk2dUIHoverItem>();
		}
		if (this.downButton != null)
		{
			this.hoverDownButton = this.downButton.GetComponent<tk2dUIHoverItem>();
		}
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0015558C File Offset: 0x0015378C
	private void Start()
	{
		this.SetScrollThumbPosition();
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x00155594 File Offset: 0x00153794
	private void TrackHoverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			this.ScrollUpFixed();
		}
		else if (mouseWheelChange < 0f)
		{
			this.ScrollDownFixed();
		}
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x001555C0 File Offset: 0x001537C0
	private void SetScrollThumbPosition()
	{
		if (this.thumbTransform != null)
		{
			float num = -((this.scrollBarLength - this.thumbLength) * this.Value);
			Vector3 localPosition = this.thumbTransform.localPosition;
			if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
			{
				localPosition.x = -num;
			}
			else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
			{
				localPosition.y = num;
			}
			this.thumbTransform.localPosition = localPosition;
		}
		if (this.highlightProgressBar != null)
		{
			this.highlightProgressBar.Value = this.Value;
		}
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x0015565C File Offset: 0x0015385C
	private void MoveScrollThumbButton()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.thumbBtn) + this.moveThumbBtnOffset);
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x0015567C File Offset: 0x0015387C
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector2 position = btn.Touch.position;
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = btn.transform.position.z;
		return vector;
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x00155708 File Offset: 0x00153908
	private void ScrollToPosition(Vector3 worldPos)
	{
		Vector3 vector = this.thumbTransform.parent.InverseTransformPoint(worldPos);
		float num = 0f;
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			num = vector.x;
		}
		else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
		{
			num = -vector.y;
		}
		this.Value = num / (this.scrollBarLength - this.thumbLength);
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x00155770 File Offset: 0x00153970
	private void ScrollTrackButtonDown()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.barUIItem));
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x00155784 File Offset: 0x00153984
	private void ScrollTrackButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isTrackHoverOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = true;
		}
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x001557BC File Offset: 0x001539BC
	private void ScrollTrackButtonHoverOut()
	{
		if (this.isTrackHoverOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
		}
		this.isTrackHoverOver = false;
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x001557E8 File Offset: 0x001539E8
	private void ScrollThumbButtonDown()
	{
		if (!this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = true;
		Vector3 vector = this.CalculateClickWorldPos(this.thumbBtn);
		this.moveThumbBtnOffset = this.thumbBtn.transform.position - vector;
		this.moveThumbBtnOffset.z = 0f;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = true;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = true;
		}
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x00155890 File Offset: 0x00153A90
	private void ScrollThumbButtonRelease()
	{
		if (this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = false;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = false;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = false;
		}
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x00155900 File Offset: 0x00153B00
	private void ScrollUpButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = -1;
		this.ScrollUpFixed();
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x00155954 File Offset: 0x00153B54
	private void ScrollUpButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x00155980 File Offset: 0x00153B80
	private void ScrollDownButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 1;
		this.ScrollDownFixed();
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x001559D4 File Offset: 0x00153BD4
	private void ScrollDownButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x00155A00 File Offset: 0x00153C00
	public void ScrollUpFixed()
	{
		this.ScrollDirection(-1);
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x00155A0C File Offset: 0x00153C0C
	public void ScrollDownFixed()
	{
		this.ScrollDirection(1);
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x00155A18 File Offset: 0x00153C18
	private void CheckRepeatScrollUpDownButton()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			float num = Time.realtimeSinceStartup - this.timeOfUpDownButtonPressStart;
			if (this.repeatUpDownButtonHoldCounter == 0f)
			{
				if (num > 0.55f)
				{
					this.repeatUpDownButtonHoldCounter += 1f;
					num -= 0.55f;
					this.ScrollDirection(this.scrollUpDownButtonState);
				}
			}
			else if (num > 0.45f)
			{
				this.repeatUpDownButtonHoldCounter += 1f;
				num -= 0.45f;
				this.ScrollDirection(this.scrollUpDownButtonState);
			}
		}
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x00155AB4 File Offset: 0x00153CB4
	public void ScrollDirection(int dir)
	{
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			this.Value -= this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir * this.buttonUpDownScrollDistance;
		}
		else
		{
			this.Value += this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir * this.buttonUpDownScrollDistance;
		}
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x00155B0C File Offset: 0x00153D0C
	private float CalcScrollPercentOffsetButtonScrollDistance()
	{
		return 0.1f;
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x00155B14 File Offset: 0x00153D14
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.scrollBarLength += ((this.scrollAxes != tk2dUIScrollbar.Axes.XAxis) ? (dMax.y - dMin.y) : (dMax.x - dMin.x));
	}

	// Token: 0x04003463 RID: 13411
	public tk2dUIItem barUIItem;

	// Token: 0x04003464 RID: 13412
	public float scrollBarLength;

	// Token: 0x04003465 RID: 13413
	public tk2dUIItem thumbBtn;

	// Token: 0x04003466 RID: 13414
	public Transform thumbTransform;

	// Token: 0x04003467 RID: 13415
	public float thumbLength;

	// Token: 0x04003468 RID: 13416
	public tk2dUIItem upButton;

	// Token: 0x04003469 RID: 13417
	private tk2dUIHoverItem hoverUpButton;

	// Token: 0x0400346A RID: 13418
	public tk2dUIItem downButton;

	// Token: 0x0400346B RID: 13419
	private tk2dUIHoverItem hoverDownButton;

	// Token: 0x0400346C RID: 13420
	public float buttonUpDownScrollDistance = 1f;

	// Token: 0x0400346D RID: 13421
	public bool allowScrollWheel = true;

	// Token: 0x0400346E RID: 13422
	public tk2dUIScrollbar.Axes scrollAxes = tk2dUIScrollbar.Axes.YAxis;

	// Token: 0x0400346F RID: 13423
	public tk2dUIProgressBar highlightProgressBar;

	// Token: 0x04003470 RID: 13424
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout barLayoutItem;

	// Token: 0x04003471 RID: 13425
	private bool isScrollThumbButtonDown;

	// Token: 0x04003472 RID: 13426
	private bool isTrackHoverOver;

	// Token: 0x04003473 RID: 13427
	private float percent;

	// Token: 0x04003474 RID: 13428
	private Vector3 moveThumbBtnOffset = Vector3.zero;

	// Token: 0x04003475 RID: 13429
	private int scrollUpDownButtonState;

	// Token: 0x04003476 RID: 13430
	private float timeOfUpDownButtonPressStart;

	// Token: 0x04003477 RID: 13431
	private float repeatUpDownButtonHoldCounter;

	// Token: 0x04003478 RID: 13432
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04003479 RID: 13433
	private const float INITIAL_TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.55f;

	// Token: 0x0400347A RID: 13434
	private const float TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.45f;

	// Token: 0x0400347C RID: 13436
	public string SendMessageOnScrollMethodName = string.Empty;

	// Token: 0x02000C0D RID: 3085
	public enum Axes
	{
		// Token: 0x0400347E RID: 13438
		XAxis,
		// Token: 0x0400347F RID: 13439
		YAxis
	}
}
