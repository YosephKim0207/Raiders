using System;
using UnityEngine;

// Token: 0x0200175E RID: 5982
public class DFMaxScaler : MonoBehaviour
{
	// Token: 0x06008B3D RID: 35645 RVA: 0x0039F8E0 File Offset: 0x0039DAE0
	private void Start()
	{
		this.m_control = base.GetComponent<dfControl>();
		dfControl control = this.m_control;
		control.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(control.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.m_control_SizeChanged));
		this.m_control_SizeChanged(this.m_control, Vector3.zero, Vector3.zero);
	}

	// Token: 0x06008B3E RID: 35646 RVA: 0x0039F938 File Offset: 0x0039DB38
	private void m_control_SizeChanged(dfControl control, Vector3 pre, Vector3 post)
	{
		if (control is dfSprite)
		{
			dfSprite dfSprite = control as dfSprite;
			dfGUIManager manager = control.GetManager();
			Vector2 screenSize = manager.GetScreenSize();
			Vector2 sizeInPixels = dfSprite.SpriteInfo.sizeInPixels;
			float num = screenSize.x / screenSize.y;
			float num2 = sizeInPixels.x / sizeInPixels.y;
			dfSprite.Anchor = dfAnchorStyle.None;
			if (num > num2)
			{
				Vector2 vector = new Vector2(screenSize.y * num2, screenSize.y);
				if (dfSprite.Size != vector)
				{
					dfSprite.Size = vector;
					dfSprite.RelativePosition = new Vector3((screenSize.x - vector.x) / 2f, 0f, dfSprite.RelativePosition.z);
				}
			}
			else
			{
				Vector2 vector2 = new Vector2(screenSize.x, screenSize.x / num2);
				if (dfSprite.Size != vector2)
				{
					dfSprite.Size = vector2;
					dfSprite.RelativePosition = new Vector3(0f, (screenSize.y - vector2.y) / 2f, dfSprite.RelativePosition.z);
				}
			}
		}
	}

	// Token: 0x0400920F RID: 37391
	private dfControl m_control;
}
