using System;
using UnityEngine;

// Token: 0x020013DA RID: 5082
public class PushNearbyProjectilesOnReloadModifier : MonoBehaviour
{
	// Token: 0x06007356 RID: 29526 RVA: 0x002DE2D4 File Offset: 0x002DC4D4
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_gun.CanReloadNoMatterAmmo = true;
		Gun gun = this.m_gun;
		gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
	}

	// Token: 0x06007357 RID: 29527 RVA: 0x002DE320 File Offset: 0x002DC520
	private void HandleReload(PlayerController ownerPlayer, Gun ownerGun, bool someBool)
	{
		if (!ownerGun || !ownerPlayer || !ownerGun.IsReloading)
		{
			return;
		}
		if (this.IsSynergyContingent && !ownerPlayer.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			return;
		}
		if (this.OnlyInSpecificForm && ownerGun.RawSourceVolley != this.RequiredVolley)
		{
			return;
		}
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.Owner == ownerPlayer && projectile.specRigidbody && projectile.PossibleSourceGun == ownerGun)
			{
				Vector2 unitCenter = projectile.specRigidbody.UnitCenter;
				Vector2 centerPosition = ownerPlayer.CenterPosition;
				Vector2 vector = unitCenter - centerPosition;
				float magnitude = vector.magnitude;
				float num = Mathf.DeltaAngle(ownerGun.CurrentAngle, vector.ToAngle());
				if (Mathf.Abs(num) < this.AngleCutoff && magnitude < this.DistanceCutoff)
				{
					projectile.baseData.speed *= this.SpeedMultiplier;
					projectile.baseData.AccelerationCurve = this.NewSlowdownCurve;
					projectile.baseData.IgnoreAccelCurveTime = projectile.ElapsedTime;
					projectile.baseData.CustomAccelerationCurveDuration = this.CurveTime;
					projectile.UpdateSpeed();
				}
			}
		}
	}

	// Token: 0x040074E1 RID: 29921
	public float DistanceCutoff = 5f;

	// Token: 0x040074E2 RID: 29922
	public float AngleCutoff = 45f;

	// Token: 0x040074E3 RID: 29923
	public float SpeedMultiplier = 10f;

	// Token: 0x040074E4 RID: 29924
	public AnimationCurve NewSlowdownCurve;

	// Token: 0x040074E5 RID: 29925
	public float CurveTime = 1f;

	// Token: 0x040074E6 RID: 29926
	public bool IsSynergyContingent;

	// Token: 0x040074E7 RID: 29927
	[ShowInInspectorIf("IsSynergyContingent", false)]
	public CustomSynergyType RequiredSynergy = CustomSynergyType.BUBBLE_BUSTER;

	// Token: 0x040074E8 RID: 29928
	[ShowInInspectorIf("IsSynergyContingent", false)]
	public bool OnlyInSpecificForm;

	// Token: 0x040074E9 RID: 29929
	[ShowInInspectorIf("OnlyInSpecificForm", false)]
	public ProjectileVolleyData RequiredVolley;

	// Token: 0x040074EA RID: 29930
	private Gun m_gun;
}
