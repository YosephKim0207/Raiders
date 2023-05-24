using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001279 RID: 4729
public class FlameTrapChallengeModifier : ChallengeModifier
{
	// Token: 0x060069E7 RID: 27111 RVA: 0x00297FC4 File Offset: 0x002961C4
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.BestActivePlayer.CurrentRoom;
		float num = 0f;
		GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
		if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
		{
			if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
					{
						if (tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
						{
							num = this.TrapChanceDecrementPerFloor * 4f;
						}
					}
					else
					{
						num = this.TrapChanceDecrementPerFloor * 4f;
					}
				}
				else
				{
					num = this.TrapChanceDecrementPerFloor * 3f;
				}
			}
			else
			{
				num = this.TrapChanceDecrementPerFloor * 2f;
			}
		}
		else
		{
			num = this.TrapChanceDecrementPerFloor;
		}
		Vector2 centerPosition = GameManager.Instance.BestActivePlayer.CenterPosition;
		for (int i = 0; i < currentRoom.area.dimensions.x; i++)
		{
			for (int j = 0; j < currentRoom.area.dimensions.y; j++)
			{
				IntVector2 intVector = currentRoom.area.basePosition + new IntVector2(i, j);
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
				{
					CellData cellData = GameManager.Instance.Dungeon.data[intVector];
					if (Vector2.Distance(cellData.position.ToCenterVector2(), centerPosition) >= 5f)
					{
						if (cellData.parentRoom == currentRoom && cellData.type == CellType.FLOOR && !cellData.containsTrap && !cellData.isOccupied && !cellData.isOccludedByTopWall && !cellData.PreventRewardSpawn && UnityEngine.Random.value < this.ChanceToTrap - num)
						{
							GameObject gameObject = this.FlameTrap.InstantiateObject(currentRoom, cellData.position - currentRoom.area.basePosition, false, false);
							FlameTrapChallengeModifier.m_activeTraps.Add(gameObject.GetComponent<BasicTrapController>());
							Exploder.DoRadialMinorBreakableBreak(cellData.position.ToCenterVector3((float)cellData.position.y), 1f);
							cellData.containsTrap = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x060069E8 RID: 27112 RVA: 0x0029820C File Offset: 0x0029640C
	private void OnDestroy()
	{
		for (int i = 0; i < FlameTrapChallengeModifier.m_activeTraps.Count; i++)
		{
			if (FlameTrapChallengeModifier.m_activeTraps[i])
			{
				FlameTrapChallengeModifier.m_activeTraps[i].triggerMethod = BasicTrapController.TriggerMethod.Script;
			}
		}
		FlameTrapChallengeModifier.m_activeTraps.Clear();
	}

	// Token: 0x04006661 RID: 26209
	public DungeonPlaceable FlameTrap;

	// Token: 0x04006662 RID: 26210
	public float ChanceToTrap = 0.2f;

	// Token: 0x04006663 RID: 26211
	public float TrapChanceDecrementPerFloor = 0.005f;

	// Token: 0x04006664 RID: 26212
	private static List<BasicTrapController> m_activeTraps = new List<BasicTrapController>();
}
