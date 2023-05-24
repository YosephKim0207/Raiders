using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001374 RID: 4980
public class CigaretteItem : MonoBehaviour
{
	// Token: 0x060070D9 RID: 28889 RVA: 0x002CCCF0 File Offset: 0x002CAEF0
	private void Start()
	{
		DebrisObject component = base.GetComponent<DebrisObject>();
		AkSoundEngine.PostEvent("Play_OBJ_cigarette_throw_01", base.gameObject);
		component.killTranslationOnBounce = false;
		if (component)
		{
			DebrisObject debrisObject = component;
			debrisObject.OnBounced = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnBounced, new Action<DebrisObject>(this.OnBounced));
			DebrisObject debrisObject2 = component;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.OnHitGround));
		}
		if (this.inAirVFX != null)
		{
			base.StartCoroutine(this.SpawnVFX());
		}
	}

	// Token: 0x060070DA RID: 28890 RVA: 0x002CCD8C File Offset: 0x002CAF8C
	private IEnumerator SpawnVFX()
	{
		while (this.m_inAir)
		{
			SpawnManager.SpawnVFX(this.inAirVFX, base.transform.position, Quaternion.identity, false);
			yield return new WaitForSeconds(0.33f);
		}
		yield break;
	}

	// Token: 0x060070DB RID: 28891 RVA: 0x002CCDA8 File Offset: 0x002CAFA8
	private void OnBounced(DebrisObject obj)
	{
		DeadlyDeadlyGoopManager.IgniteGoopsCircle(base.transform.position.XY(), 1f);
	}

	// Token: 0x060070DC RID: 28892 RVA: 0x002CCDC4 File Offset: 0x002CAFC4
	private void OnHitGround(DebrisObject obj)
	{
		this.OnBounced(obj);
		if (this.smokeSystem)
		{
			BraveUtility.EnableEmission(this.smokeSystem.GetComponent<ParticleSystem>(), false);
		}
		base.GetComponent<tk2dSpriteAnimator>().Stop();
		if (this.DestroyOnGrounded)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400705C RID: 28764
	public GameObject inAirVFX;

	// Token: 0x0400705D RID: 28765
	private bool m_inAir = true;

	// Token: 0x0400705E RID: 28766
	public GameObject smokeSystem;

	// Token: 0x0400705F RID: 28767
	public GameObject sparkVFX;

	// Token: 0x04007060 RID: 28768
	public bool DestroyOnGrounded;
}
