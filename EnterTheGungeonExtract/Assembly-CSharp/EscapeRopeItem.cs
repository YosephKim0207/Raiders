using System;
using Dungeonator;

// Token: 0x020013FA RID: 5114
public class EscapeRopeItem : PlayerItem
{
	// Token: 0x06007410 RID: 29712 RVA: 0x002E30F0 File Offset: 0x002E12F0
	public override bool CanBeUsed(PlayerController user)
	{
		return user && user.CurrentRoom != null && (user.CurrentRoom.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS || GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON) && !user.CurrentRoom.CompletelyPreventLeaving && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.RATGEON && (!user.CurrentRoom.IsWildWestEntrance || true);
	}

	// Token: 0x06007411 RID: 29713 RVA: 0x002E3190 File Offset: 0x002E1390
	protected override void DoEffect(PlayerController user)
	{
		if (user.CurrentRoom.CompletelyPreventLeaving)
		{
			return;
		}
		if (user.IsInMinecart)
		{
			user.currentMineCart.EvacuateSpecificPlayer(user, true);
		}
		AkSoundEngine.PostEvent("Play_OBJ_rope_escape_01", base.gameObject);
		if (!user.CurrentRoom.IsWildWestEntrance)
		{
			RoomHandler roomHandler = null;
			BaseShopController[] componentsInChildren = GameManager.Instance.Dungeon.data.Entrance.hierarchyParent.parent.GetComponentsInChildren<BaseShopController>(true);
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				roomHandler = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(componentsInChildren[0].transform.position.IntXY(VectorConversions.Round));
			}
			user.EscapeRoom(PlayerController.EscapeSealedRoomStyle.ESCAPE_SPIN, true, roomHandler);
		}
	}

	// Token: 0x06007412 RID: 29714 RVA: 0x002E3254 File Offset: 0x002E1454
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
