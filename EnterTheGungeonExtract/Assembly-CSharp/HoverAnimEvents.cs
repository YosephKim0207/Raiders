using System;
using UnityEngine;

// Token: 0x0200047A RID: 1146
[AddComponentMenu("Daikon Forge/Examples/Sprites/Hover Animation Events")]
public class HoverAnimEvents : MonoBehaviour
{
	// Token: 0x06001A53 RID: 6739 RVA: 0x0007A9E8 File Offset: 0x00078BE8
	public void OnMouseEnter(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.hoverAnimation.PlayForward();
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x0007A9F8 File Offset: 0x00078BF8
	public void OnMouseLeave(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.hoverAnimation.PlayReverse();
	}

	// Token: 0x040014A0 RID: 5280
	public dfSpriteAnimation hoverAnimation;
}
