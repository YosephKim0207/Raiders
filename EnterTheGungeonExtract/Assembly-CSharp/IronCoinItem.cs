using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001423 RID: 5155
public class IronCoinItem : PlayerItem
{
	// Token: 0x060074FF RID: 29951 RVA: 0x002E93F0 File Offset: 0x002E75F0
	protected override void DoEffect(PlayerController user)
	{
		if (this.OnUsedVFX)
		{
			user.PlayEffectOnActor(this.OnUsedVFX, new Vector3(0f, 0.25f, 0f), false, false, false);
		}
		List<RoomHandler> list = new List<RoomHandler>();
		bool flag = UnityEngine.Random.value < this.ChanceToTargetBoss;
		flag = false;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			if (GameManager.Instance.Dungeon.data.rooms[i].visibility == RoomHandler.VisibilityStatus.OBSCURED && GameManager.Instance.Dungeon.data.rooms[i].HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				if (flag)
				{
					if (GameManager.Instance.Dungeon.data.rooms[i].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
					{
						list.Add(GameManager.Instance.Dungeon.data.rooms[i]);
					}
				}
				else if (GameManager.Instance.Dungeon.data.rooms[i].area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS)
				{
					list.Add(GameManager.Instance.Dungeon.data.rooms[i]);
				}
			}
		}
		if (list.Count > 0)
		{
			RoomHandler roomHandler = list[UnityEngine.Random.Range(0, list.Count)];
			user.StartCoroutine(this.SlaughterRoom(roomHandler));
		}
	}

	// Token: 0x06007500 RID: 29952 RVA: 0x002E9588 File Offset: 0x002E7788
	private IEnumerator SlaughterRoom(RoomHandler targetRoom)
	{
		if (targetRoom != null)
		{
			targetRoom.ClearReinforcementLayers();
			List<AIActor> enemies = targetRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (enemies != null)
			{
				List<AIActor> enemiesToKill = new List<AIActor>(enemies);
				for (int i = 0; i < enemiesToKill.Count; i++)
				{
					AIActor aiactor = enemiesToKill[i];
					if (aiactor)
					{
						aiactor.enabled = true;
					}
				}
				yield return null;
				for (int j = 0; j < enemiesToKill.Count; j++)
				{
					AIActor aiactor2 = enemiesToKill[j];
					if (aiactor2)
					{
						UnityEngine.Object.Destroy(aiactor2.gameObject);
					}
				}
			}
			if (this.NotePrefab != null)
			{
				bool flag = false;
				IntVector2 intVector = targetRoom.GetCenteredVisibleClearSpot(3, 3, out flag, true);
				intVector = intVector - targetRoom.area.basePosition + IntVector2.One;
				if (flag)
				{
					GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(this.NotePrefab.gameObject, targetRoom, intVector, false, AIActor.AwakenAnimationType.Default, false);
					if (gameObject)
					{
						IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
						for (int k = 0; k < interfacesInChildren.Length; k++)
						{
							targetRoom.RegisterInteractable(interfacesInChildren[k]);
						}
					}
					if (this.BloodDefinition != null)
					{
						DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.BloodDefinition);
						goopManagerForGoopType.AddGoopCircle(intVector.ToVector2(0.75f, 0.5f) + targetRoom.area.basePosition.ToVector2(), 2f, -1, true, -1);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06007501 RID: 29953 RVA: 0x002E95AC File Offset: 0x002E77AC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040076D7 RID: 30423
	public float ChanceToTargetBoss = 0.01f;

	// Token: 0x040076D8 RID: 30424
	public GameObject OnUsedVFX;

	// Token: 0x040076D9 RID: 30425
	public GameObject NotePrefab;

	// Token: 0x040076DA RID: 30426
	public GoopDefinition BloodDefinition;
}
