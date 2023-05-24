using System;
using UnityEngine;

// Token: 0x02001440 RID: 5184
public class MoveAmmoToClipItem : PassiveItem
{
	// Token: 0x060075AA RID: 30122 RVA: 0x002EDC30 File Offset: 0x002EBE30
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (this.TriggerOnRoll)
		{
			player.OnRollStarted += this.HandleRollStarted;
		}
		base.Pickup(player);
	}

	// Token: 0x060075AB RID: 30123 RVA: 0x002EDC64 File Offset: 0x002EBE64
	private void HandleRollStarted(PlayerController arg1, Vector2 arg2)
	{
		this.DoEffect(arg1);
	}

	// Token: 0x060075AC RID: 30124 RVA: 0x002EDC70 File Offset: 0x002EBE70
	private int GetBulletsToMove(PlayerController source)
	{
		float num = (float)this.BulletsToMove;
		for (int i = 0; i < this.moveMultipliers.Length; i++)
		{
			if (source && source.HasActiveBonusSynergy(this.moveMultipliers[i].RequiredSynergy, false))
			{
				num *= this.moveMultipliers[i].SynergyMultiplier;
			}
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x060075AD RID: 30125 RVA: 0x002EDCE0 File Offset: 0x002EBEE0
	private void DoEffect(PlayerController source)
	{
		if (UnityEngine.Random.value < this.ActivationChance && source.CurrentGun != null)
		{
			source.CurrentGun.MoveBulletsIntoClip(this.GetBulletsToMove(source));
		}
	}

	// Token: 0x060075AE RID: 30126 RVA: 0x002EDD18 File Offset: 0x002EBF18
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnRollStarted -= this.HandleRollStarted;
		debrisObject.GetComponent<MoveAmmoToClipItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060075AF RID: 30127 RVA: 0x002EDD4C File Offset: 0x002EBF4C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.HandleRollStarted;
		}
	}

	// Token: 0x04007771 RID: 30577
	public int BulletsToMove = 1;

	// Token: 0x04007772 RID: 30578
	public bool TriggerOnRoll;

	// Token: 0x04007773 RID: 30579
	public float ActivationChance = 1f;

	// Token: 0x04007774 RID: 30580
	public NumericSynergyMultiplier[] moveMultipliers;
}
