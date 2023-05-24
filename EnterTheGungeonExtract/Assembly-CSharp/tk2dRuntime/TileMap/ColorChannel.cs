using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF4 RID: 3060
	[Serializable]
	public class ColorChannel
	{
		// Token: 0x0600410C RID: 16652 RVA: 0x0014F060 File Offset: 0x0014D260
		public ColorChannel(int width, int height, int divX, int divY)
		{
			this.Init(width, height, divX, divY);
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x0014F080 File Offset: 0x0014D280
		public ColorChannel()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0014F0A0 File Offset: 0x0014D2A0
		public void Init(int width, int height, int divX, int divY)
		{
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.chunks = new ColorChunk[0];
			this.divX = divX;
			this.divY = divY;
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0014F0DC File Offset: 0x0014D2DC
		public ColorChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			num = Mathf.Clamp(num, 0, this.numColumns - 1);
			num2 = Mathf.Clamp(num2, 0, this.numRows - 1);
			int num3 = num2 * this.numColumns + num;
			ColorChunk colorChunk = this.chunks[num3];
			int num4 = x - num * this.divX;
			int num5 = y - num2 * this.divY;
			offset = num5 * (this.divX + 1) + num4;
			return colorChunk;
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0014F158 File Offset: 0x0014D358
		public Color GetColor(int x, int y)
		{
			if (this.IsEmpty)
			{
				return this.clearColor;
			}
			int num;
			ColorChunk colorChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (colorChunk.colors.Length == 0)
			{
				return this.clearColor;
			}
			return colorChunk.colors[num];
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x0014F1AC File Offset: 0x0014D3AC
		private void InitChunk(ColorChunk chunk)
		{
			if (chunk.colors.Length == 0)
			{
				chunk.colors = new Color32[(this.divX + 1) * (this.divY + 1)];
				for (int i = 0; i < chunk.colors.Length; i++)
				{
					chunk.colors[i] = this.clearColor;
				}
				chunk.colorOverrides = new Color32[(this.divX + 1) * (this.divY + 1), 4];
				for (int j = 0; j < chunk.colorOverrides.GetLength(0); j++)
				{
					chunk.colorOverrides[j, 0] = this.clearColor;
					chunk.colorOverrides[j, 1] = this.clearColor;
					chunk.colorOverrides[j, 2] = this.clearColor;
					chunk.colorOverrides[j, 3] = this.clearColor;
				}
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x0014F2C8 File Offset: 0x0014D4C8
		public void SetTileColorOverride(int x, int y, Color32 color)
		{
			if (this.IsEmpty)
			{
				this.Create();
			}
			int num = x / this.divX;
			int num2 = y / this.divY;
			ColorChunk chunk = this.GetChunk(num, num2, true);
			int num3 = x - num * this.divX;
			int num4 = y - num2 * this.divY;
			int num5 = this.divX + 1;
			chunk.colorOverrides[num4 * num5 + num3, 0] = color;
			chunk.colorOverrides[num4 * num5 + num3, 1] = color;
			chunk.colorOverrides[num4 * num5 + num3, 2] = color;
			chunk.colorOverrides[num4 * num5 + num3, 3] = color;
			chunk.Dirty = true;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0014F390 File Offset: 0x0014D590
		public void SetTileColorGradient(int x, int y, Color32 bottomLeft, Color32 bottomRight, Color32 topLeft, Color32 topRight)
		{
			if (this.IsEmpty)
			{
				this.Create();
			}
			int num = this.divX + 1;
			int num2 = x / this.divX;
			int num3 = y / this.divY;
			ColorChunk chunk = this.GetChunk(num2, num3, true);
			int num4 = x - num2 * this.divX;
			int num5 = y - num3 * this.divY;
			chunk.colorOverrides[num5 * num + num4, 0] = bottomLeft;
			chunk.colorOverrides[num5 * num + num4, 1] = bottomRight;
			chunk.colorOverrides[num5 * num + num4, 2] = topLeft;
			chunk.colorOverrides[num5 * num + num4, 3] = topRight;
			chunk.Dirty = true;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0014F45C File Offset: 0x0014D65C
		public void SetColor(int x, int y, Color color)
		{
			if (this.IsEmpty)
			{
				this.Create();
			}
			int num = this.divX + 1;
			int num2 = Mathf.Max(x - 1, 0) / this.divX;
			int num3 = Mathf.Max(y - 1, 0) / this.divY;
			ColorChunk colorChunk = this.GetChunk(num2, num3, true);
			int num4 = x - num2 * this.divX;
			int num5 = y - num3 * this.divY;
			colorChunk.colors[num5 * num + num4] = color;
			colorChunk.Dirty = true;
			bool flag = false;
			bool flag2 = false;
			if (x != 0 && x % this.divX == 0 && num2 + 1 < this.numColumns)
			{
				flag = true;
			}
			if (y != 0 && y % this.divY == 0 && num3 + 1 < this.numRows)
			{
				flag2 = true;
			}
			if (flag)
			{
				int num6 = num2 + 1;
				colorChunk = this.GetChunk(num6, num3, true);
				num4 = x - num6 * this.divX;
				num5 = y - num3 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
			if (flag2)
			{
				int num7 = num3 + 1;
				colorChunk = this.GetChunk(num2, num7, true);
				num4 = x - num2 * this.divX;
				num5 = y - num7 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
			if (flag && flag2)
			{
				int num8 = num2 + 1;
				int num9 = num3 + 1;
				colorChunk = this.GetChunk(num8, num9, true);
				num4 = x - num8 * this.divX;
				num5 = y - num9 * this.divY;
				colorChunk.colors[num5 * num + num4] = color;
				colorChunk.Dirty = true;
			}
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0014F644 File Offset: 0x0014D844
		public ColorChunk GetChunk(int x, int y)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			return this.chunks[y * this.numColumns + x];
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x0014F674 File Offset: 0x0014D874
		public ColorChunk GetChunk(int x, int y, bool init)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			ColorChunk colorChunk = this.chunks[y * this.numColumns + x];
			this.InitChunk(colorChunk);
			return colorChunk;
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0014F6B8 File Offset: 0x0014D8B8
		public void ClearChunk(ColorChunk chunk)
		{
			for (int i = 0; i < chunk.colors.Length; i++)
			{
				chunk.colors[i] = this.clearColor;
			}
			for (int j = 0; j < chunk.colorOverrides.GetLength(0); j++)
			{
				chunk.colorOverrides[j, 0] = this.clearColor;
				chunk.colorOverrides[j, 1] = this.clearColor;
				chunk.colorOverrides[j, 2] = this.clearColor;
				chunk.colorOverrides[j, 3] = this.clearColor;
			}
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0014F78C File Offset: 0x0014D98C
		public void ClearDirtyFlag()
		{
			foreach (ColorChunk colorChunk in this.chunks)
			{
				colorChunk.Dirty = false;
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0014F7C0 File Offset: 0x0014D9C0
		public void Clear(Color color)
		{
			this.clearColor = color;
			foreach (ColorChunk colorChunk in this.chunks)
			{
				this.ClearChunk(colorChunk);
			}
			this.Optimize();
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x0014F800 File Offset: 0x0014DA00
		public void Delete()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0014F810 File Offset: 0x0014DA10
		public void Create()
		{
			this.chunks = new ColorChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.chunks.Length; i++)
			{
				this.chunks[i] = new ColorChunk();
			}
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x0014F85C File Offset: 0x0014DA5C
		private void Optimize(ColorChunk chunk)
		{
			bool flag = true;
			Color32 color = this.clearColor;
			foreach (Color32 color2 in chunk.colors)
			{
				if (color2.r != color.r || color2.g != color.g || color2.b != color.b || color2.a != color.a)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Color32[,] colorOverrides = chunk.colorOverrides;
				int length = colorOverrides.GetLength(0);
				int length2 = colorOverrides.GetLength(1);
				for (int j = 0; j < length; j++)
				{
					for (int k = 0; k < length2; k++)
					{
						Color32 color3 = colorOverrides[j, k];
						if (color3.r != color.r || color3.g != color.g || color3.b != color.b || color3.a != color.a)
						{
							flag = false;
							goto IL_13E;
						}
					}
				}
			}
			IL_13E:
			if (flag)
			{
				chunk.colors = new Color32[0];
				chunk.colorOverrides = new Color32[0, 0];
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x0014F9C8 File Offset: 0x0014DBC8
		public void Optimize()
		{
			foreach (ColorChunk colorChunk in this.chunks)
			{
				this.Optimize(colorChunk);
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x0014F9FC File Offset: 0x0014DBFC
		public bool IsEmpty
		{
			get
			{
				return this.chunks.Length == 0;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600411F RID: 16671 RVA: 0x0014FA0C File Offset: 0x0014DC0C
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (ColorChunk colorChunk in this.chunks)
				{
					if (colorChunk != null && colorChunk.colors != null && colorChunk.colors.Length > 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x040033CF RID: 13263
		public Color clearColor = Color.white;

		// Token: 0x040033D0 RID: 13264
		public ColorChunk[] chunks;

		// Token: 0x040033D1 RID: 13265
		public int numColumns;

		// Token: 0x040033D2 RID: 13266
		public int numRows;

		// Token: 0x040033D3 RID: 13267
		public int divX;

		// Token: 0x040033D4 RID: 13268
		public int divY;
	}
}
