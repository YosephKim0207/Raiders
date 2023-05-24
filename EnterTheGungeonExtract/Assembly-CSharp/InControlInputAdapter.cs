using System;
using InControl;
using UnityEngine;

// Token: 0x0200179F RID: 6047
public class InControlInputAdapter : MonoBehaviour
{
	// Token: 0x06008D89 RID: 36233 RVA: 0x003B8A7C File Offset: 0x003B6C7C
	private void OnEnable()
	{
	}

	// Token: 0x1700152E RID: 5422
	// (get) Token: 0x06008D8A RID: 36234 RVA: 0x003B8A80 File Offset: 0x003B6C80
	// (set) Token: 0x06008D8B RID: 36235 RVA: 0x003B8A8C File Offset: 0x003B6C8C
	public static bool SkipInputForRestOfFrame
	{
		get
		{
			return InControlInputAdapter.m_skipInputForRestOfFrame > 0;
		}
		set
		{
			InControlInputAdapter.m_skipInputForRestOfFrame = ((!value) ? 0 : Mathf.Max(5, InControlInputAdapter.m_skipInputForRestOfFrame));
		}
	}

	// Token: 0x06008D8C RID: 36236 RVA: 0x003B8AAC File Offset: 0x003B6CAC
	private void Update()
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		bool skipInputForRestOfFrame = InControlInputAdapter.SkipInputForRestOfFrame;
		this.ProcessPrimaryPlayerInput(ref skipInputForRestOfFrame);
		if (GameManager.Instance.IsPaused && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.ProcessSecondaryPlayerInput(ref skipInputForRestOfFrame);
		}
		if (InControlInputAdapter.CurrentlyUsingAllDevices)
		{
			for (int i = 0; i < InputManager.Devices.Count; i++)
			{
				this.ProcessRawDeviceInput(InputManager.Devices[i], ref skipInputForRestOfFrame);
			}
		}
	}

	// Token: 0x06008D8D RID: 36237 RVA: 0x003B8B38 File Offset: 0x003B6D38
	private void ProcessRawDeviceInput(InputDevice device, ref bool didProcessInput)
	{
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || !activeControl.transform.IsChildOf(base.transform))
		{
			return;
		}
		if (!device.LeftStickUp.IsPressed)
		{
			InControlInputAdapter.HandleControl(device.DPadUp, activeControl, KeyCode.UpArrow, ref didProcessInput, true);
		}
		if (!device.LeftStickRight.IsPressed)
		{
			InControlInputAdapter.HandleControl(device.DPadRight, activeControl, KeyCode.RightArrow, ref didProcessInput, true);
		}
		if (!device.LeftStickDown.IsPressed)
		{
			InControlInputAdapter.HandleControl(device.DPadDown, activeControl, KeyCode.DownArrow, ref didProcessInput, true);
		}
		if (!device.LeftStickLeft.IsPressed)
		{
			InControlInputAdapter.HandleControl(device.DPadLeft, activeControl, KeyCode.LeftArrow, ref didProcessInput, true);
		}
		if (!device.DPadUp.IsPressed)
		{
			InControlInputAdapter.HandleControlAsDpad(device.LeftStickUp, activeControl, KeyCode.UpArrow, ref didProcessInput, true);
		}
		if (!device.DPadRight.IsPressed)
		{
			InControlInputAdapter.HandleControlAsDpad(device.LeftStickRight, activeControl, KeyCode.RightArrow, ref didProcessInput, true);
		}
		if (!device.DPadDown.IsPressed)
		{
			InControlInputAdapter.HandleControlAsDpad(device.LeftStickDown, activeControl, KeyCode.DownArrow, ref didProcessInput, true);
		}
		if (!device.DPadLeft.IsPressed)
		{
			InControlInputAdapter.HandleControlAsDpad(device.LeftStickLeft, activeControl, KeyCode.LeftArrow, ref didProcessInput, true);
		}
		InputControlType localizedMenuSelectAction = GungeonActions.LocalizedMenuSelectAction;
		if (localizedMenuSelectAction != InputControlType.Action1)
		{
			if (localizedMenuSelectAction == InputControlType.Action2)
			{
				InControlInputAdapter.HandleControl(device.Action2, activeControl, KeyCode.Return, ref didProcessInput, false);
			}
		}
		else
		{
			InControlInputAdapter.HandleControl(device.Action1, activeControl, KeyCode.Return, ref didProcessInput, false);
		}
		InputControlType localizedMenuCancelAction = GungeonActions.LocalizedMenuCancelAction;
		if (localizedMenuCancelAction != InputControlType.Action1)
		{
			if (localizedMenuCancelAction == InputControlType.Action2)
			{
				InControlInputAdapter.HandleControl(device.Action2, activeControl, KeyCode.Escape, ref didProcessInput, false);
			}
		}
		else
		{
			InControlInputAdapter.HandleControl(device.Action1, activeControl, KeyCode.Escape, ref didProcessInput, false);
		}
	}

	// Token: 0x06008D8E RID: 36238 RVA: 0x003B8D10 File Offset: 0x003B6F10
	private void ProcessPrimaryPlayerInput(ref bool didProcessInput)
	{
		if (BraveInput.PrimaryPlayerInstance == null)
		{
			return;
		}
		GungeonActions activeActions = BraveInput.PrimaryPlayerInstance.ActiveActions;
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || !activeControl.transform.IsChildOf(base.transform))
		{
			return;
		}
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectUp, activeControl, KeyCode.UpArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectDown, activeControl, KeyCode.DownArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectLeft, activeControl, KeyCode.LeftArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectRight, activeControl, KeyCode.RightArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControl(activeActions.MenuSelectAction, activeControl, KeyCode.Return, ref didProcessInput, false);
		InControlInputAdapter.HandleControl(activeActions.CancelAction, activeControl, KeyCode.Escape, ref didProcessInput, false);
		if (Input.GetKeyUp(KeyCode.Return))
		{
			activeControl.OnKeyUp(new dfKeyEventArgs(activeControl, KeyCode.Return, false, false, false));
			didProcessInput = true;
		}
	}

	// Token: 0x06008D8F RID: 36239 RVA: 0x003B8DF0 File Offset: 0x003B6FF0
	private void ProcessSecondaryPlayerInput(ref bool didProcessInput)
	{
		GungeonActions activeActions = BraveInput.SecondaryPlayerInstance.ActiveActions;
		if (activeActions == null || activeActions.ForceDisable)
		{
			return;
		}
		dfControl activeControl = dfGUIManager.ActiveControl;
		if (activeControl == null || !activeControl.transform.IsChildOf(base.transform))
		{
			return;
		}
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectUp, activeControl, KeyCode.UpArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectDown, activeControl, KeyCode.DownArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectLeft, activeControl, KeyCode.LeftArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControlAsDpad(activeActions.SelectRight, activeControl, KeyCode.RightArrow, ref didProcessInput, true);
		InControlInputAdapter.HandleControl(activeActions.MenuSelectAction, activeControl, KeyCode.Return, ref didProcessInput, false);
		InControlInputAdapter.HandleControl(activeActions.CancelAction, activeControl, KeyCode.Escape, ref didProcessInput, false);
	}

	// Token: 0x06008D90 RID: 36240 RVA: 0x003B8EB0 File Offset: 0x003B70B0
	public void LateUpdate()
	{
		InControlInputAdapter.m_skipInputForRestOfFrame = Mathf.Max(InControlInputAdapter.m_skipInputForRestOfFrame - 1, 0);
	}

	// Token: 0x06008D91 RID: 36241 RVA: 0x003B8EC4 File Offset: 0x003B70C4
	private static void HandleControl(OneAxisInputControl control, dfControl target, KeyCode keyCode, ref bool didProcessInput, bool repeating = false)
	{
		if ((!repeating) ? control.WasPressed : control.WasPressedRepeating)
		{
			if (!didProcessInput)
			{
				target.OnKeyDown(new dfKeyEventArgs(target, keyCode, false, false, false));
				didProcessInput = true;
			}
		}
		else if (control.WasReleased && !didProcessInput)
		{
			target.OnKeyUp(new dfKeyEventArgs(target, keyCode, false, false, false));
			didProcessInput = true;
		}
	}

	// Token: 0x06008D92 RID: 36242 RVA: 0x003B8F34 File Offset: 0x003B7134
	private static void HandleControlAsDpad(OneAxisInputControl control, dfControl target, KeyCode keyCode, ref bool didProcessInput, bool repeating = false)
	{
		if ((!repeating) ? control.WasPressedAsDpad : control.WasPressedAsDpadRepeating)
		{
			if (!didProcessInput)
			{
				target.OnKeyDown(new dfKeyEventArgs(target, keyCode, false, false, false));
				didProcessInput = true;
			}
		}
		else if (control.WasReleased && !didProcessInput)
		{
			target.OnKeyUp(new dfKeyEventArgs(target, keyCode, false, false, false));
			didProcessInput = true;
		}
	}

	// Token: 0x04009545 RID: 38213
	private static int m_skipInputForRestOfFrame;

	// Token: 0x04009546 RID: 38214
	public static bool CurrentlyUsingAllDevices;
}
