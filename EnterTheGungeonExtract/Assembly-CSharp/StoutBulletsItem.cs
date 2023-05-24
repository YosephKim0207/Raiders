using System;
using UnityEngine;

// Token: 0x020014C2 RID: 5314
public class StoutBulletsItem : PassiveItem
{
	// Token: 0x060078C9 RID: 30921 RVA: 0x00304C00 File Offset: 0x00302E00
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
	}

	// Token: 0x060078CA RID: 30922 RVA: 0x00304C40 File Offset: 0x00302E40
	private void PostProcessBeam(BeamController obj)
	{
		if (obj)
		{
			Projectile projectile = obj.projectile;
			if (projectile)
			{
				this.PostProcessProjectile(projectile, 1f);
			}
		}
	}

	// Token: 0x060078CB RID: 30923 RVA: 0x00304C78 File Offset: 0x00302E78
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		float num = Mathf.Max(0f, obj.baseData.range - this.RangeCap);
		float num2 = Mathf.Lerp(this.MinDamageIncrease, this.MaxDamageIncrease, Mathf.Clamp01(num / 15f));
		obj.OnPostUpdate += this.HandlePostUpdate;
		obj.AdditionalScaleMultiplier *= this.ScaleIncrease;
		obj.baseData.damage *= num2;
	}

	// Token: 0x060078CC RID: 30924 RVA: 0x00304CF8 File Offset: 0x00302EF8
	private void HandlePostUpdate(Projectile proj)
	{
		if (proj && proj.GetElapsedDistance() > this.RangeCap)
		{
			proj.RuntimeUpdateScale(this.DescaleAmount);
			proj.baseData.damage /= this.DamageCutOnDescale;
			proj.OnPostUpdate -= this.HandlePostUpdate;
		}
	}

	// Token: 0x060078CD RID: 30925 RVA: 0x00304D58 File Offset: 0x00302F58
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<StoutBulletsItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		return debrisObject;
	}

	// Token: 0x060078CE RID: 30926 RVA: 0x00304DA8 File Offset: 0x00302FA8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
		}
	}

	// Token: 0x04007B0F RID: 31503
	public float RangeCap = 7f;

	// Token: 0x04007B10 RID: 31504
	public float MaxDamageIncrease = 1.75f;

	// Token: 0x04007B11 RID: 31505
	public float MinDamageIncrease = 1.125f;

	// Token: 0x04007B12 RID: 31506
	public float ScaleIncrease = 1.5f;

	// Token: 0x04007B13 RID: 31507
	public float DescaleAmount = 0.5f;

	// Token: 0x04007B14 RID: 31508
	public float DamageCutOnDescale = 2f;

	// Token: 0x04007B15 RID: 31509
	private PlayerController m_player;
}
