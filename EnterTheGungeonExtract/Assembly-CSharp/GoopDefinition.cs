using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001172 RID: 4466
public class GoopDefinition : ScriptableObject
{
	// Token: 0x0600632E RID: 25390 RVA: 0x00267348 File Offset: 0x00265548
	public float GetLifespan(float radialFraction)
	{
		float num = float.MaxValue;
		if (this.usesLifespan)
		{
			num = Mathf.Lerp(this.lifespan, this.lifespan - this.lifespanRadialReduction, radialFraction);
		}
		return num;
	}

	// Token: 0x04005E4F RID: 24143
	public Texture2D goopTexture;

	// Token: 0x04005E50 RID: 24144
	public Texture2D worldTexture;

	// Token: 0x04005E51 RID: 24145
	public bool usesWorldTextureByDefault;

	// Token: 0x04005E52 RID: 24146
	public Color32 baseColor32 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005E53 RID: 24147
	public bool usesLifespan = true;

	// Token: 0x04005E54 RID: 24148
	[ShowInInspectorIf("usesLifespan", false)]
	public float lifespan = 10f;

	// Token: 0x04005E55 RID: 24149
	[ShowInInspectorIf("usesLifespan", false)]
	public float fadePeriod = 2f;

	// Token: 0x04005E56 RID: 24150
	[ShowInInspectorIf("usesLifespan", false)]
	public Color32 fadeColor32 = new Color32(128, 128, 128, 0);

	// Token: 0x04005E57 RID: 24151
	[ShowInInspectorIf("usesLifespan", false)]
	public float lifespanRadialReduction = 3f;

	// Token: 0x04005E58 RID: 24152
	public bool damagesPlayers;

	// Token: 0x04005E59 RID: 24153
	[ShowInInspectorIf("damagesPlayers", false)]
	public float damageToPlayers = 0.5f;

	// Token: 0x04005E5A RID: 24154
	[ShowInInspectorIf("damagesPlayers", false)]
	public float delayBeforeDamageToPlayers = 0.5f;

	// Token: 0x04005E5B RID: 24155
	public CoreDamageTypes damageTypes;

	// Token: 0x04005E5C RID: 24156
	public bool damagesEnemies;

	// Token: 0x04005E5D RID: 24157
	[ShowInInspectorIf("damagesEnemies", false)]
	public float damagePerSecondtoEnemies = 10f;

	// Token: 0x04005E5E RID: 24158
	public List<GoopDefinition.GoopDamageTypeInteraction> goopDamageTypeInteractions;

	// Token: 0x04005E5F RID: 24159
	public bool usesAmbientGoopFX;

	// Token: 0x04005E60 RID: 24160
	[ShowInInspectorIf("usesAmbientGoopFX", false)]
	public float ambientGoopFXChance = 0.01f;

	// Token: 0x04005E61 RID: 24161
	[ShowInInspectorIf("usesAmbientGoopFX", false)]
	public VFXPool ambientGoopFX;

	// Token: 0x04005E62 RID: 24162
	public bool usesAcidAudio;

	// Token: 0x04005E63 RID: 24163
	public bool isOily;

	// Token: 0x04005E64 RID: 24164
	public bool usesWaterVfx = true;

	// Token: 0x04005E65 RID: 24165
	public bool eternal;

	// Token: 0x04005E66 RID: 24166
	public bool usesOverrideOpaqueness;

	// Token: 0x04005E67 RID: 24167
	public float overrideOpaqueness = 0.5f;

	// Token: 0x04005E68 RID: 24168
	[Header("On Fire Settings")]
	public bool CanBeIgnited;

	// Token: 0x04005E69 RID: 24169
	public float igniteSpreadTime = 0.1f;

	// Token: 0x04005E6A RID: 24170
	public bool SelfIgnites;

	// Token: 0x04005E6B RID: 24171
	public float selfIgniteDelay = 0.5f;

	// Token: 0x04005E6C RID: 24172
	public bool ignitionChangesLifetime;

	// Token: 0x04005E6D RID: 24173
	[ShowInInspectorIf("ignitionChangesLifetime", false)]
	public float ignitedLifetime = 5f;

	// Token: 0x04005E6E RID: 24174
	public bool playerStepsChangeLifetime;

	// Token: 0x04005E6F RID: 24175
	[ShowInInspectorIf("playerStepsChangeLifetime", false)]
	public float playerStepsLifetime = 2f;

	// Token: 0x04005E70 RID: 24176
	public float fireDamageToPlayer = 0.5f;

	// Token: 0x04005E71 RID: 24177
	public float fireDamagePerSecondToEnemies = 10f;

	// Token: 0x04005E72 RID: 24178
	[ShowInInspectorIf("CanBeIgnited", false)]
	public bool fireBurnsEnemies = true;

	// Token: 0x04005E73 RID: 24179
	[ShowInInspectorIf("CanBeIgnited", false)]
	public GameActorFireEffect fireEffect;

	// Token: 0x04005E74 RID: 24180
	public Color32 igniteColor32 = new Color32(byte.MaxValue, 128, 128, byte.MaxValue);

	// Token: 0x04005E75 RID: 24181
	public Color32 fireColor32 = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	// Token: 0x04005E76 RID: 24182
	[ShowInInspectorIf("CanBeIgnited", false)]
	public bool UsesGreenFire;

	// Token: 0x04005E77 RID: 24183
	[Header("On Electrified Settings")]
	public bool CanBeElectrified;

	// Token: 0x04005E78 RID: 24184
	public float electrifiedDamageToPlayer = 0.5f;

	// Token: 0x04005E79 RID: 24185
	public float electrifiedDamagePerSecondToEnemies = 10f;

	// Token: 0x04005E7A RID: 24186
	public float electrifiedTime = 2f;

	// Token: 0x04005E7B RID: 24187
	[Header("On Frozen Settings")]
	public bool CanBeFrozen;

	// Token: 0x04005E7C RID: 24188
	public float freezeLifespan = 10f;

	// Token: 0x04005E7D RID: 24189
	public float freezeSpreadTime = 0.1f;

	// Token: 0x04005E7E RID: 24190
	public Color32 prefreezeColor32 = new Color32(238, 240, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005E7F RID: 24191
	public Color32 frozenColor32 = new Color32(238, 240, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005E80 RID: 24192
	[Header("Status Effects")]
	public bool AppliesSpeedModifier;

	// Token: 0x04005E81 RID: 24193
	public bool AppliesSpeedModifierContinuously;

	// Token: 0x04005E82 RID: 24194
	public GameActorSpeedEffect SpeedModifierEffect;

	// Token: 0x04005E83 RID: 24195
	public bool AppliesDamageOverTime;

	// Token: 0x04005E84 RID: 24196
	public GameActorHealthEffect HealthModifierEffect;

	// Token: 0x04005E85 RID: 24197
	public bool DrainsAmmo;

	// Token: 0x04005E86 RID: 24198
	public float PercentAmmoDrainPerSecond = 0.1f;

	// Token: 0x04005E87 RID: 24199
	public bool AppliesCharm;

	// Token: 0x04005E88 RID: 24200
	public GameActorCharmEffect CharmModifierEffect;

	// Token: 0x04005E89 RID: 24201
	public bool AppliesCheese;

	// Token: 0x04005E8A RID: 24202
	public GameActorCheeseEffect CheeseModifierEffect;

	// Token: 0x02001173 RID: 4467
	[Serializable]
	public class GoopDamageTypeInteraction
	{
		// Token: 0x04005E8B RID: 24203
		[EnumFlags]
		public CoreDamageTypes damageType;

		// Token: 0x04005E8C RID: 24204
		public bool electrifiesGoop;

		// Token: 0x04005E8D RID: 24205
		public bool freezesGoop;

		// Token: 0x04005E8E RID: 24206
		public GoopDefinition.GoopDamageTypeInteraction.GoopIgnitionMode ignitionMode;

		// Token: 0x02001174 RID: 4468
		public enum GoopIgnitionMode
		{
			// Token: 0x04005E90 RID: 24208
			NONE,
			// Token: 0x04005E91 RID: 24209
			IGNITE,
			// Token: 0x04005E92 RID: 24210
			DOUSE
		}
	}
}
