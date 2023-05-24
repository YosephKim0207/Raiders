using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001280 RID: 4736
public class HammerTimeChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A0D RID: 27149 RVA: 0x0029901C File Offset: 0x0029721C
	public override bool IsValid(RoomHandler room)
	{
		GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
		return tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON;
	}

	// Token: 0x06006A0E RID: 27150 RVA: 0x00299054 File Offset: 0x00297254
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		GameObject gameObject = this.HammerPlaceable.InstantiateObject(currentRoom, currentRoom.Epicenter, false, false);
		if (gameObject)
		{
			ForgeHammerController component = gameObject.GetComponent<ForgeHammerController>();
			if (component)
			{
				component.MinTimeBetweenAttacks = this.MinTimeBetweenAttacks;
				component.MaxTimeBetweenAttacks = this.MaxTimeBetweenAttacks;
			}
		}
	}

	// Token: 0x04006685 RID: 26245
	public DungeonPlaceable HammerPlaceable;

	// Token: 0x04006686 RID: 26246
	public float MinTimeBetweenAttacks = 3f;

	// Token: 0x04006687 RID: 26247
	public float MaxTimeBetweenAttacks = 3f;
}
