using System;
using System.Text.RegularExpressions;

// Token: 0x020004A3 RID: 1187
public struct dfMarkupBorders
{
	// Token: 0x06001B9B RID: 7067 RVA: 0x000825BC File Offset: 0x000807BC
	public dfMarkupBorders(int left, int right, int top, int bottom)
	{
		this.left = left;
		this.top = top;
		this.right = right;
		this.bottom = bottom;
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000825DC File Offset: 0x000807DC
	public int horizontal
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06001B9D RID: 7069 RVA: 0x000825EC File Offset: 0x000807EC
	public int vertical
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000825FC File Offset: 0x000807FC
	public static dfMarkupBorders Parse(string value)
	{
		dfMarkupBorders dfMarkupBorders = default(dfMarkupBorders);
		value = Regex.Replace(value, "\\s+", " ");
		string[] array = value.Split(new char[] { ' ' });
		if (array.Length == 1)
		{
			int num = dfMarkupStyle.ParseSize(value, 0);
			dfMarkupBorders.left = (dfMarkupBorders.right = num);
			dfMarkupBorders.top = (dfMarkupBorders.bottom = num);
		}
		else if (array.Length == 2)
		{
			int num2 = dfMarkupStyle.ParseSize(array[0], 0);
			dfMarkupBorders.top = (dfMarkupBorders.bottom = num2);
			int num3 = dfMarkupStyle.ParseSize(array[1], 0);
			dfMarkupBorders.left = (dfMarkupBorders.right = num3);
		}
		else if (array.Length == 3)
		{
			int num4 = dfMarkupStyle.ParseSize(array[0], 0);
			dfMarkupBorders.top = num4;
			int num5 = dfMarkupStyle.ParseSize(array[1], 0);
			dfMarkupBorders.left = (dfMarkupBorders.right = num5);
			int num6 = dfMarkupStyle.ParseSize(array[2], 0);
			dfMarkupBorders.bottom = num6;
		}
		else if (array.Length == 4)
		{
			int num7 = dfMarkupStyle.ParseSize(array[0], 0);
			dfMarkupBorders.top = num7;
			int num8 = dfMarkupStyle.ParseSize(array[1], 0);
			dfMarkupBorders.right = num8;
			int num9 = dfMarkupStyle.ParseSize(array[2], 0);
			dfMarkupBorders.bottom = num9;
			int num10 = dfMarkupStyle.ParseSize(array[3], 0);
			dfMarkupBorders.left = num10;
		}
		return dfMarkupBorders;
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x00082768 File Offset: 0x00080968
	public override string ToString()
	{
		return string.Format("[T:{0},R:{1},L:{2},B:{3}]", new object[] { this.top, this.right, this.left, this.bottom });
	}

	// Token: 0x04001592 RID: 5522
	public int left;

	// Token: 0x04001593 RID: 5523
	public int top;

	// Token: 0x04001594 RID: 5524
	public int right;

	// Token: 0x04001595 RID: 5525
	public int bottom;
}
