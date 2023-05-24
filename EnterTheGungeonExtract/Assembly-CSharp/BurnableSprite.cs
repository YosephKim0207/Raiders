using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200111A RID: 4378
public class BurnableSprite : MonoBehaviour
{
	// Token: 0x0600609A RID: 24730 RVA: 0x00252FB0 File Offset: 0x002511B0
	public void Initialize()
	{
		SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody speculativeRigidbody = component;
		speculativeRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
	}

	// Token: 0x0600609B RID: 24731 RVA: 0x00252FE8 File Offset: 0x002511E8
	public void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_isBurning)
		{
			return;
		}
		Projectile component = rigidbodyCollision.OtherRigidbody.GetComponent<Projectile>();
		if (component != null)
		{
			this.Burn();
		}
	}

	// Token: 0x0600609C RID: 24732 RVA: 0x00253020 File Offset: 0x00251220
	public void Burn()
	{
		this.m_isBurning = true;
		this.burnParticleSystem = SpawnManager.SpawnParticleSystem(BraveResources.Load<GameObject>("BurningSpriteEffect", ".prefab"));
		this.burnParticleSystem.transform.parent = base.transform;
		this.burnParticleSystem.transform.localPosition = new Vector3(0.5f, 0f, 0f);
		base.StartCoroutine(this.HandleBurning());
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x00253098 File Offset: 0x00251298
	private IEnumerator HandleBurning()
	{
		float elapsed = 0f;
		Material material = base.GetComponent<Renderer>().material;
		float spriteHeight = material.GetFloat("_PixelHeight") / 16f;
		while (elapsed < this.burnDuration)
		{
			elapsed += BraveTime.DeltaTime;
			float percentComplete = elapsed / this.burnDuration;
			material.SetFloat("_Threshold", percentComplete);
			this.burnParticleSystem.transform.localPosition = this.burnParticleSystem.transform.localPosition.WithY(percentComplete * spriteHeight);
			yield return null;
		}
		UnityEngine.Object.Destroy(this.burnParticleSystem);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04005B41 RID: 23361
	public float burnDuration = 2f;

	// Token: 0x04005B42 RID: 23362
	private GameObject burnParticleSystem;

	// Token: 0x04005B43 RID: 23363
	private bool m_isBurning;
}
