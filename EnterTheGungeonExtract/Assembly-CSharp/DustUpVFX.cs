using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012EA RID: 4842
[Serializable]
public class DustUpVFX
{
	// Token: 0x06006CB6 RID: 27830 RVA: 0x002ACBCC File Offset: 0x002AADCC
	public void InstantiateLandDustup(Vector3 worldPosition)
	{
		SpawnManager.SpawnVFX(this.rollLandDustup, worldPosition, Quaternion.identity);
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x002ACBE0 File Offset: 0x002AADE0
	public void InstantiateDodgeDustup(Vector2 velocity, Vector3 worldPosition)
	{
		switch (DungeonData.GetDirectionFromVector2(velocity))
		{
		case DungeonData.Direction.NORTH:
			if (this.rollNorthDustup != null)
			{
				GameObject gameObject = SpawnManager.SpawnVFX(this.rollNorthDustup, worldPosition, Quaternion.identity);
				gameObject.GetComponent<tk2dSprite>().FlipX = false;
				gameObject.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject2 = SpawnManager.SpawnVFX(this.rollSouthDustup, worldPosition, Quaternion.identity);
				gameObject2.GetComponent<tk2dSprite>().FlipX = false;
				gameObject2.GetComponent<tk2dSprite>().FlipY = true;
			}
			break;
		case DungeonData.Direction.NORTHEAST:
			if (this.rollNorthEastDustup != null)
			{
				GameObject gameObject3 = SpawnManager.SpawnVFX(this.rollNorthEastDustup, worldPosition, Quaternion.identity);
				gameObject3.GetComponent<tk2dSprite>().FlipX = false;
				gameObject3.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject4 = SpawnManager.SpawnVFX(this.rollNorthWestDustup, worldPosition, Quaternion.identity);
				gameObject4.GetComponent<tk2dSprite>().FlipX = true;
				gameObject4.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		case DungeonData.Direction.EAST:
			if (this.rollEastDustup != null)
			{
				GameObject gameObject5 = SpawnManager.SpawnVFX(this.rollEastDustup, worldPosition, Quaternion.identity);
				gameObject5.GetComponent<tk2dSprite>().FlipX = false;
				gameObject5.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject6 = SpawnManager.SpawnVFX(this.rollWestDustup, worldPosition, Quaternion.identity);
				gameObject6.GetComponent<tk2dSprite>().FlipX = true;
				gameObject6.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		case DungeonData.Direction.SOUTHEAST:
			if (this.rollSouthEastDustup != null)
			{
				GameObject gameObject7 = SpawnManager.SpawnVFX(this.rollSouthEastDustup, worldPosition, Quaternion.identity);
				gameObject7.GetComponent<tk2dSprite>().FlipX = false;
				gameObject7.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject8 = SpawnManager.SpawnVFX(this.rollSouthWestDustup, worldPosition, Quaternion.identity);
				gameObject8.GetComponent<tk2dSprite>().FlipX = true;
				gameObject8.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		case DungeonData.Direction.SOUTH:
			if (this.rollSouthDustup != null)
			{
				GameObject gameObject9 = SpawnManager.SpawnVFX(this.rollSouthDustup, worldPosition, Quaternion.identity);
				gameObject9.GetComponent<tk2dSprite>().FlipX = false;
				gameObject9.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject10 = SpawnManager.SpawnVFX(this.rollNorthDustup, worldPosition, Quaternion.identity);
				gameObject10.GetComponent<tk2dSprite>().FlipX = false;
				gameObject10.GetComponent<tk2dSprite>().FlipY = true;
			}
			break;
		case DungeonData.Direction.SOUTHWEST:
			if (this.rollSouthWestDustup != null)
			{
				GameObject gameObject11 = SpawnManager.SpawnVFX(this.rollSouthWestDustup, worldPosition, Quaternion.identity);
				gameObject11.GetComponent<tk2dSprite>().FlipX = false;
				gameObject11.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject12 = SpawnManager.SpawnVFX(this.rollSouthEastDustup, worldPosition, Quaternion.identity);
				gameObject12.GetComponent<tk2dSprite>().FlipX = true;
				gameObject12.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		case DungeonData.Direction.WEST:
			if (this.rollWestDustup != null)
			{
				GameObject gameObject13 = SpawnManager.SpawnVFX(this.rollWestDustup, worldPosition, Quaternion.identity);
				gameObject13.GetComponent<tk2dSprite>().FlipX = false;
				gameObject13.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject14 = SpawnManager.SpawnVFX(this.rollEastDustup, worldPosition, Quaternion.identity);
				gameObject14.GetComponent<tk2dSprite>().FlipX = true;
				gameObject14.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		case DungeonData.Direction.NORTHWEST:
			if (this.rollNorthWestDustup != null)
			{
				GameObject gameObject15 = SpawnManager.SpawnVFX(this.rollNorthWestDustup, worldPosition, Quaternion.identity);
				gameObject15.GetComponent<tk2dSprite>().FlipX = false;
				gameObject15.GetComponent<tk2dSprite>().FlipY = false;
			}
			else
			{
				GameObject gameObject16 = SpawnManager.SpawnVFX(this.rollNorthEastDustup, worldPosition, Quaternion.identity);
				gameObject16.GetComponent<tk2dSprite>().FlipX = true;
				gameObject16.GetComponent<tk2dSprite>().FlipY = false;
			}
			break;
		}
	}

	// Token: 0x040069BE RID: 27070
	public GameObject runDustup;

	// Token: 0x040069BF RID: 27071
	public GameObject waterDustup;

	// Token: 0x040069C0 RID: 27072
	public GameObject additionalWaterDustup;

	// Token: 0x040069C1 RID: 27073
	public GameObject rollNorthDustup;

	// Token: 0x040069C2 RID: 27074
	public GameObject rollNorthEastDustup;

	// Token: 0x040069C3 RID: 27075
	public GameObject rollEastDustup;

	// Token: 0x040069C4 RID: 27076
	public GameObject rollSouthEastDustup;

	// Token: 0x040069C5 RID: 27077
	public GameObject rollSouthDustup;

	// Token: 0x040069C6 RID: 27078
	public GameObject rollSouthWestDustup;

	// Token: 0x040069C7 RID: 27079
	public GameObject rollWestDustup;

	// Token: 0x040069C8 RID: 27080
	public GameObject rollNorthWestDustup;

	// Token: 0x040069C9 RID: 27081
	public GameObject rollLandDustup;
}
