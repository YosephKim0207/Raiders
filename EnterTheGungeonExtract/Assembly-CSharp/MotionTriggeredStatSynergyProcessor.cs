using System;
using UnityEngine;

// Token: 0x02001701 RID: 5889
public class MotionTriggeredStatSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088E2 RID: 35042 RVA: 0x0038C630 File Offset: 0x0038A830
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x060088E3 RID: 35043 RVA: 0x0038C640 File Offset: 0x0038A840
	private void Update()
	{
		if (this.m_gun.CurrentOwner)
		{
			this.m_cachedPlayer = this.m_gun.CurrentOwner as PlayerController;
			if (this.m_cachedPlayer.specRigidbody.Velocity.magnitude > 0.05f)
			{
				this.m_elapsedMoving += BraveTime.DeltaTime;
			}
			else
			{
				this.m_elapsedMoving = 0f;
			}
		}
		else
		{
			this.m_elapsedMoving = 0f;
		}
		bool flag = this.m_cachedPlayer && this.m_cachedPlayer.HasActiveBonusSynergy(this.RequiredSynergy, false);
		if (flag && this.m_elapsedMoving > this.TimeRequiredMoving && !this.m_isActive)
		{
			this.m_isActive = true;
			this.m_cachedPlayer.ownerlessStatModifiers.Add(this.Stat);
			this.m_cachedPlayer.stats.RecalculateStats(this.m_cachedPlayer, false, false);
			this.EnableVFX(this.m_cachedPlayer);
		}
		else if (this.m_isActive && (!flag || this.m_elapsedMoving < this.TimeRequiredMoving))
		{
			this.m_isActive = false;
			if (this.m_cachedPlayer)
			{
				this.DisableVFX(this.m_cachedPlayer);
				this.m_cachedPlayer.ownerlessStatModifiers.Remove(this.Stat);
				this.m_cachedPlayer.stats.RecalculateStats(this.m_cachedPlayer, false, false);
				this.m_cachedPlayer = null;
			}
		}
	}

	// Token: 0x060088E4 RID: 35044 RVA: 0x0038C7D4 File Offset: 0x0038A9D4
	private void OnDisable()
	{
		if (this.m_isActive)
		{
			this.m_isActive = false;
			if (this.m_cachedPlayer)
			{
				this.DisableVFX(this.m_cachedPlayer);
				this.m_cachedPlayer.ownerlessStatModifiers.Remove(this.Stat);
				this.m_cachedPlayer.stats.RecalculateStats(this.m_cachedPlayer, false, false);
				this.m_cachedPlayer = null;
			}
		}
	}

	// Token: 0x060088E5 RID: 35045 RVA: 0x0038C848 File Offset: 0x0038AA48
	private void OnDestroy()
	{
		if (this.m_isActive)
		{
			this.m_isActive = false;
			if (this.m_cachedPlayer)
			{
				this.DisableVFX(this.m_cachedPlayer);
				this.m_cachedPlayer.ownerlessStatModifiers.Remove(this.Stat);
				this.m_cachedPlayer.stats.RecalculateStats(this.m_cachedPlayer, false, false);
				this.m_cachedPlayer = null;
			}
		}
	}

	// Token: 0x060088E6 RID: 35046 RVA: 0x0038C8BC File Offset: 0x0038AABC
	public void EnableVFX(PlayerController target)
	{
		Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
		if (outlineMaterial != null)
		{
			outlineMaterial.SetColor("_OverrideColor", new Color(99f, 99f, 0f));
		}
	}

	// Token: 0x060088E7 RID: 35047 RVA: 0x0038C900 File Offset: 0x0038AB00
	public void DisableVFX(PlayerController target)
	{
		Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
		if (outlineMaterial != null)
		{
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
		}
	}

	// Token: 0x04008EA1 RID: 36513
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008EA2 RID: 36514
	public StatModifier Stat;

	// Token: 0x04008EA3 RID: 36515
	public float TimeRequiredMoving = 2f;

	// Token: 0x04008EA4 RID: 36516
	private Gun m_gun;

	// Token: 0x04008EA5 RID: 36517
	private bool m_isActive;

	// Token: 0x04008EA6 RID: 36518
	private PlayerController m_cachedPlayer;

	// Token: 0x04008EA7 RID: 36519
	private float m_elapsedMoving;
}
