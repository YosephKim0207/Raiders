using System;
using UnityEngine;

// Token: 0x020013BD RID: 5053
public class HungryProjectileModifier : MonoBehaviour
{
	// Token: 0x0600728B RID: 29323 RVA: 0x002D8A84 File Offset: 0x002D6C84
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_projectile.AdjustPlayerProjectileTint(new Color(0.45f, 0.3f, 0.87f), 2, 0f);
		this.m_projectile.collidesWithProjectiles = true;
		SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
	}

	// Token: 0x0600728C RID: 29324 RVA: 0x002D8AFC File Offset: 0x002D6CFC
	private void Update()
	{
		if (this.m_sated)
		{
			return;
		}
		Vector2 vector = this.m_projectile.transform.position.XY();
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.Owner is AIActor)
			{
				float sqrMagnitude = (projectile.transform.position.XY() - vector).sqrMagnitude;
				if (sqrMagnitude < this.HungryRadius)
				{
					this.EatBullet(projectile);
				}
			}
		}
	}

	// Token: 0x0600728D RID: 29325 RVA: 0x002D8BA0 File Offset: 0x002D6DA0
	private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (this.m_sated)
		{
			return;
		}
		if (otherRigidbody && otherRigidbody.projectile)
		{
			if (otherRigidbody.projectile.Owner is AIActor)
			{
				this.EatBullet(otherRigidbody.projectile);
			}
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x0600728E RID: 29326 RVA: 0x002D8BFC File Offset: 0x002D6DFC
	private void EatBullet(Projectile other)
	{
		if (this.m_sated)
		{
			return;
		}
		other.DieInAir(false, true, true, false);
		float num = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
		this.m_numberOfBulletsEaten++;
		float num2 = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
		float num3 = num2 / num;
		float num4 = Mathf.Max(1f, num3);
		if (num4 > 1f)
		{
			this.m_projectile.RuntimeUpdateScale(num4);
			this.m_projectile.baseData.damage *= num4;
		}
		if (this.m_numberOfBulletsEaten >= this.MaximumBulletsEaten)
		{
			this.m_sated = true;
			this.m_projectile.AdjustPlayerProjectileTint(this.m_projectile.DefaultTintColor, 3, 0f);
		}
	}

	// Token: 0x040073E1 RID: 29665
	public float DamagePercentGainPerSnack = 0.25f;

	// Token: 0x040073E2 RID: 29666
	public float MaxMultiplier = 3f;

	// Token: 0x040073E3 RID: 29667
	public float HungryRadius = 3f;

	// Token: 0x040073E4 RID: 29668
	public int MaximumBulletsEaten = 10;

	// Token: 0x040073E5 RID: 29669
	private Projectile m_projectile;

	// Token: 0x040073E6 RID: 29670
	private int m_numberOfBulletsEaten;

	// Token: 0x040073E7 RID: 29671
	private bool m_sated;
}
