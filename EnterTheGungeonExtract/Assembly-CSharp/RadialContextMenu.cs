using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
[AddComponentMenu("Daikon Forge/Examples/Menus/Radial Context Menu Helper")]
public class RadialContextMenu : MonoBehaviour
{
	// Token: 0x060019D8 RID: 6616 RVA: 0x000789F4 File Offset: 0x00076BF4
	public void Start()
	{
		this.contextMenu.MenuClosed += delegate(dfRadialMenu menu)
		{
			menu.host.Hide();
		};
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00078A20 File Offset: 0x00076C20
	public void OnMouseDown(dfControl control, dfMouseEventArgs args)
	{
		if (!args.Used && args.Buttons == dfMouseButtons.Middle)
		{
			if (this.contextMenu.IsOpen)
			{
				this.contextMenu.Close();
				return;
			}
			args.Use();
			Vector2 hitPosition = control.GetHitPosition(args);
			dfControl host = this.contextMenu.host;
			host.RelativePosition = hitPosition - host.Size * 0.5f;
			host.BringToFront();
			host.Show();
			host.Focus(true);
			this.contextMenu.Open();
		}
	}

	// Token: 0x04001433 RID: 5171
	public dfRadialMenu contextMenu;
}
