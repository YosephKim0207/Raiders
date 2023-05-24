using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001618 RID: 5656
public class AppliesGoopDoerModifier : MonoBehaviour
{
	// Token: 0x060083D0 RID: 33744 RVA: 0x0036016C File Offset: 0x0035E36C
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		Projectile projectile = this.m_projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x060083D1 RID: 33745 RVA: 0x003601A4 File Offset: 0x0035E3A4
	private void HandleHitEnemy(Projectile p1, SpeculativeRigidbody srb1, bool killedEnemy)
	{
		if (this.IsSynergyContingent && (!p1 || !p1.PossibleSourceGun || !(p1.PossibleSourceGun.CurrentOwner is PlayerController) || !(p1.PossibleSourceGun.CurrentOwner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false)))
		{
			return;
		}
		if (this && srb1)
		{
			AIActor component = srb1.GetComponent<AIActor>();
			if (component && !this.m_processedActors.Contains(component))
			{
				this.m_processedActors.Add(component);
				if (killedEnemy)
				{
					Vector2 unitCenter = srb1.UnitCenter;
					DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinitionToUse).TimedAddGoopCircle(unitCenter, this.goopRadius, 1f, false);
				}
				else
				{
					GoopDoer goopDoer = srb1.gameObject.AddComponent<GoopDoer>();
					goopDoer.updateTiming = GoopDoer.UpdateTiming.TriggerOnly;
					goopDoer.updateOnPreDeath = true;
					goopDoer.goopDefinition = this.goopDefinitionToUse;
					goopDoer.defaultGoopRadius = this.goopRadius;
					goopDoer.isTimed = true;
				}
			}
		}
	}

	// Token: 0x04008725 RID: 34597
	public GoopDefinition goopDefinitionToUse;

	// Token: 0x04008726 RID: 34598
	public float goopRadius = 3f;

	// Token: 0x04008727 RID: 34599
	public bool IsSynergyContingent;

	// Token: 0x04008728 RID: 34600
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008729 RID: 34601
	protected Projectile m_projectile;

	// Token: 0x0400872A RID: 34602
	protected HashSet<AIActor> m_processedActors = new HashSet<AIActor>();
}
