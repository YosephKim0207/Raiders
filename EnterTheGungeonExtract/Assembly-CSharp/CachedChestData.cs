using System;

// Token: 0x02001373 RID: 4979
[Serializable]
public class CachedChestData
{
	// Token: 0x060070D2 RID: 28882 RVA: 0x002CC908 File Offset: 0x002CAB08
	public CachedChestData(string data)
	{
		this.Deserialize(data);
	}

	// Token: 0x060070D3 RID: 28883 RVA: 0x002CC920 File Offset: 0x002CAB20
	public CachedChestData(Chest c)
	{
		this.m_isLocked = c.IsLocked;
		this.m_glitch = c.IsGlitched;
		this.m_rainbow = c.IsRainbowChest;
		this.m_synergy = c.lootTable != null && c.lootTable.CompletesSynergy;
		RewardManager rewardManager = GameManager.Instance.RewardManager;
		this.chestQuality = PickupObject.ItemQuality.D;
		if (c.name.Contains(rewardManager.S_Chest.name))
		{
			this.chestQuality = PickupObject.ItemQuality.S;
		}
		else if (c.name.Contains(rewardManager.A_Chest.name))
		{
			this.chestQuality = PickupObject.ItemQuality.A;
		}
		else if (c.name.Contains(rewardManager.B_Chest.name))
		{
			this.chestQuality = PickupObject.ItemQuality.B;
		}
		else if (c.name.Contains(rewardManager.C_Chest.name))
		{
			this.chestQuality = PickupObject.ItemQuality.C;
		}
	}

	// Token: 0x060070D4 RID: 28884 RVA: 0x002CCA28 File Offset: 0x002CAC28
	public void Upgrade()
	{
		switch (this.chestQuality)
		{
		case PickupObject.ItemQuality.COMMON:
			this.chestQuality = PickupObject.ItemQuality.D;
			break;
		case PickupObject.ItemQuality.D:
			this.chestQuality = PickupObject.ItemQuality.C;
			break;
		case PickupObject.ItemQuality.C:
			this.chestQuality = PickupObject.ItemQuality.B;
			break;
		case PickupObject.ItemQuality.B:
			this.chestQuality = PickupObject.ItemQuality.A;
			break;
		case PickupObject.ItemQuality.A:
			this.chestQuality = PickupObject.ItemQuality.S;
			break;
		}
	}

	// Token: 0x060070D5 RID: 28885 RVA: 0x002CCA98 File Offset: 0x002CAC98
	public string Serialize()
	{
		return string.Concat(new string[]
		{
			this.chestQuality.ToString(),
			"|",
			this.m_glitch.ToString(),
			"|",
			this.m_rainbow.ToString(),
			"|",
			this.m_isLocked.ToString(),
			"|",
			this.m_synergy.ToString()
		});
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x002CCB38 File Offset: 0x002CAD38
	public void Deserialize(string data)
	{
		string[] array = data.Split(new char[] { '|' });
		this.chestQuality = (PickupObject.ItemQuality)Enum.Parse(typeof(PickupObject.ItemQuality), array[0]);
		this.m_glitch = bool.Parse(array[1]);
		this.m_rainbow = bool.Parse(array[2]);
		this.m_isLocked = bool.Parse(array[3]);
		if (array.Length > 4)
		{
			this.m_synergy = bool.Parse(array[4]);
		}
	}

	// Token: 0x060070D7 RID: 28887 RVA: 0x002CCBB8 File Offset: 0x002CADB8
	public void SpawnChest(IntVector2 position)
	{
		Chest chest;
		if (this.m_synergy)
		{
			chest = GameManager.Instance.RewardManager.Synergy_Chest;
		}
		else
		{
			switch (this.chestQuality)
			{
			case PickupObject.ItemQuality.D:
				chest = GameManager.Instance.RewardManager.D_Chest;
				break;
			case PickupObject.ItemQuality.C:
				chest = GameManager.Instance.RewardManager.C_Chest;
				break;
			case PickupObject.ItemQuality.B:
				chest = GameManager.Instance.RewardManager.B_Chest;
				break;
			case PickupObject.ItemQuality.A:
				chest = GameManager.Instance.RewardManager.A_Chest;
				break;
			case PickupObject.ItemQuality.S:
				chest = GameManager.Instance.RewardManager.S_Chest;
				break;
			default:
				chest = GameManager.Instance.RewardManager.D_Chest;
				break;
			}
		}
		if (chest)
		{
			Chest chest2 = Chest.Spawn(chest, position);
			chest2.RegisterChestOnMinimap(position.ToVector2().GetAbsoluteRoom());
			chest2.IsLocked = this.m_isLocked;
			if (this.m_glitch)
			{
				chest2.BecomeGlitchChest();
			}
			if (this.m_rainbow)
			{
				chest2.BecomeRainbowChest();
			}
		}
	}

	// Token: 0x04007057 RID: 28759
	private bool m_glitch;

	// Token: 0x04007058 RID: 28760
	private bool m_rainbow;

	// Token: 0x04007059 RID: 28761
	private bool m_synergy;

	// Token: 0x0400705A RID: 28762
	private PickupObject.ItemQuality chestQuality = PickupObject.ItemQuality.D;

	// Token: 0x0400705B RID: 28763
	private bool m_isLocked;
}
