using System;
using UnityEngine;

// Token: 0x02001799 RID: 6041
public class GameUIUtility
{
	// Token: 0x06008D6C RID: 36204 RVA: 0x003B8134 File Offset: 0x003B6334
	public static float GetCurrentTK2D_DFScale(dfGUIManager manager)
	{
		return Pixelator.Instance.CurrentTileScale * 16f * manager.PixelsToUnits();
	}

	// Token: 0x06008D6D RID: 36205 RVA: 0x003B8150 File Offset: 0x003B6350
	public static Vector2 TK2DtoDF(Vector2 input, float p2u)
	{
		float num = 64f * p2u;
		return input * num;
	}

	// Token: 0x06008D6E RID: 36206 RVA: 0x003B816C File Offset: 0x003B636C
	public static Vector2 DFtoTK2D(Vector2 input, float p2u)
	{
		float num = 64f * p2u;
		return input / num;
	}

	// Token: 0x06008D6F RID: 36207 RVA: 0x003B8188 File Offset: 0x003B6388
	public static Vector2 TK2DtoDF(Vector2 input)
	{
		return input * Pixelator.Instance.ScaleTileScale * 16f * GameUIRoot.Instance.PixelsToUnits();
	}

	// Token: 0x06008D70 RID: 36208 RVA: 0x003B81B4 File Offset: 0x003B63B4
	public static Vector2 QuantizeUIPosition(Vector2 input)
	{
		float currentTileScale = Pixelator.Instance.CurrentTileScale;
		int num = Mathf.RoundToInt(input.x / currentTileScale * currentTileScale);
		int num2 = Mathf.RoundToInt(input.y / currentTileScale * currentTileScale);
		return new Vector2((float)num, (float)num2);
	}
}
