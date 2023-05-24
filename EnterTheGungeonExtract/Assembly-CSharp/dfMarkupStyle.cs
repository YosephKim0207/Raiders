using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public struct dfMarkupStyle
{
	// Token: 0x06001BA0 RID: 7072 RVA: 0x000827C0 File Offset: 0x000809C0
	public dfMarkupStyle(dfDynamicFont Font, int FontSize, FontStyle FontStyle)
	{
		this.Host = null;
		this.Atlas = null;
		this.Version = 0;
		this.Font = Font;
		this.FontSize = FontSize;
		this.FontStyle = FontStyle;
		this.Align = dfMarkupTextAlign.Left;
		this.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		this.Color = Color.white;
		this.BackgroundColor = Color.clear;
		this.TextDecoration = dfMarkupTextDecoration.None;
		this.PreserveWhitespace = false;
		this.Preformatted = false;
		this.WordSpacing = 0;
		this.CharacterSpacing = 0;
		this.lineHeight = 0;
		this.Opacity = 1f;
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00082850 File Offset: 0x00080A50
	// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x0008287C File Offset: 0x00080A7C
	public int LineHeight
	{
		get
		{
			if (this.lineHeight == 0)
			{
				return Mathf.CeilToInt((float)this.FontSize);
			}
			return Mathf.Max(this.FontSize, this.lineHeight);
		}
		set
		{
			this.lineHeight = value;
		}
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00082888 File Offset: 0x00080A88
	public static dfMarkupTextDecoration ParseTextDecoration(string value)
	{
		if (value == "underline")
		{
			return dfMarkupTextDecoration.Underline;
		}
		if (value == "overline")
		{
			return dfMarkupTextDecoration.Overline;
		}
		if (value == "line-through")
		{
			return dfMarkupTextDecoration.LineThrough;
		}
		return dfMarkupTextDecoration.None;
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x000828C4 File Offset: 0x00080AC4
	public static dfMarkupVerticalAlign ParseVerticalAlignment(string value)
	{
		if (value == "top")
		{
			return dfMarkupVerticalAlign.Top;
		}
		if (value == "center" || value == "middle")
		{
			return dfMarkupVerticalAlign.Middle;
		}
		if (value == "bottom")
		{
			return dfMarkupVerticalAlign.Bottom;
		}
		return dfMarkupVerticalAlign.Baseline;
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00082918 File Offset: 0x00080B18
	public static dfMarkupTextAlign ParseTextAlignment(string value)
	{
		if (value == "right")
		{
			return dfMarkupTextAlign.Right;
		}
		if (value == "center")
		{
			return dfMarkupTextAlign.Center;
		}
		if (value == "justify")
		{
			return dfMarkupTextAlign.Justify;
		}
		return dfMarkupTextAlign.Left;
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00082954 File Offset: 0x00080B54
	public static FontStyle ParseFontStyle(string value, FontStyle baseStyle)
	{
		if (value == "normal")
		{
			return FontStyle.Normal;
		}
		if (value == "bold")
		{
			if (baseStyle == FontStyle.Normal)
			{
				return FontStyle.Bold;
			}
			if (baseStyle == FontStyle.Italic)
			{
				return FontStyle.BoldAndItalic;
			}
		}
		else if (value == "italic")
		{
			if (baseStyle == FontStyle.Normal)
			{
				return FontStyle.Italic;
			}
			if (baseStyle == FontStyle.Bold)
			{
				return FontStyle.BoldAndItalic;
			}
		}
		return baseStyle;
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x000829BC File Offset: 0x00080BBC
	public static int ParseSize(string value, int baseValue)
	{
		int num;
		if (value.Length > 1 && value.EndsWith("%") && int.TryParse(value.TrimEnd(new char[] { '%' }), out num))
		{
			return (int)((float)baseValue * ((float)num / 100f));
		}
		if (value.EndsWith("px"))
		{
			value = value.Substring(0, value.Length - 2);
		}
		int num2;
		if (int.TryParse(value, out num2))
		{
			return num2;
		}
		return baseValue;
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x00082A40 File Offset: 0x00080C40
	public static Color ParseColor(string color, Color defaultColor)
	{
		Color color2 = defaultColor;
		Color color3;
		if (color.StartsWith("#"))
		{
			uint num = 0U;
			if (uint.TryParse(color.Substring(1), NumberStyles.HexNumber, null, out num))
			{
				color2 = dfMarkupStyle.UIntToColor(num);
			}
			else
			{
				color2 = Color.red;
			}
		}
		else if (dfMarkupStyle.namedColors.TryGetValue(color.ToLowerInvariant(), out color3))
		{
			color2 = color3;
		}
		return color2;
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x00082AB0 File Offset: 0x00080CB0
	private static Color32 UIntToColor(uint color)
	{
		byte b = (byte)(color >> 16);
		byte b2 = (byte)(color >> 8);
		byte b3 = (byte)(color >> 0);
		return new Color32(b, b2, b3, byte.MaxValue);
	}

	// Token: 0x040015A6 RID: 5542
	private static Dictionary<string, Color> namedColors = new Dictionary<string, Color>
	{
		{
			"aqua",
			dfMarkupStyle.UIntToColor(65535U)
		},
		{
			"black",
			Color.black
		},
		{
			"blue",
			Color.blue
		},
		{
			"cyan",
			Color.cyan
		},
		{
			"fuchsia",
			dfMarkupStyle.UIntToColor(16711935U)
		},
		{
			"gray",
			Color.gray
		},
		{
			"green",
			Color.green
		},
		{
			"lime",
			dfMarkupStyle.UIntToColor(65280U)
		},
		{
			"magenta",
			Color.magenta
		},
		{
			"maroon",
			dfMarkupStyle.UIntToColor(8388608U)
		},
		{
			"navy",
			dfMarkupStyle.UIntToColor(128U)
		},
		{
			"olive",
			dfMarkupStyle.UIntToColor(8421376U)
		},
		{
			"orange",
			dfMarkupStyle.UIntToColor(16753920U)
		},
		{
			"purple",
			dfMarkupStyle.UIntToColor(8388736U)
		},
		{
			"red",
			Color.red
		},
		{
			"silver",
			dfMarkupStyle.UIntToColor(12632256U)
		},
		{
			"teal",
			dfMarkupStyle.UIntToColor(32896U)
		},
		{
			"white",
			Color.white
		},
		{
			"yellow",
			Color.yellow
		}
	};

	// Token: 0x040015A7 RID: 5543
	internal int Version;

	// Token: 0x040015A8 RID: 5544
	public dfRichTextLabel Host;

	// Token: 0x040015A9 RID: 5545
	public dfAtlas Atlas;

	// Token: 0x040015AA RID: 5546
	public dfDynamicFont Font;

	// Token: 0x040015AB RID: 5547
	public int FontSize;

	// Token: 0x040015AC RID: 5548
	public FontStyle FontStyle;

	// Token: 0x040015AD RID: 5549
	public dfMarkupTextDecoration TextDecoration;

	// Token: 0x040015AE RID: 5550
	public dfMarkupTextAlign Align;

	// Token: 0x040015AF RID: 5551
	public dfMarkupVerticalAlign VerticalAlign;

	// Token: 0x040015B0 RID: 5552
	public Color Color;

	// Token: 0x040015B1 RID: 5553
	public Color BackgroundColor;

	// Token: 0x040015B2 RID: 5554
	public float Opacity;

	// Token: 0x040015B3 RID: 5555
	public bool PreserveWhitespace;

	// Token: 0x040015B4 RID: 5556
	public bool Preformatted;

	// Token: 0x040015B5 RID: 5557
	public int WordSpacing;

	// Token: 0x040015B6 RID: 5558
	public int CharacterSpacing;

	// Token: 0x040015B7 RID: 5559
	private int lineHeight;
}
