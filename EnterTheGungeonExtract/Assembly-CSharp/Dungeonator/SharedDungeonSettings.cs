using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F63 RID: 3939
	public class SharedDungeonSettings : MonoBehaviour
	{
		// Token: 0x060054E1 RID: 21729 RVA: 0x0020295C File Offset: 0x00200B5C
		public List<GameObject> GetCurrencyToDrop(int amountToDrop, bool isMetaCurrency = false, bool randomAmounts = false)
		{
			List<GameObject> list = new List<GameObject>();
			int currencyValue = this.currencyDropSettings.goldCoinPrefab.GetComponent<CurrencyPickup>().currencyValue;
			int currencyValue2 = this.currencyDropSettings.silverCoinPrefab.GetComponent<CurrencyPickup>().currencyValue;
			int currencyValue3 = this.currencyDropSettings.bronzeCoinPrefab.GetComponent<CurrencyPickup>().currencyValue;
			int num = 1;
			while (amountToDrop > 0)
			{
				GameObject gameObject;
				if (isMetaCurrency)
				{
					amountToDrop -= num;
					gameObject = this.currencyDropSettings.metaCoinPrefab;
				}
				else if (randomAmounts)
				{
					if (amountToDrop >= currencyValue)
					{
						float value = UnityEngine.Random.value;
						if (value < 0.05f)
						{
							amountToDrop -= currencyValue;
							gameObject = this.currencyDropSettings.goldCoinPrefab;
						}
						else if (value < 0.25f)
						{
							amountToDrop -= currencyValue2;
							gameObject = this.currencyDropSettings.silverCoinPrefab;
						}
						else
						{
							amountToDrop -= currencyValue3;
							gameObject = this.currencyDropSettings.bronzeCoinPrefab;
						}
					}
					else if (amountToDrop >= currencyValue2)
					{
						if (UnityEngine.Random.value < 0.25f)
						{
							amountToDrop -= currencyValue2;
							gameObject = this.currencyDropSettings.silverCoinPrefab;
						}
						else
						{
							amountToDrop -= currencyValue3;
							gameObject = this.currencyDropSettings.bronzeCoinPrefab;
						}
					}
					else
					{
						if (amountToDrop < currencyValue3)
						{
							amountToDrop = 0;
							break;
						}
						amountToDrop -= currencyValue3;
						gameObject = this.currencyDropSettings.bronzeCoinPrefab;
					}
				}
				else if (amountToDrop >= currencyValue)
				{
					amountToDrop -= currencyValue;
					gameObject = this.currencyDropSettings.goldCoinPrefab;
				}
				else if (amountToDrop >= currencyValue2)
				{
					amountToDrop -= currencyValue2;
					gameObject = this.currencyDropSettings.silverCoinPrefab;
				}
				else
				{
					if (amountToDrop < currencyValue3)
					{
						amountToDrop = 0;
						break;
					}
					amountToDrop -= currencyValue3;
					gameObject = this.currencyDropSettings.bronzeCoinPrefab;
				}
				if (gameObject != null)
				{
					list.Add(gameObject);
				}
			}
			return list;
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x00202B3C File Offset: 0x00200D3C
		public bool RandomShouldBecomeMimic(float overrideChance = -1f)
		{
			for (int i = 0; i < this.MimicPrerequisites.Count; i++)
			{
				if (!this.MimicPrerequisites[i].CheckConditionsFulfilled())
				{
					return false;
				}
			}
			float num;
			if (overrideChance >= 0f)
			{
				num = overrideChance;
			}
			else
			{
				float num2 = (float)PlayerStats.GetTotalCurse();
				num = this.MimicChance + this.MimicChancePerCurseLevel * num2;
				if (PassiveItem.IsFlagSetAtAll(typeof(MimicToothNecklaceItem)))
				{
					num += 10f;
				}
			}
			float value = UnityEngine.Random.value;
			Debug.Log(string.Concat(new object[] { "mimic ", value, "|", num }));
			return value <= num;
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x00202C04 File Offset: 0x00200E04
		public bool RandomShouldBecomePedestalMimic(float overrideChance = -1f)
		{
			for (int i = 0; i < this.PedestalMimicPrerequisites.Count; i++)
			{
				if (!this.PedestalMimicPrerequisites[i].CheckConditionsFulfilled())
				{
					return false;
				}
			}
			float num;
			if (overrideChance >= 0f)
			{
				num = overrideChance;
			}
			else
			{
				float num2 = (float)PlayerStats.GetTotalCurse();
				num = this.PedestalMimicChance + this.PedestalMimicChancePerCurseLevel * num2;
				if (PassiveItem.IsFlagSetAtAll(typeof(MimicToothNecklaceItem)))
				{
					num += 10f;
				}
			}
			float value = UnityEngine.Random.value;
			Debug.Log(string.Concat(new object[] { "pedestal mimic ", value, "|", num }));
			return value <= num;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x00202CCC File Offset: 0x00200ECC
		public bool RandomShouldSpawnPotFairy()
		{
			for (int i = 0; i < this.PotFairyPrerequisites.Count; i++)
			{
				if (!this.PotFairyPrerequisites[i].CheckConditionsFulfilled())
				{
					return false;
				}
			}
			return UnityEngine.Random.value <= this.PotFairyChance;
		}

		// Token: 0x04004DD7 RID: 19927
		[Header("Currency")]
		public GenericCurrencyDropSettings currencyDropSettings;

		// Token: 0x04004DD8 RID: 19928
		[Header("Boss Chests")]
		public WeightedGameObjectCollection ChestsForBosses;

		// Token: 0x04004DD9 RID: 19929
		[Header("Mimics")]
		public List<DungeonPrerequisite> MimicPrerequisites = new List<DungeonPrerequisite>();

		// Token: 0x04004DDA RID: 19930
		public float MimicChance = 0.05f;

		// Token: 0x04004DDB RID: 19931
		public float MimicChancePerCurseLevel = 0.01f;

		// Token: 0x04004DDC RID: 19932
		[Header("Pedestal Mimics")]
		public List<DungeonPrerequisite> PedestalMimicPrerequisites = new List<DungeonPrerequisite>();

		// Token: 0x04004DDD RID: 19933
		public float PedestalMimicChance = 0.05f;

		// Token: 0x04004DDE RID: 19934
		public float PedestalMimicChancePerCurseLevel = 0.01f;

		// Token: 0x04004DDF RID: 19935
		[Header("Pot Fairies")]
		public List<DungeonPrerequisite> PotFairyPrerequisites = new List<DungeonPrerequisite>();

		// Token: 0x04004DE0 RID: 19936
		[EnemyIdentifier]
		public string PotFairyGuid;

		// Token: 0x04004DE1 RID: 19937
		public float PotFairyChance = 0.005f;

		// Token: 0x04004DE2 RID: 19938
		[Header("Cross-Dungeon Settings")]
		public RobotDaveIdea DefaultProceduralIdea;

		// Token: 0x04004DE3 RID: 19939
		public ExplosionData DefaultExplosionData;

		// Token: 0x04004DE4 RID: 19940
		public ExplosionData DefaultSmallExplosionData;

		// Token: 0x04004DE5 RID: 19941
		public GameActorFreezeEffect DefaultFreezeExplosionEffect;

		// Token: 0x04004DE6 RID: 19942
		public GoopDefinition DefaultFreezeGoop;

		// Token: 0x04004DE7 RID: 19943
		public GoopDefinition DefaultFireGoop;

		// Token: 0x04004DE8 RID: 19944
		public GoopDefinition DefaultPoisonGoop;

		// Token: 0x04004DE9 RID: 19945
		public GameObject additionalCheeseDustup;

		// Token: 0x04004DEA RID: 19946
		public GameObject additionalWebDustup;

		// Token: 0x04004DEB RID: 19947
		public GameObject additionalTableDustup;

		// Token: 0x04004DEC RID: 19948
		public GameActorCharmEffect DefaultPermanentCharmEffect;
	}
}
