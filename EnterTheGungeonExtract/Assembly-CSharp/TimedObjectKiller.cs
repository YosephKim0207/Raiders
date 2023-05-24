using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001719 RID: 5913
public class TimedObjectKiller : MonoBehaviour
{
	// Token: 0x06008954 RID: 35156 RVA: 0x00390AB0 File Offset: 0x0038ECB0
	public void Start()
	{
		if (TimedObjectKiller.m_lightCullingMaskID == -1)
		{
			TimedObjectKiller.m_lightCullingMaskID = (1 << LayerMask.NameToLayer("BG_Critical")) | (1 << LayerMask.NameToLayer("BG_Nonsense"));
		}
		if (SpawnManager.IsPooled(base.gameObject))
		{
			this.m_poolType = TimedObjectKiller.PoolType.Pooled;
		}
		else
		{
			this.m_poolType = TimedObjectKiller.PoolType.NonPooled;
			Transform transform = base.transform.parent;
			while (transform)
			{
				if (SpawnManager.IsPooled(transform.gameObject))
				{
					this.m_poolType = TimedObjectKiller.PoolType.SonOfPooled;
					break;
				}
				transform = transform.parent;
			}
		}
		if (this.m_poolType == TimedObjectKiller.PoolType.SonOfPooled)
		{
			this.m_light = base.GetComponent<Light>();
			if (this.m_light != null)
			{
				this.m_light.cullingMask = TimedObjectKiller.m_lightCullingMaskID;
			}
			this.m_renderer = base.GetComponent<Renderer>();
		}
		this.Init();
	}

	// Token: 0x06008955 RID: 35157 RVA: 0x00390B9C File Offset: 0x0038ED9C
	private void Init()
	{
		base.StartCoroutine(this.HandleDeath());
	}

	// Token: 0x06008956 RID: 35158 RVA: 0x00390BAC File Offset: 0x0038EDAC
	private IEnumerator HandleDeath()
	{
		yield return new WaitForSeconds(this.lifeTime);
		if (this.m_poolType == TimedObjectKiller.PoolType.SonOfPooled)
		{
			if (this.m_light)
			{
				this.m_light.enabled = false;
			}
			if (this.m_renderer)
			{
				this.m_renderer.enabled = false;
			}
		}
		else
		{
			SpawnManager.Despawn(base.gameObject);
		}
		yield break;
	}

	// Token: 0x06008957 RID: 35159 RVA: 0x00390BC8 File Offset: 0x0038EDC8
	public void OnSpawned()
	{
		if (base.enabled)
		{
			if (this.m_light)
			{
				this.m_light.enabled = true;
			}
			if (this.m_renderer)
			{
				this.m_renderer.enabled = true;
			}
			this.Start();
		}
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x00390C20 File Offset: 0x0038EE20
	public void OnDespawned()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04008F5C RID: 36700
	public float lifeTime = 1f;

	// Token: 0x04008F5D RID: 36701
	private static int m_lightCullingMaskID = -1;

	// Token: 0x04008F5E RID: 36702
	public TimedObjectKiller.PoolType m_poolType;

	// Token: 0x04008F5F RID: 36703
	private Light m_light;

	// Token: 0x04008F60 RID: 36704
	private Renderer m_renderer;

	// Token: 0x0200171A RID: 5914
	public enum PoolType
	{
		// Token: 0x04008F62 RID: 36706
		Pooled,
		// Token: 0x04008F63 RID: 36707
		SonOfPooled,
		// Token: 0x04008F64 RID: 36708
		NonPooled
	}
}
