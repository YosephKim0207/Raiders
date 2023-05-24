using System;
using UnityEngine;

// Token: 0x0200144C RID: 5196
public class OnKillEnemyItem : PassiveItem
{
	// Token: 0x06007602 RID: 30210 RVA: 0x002EF834 File Offset: 0x002EDA34
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnKilledEnemy += this.OnKilledEnemy;
	}

	// Token: 0x06007603 RID: 30211 RVA: 0x002EF85C File Offset: 0x002EDA5C
	public void OnKilledEnemy(PlayerController source)
	{
		this.m_activations++;
		if (this.activationStyle == OnKillEnemyItem.ActivationStyle.RANDOM_CHANCE)
		{
			if (UnityEngine.Random.value < this.chanceOfActivating)
			{
				this.DoEffect(source);
			}
		}
		else if (this.activationStyle == OnKillEnemyItem.ActivationStyle.EVERY_X_ENEMIES && this.m_activations % this.numEnemiesBeforeActivation == 0)
		{
			this.DoEffect(source);
		}
	}

	// Token: 0x06007604 RID: 30212 RVA: 0x002EF8C4 File Offset: 0x002EDAC4
	private void DoEffect(PlayerController source)
	{
		if (this.ammoToGain > 0 && source.CurrentGun != null)
		{
			source.CurrentGun.GainAmmo(Mathf.Max(1, (int)((float)this.ammoToGain * 0.01f * (float)source.CurrentGun.AdjustedMaxAmmo)));
		}
		if (this.healthToGain > 0f)
		{
			source.healthHaver.ApplyHealing(this.healthToGain);
		}
	}

	// Token: 0x06007605 RID: 30213 RVA: 0x002EF93C File Offset: 0x002EDB3C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<OnKillEnemyItem>().m_pickedUpThisRun = true;
		player.OnKilledEnemy -= this.OnKilledEnemy;
		return debrisObject;
	}

	// Token: 0x06007606 RID: 30214 RVA: 0x002EF970 File Offset: 0x002EDB70
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040077B8 RID: 30648
	public OnKillEnemyItem.ActivationStyle activationStyle;

	// Token: 0x040077B9 RID: 30649
	[ShowInInspectorIf("activationStyle", 0, false)]
	public float chanceOfActivating = 1f;

	// Token: 0x040077BA RID: 30650
	[ShowInInspectorIf("activationStyle", 1, false)]
	public int numEnemiesBeforeActivation = 3;

	// Token: 0x040077BB RID: 30651
	public int ammoToGain = 1;

	// Token: 0x040077BC RID: 30652
	public float healthToGain;

	// Token: 0x040077BD RID: 30653
	private int m_activations;

	// Token: 0x0200144D RID: 5197
	public enum ActivationStyle
	{
		// Token: 0x040077BF RID: 30655
		RANDOM_CHANCE,
		// Token: 0x040077C0 RID: 30656
		EVERY_X_ENEMIES
	}
}
