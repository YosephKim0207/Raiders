using System;

// Token: 0x0200084D RID: 2125
public class BitArray2D
{
	// Token: 0x06002EB1 RID: 11953 RVA: 0x000F1C04 File Offset: 0x000EFE04
	public BitArray2D(bool useBackingFloats = false)
	{
		this.UsesBackingFloats = useBackingFloats;
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x000F1C20 File Offset: 0x000EFE20
	public int Width
	{
		get
		{
			return this.m_width;
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000F1C28 File Offset: 0x000EFE28
	public int Height
	{
		get
		{
			return this.m_height;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000F1C30 File Offset: 0x000EFE30
	public bool IsEmpty
	{
		get
		{
			return this.m_width == 0 && this.m_height == 0;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000F1C4C File Offset: 0x000EFE4C
	// (set) Token: 0x06002EB6 RID: 11958 RVA: 0x000F1C54 File Offset: 0x000EFE54
	public bool IsValid { get; set; }

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x000F1C60 File Offset: 0x000EFE60
	// (set) Token: 0x06002EB8 RID: 11960 RVA: 0x000F1C68 File Offset: 0x000EFE68
	public bool IsAabb { get; set; }

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000F1C74 File Offset: 0x000EFE74
	// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000F1C7C File Offset: 0x000EFE7C
	public bool UsesBackingFloats { get; set; }

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000F1C88 File Offset: 0x000EFE88
	// (set) Token: 0x06002EBC RID: 11964 RVA: 0x000F1C90 File Offset: 0x000EFE90
	public bool ReadOnly { get; set; }

	// Token: 0x06002EBD RID: 11965 RVA: 0x000F1C9C File Offset: 0x000EFE9C
	public void Reinitialize(int width, int height, bool fixedSize = false)
	{
		this.m_width = width;
		this.m_height = height;
		int num = this.m_width * this.m_height;
		if (this.m_bits == null || num > this.m_bits.Length)
		{
			if (!fixedSize)
			{
				num = (int)((float)num * this.c_sizeScalar);
			}
			this.m_bits = new bool[num];
		}
		if (this.UsesBackingFloats && (this.m_floats == null || num > this.m_floats.Length))
		{
			this.m_floats = new float[num];
		}
		this.IsValid = true;
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x000F1D34 File Offset: 0x000EFF34
	public void ReinitializeWithDefault(int width, int height, bool defaultValue, float defaultFloatValue = 0f, bool fixedSize = false)
	{
		this.m_width = width;
		this.m_height = height;
		int num = this.m_width * this.m_height;
		if (!defaultValue && (this.m_bits == null || num > this.m_bits.Length))
		{
			if (!fixedSize)
			{
				num = (int)((float)num * this.c_sizeScalar);
			}
			this.m_bits = new bool[num];
		}
		if (this.UsesBackingFloats && (this.m_floats == null || num > this.m_floats.Length))
		{
			this.m_floats = new float[num];
		}
		int num2 = this.m_width * this.m_height;
		if (!defaultValue)
		{
			Array.Clear(this.m_bits, 0, num2);
		}
		else
		{
			this.IsAabb = true;
		}
		if (this.UsesBackingFloats)
		{
			for (int i = 0; i < num2; i++)
			{
				this.m_floats[i] = defaultFloatValue;
			}
		}
		this.IsValid = true;
	}

	// Token: 0x170008B0 RID: 2224
	public bool this[int x, int y]
	{
		get
		{
			return this.IsAabb || this.m_bits[x + y * this.m_width];
		}
		set
		{
			this.m_bits[x + y * this.m_width] = value;
		}
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x000F1E60 File Offset: 0x000F0060
	public float GetFloat(int x, int y)
	{
		return this.m_floats[x + y * this.m_width];
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x000F1E74 File Offset: 0x000F0074
	public void SetFloat(int x, int y, float value)
	{
		this.m_floats[x + y * this.m_width] = value;
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x000F1E88 File Offset: 0x000F0088
	public void SetCircle(int x0, int y0, int radius, bool value, SetBackingFloatFunc floatFunc = null)
	{
		int num = radius;
		int i = 0;
		int num2 = 1 - num;
		while (i <= num)
		{
			this.SetColumn(num + x0, i + y0, -i + y0, value, floatFunc);
			this.SetColumn(i + x0, num + y0, -num + y0, value, floatFunc);
			this.SetColumn(-i + x0, num + y0, -num + y0, value, floatFunc);
			this.SetColumn(-num + x0, i + y0, -i + y0, value, floatFunc);
			i++;
			if (num2 <= 0)
			{
				num2 += 2 * i + 1;
			}
			else
			{
				num--;
				num2 += 2 * (i - num) + 1;
			}
		}
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x000F1F24 File Offset: 0x000F0124
	public void SetColumn(int x, int y0, int y1, bool value, SetBackingFloatFunc floatFunc = null)
	{
		if (y0 > y1)
		{
			BraveUtility.Swap<int>(ref y0, ref y1);
		}
		for (int i = y0; i <= y1; i++)
		{
			this.SetSafe(x, i, value, floatFunc);
		}
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x000F1F60 File Offset: 0x000F0160
	public void SetSafe(int x, int y, bool value, SetBackingFloatFunc floatFunc = null)
	{
		if (x >= 0 && x < this.m_width && y >= 0 && y < this.m_height)
		{
			int num = x + y * this.m_width;
			this.m_bits[num] = value;
			if (floatFunc != null)
			{
				this.m_floats[num] = floatFunc(x, y, value, this.m_floats[num]);
			}
		}
	}

	// Token: 0x04001F85 RID: 8069
	private int m_width;

	// Token: 0x04001F86 RID: 8070
	private int m_height;

	// Token: 0x04001F87 RID: 8071
	private bool[] m_bits;

	// Token: 0x04001F88 RID: 8072
	private float[] m_floats;

	// Token: 0x04001F89 RID: 8073
	private float c_sizeScalar = 2f;
}
