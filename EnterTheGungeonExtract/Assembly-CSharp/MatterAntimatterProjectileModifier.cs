using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001435 RID: 5173
public class MatterAntimatterProjectileModifier : BraveBehaviour
{
	// Token: 0x06007567 RID: 30055 RVA: 0x002EC428 File Offset: 0x002EA628
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.25f);
		base.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		yield break;
	}

	// Token: 0x06007568 RID: 30056 RVA: 0x002EC444 File Offset: 0x002EA644
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (this.m_hasAnnihilated)
		{
			return;
		}
		if (otherRigidbody.projectile)
		{
			MatterAntimatterProjectileModifier component = otherRigidbody.GetComponent<MatterAntimatterProjectileModifier>();
			if (component && component.isAntimatter != this.isAntimatter)
			{
				this.m_hasAnnihilated = true;
				component.m_hasAnnihilated = true;
				otherRigidbody.projectile.DieInAir(false, true, true, false);
				base.projectile.DieInAir(false, true, true, false);
				Vector3 vector = (myRigidbody.UnitCenter + otherRigidbody.UnitCenter) / 2f;
				Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.05f);
				GameManager.Instance.BestActivePlayer.ForceBlank(25f, 0.5f, false, false, new Vector2?(vector.XY()), true, -1f);
				if (this.isAntimatter)
				{
					Exploder.Explode(vector, this.antimatterExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
				}
				else
				{
					Exploder.Explode(vector, component.antimatterExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
				}
			}
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06007569 RID: 30057 RVA: 0x002EC564 File Offset: 0x002EA764
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400774C RID: 30540
	public bool isAntimatter;

	// Token: 0x0400774D RID: 30541
	private bool m_hasAnnihilated;

	// Token: 0x0400774E RID: 30542
	public ExplosionData antimatterExplosion;
}
