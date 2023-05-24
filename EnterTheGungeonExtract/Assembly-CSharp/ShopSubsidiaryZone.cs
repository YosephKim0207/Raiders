using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001559 RID: 5465
public class ShopSubsidiaryZone : MonoBehaviour
{
	// Token: 0x06007D3D RID: 32061 RVA: 0x00329AE4 File Offset: 0x00327CE4
	public void HandleSetup(ShopController controller, RoomHandler room, List<GameObject> shopItemObjects, List<ShopItemController> shopItemControllers)
	{
		int count = shopItemObjects.Count;
		for (int i = 0; i < this.spawnPositions.Length; i++)
		{
			if (this.IsShopRoundTable && i == 0 && (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON))
			{
				shopItemObjects.Add(this.shopItems.defaultItemDrops.elements[0].gameObject);
			}
			else
			{
				GameObject gameObject = this.shopItems.SelectByWeightWithoutDuplicatesFullPrereqs(shopItemObjects, true, false);
				shopItemObjects.Add(gameObject);
			}
		}
		bool flag = false;
		for (int j = 0; j < this.spawnPositions.Length; j++)
		{
			if (!(shopItemObjects[count + j] == null))
			{
				flag = true;
				Transform transform = this.spawnPositions[j];
				PickupObject component = shopItemObjects[count + j].GetComponent<PickupObject>();
				if (!(component == null))
				{
					GameObject gameObject2 = new GameObject("Shop item " + j.ToString());
					Transform transform2 = gameObject2.transform;
					transform2.parent = transform;
					transform2.localPosition = Vector3.zero;
					EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
					if (component2 != null)
					{
						GameManager.Instance.ExtantShopTrackableGuids.Add(component2.EncounterGuid);
					}
					ShopItemController shopItemController = gameObject2.AddComponent<ShopItemController>();
					shopItemController.PrecludeAllDiscounts = this.PrecludeAllDiscounts;
					if (transform.name.Contains("SIDE") || transform.name.Contains("EAST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.EAST;
					}
					else if (transform.name.Contains("WEST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.WEST;
					}
					else if (transform.name.Contains("NORTH"))
					{
						shopItemController.itemFacing = DungeonData.Direction.NORTH;
					}
					if (!room.IsRegistered(shopItemController))
					{
						room.RegisterInteractable(shopItemController);
					}
					shopItemController.Initialize(component, controller);
					shopItemControllers.Add(shopItemController);
				}
			}
		}
		if (!flag)
		{
			SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				componentsInChildren[k].enabled = false;
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06007D3E RID: 32062 RVA: 0x00329D5C File Offset: 0x00327F5C
	public void HandleSetup(BaseShopController controller, RoomHandler room, List<GameObject> shopItemObjects, List<ShopItemController> shopItemControllers)
	{
		int count = shopItemObjects.Count;
		for (int i = 0; i < this.spawnPositions.Length; i++)
		{
			GameObject gameObject = this.shopItems.SelectByWeightWithoutDuplicatesFullPrereqs(shopItemObjects, true, false);
			shopItemObjects.Add(gameObject);
		}
		bool flag = false;
		for (int j = 0; j < this.spawnPositions.Length; j++)
		{
			if (!(shopItemObjects[count + j] == null))
			{
				flag = true;
				Transform transform = this.spawnPositions[j];
				PickupObject component = shopItemObjects[count + j].GetComponent<PickupObject>();
				if (!(component == null))
				{
					GameObject gameObject2 = new GameObject("Shop item " + j.ToString());
					Transform transform2 = gameObject2.transform;
					transform2.parent = transform;
					transform2.localPosition = Vector3.zero;
					EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
					if (component2 != null)
					{
						GameManager.Instance.ExtantShopTrackableGuids.Add(component2.EncounterGuid);
					}
					ShopItemController shopItemController = gameObject2.AddComponent<ShopItemController>();
					shopItemController.PrecludeAllDiscounts = this.PrecludeAllDiscounts;
					if (transform.name.Contains("SIDE") || transform.name.Contains("EAST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.EAST;
					}
					else if (transform.name.Contains("WEST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.WEST;
					}
					else if (transform.name.Contains("NORTH"))
					{
						shopItemController.itemFacing = DungeonData.Direction.NORTH;
					}
					if (!room.IsRegistered(shopItemController))
					{
						room.RegisterInteractable(shopItemController);
					}
					shopItemController.Initialize(component, controller);
					shopItemControllers.Add(shopItemController);
				}
			}
		}
		if (!flag)
		{
			SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				componentsInChildren[k].enabled = false;
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04008049 RID: 32841
	public GenericLootTable shopItems;

	// Token: 0x0400804A RID: 32842
	public Transform[] spawnPositions;

	// Token: 0x0400804B RID: 32843
	public GameObject shopItemShadowPrefab;

	// Token: 0x0400804C RID: 32844
	public bool IsShopRoundTable;

	// Token: 0x0400804D RID: 32845
	public bool PrecludeAllDiscounts;
}
