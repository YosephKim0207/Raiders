using System;
using UnityEngine;

// Token: 0x020017B2 RID: 6066
[Serializable]
public class CharacterSelectIdlePhase
{
	// Token: 0x040095DB RID: 38363
	public float holdMin = 4f;

	// Token: 0x040095DC RID: 38364
	public float holdMax = 10f;

	// Token: 0x040095DD RID: 38365
	public string inAnimation = string.Empty;

	// Token: 0x040095DE RID: 38366
	public string holdAnimation = string.Empty;

	// Token: 0x040095DF RID: 38367
	public float optionalHoldChance = 0.5f;

	// Token: 0x040095E0 RID: 38368
	public string optionalHoldIdleAnimation = string.Empty;

	// Token: 0x040095E1 RID: 38369
	public string outAnimation = string.Empty;

	// Token: 0x040095E2 RID: 38370
	[Header("Optional VFX")]
	public CharacterSelectIdlePhase.VFXPhaseTrigger vfxTrigger;

	// Token: 0x040095E3 RID: 38371
	public float vfxHoldPeriod = 1f;

	// Token: 0x040095E4 RID: 38372
	public tk2dSpriteAnimator vfxSpriteAnimator;

	// Token: 0x040095E5 RID: 38373
	public tk2dSpriteAnimator endVFXSpriteAnimator;

	// Token: 0x020017B3 RID: 6067
	public enum VFXPhaseTrigger
	{
		// Token: 0x040095E7 RID: 38375
		NONE,
		// Token: 0x040095E8 RID: 38376
		IN,
		// Token: 0x040095E9 RID: 38377
		HOLD,
		// Token: 0x040095EA RID: 38378
		OUT
	}
}
