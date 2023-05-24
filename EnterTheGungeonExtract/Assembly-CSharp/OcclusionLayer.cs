using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200152B RID: 5419
public class OcclusionLayer
{
	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06007BB4 RID: 31668 RVA: 0x00318474 File Offset: 0x00316674
	// (set) Token: 0x06007BB5 RID: 31669 RVA: 0x0031847C File Offset: 0x0031667C
	public Texture2D SourceOcclusionTexture
	{
		get
		{
			return this.m_occlusionTexture;
		}
		set
		{
			this.m_occlusionTexture = value;
		}
	}

	// Token: 0x06007BB6 RID: 31670 RVA: 0x00318488 File Offset: 0x00316688
	protected float GetCellOcclusion(int x0, int y0, DungeonData d)
	{
		float num = ((d.cellData[x0][y0] != null) ? d.cellData[x0][y0].occlusionData.cellOcclusion : 1f);
		if (!this.m_pixelatorCached.UseTexturedOcclusion && x0 >= 2 && y0 >= 2 && x0 < d.Width - 2 && y0 < d.Height - 2)
		{
			float num2 = 0f;
			float num3 = 0f;
			for (int i = -2; i <= 2; i++)
			{
				for (int j = -2; j <= 2; j++)
				{
					float num4 = this.KERNEL[i + 2] * this.KERNEL[j + 2];
					float num5;
					if (d.cellData[x0 + i][y0 + j] == null)
					{
						num5 = 1f * num4;
					}
					else
					{
						num5 = d.cellData[x0 + i][y0 + j].occlusionData.cellOcclusion * num4;
					}
					num2 += num5;
					num3 += num4;
				}
			}
			return Mathf.Min(num, num2 / num3);
		}
		return num;
	}

	// Token: 0x06007BB7 RID: 31671 RVA: 0x003185A8 File Offset: 0x003167A8
	protected float GetGValueForCell(int x0, int y0, DungeonData d)
	{
		float num = 0f;
		if (x0 < 0 || x0 >= d.Width || y0 < 0 || y0 >= d.Height)
		{
			return num;
		}
		CellData cellData = d[x0, y0];
		if (cellData == null)
		{
			return num;
		}
		bool useTexturedOcclusion = this.m_pixelatorCached.UseTexturedOcclusion;
		if (cellData.type == CellType.FLOOR || cellData.type == CellType.PIT || cellData.IsLowerFaceWall() || (cellData.IsUpperFacewall() && !useTexturedOcclusion))
		{
			if (cellData.nearestRoom.visibility == RoomHandler.VisibilityStatus.CURRENT)
			{
				num = 1f * (1f - cellData.occlusionData.minCellOccluionHistory);
			}
			else if (cellData.nearestRoom.hasEverBeenVisited && cellData.nearestRoom.visibility != RoomHandler.VisibilityStatus.REOBSCURED)
			{
				num = 1f;
			}
		}
		return num;
	}

	// Token: 0x06007BB8 RID: 31672 RVA: 0x0031868C File Offset: 0x0031688C
	protected float GetRValueForCell(int x0, int y0, DungeonData d)
	{
		float num = 0f;
		if (this.m_pixelatorCached.UseTexturedOcclusion)
		{
			return num;
		}
		if (x0 < 0 || x0 >= d.Width || y0 < 0 || y0 >= d.Height)
		{
			return num;
		}
		CellData cellData = d[x0, y0];
		if (cellData == null)
		{
			return num;
		}
		if (cellData.isExitCell)
		{
			return num;
		}
		if (cellData.type == CellType.WALL && !cellData.IsAnyFaceWall())
		{
			return num;
		}
		if (y0 - 2 >= 0 && d[x0, y0 - 2] != null && d[x0, y0 - 2].isExitCell)
		{
			return num;
		}
		RoomHandler roomHandler = d[x0, y0].parentRoom ?? d[x0, y0].nearestRoom;
		bool flag = false;
		if (roomHandler != null)
		{
			for (int i = 0; i < this.m_allPlayersCached.Length; i++)
			{
				if (i != 0 || !this.m_playerOneDead)
				{
					if (i != 1 || !this.m_playerTwoDead)
					{
						if (this.m_allPlayersCached[i].CurrentRoom != null && this.m_allPlayersCached[i].CurrentRoom.connectedRooms != null && this.m_allPlayersCached[i].CurrentRoom.connectedRooms.Contains(roomHandler))
						{
							flag = true;
						}
					}
				}
			}
		}
		if (x0 < 1 || x0 > d.Width - 2 || y0 < 3 || y0 > d.Height - 2)
		{
			return num;
		}
		if (roomHandler == null || roomHandler.visibility == RoomHandler.VisibilityStatus.OBSCURED || roomHandler.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
		{
			if (flag)
			{
				if (cellData.isExitNonOccluder)
				{
					return num;
				}
				if (cellData.isExitCell)
				{
					return num;
				}
				if (y0 > 1 && d[x0, y0 - 1] != null && d[x0, y0 - 1].isExitCell)
				{
					return num;
				}
				if (y0 > 2 && d[x0, y0 - 2] != null && d[x0, y0 - 2].isExitCell)
				{
					return num;
				}
				if (y0 > 3 && d[x0, y0 - 3] != null && d[x0, y0 - 3].isExitCell)
				{
					return num;
				}
				if (x0 > 1 && d[x0 - 1, y0] != null && d[x0 - 1, y0].isExitCell)
				{
					return num;
				}
				if (x0 < d.Width - 1 && d[x0 + 1, y0] != null && d[x0 + 1, y0].isExitCell)
				{
					return num;
				}
			}
			num = 1f;
		}
		return num;
	}

	// Token: 0x06007BB9 RID: 31673 RVA: 0x00318954 File Offset: 0x00316B54
	protected Color GetInterpolatedValueAtPoint(int baseX, int baseY, float worldX, float worldY, DungeonData d)
	{
		int num = baseX + Mathf.FloorToInt(worldX);
		int num2 = baseY + Mathf.FloorToInt(worldY);
		num = baseX + (int)worldX;
		num2 = baseY + (int)worldY;
		float rvalueForCell = this.GetRValueForCell(num, num2, d);
		float gvalueForCell = this.GetGValueForCell(num, num2, d);
		if (!d.CheckInBounds(num, num2))
		{
			return new Color(rvalueForCell, gvalueForCell, 0f, 0f);
		}
		float num3 = Mathf.Clamp01(this.GetCellOcclusion(num, num2, d));
		float num4 = 1f - num3 * num3;
		return new Color(rvalueForCell, gvalueForCell, 0f, num4);
	}

	// Token: 0x06007BBA RID: 31674 RVA: 0x003189E4 File Offset: 0x00316BE4
	public Texture2D GenerateOcclusionTexture(int baseX, int baseY, DungeonData d)
	{
		this.m_gameManagerCached = GameManager.Instance;
		this.m_pixelatorCached = Pixelator.Instance;
		this.m_allPlayersCached = GameManager.Instance.AllPlayers;
		this.m_playerOneDead = !this.m_gameManagerCached.PrimaryPlayer || this.m_gameManagerCached.PrimaryPlayer.healthHaver.IsDead;
		if (this.m_gameManagerCached.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_playerTwoDead = !this.m_gameManagerCached.SecondaryPlayer || this.m_gameManagerCached.SecondaryPlayer.healthHaver.IsDead;
		}
		int num = this.m_pixelatorCached.CurrentMacroResolutionX / 16 + 4;
		int num2 = this.m_pixelatorCached.CurrentMacroResolutionY / 16 + 4;
		int num3 = num * this.textureMultiplier;
		int num4 = num2 * this.textureMultiplier;
		if (this.m_occlusionTexture == null || this.m_occlusionTexture.width != num3 || this.m_occlusionTexture.height != num4)
		{
			if (this.m_occlusionTexture != null)
			{
				this.m_occlusionTexture.Resize(num3, num4);
			}
			else
			{
				this.m_occlusionTexture = new Texture2D(num3, num4, TextureFormat.ARGB32, false);
				this.m_occlusionTexture.filterMode = FilterMode.Bilinear;
				this.m_occlusionTexture.wrapMode = TextureWrapMode.Clamp;
			}
		}
		if (this.m_colorCache == null || this.m_colorCache.Length != num3 * num4)
		{
			this.m_colorCache = new Color[num3 * num4];
		}
		this.cachedX = baseX;
		this.cachedY = baseY;
		if (!this.m_gameManagerCached.IsLoadingLevel)
		{
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num4; j++)
				{
					int num5 = j * num3 + i;
					float num6 = (float)i / (float)this.textureMultiplier;
					float num7 = (float)j / (float)this.textureMultiplier;
					this.m_colorCache[num5] = this.GetInterpolatedValueAtPoint(baseX, baseY, num6, num7, d);
				}
			}
		}
		this.m_occlusionTexture.SetPixels(this.m_colorCache);
		this.m_occlusionTexture.Apply();
		return this.m_occlusionTexture;
	}

	// Token: 0x04007E52 RID: 32338
	public Color occludedColor;

	// Token: 0x04007E53 RID: 32339
	public int cachedX;

	// Token: 0x04007E54 RID: 32340
	public int cachedY;

	// Token: 0x04007E55 RID: 32341
	private GameManager m_gameManagerCached;

	// Token: 0x04007E56 RID: 32342
	private PlayerController[] m_allPlayersCached;

	// Token: 0x04007E57 RID: 32343
	private Pixelator m_pixelatorCached;

	// Token: 0x04007E58 RID: 32344
	private bool m_playerOneDead;

	// Token: 0x04007E59 RID: 32345
	private bool m_playerTwoDead;

	// Token: 0x04007E5A RID: 32346
	protected Texture2D m_occlusionTexture;

	// Token: 0x04007E5B RID: 32347
	protected int textureMultiplier = 1;

	// Token: 0x04007E5C RID: 32348
	protected float[] KERNEL = new float[] { 0.12f, 0.25f, 0.3f, 0.25f, 0.12f };

	// Token: 0x04007E5D RID: 32349
	protected Color[] m_colorCache;
}
