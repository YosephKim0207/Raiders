using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
[Serializable]
public struct HSBColor
{
	// Token: 0x060018F8 RID: 6392 RVA: 0x00075944 File Offset: 0x00073B44
	public HSBColor(float h, float s, float b, float a)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = a;
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x00075964 File Offset: 0x00073B64
	public HSBColor(float h, float s, float b)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = 1f;
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x00075988 File Offset: 0x00073B88
	public HSBColor(Color col)
	{
		HSBColor hsbcolor = HSBColor.FromColor(col);
		this.h = hsbcolor.h;
		this.s = hsbcolor.s;
		this.b = hsbcolor.b;
		this.a = hsbcolor.a;
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x000759D0 File Offset: 0x00073BD0
	public static Color GetHue(Color color)
	{
		HSBColor hsbcolor = HSBColor.FromColor(color);
		hsbcolor.s = (hsbcolor.b = 1f);
		return hsbcolor;
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x00075A00 File Offset: 0x00073C00
	public static HSBColor FromColor(Color color)
	{
		HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
		HSBColor.RGBToHSV(color, out hsbcolor.h, out hsbcolor.s, out hsbcolor.b);
		return hsbcolor;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x00075A48 File Offset: 0x00073C48
	public static Color ToColor(HSBColor hsbColor)
	{
		float num = hsbColor.b;
		float num2 = hsbColor.b;
		float num3 = hsbColor.b;
		if (hsbColor.s != 0f)
		{
			float num4 = hsbColor.b;
			float num5 = hsbColor.b * hsbColor.s;
			float num6 = hsbColor.b - num5;
			float num7 = hsbColor.h * 360f;
			if (num7 < 60f)
			{
				num = num4;
				num2 = num7 * num5 / 60f + num6;
				num3 = num6;
			}
			else if (num7 < 120f)
			{
				num = -(num7 - 120f) * num5 / 60f + num6;
				num2 = num4;
				num3 = num6;
			}
			else if (num7 < 180f)
			{
				num = num6;
				num2 = num4;
				num3 = (num7 - 120f) * num5 / 60f + num6;
			}
			else if (num7 < 240f)
			{
				num = num6;
				num2 = -(num7 - 240f) * num5 / 60f + num6;
				num3 = num4;
			}
			else if (num7 < 300f)
			{
				num = (num7 - 240f) * num5 / 60f + num6;
				num2 = num6;
				num3 = num4;
			}
			else if (num7 <= 360f)
			{
				num = num4;
				num2 = num6;
				num3 = -(num7 - 360f) * num5 / 60f + num6;
			}
			else
			{
				num = 0f;
				num2 = 0f;
				num3 = 0f;
			}
		}
		return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x00075BE4 File Offset: 0x00073DE4
	public Color ToColor()
	{
		return HSBColor.ToColor(this);
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x00075BF4 File Offset: 0x00073DF4
	public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
	{
		float num;
		float num2;
		if (a.b == 0f)
		{
			num = b.h;
			num2 = b.s;
		}
		else if (b.b == 0f)
		{
			num = a.h;
			num2 = a.s;
		}
		else
		{
			if (a.s == 0f)
			{
				num = b.h;
			}
			else if (b.s == 0f)
			{
				num = a.h;
			}
			else
			{
				float num3;
				for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
				{
				}
				while (num3 > 360f)
				{
					num3 -= 360f;
				}
				num = num3 / 360f;
			}
			num2 = Mathf.Lerp(a.s, b.s, t);
		}
		return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x00075D28 File Offset: 0x00073F28
	private static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
	{
		if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
		{
			HSBColor.RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
		}
		else if (rgbColor.g > rgbColor.r)
		{
			HSBColor.RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
		}
		else
		{
			HSBColor.RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x00075DE0 File Offset: 0x00073FE0
	private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
	{
		V = dominantcolor;
		if (V != 0f)
		{
			float num;
			if (colorone > colortwo)
			{
				num = colortwo;
			}
			else
			{
				num = colorone;
			}
			float num2 = V - num;
			if (num2 != 0f)
			{
				S = num2 / V;
				H = offset + (colorone - colortwo) / num2;
			}
			else
			{
				S = 0f;
				H = offset + (colorone - colortwo);
			}
			H /= 6f;
			if (H < 0f)
			{
				H += 1f;
			}
		}
		else
		{
			S = 0f;
			H = 0f;
		}
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x00075E80 File Offset: 0x00074080
	public static implicit operator HSBColor(Color color)
	{
		return HSBColor.FromColor(color);
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x00075E88 File Offset: 0x00074088
	public static implicit operator HSBColor(Color32 color)
	{
		return HSBColor.FromColor(color);
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x00075E98 File Offset: 0x00074098
	public static implicit operator Color(HSBColor hsb)
	{
		return hsb.ToColor();
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00075EA4 File Offset: 0x000740A4
	public static implicit operator Color32(HSBColor hsb)
	{
		return hsb.ToColor();
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00075EB4 File Offset: 0x000740B4
	public override string ToString()
	{
		return string.Format("H:{0}, S:{1}, B:{2}", this.h, this.s, this.b);
	}

	// Token: 0x040013BA RID: 5050
	public float h;

	// Token: 0x040013BB RID: 5051
	public float s;

	// Token: 0x040013BC RID: 5052
	public float b;

	// Token: 0x040013BD RID: 5053
	public float a;
}
