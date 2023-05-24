using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000458 RID: 1112
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/Examples/Menus/Radial Menu")]
[Serializable]
public class dfRadialMenu : MonoBehaviour
{
	// Token: 0x1400005C RID: 92
	// (add) Token: 0x060019B0 RID: 6576 RVA: 0x00077F28 File Offset: 0x00076128
	// (remove) Token: 0x060019B1 RID: 6577 RVA: 0x00077F60 File Offset: 0x00076160
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfRadialMenu.CircularMenuEventHandler BeforeMenuOpened;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x060019B2 RID: 6578 RVA: 0x00077F98 File Offset: 0x00076198
	// (remove) Token: 0x060019B3 RID: 6579 RVA: 0x00077FD0 File Offset: 0x000761D0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfRadialMenu.CircularMenuEventHandler MenuOpened;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x060019B4 RID: 6580 RVA: 0x00078008 File Offset: 0x00076208
	// (remove) Token: 0x060019B5 RID: 6581 RVA: 0x00078040 File Offset: 0x00076240
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfRadialMenu.CircularMenuEventHandler MenuClosed;

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x060019B6 RID: 6582 RVA: 0x00078078 File Offset: 0x00076278
	// (set) Token: 0x060019B7 RID: 6583 RVA: 0x00078080 File Offset: 0x00076280
	public bool IsOpen
	{
		get
		{
			return this.isOpen;
		}
		set
		{
			if (this.isOpen != value)
			{
				if (value)
				{
					this.Open();
				}
				else
				{
					this.Close();
				}
			}
		}
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000780A8 File Offset: 0x000762A8
	public void Open()
	{
		if (!this.isOpen && !this.isAnimating && base.enabled && base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.openMenu());
		}
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000780E8 File Offset: 0x000762E8
	public void Close()
	{
		if (this.isOpen && !this.isAnimating && base.enabled && base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.closeMenu());
			if (this.host.ContainsFocus)
			{
				dfGUIManager.SetFocus(null, true);
			}
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x0007814C File Offset: 0x0007634C
	public void Toggle()
	{
		if (this.isAnimating)
		{
			return;
		}
		if (this.isOpen)
		{
			this.Close();
		}
		else
		{
			this.Open();
		}
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x00078178 File Offset: 0x00076378
	public void OnEnable()
	{
		if (this.host == null)
		{
			this.host = base.GetComponent<dfControl>();
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x00078198 File Offset: 0x00076398
	public void Start()
	{
		if (Application.isPlaying)
		{
			using (dfList<dfControl> buttons = this.getButtons())
			{
				for (int i = 0; i < buttons.Count; i++)
				{
					buttons[i].Hide();
				}
			}
		}
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000781FC File Offset: 0x000763FC
	public void Update()
	{
		if (!Application.isPlaying)
		{
			this.arrangeButtons();
		}
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x00078210 File Offset: 0x00076410
	public void OnLeaveFocus(dfControl sender, dfFocusEventArgs args)
	{
		if (this.closeOnLostFocus && !this.host.ContainsFocus && Application.isPlaying)
		{
			this.Close();
		}
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x00078240 File Offset: 0x00076440
	public void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		if (this.autoToggle || args.Source == this.host)
		{
			this.Toggle();
		}
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x0007826C File Offset: 0x0007646C
	private dfList<dfControl> getButtons()
	{
		return this.host.Controls.Where((dfControl x) => x.enabled && !this.excludedControls.Contains(x));
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x0007828C File Offset: 0x0007648C
	private void arrangeButtons()
	{
		this.arrangeButtons(this.startAngle, this.radius, this.openAngle, 1f);
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000782AC File Offset: 0x000764AC
	private IEnumerator openMenu()
	{
		if (this.BeforeMenuOpened != null)
		{
			this.BeforeMenuOpened(this);
		}
		this.host.Signal("OnBeforeMenuOpened", this);
		this.isAnimating = true;
		bool animate = this.animateOpacity || this.animateOpenAngle || this.animateRadius;
		if (animate)
		{
			float time = Mathf.Max(0.1f, this.animationLength);
			dfAnimatedFloat animOpenAngle = new dfAnimatedFloat((!this.animateOpenAngle) ? this.openAngle : 0f, this.openAngle, time);
			dfAnimatedFloat animRadius = new dfAnimatedFloat((!this.animateRadius) ? this.radius : 0f, this.radius, time);
			dfAnimatedFloat animOpacity = new dfAnimatedFloat((float)((!this.animateOpacity) ? 1 : 0), 1f, time);
			float endTime = Time.realtimeSinceStartup + time;
			while (Time.realtimeSinceStartup < endTime)
			{
				this.arrangeButtons(this.startAngle, animRadius, animOpenAngle, animOpacity);
				yield return null;
			}
		}
		this.arrangeButtons();
		this.isOpen = true;
		this.isAnimating = false;
		if (this.MenuOpened != null)
		{
			this.MenuOpened(this);
		}
		this.host.Signal("OnMenuOpened", this);
		yield break;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000782C8 File Offset: 0x000764C8
	private IEnumerator closeMenu()
	{
		this.isAnimating = true;
		bool animate = this.animateOpacity || this.animateOpenAngle || this.animateRadius;
		if (animate)
		{
			float time = Mathf.Max(0.1f, this.animationLength);
			dfAnimatedFloat animOpenAngle = new dfAnimatedFloat(this.openAngle, (!this.animateOpenAngle) ? this.openAngle : 0f, time);
			dfAnimatedFloat animRadius = new dfAnimatedFloat(this.radius, (!this.animateRadius) ? this.radius : 0f, time);
			dfAnimatedFloat animOpacity = new dfAnimatedFloat(1f, (float)((!this.animateOpacity) ? 1 : 0), time);
			float endTime = Time.realtimeSinceStartup + time;
			while (Time.realtimeSinceStartup < endTime)
			{
				this.arrangeButtons(this.startAngle, animRadius, animOpenAngle, animOpacity);
				yield return null;
			}
		}
		using (dfList<dfControl> buttons = this.getButtons())
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].IsVisible = false;
			}
		}
		this.isOpen = false;
		this.isAnimating = false;
		if (this.MenuClosed != null)
		{
			this.MenuClosed(this);
		}
		this.host.Signal("OnMenuOpened", this);
		yield break;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000782E4 File Offset: 0x000764E4
	private void arrangeButtons(float startAngle, float radius, float openAngle, float opacity)
	{
		float num = this.clampRotation(startAngle);
		Vector3 vector = this.host.Size * 0.5f;
		using (dfList<dfControl> buttons = this.getButtons())
		{
			if (buttons.Count != 0)
			{
				float num2 = Mathf.Sign(openAngle);
				float num3 = num2 * Mathf.Min(Mathf.Abs(this.clampRotation(openAngle)) / (float)(buttons.Count - 1), 360f / (float)buttons.Count);
				for (int i = 0; i < buttons.Count; i++)
				{
					dfControl dfControl = buttons[i];
					Quaternion quaternion = Quaternion.Euler(0f, 0f, num);
					Vector3 vector2 = vector + quaternion * Vector3.down * radius;
					dfControl.RelativePosition = vector2 - dfControl.Size * 0.5f;
					if (this.rotateButtons)
					{
						dfControl.Pivot = dfPivotPoint.MiddleCenter;
						dfControl.transform.localRotation = Quaternion.Euler(0f, 0f, -num);
					}
					else
					{
						dfControl.transform.localRotation = Quaternion.identity;
					}
					dfControl.IsVisible = true;
					dfControl.Opacity = opacity;
					num += num3;
				}
			}
		}
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x00078464 File Offset: 0x00076664
	private float clampRotation(float rotation)
	{
		return Mathf.Sign(rotation) * Mathf.Max(0.1f, Mathf.Min(360f, Mathf.Abs(rotation)));
	}

	// Token: 0x04001411 RID: 5137
	public float radius = 200f;

	// Token: 0x04001412 RID: 5138
	public float startAngle;

	// Token: 0x04001413 RID: 5139
	public float openAngle = 360f;

	// Token: 0x04001414 RID: 5140
	public bool rotateButtons;

	// Token: 0x04001415 RID: 5141
	public bool animateOpacity;

	// Token: 0x04001416 RID: 5142
	public bool animateOpenAngle;

	// Token: 0x04001417 RID: 5143
	public bool animateRadius;

	// Token: 0x04001418 RID: 5144
	public bool autoToggle;

	// Token: 0x04001419 RID: 5145
	public bool closeOnLostFocus;

	// Token: 0x0400141A RID: 5146
	public float animationLength = 0.5f;

	// Token: 0x0400141B RID: 5147
	public List<dfControl> excludedControls = new List<dfControl>();

	// Token: 0x0400141C RID: 5148
	public dfControl host;

	// Token: 0x0400141D RID: 5149
	private bool isAnimating;

	// Token: 0x0400141E RID: 5150
	private bool isOpen;

	// Token: 0x02000459 RID: 1113
	// (Invoke) Token: 0x060019C8 RID: 6600
	public delegate void CircularMenuEventHandler(dfRadialMenu sender);
}
