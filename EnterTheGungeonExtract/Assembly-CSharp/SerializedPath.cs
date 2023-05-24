using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F61 RID: 3937
[Serializable]
public class SerializedPath
{
	// Token: 0x060054D5 RID: 21717 RVA: 0x00201BB0 File Offset: 0x001FFDB0
	public SerializedPath(IntVector2 cellPosition)
	{
		this.nodes = new List<SerializedPathNode>();
		this.nodes.Add(new SerializedPathNode(cellPosition));
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x00201BE8 File Offset: 0x001FFDE8
	public SerializedPath(SerializedPath prototypePath, IntVector2 basePositionAdjustment)
	{
		this.nodes = new List<SerializedPathNode>();
		for (int i = 0; i < prototypePath.nodes.Count; i++)
		{
			this.nodes.Add(new SerializedPathNode(prototypePath.nodes[i], basePositionAdjustment));
		}
		this.wrapMode = prototypePath.wrapMode;
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x00201C60 File Offset: 0x001FFE60
	public static SerializedPath CreateMirror(SerializedPath source, IntVector2 roomDimensions, PrototypeDungeonRoom sourceRoom)
	{
		SerializedPath serializedPath = new SerializedPath(IntVector2.Zero);
		serializedPath.nodes.Clear();
		for (int i = 0; i < source.nodes.Count; i++)
		{
			serializedPath.nodes.Add(SerializedPathNode.CreateMirror(source.nodes[i], roomDimensions));
		}
		serializedPath.wrapMode = source.wrapMode;
		serializedPath.overrideSpeed = source.overrideSpeed;
		serializedPath.tilesetPathGrid = source.tilesetPathGrid;
		int num = sourceRoom.paths.IndexOf(source);
		int num2 = 0;
		for (int j = 0; j < sourceRoom.placedObjects.Count; j++)
		{
			if (num >= 0 && sourceRoom.placedObjects[j].assignedPathIDx == num)
			{
				num2 = Mathf.Max(num2, sourceRoom.placedObjects[j].GetWidth(true));
			}
		}
		for (int k = 0; k < sourceRoom.additionalObjectLayers.Count; k++)
		{
			for (int l = 0; l < sourceRoom.additionalObjectLayers[k].placedObjects.Count; l++)
			{
				if (num >= 0 && sourceRoom.additionalObjectLayers[k].placedObjects[l].assignedPathIDx == num)
				{
					num2 = Mathf.Max(num2, sourceRoom.additionalObjectLayers[k].placedObjects[l].GetWidth(true));
				}
			}
		}
		if (num2 > 0)
		{
			for (int m = 0; m < serializedPath.nodes.Count; m++)
			{
				SerializedPathNode serializedPathNode = serializedPath.nodes[m];
				serializedPathNode.position += new IntVector2(-1, 0) * (num2 - 1);
				serializedPath.nodes[m] = serializedPathNode;
			}
		}
		return serializedPath;
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x00201E50 File Offset: 0x00200050
	public void StampPathToTilemap(RoomHandler parentRoom)
	{
		if (this.tilesetPathGrid < 0)
		{
			return;
		}
		if (this.tilesetPathGrid >= GameManager.Instance.Dungeon.pathGridDefinitions.Count)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = 1; i < this.nodes.Count + 1; i++)
		{
			SerializedPathNode serializedPathNode;
			SerializedPathNode serializedPathNode2;
			if (i == this.nodes.Count)
			{
				if (this.wrapMode != SerializedPath.SerializedPathWrapMode.Loop)
				{
					break;
				}
				serializedPathNode = this.nodes[i - 1];
				serializedPathNode2 = this.nodes[0];
			}
			else
			{
				serializedPathNode = this.nodes[i - 1];
				serializedPathNode2 = this.nodes[i];
			}
			if (serializedPathNode.position.x != serializedPathNode2.position.x && serializedPathNode.position.y != serializedPathNode2.position.y)
			{
				Debug.LogError("Attempting to stamp a path grid to the tilemap and the path contains diagonals! This cannot be.");
				break;
			}
			IntVector2 intVector = parentRoom.area.basePosition + serializedPathNode.position;
			IntVector2 intVector2 = parentRoom.area.basePosition + serializedPathNode2.position;
			if (serializedPathNode.position.x == serializedPathNode2.position.x)
			{
				TileIndexGrid tileIndexGrid = GameManager.Instance.Dungeon.pathGridDefinitions[this.tilesetPathGrid];
				if (tileIndexGrid.PathFacewallStamp != null)
				{
					for (int j = Mathf.Min(intVector.y, intVector2.y); j < Mathf.Max(intVector.y, intVector2.y); j++)
					{
						if (data[intVector.x, j].type != CellType.WALL && data[intVector.x, j + 1].type == CellType.WALL)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid.PathFacewallStamp, new Vector3((float)intVector.x, (float)(j + 1), 0f) + tileIndexGrid.PathFacewallStamp.transform.position, Quaternion.identity);
							gameObject.GetComponent<PlacedWallDecorator>().ConfigureOnPlacement(data.GetAbsoluteRoomFromPosition(gameObject.transform.position.IntXY(VectorConversions.Round)));
						}
					}
				}
			}
			else
			{
				TileIndexGrid tileIndexGrid2 = GameManager.Instance.Dungeon.pathGridDefinitions[this.tilesetPathGrid];
				if (tileIndexGrid2.PathSidewallStamp != null)
				{
					for (int k = Mathf.Min(intVector.x, intVector2.x); k < Mathf.Max(intVector.x, intVector2.x); k++)
					{
						if (data[k, intVector.y].type == CellType.FLOOR && data[k + 1, intVector.y].type == CellType.WALL)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid2.PathSidewallStamp, new Vector3((float)(k + 1), (float)intVector.y, 0f) + tileIndexGrid2.PathSidewallStamp.transform.position, Quaternion.identity);
							gameObject2.GetComponent<tk2dSprite>().FlipX = true;
						}
						else if (data[k, intVector.y].type == CellType.WALL && data[k + 1, intVector.y].type == CellType.FLOOR)
						{
							UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid2.PathSidewallStamp, new Vector3((float)(k + 1), (float)intVector.y, 0f) + tileIndexGrid2.PathSidewallStamp.transform.position, Quaternion.identity);
						}
					}
				}
			}
			if ((i == this.nodes.Count - 1 || i == 1) && this.wrapMode != SerializedPath.SerializedPathWrapMode.Loop)
			{
				TileIndexGrid tileIndexGrid3 = GameManager.Instance.Dungeon.pathGridDefinitions[this.tilesetPathGrid];
				if (i == this.nodes.Count - 1)
				{
					if (intVector.y == intVector2.y && data[intVector2].type != CellType.WALL && data[intVector2 + BraveUtility.GetIntMajorAxis((intVector2 - intVector).ToVector2())].type != CellType.WALL)
					{
						intVector2 += BraveUtility.GetIntMajorAxis((intVector2 - intVector).ToVector2());
					}
				}
				else if (i == 1 && intVector.y == intVector2.y && data[intVector].type != CellType.WALL && data[intVector + BraveUtility.GetIntMajorAxis((intVector - intVector2).ToVector2())].type != CellType.WALL)
				{
					intVector += BraveUtility.GetIntMajorAxis((intVector - intVector2).ToVector2());
				}
				int num = 1;
				if (this.nodes.Count - 1 == 1)
				{
					num = 2;
				}
				for (int l = 0; l < num; l++)
				{
					IntVector2 intVector3 = ((i != 1 || l == 1) ? intVector2 : intVector);
					IntVector2 intVector4 = ((i != 1 || l == 1) ? BraveUtility.GetIntMajorAxis(intVector2 - intVector) : BraveUtility.GetIntMajorAxis(intVector - intVector2));
					if (i == 1 && l != 1)
					{
						intVector3 += intVector4;
					}
					if (data[intVector3] != null && data[intVector3].type == CellType.FLOOR)
					{
						switch (DungeonData.GetDirectionFromIntVector2(intVector4))
						{
						case DungeonData.Direction.NORTH:
							if (tileIndexGrid3.PathStubNorth != null)
							{
								UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid3.PathStubNorth, intVector3.ToVector3(), Quaternion.identity);
							}
							break;
						case DungeonData.Direction.EAST:
							if (tileIndexGrid3.PathStubEast != null)
							{
								UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid3.PathStubEast, intVector3.ToVector3(), Quaternion.identity);
							}
							break;
						case DungeonData.Direction.SOUTH:
							if (tileIndexGrid3.PathStubSouth != null)
							{
								UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid3.PathStubSouth, intVector3.ToVector3(), Quaternion.identity);
							}
							break;
						case DungeonData.Direction.WEST:
							if (tileIndexGrid3.PathStubWest != null)
							{
								UnityEngine.Object.Instantiate<GameObject>(tileIndexGrid3.PathStubWest, intVector3.ToVector3(), Quaternion.identity);
							}
							break;
						}
					}
				}
			}
			IntVector2 majorAxis = (intVector2 - intVector).MajorAxis;
			while (intVector != intVector2)
			{
				data[intVector].cellVisualData.containsObjectSpaceStamp = true;
				data[intVector].cellVisualData.pathTilesetGridIndex = this.tilesetPathGrid;
				data[intVector].cellVisualData.hasStampedPath = true;
				BraveUtility.DrawDebugSquare(intVector.ToVector2(), Color.magenta, 1000f);
				data[intVector].fallingPrevented = true;
				intVector += majorAxis;
			}
		}
	}

	// Token: 0x060054D9 RID: 21721 RVA: 0x002025A8 File Offset: 0x002007A8
	public void ChangeNodePlacement(IntVector2 position)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == position)
			{
				int num = (int)((this.nodes[i].placement + 1) % (SerializedPathNode.SerializedNodePlacement)Enum.GetValues(typeof(SerializedPathNode.SerializedNodePlacement)).Length);
				SerializedPathNode serializedPathNode = this.nodes[i];
				serializedPathNode.placement = (SerializedPathNode.SerializedNodePlacement)num;
				this.nodes[i] = serializedPathNode;
			}
		}
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x00202640 File Offset: 0x00200840
	public void ChangeNodePlacement(IntVector2 position, SerializedPathNode.SerializedNodePlacement placement)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == position)
			{
				SerializedPathNode serializedPathNode = this.nodes[i];
				serializedPathNode.placement = placement;
				this.nodes[i] = serializedPathNode;
			}
		}
	}

	// Token: 0x060054DB RID: 21723 RVA: 0x002026AC File Offset: 0x002008AC
	public SerializedPathNode? GetNodeAtPoint(IntVector2 position, out int index)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == position)
			{
				index = i;
				return new SerializedPathNode?(this.nodes[i]);
			}
		}
		index = -1;
		return null;
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x00202718 File Offset: 0x00200918
	public void AddPosition(IntVector2 position)
	{
		this.nodes.Add(new SerializedPathNode(position));
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x0020272C File Offset: 0x0020092C
	public void AddPosition(IntVector2 position, IntVector2 previousPosition)
	{
		bool flag = false;
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == previousPosition)
			{
				flag = true;
				SerializedPathNode serializedPathNode = new SerializedPathNode(position);
				serializedPathNode.placement = this.nodes[i].placement;
				this.nodes.Insert(i + 1, serializedPathNode);
				break;
			}
		}
		if (!flag)
		{
			this.AddPosition(position);
		}
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x002027C0 File Offset: 0x002009C0
	public bool TranslatePosition(IntVector2 position, IntVector2 translation)
	{
		IntVector2 intVector = position + translation;
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == position)
			{
				num = i;
			}
			if (this.nodes[i].position == intVector)
			{
				num2 = i;
			}
		}
		if (num != -1 && num2 == -1)
		{
			SerializedPathNode serializedPathNode = new SerializedPathNode(intVector);
			serializedPathNode.placement = this.nodes[num].placement;
			serializedPathNode.delayTime = this.nodes[num].delayTime;
			this.nodes[num] = serializedPathNode;
			return true;
		}
		return false;
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x00202898 File Offset: 0x00200A98
	public void RemovePosition(IntVector2 position)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (this.nodes[i].position == position)
			{
				this.nodes.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x04004DCF RID: 19919
	public List<SerializedPathNode> nodes;

	// Token: 0x04004DD0 RID: 19920
	public SerializedPath.SerializedPathWrapMode wrapMode;

	// Token: 0x04004DD1 RID: 19921
	public float overrideSpeed = -1f;

	// Token: 0x04004DD2 RID: 19922
	public int tilesetPathGrid = -1;

	// Token: 0x02000F62 RID: 3938
	public enum SerializedPathWrapMode
	{
		// Token: 0x04004DD4 RID: 19924
		PingPong,
		// Token: 0x04004DD5 RID: 19925
		Loop,
		// Token: 0x04004DD6 RID: 19926
		Once
	}
}
