using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200007B RID: 123
[InspectorDropdownName("Bosses/BossFinalConvict/SpinFire1")]
public class BossFinalConvictSpinFire1 : Script
{
	// Token: 0x060001DB RID: 475 RVA: 0x0000943C File Offset: 0x0000763C
	protected override IEnumerator Top()
	{
		Animation animation = this.BulletManager.GetUnityAnimation();
		AnimationClip clip = animation.GetClip("BossFinalConvictSpinAttack");
		for (int i = 0; i < 48; i++)
		{
			clip.SampleAnimation(animation.gameObject, (float)i / 60f);
			base.Fire(new Offset("left hand shoot point"), new Direction((float)UnityEngine.Random.Range(-15, 15), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), null);
			base.Fire(new Offset("right hand shoot point"), new Direction((float)UnityEngine.Random.Range(-15, 15), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), null);
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x040001EF RID: 495
	private const int NumBullets = 48;
}
