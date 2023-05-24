using System;
using UnityEngine;

// Token: 0x02001366 RID: 4966
public class BurningHandModifier : MonoBehaviour
{
	// Token: 0x06007086 RID: 28806 RVA: 0x002CA418 File Offset: 0x002C8618
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.HandleProjectileMod));
	}

	// Token: 0x06007087 RID: 28807 RVA: 0x002CA450 File Offset: 0x002C8650
	private void HandleProjectileMod(Projectile p)
	{
		int num = UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
		int num2 = 0;
		if (this.m_gun.CurrentOwner is PlayerController)
		{
			PlayableCharacters characterIdentity = (this.m_gun.CurrentOwner as PlayerController).characterIdentity;
			if (characterIdentity == PlayableCharacters.Robot)
			{
				num2 = 1;
			}
			else if (characterIdentity == PlayableCharacters.Bullet)
			{
				num2 = -1;
			}
		}
		int num3 = Mathf.Clamp(num + num2, 1, 100);
		int num4 = 0;
		if (PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.LOADED_DICE, out num4))
		{
			num3 = Mathf.Max(12, num3);
		}
		float num5 = Mathf.Lerp(this.MinScale, this.MaxScale, Mathf.Clamp01((float)num3 / this.MaxRoll));
		float num6 = Mathf.Lerp(this.MinDamageMultiplier, this.MaxDamageMultiplier, Mathf.Clamp01((float)num3 / this.MaxRoll));
		p.AdditionalScaleMultiplier *= num5;
		p.baseData.damage *= num6;
	}

	// Token: 0x04007004 RID: 28676
	public float MinDamageMultiplier = 1f;

	// Token: 0x04007005 RID: 28677
	public float MaxDamageMultiplier = 10f;

	// Token: 0x04007006 RID: 28678
	public float MinScale = 0.5f;

	// Token: 0x04007007 RID: 28679
	public float MaxScale = 2.5f;

	// Token: 0x04007008 RID: 28680
	public float MaxRoll = 13f;

	// Token: 0x04007009 RID: 28681
	private Gun m_gun;
}
