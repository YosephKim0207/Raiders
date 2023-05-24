using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000FCA RID: 4042
public class BeholsterBounceRocket : BraveBehaviour
{
	// Token: 0x06005813 RID: 22547 RVA: 0x0021A0DC File Offset: 0x002182DC
	public void Start()
	{
		this.m_projectile = base.GetComponent<RobotechProjectile>();
		if (this.m_projectile)
		{
			this.m_projectile.OnDestruction += this.OnDestruction;
		}
		BounceProjModifier component = base.GetComponent<BounceProjModifier>();
		if (component && this.m_projectile)
		{
			component.OnBounce += this.OnBounce;
			this.m_startAcceleration = this.m_projectile.angularAcceleration * this.modifiedAccelertionFactor;
			this.m_endAcceleration = this.m_projectile.angularAcceleration;
		}
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x0021A17C File Offset: 0x0021837C
	public void Update()
	{
		if (this.m_modifyingAcceleration)
		{
			this.m_modifiedAccelerationTimer += BraveTime.DeltaTime;
			this.m_projectile.angularAcceleration = Mathf.Lerp(this.m_startAcceleration, this.m_endAcceleration, this.modifiedAccelerationCurve.Evaluate(this.m_modifiedAccelerationTimer / this.modifiedAccelerationTime));
			if (this.m_modifiedAccelerationTimer > this.modifiedAccelerationTime)
			{
				this.m_modifyingAcceleration = false;
				this.m_projectile.angularAcceleration = this.m_endAcceleration;
			}
		}
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x0021A204 File Offset: 0x00218404
	private void OnBounce()
	{
		this.m_modifyingAcceleration = true;
		this.m_modifiedAccelerationTimer = 0f;
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x0021A218 File Offset: 0x00218418
	private void OnDestruction(Projectile source)
	{
		this.m_destroyed = true;
		BeholsterBounceRocket[] array = UnityEngine.Object.FindObjectsOfType<BeholsterBounceRocket>();
		ExplosiveModifier component = base.GetComponent<ExplosiveModifier>();
		if (array.Length > 1 && component)
		{
			float num = component.explosionData.pushRadius;
			if (base.specRigidbody.PrimaryPixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Circle)
			{
				num += PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.ManualDiameter) / 2f;
			}
			for (int i = 0; i < array.Count<BeholsterBounceRocket>(); i++)
			{
				BeholsterBounceRocket beholsterBounceRocket = array[i];
				if (!beholsterBounceRocket.m_destroyed && Vector2.Distance(base.specRigidbody.UnitCenter, beholsterBounceRocket.specRigidbody.UnitCenter) < num)
				{
					RobotechProjectile component2 = beholsterBounceRocket.GetComponent<RobotechProjectile>();
					LinearCastResult linearCastResult = LinearCastResult.Pool.Allocate();
					linearCastResult.Contact = (base.specRigidbody.UnitCenter + beholsterBounceRocket.specRigidbody.UnitCenter) * 0.5f;
					linearCastResult.Normal = base.specRigidbody.UnitCenter - beholsterBounceRocket.specRigidbody.UnitCenter;
					linearCastResult.OtherPixelCollider = base.specRigidbody.PrimaryPixelCollider;
					linearCastResult.MyPixelCollider = beholsterBounceRocket.specRigidbody.PrimaryPixelCollider;
					component2.ForceCollision(base.specRigidbody, linearCastResult);
					LinearCastResult.Pool.Free(ref linearCastResult);
				}
			}
		}
	}

	// Token: 0x06005817 RID: 22551 RVA: 0x0021A37C File Offset: 0x0021857C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005114 RID: 20756
	public float modifiedAccelertionFactor = 0.5f;

	// Token: 0x04005115 RID: 20757
	public float modifiedAccelerationTime = 1f;

	// Token: 0x04005116 RID: 20758
	public AnimationCurve modifiedAccelerationCurve;

	// Token: 0x04005117 RID: 20759
	private RobotechProjectile m_projectile;

	// Token: 0x04005118 RID: 20760
	private bool m_modifyingAcceleration;

	// Token: 0x04005119 RID: 20761
	private float m_modifiedAccelerationTimer;

	// Token: 0x0400511A RID: 20762
	private float m_startAcceleration;

	// Token: 0x0400511B RID: 20763
	private float m_endAcceleration;

	// Token: 0x0400511C RID: 20764
	private bool m_destroyed;
}
