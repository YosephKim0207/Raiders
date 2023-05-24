using System;
using UnityEngine;

// Token: 0x020016EB RID: 5867
public class GunExtraSettingSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008865 RID: 34917 RVA: 0x00388CBC File Offset: 0x00386EBC
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.ChangesReflectedBulletDamage)
		{
			Gun gun = this.m_gun;
			gun.OnReflectedBulletDamageModifier = (Func<float, float>)Delegate.Combine(gun.OnReflectedBulletDamageModifier, new Func<float, float>(this.GetReflectedBulletDamageModifier));
		}
		if (this.ChangesReflectedBulletScale)
		{
			Gun gun2 = this.m_gun;
			gun2.OnReflectedBulletScaleModifier = (Func<float, float>)Delegate.Combine(gun2.OnReflectedBulletScaleModifier, new Func<float, float>(this.GetReflectedBulletScaleModifier));
		}
	}

	// Token: 0x06008866 RID: 34918 RVA: 0x00388D3C File Offset: 0x00386F3C
	private float GetReflectedBulletScaleModifier(float inScale)
	{
		if (this.HasSynergy())
		{
			return inScale * this.ReflectedBulletScaleModifier;
		}
		return inScale;
	}

	// Token: 0x06008867 RID: 34919 RVA: 0x00388D54 File Offset: 0x00386F54
	private bool HasSynergy()
	{
		return this.m_gun && this.m_gun.CurrentOwner is PlayerController && (this.m_gun.CurrentOwner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false);
	}

	// Token: 0x06008868 RID: 34920 RVA: 0x00388DA8 File Offset: 0x00386FA8
	private float GetReflectedBulletDamageModifier(float inDamage)
	{
		if (this.HasSynergy())
		{
			return inDamage * this.ReflectedBulletDamageModifier;
		}
		return inDamage;
	}

	// Token: 0x04008DCA RID: 36298
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008DCB RID: 36299
	public bool ChangesReflectedBulletDamage;

	// Token: 0x04008DCC RID: 36300
	public float ReflectedBulletDamageModifier = 1f;

	// Token: 0x04008DCD RID: 36301
	public bool ChangesReflectedBulletScale;

	// Token: 0x04008DCE RID: 36302
	public float ReflectedBulletScaleModifier = 1f;

	// Token: 0x04008DCF RID: 36303
	private Gun m_gun;
}
