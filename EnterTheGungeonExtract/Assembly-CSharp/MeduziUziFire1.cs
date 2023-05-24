using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000248 RID: 584
[InspectorDropdownName("Bosses/Meduzi/UziFire1")]
public abstract class MeduziUziFire1 : Script
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x0002B63C File Offset: 0x0002983C
	protected override IEnumerator Top()
	{
		Animation animation = this.BulletManager.GetUnityAnimation();
		AnimationClip clip = animation.GetClip(this.UnityAnimationName);
		for (int i = 0; i < 60; i++)
		{
			clip.SampleAnimation(animation.gameObject, (float)i / 60f);
			base.Fire(new Offset("left hand shoot point"), new Direction((float)UnityEngine.Random.Range(-15, 15), DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), null);
			base.Fire(new Offset("right hand shoot point"), new Direction((float)UnityEngine.Random.Range(-15, 15), DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060008D6 RID: 2262
	protected abstract string UnityAnimationName { get; }

	// Token: 0x040008F8 RID: 2296
	private const int NumBullets = 60;
}
