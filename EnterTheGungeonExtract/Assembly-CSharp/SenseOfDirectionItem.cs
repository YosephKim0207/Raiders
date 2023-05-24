using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200149E RID: 5278
public class SenseOfDirectionItem : PlayerItem
{
	// Token: 0x06007815 RID: 30741 RVA: 0x002FFAF0 File Offset: 0x002FDCF0
	protected override void DoEffect(PlayerController user)
	{
		RoomHandler roomHandler = null;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[i];
			if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT)
			{
				roomHandler = roomHandler2;
				break;
			}
		}
		if (roomHandler == null)
		{
			List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
			for (int j = 0; j < allEnemies.Count; j++)
			{
				if (allEnemies[j])
				{
					if (allEnemies[j].GetComponent<LichDeathController>())
					{
						roomHandler = allEnemies[j].ParentRoom;
						break;
					}
				}
			}
		}
		if (roomHandler == null)
		{
			Debug.LogError("Using SenseOfDirection in Dungeon with no EXIT?");
			return;
		}
		RoomHandler currentRoom = user.CurrentRoom;
		if (roomHandler == currentRoom)
		{
			return;
		}
		List<RoomHandler> list = SenseOfDirectionItem.FindPathBetweenNodes(currentRoom, roomHandler, GameManager.Instance.Dungeon.data.rooms);
		IntVector2 intVector = user.CenterPosition.ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = intVector;
		if (list != null && list.Count > 0 && (list[0] != currentRoom || list.Count >= 2))
		{
			RoomHandler roomHandler3 = ((list[0] != currentRoom) ? list[0] : list[1]);
			RuntimeExitDefinition exitDefinitionForConnectedRoom = currentRoom.GetExitDefinitionForConnectedRoom(roomHandler3);
			intVector2 = exitDefinitionForConnectedRoom.GetUpstreamBasePosition();
		}
		Vector2 vector = intVector2.ToCenterVector2() - user.CenterPosition;
		SpawnManager.SpawnVFX(this.arrowVFX, user.SpriteBottomCenter + vector.ToVector3ZUp(0f).normalized, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector)), false);
	}

	// Token: 0x06007816 RID: 30742 RVA: 0x002FFCD8 File Offset: 0x002FDED8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06007817 RID: 30743 RVA: 0x002FFCE0 File Offset: 0x002FDEE0
	public static List<RoomHandler> FindPathBetweenNodes(RoomHandler origin, RoomHandler target, List<RoomHandler> allRooms)
	{
		Dictionary<RoomHandler, int> dictionary = new Dictionary<RoomHandler, int>();
		Dictionary<RoomHandler, RoomHandler> dictionary2 = new Dictionary<RoomHandler, RoomHandler>();
		for (int i = 0; i < allRooms.Count; i++)
		{
			int num = int.MaxValue;
			if (allRooms[i] == origin)
			{
				num = 0;
			}
			if (!dictionary.ContainsKey(allRooms[i]))
			{
				dictionary.Add(allRooms[i], num);
			}
		}
		RoomHandler roomHandler = origin;
		int num2 = 1;
		List<RoomHandler> connectedRooms;
		int j;
		for (;;)
		{
			connectedRooms = roomHandler.connectedRooms;
			for (j = 0; j < connectedRooms.Count; j++)
			{
				if (connectedRooms[j] == target)
				{
					goto Block_4;
				}
				if (dictionary.ContainsKey(connectedRooms[j]) && dictionary[connectedRooms[j]] > num2)
				{
					dictionary[connectedRooms[j]] = num2;
					if (dictionary2.ContainsKey(connectedRooms[j]))
					{
						dictionary2[connectedRooms[j]] = roomHandler;
					}
					else
					{
						dictionary2.Add(connectedRooms[j], roomHandler);
					}
				}
			}
			dictionary.Remove(roomHandler);
			if (dictionary.Count == 0)
			{
				goto Block_10;
			}
			roomHandler = null;
			num2 = int.MaxValue;
			foreach (RoomHandler roomHandler2 in dictionary.Keys)
			{
				if (dictionary[roomHandler2] < num2)
				{
					roomHandler = roomHandler2;
					num2 = dictionary[roomHandler2];
				}
			}
			if (roomHandler == null)
			{
				goto Block_12;
			}
		}
		Block_4:
		if (dictionary2.ContainsKey(connectedRooms[j]))
		{
			dictionary2[connectedRooms[j]] = roomHandler;
		}
		else
		{
			dictionary2.Add(connectedRooms[j], roomHandler);
		}
		goto IL_1D0;
		Block_10:
		return null;
		Block_12:
		IL_1D0:
		if (!dictionary2.ContainsKey(target))
		{
			return null;
		}
		List<RoomHandler> list = new List<RoomHandler>();
		RoomHandler roomHandler3 = target;
		while (roomHandler3 != null)
		{
			list.Insert(0, roomHandler3);
			if (dictionary2.ContainsKey(roomHandler3))
			{
				roomHandler3 = dictionary2[roomHandler3];
			}
			else
			{
				roomHandler3 = null;
			}
		}
		return list;
	}

	// Token: 0x04007A37 RID: 31287
	public GameObject arrowVFX;
}
