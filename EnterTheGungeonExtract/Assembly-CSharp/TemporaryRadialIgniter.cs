using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020014D9 RID: 5337
public class TemporaryRadialIgniter : MonoBehaviour
{
	// Token: 0x06007955 RID: 31061 RVA: 0x00308D90 File Offset: 0x00306F90
	private void Start()
	{
		this.HandleRadialIndicator();
		UnityEngine.Object.Destroy(base.gameObject, this.Lifespan);
	}

	// Token: 0x06007956 RID: 31062 RVA: 0x00308DAC File Offset: 0x00306FAC
	private void Update()
	{
		this.DoAura();
	}

	// Token: 0x06007957 RID: 31063 RVA: 0x00308DB4 File Offset: 0x00306FB4
	protected virtual void DoAura()
	{
		if (this.AuraAction == null)
		{
			this.AuraAction = delegate(AIActor actor, float dist)
			{
				actor.ApplyEffect(this.igniteEffect, 1f, null);
			};
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			if (this.m_radialIndicator)
			{
				this.m_radialIndicator.CurrentRadius = this.Radius;
			}
			absoluteRoom.ApplyActionToNearbyEnemies(base.transform.position, this.Radius, this.AuraAction);
		}
	}

	// Token: 0x06007958 RID: 31064 RVA: 0x00308E38 File Offset: 0x00307038
	private void OnDestroy()
	{
		this.UnhandleRadialIndicator();
	}

	// Token: 0x06007959 RID: 31065 RVA: 0x00308E40 File Offset: 0x00307040
	private void HandleRadialIndicator()
	{
		if (!this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = true;
			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<HeatIndicatorController>();
		}
	}

	// Token: 0x0600795A RID: 31066 RVA: 0x00308E94 File Offset: 0x00307094
	private void UnhandleRadialIndicator()
	{
		if (this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = false;
			if (this.m_radialIndicator)
			{
				this.m_radialIndicator.EndEffect();
			}
			this.m_radialIndicator = null;
		}
	}

	// Token: 0x04007BC9 RID: 31689
	public float Radius = 5f;

	// Token: 0x04007BCA RID: 31690
	public float Lifespan = 5f;

	// Token: 0x04007BCB RID: 31691
	public GameActorFireEffect igniteEffect;

	// Token: 0x04007BCC RID: 31692
	private bool m_radialIndicatorActive;

	// Token: 0x04007BCD RID: 31693
	private HeatIndicatorController m_radialIndicator;

	// Token: 0x04007BCE RID: 31694
	private Action<AIActor, float> AuraAction;
}
