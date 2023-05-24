using System;

// Token: 0x02001483 RID: 5251
public class RegenerationPassiveItem : PassiveItem
{
	// Token: 0x06007765 RID: 30565 RVA: 0x002F97AC File Offset: 0x002F79AC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		this.m_player.OnDealtDamage += this.PlayerDealtDamage;
		base.Pickup(player);
	}

	// Token: 0x06007766 RID: 30566 RVA: 0x002F97E0 File Offset: 0x002F79E0
	private void PlayerDealtDamage(PlayerController p, float damage)
	{
		if (p.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			this.m_damageDealtCounter = 0f;
			return;
		}
		this.m_damageDealtCounter += damage;
		if (this.m_damageDealtCounter >= this.RequiredDamage)
		{
			p.healthHaver.ApplyHealing(0.5f);
			this.m_damageDealtCounter = 0f;
		}
	}

	// Token: 0x06007767 RID: 30567 RVA: 0x002F9848 File Offset: 0x002F7A48
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<RegenerationPassiveItem>().m_pickedUpThisRun = true;
		this.m_player.OnDealtDamage -= this.PlayerDealtDamage;
		this.m_player = null;
		return debrisObject;
	}

	// Token: 0x06007768 RID: 30568 RVA: 0x002F9888 File Offset: 0x002F7A88
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.OnDealtDamage -= this.PlayerDealtDamage;
		}
	}

	// Token: 0x0400795E RID: 31070
	public float RequiredDamage = 1000f;

	// Token: 0x0400795F RID: 31071
	protected PlayerController m_player;

	// Token: 0x04007960 RID: 31072
	protected float m_damageDealtCounter;
}
