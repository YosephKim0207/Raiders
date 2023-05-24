using System;
using InControl;
using UnityEngine;

// Token: 0x02000CDF RID: 3295
public class GungeonActions : PlayerActionSet
{
	// Token: 0x060045EB RID: 17899 RVA: 0x0016B3D8 File Offset: 0x001695D8
	public GungeonActions()
	{
		this.Left = base.CreatePlayerAction("Move Left");
		this.Right = base.CreatePlayerAction("Move Right");
		this.Up = base.CreatePlayerAction("Move Up");
		this.Down = base.CreatePlayerAction("Move Down");
		this.Move = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		this.AimLeft = base.CreatePlayerAction("Aim Left");
		this.AimRight = base.CreatePlayerAction("Aim Right");
		this.AimUp = base.CreatePlayerAction("Aim Up");
		this.AimDown = base.CreatePlayerAction("Aim Down");
		this.Aim = base.CreateTwoAxisPlayerAction(this.AimLeft, this.AimRight, this.AimDown, this.AimUp);
		this.SelectLeft = base.CreatePlayerAction("Select Left");
		this.SelectRight = base.CreatePlayerAction("Select Right");
		this.SelectUp = base.CreatePlayerAction("Select Up");
		this.SelectDown = base.CreatePlayerAction("Select Down");
		this.SelectUp.StateThreshold = 0.5f;
		this.SelectDown.StateThreshold = 0.5f;
		this.SelectLeft.StateThreshold = 0.5f;
		this.SelectRight.StateThreshold = 0.5f;
		this.SelectAxis = base.CreateTwoAxisPlayerAction(this.SelectLeft, this.SelectRight, this.SelectDown, this.SelectUp);
		this.SelectAxis.StateThreshold = 0.9f;
		this.ShootAction = base.CreatePlayerAction("Shoot");
		this.DodgeRollAction = base.CreatePlayerAction("Dodge Roll");
		this.InteractAction = base.CreatePlayerAction("Interact");
		this.CancelAction = base.CreatePlayerAction("Cancel");
		this.ReloadAction = base.CreatePlayerAction("Reload");
		this.UseItemAction = base.CreatePlayerAction("Use Item");
		this.MapAction = base.CreatePlayerAction("Map");
		this.GunUpAction = base.CreatePlayerAction("Cycle Gun Up");
		this.GunDownAction = base.CreatePlayerAction("Cycle Gun Down");
		this.ItemUpAction = base.CreatePlayerAction("Cycle Item Up");
		this.ItemDownAction = base.CreatePlayerAction("Cycle Item Down");
		this.KeybulletAction = base.CreatePlayerAction("Keybullet");
		this.PauseAction = base.CreatePlayerAction("Pause");
		this.DropGunAction = base.CreatePlayerAction("Drop Gun");
		this.DropItemAction = base.CreatePlayerAction("Drop Item");
		this.EquipmentMenuAction = base.CreatePlayerAction("Equipment Menu");
		this.BlankAction = base.CreatePlayerAction("Blank");
		this.GunQuickEquipAction = base.CreatePlayerAction("Gun Quick Equip");
		this.SwapDualGunsAction = base.CreatePlayerAction("Swap Dual Guns");
		this.MenuSelectAction = base.CreatePlayerAction("Menu Select");
		this.MinimapZoomInAction = base.CreatePlayerAction("Minimap Zoom In");
		this.MinimapZoomOutAction = base.CreatePlayerAction("Minimap Zoom Out");
		this.PunchoutDodgeLeft = base.CreatePlayerAction("Dodge Left");
		this.PunchoutDodgeRight = base.CreatePlayerAction("Dodge Right");
		this.PunchoutBlock = base.CreatePlayerAction("Block");
		this.PunchoutDuck = base.CreatePlayerAction("Duck");
		this.PunchoutPunchLeft = base.CreatePlayerAction("Punch Left");
		this.PunchoutPunchRight = base.CreatePlayerAction("Punch Right");
		this.PunchoutSuper = base.CreatePlayerAction("Super");
		this.PunchoutDodgeLeft.StateThreshold = 0.3f;
		this.PunchoutDodgeRight.StateThreshold = 0.3f;
		this.PunchoutBlock.StateThreshold = 0.3f;
		this.PunchoutDuck.StateThreshold = 0.3f;
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x0016B7A0 File Offset: 0x001699A0
	public PlayerAction GetActionFromType(GungeonActions.GungeonActionType type)
	{
		switch (type)
		{
		case GungeonActions.GungeonActionType.Left:
			return this.Left;
		case GungeonActions.GungeonActionType.Right:
			return this.Right;
		case GungeonActions.GungeonActionType.Up:
			return this.Up;
		case GungeonActions.GungeonActionType.Down:
			return this.Down;
		case GungeonActions.GungeonActionType.AimLeft:
			return this.AimLeft;
		case GungeonActions.GungeonActionType.AimRight:
			return this.AimRight;
		case GungeonActions.GungeonActionType.AimUp:
			return this.AimUp;
		case GungeonActions.GungeonActionType.AimDown:
			return this.AimDown;
		case GungeonActions.GungeonActionType.Shoot:
			return this.ShootAction;
		case GungeonActions.GungeonActionType.DodgeRoll:
			return this.DodgeRollAction;
		case GungeonActions.GungeonActionType.Interact:
			return this.InteractAction;
		case GungeonActions.GungeonActionType.Reload:
			return this.ReloadAction;
		case GungeonActions.GungeonActionType.UseItem:
			return this.UseItemAction;
		case GungeonActions.GungeonActionType.Map:
			return this.MapAction;
		case GungeonActions.GungeonActionType.CycleGunUp:
			return this.GunUpAction;
		case GungeonActions.GungeonActionType.CycleGunDown:
			return this.GunDownAction;
		case GungeonActions.GungeonActionType.CycleItemUp:
			return this.ItemUpAction;
		case GungeonActions.GungeonActionType.CycleItemDown:
			return this.ItemDownAction;
		case GungeonActions.GungeonActionType.Keybullet:
			return this.KeybulletAction;
		case GungeonActions.GungeonActionType.Pause:
			return this.PauseAction;
		case GungeonActions.GungeonActionType.SelectLeft:
			return this.SelectLeft;
		case GungeonActions.GungeonActionType.SelectRight:
			return this.SelectRight;
		case GungeonActions.GungeonActionType.SelectUp:
			return this.SelectUp;
		case GungeonActions.GungeonActionType.SelectDown:
			return this.SelectDown;
		case GungeonActions.GungeonActionType.Cancel:
			return this.CancelAction;
		case GungeonActions.GungeonActionType.DropGun:
			return this.DropGunAction;
		case GungeonActions.GungeonActionType.EquipmentMenu:
			return this.EquipmentMenuAction;
		case GungeonActions.GungeonActionType.Blank:
			return this.BlankAction;
		case GungeonActions.GungeonActionType.GunQuickEquip:
			return this.GunQuickEquipAction;
		case GungeonActions.GungeonActionType.MenuInteract:
			return this.MenuSelectAction;
		case GungeonActions.GungeonActionType.DropItem:
			return this.DropItemAction;
		case GungeonActions.GungeonActionType.MinimapZoomIn:
			return this.MinimapZoomInAction;
		case GungeonActions.GungeonActionType.MinimapZoomOut:
			return this.MinimapZoomOutAction;
		case GungeonActions.GungeonActionType.SwapDualGuns:
			return this.SwapDualGunsAction;
		case GungeonActions.GungeonActionType.PunchoutDodgeLeft:
			return this.PunchoutDodgeLeft;
		case GungeonActions.GungeonActionType.PunchoutDodgeRight:
			return this.PunchoutDodgeRight;
		case GungeonActions.GungeonActionType.PunchoutBlock:
			return this.PunchoutBlock;
		case GungeonActions.GungeonActionType.PunchoutDuck:
			return this.PunchoutDuck;
		case GungeonActions.GungeonActionType.PunchoutPunchLeft:
			return this.PunchoutPunchLeft;
		case GungeonActions.GungeonActionType.PunchoutPunchRight:
			return this.PunchoutPunchRight;
		case GungeonActions.GungeonActionType.PunchoutSuper:
			return this.PunchoutSuper;
		default:
			return null;
		}
	}

	// Token: 0x060045ED RID: 17901 RVA: 0x0016B97C File Offset: 0x00169B7C
	public bool IntroSkipActionPressed()
	{
		return this.MenuSelectAction.WasPressed || this.PauseAction.WasPressed || this.CancelAction.WasPressed;
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x0016B9AC File Offset: 0x00169BAC
	public bool AnyActionPressed()
	{
		for (int i = 0; i < base.Actions.Count; i++)
		{
			if (base.Actions[i].WasPressed)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060045EF RID: 17903 RVA: 0x0016B9F0 File Offset: 0x00169BF0
	public void IgnoreBindingsOfType(BindingSourceType sourceType)
	{
		for (int i = 0; i < base.Actions.Count; i++)
		{
			base.Actions[i].IgnoreBindingsOfType(sourceType);
		}
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x0016BA2C File Offset: 0x00169C2C
	public void ClearBindingsOfType(BindingSourceType sourceType)
	{
		for (int i = 0; i < base.Actions.Count; i++)
		{
			base.Actions[i].ClearBindingsOfType(sourceType);
		}
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x0016BA68 File Offset: 0x00169C68
	public bool CheckBothSticksButton()
	{
		return base.Device != null && ((base.Device.LeftStickButton.WasPressed && base.Device.RightStickButton.IsPressed) || (base.Device.LeftStickButton.IsPressed && base.Device.RightStickButton.WasPressed) || (base.Device.LeftStickButton.WasPressed && base.Device.RightStickButton.WasPressed));
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0016BB08 File Offset: 0x00169D08
	public bool CheckAllFaceButtonsPressed()
	{
		int num = 0;
		if (base.Device.Action1.IsPressed)
		{
			num++;
		}
		if (base.Device.Action2.IsPressed)
		{
			num++;
		}
		if (base.Device.Action3.IsPressed)
		{
			num++;
		}
		if (base.Device.Action4.IsPressed)
		{
			num++;
		}
		return num >= 3;
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x0016BB84 File Offset: 0x00169D84
	public void PostProcessAdditionalBlankControls(int playerNum)
	{
		GameOptions.ControllerBlankControl controllerBlankControl = ((playerNum != 0) ? GameManager.Options.additionalBlankControlTwo : GameManager.Options.additionalBlankControl);
		if (controllerBlankControl != GameOptions.ControllerBlankControl.CIRCLE)
		{
			if (controllerBlankControl == GameOptions.ControllerBlankControl.L1)
			{
				this.DodgeRollAction.RemoveBindingOfType(InputControlType.LeftBumper);
				if (this.BlankAction.Bindings.Count < 2)
				{
					this.BlankAction.AddBinding(new DeviceBindingSource(InputControlType.LeftBumper));
				}
				if (playerNum == 0)
				{
					GameManager.Options.additionalBlankControl = GameOptions.ControllerBlankControl.NONE;
					GameManager.Options.CurrentControlPreset = GameOptions.ControlPreset.CUSTOM;
				}
				else if (playerNum == 1)
				{
					GameManager.Options.additionalBlankControlTwo = GameOptions.ControllerBlankControl.NONE;
					GameManager.Options.CurrentControlPresetP2 = GameOptions.ControlPreset.CUSTOM;
				}
			}
		}
		else
		{
			this.DodgeRollAction.RemoveBindingOfType(InputControlType.Action2);
			if (this.BlankAction.Bindings.Count < 2)
			{
				this.BlankAction.AddBinding(new DeviceBindingSource(InputControlType.Action2));
			}
			if (playerNum == 0)
			{
				GameManager.Options.additionalBlankControl = GameOptions.ControllerBlankControl.NONE;
				GameManager.Options.CurrentControlPreset = GameOptions.ControlPreset.CUSTOM;
			}
			else if (playerNum == 1)
			{
				GameManager.Options.additionalBlankControlTwo = GameOptions.ControllerBlankControl.NONE;
				GameManager.Options.CurrentControlPresetP2 = GameOptions.ControlPreset.CUSTOM;
			}
		}
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x0016BCB8 File Offset: 0x00169EB8
	public void ReinitializeDefaults()
	{
		for (int i = 0; i < base.Actions.Count; i++)
		{
			base.Actions[i].ResetBindings();
		}
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x0016BCF4 File Offset: 0x00169EF4
	public void InitializeSwappedTriggersPreset()
	{
		for (int i = 0; i < base.Actions.Count; i++)
		{
			base.Actions[i].ResetBindings();
		}
		this.ShootAction.ClearBindings();
		this.ShootAction.AddBinding(new DeviceBindingSource(InputControlType.RightTrigger));
		this.DodgeRollAction.ClearBindings();
		this.DodgeRollAction.AddBinding(new DeviceBindingSource(InputControlType.LeftTrigger));
		this.DodgeRollAction.AddBinding(new DeviceBindingSource(InputControlType.Action2));
		this.MapAction.ClearBindings();
		this.MapAction.AddBinding(new DeviceBindingSource(InputControlType.LeftBumper));
		this.MapAction.AddBinding(new KeyBindingSource(new Key[] { Key.Tab }));
		this.UseItemAction.ClearBindings();
		this.UseItemAction.AddBinding(new DeviceBindingSource(InputControlType.RightBumper));
		this.UseItemAction.AddBinding(new KeyBindingSource(new Key[] { Key.Space }));
		this.ShootAction.AddBinding(new MouseBindingSource(Mouse.LeftButton));
		this.DodgeRollAction.AddBinding(new MouseBindingSource(Mouse.RightButton));
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x060045F6 RID: 17910 RVA: 0x0016BE18 File Offset: 0x0016A018
	public static InputControlType LocalizedMenuSelectAction
	{
		get
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
			{
				return InputControlType.Action2;
			}
			return InputControlType.Action1;
		}
	}

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x060045F7 RID: 17911 RVA: 0x0016BE30 File Offset: 0x0016A030
	public static InputControlType LocalizedMenuCancelAction
	{
		get
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
			{
				return InputControlType.Action1;
			}
			return InputControlType.Action2;
		}
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x0016BE48 File Offset: 0x0016A048
	public void ReinitializeMenuDefaults()
	{
		this.CancelAction.ResetBindings();
		this.MenuSelectAction.ResetBindings();
		this.CancelAction.ClearBindings();
		this.MenuSelectAction.ClearBindings();
		this.CancelAction.AddDefaultBinding(GungeonActions.LocalizedMenuCancelAction);
		this.CancelAction.AddDefaultBinding(new Key[] { Key.Escape });
		this.MenuSelectAction.AddDefaultBinding(GungeonActions.LocalizedMenuSelectAction);
		this.MenuSelectAction.AddDefaultBinding(new Key[] { Key.Return });
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x0016BED0 File Offset: 0x0016A0D0
	public void InitializeDefaults()
	{
		this.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.Left.AddDefaultBinding(new Key[] { Key.A });
		this.Right.AddDefaultBinding(InputControlType.LeftStickRight);
		this.Right.AddDefaultBinding(new Key[] { Key.D });
		this.Up.AddDefaultBinding(InputControlType.LeftStickUp);
		this.Up.AddDefaultBinding(new Key[] { Key.W });
		this.Down.AddDefaultBinding(InputControlType.LeftStickDown);
		this.Down.AddDefaultBinding(new Key[] { Key.S });
		this.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		this.AimRight.AddDefaultBinding(InputControlType.RightStickRight);
		this.AimUp.AddDefaultBinding(InputControlType.RightStickUp);
		this.AimDown.AddDefaultBinding(InputControlType.RightStickDown);
		this.SelectLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.SelectLeft.AddDefaultBinding(InputControlType.DPadLeft);
		this.SelectLeft.AddDefaultBinding(new Key[] { Key.A });
		this.SelectLeft.AddDefaultBinding(new Key[] { Key.LeftArrow });
		this.SelectRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.SelectRight.AddDefaultBinding(InputControlType.DPadRight);
		this.SelectRight.AddDefaultBinding(new Key[] { Key.D });
		this.SelectRight.AddDefaultBinding(new Key[] { Key.RightArrow });
		this.SelectUp.AddDefaultBinding(InputControlType.LeftStickUp);
		this.SelectUp.AddDefaultBinding(InputControlType.DPadUp);
		this.SelectUp.AddDefaultBinding(new Key[] { Key.W });
		this.SelectUp.AddDefaultBinding(new Key[] { Key.UpArrow });
		this.SelectDown.AddDefaultBinding(InputControlType.LeftStickDown);
		this.SelectDown.AddDefaultBinding(InputControlType.DPadDown);
		this.SelectDown.AddDefaultBinding(new Key[] { Key.S });
		this.SelectDown.AddDefaultBinding(new Key[] { Key.DownArrow });
		this.ShootAction.AddDefaultBinding(InputControlType.RightBumper);
		this.DodgeRollAction.AddDefaultBinding(InputControlType.LeftBumper);
		this.DodgeRollAction.AddDefaultBinding(InputControlType.Action2);
		this.InteractAction.AddDefaultBinding(InputControlType.Action1);
		this.InteractAction.AddDefaultBinding(new Key[] { Key.E });
		this.ReloadAction.AddDefaultBinding(InputControlType.Action3);
		this.ReloadAction.AddDefaultBinding(new Key[] { Key.R });
		this.UseItemAction.AddDefaultBinding(InputControlType.RightTrigger);
		this.UseItemAction.AddDefaultBinding(new Key[] { Key.Space });
		this.MapAction.AddDefaultBinding(InputControlType.LeftTrigger);
		this.MapAction.AddDefaultBinding(new Key[] { Key.Tab });
		this.GunUpAction.AddDefaultBinding(InputControlType.DPadLeft);
		this.GunDownAction.AddDefaultBinding(InputControlType.DPadRight);
		this.DropGunAction.AddDefaultBinding(new Key[] { Key.F });
		this.DropGunAction.AddDefaultBinding(InputControlType.DPadDown);
		this.DropItemAction.AddDefaultBinding(new Key[] { Key.G });
		this.DropItemAction.AddDefaultBinding(InputControlType.DPadUp);
		this.ItemUpAction.AddDefaultBinding(InputControlType.DPadUp);
		this.ItemUpAction.AddDefaultBinding(new Key[] { Key.LeftShift });
		this.GunQuickEquipAction.AddDefaultBinding(InputControlType.Action4);
		this.GunQuickEquipAction.AddDefaultBinding(new Key[] { Key.LeftControl });
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			this.GunQuickEquipAction.AddDefaultBinding(new Key[] { Key.LeftCommand });
		}
		this.SwapDualGunsAction.AddDefaultBinding(Mouse.MiddleButton);
		this.PauseAction.AddDefaultBinding(InputControlType.Start);
		this.PauseAction.AddDefaultBinding(InputControlType.Select);
		this.PauseAction.AddDefaultBinding(InputControlType.Options);
		this.PauseAction.AddDefaultBinding(new Key[] { Key.Escape });
		this.EquipmentMenuAction.AddDefaultBinding(InputControlType.TouchPadButton);
		this.EquipmentMenuAction.AddDefaultBinding(InputControlType.Back);
		this.EquipmentMenuAction.AddDefaultBinding(new Key[] { Key.I });
		this.BlankAction.AddDefaultBinding(new Key[] { Key.Q });
		this.ReinitializeMenuDefaults();
		this.MinimapZoomInAction.AddDefaultBinding(new Key[] { Key.Equals });
		this.MinimapZoomInAction.AddDefaultBinding(new Key[] { Key.PadPlus });
		this.MinimapZoomOutAction.AddDefaultBinding(new Key[] { Key.Minus });
		this.MinimapZoomOutAction.AddDefaultBinding(new Key[] { Key.PadMinus });
		this.GunUpAction.AddDefaultBinding(Mouse.PositiveScrollWheel);
		this.GunDownAction.AddDefaultBinding(Mouse.NegativeScrollWheel);
		this.DodgeRollAction.AddDefaultBinding(Mouse.RightButton);
		this.ShootAction.AddDefaultBinding(Mouse.LeftButton);
		this.PunchoutDodgeLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.PunchoutDodgeLeft.AddDefaultBinding(InputControlType.DPadLeft);
		this.PunchoutDodgeLeft.AddDefaultBinding(new Key[] { Key.A });
		this.PunchoutDodgeRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.PunchoutDodgeRight.AddDefaultBinding(InputControlType.DPadRight);
		this.PunchoutDodgeRight.AddDefaultBinding(new Key[] { Key.D });
		this.PunchoutBlock.AddDefaultBinding(InputControlType.LeftStickUp);
		this.PunchoutBlock.AddDefaultBinding(InputControlType.DPadUp);
		this.PunchoutBlock.AddDefaultBinding(new Key[] { Key.W });
		this.PunchoutDuck.AddDefaultBinding(InputControlType.LeftStickDown);
		this.PunchoutDuck.AddDefaultBinding(InputControlType.DPadDown);
		this.PunchoutDuck.AddDefaultBinding(new Key[] { Key.S });
		this.PunchoutPunchLeft.AddDefaultBinding(InputControlType.Action1);
		this.PunchoutPunchLeft.AddDefaultBinding(InputControlType.LeftBumper);
		this.PunchoutPunchRight.AddDefaultBinding(InputControlType.Action2);
		this.PunchoutPunchRight.AddDefaultBinding(InputControlType.RightBumper);
		this.PunchoutSuper.AddDefaultBinding(InputControlType.Action3);
		this.PunchoutSuper.AddDefaultBinding(new Key[] { Key.Space });
		this.PunchoutPunchLeft.AddDefaultBinding(Mouse.LeftButton);
		this.PunchoutPunchRight.AddDefaultBinding(Mouse.RightButton);
	}

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x060045FA RID: 17914 RVA: 0x0016C478 File Offset: 0x0016A678
	// (set) Token: 0x060045FB RID: 17915 RVA: 0x0016C480 File Offset: 0x0016A680
	public bool HighAccuracyAimMode
	{
		get
		{
			return this.m_highAccuraceAimMode;
		}
		set
		{
			if (value != this.m_highAccuraceAimMode)
			{
				this.SetHighAccuracy(this.AimLeft, value);
				this.SetHighAccuracy(this.AimRight, value);
				this.SetHighAccuracy(this.AimUp, value);
				this.SetHighAccuracy(this.AimDown, value);
				this.m_highAccuraceAimMode = value;
			}
		}
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x0016C4D4 File Offset: 0x0016A6D4
	private void SetHighAccuracy(PlayerAction action, bool value)
	{
		foreach (BindingSource bindingSource in action.Bindings)
		{
			DeviceBindingSource deviceBindingSource = bindingSource as DeviceBindingSource;
			if (deviceBindingSource != null)
			{
				deviceBindingSource.ForceRawInput = value;
			}
		}
	}

	// Token: 0x0400382E RID: 14382
	public PlayerAction Left;

	// Token: 0x0400382F RID: 14383
	public PlayerAction Right;

	// Token: 0x04003830 RID: 14384
	public PlayerAction Up;

	// Token: 0x04003831 RID: 14385
	public PlayerAction Down;

	// Token: 0x04003832 RID: 14386
	public PlayerTwoAxisAction Move;

	// Token: 0x04003833 RID: 14387
	public PlayerAction AimLeft;

	// Token: 0x04003834 RID: 14388
	public PlayerAction AimRight;

	// Token: 0x04003835 RID: 14389
	public PlayerAction AimUp;

	// Token: 0x04003836 RID: 14390
	public PlayerAction AimDown;

	// Token: 0x04003837 RID: 14391
	public PlayerTwoAxisAction Aim;

	// Token: 0x04003838 RID: 14392
	public PlayerAction SelectLeft;

	// Token: 0x04003839 RID: 14393
	public PlayerAction SelectRight;

	// Token: 0x0400383A RID: 14394
	public PlayerAction SelectUp;

	// Token: 0x0400383B RID: 14395
	public PlayerAction SelectDown;

	// Token: 0x0400383C RID: 14396
	public PlayerTwoAxisAction SelectAxis;

	// Token: 0x0400383D RID: 14397
	public PlayerAction ShootAction;

	// Token: 0x0400383E RID: 14398
	public PlayerAction DodgeRollAction;

	// Token: 0x0400383F RID: 14399
	public PlayerAction InteractAction;

	// Token: 0x04003840 RID: 14400
	public PlayerAction ReloadAction;

	// Token: 0x04003841 RID: 14401
	public PlayerAction UseItemAction;

	// Token: 0x04003842 RID: 14402
	public PlayerAction MapAction;

	// Token: 0x04003843 RID: 14403
	public PlayerAction GunUpAction;

	// Token: 0x04003844 RID: 14404
	public PlayerAction GunDownAction;

	// Token: 0x04003845 RID: 14405
	public PlayerAction ItemUpAction;

	// Token: 0x04003846 RID: 14406
	public PlayerAction ItemDownAction;

	// Token: 0x04003847 RID: 14407
	public PlayerAction KeybulletAction;

	// Token: 0x04003848 RID: 14408
	public PlayerAction PauseAction;

	// Token: 0x04003849 RID: 14409
	public PlayerAction CancelAction;

	// Token: 0x0400384A RID: 14410
	public PlayerAction MenuSelectAction;

	// Token: 0x0400384B RID: 14411
	public PlayerAction EquipmentMenuAction;

	// Token: 0x0400384C RID: 14412
	public PlayerAction BlankAction;

	// Token: 0x0400384D RID: 14413
	public PlayerAction DropGunAction;

	// Token: 0x0400384E RID: 14414
	public PlayerAction DropItemAction;

	// Token: 0x0400384F RID: 14415
	public PlayerAction GunQuickEquipAction;

	// Token: 0x04003850 RID: 14416
	public PlayerAction MinimapZoomInAction;

	// Token: 0x04003851 RID: 14417
	public PlayerAction MinimapZoomOutAction;

	// Token: 0x04003852 RID: 14418
	public PlayerAction SwapDualGunsAction;

	// Token: 0x04003853 RID: 14419
	public PlayerAction PunchoutDodgeLeft;

	// Token: 0x04003854 RID: 14420
	public PlayerAction PunchoutDodgeRight;

	// Token: 0x04003855 RID: 14421
	public PlayerAction PunchoutBlock;

	// Token: 0x04003856 RID: 14422
	public PlayerAction PunchoutDuck;

	// Token: 0x04003857 RID: 14423
	public PlayerAction PunchoutPunchLeft;

	// Token: 0x04003858 RID: 14424
	public PlayerAction PunchoutPunchRight;

	// Token: 0x04003859 RID: 14425
	public PlayerAction PunchoutSuper;

	// Token: 0x0400385A RID: 14426
	private bool m_highAccuraceAimMode;

	// Token: 0x02000CE0 RID: 3296
	public enum GungeonActionType
	{
		// Token: 0x0400385C RID: 14428
		Left,
		// Token: 0x0400385D RID: 14429
		Right,
		// Token: 0x0400385E RID: 14430
		Up,
		// Token: 0x0400385F RID: 14431
		Down,
		// Token: 0x04003860 RID: 14432
		AimLeft,
		// Token: 0x04003861 RID: 14433
		AimRight,
		// Token: 0x04003862 RID: 14434
		AimUp,
		// Token: 0x04003863 RID: 14435
		AimDown,
		// Token: 0x04003864 RID: 14436
		Shoot,
		// Token: 0x04003865 RID: 14437
		DodgeRoll,
		// Token: 0x04003866 RID: 14438
		Interact,
		// Token: 0x04003867 RID: 14439
		Reload,
		// Token: 0x04003868 RID: 14440
		UseItem,
		// Token: 0x04003869 RID: 14441
		Map,
		// Token: 0x0400386A RID: 14442
		CycleGunUp,
		// Token: 0x0400386B RID: 14443
		CycleGunDown,
		// Token: 0x0400386C RID: 14444
		CycleItemUp,
		// Token: 0x0400386D RID: 14445
		CycleItemDown,
		// Token: 0x0400386E RID: 14446
		Keybullet,
		// Token: 0x0400386F RID: 14447
		Pause,
		// Token: 0x04003870 RID: 14448
		SelectLeft,
		// Token: 0x04003871 RID: 14449
		SelectRight,
		// Token: 0x04003872 RID: 14450
		SelectUp,
		// Token: 0x04003873 RID: 14451
		SelectDown,
		// Token: 0x04003874 RID: 14452
		Cancel,
		// Token: 0x04003875 RID: 14453
		DropGun,
		// Token: 0x04003876 RID: 14454
		EquipmentMenu,
		// Token: 0x04003877 RID: 14455
		Blank,
		// Token: 0x04003878 RID: 14456
		GunQuickEquip,
		// Token: 0x04003879 RID: 14457
		MenuInteract,
		// Token: 0x0400387A RID: 14458
		DropItem,
		// Token: 0x0400387B RID: 14459
		MinimapZoomIn,
		// Token: 0x0400387C RID: 14460
		MinimapZoomOut,
		// Token: 0x0400387D RID: 14461
		SwapDualGuns,
		// Token: 0x0400387E RID: 14462
		PunchoutDodgeLeft,
		// Token: 0x0400387F RID: 14463
		PunchoutDodgeRight,
		// Token: 0x04003880 RID: 14464
		PunchoutBlock,
		// Token: 0x04003881 RID: 14465
		PunchoutDuck,
		// Token: 0x04003882 RID: 14466
		PunchoutPunchLeft,
		// Token: 0x04003883 RID: 14467
		PunchoutPunchRight,
		// Token: 0x04003884 RID: 14468
		PunchoutSuper
	}
}
