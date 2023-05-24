using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001299 RID: 4761
public class FuturegeonGroundLightDoer : MonoBehaviour
{
	// Token: 0x06006A8B RID: 27275 RVA: 0x0029C4D8 File Offset: 0x0029A6D8
	private void Start()
	{
		base.StartCoroutine(this.HandleLightningTrails());
	}

	// Token: 0x06006A8C RID: 27276 RVA: 0x0029C4E8 File Offset: 0x0029A6E8
	private IEnumerator HandleAllLightTrails()
	{
		for (;;)
		{
			if (this.numActiveLightTrails < this.maxActiveLightTrails)
			{
				base.StartCoroutine(this.HandleSingleLightTrail());
			}
			yield return new WaitForSeconds(1.5f);
		}
		yield break;
	}

	// Token: 0x06006A8D RID: 27277 RVA: 0x0029C504 File Offset: 0x0029A704
	private IEnumerator HandleLightningTrails()
	{
		for (;;)
		{
			if (Dungeon.IsGenerating || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
			{
				yield return new WaitForSeconds(1f);
			}
			else
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController && !playerController.IsGhost)
					{
						RoomHandler currentRoom = playerController.CurrentRoom;
						if (currentRoom != null && (currentRoom.RoomVisualSubtype == 7 || currentRoom.RoomVisualSubtype == 8))
						{
							base.StartCoroutine(this.HandleLightingLightTrail(DungeonData.GetRandomCardinalDirection(), playerController.CenterPosition.ToIntVector2(VectorConversions.Floor), false));
						}
					}
				}
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
			}
		}
		yield break;
	}

	// Token: 0x06006A8E RID: 27278 RVA: 0x0029C520 File Offset: 0x0029A720
	private IEnumerator HandleLightingLightTrail(DungeonData.Direction startDir, IntVector2 startPos, bool isBranch = false)
	{
		bool isAlive = true;
		IntVector2 currentCellPosition = startPos;
		DungeonData.Direction currentDirection = startDir;
		GameObject currentVFX = this.InstantiateVFXFromDirection(currentDirection);
		currentVFX.transform.position = currentCellPosition.ToVector3((float)currentCellPosition.y + 1.5f);
		int numIterations = 0;
		int tilesSinceTurn = 0;
		float currentNextTimer = ((!isBranch) ? 0.25f : 0f);
		while (isAlive)
		{
			if (currentNextTimer <= 0f)
			{
				tilesSinceTurn++;
				currentCellPosition += DungeonData.GetIntVector2FromDirection(currentDirection);
				CellData cellData = GameManager.Instance.Dungeon.data[currentCellPosition];
				if (cellData == null || cellData.type == CellType.WALL || cellData.type == CellType.PIT)
				{
					isAlive = false;
					break;
				}
				if (UnityEngine.Random.value > 0.75f && tilesSinceTurn >= 2)
				{
					tilesSinceTurn = 0;
					switch (currentDirection)
					{
					case DungeonData.Direction.NORTH:
						if (UnityEngine.Random.value > 0.95f && numIterations > 3)
						{
							base.StartCoroutine(this.HandleLightingLightTrail(DungeonData.Direction.WEST, currentCellPosition, true));
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right;
						}
						else if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right;
						}
						else
						{
							currentDirection = DungeonData.Direction.WEST;
						}
						break;
					case DungeonData.Direction.EAST:
						if (UnityEngine.Random.value > 0.95f && numIterations > 3)
						{
							base.StartCoroutine(this.HandleLightingLightTrail(DungeonData.Direction.SOUTH, currentCellPosition + IntVector2.Down + IntVector2.Left, true));
							currentDirection = DungeonData.Direction.NORTH;
							currentCellPosition += IntVector2.Left;
						}
						else if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.NORTH;
							currentCellPosition += IntVector2.Left;
						}
						else
						{
							currentDirection = DungeonData.Direction.SOUTH;
							currentCellPosition += IntVector2.Down + IntVector2.Left;
						}
						break;
					case DungeonData.Direction.SOUTH:
						if (UnityEngine.Random.value > 0.95f && numIterations > 3)
						{
							base.StartCoroutine(this.HandleLightingLightTrail(DungeonData.Direction.WEST, currentCellPosition + IntVector2.Up, true));
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right + IntVector2.Up;
						}
						else if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right + IntVector2.Up;
						}
						else
						{
							currentDirection = DungeonData.Direction.WEST;
							currentCellPosition += IntVector2.Up;
						}
						break;
					case DungeonData.Direction.WEST:
						if (UnityEngine.Random.value > 0.95f && numIterations > 3)
						{
							base.StartCoroutine(this.HandleLightingLightTrail(DungeonData.Direction.SOUTH, currentCellPosition + IntVector2.Down, true));
							currentDirection = DungeonData.Direction.NORTH;
						}
						else if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.NORTH;
						}
						else
						{
							currentDirection = DungeonData.Direction.SOUTH;
							currentCellPosition += IntVector2.Down;
						}
						break;
					}
				}
				currentVFX = this.InstantiateVFXFromDirection(currentDirection);
				currentVFX.transform.position = currentCellPosition.ToVector3((float)currentCellPosition.y + 1.5f);
				numIterations++;
			}
			else
			{
				currentNextTimer -= BraveTime.DeltaTime;
			}
			if (numIterations > 200)
			{
				isAlive = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006A8F RID: 27279 RVA: 0x0029C550 File Offset: 0x0029A750
	private GameObject InstantiateVFXFromDirection(DungeonData.Direction dir)
	{
		GameObject gameObject = null;
		if (dir == DungeonData.Direction.NORTH)
		{
			gameObject = this.lightNorthVFX;
		}
		else if (dir == DungeonData.Direction.EAST)
		{
			gameObject = this.lightEastVFX;
		}
		else if (dir == DungeonData.Direction.SOUTH)
		{
			gameObject = this.lightSouthVFX;
		}
		else if (dir == DungeonData.Direction.WEST)
		{
			gameObject = this.lightWestVFX;
		}
		return SpawnManager.SpawnVFX(gameObject, false);
	}

	// Token: 0x06006A90 RID: 27280 RVA: 0x0029C5AC File Offset: 0x0029A7AC
	private IEnumerator HandleSingleLightTrail()
	{
		this.numActiveLightTrails++;
		bool isAlive = true;
		IntVector2 currentCellPosition = GameManager.Instance.PrimaryPlayer.CenterPosition.ToIntVector2(VectorConversions.Floor);
		DungeonData.Direction currentDirection = DungeonData.GetRandomCardinalDirection();
		GameObject currentVFX = this.InstantiateVFXFromDirection(currentDirection);
		currentVFX.transform.position = currentCellPosition.ToVector3((float)currentCellPosition.y + 1.5f);
		int numIterations = 0;
		float currentNextTimer = 0.25f;
		while (isAlive)
		{
			if (currentNextTimer <= 0f)
			{
				currentCellPosition += DungeonData.GetIntVector2FromDirection(currentDirection);
				CellData cellData = GameManager.Instance.Dungeon.data[currentCellPosition];
				if (cellData == null || cellData.type == CellType.WALL || cellData.type == CellType.PIT)
				{
					isAlive = false;
					break;
				}
				if (UnityEngine.Random.value > 0.5f)
				{
					switch (currentDirection)
					{
					case DungeonData.Direction.NORTH:
						if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right;
						}
						else
						{
							currentDirection = DungeonData.Direction.WEST;
						}
						break;
					case DungeonData.Direction.EAST:
						if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.NORTH;
							currentCellPosition += IntVector2.Left;
						}
						else
						{
							currentDirection = DungeonData.Direction.SOUTH;
							currentCellPosition += IntVector2.Down + IntVector2.Left;
						}
						break;
					case DungeonData.Direction.SOUTH:
						if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.EAST;
							currentCellPosition += IntVector2.Right + IntVector2.Up;
						}
						else
						{
							currentDirection = DungeonData.Direction.WEST;
							currentCellPosition += IntVector2.Up;
						}
						break;
					case DungeonData.Direction.WEST:
						if (UnityEngine.Random.value > 0.5f)
						{
							currentDirection = DungeonData.Direction.NORTH;
						}
						else
						{
							currentDirection = DungeonData.Direction.SOUTH;
							currentCellPosition += IntVector2.Down;
						}
						break;
					}
				}
				currentVFX = this.InstantiateVFXFromDirection(currentDirection);
				currentVFX.transform.position = currentCellPosition.ToVector3((float)currentCellPosition.y + 1.5f);
				numIterations++;
				currentNextTimer = 0.25f;
			}
			else
			{
				currentNextTimer -= BraveTime.DeltaTime;
			}
			if (numIterations > 20)
			{
				isAlive = false;
			}
			yield return null;
		}
		this.numActiveLightTrails--;
		yield break;
	}

	// Token: 0x0400670A RID: 26378
	public GameObject lightNorthVFX;

	// Token: 0x0400670B RID: 26379
	public GameObject lightEastVFX;

	// Token: 0x0400670C RID: 26380
	public GameObject lightSouthVFX;

	// Token: 0x0400670D RID: 26381
	public GameObject lightWestVFX;

	// Token: 0x0400670E RID: 26382
	public int maxActiveLightTrails = 5;

	// Token: 0x0400670F RID: 26383
	private int numActiveLightTrails;
}
