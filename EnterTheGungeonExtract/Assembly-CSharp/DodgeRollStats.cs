using System;
using UnityEngine;

// Token: 0x020015F7 RID: 5623
[Serializable]
public class DodgeRollStats
{
	// Token: 0x06008279 RID: 33401 RVA: 0x00355308 File Offset: 0x00353508
	public float GetModifiedTime(PlayerController owner)
	{
		float num = 1f;
		if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.IsEndTimes)
		{
			return this.time;
		}
		if (PassiveItem.IsFlagSetForCharacter(owner, typeof(SunglassesItem)) && SunglassesItem.SunglassesActive)
		{
			num *= 0.75f;
		}
		float statModifier = owner.stats.GetStatModifier(PlayerStats.StatType.DodgeRollSpeedMultiplier);
		float num2 = ((statModifier == 0f) ? 1f : (1f / statModifier));
		return this.time * this.rollTimeMultiplier * num * num2;
	}

	// Token: 0x0600827A RID: 33402 RVA: 0x003553B0 File Offset: 0x003535B0
	public float GetModifiedDistance(PlayerController owner)
	{
		float num = 1f;
		if (PassiveItem.IsFlagSetForCharacter(owner, typeof(SunglassesItem)) && SunglassesItem.SunglassesActive)
		{
			num *= 1.25f;
		}
		float statModifier = owner.stats.GetStatModifier(PlayerStats.StatType.DodgeRollDistanceMultiplier);
		float statModifier2 = owner.stats.GetStatModifier(PlayerStats.StatType.MovementSpeed);
		float num2 = (statModifier2 - 1f) * 0.5f + 1f;
		return this.distance * this.rollDistanceMultiplier * num2 * num * this.blinkDistanceMultiplier * statModifier;
	}

	// Token: 0x04008580 RID: 34176
	[HideInInspector]
	public bool hasPreDodgeDelay;

	// Token: 0x04008581 RID: 34177
	[TogglableProperty("hasPreDodgeDelay", "Pre-Dodge Delay")]
	public float preDodgeDelay;

	// Token: 0x04008582 RID: 34178
	public float time;

	// Token: 0x04008583 RID: 34179
	public float distance;

	// Token: 0x04008584 RID: 34180
	[NonSerialized]
	public int additionalInvulnerabilityFrames;

	// Token: 0x04008585 RID: 34181
	[NonSerialized]
	public float blinkDistanceMultiplier = 1f;

	// Token: 0x04008586 RID: 34182
	[NonSerialized]
	public float rollTimeMultiplier = 1f;

	// Token: 0x04008587 RID: 34183
	[NonSerialized]
	public float rollDistanceMultiplier = 1f;

	// Token: 0x04008588 RID: 34184
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve speed;

	// Token: 0x04008589 RID: 34185
	private const float c_moveSpeedToRollDistanceConversion = 0.5f;
}
