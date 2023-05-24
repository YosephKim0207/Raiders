using System;
using UnityEngine;

// Token: 0x02000461 RID: 1121
[AddComponentMenu("Daikon Forge/Examples/General/Control Navigation")]
public class ControlNavigation : MonoBehaviour
{
	// Token: 0x060019EF RID: 6639 RVA: 0x00078F00 File Offset: 0x00077100
	private void OnMouseEnter(dfControl sender, dfMouseEventArgs args)
	{
		if (this.FocusOnMouseEnter)
		{
			dfControl component = base.GetComponent<dfControl>();
			if (component != null)
			{
				component.Focus(true);
			}
		}
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00078F34 File Offset: 0x00077134
	private void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		if (this.SelectOnClick != null)
		{
			this.SelectOnClick.Focus(true);
		}
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00078F54 File Offset: 0x00077154
	private void OnKeyDown(dfControl sender, dfKeyEventArgs args)
	{
		KeyCode keyCode = args.KeyCode;
		switch (keyCode)
		{
		case KeyCode.UpArrow:
			if (this.SelectOnUp != null)
			{
				this.SelectOnUp.Focus(true);
				args.Use();
			}
			break;
		case KeyCode.DownArrow:
			if (this.SelectOnDown != null)
			{
				this.SelectOnDown.Focus(true);
				args.Use();
			}
			break;
		case KeyCode.RightArrow:
			if (this.SelectOnRight != null)
			{
				this.SelectOnRight.Focus(true);
				args.Use();
			}
			break;
		case KeyCode.LeftArrow:
			if (this.SelectOnLeft != null)
			{
				this.SelectOnLeft.Focus(true);
				args.Use();
			}
			break;
		default:
			if (keyCode == KeyCode.Tab)
			{
				if (args.Shift)
				{
					if (this.SelectOnShiftTab != null)
					{
						this.SelectOnShiftTab.Focus(true);
						args.Use();
					}
				}
				else if (this.SelectOnTab != null)
				{
					this.SelectOnTab.Focus(true);
					args.Use();
				}
			}
			break;
		}
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x00079094 File Offset: 0x00077294
	private void Awake()
	{
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x00079098 File Offset: 0x00077298
	private void OnEnable()
	{
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x0007909C File Offset: 0x0007729C
	private void Start()
	{
		if (this.FocusOnStart)
		{
			dfControl component = base.GetComponent<dfControl>();
			if (component != null)
			{
				component.Focus(true);
			}
		}
	}

	// Token: 0x0400144D RID: 5197
	public bool FocusOnStart;

	// Token: 0x0400144E RID: 5198
	public bool FocusOnMouseEnter;

	// Token: 0x0400144F RID: 5199
	public dfControl SelectOnLeft;

	// Token: 0x04001450 RID: 5200
	public dfControl SelectOnRight;

	// Token: 0x04001451 RID: 5201
	public dfControl SelectOnUp;

	// Token: 0x04001452 RID: 5202
	public dfControl SelectOnDown;

	// Token: 0x04001453 RID: 5203
	public dfControl SelectOnTab;

	// Token: 0x04001454 RID: 5204
	public dfControl SelectOnShiftTab;

	// Token: 0x04001455 RID: 5205
	public dfControl SelectOnClick;
}
