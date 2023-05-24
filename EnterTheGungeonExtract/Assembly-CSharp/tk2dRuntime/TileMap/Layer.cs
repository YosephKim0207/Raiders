using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF5 RID: 3061
	[Serializable]
	public class Layer
	{
		// Token: 0x06004120 RID: 16672 RVA: 0x0014FA60 File Offset: 0x0014DC60
		public Layer(int hash, int width, int height, int divX, int divY)
		{
			this.spriteChannel = new SpriteChannel();
			this.Init(hash, width, height, divX, divY);
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0014FA80 File Offset: 0x0014DC80
		public void Init(int hash, int width, int height, int divX, int divY)
		{
			this.divX = divX;
			this.divY = divY;
			this.hash = hash;
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.width = width;
			this.height = height;
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.numRows; i++)
			{
				for (int j = 0; j < this.numColumns; j++)
				{
					int num = j * divX;
					int num2 = (j + 1) * divX;
					int num3 = i * divY;
					int num4 = (i + 1) * divY;
					this.spriteChannel.chunks[i * this.numColumns + j] = new SpriteChunk(num, num3, num2, num4);
				}
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06004122 RID: 16674 RVA: 0x0014FB58 File Offset: 0x0014DD58
		public bool IsEmpty
		{
			get
			{
				return this.spriteChannel.chunks.Length == 0;
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0014FB6C File Offset: 0x0014DD6C
		public void Create()
		{
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0014FB8C File Offset: 0x0014DD8C
		public int[] GetChunkData(int x, int y)
		{
			return this.GetChunk(x, y).spriteIds;
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0014FB9C File Offset: 0x0014DD9C
		public SpriteChunk GetChunk(int x, int y)
		{
			return this.spriteChannel.chunks[y * this.numColumns + x];
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x0014FBB4 File Offset: 0x0014DDB4
		private SpriteChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			SpriteChunk spriteChunk = this.spriteChannel.chunks[num2 * this.numColumns + num];
			int num3 = x - num * this.divX;
			int num4 = y - num2 * this.divY;
			offset = num4 * this.divX + num3;
			return spriteChunk;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0014FC10 File Offset: 0x0014DE10
		private bool GetRawTileValue(int x, int y, ref int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk.spriteIds == null || spriteChunk.spriteIds.Length == 0)
			{
				return false;
			}
			value = spriteChunk.spriteIds[num];
			return true;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0014FC50 File Offset: 0x0014DE50
		private void SetRawTileValue(int x, int y, int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk != null)
			{
				this.CreateChunk(spriteChunk);
				spriteChunk.spriteIds[num] = value;
				spriteChunk.Dirty = true;
			}
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0014FC88 File Offset: 0x0014DE88
		public void DestroyGameData(tk2dTileMap tilemap)
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				if (spriteChunk.HasGameData)
				{
					spriteChunk.DestroyColliderData(tilemap);
					spriteChunk.DestroyGameData(tilemap);
				}
			}
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0014FCD4 File Offset: 0x0014DED4
		public int GetTile(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return num & 16777215;
			}
			return -1;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0014FD04 File Offset: 0x0014DF04
		public tk2dTileFlags GetTileFlags(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return (tk2dTileFlags)(num & -16777216);
			}
			return tk2dTileFlags.None;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0014FD34 File Offset: 0x0014DF34
		public int GetRawTile(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0014FD58 File Offset: 0x0014DF58
		public void SetTile(int x, int y, int tile)
		{
			tk2dTileFlags tileFlags = this.GetTileFlags(x, y);
			int num = ((tile != -1) ? (tile | (int)tileFlags) : (-1));
			this.SetRawTileValue(x, y, num);
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x0014FD88 File Offset: 0x0014DF88
		public void SetTileFlags(int x, int y, tk2dTileFlags flags)
		{
			int tile = this.GetTile(x, y);
			if (tile != -1)
			{
				int num = tile | (int)flags;
				this.SetRawTileValue(x, y, num);
			}
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0014FDB4 File Offset: 0x0014DFB4
		public void ClearTile(int x, int y)
		{
			this.SetTile(x, y, -1);
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0014FDC0 File Offset: 0x0014DFC0
		public void SetRawTile(int x, int y, int rawTile)
		{
			this.SetRawTileValue(x, y, rawTile);
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0014FDCC File Offset: 0x0014DFCC
		public void CreateOverrideChunk(SpriteChunk chunk)
		{
			if (chunk.spriteIds == null || chunk.spriteIds.Length == 0)
			{
				chunk.spriteIds = new int[chunk.Width * chunk.Height];
			}
			int num = 0;
			for (int i = 0; i < chunk.Width; i++)
			{
				for (int j = 0; j < chunk.Height; j++)
				{
					IntVector2 intVector = new IntVector2(chunk.startX + i, chunk.startY + j);
					int num2 = 0;
					SpriteChunk spriteChunk = this.FindChunkAndCoordinate(intVector.x, intVector.y, out num2);
					if (num2 >= 0 && num2 < spriteChunk.spriteIds.Length)
					{
						chunk.spriteIds[j * chunk.Width + i] = spriteChunk.spriteIds[num2];
						num++;
					}
					else
					{
						chunk.spriteIds[j * chunk.Width + i] = -1;
					}
				}
			}
			if (num == 0)
			{
				chunk.spriteIds = new int[0];
			}
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0014FECC File Offset: 0x0014E0CC
		private void CreateChunk(SpriteChunk chunk)
		{
			if (chunk.spriteIds == null || chunk.spriteIds.Length == 0)
			{
				chunk.spriteIds = new int[this.divX * this.divY];
				for (int i = 0; i < this.divX * this.divY; i++)
				{
					chunk.spriteIds[i] = -1;
				}
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x0014FF30 File Offset: 0x0014E130
		private void Optimize(SpriteChunk chunk)
		{
			bool flag = true;
			foreach (int num in chunk.spriteIds)
			{
				if (num != -1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				chunk.spriteIds = new int[0];
			}
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0014FF80 File Offset: 0x0014E180
		public void Optimize()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				this.Optimize(spriteChunk);
			}
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x0014FFB8 File Offset: 0x0014E1B8
		public void OptimizeIncremental()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				if (spriteChunk.Dirty)
				{
					this.Optimize(spriteChunk);
				}
			}
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0014FFFC File Offset: 0x0014E1FC
		public void ClearDirtyFlag()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				spriteChunk.Dirty = false;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06004137 RID: 16695 RVA: 0x00150034 File Offset: 0x0014E234
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
				{
					if (!spriteChunk.IsEmpty)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x040033D5 RID: 13269
		public int hash;

		// Token: 0x040033D6 RID: 13270
		public SpriteChannel spriteChannel;

		// Token: 0x040033D7 RID: 13271
		private const int tileMask = 16777215;

		// Token: 0x040033D8 RID: 13272
		private const int flagMask = -16777216;

		// Token: 0x040033D9 RID: 13273
		public int width;

		// Token: 0x040033DA RID: 13274
		public int height;

		// Token: 0x040033DB RID: 13275
		public int numColumns;

		// Token: 0x040033DC RID: 13276
		public int numRows;

		// Token: 0x040033DD RID: 13277
		public int divX;

		// Token: 0x040033DE RID: 13278
		public int divY;

		// Token: 0x040033DF RID: 13279
		public GameObject gameObject;
	}
}
