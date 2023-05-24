using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F3D RID: 3901
[Serializable]
public class TileIndexGrid : ScriptableObject
{
	// Token: 0x060053CA RID: 21450 RVA: 0x001EB180 File Offset: 0x001E9380
	protected virtual int ProcessBenubbedTiles(bool isNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder)
	{
		if (!isNorthBorder && !isEastBorder && !isWestBorder && !isSouthBorder)
		{
			if (isNortheastBorder && isNorthwestBorder && isSoutheastBorder && isSouthwestBorder && this.quadNubs.ContainsValid())
			{
				return this.quadNubs.GetIndexByWeight();
			}
			if (isNortheastBorder)
			{
				if (isNorthwestBorder && this.doubleNubsTop.ContainsValid())
				{
					return this.doubleNubsTop.GetIndexByWeight();
				}
				if (isSoutheastBorder && this.doubleNubsRight.ContainsValid())
				{
					return this.doubleNubsRight.GetIndexByWeight();
				}
				if (isSouthwestBorder && this.diagonalNubsTopRightBottomLeft.ContainsValid())
				{
					return this.diagonalNubsTopRightBottomLeft.GetIndexByWeight();
				}
				if (this.topRightNubIndices.ContainsValid())
				{
					return this.topRightNubIndices.GetIndexByWeight();
				}
			}
			if (isNorthwestBorder)
			{
				if (isSouthwestBorder && this.doubleNubsLeft.ContainsValid())
				{
					return this.doubleNubsLeft.GetIndexByWeight();
				}
				if (isSoutheastBorder && this.diagonalNubsTopLeftBottomRight.ContainsValid())
				{
					return this.diagonalNubsTopLeftBottomRight.GetIndexByWeight();
				}
				if (this.topLeftNubIndices.ContainsValid())
				{
					return this.topLeftNubIndices.GetIndexByWeight();
				}
			}
			if (isSoutheastBorder)
			{
				if (isSouthwestBorder && this.doubleNubsBottom.ContainsValid())
				{
					return this.doubleNubsBottom.GetIndexByWeight();
				}
				if (this.bottomRightNubIndices.ContainsValid())
				{
					return this.bottomRightNubIndices.GetIndexByWeight();
				}
			}
			if (isSouthwestBorder && this.bottomLeftNubIndices.ContainsValid())
			{
				return this.bottomLeftNubIndices.GetIndexByWeight();
			}
		}
		if (isNorthBorder && !isEastBorder && !isSouthBorder && !isWestBorder)
		{
			if (isSoutheastBorder && isSouthwestBorder && this.borderTopNubBothIndices.ContainsValid())
			{
				return this.borderTopNubBothIndices.GetIndexByWeight();
			}
			if (isSoutheastBorder && this.borderTopNubRightIndices.ContainsValid())
			{
				return this.borderTopNubRightIndices.GetIndexByWeight();
			}
			if (isSouthwestBorder && this.borderTopNubLeftIndices.ContainsValid())
			{
				return this.borderTopNubLeftIndices.GetIndexByWeight();
			}
		}
		if (!isNorthBorder && isEastBorder && !isSouthBorder && !isWestBorder)
		{
			if (isNorthwestBorder && isSouthwestBorder && this.borderRightNubBothIndices.ContainsValid())
			{
				return this.borderRightNubBothIndices.GetIndexByWeight();
			}
			if (isSouthwestBorder && this.borderRightNubBottomIndices.ContainsValid())
			{
				return this.borderRightNubBottomIndices.GetIndexByWeight();
			}
			if (isNorthwestBorder && this.borderRightNubTopIndices.ContainsValid())
			{
				return this.borderRightNubTopIndices.GetIndexByWeight();
			}
		}
		if (!isNorthBorder && !isEastBorder && isSouthBorder && !isWestBorder)
		{
			if (isNortheastBorder && isNorthwestBorder && this.borderBottomNubBothIndices.ContainsValid())
			{
				return this.borderBottomNubBothIndices.GetIndexByWeight();
			}
			if (isNorthwestBorder && this.borderBottomNubLeftIndices.ContainsValid())
			{
				return this.borderBottomNubLeftIndices.GetIndexByWeight();
			}
			if (isNortheastBorder && this.borderBottomNubRightIndices.ContainsValid())
			{
				return this.borderBottomNubRightIndices.GetIndexByWeight();
			}
		}
		if (!isNorthBorder && !isEastBorder && !isSouthBorder && isWestBorder)
		{
			if (isNortheastBorder && isSoutheastBorder && this.borderLeftNubBothIndices.ContainsValid())
			{
				return this.borderLeftNubBothIndices.GetIndexByWeight();
			}
			if (isNortheastBorder && this.borderLeftNubTopIndices.ContainsValid())
			{
				return this.borderLeftNubTopIndices.GetIndexByWeight();
			}
			if (isSoutheastBorder && this.borderLeftNubBottomIndices.ContainsValid())
			{
				return this.borderLeftNubBottomIndices.GetIndexByWeight();
			}
		}
		if (isNorthBorder && isEastBorder && !isSouthBorder && !isWestBorder && isSouthwestBorder)
		{
			return this.topRightWithNub.GetIndexByWeight();
		}
		if (!isNorthBorder && isEastBorder && isSouthBorder && !isWestBorder && isNorthwestBorder)
		{
			return this.bottomRightWithNub.GetIndexByWeight();
		}
		if (!isNorthBorder && !isEastBorder && isSouthBorder && isWestBorder && isNortheastBorder)
		{
			return this.bottomLeftWithNub.GetIndexByWeight();
		}
		if (isNorthBorder && !isEastBorder && !isSouthBorder && isWestBorder && isSoutheastBorder)
		{
			return this.topLeftWithNub.GetIndexByWeight();
		}
		return -1;
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x001EB614 File Offset: 0x001E9814
	public virtual int GetIndexGivenEightSides(bool[] eightSides)
	{
		return this.GetIndexGivenSides(eightSides[0], eightSides[1], eightSides[2], eightSides[3], eightSides[4], eightSides[5], eightSides[6], eightSides[7]);
	}

	// Token: 0x060053CC RID: 21452 RVA: 0x001EB640 File Offset: 0x001E9840
	public virtual int GetStaticIndexGivenSides(bool isNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder)
	{
		UnityEngine.Random.InitState(147);
		int num = this.ProcessBenubbedTiles(isNorthBorder, isNortheastBorder, isEastBorder, isSoutheastBorder, isSouthBorder, isSouthwestBorder, isWestBorder, isNorthwestBorder);
		if (num != -1)
		{
			return num;
		}
		return this.GetIndexGivenSides(isNorthBorder, isEastBorder, isSouthBorder, isWestBorder);
	}

	// Token: 0x060053CD RID: 21453 RVA: 0x001EB680 File Offset: 0x001E9880
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder)
	{
		int num = this.ProcessBenubbedTiles(isNorthBorder, isNortheastBorder, isEastBorder, isSoutheastBorder, isSouthBorder, isSouthwestBorder, isWestBorder, isNorthwestBorder);
		if (num != -1)
		{
			return num;
		}
		return this.GetIndexGivenSides(isNorthBorder, isEastBorder, isSouthBorder, isWestBorder);
	}

	// Token: 0x060053CE RID: 21454 RVA: 0x001EB6B8 File Offset: 0x001E98B8
	public virtual int GetIndexGivenSides(List<CellData> cells, Func<CellData, bool> evalFunc)
	{
		return this.GetIndexGivenSides(evalFunc(cells[0]), evalFunc(cells[1]), evalFunc(cells[2]), evalFunc(cells[3]), evalFunc(cells[4]), evalFunc(cells[5]), evalFunc(cells[6]), evalFunc(cells[7]));
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x001EB734 File Offset: 0x001E9934
	public virtual int GetRatChunkIndexGivenSidesStatic(bool isNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder, bool isTwoSouthEmpty, out TileIndexGrid.RatChunkResult result)
	{
		result = TileIndexGrid.RatChunkResult.NONE;
		if (isNorthBorder || isEastBorder || isSouthBorder || isWestBorder)
		{
			if ((isNorthBorder && isTwoSouthEmpty) || (isEastBorder && isTwoSouthEmpty) || (isWestBorder && isTwoSouthEmpty))
			{
				result = TileIndexGrid.RatChunkResult.BOTTOM;
				return this.RatChunkBottomSet.indices[0];
			}
			result = TileIndexGrid.RatChunkResult.NORMAL;
			return this.RatChunkNormalSet.indices[0];
		}
		else
		{
			if (isNortheastBorder || isNorthwestBorder || isSoutheastBorder || isSouthwestBorder)
			{
				result = TileIndexGrid.RatChunkResult.CORNER;
				return this.bottomRightNubIndices.indices[0];
			}
			return this.centerIndices.indices[0];
		}
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x001EB7F8 File Offset: 0x001E99F8
	public virtual int GetRatChunkIndexGivenSides(bool isNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder, bool isTwoSouthEmpty, out TileIndexGrid.RatChunkResult result)
	{
		result = TileIndexGrid.RatChunkResult.NONE;
		if (isNorthBorder || isEastBorder || isSouthBorder || isWestBorder)
		{
			if ((isNorthBorder && isTwoSouthEmpty) || (isEastBorder && isTwoSouthEmpty) || (isWestBorder && isTwoSouthEmpty))
			{
				result = TileIndexGrid.RatChunkResult.BOTTOM;
				return this.RatChunkBottomSet.GetIndexByWeight();
			}
			result = TileIndexGrid.RatChunkResult.NORMAL;
			return this.RatChunkNormalSet.GetIndexByWeight();
		}
		else
		{
			if (isNortheastBorder || isNorthwestBorder || isSoutheastBorder || isSouthwestBorder)
			{
				result = TileIndexGrid.RatChunkResult.CORNER;
				return this.bottomRightNubIndices.GetIndexByWeight();
			}
			return this.centerIndices.GetIndexByWeight();
		}
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x001EB8A4 File Offset: 0x001E9AA4
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isEastBorder, bool isSouthBorder, bool isWestBorder)
	{
		if (isNorthBorder && isEastBorder && isSouthBorder && isWestBorder)
		{
			return (!this.allSidesIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.allSidesIndices.GetIndexByWeight();
		}
		if (isNorthBorder && isEastBorder && isWestBorder)
		{
			return (!this.topCapIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.topCapIndices.GetIndexByWeight();
		}
		if (isEastBorder && isNorthBorder && isSouthBorder)
		{
			return (!this.rightCapIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.rightCapIndices.GetIndexByWeight();
		}
		if (isSouthBorder && isEastBorder && isWestBorder)
		{
			return (!this.bottomCapIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.bottomCapIndices.GetIndexByWeight();
		}
		if (isWestBorder && isSouthBorder && isNorthBorder)
		{
			return (!this.leftCapIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.leftCapIndices.GetIndexByWeight();
		}
		if (isNorthBorder && isEastBorder)
		{
			return (!this.topRightIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.topRightIndices.GetIndexByWeight();
		}
		if (isEastBorder && isSouthBorder)
		{
			return (!this.bottomRightIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.bottomRightIndices.GetIndexByWeight();
		}
		if (isSouthBorder && isWestBorder)
		{
			return (!this.bottomLeftIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.bottomLeftIndices.GetIndexByWeight();
		}
		if (isWestBorder && isNorthBorder)
		{
			return (!this.topLeftIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.topLeftIndices.GetIndexByWeight();
		}
		if (isNorthBorder && isSouthBorder)
		{
			return (!this.horizontalIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.horizontalIndices.GetIndexByWeight();
		}
		if (isEastBorder && isWestBorder)
		{
			return (!this.verticalIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.verticalIndices.GetIndexByWeight();
		}
		if (isNorthBorder)
		{
			return (!this.topIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.topIndices.GetIndexByWeight();
		}
		if (isEastBorder)
		{
			return (!this.rightIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.rightIndices.GetIndexByWeight();
		}
		if (isSouthBorder)
		{
			return (!this.bottomIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.bottomIndices.GetIndexByWeight();
		}
		if (isWestBorder)
		{
			return (!this.leftIndices.ContainsValid()) ? this.centerIndices.GetIndexByWeight() : this.leftIndices.GetIndexByWeight();
		}
		return this.centerIndices.GetIndexByWeight();
	}

	// Token: 0x060053D2 RID: 21458 RVA: 0x001EBC18 File Offset: 0x001E9E18
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isSecondNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder)
	{
		if (!isNorthBorder && isSecondNorthBorder && this.extendedSet)
		{
			if (isEastBorder && this.topCenterRightIndices.ContainsValid())
			{
				return this.topCenterRightIndices.GetIndexByWeight();
			}
			if (isWestBorder && this.topCenterLeftIndices.ContainsValid())
			{
				return this.topCenterLeftIndices.GetIndexByWeight();
			}
			if (this.topCenterIndices.ContainsValid())
			{
				return this.topCenterIndices.GetIndexByWeight();
			}
		}
		return this.GetIndexGivenSides(isNorthBorder, isNortheastBorder, isEastBorder, isSoutheastBorder, isSouthBorder, isSouthwestBorder, isWestBorder, isNorthwestBorder);
	}

	// Token: 0x060053D3 RID: 21459 RVA: 0x001EBCB4 File Offset: 0x001E9EB4
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isSecondNorthBorder, bool isThirdNorthBorder, bool isNortheastBorder, bool isEastBorder, bool isSoutheastBorder, bool isSouthBorder, bool isSouthwestBorder, bool isWestBorder, bool isNorthwestBorder)
	{
		if (!isNorthBorder && !isSecondNorthBorder && isThirdNorthBorder && this.extendedSet)
		{
			if (isEastBorder && this.thirdTopRowRightIndices.ContainsValid())
			{
				return this.thirdTopRowRightIndices.GetIndexByWeight();
			}
			if (isWestBorder && this.thirdTopRowLeftIndices.ContainsValid())
			{
				return this.thirdTopRowLeftIndices.GetIndexByWeight();
			}
			if (this.thirdTopRowCenterIndices.ContainsValid())
			{
				return this.thirdTopRowCenterIndices.GetIndexByWeight();
			}
		}
		return this.GetIndexGivenSides(isNorthBorder, isSecondNorthBorder, isNortheastBorder, isEastBorder, isSoutheastBorder, isSouthBorder, isSouthwestBorder, isWestBorder, isNorthwestBorder);
	}

	// Token: 0x060053D4 RID: 21460 RVA: 0x001EBD58 File Offset: 0x001E9F58
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isSecondNorthBorder, bool isEastBorder, bool isSouthBorder, bool isWestBorder)
	{
		if (!isNorthBorder && isSecondNorthBorder && this.extendedSet)
		{
			if (isEastBorder && this.topCenterRightIndices.ContainsValid())
			{
				return this.topCenterRightIndices.GetIndexByWeight();
			}
			if (isWestBorder && this.topCenterLeftIndices.ContainsValid())
			{
				return this.topCenterLeftIndices.GetIndexByWeight();
			}
			if (this.topCenterIndices.ContainsValid())
			{
				return this.topCenterIndices.GetIndexByWeight();
			}
		}
		return this.GetIndexGivenSides(isNorthBorder, isEastBorder, isSouthBorder, isWestBorder);
	}

	// Token: 0x060053D5 RID: 21461 RVA: 0x001EBDEC File Offset: 0x001E9FEC
	public virtual int GetIndexGivenSides(bool isNorthBorder, bool isSecondNorthBorder, bool isThirdNorthBorder, bool isEastBorder, bool isSouthBorder, bool isWestBorder)
	{
		if (!isNorthBorder && !isSecondNorthBorder && isThirdNorthBorder && this.extendedSet)
		{
			if (isEastBorder && this.thirdTopRowRightIndices.ContainsValid())
			{
				return this.thirdTopRowRightIndices.GetIndexByWeight();
			}
			if (isWestBorder && this.thirdTopRowLeftIndices.ContainsValid())
			{
				return this.thirdTopRowLeftIndices.GetIndexByWeight();
			}
			if (this.thirdTopRowCenterIndices.ContainsValid())
			{
				return this.thirdTopRowCenterIndices.GetIndexByWeight();
			}
		}
		return this.GetIndexGivenSides(isNorthBorder, isSecondNorthBorder, isEastBorder, isSouthBorder, isWestBorder);
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x001EBE88 File Offset: 0x001EA088
	public virtual int GetInternalIndexGivenSides(bool isNorthBorder, bool isEastBorder, bool isSouthBorder, bool isWestBorder)
	{
		if (!this.extendedSet)
		{
			return -1;
		}
		if (isSouthBorder)
		{
			return -1;
		}
		if (!isEastBorder)
		{
			return this.internalBottomRightCenterIndices.GetIndexByWeight();
		}
		if (!isWestBorder)
		{
			return this.internalBottomLeftCenterIndices.GetIndexByWeight();
		}
		return this.internalBottomCenterIndices.GetIndexByWeight();
	}

	// Token: 0x04004C6E RID: 19566
	public int roomTypeRestriction = -1;

	// Token: 0x04004C6F RID: 19567
	[TileIndexList]
	public TileIndexList topLeftIndices;

	// Token: 0x04004C70 RID: 19568
	[TileIndexList]
	public TileIndexList topIndices;

	// Token: 0x04004C71 RID: 19569
	[TileIndexList]
	public TileIndexList topRightIndices;

	// Token: 0x04004C72 RID: 19570
	[TileIndexList]
	public TileIndexList leftIndices;

	// Token: 0x04004C73 RID: 19571
	[TileIndexList]
	public TileIndexList centerIndices;

	// Token: 0x04004C74 RID: 19572
	[TileIndexList]
	public TileIndexList rightIndices;

	// Token: 0x04004C75 RID: 19573
	[TileIndexList]
	public TileIndexList bottomLeftIndices;

	// Token: 0x04004C76 RID: 19574
	[TileIndexList]
	public TileIndexList bottomIndices;

	// Token: 0x04004C77 RID: 19575
	[TileIndexList]
	public TileIndexList bottomRightIndices;

	// Token: 0x04004C78 RID: 19576
	[TileIndexList]
	public TileIndexList horizontalIndices;

	// Token: 0x04004C79 RID: 19577
	[TileIndexList]
	public TileIndexList verticalIndices;

	// Token: 0x04004C7A RID: 19578
	[TileIndexList]
	public TileIndexList topCapIndices;

	// Token: 0x04004C7B RID: 19579
	[TileIndexList]
	public TileIndexList rightCapIndices;

	// Token: 0x04004C7C RID: 19580
	[TileIndexList]
	public TileIndexList bottomCapIndices;

	// Token: 0x04004C7D RID: 19581
	[TileIndexList]
	public TileIndexList leftCapIndices;

	// Token: 0x04004C7E RID: 19582
	[TileIndexList]
	public TileIndexList allSidesIndices;

	// Token: 0x04004C7F RID: 19583
	[TileIndexList]
	public TileIndexList topLeftNubIndices;

	// Token: 0x04004C80 RID: 19584
	[TileIndexList]
	public TileIndexList topRightNubIndices;

	// Token: 0x04004C81 RID: 19585
	[TileIndexList]
	public TileIndexList bottomLeftNubIndices;

	// Token: 0x04004C82 RID: 19586
	[TileIndexList]
	public TileIndexList bottomRightNubIndices;

	// Token: 0x04004C83 RID: 19587
	public bool extendedSet;

	// Token: 0x04004C84 RID: 19588
	[TileIndexList]
	[Header("Extended Set")]
	public TileIndexList topCenterLeftIndices;

	// Token: 0x04004C85 RID: 19589
	[TileIndexList]
	public TileIndexList topCenterIndices;

	// Token: 0x04004C86 RID: 19590
	[TileIndexList]
	public TileIndexList topCenterRightIndices;

	// Token: 0x04004C87 RID: 19591
	[TileIndexList]
	public TileIndexList thirdTopRowLeftIndices;

	// Token: 0x04004C88 RID: 19592
	[TileIndexList]
	public TileIndexList thirdTopRowCenterIndices;

	// Token: 0x04004C89 RID: 19593
	[TileIndexList]
	public TileIndexList thirdTopRowRightIndices;

	// Token: 0x04004C8A RID: 19594
	[TileIndexList]
	public TileIndexList internalBottomLeftCenterIndices;

	// Token: 0x04004C8B RID: 19595
	[TileIndexList]
	public TileIndexList internalBottomCenterIndices;

	// Token: 0x04004C8C RID: 19596
	[TileIndexList]
	public TileIndexList internalBottomRightCenterIndices;

	// Token: 0x04004C8D RID: 19597
	[Header("Additional Borders")]
	[TileIndexList]
	public TileIndexList borderTopNubLeftIndices;

	// Token: 0x04004C8E RID: 19598
	[TileIndexList]
	public TileIndexList borderTopNubRightIndices;

	// Token: 0x04004C8F RID: 19599
	[TileIndexList]
	public TileIndexList borderTopNubBothIndices;

	// Token: 0x04004C90 RID: 19600
	[TileIndexList]
	public TileIndexList borderRightNubTopIndices;

	// Token: 0x04004C91 RID: 19601
	[TileIndexList]
	public TileIndexList borderRightNubBottomIndices;

	// Token: 0x04004C92 RID: 19602
	[TileIndexList]
	public TileIndexList borderRightNubBothIndices;

	// Token: 0x04004C93 RID: 19603
	[TileIndexList]
	public TileIndexList borderBottomNubLeftIndices;

	// Token: 0x04004C94 RID: 19604
	[TileIndexList]
	public TileIndexList borderBottomNubRightIndices;

	// Token: 0x04004C95 RID: 19605
	[TileIndexList]
	public TileIndexList borderBottomNubBothIndices;

	// Token: 0x04004C96 RID: 19606
	[TileIndexList]
	public TileIndexList borderLeftNubTopIndices;

	// Token: 0x04004C97 RID: 19607
	[TileIndexList]
	public TileIndexList borderLeftNubBottomIndices;

	// Token: 0x04004C98 RID: 19608
	[TileIndexList]
	public TileIndexList borderLeftNubBothIndices;

	// Token: 0x04004C99 RID: 19609
	[TileIndexList]
	public TileIndexList diagonalNubsTopLeftBottomRight;

	// Token: 0x04004C9A RID: 19610
	[TileIndexList]
	public TileIndexList diagonalNubsTopRightBottomLeft;

	// Token: 0x04004C9B RID: 19611
	[TileIndexList]
	public TileIndexList doubleNubsTop;

	// Token: 0x04004C9C RID: 19612
	[TileIndexList]
	public TileIndexList doubleNubsRight;

	// Token: 0x04004C9D RID: 19613
	[TileIndexList]
	public TileIndexList doubleNubsBottom;

	// Token: 0x04004C9E RID: 19614
	[TileIndexList]
	public TileIndexList doubleNubsLeft;

	// Token: 0x04004C9F RID: 19615
	[TileIndexList]
	public TileIndexList quadNubs;

	// Token: 0x04004CA0 RID: 19616
	[TileIndexList]
	public TileIndexList topRightWithNub;

	// Token: 0x04004CA1 RID: 19617
	[TileIndexList]
	public TileIndexList topLeftWithNub;

	// Token: 0x04004CA2 RID: 19618
	[TileIndexList]
	public TileIndexList bottomRightWithNub;

	// Token: 0x04004CA3 RID: 19619
	[TileIndexList]
	public TileIndexList bottomLeftWithNub;

	// Token: 0x04004CA4 RID: 19620
	[Header("Diagonals--For Borders Only")]
	[TileIndexList]
	public TileIndexList diagonalBorderNE;

	// Token: 0x04004CA5 RID: 19621
	[TileIndexList]
	public TileIndexList diagonalBorderSE;

	// Token: 0x04004CA6 RID: 19622
	[TileIndexList]
	public TileIndexList diagonalBorderSW;

	// Token: 0x04004CA7 RID: 19623
	[TileIndexList]
	public TileIndexList diagonalBorderNW;

	// Token: 0x04004CA8 RID: 19624
	[TileIndexList]
	public TileIndexList diagonalCeilingNE;

	// Token: 0x04004CA9 RID: 19625
	[TileIndexList]
	public TileIndexList diagonalCeilingSE;

	// Token: 0x04004CAA RID: 19626
	[TileIndexList]
	public TileIndexList diagonalCeilingSW;

	// Token: 0x04004CAB RID: 19627
	[TileIndexList]
	public TileIndexList diagonalCeilingNW;

	// Token: 0x04004CAC RID: 19628
	[Header("Carpet Options")]
	public bool CenterCheckerboard;

	// Token: 0x04004CAD RID: 19629
	public int CheckerboardDimension = 1;

	// Token: 0x04004CAE RID: 19630
	public bool CenterIndicesAreStrata;

	// Token: 0x04004CAF RID: 19631
	[Header("Weirdo Options")]
	[Space(5f)]
	public List<TileIndexGrid> PitInternalSquareGrids;

	// Token: 0x04004CB0 RID: 19632
	[Space(5f)]
	public PitSquarePlacementOptions PitInternalSquareOptions;

	// Token: 0x04004CB1 RID: 19633
	[Space(5f)]
	public bool PitBorderIsInternal;

	// Token: 0x04004CB2 RID: 19634
	[Space(5f)]
	public bool PitBorderOverridesFloorTile;

	// Token: 0x04004CB3 RID: 19635
	[Space(5f)]
	public bool CeilingBorderUsesDistancedCenters;

	// Token: 0x04004CB4 RID: 19636
	[Header("For Rat Chunk Borders")]
	[Space(5f)]
	public bool UsesRatChunkBorders;

	// Token: 0x04004CB5 RID: 19637
	[TileIndexList]
	public TileIndexList RatChunkNormalSet;

	// Token: 0x04004CB6 RID: 19638
	[TileIndexList]
	public TileIndexList RatChunkBottomSet;

	// Token: 0x04004CB7 RID: 19639
	[Header("Path Options")]
	[Space(5f)]
	public GameObject PathFacewallStamp;

	// Token: 0x04004CB8 RID: 19640
	public GameObject PathSidewallStamp;

	// Token: 0x04004CB9 RID: 19641
	[Space(5f)]
	[TileIndexList]
	public TileIndexList PathPitPosts;

	// Token: 0x04004CBA RID: 19642
	[TileIndexList]
	public TileIndexList PathPitPostsBL;

	// Token: 0x04004CBB RID: 19643
	[TileIndexList]
	public TileIndexList PathPitPostsBR;

	// Token: 0x04004CBC RID: 19644
	[Space(5f)]
	public GameObject PathStubNorth;

	// Token: 0x04004CBD RID: 19645
	public GameObject PathStubEast;

	// Token: 0x04004CBE RID: 19646
	public GameObject PathStubSouth;

	// Token: 0x04004CBF RID: 19647
	public GameObject PathStubWest;

	// Token: 0x02000F3E RID: 3902
	public enum RatChunkResult
	{
		// Token: 0x04004CC1 RID: 19649
		NONE,
		// Token: 0x04004CC2 RID: 19650
		NORMAL,
		// Token: 0x04004CC3 RID: 19651
		BOTTOM,
		// Token: 0x04004CC4 RID: 19652
		CORNER
	}
}
