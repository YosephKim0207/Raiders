using System;
using UnityEngine;

// Token: 0x020016C6 RID: 5830
public class SlideSurface : MonoBehaviour
{
	// Token: 0x0600877F RID: 34687 RVA: 0x003832FC File Offset: 0x003814FC
	public void Awake()
	{
		this.m_table = base.GetComponent<FlippableCover>();
		if (!this.m_table && base.transform.parent)
		{
			this.m_table = base.transform.parent.GetComponent<FlippableCover>();
		}
		if (this.m_table)
		{
			this.m_surface = this.m_table.GetComponent<SurfaceDecorator>();
		}
		SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
		component.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(component.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
	}

	// Token: 0x06008780 RID: 34688 RVA: 0x00383398 File Offset: 0x00381598
	private void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody)
		{
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (component && component.CurrentRollState == PlayerController.DodgeRollState.InAir && this.IsAccessible(component))
			{
				if (!component.IsSlidingOverSurface)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_SLID_OVER_TABLE, 1f);
				}
				component.IsSlidingOverSurface = true;
				PhysicsEngine.SkipCollision = true;
				if (this.m_surface)
				{
					this.m_surface.Destabilize(component.specRigidbody.Velocity);
				}
			}
		}
	}

	// Token: 0x06008781 RID: 34689 RVA: 0x0038342C File Offset: 0x0038162C
	public bool IsAccessible(PlayerController collidingPlayer)
	{
		return !this.m_table || (!this.m_table.IsFlipped && !this.m_table.IsBroken && (collidingPlayer.IsSlidingOverSurface || true));
	}

	// Token: 0x04008CC1 RID: 36033
	private FlippableCover m_table;

	// Token: 0x04008CC2 RID: 36034
	private SurfaceDecorator m_surface;
}
