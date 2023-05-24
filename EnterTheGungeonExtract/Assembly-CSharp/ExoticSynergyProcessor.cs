using System;
using UnityEngine;

// Token: 0x020016E2 RID: 5858
public class ExoticSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008848 RID: 34888 RVA: 0x00387B34 File Offset: 0x00385D34
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.HasChanceToGainAmmo)
		{
			Gun gun = this.m_gun;
			gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.HandleGainAmmo));
		}
	}

	// Token: 0x06008849 RID: 34889 RVA: 0x00387B74 File Offset: 0x00385D74
	private void HandleGainAmmo(Projectile obj)
	{
		if (this.m_gun && this.m_gun.OwnerHasSynergy(this.RequiredSynergy) && UnityEngine.Random.value < this.ChanceToGainAmmo)
		{
			this.m_gun.GainAmmo(1);
		}
	}

	// Token: 0x0600884A RID: 34890 RVA: 0x00387BC4 File Offset: 0x00385DC4
	public void Update()
	{
		if (this.SnapsToAngleMultiple && this.m_gun)
		{
			if (this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
			{
				this.m_gun.preventRotation = true;
				this.m_gun.OverrideAngleSnap = new float?(this.AngleMultiple);
			}
			else
			{
				this.m_gun.preventRotation = false;
				this.m_gun.OverrideAngleSnap = null;
			}
		}
		if (this.SetsGoopReloadFree && this.m_gun)
		{
			if (this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
			{
				this.m_gun.GoopReloadsFree = true;
			}
			else
			{
				this.m_gun.GoopReloadsFree = false;
			}
		}
		if (this.SetsFlying)
		{
			if (this.m_gun && this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
			{
				if (!this.m_cachedPlayer)
				{
					this.m_cachedPlayer = this.m_gun.CurrentOwner as PlayerController;
					this.m_cachedPlayer.SetIsFlying(true, "synergy flight", true, false);
					this.m_cachedPlayer.AdditionalCanDodgeRollWhileFlying.AddOverride("synergy flight", null);
				}
			}
			else if (this.m_cachedPlayer)
			{
				this.m_cachedPlayer.AdditionalCanDodgeRollWhileFlying.RemoveOverride("synergy flight");
				this.m_cachedPlayer.SetIsFlying(false, "synergy flight", true, false);
				this.m_cachedPlayer = null;
			}
		}
	}

	// Token: 0x0600884B RID: 34891 RVA: 0x00387D64 File Offset: 0x00385F64
	private void OnDisable()
	{
		if (this.m_cachedPlayer)
		{
			this.m_cachedPlayer.AdditionalCanDodgeRollWhileFlying.RemoveOverride("synergy flight");
			this.m_cachedPlayer.SetIsFlying(false, "synergy flight", true, false);
			this.m_cachedPlayer = null;
		}
	}

	// Token: 0x0600884C RID: 34892 RVA: 0x00387DB0 File Offset: 0x00385FB0
	private void OnDestroy()
	{
		if (this.m_cachedPlayer)
		{
			this.m_cachedPlayer.AdditionalCanDodgeRollWhileFlying.RemoveOverride("synergy flight");
			this.m_cachedPlayer.SetIsFlying(false, "synergy flight", true, false);
			this.m_cachedPlayer = null;
		}
	}

	// Token: 0x04008D92 RID: 36242
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D93 RID: 36243
	public bool SnapsToAngleMultiple;

	// Token: 0x04008D94 RID: 36244
	public float AngleMultiple = 90f;

	// Token: 0x04008D95 RID: 36245
	public bool HasChanceToGainAmmo;

	// Token: 0x04008D96 RID: 36246
	public float ChanceToGainAmmo;

	// Token: 0x04008D97 RID: 36247
	public bool SetsFlying;

	// Token: 0x04008D98 RID: 36248
	public bool SetsGoopReloadFree;

	// Token: 0x04008D99 RID: 36249
	private Gun m_gun;

	// Token: 0x04008D9A RID: 36250
	private PlayerController m_cachedPlayer;
}
