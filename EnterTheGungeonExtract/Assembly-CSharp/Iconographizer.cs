using System;
using System.Collections;
using System.Collections.ObjectModel;
using InControl;
using UnityEngine;

// Token: 0x0200179D RID: 6045
public class Iconographizer : MonoBehaviour
{
	// Token: 0x06008D7F RID: 36223 RVA: 0x003B86D8 File Offset: 0x003B68D8
	private IEnumerator Start()
	{
		yield return null;
		this.m_sprite = base.GetComponent<tk2dBaseSprite>();
		this.m_control = base.GetComponent<dfSprite>();
		if (this.m_control)
		{
			this.m_control.IsVisibleChanged += this.HandleVisibilityChanged;
			this.HandleVisibilityChanged(this.m_control, false);
		}
		else if (this.m_sprite)
		{
			GameOptions.ControllerSymbology playerOneCurrentSymbology = BraveInput.PlayerOneCurrentSymbology;
			string text;
			if (playerOneCurrentSymbology != GameOptions.ControllerSymbology.PS4)
			{
				if (playerOneCurrentSymbology != GameOptions.ControllerSymbology.Switch)
				{
					text = this.XboxSpriteName;
				}
				else
				{
					text = this.SwitchSpriteName;
				}
			}
			else
			{
				text = this.SonySpriteName;
			}
			this.m_sprite.SetSprite(text);
		}
		yield break;
	}

	// Token: 0x06008D80 RID: 36224 RVA: 0x003B86F4 File Offset: 0x003B68F4
	private void HandleVisibilityChanged(dfControl control, bool value)
	{
		Vector2 vector = this.m_control.Position + new Vector2(this.m_control.Size.x / 2f, -this.m_control.Size.y / 2f);
		string text;
		if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.PS4)
		{
			text = this.SonySpriteName;
		}
		else if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.Switch)
		{
			text = this.SwitchSpriteName;
		}
		else
		{
			text = this.XboxSpriteName;
		}
		if (this.InteractOverride)
		{
			string controllerInteractKey = this.GetControllerInteractKey();
			if (controllerInteractKey != null)
			{
				text = controllerInteractKey;
			}
		}
		if (text != null)
		{
			this.m_control.SpriteName = text;
			if (this.DoResize)
			{
				this.m_control.Size = this.m_control.SpriteInfo.sizeInPixels * this.ResizeScale;
				this.m_control.Position = vector + new Vector2(-this.m_control.Size.x / 2f, this.m_control.Size.y / 2f);
			}
		}
	}

	// Token: 0x06008D81 RID: 36225 RVA: 0x003B8838 File Offset: 0x003B6A38
	private string GetControllerInteractKey()
	{
		if (!Minimap.HasInstance)
		{
			return null;
		}
		BraveInput primaryPlayerInstance = BraveInput.PrimaryPlayerInstance;
		if (primaryPlayerInstance == null || primaryPlayerInstance.IsKeyboardAndMouse(false))
		{
			return null;
		}
		GungeonActions activeActions = primaryPlayerInstance.ActiveActions;
		if (activeActions != null && activeActions.InteractAction.Bindings.Count > 0)
		{
			ReadOnlyCollection<BindingSource> bindings = activeActions.InteractAction.Bindings;
			for (int i = 0; i < bindings.Count; i++)
			{
				DeviceBindingSource deviceBindingSource = bindings[i] as DeviceBindingSource;
				if (deviceBindingSource != null && deviceBindingSource.Control != InputControlType.None)
				{
					return UIControllerButtonHelper.GetControllerButtonSpriteName(deviceBindingSource.Control, BraveInput.PlayerOneCurrentSymbology, null);
				}
			}
		}
		return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action1, BraveInput.PlayerOneCurrentSymbology, null);
	}

	// Token: 0x04009538 RID: 38200
	public string SonySpriteName;

	// Token: 0x04009539 RID: 38201
	public string XboxSpriteName;

	// Token: 0x0400953A RID: 38202
	public string SwitchSpriteName;

	// Token: 0x0400953B RID: 38203
	public string SwitchSingleJoyConSpriteName;

	// Token: 0x0400953C RID: 38204
	public bool InteractOverride;

	// Token: 0x0400953D RID: 38205
	public bool DoResize;

	// Token: 0x0400953E RID: 38206
	[ShowInInspectorIf("DoResize", true)]
	public float ResizeScale = 3f;

	// Token: 0x0400953F RID: 38207
	private dfSprite m_control;

	// Token: 0x04009540 RID: 38208
	private tk2dBaseSprite m_sprite;
}
