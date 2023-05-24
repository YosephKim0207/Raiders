using System;
using UnityEngine;

// Token: 0x02001614 RID: 5652
public abstract class ProjectileAndBeamMotionModule : ProjectileMotionModule
{
	// Token: 0x060083BF RID: 33727
	public abstract Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted);
}
