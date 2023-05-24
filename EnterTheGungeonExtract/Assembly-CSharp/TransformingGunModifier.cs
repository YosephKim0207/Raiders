using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001691 RID: 5777
public class TransformingGunModifier : MonoBehaviour
{
	// Token: 0x060086B8 RID: 34488 RVA: 0x0037D724 File Offset: 0x0037B924
	private IEnumerator Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.TransformsOnAmmoThresholds)
		{
			Gun gun = this.m_gun;
			gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
			Gun gun2 = this.m_gun;
			gun2.OnAmmoChanged = (Action<PlayerController, Gun>)Delegate.Combine(gun2.OnAmmoChanged, new Action<PlayerController, Gun>(this.HandlePostFired));
		}
		yield return null;
		if (this.m_gun.CurrentOwner != null && this.m_gun.CurrentOwner is PlayerController)
		{
			this.HandlePostFired(this.m_gun.CurrentOwner as PlayerController, this.m_gun);
		}
		yield break;
	}

	// Token: 0x060086B9 RID: 34489 RVA: 0x0037D740 File Offset: 0x0037B940
	private float GetMaxAmmoSansInfinity(Gun g)
	{
		if (g.CurrentOwner == null)
		{
			return (float)g.GetBaseMaxAmmo();
		}
		if (!(g.CurrentOwner is PlayerController))
		{
			return (float)g.GetBaseMaxAmmo();
		}
		if (g.RequiresFundsToShoot)
		{
			return (float)g.ClipShotsRemaining;
		}
		if ((g.CurrentOwner as PlayerController).stats != null)
		{
			float statValue = (g.CurrentOwner as PlayerController).stats.GetStatValue(PlayerStats.StatType.AmmoCapacityMultiplier);
			return (float)Mathf.RoundToInt(statValue * (float)g.GetBaseMaxAmmo());
		}
		return (float)g.GetBaseMaxAmmo();
	}

	// Token: 0x060086BA RID: 34490 RVA: 0x0037D7DC File Offset: 0x0037B9DC
	private void HandlePostFired(PlayerController arg1, Gun arg2)
	{
		if (!arg2.enabled)
		{
			return;
		}
		float num = (float)this.m_gun.CurrentAmmo / (1f * this.GetMaxAmmoSansInfinity(this.m_gun));
		AmmoThresholdTransformation? ammoThresholdTransformation = null;
		for (int i = 0; i < this.AmmoThresholdTransformations.Count; i++)
		{
			AmmoThresholdTransformation ammoThresholdTransformation2 = this.AmmoThresholdTransformations[i];
			if (num <= ammoThresholdTransformation2.GetAmmoPercentage())
			{
				if (ammoThresholdTransformation == null)
				{
					ammoThresholdTransformation = new AmmoThresholdTransformation?(ammoThresholdTransformation2);
				}
				else if (ammoThresholdTransformation2.GetAmmoPercentage() < ammoThresholdTransformation.Value.GetAmmoPercentage())
				{
					ammoThresholdTransformation = new AmmoThresholdTransformation?(ammoThresholdTransformation2);
				}
			}
		}
		if (ammoThresholdTransformation != null)
		{
			Gun gun = PickupObjectDatabase.GetById(ammoThresholdTransformation.Value.TargetGunID) as Gun;
			if (gun && gun.shootAnimation != this.m_gun.shootAnimation)
			{
				this.m_gun.TransformToTargetGun(gun);
			}
		}
		this.m_previousAmmoPercentage = num;
	}

	// Token: 0x04008BD5 RID: 35797
	[PickupIdentifier]
	public int BaseGunID;

	// Token: 0x04008BD6 RID: 35798
	public bool TransformsOnAmmoThresholds;

	// Token: 0x04008BD7 RID: 35799
	public List<AmmoThresholdTransformation> AmmoThresholdTransformations;

	// Token: 0x04008BD8 RID: 35800
	public bool TransformsOnDamageDealt;

	// Token: 0x04008BD9 RID: 35801
	public bool TransformationsAreTimeLimited;

	// Token: 0x04008BDA RID: 35802
	[ShowInInspectorIf("TransformationsAreTimeLimited", false)]
	public float TransformationDuration = 10f;

	// Token: 0x04008BDB RID: 35803
	public bool TransformationsAreAmmoLimited;

	// Token: 0x04008BDC RID: 35804
	[ShowInInspectorIf("TransformationsAreAmmoLimited", false)]
	public int TransformationAmmoCount = 10;

	// Token: 0x04008BDD RID: 35805
	private Gun m_gun;

	// Token: 0x04008BDE RID: 35806
	private float m_previousAmmoPercentage = 1f;
}
