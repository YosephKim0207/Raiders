using System;
using InControl;
using UnityEngine;

// Token: 0x0200180D RID: 6157
public class UIKeyControls : MonoBehaviour
{
	// Token: 0x0600912A RID: 37162 RVA: 0x003D59BC File Offset: 0x003D3BBC
	public void Awake()
	{
		this.button = base.GetComponent<dfButton>();
		this.selfControl = base.GetComponent<dfControl>();
		if (this.clearRepeatingOnSelect)
		{
			this.button.GotFocus += this.GotFocus;
		}
	}

	// Token: 0x0600912B RID: 37163 RVA: 0x003D59F8 File Offset: 0x003D3BF8
	private static void CheckForControllerFails()
	{
		if (BraveInput.PrimaryPlayerInstance != null && BraveInput.PrimaryPlayerInstance.ActiveActions != null && BraveInput.PrimaryPlayerInstance.ActiveActions.LastInputType != BindingSourceType.MouseBindingSource)
		{
			if (UIKeyControls.m_lastFocusedUIKeyControl != null && dfGUIManager.ActiveControl != null && dfGUIManager.ActiveControl.GetComponent<UIKeyControls>() == null && dfGUIManager.ActiveControl.GetComponent<BraveOptionsMenuItem>() == null)
			{
				UIKeyControls.m_timer += GameManager.INVARIANT_DELTA_TIME;
				if (UIKeyControls.m_timer > 1.5f)
				{
					UIKeyControls.m_lastFocusedUIKeyControl.selfControl.Focus(true);
				}
			}
			else
			{
				UIKeyControls.m_timer = 0f;
			}
		}
		else
		{
			UIKeyControls.m_timer = 0f;
		}
		UIKeyControls.m_hasCheckedThisFrame = true;
	}

	// Token: 0x0600912C RID: 37164 RVA: 0x003D5AD8 File Offset: 0x003D3CD8
	public void Update()
	{
		if (this.selfControl.HasFocus)
		{
			UIKeyControls.m_lastFocusedUIKeyControl = this;
		}
		if (!UIKeyControls.m_hasCheckedThisFrame)
		{
			UIKeyControls.CheckForControllerFails();
		}
	}

	// Token: 0x0600912D RID: 37165 RVA: 0x003D5B00 File Offset: 0x003D3D00
	public void LateUpdate()
	{
		UIKeyControls.m_hasCheckedThisFrame = false;
	}

	// Token: 0x0600912E RID: 37166 RVA: 0x003D5B08 File Offset: 0x003D3D08
	public void OnKeyDown(dfControl sender, dfKeyEventArgs args)
	{
		if (!args.Used)
		{
			if (args.KeyCode == KeyCode.UpArrow)
			{
				if (this.up)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.up);
					}
					this.up.Focus(true);
				}
				if (this.OnUpDown != null)
				{
					this.OnUpDown();
				}
			}
			else if (args.KeyCode == KeyCode.DownArrow)
			{
				if (this.down)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.down);
					}
					this.down.Focus(true);
				}
				if (this.OnDownDown != null)
				{
					this.OnDownDown();
				}
			}
			else if (args.KeyCode == KeyCode.LeftArrow)
			{
				if (this.left)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.left);
					}
					this.left.Focus(true);
				}
				if (this.OnLeftDown != null)
				{
					this.OnLeftDown();
				}
			}
			else if (args.KeyCode == KeyCode.RightArrow)
			{
				if (this.right)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.right);
					}
					this.right.Focus(true);
				}
				if (this.OnRightDown != null)
				{
					this.OnRightDown();
				}
			}
			if (this.selectOnAction && this.button && args.KeyCode == KeyCode.Return)
			{
				AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
				this.button.DoClick();
			}
		}
	}

	// Token: 0x0600912F RID: 37167 RVA: 0x003D5CEC File Offset: 0x003D3EEC
	private void GotFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this.clearRepeatingOnSelect)
		{
			if (BraveInput.PrimaryPlayerInstance != null)
			{
				GungeonActions gungeonActions = BraveInput.PrimaryPlayerInstance.ActiveActions;
				gungeonActions.SelectUp.ResetRepeating();
				gungeonActions.SelectDown.ResetRepeating();
				gungeonActions.SelectLeft.ResetRepeating();
				gungeonActions.SelectRight.ResetRepeating();
			}
			if (BraveInput.SecondaryPlayerInstance != null)
			{
				GungeonActions gungeonActions = BraveInput.SecondaryPlayerInstance.ActiveActions;
				gungeonActions.SelectUp.ResetRepeating();
				gungeonActions.SelectDown.ResetRepeating();
				gungeonActions.SelectLeft.ResetRepeating();
				gungeonActions.SelectRight.ResetRepeating();
			}
		}
	}

	// Token: 0x04009938 RID: 39224
	public dfControl up;

	// Token: 0x04009939 RID: 39225
	public dfControl down;

	// Token: 0x0400993A RID: 39226
	public dfControl left;

	// Token: 0x0400993B RID: 39227
	public dfControl right;

	// Token: 0x0400993C RID: 39228
	public bool selectOnAction;

	// Token: 0x0400993D RID: 39229
	public bool clearRepeatingOnSelect;

	// Token: 0x0400993E RID: 39230
	private dfControl selfControl;

	// Token: 0x0400993F RID: 39231
	public Action OnUpDown;

	// Token: 0x04009940 RID: 39232
	public Action OnDownDown;

	// Token: 0x04009941 RID: 39233
	public Action OnLeftDown;

	// Token: 0x04009942 RID: 39234
	public Action OnRightDown;

	// Token: 0x04009943 RID: 39235
	public Action<dfControl> OnNewControlSelected;

	// Token: 0x04009944 RID: 39236
	private static bool m_hasCheckedThisFrame;

	// Token: 0x04009945 RID: 39237
	private static UIKeyControls m_lastFocusedUIKeyControl;

	// Token: 0x04009946 RID: 39238
	private static float m_timer;

	// Token: 0x04009947 RID: 39239
	private const float TIMER_THRESHOLD = 1.5f;

	// Token: 0x04009948 RID: 39240
	private dfButton button;
}
