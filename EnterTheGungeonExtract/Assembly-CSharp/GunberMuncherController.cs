using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001183 RID: 4483
public class GunberMuncherController : BraveBehaviour
{
	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x0600638C RID: 25484 RVA: 0x00269CE0 File Offset: 0x00267EE0
	// (set) Token: 0x0600638D RID: 25485 RVA: 0x00269CE8 File Offset: 0x00267EE8
	public bool ShouldGiveReward { get; set; }

	// Token: 0x0600638E RID: 25486 RVA: 0x00269CF4 File Offset: 0x00267EF4
	private void Start()
	{
		if (this.RequiredNumberOfGuns > 2)
		{
			this.m_gunsTossed = (int)GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.GUNBERS_EVIL_MUNCHED);
		}
		Minimap.Instance.RegisterRoomIcon(base.transform.position.GetAbsoluteRoom(), (GameObject)ResourceCache.Acquire("Global Prefabs/Minimap_Muncher_Icon"), false);
	}

	// Token: 0x0600638F RID: 25487 RVA: 0x00269D4C File Offset: 0x00267F4C
	public IEnumerator DoReward(PlayerController player)
	{
		yield return new WaitForSeconds(0.6f);
		if ((this.m_first != null && this.m_second != null) || this.m_gunsTossed >= this.RequiredNumberOfGuns)
		{
			GameObject itemForPlayer = this.GetItemForPlayer(player);
			tk2dBaseSprite component = itemForPlayer.GetComponent<tk2dBaseSprite>();
			Vector2 vector = Vector2.zero;
			if (component != null)
			{
				vector = -1f * component.GetBounds().center.XY();
			}
			DebrisObject debrisObject = LootEngine.SpawnItem(itemForPlayer, base.sprite.WorldCenter + vector, Vector2.down, 20f, true, false, false);
			debrisObject.bounceCount = 0;
			DebrisObject debrisObject2 = debrisObject;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.DoSteamOnGrounded));
		}
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.GUNBERS_MUNCHED, 1f);
		if (this.RequiredNumberOfGuns > 2)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.MUNCHER_EVIL_COMPLETE, true);
		}
		yield return new WaitForSeconds(2.4f);
		if (!this.CanBeReused)
		{
			PlayMakerFSM component2 = base.GetComponent<PlayMakerFSM>();
			component2.FsmVariables.FindFsmBool("canBeUsed").Value = false;
		}
		this.m_first = null;
		this.m_second = null;
		yield break;
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x00269D70 File Offset: 0x00267F70
	private void DoSteamOnGrounded(DebrisObject obj)
	{
		SpawnManager.SpawnVFX(this.PoopSteamPrefab, obj.sprite.WorldCenter.ToVector3ZUp(obj.sprite.WorldCenter.y - 1f), Quaternion.identity);
	}

	// Token: 0x06006391 RID: 25489 RVA: 0x00269DB8 File Offset: 0x00267FB8
	protected GameObject GetRecipeItem()
	{
		PickupObject pickupObject = null;
		for (int i = 0; i < this.DefinedRecipes.Count; i++)
		{
			if (this.DefinedRecipes[i].gunIDs_A.Contains(this.m_first.PickupObjectId) && this.DefinedRecipes[i].gunIDs_B.Contains(this.m_second.PickupObjectId))
			{
				pickupObject = PickupObjectDatabase.GetById(this.DefinedRecipes[i].resultID);
				if (pickupObject != null && this.DefinedRecipes[i].flagToSet != GungeonFlags.NONE)
				{
					GameStatsManager.Instance.SetFlag(this.DefinedRecipes[i].flagToSet, true);
				}
				break;
			}
			if (this.DefinedRecipes[i].gunIDs_A.Contains(this.m_second.PickupObjectId) && this.DefinedRecipes[i].gunIDs_B.Contains(this.m_first.PickupObjectId))
			{
				pickupObject = PickupObjectDatabase.GetById(this.DefinedRecipes[i].resultID);
				if (pickupObject != null && this.DefinedRecipes[i].flagToSet != GungeonFlags.NONE)
				{
					GameStatsManager.Instance.SetFlag(this.DefinedRecipes[i].flagToSet, true);
				}
				break;
			}
		}
		return (!(pickupObject != null)) ? null : pickupObject.gameObject;
	}

	// Token: 0x06006392 RID: 25490 RVA: 0x00269F48 File Offset: 0x00268148
	protected GameObject GetItemForPlayer(PlayerController player)
	{
		if (this.RequiredNumberOfGuns > 2 && !GameStatsManager.Instance.GetFlag(GungeonFlags.MUNCHER_EVIL_COMPLETE))
		{
			return PickupObjectDatabase.GetById(this.evilMuncherReward).gameObject;
		}
		if (this.m_first != null && this.m_second != null)
		{
			GameObject recipeItem = this.GetRecipeItem();
			if (recipeItem != null)
			{
				return recipeItem;
			}
		}
		PickupObject.ItemQuality itemQuality = this.DetermineQualityToSpawn();
		itemQuality = (PickupObject.ItemQuality)Mathf.Min(5, Mathf.Max(0, (int)itemQuality));
		bool flag = false;
		while (itemQuality >= PickupObject.ItemQuality.COMMON)
		{
			if (itemQuality > PickupObject.ItemQuality.COMMON)
			{
				flag = true;
			}
			List<WeightedGameObject> compiledRawItems = this.LootTable.GetCompiledRawItems();
			List<KeyValuePair<WeightedGameObject, float>> list = new List<KeyValuePair<WeightedGameObject, float>>();
			float num = 0f;
			List<KeyValuePair<WeightedGameObject, float>> list2 = new List<KeyValuePair<WeightedGameObject, float>>();
			float num2 = 0f;
			for (int i = 0; i < compiledRawItems.Count; i++)
			{
				if (compiledRawItems[i].gameObject != null)
				{
					PickupObject component = compiledRawItems[i].gameObject.GetComponent<PickupObject>();
					bool flag2 = component.quality == itemQuality;
					if (component != null && flag2)
					{
						bool flag3 = true;
						float weight = compiledRawItems[i].weight;
						if (!component.PrerequisitesMet())
						{
							flag3 = false;
						}
						if (!component.CanActuallyBeDropped(player))
						{
							flag3 = false;
						}
						EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
						if (component2 != null)
						{
							int num3 = 0;
							if (Application.isPlaying)
							{
								num3 = GameStatsManager.Instance.QueryEncounterableDifferentiator(component2);
							}
							if (num3 > 0 || (Application.isPlaying && GameManager.Instance.ExtantShopTrackableGuids.Contains(component2.EncounterGuid)))
							{
								flag3 = false;
								num2 += weight;
								KeyValuePair<WeightedGameObject, float> keyValuePair = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], weight);
								list2.Add(keyValuePair);
							}
						}
						if (flag3)
						{
							num += weight;
							KeyValuePair<WeightedGameObject, float> keyValuePair2 = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], weight);
							list.Add(keyValuePair2);
						}
					}
				}
			}
			if (list.Count == 0 && list2.Count > 0)
			{
				list = list2;
				num = num2;
			}
			if (num > 0f && list.Count > 0)
			{
				float num4 = num * UnityEngine.Random.value;
				for (int j = 0; j < list.Count; j++)
				{
					num4 -= list[j].Value;
					if (num4 <= 0f)
					{
						return list[j].Key.gameObject;
					}
				}
				return list[list.Count - 1].Key.gameObject;
			}
			itemQuality--;
			if (itemQuality < PickupObject.ItemQuality.COMMON && !flag)
			{
				itemQuality = PickupObject.ItemQuality.D;
			}
		}
		Debug.LogError("Failed to get any item at all.");
		return null;
	}

	// Token: 0x06006393 RID: 25491 RVA: 0x0026A22C File Offset: 0x0026842C
	private PickupObject.ItemQuality DetermineQualityToSpawn()
	{
		if (this.m_first == null && this.m_gunsTossed >= this.RequiredNumberOfGuns)
		{
			return (UnityEngine.Random.value <= 0.25f) ? PickupObject.ItemQuality.S : PickupObject.ItemQuality.A;
		}
		if (!(this.m_first == null) && !(this.m_second == null))
		{
			int quality = (int)this.m_first.quality;
			int quality2 = (int)this.m_second.quality;
			int num = Mathf.Min(quality, quality2);
			int num2 = Mathf.Max(quality, quality2);
			bool flag = num2 < 5;
			int num3 = num2 - num + 1;
			float num4 = 1f / (float)num3;
			float value = UnityEngine.Random.value;
			float num5 = 0f;
			int num6 = -1;
			for (int i = num; i <= num2; i++)
			{
				num5 += num4;
				float num7 = this.QualityDistribution.Evaluate(num5);
				if (num7 > value)
				{
					num6 = i;
					break;
				}
			}
			if (num6 == -1)
			{
				num6 = num2;
			}
			if (flag && UnityEngine.Random.value > 0.95f)
			{
				num6 = Mathf.Min(num6 + 1, 5);
			}
			return (PickupObject.ItemQuality)num6;
		}
		Debug.LogError("Problem of type 2 in Gunber Muncher!");
		if (this.m_first != null)
		{
			return this.m_first.quality;
		}
		if (this.m_second != null)
		{
			return this.m_second.quality;
		}
		return PickupObject.ItemQuality.C;
	}

	// Token: 0x06006394 RID: 25492 RVA: 0x0026A398 File Offset: 0x00268598
	public void TossPlayerEquippedGun(PlayerController player)
	{
		if (player.CurrentGun != null && player.CurrentGun.CanActuallyBeDropped(player))
		{
			Gun currentGun = player.CurrentGun;
			if (this.RequiredNumberOfGuns == 2)
			{
				if (this.m_first == null)
				{
					this.m_first = PickupObjectDatabase.Instance.InternalGetById(currentGun.PickupObjectId) as Gun;
				}
				else if (this.m_second == null)
				{
					this.m_second = PickupObjectDatabase.Instance.InternalGetById(currentGun.PickupObjectId) as Gun;
				}
				else
				{
					Debug.LogError("GUNBER MUNCHER FAIL TYPE 1");
				}
			}
			else
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.GUNBERS_EVIL_MUNCHED, 1f);
				this.m_gunsTossed++;
			}
			this.TossObjectIntoPot(player, currentGun.GetSprite(), player.CenterPosition);
			player.inventory.DestroyCurrentGun();
		}
	}

	// Token: 0x06006395 RID: 25493 RVA: 0x0026A490 File Offset: 0x00268690
	public void TossObjectIntoPot(PlayerController player, tk2dBaseSprite spriteSource, Vector3 startPosition)
	{
		base.StartCoroutine(this.HandleObjectPotToss(player, spriteSource, startPosition));
	}

	// Token: 0x06006396 RID: 25494 RVA: 0x0026A4A4 File Offset: 0x002686A4
	private IEnumerator HandleObjectPotToss(PlayerController player, tk2dBaseSprite spriteSource, Vector3 startPosition)
	{
		this.IsProcessing = true;
		this.ShouldGiveReward = false;
		base.aiAnimator.PlayUntilFinished("activate", false, null, -1f, false);
		yield return new WaitForSeconds(0.4f);
		GameObject fakeObject = new GameObject("cauldron temp object");
		tk2dSprite sprite = tk2dBaseSprite.AddComponent<tk2dSprite>(fakeObject, spriteSource.Collection, spriteSource.spriteId);
		sprite.HeightOffGround = 2f;
		sprite.PlaceAtPositionByAnchor(startPosition, tk2dBaseSprite.Anchor.MiddleCenter);
		Vector3 endPosition = base.sprite.WorldCenter.ToVector3ZUp(0f);
		float duration = 0.5f;
		float elapsed = 0f;
		while (elapsed < duration && base.spriteAnimator.CurrentFrame != 8)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector3 targetPosition = Vector3.Lerp(startPosition, endPosition, t);
			sprite.PlaceAtPositionByAnchor(targetPosition, tk2dBaseSprite.Anchor.MiddleCenter);
			sprite.UpdateZDepth();
			yield return null;
		}
		UnityEngine.Object.Destroy(fakeObject);
		yield return new WaitForSeconds(0.25f);
		if (this.RequiredNumberOfGuns > 2)
		{
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.MUNCHER_EVIL_COMPLETE))
			{
				float value = UnityEngine.Random.value;
				Debug.Log("evil chance! " + value);
				if (value < this.evilMuncherPostRewardChance)
				{
					this.ShouldGiveReward = true;
				}
			}
			else if ((this.m_first != null && this.m_second != null) || this.m_gunsTossed >= this.RequiredNumberOfGuns)
			{
				this.ShouldGiveReward = true;
			}
		}
		else if (this.m_first != null && this.m_second != null)
		{
			this.ShouldGiveReward = true;
		}
		if (this.ShouldGiveReward)
		{
			base.talkDoer.IsInteractable = false;
			yield return new WaitForSeconds(4.75f);
			this.IsProcessing = false;
			yield return base.StartCoroutine(this.DoReward(player));
			base.talkDoer.IsInteractable = true;
		}
		else
		{
			base.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
			this.IsProcessing = false;
		}
		yield break;
	}

	// Token: 0x04005F13 RID: 24339
	public int RequiredNumberOfGuns = 2;

	// Token: 0x04005F14 RID: 24340
	public GenericLootTable LootTable;

	// Token: 0x04005F15 RID: 24341
	public List<GunberMuncherRecipe> DefinedRecipes;

	// Token: 0x04005F16 RID: 24342
	public AnimationCurve QualityDistribution;

	// Token: 0x04005F17 RID: 24343
	public bool IsProcessing;

	// Token: 0x04005F18 RID: 24344
	public bool CanBeReused;

	// Token: 0x04005F19 RID: 24345
	[PickupIdentifier]
	public int evilMuncherReward;

	// Token: 0x04005F1A RID: 24346
	public float evilMuncherPostRewardChance = 0.07f;

	// Token: 0x04005F1B RID: 24347
	public GameObject PoopSteamPrefab;

	// Token: 0x04005F1D RID: 24349
	[NonSerialized]
	private Gun m_first;

	// Token: 0x04005F1E RID: 24350
	[NonSerialized]
	private Gun m_second;

	// Token: 0x04005F1F RID: 24351
	[NonSerialized]
	private int m_gunsTossed;
}
