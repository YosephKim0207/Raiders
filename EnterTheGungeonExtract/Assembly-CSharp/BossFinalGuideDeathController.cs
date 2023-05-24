using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000FD9 RID: 4057
public class BossFinalGuideDeathController : BraveBehaviour
{
	// Token: 0x0600587C RID: 22652 RVA: 0x0021D4AC File Offset: 0x0021B6AC
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(5f);
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x0021D4E8 File Offset: 0x0021B6E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x0021D4F0 File Offset: 0x0021B6F0
	private void OnBossDeath(Vector2 dir)
	{
		base.aiAnimator.ChildAnimator.gameObject.SetActive(false);
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		base.StartCoroutine(this.HandlePostDeathExplosionCR());
		base.healthHaver.OnPreDeath -= this.OnBossDeath;
		GameObject gameObject = GameObject.Find("BossFinalGuide_DrWolf(Clone)");
		if (gameObject)
		{
			HealthHaver component = gameObject.GetComponent<HealthHaver>();
			component.healthIsNumberOfHits = false;
			component.ApplyDamage(10000f, Vector2.zero, "Boss Death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
		}
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x0021D590 File Offset: 0x0021B790
	private IEnumerator HandlePostDeathExplosionCR()
	{
		while (base.aiAnimator.IsPlaying("death"))
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		if (base.aiActor)
		{
			UnityEngine.Object.Destroy(base.aiActor);
		}
		if (base.healthHaver)
		{
			UnityEngine.Object.Destroy(base.healthHaver);
		}
		if (base.behaviorSpeculator)
		{
			UnityEngine.Object.Destroy(base.behaviorSpeculator);
		}
		base.RegenerateCache();
		base.specRigidbody.PixelColliders[1].ManualHeight = 32;
		base.specRigidbody.RegenerateColliders = true;
		base.specRigidbody.CollideWithOthers = true;
		GuidePastController gpc = UnityEngine.Object.FindObjectOfType<GuidePastController>();
		gpc.OnBossKilled();
		yield break;
	}
}
