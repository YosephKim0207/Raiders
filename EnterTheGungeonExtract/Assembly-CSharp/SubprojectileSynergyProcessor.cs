using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001710 RID: 5904
public class SubprojectileSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600892F RID: 35119 RVA: 0x0038E9CC File Offset: 0x0038CBCC
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		if (this.m_projectile && this.m_projectile.PossibleSourceGun && this.m_projectile.PossibleSourceGun.OwnerHasSynergy(this.RequiredSynergy))
		{
			this.m_projectile.StartCoroutine(this.CreateSubprojectile());
		}
	}

	// Token: 0x06008930 RID: 35120 RVA: 0x0038EA38 File Offset: 0x0038CC38
	private IEnumerator CreateSubprojectile()
	{
		Projectile instanceSubprojectile = VolleyUtility.ShootSingleProjectile(this.Subprojectile, this.m_projectile.transform.position.XY(), 0f, false, this.m_projectile.Owner);
		yield return null;
		if (this.DoesOrbit)
		{
			instanceSubprojectile.OverrideMotionModule = new OrbitProjectileMotionModule
			{
				MinRadius = this.OrbitMinRadius,
				MaxRadius = this.OrbitMaxRadius,
				usesAlternateOrbitTarget = true,
				alternateOrbitTarget = this.m_projectile.specRigidbody
			};
		}
		yield break;
	}

	// Token: 0x04008F16 RID: 36630
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008F17 RID: 36631
	public Projectile Subprojectile;

	// Token: 0x04008F18 RID: 36632
	public bool DoesOrbit = true;

	// Token: 0x04008F19 RID: 36633
	public float OrbitMinRadius = 1f;

	// Token: 0x04008F1A RID: 36634
	public float OrbitMaxRadius = 1f;

	// Token: 0x04008F1B RID: 36635
	private Projectile m_projectile;
}
