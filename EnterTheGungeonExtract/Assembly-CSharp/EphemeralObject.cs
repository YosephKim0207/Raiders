using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001641 RID: 5697
public abstract class EphemeralObject : ClusteredTimeInvariantMonoBehaviour
{
	// Token: 0x0600850E RID: 34062 RVA: 0x0036DC70 File Offset: 0x0036BE70
	public virtual void Start()
	{
		this.OnSpawned();
	}

	// Token: 0x0600850F RID: 34063 RVA: 0x0036DC78 File Offset: 0x0036BE78
	public virtual void OnSpawned()
	{
		if (!this.m_isRegistered)
		{
			SpawnManager.RegisterEphemeralObject(this);
			this.m_isRegistered = true;
		}
	}

	// Token: 0x06008510 RID: 34064 RVA: 0x0036DC94 File Offset: 0x0036BE94
	protected override void OnDestroy()
	{
		this.OnDespawned();
		base.OnDestroy();
	}

	// Token: 0x06008511 RID: 34065 RVA: 0x0036DCA4 File Offset: 0x0036BEA4
	public virtual void OnDespawned()
	{
		if (this.m_isRegistered)
		{
			if (SpawnManager.HasInstance)
			{
				SpawnManager.DeregisterEphemeralObject(this);
			}
			this.m_isRegistered = false;
		}
	}

	// Token: 0x06008512 RID: 34066 RVA: 0x0036DCC8 File Offset: 0x0036BEC8
	public void TriggerDestruction(bool forceImmediate = false)
	{
		SpawnManager.DeregisterEphemeralObject(this);
		if (base.gameObject.activeInHierarchy && !forceImmediate)
		{
			base.StartCoroutine(this.DestroyCR());
		}
		else
		{
			SpawnManager.Despawn(base.gameObject);
		}
	}

	// Token: 0x06008513 RID: 34067 RVA: 0x0036DD04 File Offset: 0x0036BF04
	private IEnumerator DestroyCR()
	{
		float timer = 0f;
		tk2dSprite sprite = base.GetComponent<tk2dSprite>();
		Color startColor = ((!sprite) ? Color.white : sprite.color);
		while (timer < 1f)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			if (sprite)
			{
				Color color = sprite.color;
				color.a = Mathf.Lerp(startColor.a, 0f, timer / 1f);
				sprite.color = color;
			}
		}
		if (sprite)
		{
			sprite.color = startColor;
		}
		SpawnManager.Despawn(base.gameObject);
		yield break;
	}

	// Token: 0x040088EC RID: 35052
	public EphemeralObject.EphemeralPriority Priority = EphemeralObject.EphemeralPriority.Middling;

	// Token: 0x040088ED RID: 35053
	private float m_destructionTimer;

	// Token: 0x040088EE RID: 35054
	private bool m_isRegistered;

	// Token: 0x040088EF RID: 35055
	private const float c_destroyTime = 1f;

	// Token: 0x02001642 RID: 5698
	public enum EphemeralPriority
	{
		// Token: 0x040088F1 RID: 35057
		Critical,
		// Token: 0x040088F2 RID: 35058
		Important,
		// Token: 0x040088F3 RID: 35059
		Middling,
		// Token: 0x040088F4 RID: 35060
		Minor,
		// Token: 0x040088F5 RID: 35061
		Ephemeral
	}
}
