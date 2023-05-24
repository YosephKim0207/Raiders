using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013FE RID: 5118
public class EvolverGunController : MonoBehaviour, IGunInheritable
{
	// Token: 0x06007425 RID: 29733 RVA: 0x002E35C0 File Offset: 0x002E17C0
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06007426 RID: 29734 RVA: 0x002E35D0 File Offset: 0x002E17D0
	private void KilledEnemyContext(PlayerController sourcePlayer, HealthHaver killedEnemy)
	{
		if (killedEnemy)
		{
			AIActor component = killedEnemy.GetComponent<AIActor>();
			if (component)
			{
				this.m_enemiesKilled.Add(component.EnemyGuid);
				this.UpdateTier(sourcePlayer);
			}
		}
	}

	// Token: 0x06007427 RID: 29735 RVA: 0x002E3614 File Offset: 0x002E1814
	private void UpdateTier(PlayerController sourcePlayer)
	{
		int num = this.m_enemiesKilled.Count + this.m_savedEnemiesKilled;
		int num2 = this.TypesPerForm;
		if (sourcePlayer && sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.NATURAL_SELECTION, false))
		{
			num2 = Mathf.Max(1, this.TypesPerForm - 2);
		}
		if (sourcePlayer && sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.POWERHOUSE_OF_THE_CELL, false))
		{
			num += num2;
		}
		int num3 = Mathf.FloorToInt((float)num / (float)num2);
		num3 = Mathf.Min(num3, 5);
		if (num3 != this.m_currentForm)
		{
			this.m_currentForm = num3;
			this.TransformToForm(this.m_currentForm);
		}
	}

	// Token: 0x06007428 RID: 29736 RVA: 0x002E36B8 File Offset: 0x002E18B8
	private void Update()
	{
		if (this.m_initialized && !this.m_gun.CurrentOwner)
		{
			this.Disengage();
		}
		else if (!this.m_initialized && this.m_gun.CurrentOwner)
		{
			this.Engage();
		}
		if (this.m_gun.CurrentOwner)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (this.m_synergyActive && !playerController.HasActiveBonusSynergy(CustomSynergyType.POWERHOUSE_OF_THE_CELL, false))
			{
				this.m_synergyActive = false;
				this.UpdateTier(playerController);
			}
			else if (!this.m_synergyActive && playerController.HasActiveBonusSynergy(CustomSynergyType.POWERHOUSE_OF_THE_CELL, false))
			{
				this.m_synergyActive = true;
				this.UpdateTier(playerController);
			}
		}
		if (this.m_wasDeserialized && this.m_gun && this.m_gun.CurrentOwner && this.m_gun.CurrentOwner.CurrentGun == this.m_gun)
		{
			this.m_wasDeserialized = false;
			this.UpdateTier(this.m_gun.CurrentOwner as PlayerController);
		}
	}

	// Token: 0x06007429 RID: 29737 RVA: 0x002E3804 File Offset: 0x002E1A04
	private void OnDestroy()
	{
		this.m_enemiesKilled.Clear();
		this.m_savedEnemiesKilled = 0;
		this.Disengage();
	}

	// Token: 0x0600742A RID: 29738 RVA: 0x002E3820 File Offset: 0x002E1A20
	private void Engage()
	{
		this.m_initialized = true;
		this.m_player = this.m_gun.CurrentOwner as PlayerController;
		this.m_player.OnKilledEnemyContext += this.KilledEnemyContext;
	}

	// Token: 0x0600742B RID: 29739 RVA: 0x002E3858 File Offset: 0x002E1A58
	private void Disengage()
	{
		if (this.m_player)
		{
			this.m_player.OnKilledEnemyContext -= this.KilledEnemyContext;
		}
		this.m_player = null;
		this.m_initialized = false;
	}

	// Token: 0x0600742C RID: 29740 RVA: 0x002E3890 File Offset: 0x002E1A90
	private void TransformToForm(int targetForm)
	{
		switch (targetForm)
		{
		case 0:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form01ID) as Gun);
			break;
		case 1:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form02ID) as Gun);
			break;
		case 2:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form03ID) as Gun);
			break;
		case 3:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form04ID) as Gun);
			break;
		case 4:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form05ID) as Gun);
			break;
		case 5:
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.Form06ID) as Gun);
			break;
		}
		this.m_gun.GainAmmo(this.m_gun.AdjustedMaxAmmo);
	}

	// Token: 0x0600742D RID: 29741 RVA: 0x002E399C File Offset: 0x002E1B9C
	public void InheritData(Gun sourceGun)
	{
		EvolverGunController component = sourceGun.GetComponent<EvolverGunController>();
		if (component)
		{
			this.m_savedEnemiesKilled = component.m_savedEnemiesKilled;
			this.m_enemiesKilled = component.m_enemiesKilled;
		}
	}

	// Token: 0x0600742E RID: 29742 RVA: 0x002E39D4 File Offset: 0x002E1BD4
	public void MidGameSerialize(List<object> data, int dataIndex)
	{
		data.Add(this.m_savedEnemiesKilled + this.m_enemiesKilled.Count);
	}

	// Token: 0x0600742F RID: 29743 RVA: 0x002E39F4 File Offset: 0x002E1BF4
	public void MidGameDeserialize(List<object> data, ref int dataIndex)
	{
		this.m_savedEnemiesKilled = (int)data[dataIndex];
		dataIndex++;
		this.m_wasDeserialized = true;
	}

	// Token: 0x040075BC RID: 30140
	[PickupIdentifier]
	public int Form01ID;

	// Token: 0x040075BD RID: 30141
	[PickupIdentifier]
	public int Form02ID;

	// Token: 0x040075BE RID: 30142
	[PickupIdentifier]
	public int Form03ID;

	// Token: 0x040075BF RID: 30143
	[PickupIdentifier]
	public int Form04ID;

	// Token: 0x040075C0 RID: 30144
	[PickupIdentifier]
	public int Form05ID;

	// Token: 0x040075C1 RID: 30145
	[PickupIdentifier]
	public int Form06ID;

	// Token: 0x040075C2 RID: 30146
	public int TypesPerForm = 5;

	// Token: 0x040075C3 RID: 30147
	private Gun m_gun;

	// Token: 0x040075C4 RID: 30148
	private bool m_initialized;

	// Token: 0x040075C5 RID: 30149
	private PlayerController m_player;

	// Token: 0x040075C6 RID: 30150
	private int m_currentForm;

	// Token: 0x040075C7 RID: 30151
	private HashSet<string> m_enemiesKilled = new HashSet<string>();

	// Token: 0x040075C8 RID: 30152
	private int m_savedEnemiesKilled;

	// Token: 0x040075C9 RID: 30153
	private bool m_synergyActive;

	// Token: 0x040075CA RID: 30154
	private bool m_wasDeserialized;
}
