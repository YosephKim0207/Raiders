using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200147D RID: 5245
public class RagePassiveItem : PassiveItem
{
	// Token: 0x06007744 RID: 30532 RVA: 0x002F8C58 File Offset: 0x002F6E58
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnReceivedDamage += this.HandleReceivedDamage;
		this.m_player = player;
	}

	// Token: 0x06007745 RID: 30533 RVA: 0x002F8C88 File Offset: 0x002F6E88
	private void HandleReceivedDamage(PlayerController obj)
	{
		if (this.m_isRaged)
		{
			if (this.OverheadVFX && !this.instanceVFX)
			{
				this.instanceVFX = this.m_player.PlayEffectOnActor(this.OverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
			}
			this.m_elapsed = 0f;
		}
		else
		{
			obj.StartCoroutine(this.HandleRage());
		}
	}

	// Token: 0x06007746 RID: 30534 RVA: 0x002F8D0C File Offset: 0x002F6F0C
	private IEnumerator HandleRage()
	{
		this.m_isRaged = true;
		this.instanceVFX = null;
		if (this.OverheadVFX)
		{
			this.instanceVFX = this.m_player.PlayEffectOnActor(this.OverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
		}
		StatModifier damageStat = new StatModifier();
		damageStat.amount = this.DamageMultiplier;
		damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
		damageStat.statToBoost = PlayerStats.StatType.Damage;
		this.m_player.ownerlessStatModifiers.Add(damageStat);
		this.m_player.stats.RecalculateStats(this.m_player, false, false);
		if (this.m_player.CurrentGun != null)
		{
			this.m_player.CurrentGun.ForceImmediateReload(false);
		}
		this.m_elapsed = 0f;
		float particleCounter = 0f;
		while (this.m_elapsed < this.Duration)
		{
			this.m_elapsed += BraveTime.DeltaTime;
			this.m_player.baseFlatColorOverride = this.flatColorOverride.WithAlpha(Mathf.Lerp(this.flatColorOverride.a, 0f, Mathf.Clamp01(this.m_elapsed - (this.Duration - 1f))));
			if (this.instanceVFX && this.m_elapsed > 1f)
			{
				this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
				this.instanceVFX = null;
			}
			if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && this.m_player && this.m_player.IsVisible && !this.m_player.IsFalling)
			{
				particleCounter += BraveTime.DeltaTime * 40f;
				if (particleCounter > 1f)
				{
					int num = Mathf.FloorToInt(particleCounter);
					particleCounter %= 1f;
					GlobalSparksDoer.DoRandomParticleBurst(num, this.m_player.sprite.WorldBottomLeft.ToVector3ZisY(0f), this.m_player.sprite.WorldTopRight.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
				}
			}
			yield return null;
		}
		if (this.instanceVFX)
		{
			this.instanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
		}
		this.m_player.ownerlessStatModifiers.Remove(damageStat);
		this.m_player.stats.RecalculateStats(this.m_player, false, false);
		this.m_isRaged = false;
		yield break;
	}

	// Token: 0x06007747 RID: 30535 RVA: 0x002F8D28 File Offset: 0x002F6F28
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<RagePassiveItem>().m_pickedUpThisRun = true;
		player.OnReceivedDamage -= this.HandleReceivedDamage;
		return debrisObject;
	}

	// Token: 0x06007748 RID: 30536 RVA: 0x002F8D5C File Offset: 0x002F6F5C
	protected override void OnDestroy()
	{
		if (this.m_player != null)
		{
			this.m_player.OnReceivedDamage -= this.HandleReceivedDamage;
		}
		base.OnDestroy();
	}

	// Token: 0x04007941 RID: 31041
	public float Duration = 3f;

	// Token: 0x04007942 RID: 31042
	public float DamageMultiplier = 2f;

	// Token: 0x04007943 RID: 31043
	public Color flatColorOverride = new Color(0.5f, 0f, 0f, 0.75f);

	// Token: 0x04007944 RID: 31044
	public GameObject OverheadVFX;

	// Token: 0x04007945 RID: 31045
	private bool m_isRaged;

	// Token: 0x04007946 RID: 31046
	private float m_elapsed;

	// Token: 0x04007947 RID: 31047
	private GameObject instanceVFX;

	// Token: 0x04007948 RID: 31048
	private PlayerController m_player;
}
