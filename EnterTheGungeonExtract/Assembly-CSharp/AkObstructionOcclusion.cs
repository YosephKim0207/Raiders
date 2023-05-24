using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018FA RID: 6394
public abstract class AkObstructionOcclusion : MonoBehaviour
{
	// Token: 0x06009D98 RID: 40344 RVA: 0x003F061C File Offset: 0x003EE81C
	protected void InitIntervalsAndFadeRates()
	{
		this.refreshTime = UnityEngine.Random.Range(0f, this.refreshInterval);
		this.fadeRate = 1f / this.fadeTime;
	}

	// Token: 0x06009D99 RID: 40345 RVA: 0x003F0648 File Offset: 0x003EE848
	protected void UpdateObstructionOcclusionValues(List<AkAudioListener> listenerList)
	{
		for (int i = 0; i < listenerList.Count; i++)
		{
			if (!this.ObstructionOcclusionValues.ContainsKey(listenerList[i]))
			{
				this.ObstructionOcclusionValues.Add(listenerList[i], new AkObstructionOcclusion.ObstructionOcclusionValue());
			}
		}
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
		{
			if (!listenerList.Contains(keyValuePair.Key))
			{
				this.listenersToRemove.Add(keyValuePair.Key);
			}
		}
		for (int j = 0; j < this.listenersToRemove.Count; j++)
		{
			this.ObstructionOcclusionValues.Remove(this.listenersToRemove[j]);
		}
	}

	// Token: 0x06009D9A RID: 40346 RVA: 0x003F073C File Offset: 0x003EE93C
	protected void UpdateObstructionOcclusionValues(AkAudioListener listener)
	{
		if (!listener)
		{
			return;
		}
		if (!this.ObstructionOcclusionValues.ContainsKey(listener))
		{
			this.ObstructionOcclusionValues.Add(listener, new AkObstructionOcclusion.ObstructionOcclusionValue());
		}
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
		{
			if (listener != keyValuePair.Key)
			{
				this.listenersToRemove.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.listenersToRemove.Count; i++)
		{
			this.ObstructionOcclusionValues.Remove(this.listenersToRemove[i]);
		}
	}

	// Token: 0x06009D9B RID: 40347 RVA: 0x003F0818 File Offset: 0x003EEA18
	private void CastRays()
	{
		if (this.refreshTime > this.refreshInterval)
		{
			this.refreshTime -= this.refreshInterval;
			foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
			{
				AkAudioListener key = keyValuePair.Key;
				AkObstructionOcclusion.ObstructionOcclusionValue value = keyValuePair.Value;
				Vector3 vector = key.transform.position - base.transform.position;
				float magnitude = vector.magnitude;
				if (this.maxDistance > 0f && magnitude > this.maxDistance)
				{
					value.targetValue = value.currentValue;
				}
				else
				{
					value.targetValue = ((!Physics.Raycast(base.transform.position, vector / magnitude, magnitude, this.LayerMask.value)) ? 0f : 1f);
				}
			}
		}
		this.refreshTime += Time.deltaTime;
	}

	// Token: 0x06009D9C RID: 40348
	protected abstract void UpdateObstructionOcclusionValuesForListeners();

	// Token: 0x06009D9D RID: 40349
	protected abstract void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair);

	// Token: 0x06009D9E RID: 40350 RVA: 0x003F0948 File Offset: 0x003EEB48
	private void Update()
	{
		this.UpdateObstructionOcclusionValuesForListeners();
		this.CastRays();
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
		{
			if (keyValuePair.Value.Update(this.fadeRate))
			{
				this.SetObstructionOcclusion(keyValuePair);
			}
		}
	}

	// Token: 0x04009F1A RID: 40730
	private readonly List<AkAudioListener> listenersToRemove = new List<AkAudioListener>();

	// Token: 0x04009F1B RID: 40731
	private readonly Dictionary<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObstructionOcclusionValues = new Dictionary<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue>();

	// Token: 0x04009F1C RID: 40732
	protected float fadeRate;

	// Token: 0x04009F1D RID: 40733
	[Tooltip("Fade time in seconds")]
	public float fadeTime = 0.5f;

	// Token: 0x04009F1E RID: 40734
	[Tooltip("Layers of obstructers/occluders")]
	public LayerMask LayerMask = -1;

	// Token: 0x04009F1F RID: 40735
	[Tooltip("Maximum distance to perform the obstruction/occlusion. Negative values mean infinite")]
	public float maxDistance = -1f;

	// Token: 0x04009F20 RID: 40736
	[Tooltip("The number of seconds between raycasts")]
	public float refreshInterval = 1f;

	// Token: 0x04009F21 RID: 40737
	private float refreshTime;

	// Token: 0x020018FB RID: 6395
	protected class ObstructionOcclusionValue
	{
		// Token: 0x06009DA0 RID: 40352 RVA: 0x003F09D0 File Offset: 0x003EEBD0
		public bool Update(float fadeRate)
		{
			if (Mathf.Approximately(this.targetValue, this.currentValue))
			{
				return false;
			}
			this.currentValue += fadeRate * Mathf.Sign(this.targetValue - this.currentValue) * Time.deltaTime;
			this.currentValue = Mathf.Clamp(this.currentValue, 0f, 1f);
			return true;
		}

		// Token: 0x04009F22 RID: 40738
		public float currentValue;

		// Token: 0x04009F23 RID: 40739
		public float targetValue;
	}
}
