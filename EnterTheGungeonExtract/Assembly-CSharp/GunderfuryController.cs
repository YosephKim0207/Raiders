using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013B5 RID: 5045
public class GunderfuryController : MonoBehaviour
{
	// Token: 0x0600724C RID: 29260 RVA: 0x002D73DC File Offset: 0x002D55DC
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.idleVFX.gameObject.SetActive(false);
	}

	// Token: 0x0600724D RID: 29261 RVA: 0x002D73FC File Offset: 0x002D55FC
	public static int GetCurrentTier()
	{
		if (!Application.isPlaying)
		{
			return 0;
		}
		int currentAccumulatedGunderfuryExperience = GameStatsManager.Instance.CurrentAccumulatedGunderfuryExperience;
		int num = 0;
		for (int i = 0; i < GunderfuryController.expTiers.Length; i++)
		{
			if (GunderfuryController.expTiers[i] <= currentAccumulatedGunderfuryExperience && GunderfuryController.expTiers[i] > GunderfuryController.expTiers[num])
			{
				num = i;
			}
		}
		return num;
	}

	// Token: 0x0600724E RID: 29262 RVA: 0x002D7460 File Offset: 0x002D5660
	public static int GetCurrentLevel()
	{
		if (!Application.isPlaying)
		{
			return 0;
		}
		int currentTier = GunderfuryController.GetCurrentTier();
		int num = currentTier * 10;
		int num2 = num + 10;
		if (currentTier < 5)
		{
			int currentAccumulatedGunderfuryExperience = GameStatsManager.Instance.CurrentAccumulatedGunderfuryExperience;
			float num3 = (float)(currentAccumulatedGunderfuryExperience - GunderfuryController.expTiers[currentTier]) / (float)(GunderfuryController.expTiers[currentTier + 1] - GunderfuryController.expTiers[currentTier]);
			num2 += Mathf.FloorToInt(num3 * 10f);
		}
		return num2;
	}

	// Token: 0x0600724F RID: 29263 RVA: 0x002D74CC File Offset: 0x002D56CC
	private void Update()
	{
		if (this.m_gun.CurrentOwner && !this.m_initialized)
		{
			this.m_player = this.m_gun.CurrentOwner as PlayerController;
			this.m_player.OnKilledEnemyContext += this.HandleKilledEnemy;
			this.m_initialized = true;
		}
		else if (!this.m_gun.CurrentOwner && this.m_initialized)
		{
			this.m_initialized = false;
			if (this.m_player)
			{
				this.m_player.OnKilledEnemyContext -= this.HandleKilledEnemy;
			}
			this.m_player = null;
		}
		int currentTier = GunderfuryController.GetCurrentTier();
		if (this.m_currentTier != currentTier)
		{
			this.m_currentTier = currentTier;
			this.m_gun.CeaseAttack(true, null);
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.tiers[currentTier].GunID) as Gun);
			if (string.IsNullOrEmpty(this.tiers[currentTier].IdleVFX))
			{
				this.idleVFX.gameObject.SetActive(false);
			}
			else
			{
				this.idleVFX.gameObject.SetActive(true);
				this.idleVFX.Play(this.tiers[currentTier].IdleVFX);
			}
		}
		if (currentTier >= 5 && GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
		{
			this.m_sparkTimer += BraveTime.DeltaTime * 30f;
			int num = Mathf.FloorToInt(this.m_sparkTimer);
			if (num > 0)
			{
				this.m_sparkTimer -= (float)num;
				GlobalSparksDoer.DoRadialParticleBurst(num, this.m_gun.PrimaryHandAttachPoint.position, this.m_gun.barrelOffset.position, 360f, 4f, 4f, null, new float?(0.5f), new Color?(Color.white), GlobalSparksDoer.SparksType.DARK_MAGICKS);
			}
		}
		if (this.idleVFX.gameObject.activeSelf && this.m_gun && this.m_gun.sprite)
		{
			this.idleVFX.sprite.FlipY = this.m_gun.sprite.FlipY;
			this.idleVFX.renderer.enabled = this.m_gun.renderer.enabled;
		}
	}

	// Token: 0x06007250 RID: 29264 RVA: 0x002D7750 File Offset: 0x002D5950
	private void OnDestroy()
	{
		if (this.m_player)
		{
			this.m_player.OnKilledEnemyContext -= this.HandleKilledEnemy;
		}
	}

	// Token: 0x06007251 RID: 29265 RVA: 0x002D777C File Offset: 0x002D597C
	private void HandleKilledEnemy(PlayerController sourcePlayer, HealthHaver killedEnemy)
	{
		if (GameStatsManager.Instance.CurrentAccumulatedGunderfuryExperience > 10000000)
		{
			return;
		}
		if (!killedEnemy || killedEnemy.GetMaxHealth() < 0f)
		{
			GameStatsManager.Instance.CurrentAccumulatedGunderfuryExperience++;
		}
		else
		{
			GameStatsManager.Instance.CurrentAccumulatedGunderfuryExperience += Mathf.Max(1, Mathf.CeilToInt(killedEnemy.GetMaxHealth() / 10f));
		}
	}

	// Token: 0x040073AD RID: 29613
	public static int[] expTiers = new int[] { 0, 800, 2100, 3750, 5500, 7500 };

	// Token: 0x040073AE RID: 29614
	[SerializeField]
	public List<GunderfuryTier> tiers;

	// Token: 0x040073AF RID: 29615
	public tk2dSpriteAnimator idleVFX;

	// Token: 0x040073B0 RID: 29616
	private Gun m_gun;

	// Token: 0x040073B1 RID: 29617
	private bool m_initialized;

	// Token: 0x040073B2 RID: 29618
	private PlayerController m_player;

	// Token: 0x040073B3 RID: 29619
	private int m_currentTier;

	// Token: 0x040073B4 RID: 29620
	private float m_sparkTimer;
}
