using System;
using UnityEngine;

// Token: 0x020013BC RID: 5052
public class GunWeaponPanelSpriteOverride : MonoBehaviour
{
	// Token: 0x06007289 RID: 29321 RVA: 0x002D8A00 File Offset: 0x002D6C00
	public int GetMatch(int input)
	{
		for (int i = 0; i < this.spritePairs.Length; i++)
		{
			if (this.spritePairs[i].x == input)
			{
				return this.spritePairs[i].y;
			}
		}
		return input;
	}

	// Token: 0x040073E0 RID: 29664
	public IntVector2[] spritePairs;
}
