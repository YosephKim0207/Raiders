using System;
using UnityEngine;

// Token: 0x020016FF RID: 5887
public class ModifyBeamSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088DA RID: 35034 RVA: 0x0038BCBC File Offset: 0x00389EBC
	public void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_beam = base.GetComponent<BeamController>();
	}

	// Token: 0x060088DB RID: 35035 RVA: 0x0038BCD8 File Offset: 0x00389ED8
	public void Start()
	{
		PlayerController playerController = this.m_projectile.Owner as PlayerController;
		if (playerController && playerController.HasActiveBonusSynergy(this.SynergyToCheck, false) && this.AddsFreezeEffect)
		{
			this.m_projectile.AppliesFreeze = true;
			this.m_projectile.FreezeApplyChance = 1f;
			this.m_projectile.freezeEffect = this.FreezeEffect;
			this.m_projectile.damageTypes = this.m_projectile.damageTypes | CoreDamageTypes.Ice;
			this.m_beam.statusEffectChance = 1f;
			this.m_beam.statusEffectAccumulateMultiplier = 1f;
		}
	}

	// Token: 0x04008E66 RID: 36454
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008E67 RID: 36455
	public bool AddsFreezeEffect;

	// Token: 0x04008E68 RID: 36456
	[ShowInInspectorIf("AddsFreezeEffect", false)]
	public GameActorFreezeEffect FreezeEffect;

	// Token: 0x04008E69 RID: 36457
	private Projectile m_projectile;

	// Token: 0x04008E6A RID: 36458
	private BeamController m_beam;
}
