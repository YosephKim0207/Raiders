using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
[AddComponentMenu("Daikon Forge/Examples/Tooltip/Floating Tooltip")]
public class DemoFloatingTooltip : MonoBehaviour
{
	// Token: 0x06001A6C RID: 6764 RVA: 0x0007AE44 File Offset: 0x00079044
	public void Start()
	{
		this._tooltip = base.GetComponent<dfLabel>();
		this._tooltip.IsInteractive = false;
		this._tooltip.IsEnabled = false;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0007AE6C File Offset: 0x0007906C
	public void Update()
	{
		dfControl controlUnderMouse = dfInputManager.ControlUnderMouse;
		if (controlUnderMouse == null)
		{
			this._tooltip.Hide();
		}
		else if (controlUnderMouse != this.lastControl)
		{
			this.tooltipDelayStart = Time.realtimeSinceStartup;
			if (string.IsNullOrEmpty(controlUnderMouse.Tooltip))
			{
				this._tooltip.Hide();
			}
			else
			{
				this._tooltip.Text = controlUnderMouse.Tooltip;
			}
		}
		else if (this.lastControl != null && !string.IsNullOrEmpty(this.lastControl.Tooltip) && Time.realtimeSinceStartup - this.tooltipDelayStart > this.tooltipDelay)
		{
			this._tooltip.Show();
			this._tooltip.BringToFront();
		}
		if (this._tooltip.IsVisible)
		{
			this.setPosition(Input.mousePosition);
		}
		this.lastControl = controlUnderMouse;
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x0007AF68 File Offset: 0x00079168
	private void setPosition(Vector2 position)
	{
		Vector2 vector = new Vector2(0f, this._tooltip.Height + 25f);
		dfGUIManager manager = this._tooltip.GetManager();
		position = manager.ScreenToGui(position) - vector;
		if (position.y < 0f)
		{
			position.y = 0f;
		}
		if (position.x + this._tooltip.Width > manager.GetScreenSize().x)
		{
			position.x = manager.GetScreenSize().x - this._tooltip.Width;
		}
		this._tooltip.RelativePosition = position;
	}

	// Token: 0x040014A4 RID: 5284
	public float tooltipDelay = 1f;

	// Token: 0x040014A5 RID: 5285
	private dfLabel _tooltip;

	// Token: 0x040014A6 RID: 5286
	private dfControl lastControl;

	// Token: 0x040014A7 RID: 5287
	private float tooltipDelayStart;
}
