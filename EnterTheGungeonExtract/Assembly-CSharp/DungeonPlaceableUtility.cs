using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E77 RID: 3703
public static class DungeonPlaceableUtility
{
	// Token: 0x06004EC6 RID: 20166 RVA: 0x001B3B90 File Offset: 0x001B1D90
	public static GameObject InstantiateDungeonPlaceable(GameObject objectToInstantiate, RoomHandler targetRoom, IntVector2 location, bool deferConfiguration, AIActor.AwakenAnimationType awakenAnimType = AIActor.AwakenAnimationType.Default, bool autoEngage = false)
	{
		if (objectToInstantiate != null)
		{
			Vector3 vector = location.ToVector3(0f) + targetRoom.area.basePosition.ToVector3();
			vector.z = vector.y + vector.z;
			AIActor aiactor = objectToInstantiate.GetComponent<AIActor>();
			if (aiactor is AIActorDummy)
			{
				objectToInstantiate = (aiactor as AIActorDummy).realPrefab;
				aiactor = objectToInstantiate.GetComponent<AIActor>();
			}
			SpeculativeRigidbody component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();
			if (aiactor && component)
			{
				PixelCollider pixelCollider = component.GetPixelCollider(ColliderType.Ground);
				if (pixelCollider.ColliderGenerationMode != PixelCollider.PixelColliderGeneration.Manual)
				{
					Debug.LogErrorFormat("Trying to spawn an AIActor who doesn't have a manual ground collider... do we still do this? Name: {0}", new object[] { objectToInstantiate.name });
				}
				Vector2 vector2 = PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualOffsetX, pixelCollider.ManualOffsetY));
				Vector2 vector3 = PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualWidth, pixelCollider.ManualHeight));
				Vector2 vector4 = new Vector2((float)Mathf.CeilToInt(vector3.x), (float)Mathf.CeilToInt(vector3.y));
				Vector2 vector5 = new Vector2((vector4.x - vector3.x) / 2f, 0f).Quantize(0.0625f);
				vector -= vector2 - vector5;
			}
			if (aiactor)
			{
				aiactor.AwakenAnimType = awakenAnimType;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(objectToInstantiate, vector, Quaternion.identity);
			if (!deferConfiguration)
			{
				Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(IPlaceConfigurable));
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					IPlaceConfigurable placeConfigurable = componentsInChildren[i] as IPlaceConfigurable;
					if (placeConfigurable != null)
					{
						placeConfigurable.ConfigureOnPlacement(targetRoom);
					}
				}
			}
			ObjectVisibilityManager component2 = gameObject.GetComponent<ObjectVisibilityManager>();
			if (component2 != null)
			{
				component2.Initialize(targetRoom, autoEngage);
			}
			MinorBreakable componentInChildren = gameObject.GetComponentInChildren<MinorBreakable>();
			if (componentInChildren != null)
			{
				IntVector2 intVector = location + targetRoom.area.basePosition;
				CellData cellData = GameManager.Instance.Dungeon.data[intVector];
				if (cellData != null)
				{
					cellData.cellVisualData.containsObjectSpaceStamp = true;
				}
			}
			PlayerItem component3 = gameObject.GetComponent<PlayerItem>();
			if (component3 != null)
			{
				component3.ForceAsExtant = true;
			}
			return gameObject;
		}
		return null;
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001B3DE8 File Offset: 0x001B1FE8
	public static GameObject InstantiateDungeonPlaceableOnlyActors(GameObject objectToInstantiate, RoomHandler targetRoom, IntVector2 location, bool deferConfiguration)
	{
		bool flag = objectToInstantiate.GetComponent<AIActor>();
		bool flag2 = GameManager.Instance.InTutorial && objectToInstantiate.GetComponent<TalkDoerLite>();
		bool flag3 = objectToInstantiate.GetComponent<GenericIntroDoer>();
		if (!flag && !flag2 && !flag3)
		{
			return null;
		}
		GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(objectToInstantiate, targetRoom, location, deferConfiguration, AIActor.AwakenAnimationType.Default, false);
		AIActor component = gameObject.GetComponent<AIActor>();
		if (component)
		{
			component.CanDropCurrency = false;
			component.CanDropItems = false;
		}
		return gameObject;
	}
}
