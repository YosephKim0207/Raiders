using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E23 RID: 3619
public class RuntimeGameActorEffectData
{
	// Token: 0x04004295 RID: 17045
	public GameActor actor;

	// Token: 0x04004296 RID: 17046
	public float elapsed;

	// Token: 0x04004297 RID: 17047
	public float tickCounter;

	// Token: 0x04004298 RID: 17048
	public GameActor.MovementModifier MovementModifier;

	// Token: 0x04004299 RID: 17049
	public Action<Vector2> OnActorPreDeath;

	// Token: 0x0400429A RID: 17050
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> OnFlameAnimationCompleted;

	// Token: 0x0400429B RID: 17051
	public float onActorVFXTimer;

	// Token: 0x0400429C RID: 17052
	public tk2dBaseSprite instanceOverheadVFX;

	// Token: 0x0400429D RID: 17053
	public float accumulator;

	// Token: 0x0400429E RID: 17054
	public bool destroyVfx;

	// Token: 0x0400429F RID: 17055
	public List<Tuple<GameObject, float>> vfxObjects;
}
