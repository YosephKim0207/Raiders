using System;
using UnityEngine;

// Token: 0x02001843 RID: 6211
public class SmartOverheadVFXController : BraveBehaviour
{
	// Token: 0x06009300 RID: 37632 RVA: 0x003E0E04 File Offset: 0x003DF004
	public void Initialize(PlayerController attachTarget, Vector3 offset)
	{
		this.m_playerInitialized = true;
		this.m_attachedPlayer = attachTarget;
		this.m_originalOffset = base.transform.localPosition.Quantize(0.0625f, VectorConversions.Floor);
	}

	// Token: 0x06009301 RID: 37633 RVA: 0x003E0E30 File Offset: 0x003DF030
	public void OnDespawned()
	{
		this.m_playerInitialized = false;
		this.m_attachedPlayer = null;
		this.m_originalOffset = Vector3.zero;
	}

	// Token: 0x06009302 RID: 37634 RVA: 0x003E0E4C File Offset: 0x003DF04C
	private void Update()
	{
		if (this.m_playerInitialized)
		{
			if (this.m_attachedPlayer.healthHaver.IsDead)
			{
				SpawnManager.Despawn(base.gameObject);
			}
			Vector3 vector = this.m_originalOffset;
			if (GameUIRoot.Instance.GetReloadBarForPlayer(this.m_attachedPlayer).AnyStatusBarVisible())
			{
				vector += new Vector3(0f, 1.25f, 0f);
			}
			base.transform.localPosition = vector;
		}
		if (this.offset != Vector2.zero)
		{
			base.transform.localPosition += this.offset;
		}
	}

	// Token: 0x04009A92 RID: 39570
	public Vector2 offset;

	// Token: 0x04009A93 RID: 39571
	private PlayerController m_attachedPlayer;

	// Token: 0x04009A94 RID: 39572
	private Vector3 m_originalOffset;

	// Token: 0x04009A95 RID: 39573
	private bool m_playerInitialized;
}
