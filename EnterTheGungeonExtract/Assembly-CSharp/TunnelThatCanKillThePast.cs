using System;
using System.Collections;
using Kvant;
using Reaktion;
using UnityEngine;

// Token: 0x02000836 RID: 2102
public class TunnelThatCanKillThePast : MonoBehaviour
{
	// Token: 0x06002DB5 RID: 11701 RVA: 0x000EE674 File Offset: 0x000EC874
	private void Awake()
	{
		if (this.BulletSprite)
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				this.BulletSprite.ignoresTiltworldDepth = true;
				JitterMotion componentInChildren = base.GetComponentInChildren<JitterMotion>();
				if (componentInChildren)
				{
					componentInChildren.InfluenceAxialX = 10f;
					componentInChildren.InfluenceAxialY = 10f;
				}
			}
			else
			{
				this.BulletSprite.gameObject.SetActive(false);
				this.BulletSprite.renderer.enabled = false;
				this.BulletSprite = null;
			}
		}
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x000EE714 File Offset: 0x000EC914
	private void Start()
	{
		this.m_displacementID = Shader.PropertyToID("_Displacement");
		this.ShellRenderer.material.SetFloat("_Gain", 0.1f);
		this.ShellRenderer.material.SetFloat("_Brightness", 0.005f);
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM || GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW)
		{
			this.ShellRenderer.enabled = false;
		}
		else
		{
			this.ShellRenderer.enabled = true;
		}
		base.StartCoroutine(this.HandleBulletPosition());
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x000EE7D0 File Offset: 0x000EC9D0
	public void ChangeToPulsed()
	{
		this.m_timeSincePhaseTransition = 0f;
		this.m_currentPhase = TunnelThatCanKillThePast.CurrentTunnelPhase.PULSED;
	}

	// Token: 0x06002DB8 RID: 11704 RVA: 0x000EE7E4 File Offset: 0x000EC9E4
	public void ManuallySetShatterAmount(float amount)
	{
		this.m_standardDisplacement = amount;
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x000EE7F0 File Offset: 0x000EC9F0
	public void Shatter()
	{
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			BraveUtility.EnableEmission(componentsInChildren[i], false);
		}
		this.m_timeSincePhaseTransition = 0f;
		this.m_currentPhase = TunnelThatCanKillThePast.CurrentTunnelPhase.SHATTER;
	}

	// Token: 0x06002DBA RID: 11706 RVA: 0x000EE834 File Offset: 0x000ECA34
	private void Update()
	{
		this.m_timeSincePhaseTransition += GameManager.INVARIANT_DELTA_TIME;
		switch (this.m_currentPhase)
		{
		case TunnelThatCanKillThePast.CurrentTunnelPhase.STANDARD:
			this.UpdateStandard();
			break;
		case TunnelThatCanKillThePast.CurrentTunnelPhase.PULSED:
			this.UpdatePulsed();
			break;
		case TunnelThatCanKillThePast.CurrentTunnelPhase.SHATTER:
			this.UpdateShatter();
			break;
		}
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x000EE898 File Offset: 0x000ECA98
	private IEnumerator HandleBulletPosition()
	{
		if (!this.BulletSprite)
		{
			yield break;
		}
		do
		{
			float curZOffset = BraveMathCollege.SmoothLerp(-2f, 2f, Mathf.PingPong(Time.realtimeSinceStartup / 6f, 1f));
			Vector3 targetPos = new Vector3(this.BulletX, this.BulletY, this.BulletZ + curZOffset);
			this.BulletSprite.transform.localPosition = targetPos;
			yield return null;
		}
		while (!this.shattering);
		if (this.shattering)
		{
			float ela = 0f;
			float dura = 1f;
			while (ela < dura + 3f)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				float curZOffset2 = BraveMathCollege.SmoothLerp(-2f, 2f, Mathf.PingPong(Time.realtimeSinceStartup / 6f, 1f));
				float realZOffset = Mathf.Lerp(curZOffset2, 100f, Mathf.Clamp01((ela - 3f) / dura));
				Vector3 targetPos2 = new Vector3(this.BulletX, this.BulletY, this.BulletZ + realZOffset);
				this.BulletSprite.transform.localPosition = targetPos2;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002DBC RID: 11708 RVA: 0x000EE8B4 File Offset: 0x000ECAB4
	private void UpdateStandard()
	{
		this.targetMaterial.SetFloat(this.m_displacementID, this.m_standardDisplacement);
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x000EE8D0 File Offset: 0x000ECAD0
	private void UpdatePulsed()
	{
		this.targetMaterial.SetFloat(this.m_displacementID, Mathf.Lerp(this.MinPulse, this.MaxPulse, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.m_timeSincePhaseTransition, this.PulsePeriod) / this.PulsePeriod)));
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x000EE928 File Offset: 0x000ECB28
	private void UpdateShatter()
	{
		float num = Mathf.Lerp(this.m_standardDisplacement, -100f, this.ShatterCurve.Evaluate(this.m_timeSincePhaseTransition / this.ShatterTime));
		this.targetMaterial.SetFloat(this.m_displacementID, num);
		float num2 = Mathf.Lerp(0.005f, 0f, this.m_timeSincePhaseTransition / this.ShatterTime);
		float num3 = Mathf.Lerp(0.1f, 0f, this.m_timeSincePhaseTransition / this.ShatterTime);
		this.ShellRenderer.material.SetFloat("_Brightness", num2);
		this.ShellRenderer.material.SetFloat("_Gain", num3);
	}

	// Token: 0x04001EF9 RID: 7929
	public Tunnel targetTunnel;

	// Token: 0x04001EFA RID: 7930
	public Material targetMaterial;

	// Token: 0x04001EFB RID: 7931
	public MeshRenderer ShellRenderer;

	// Token: 0x04001EFC RID: 7932
	public float MinPulse = -1f;

	// Token: 0x04001EFD RID: 7933
	public float MaxPulse = 1f;

	// Token: 0x04001EFE RID: 7934
	public float PulsePeriod = 0.5f;

	// Token: 0x04001EFF RID: 7935
	public float ShatterTime = 1f;

	// Token: 0x04001F00 RID: 7936
	public AnimationCurve ShatterCurve;

	// Token: 0x04001F01 RID: 7937
	public tk2dSprite BulletSprite;

	// Token: 0x04001F02 RID: 7938
	[NonSerialized]
	public bool shattering;

	// Token: 0x04001F03 RID: 7939
	private TunnelThatCanKillThePast.CurrentTunnelPhase m_currentPhase;

	// Token: 0x04001F04 RID: 7940
	private float m_timeSincePhaseTransition;

	// Token: 0x04001F05 RID: 7941
	private int m_displacementID = -1;

	// Token: 0x04001F06 RID: 7942
	private float m_standardDisplacement;

	// Token: 0x04001F07 RID: 7943
	public float BulletX = 1.3f;

	// Token: 0x04001F08 RID: 7944
	public float BulletY = -1.5f;

	// Token: 0x04001F09 RID: 7945
	public float BulletZ = 10f;

	// Token: 0x02000837 RID: 2103
	public enum CurrentTunnelPhase
	{
		// Token: 0x04001F0B RID: 7947
		STANDARD,
		// Token: 0x04001F0C RID: 7948
		PULSED,
		// Token: 0x04001F0D RID: 7949
		SHATTER
	}
}
