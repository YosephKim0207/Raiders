using System;
using UnityEngine;

// Token: 0x02001367 RID: 4967
public class BurstVariatorGunModifier : MonoBehaviour
{
	// Token: 0x06007089 RID: 28809 RVA: 0x002CA560 File Offset: 0x002C8760
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnPostFired, new Action<PlayerController, Gun>(this.PostFired));
	}

	// Token: 0x0600708A RID: 28810 RVA: 0x002CA598 File Offset: 0x002C8798
	private int DiceRoll()
	{
		return UnityEngine.Random.Range(this.DiceMin, this.DiceMax + 1);
	}

	// Token: 0x0600708B RID: 28811 RVA: 0x002CA5B0 File Offset: 0x002C87B0
	private void PostFired(PlayerController arg1, Gun arg2)
	{
		if (!arg2.MidBurstFire)
		{
			int num = 0;
			for (int i = 0; i < this.NumDiceRolls; i++)
			{
				num += this.DiceRoll();
			}
			if (arg2.RawSourceVolley != null)
			{
				for (int j = 0; j < arg2.RawSourceVolley.projectiles.Count; j++)
				{
					if (arg2.RawSourceVolley.projectiles[j].shootStyle == ProjectileModule.ShootStyle.Burst)
					{
						arg2.RawSourceVolley.projectiles[j].burstShotCount = num;
					}
				}
			}
			else if (arg2.singleModule.shootStyle == ProjectileModule.ShootStyle.Burst)
			{
				arg2.singleModule.burstShotCount = num;
			}
			if (arg2.modifiedVolley != null)
			{
				for (int k = 0; k < arg2.modifiedVolley.projectiles.Count; k++)
				{
					if (arg2.modifiedVolley.projectiles[k].shootStyle == ProjectileModule.ShootStyle.Burst)
					{
						arg2.modifiedVolley.projectiles[k].burstShotCount = num;
					}
				}
			}
		}
	}

	// Token: 0x0400700A RID: 28682
	public int NumDiceRolls = 2;

	// Token: 0x0400700B RID: 28683
	public int DiceMin = 1;

	// Token: 0x0400700C RID: 28684
	public int DiceMax = 6;

	// Token: 0x0400700D RID: 28685
	private Gun m_gun;
}
