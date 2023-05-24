using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016D7 RID: 5847
public class AlphabetSoupSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008806 RID: 34822 RVA: 0x003863DC File Offset: 0x003845DC
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.PostProcessVolley = (Action<ProjectileVolleyData>)Delegate.Combine(gun.PostProcessVolley, new Action<ProjectileVolleyData>(this.HandlePostProcessVolley));
		Gun gun2 = this.m_gun;
		gun2.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun2.PostProcessProjectile, new Action<Projectile>(this.HandlePostProcessProjectile));
		Gun gun3 = this.m_gun;
		gun3.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun3.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
		Gun gun4 = this.m_gun;
		gun4.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun4.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
		Gun gun5 = this.m_gun;
		gun5.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(gun5.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleFinishAttack));
		Gun gun6 = this.m_gun;
		gun6.OnBurstContinued = (Action<PlayerController, Gun>)Delegate.Combine(gun6.OnBurstContinued, new Action<PlayerController, Gun>(this.HandleBurstContinued));
	}

	// Token: 0x06008807 RID: 34823 RVA: 0x003864E0 File Offset: 0x003846E0
	private void HandleBurstContinued(PlayerController arg1, Gun arg2)
	{
		if (this.m_gun && this.m_gun.gunClass == GunClass.EXPLOSIVE)
		{
			return;
		}
		this.HandleFinishAttack(arg1, arg2);
	}

	// Token: 0x06008808 RID: 34824 RVA: 0x00386510 File Offset: 0x00384710
	private void HandlePostFired(PlayerController arg1, Gun arg2)
	{
		if (this.m_gun && this.m_gun.gunClass == GunClass.EXPLOSIVE)
		{
			return;
		}
		if (!this.m_hasPlayedAudioThisShot)
		{
			this.m_hasPlayedAudioThisShot = true;
			AkSoundEngine.PostEvent(this.m_currentAudioEvent, arg2.gameObject);
		}
	}

	// Token: 0x06008809 RID: 34825 RVA: 0x00386564 File Offset: 0x00384764
	private void HandlePostProcessVolley(ProjectileVolleyData obj)
	{
		this.m_currentEntryCount = 0;
	}

	// Token: 0x0600880A RID: 34826 RVA: 0x00386570 File Offset: 0x00384770
	private void HandleReloadPressed(PlayerController arg1, Gun arg2, bool arg3)
	{
		this.m_currentEntryCount = 0;
	}

	// Token: 0x0600880B RID: 34827 RVA: 0x0038657C File Offset: 0x0038477C
	private string GetLetterForWordPosition(string word)
	{
		if (this.m_currentEntryCount < 0 || this.m_currentEntryCount >= word.Length)
		{
			return "word_projectile_B_001";
		}
		char c = word[this.m_currentEntryCount];
		switch (c)
		{
		case 'A':
			return "word_projectile_A_001";
		case 'B':
			return "word_projectile_B_001";
		case 'C':
			return "word_projectile_C_001";
		case 'D':
			return "word_projectile_D_001";
		case 'E':
			return "word_projectile_B_004";
		case 'F':
			return "word_projectile_F_001";
		case 'G':
			return "word_projectile_G_001";
		case 'H':
			return "word_projectile_H_001";
		case 'I':
			return "word_projectile_I_001";
		case 'J':
			return "word_projectile_J_001";
		case 'K':
			return "word_projectile_K_001";
		case 'L':
			return "word_projectile_B_003";
		case 'M':
			return "word_projectile_M_001";
		case 'N':
			return "word_projectile_N_001";
		case 'O':
			return "word_projectile_O_001";
		case 'P':
			return "word_projectile_P_001";
		case 'Q':
			return "word_projectile_Q_001";
		case 'R':
			return "word_projectile_R_001";
		case 'S':
			return "word_projectile_S_001";
		case 'T':
			return "word_projectile_B_005";
		case 'U':
			return "word_projectile_B_002";
		case 'V':
			return "word_projectile_V_001";
		case 'W':
			return "word_projectile_W_001";
		case 'X':
			return "word_projectile_X_001";
		case 'Y':
			return "word_projectile_Y_001";
		case 'Z':
			return "word_projectile_Z_001";
		default:
			if (c == '+')
			{
				return "word_projectile_+_001";
			}
			if (c != '1')
			{
				return "word_projectile_B_001";
			}
			return "word_projectile_1_001";
		case 'a':
			return "word_projectile_alpha_001";
		case 'o':
			return "word_projectile_omega_001";
		}
	}

	// Token: 0x0600880C RID: 34828 RVA: 0x0038674C File Offset: 0x0038494C
	private void HandlePostProcessProjectile(Projectile targetProjectile)
	{
		if (!targetProjectile || !targetProjectile.sprite)
		{
			return;
		}
		if (this.m_gun && this.m_gun.gunClass == GunClass.EXPLOSIVE)
		{
			return;
		}
		targetProjectile.sprite.SetSprite(this.GetLetterForWordPosition(this.m_currentEntry));
		this.m_currentEntryCount++;
	}

	// Token: 0x0600880D RID: 34829 RVA: 0x003867C0 File Offset: 0x003849C0
	private void HandleFinishAttack(PlayerController sourcePlayer, Gun sourceGun)
	{
		if (this.m_gun && this.m_gun.gunClass == GunClass.EXPLOSIVE)
		{
			return;
		}
		this.m_hasPlayedAudioThisShot = false;
		int num = UnityEngine.Random.Range(0, this.Entries.Length);
		AlphabetSoupEntry alphabetSoupEntry = null;
		for (int i = num; i < num + this.Entries.Length; i++)
		{
			AlphabetSoupEntry alphabetSoupEntry2 = this.Entries[i % this.Entries.Length];
			if (sourcePlayer.HasActiveBonusSynergy(alphabetSoupEntry2.RequiredSynergy, false))
			{
				alphabetSoupEntry = alphabetSoupEntry2;
				break;
			}
		}
		if (alphabetSoupEntry != null)
		{
			this.ProcessVolley(this.m_gun.modifiedVolley, alphabetSoupEntry);
		}
		else
		{
			this.m_currentEntryCount = 0;
			this.m_currentEntry = "BULLET";
			this.m_currentAudioEvent = "Play_WPN_rgun_bullet_01";
		}
	}

	// Token: 0x0600880E RID: 34830 RVA: 0x00386888 File Offset: 0x00384A88
	private void ProcessVolley(ProjectileVolleyData currentVolley, AlphabetSoupEntry entry)
	{
		if (this.m_gun && this.m_gun.gunClass == GunClass.EXPLOSIVE)
		{
			return;
		}
		ProjectileModule projectileModule = currentVolley.projectiles[0];
		projectileModule.ClearOrderedProjectileData();
		if (!this.m_hasReplacedProjectileList)
		{
			this.m_hasReplacedProjectileList = true;
			projectileModule.projectiles = new List<Projectile>();
		}
		projectileModule.projectiles.Clear();
		int num = UnityEngine.Random.Range(0, entry.Words.Length);
		this.m_currentEntry = entry.Words[num];
		this.m_currentAudioEvent = entry.AudioEvents[num];
		projectileModule.burstShotCount = this.m_currentEntry.Length;
		for (int i = 0; i < this.m_currentEntry.Length; i++)
		{
			projectileModule.projectiles.Add(entry.BaseProjectile);
		}
		this.m_currentEntryCount = 0;
	}

	// Token: 0x04008D3E RID: 36158
	public AlphabetSoupEntry[] Entries;

	// Token: 0x04008D3F RID: 36159
	private Gun m_gun;

	// Token: 0x04008D40 RID: 36160
	private string m_currentEntry = "BULLET";

	// Token: 0x04008D41 RID: 36161
	private int m_currentEntryCount;

	// Token: 0x04008D42 RID: 36162
	private bool m_hasReplacedProjectileList;

	// Token: 0x04008D43 RID: 36163
	private bool m_hasPlayedAudioThisShot;

	// Token: 0x04008D44 RID: 36164
	private string m_currentAudioEvent = "Play_WPN_rgun_bullet_01";
}
