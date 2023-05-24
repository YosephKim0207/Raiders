using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017BD RID: 6077
public class MenuCrosshairSelectionDoer : TimeInvariantMonoBehaviour
{
	// Token: 0x1700154D RID: 5453
	// (get) Token: 0x06008E4B RID: 36427 RVA: 0x003BD90C File Offset: 0x003BBB0C
	public dfControl CrosshairControl
	{
		get
		{
			return (!(this.targetControlToEncrosshair == null)) ? this.targetControlToEncrosshair : this.m_control;
		}
	}

	// Token: 0x06008E4C RID: 36428 RVA: 0x003BD930 File Offset: 0x003BBB30
	private void Start()
	{
		this.m_control = base.GetComponent<dfControl>();
		this.m_extantControls = new List<dfControl>();
		if (this.mouseFocusable || true)
		{
			this.m_control.MouseEnter += delegate(dfControl control, dfMouseEventArgs mouseArgs)
			{
				if (!InControlInputAdapter.SkipInputForRestOfFrame)
				{
					this.m_control.Focus(false);
				}
			};
		}
		this.m_control.GotFocus += this.GotFocus;
		this.m_control.LostFocus += this.LostFocus;
		UIKeyControls component = base.GetComponent<UIKeyControls>();
		if (component != null)
		{
			UIKeyControls uikeyControls = component;
			uikeyControls.OnNewControlSelected = (Action<dfControl>)Delegate.Combine(uikeyControls.OnNewControlSelected, new Action<dfControl>(this.DifferentControlSelected));
		}
		BraveOptionsMenuItem component2 = base.GetComponent<BraveOptionsMenuItem>();
		if (component2 != null)
		{
			BraveOptionsMenuItem braveOptionsMenuItem = component2;
			braveOptionsMenuItem.OnNewControlSelected = (Action<dfControl>)Delegate.Combine(braveOptionsMenuItem.OnNewControlSelected, new Action<dfControl>(this.DifferentControlSelected));
		}
	}

	// Token: 0x06008E4D RID: 36429 RVA: 0x003BDA14 File Offset: 0x003BBC14
	private void LateUpdate()
	{
		if (!this.m_suppressed && this.m_extantControls != null && this.m_extantControls.Count > 0)
		{
			this.UpdatedOwnedControls();
		}
	}

	// Token: 0x06008E4E RID: 36430 RVA: 0x003BDA44 File Offset: 0x003BBC44
	private void DifferentControlSelected(dfControl newControl)
	{
		MenuCrosshairSelectionDoer component = newControl.GetComponent<MenuCrosshairSelectionDoer>();
		if (component != null && this.m_extantControls.Count == 2)
		{
			this.m_suppressed = true;
			component.m_suppressed = true;
			base.StartCoroutine(this.HandleLerpyDerpy(component));
		}
	}

	// Token: 0x06008E4F RID: 36431 RVA: 0x003BDA94 File Offset: 0x003BBC94
	private IEnumerator HandleLerpyDerpy(MenuCrosshairSelectionDoer targetCrosshairDoer)
	{
		yield return null;
		float elapsed = 0f;
		float duration = 0.1f;
		dfControl leftControl = this.m_extantControls[0];
		dfControl rightControl = this.m_extantControls[1];
		Vector3 startPosL = leftControl.transform.position;
		Vector3 startPosR = rightControl.transform.position;
		float offsetWidth = targetCrosshairDoer.CrosshairControl.Size.x / 2f + leftControl.Size.x / 2f + 6f;
		offsetWidth *= leftControl.PixelsToUnits();
		float rightOffset = -3f * this.CrosshairControl.PixelsToUnits();
		Vector3 endPosL = targetCrosshairDoer.CrosshairControl.GetCenter() + new Vector3(-offsetWidth, 0f, 0f) + targetCrosshairDoer.leftCrosshairPixelOffset.ToVector3(0f) * targetCrosshairDoer.CrosshairControl.PixelsToUnits();
		Vector3 endPosR = targetCrosshairDoer.CrosshairControl.GetCenter() + new Vector3(offsetWidth + rightOffset, 0f, 0f) + targetCrosshairDoer.rightCrosshairPixelOffset.ToVector3(0f) * targetCrosshairDoer.CrosshairControl.PixelsToUnits();
		leftControl.GUIManager.AddControl(leftControl);
		rightControl.GUIManager.AddControl(rightControl);
		leftControl.BringToFront();
		rightControl.BringToFront();
		while (elapsed < duration)
		{
			elapsed += this.m_deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			leftControl.transform.position = Vector3.Lerp(startPosL, endPosL, t);
			rightControl.transform.position = Vector3.Lerp(startPosR, endPosR, t);
			leftControl.BringToFront();
			rightControl.BringToFront();
			yield return null;
			if (!targetCrosshairDoer.m_control.HasFocus)
			{
				this.ClearExtantControls();
				this.m_suppressed = false;
				targetCrosshairDoer.m_suppressed = false;
				yield break;
			}
		}
		this.ClearExtantControls();
		targetCrosshairDoer.ClearExtantControls();
		targetCrosshairDoer.CreateOwnedControls();
		this.m_suppressed = false;
		targetCrosshairDoer.m_suppressed = false;
		yield break;
	}

	// Token: 0x06008E50 RID: 36432 RVA: 0x003BDAB8 File Offset: 0x003BBCB8
	private void GotFocus(dfControl control, dfFocusEventArgs args)
	{
		AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		if (this.m_suppressed)
		{
			return;
		}
	}

	// Token: 0x06008E51 RID: 36433 RVA: 0x003BDAD8 File Offset: 0x003BBCD8
	private void LostFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this.m_suppressed)
		{
			return;
		}
	}

	// Token: 0x06008E52 RID: 36434 RVA: 0x003BDAE8 File Offset: 0x003BBCE8
	private void UpdatedOwnedControls()
	{
		dfControl dfControl = this.m_extantControls[0];
		dfControl dfControl2 = this.m_extantControls[1];
		float num = this.CrosshairControl.Size.x / 2f + dfControl.Size.x / 2f + 6f;
		num *= this.CrosshairControl.transform.lossyScale.x;
		num *= this.CrosshairControl.PixelsToUnits();
		float num2 = -3f * this.CrosshairControl.PixelsToUnits();
		dfControl.transform.position = this.CrosshairControl.GetCenter() + new Vector3(num * -1f, 0f, 0f) + this.leftCrosshairPixelOffset.ToVector3(0f) * this.CrosshairControl.PixelsToUnits();
		dfControl2.transform.position = this.CrosshairControl.GetCenter() + new Vector3(num + num2, 0f, 0f) + this.rightCrosshairPixelOffset.ToVector3(0f) * this.CrosshairControl.PixelsToUnits();
	}

	// Token: 0x06008E53 RID: 36435 RVA: 0x003BDC2C File Offset: 0x003BBE2C
	private void CreateOwnedControls()
	{
		dfControl dfControl = this.CrosshairControl.AddPrefab(this.controlToPlace.gameObject);
		dfControl dfControl2 = this.CrosshairControl.AddPrefab(this.controlToPlace.gameObject);
		dfControl.IsVisible = false;
		dfControl2.IsVisible = false;
		dfControl.transform.localScale = Vector3.one;
		dfControl2.transform.localScale = Vector3.one;
		dfControl2.GetComponent<dfSpriteAnimation>().Direction = dfPlayDirection.Reverse;
		float num = this.CrosshairControl.Size.x / 2f + dfControl.Size.x / 2f + 6f;
		num *= this.CrosshairControl.transform.lossyScale.x;
		num *= this.CrosshairControl.PixelsToUnits();
		float num2 = -3f * this.CrosshairControl.PixelsToUnits();
		dfControl.transform.position = this.CrosshairControl.GetCenter() + new Vector3(num * -1f, 0f, 0f) + this.leftCrosshairPixelOffset.ToVector3(0f) * this.CrosshairControl.PixelsToUnits();
		dfControl2.transform.position = this.CrosshairControl.GetCenter() + new Vector3(num + num2, 0f, 0f) + this.rightCrosshairPixelOffset.ToVector3(0f) * this.CrosshairControl.PixelsToUnits();
		this.m_extantControls.Add(dfControl);
		this.m_extantControls.Add(dfControl2);
	}

	// Token: 0x06008E54 RID: 36436 RVA: 0x003BDDD8 File Offset: 0x003BBFD8
	private void ClearExtantControls()
	{
		if (this.m_extantControls.Count > 0)
		{
			for (int i = 0; i < this.m_extantControls.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_extantControls[i].gameObject);
			}
			this.m_extantControls.Clear();
		}
	}

	// Token: 0x06008E55 RID: 36437 RVA: 0x003BDE34 File Offset: 0x003BC034
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400963A RID: 38458
	public dfControl controlToPlace;

	// Token: 0x0400963B RID: 38459
	public dfControl targetControlToEncrosshair;

	// Token: 0x0400963C RID: 38460
	public IntVector2 leftCrosshairPixelOffset;

	// Token: 0x0400963D RID: 38461
	public IntVector2 rightCrosshairPixelOffset;

	// Token: 0x0400963E RID: 38462
	public bool mouseFocusable = true;

	// Token: 0x0400963F RID: 38463
	private List<dfControl> m_extantControls;

	// Token: 0x04009640 RID: 38464
	private dfControl m_control;

	// Token: 0x04009641 RID: 38465
	private bool m_suppressed;
}
