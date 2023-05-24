using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using UnityEngine;

// Token: 0x020011EE RID: 4590
public class ResourcefulRatMazeRoomController : DungeonPlaceableBehaviour
{
	// Token: 0x06006679 RID: 26233 RVA: 0x0027D638 File Offset: 0x0027B838
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		this.Initialize();
		yield break;
	}

	// Token: 0x0600667A RID: 26234 RVA: 0x0027D654 File Offset: 0x0027B854
	private int GetPositionInChain(RoomHandler room)
	{
		ResourcefulRatMazeRoomController[] array = UnityEngine.Object.FindObjectsOfType<ResourcefulRatMazeRoomController>();
		List<ResourcefulRatMazeRoomController> list = new List<ResourcefulRatMazeRoomController>(array);
		list = list.OrderBy((ResourcefulRatMazeRoomController a) => a.transform.position.x).ToList<ResourcefulRatMazeRoomController>();
		this.m_mazes = list;
		return list.IndexOf(this);
	}

	// Token: 0x0600667B RID: 26235 RVA: 0x0027D6A8 File Offset: 0x0027B8A8
	public void Initialize()
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		this.m_room = absoluteRoom;
		this.m_roomIndex = this.GetPositionInChain(absoluteRoom);
		DungeonData.Direction[] resourcefulRatSolution = GameManager.GetResourcefulRatSolution();
		this.m_correctDirection = resourcefulRatSolution[this.m_roomIndex];
		for (int i = 0; i < absoluteRoom.Cells.Count; i++)
		{
			IntVector2 intVector = absoluteRoom.Cells[i];
			this.minX = Mathf.Min(this.minX, intVector.x);
			this.minY = Mathf.Min(this.minY, intVector.y);
			this.maxX = Mathf.Max(this.maxX, intVector.x);
			this.maxY = Mathf.Max(this.maxY, intVector.y);
		}
	}

	// Token: 0x0600667C RID: 26236 RVA: 0x0027D778 File Offset: 0x0027B978
	private bool PlayerInTargetCell(Vector2 playerPos)
	{
		switch (this.m_correctDirection)
		{
		case DungeonData.Direction.NORTH:
			if (playerPos.y > (float)this.maxY)
			{
				return true;
			}
			break;
		case DungeonData.Direction.EAST:
			if (playerPos.x > (float)this.maxX)
			{
				return true;
			}
			break;
		case DungeonData.Direction.SOUTH:
			if (playerPos.y < (float)(this.minY + 1))
			{
				return true;
			}
			break;
		case DungeonData.Direction.WEST:
			if (playerPos.x < (float)(this.minX + 1))
			{
				return true;
			}
			break;
		}
		return false;
	}

	// Token: 0x0600667D RID: 26237 RVA: 0x0027D820 File Offset: 0x0027BA20
	private bool PlayerInFailCell(Vector2 playerPos)
	{
		bool flag = this.m_roomIndex != 0;
		switch (this.m_correctDirection)
		{
		case DungeonData.Direction.NORTH:
			if (playerPos.x > (float)this.maxX)
			{
				return true;
			}
			if (playerPos.y < (float)(this.minY + 1))
			{
				return true;
			}
			if (flag && playerPos.x < (float)(this.minX + 1))
			{
				return true;
			}
			break;
		case DungeonData.Direction.EAST:
			if (playerPos.y > (float)this.maxY)
			{
				return true;
			}
			if (playerPos.y < (float)(this.minY + 1))
			{
				return true;
			}
			if (flag && playerPos.x < (float)(this.minX + 1))
			{
				return true;
			}
			break;
		case DungeonData.Direction.SOUTH:
			if (playerPos.x > (float)this.maxX)
			{
				return true;
			}
			if (playerPos.y > (float)this.maxY)
			{
				return true;
			}
			if (flag && playerPos.x < (float)(this.minX + 1))
			{
				return true;
			}
			break;
		case DungeonData.Direction.WEST:
			if (playerPos.x > (float)this.maxX)
			{
				return true;
			}
			if (playerPos.y > (float)this.maxY)
			{
				return true;
			}
			if (playerPos.y < (float)(this.minY + 1))
			{
				return true;
			}
			break;
		}
		return false;
	}

	// Token: 0x0600667E RID: 26238 RVA: 0x0027D998 File Offset: 0x0027BB98
	public void Update()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			Vector2 vector = playerController.SpriteBottomCenter;
			if (playerController.CurrentRoom == this.m_room)
			{
				if (this.PlayerInTargetCell(vector))
				{
					if (this.m_mazes.IndexOf(this) == this.m_mazes.Count - 1)
					{
						RoomHandler roomHandler = null;
						foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
						{
							if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
							{
								roomHandler = roomHandler2;
								break;
							}
						}
						for (int j = 0; j < roomHandler.connectedRooms.Count; j++)
						{
							if (roomHandler.connectedRooms[j].distanceFromEntrance <= roomHandler.distanceFromEntrance)
							{
								roomHandler = roomHandler.connectedRooms[j];
								break;
							}
						}
						playerController.WarpToPoint(roomHandler.Epicenter.ToVector2(), false, false);
					}
					else
					{
						Vector2 vector2 = this.m_mazes[this.m_mazes.IndexOf(this) + 1].transform.position.XY();
						playerController.WarpToPoint(vector2, false, false);
					}
				}
				else if (this.PlayerInFailCell(vector))
				{
					RoomHandler roomHandler3 = null;
					foreach (RoomHandler roomHandler4 in GameManager.Instance.Dungeon.data.rooms)
					{
						if (roomHandler4.area.PrototypeRoomName.Contains("FailRoom"))
						{
							roomHandler3 = roomHandler4;
							break;
						}
					}
					playerController.WarpToPoint(roomHandler3.Epicenter.ToVector2(), false, false);
				}
			}
		}
	}

	// Token: 0x04006252 RID: 25170
	private List<ResourcefulRatMazeRoomController> m_mazes;

	// Token: 0x04006253 RID: 25171
	private int minX = int.MaxValue;

	// Token: 0x04006254 RID: 25172
	private int minY = int.MaxValue;

	// Token: 0x04006255 RID: 25173
	private int maxX = int.MinValue;

	// Token: 0x04006256 RID: 25174
	private int maxY = int.MinValue;

	// Token: 0x04006257 RID: 25175
	private DungeonData.Direction m_correctDirection;

	// Token: 0x04006258 RID: 25176
	private RoomHandler m_room;

	// Token: 0x04006259 RID: 25177
	private int m_roomIndex;
}
