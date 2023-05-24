using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018F1 RID: 6385
[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("Wwise/AkGameObj")]
public class AkGameObj : MonoBehaviour
{
	// Token: 0x170016FF RID: 5887
	// (get) Token: 0x06009D5F RID: 40287 RVA: 0x003EF718 File Offset: 0x003ED918
	public bool IsUsingDefaultListeners
	{
		get
		{
			return this.m_listeners.useDefaultListeners;
		}
	}

	// Token: 0x17001700 RID: 5888
	// (get) Token: 0x06009D60 RID: 40288 RVA: 0x003EF728 File Offset: 0x003ED928
	public List<AkAudioListener> ListenerList
	{
		get
		{
			return this.m_listeners.ListenerList;
		}
	}

	// Token: 0x06009D61 RID: 40289 RVA: 0x003EF738 File Offset: 0x003ED938
	internal void AddListener(AkAudioListener listener)
	{
		this.m_listeners.Add(listener);
	}

	// Token: 0x06009D62 RID: 40290 RVA: 0x003EF748 File Offset: 0x003ED948
	internal void RemoveListener(AkAudioListener listener)
	{
		this.m_listeners.Remove(listener);
	}

	// Token: 0x06009D63 RID: 40291 RVA: 0x003EF758 File Offset: 0x003ED958
	public AKRESULT Register()
	{
		if (this.isRegistered)
		{
			return AKRESULT.AK_Success;
		}
		this.isRegistered = true;
		return AkSoundEngine.RegisterGameObj(base.gameObject, base.gameObject.name);
	}

	// Token: 0x06009D64 RID: 40292 RVA: 0x003EF784 File Offset: 0x003ED984
	private void Awake()
	{
		if (!this.isStaticObject)
		{
			this.m_posData = new AkGameObjPositionData();
		}
		this.m_Collider = base.GetComponent<Collider>();
		if (this.Register() == AKRESULT.AK_Success)
		{
			AkSoundEngine.SetObjectPosition(base.gameObject, this.GetPosition(), this.GetForward(), this.GetUpward());
			if (this.isEnvironmentAware)
			{
				this.m_envData = new AkGameObjEnvironmentData();
				if (this.m_Collider)
				{
					this.m_envData.AddAkEnvironment(this.m_Collider, this.m_Collider);
				}
				this.m_envData.UpdateAuxSend(base.gameObject, base.transform.position);
			}
			this.m_listeners.Init(this);
		}
	}

	// Token: 0x06009D65 RID: 40293 RVA: 0x003EF844 File Offset: 0x003EDA44
	private void CheckStaticStatus()
	{
	}

	// Token: 0x06009D66 RID: 40294 RVA: 0x003EF848 File Offset: 0x003EDA48
	private void OnEnable()
	{
		base.enabled = !this.isStaticObject;
	}

	// Token: 0x06009D67 RID: 40295 RVA: 0x003EF85C File Offset: 0x003EDA5C
	private void OnDestroy()
	{
		AkUnityEventHandler[] components = base.gameObject.GetComponents<AkUnityEventHandler>();
		foreach (AkUnityEventHandler akUnityEventHandler in components)
		{
			if (akUnityEventHandler.triggerList.Contains(-358577003))
			{
				akUnityEventHandler.DoDestroy();
			}
		}
		if (AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.UnregisterGameObj(base.gameObject);
		}
	}

	// Token: 0x06009D68 RID: 40296 RVA: 0x003EF8C0 File Offset: 0x003EDAC0
	private void Update()
	{
		if (this.m_envData != null)
		{
			this.m_envData.UpdateAuxSend(base.gameObject, base.transform.position);
		}
		if (this.isStaticObject)
		{
			return;
		}
		Vector3 position = this.GetPosition();
		Vector3 forward = this.GetForward();
		Vector3 upward = this.GetUpward();
		if (this.m_posData.position == position && this.m_posData.forward == forward && this.m_posData.up == upward)
		{
			return;
		}
		this.m_posData.position = position;
		this.m_posData.forward = forward;
		this.m_posData.up = upward;
		AkSoundEngine.SetObjectPosition(base.gameObject, position, forward, upward);
	}

	// Token: 0x06009D69 RID: 40297 RVA: 0x003EF98C File Offset: 0x003EDB8C
	public Vector3 GetPosition()
	{
		Vector3 vector2;
		if (this.m_positionOffsetData != null)
		{
			Vector3 vector = base.transform.rotation * this.m_positionOffsetData.positionOffset;
			vector2 = base.transform.position + vector;
		}
		else
		{
			vector2 = base.transform.position;
		}
		return vector2.WithZ(vector2.y);
	}

	// Token: 0x06009D6A RID: 40298 RVA: 0x003EF9F0 File Offset: 0x003EDBF0
	public virtual Vector3 GetForward()
	{
		return base.transform.forward;
	}

	// Token: 0x06009D6B RID: 40299 RVA: 0x003EFA00 File Offset: 0x003EDC00
	public virtual Vector3 GetUpward()
	{
		return base.transform.up;
	}

	// Token: 0x06009D6C RID: 40300 RVA: 0x003EFA10 File Offset: 0x003EDC10
	private void OnTriggerEnter(Collider other)
	{
		if (this.isEnvironmentAware && this.m_envData != null)
		{
			this.m_envData.AddAkEnvironment(other, this.m_Collider);
		}
	}

	// Token: 0x06009D6D RID: 40301 RVA: 0x003EFA3C File Offset: 0x003EDC3C
	private void OnTriggerExit(Collider other)
	{
		if (this.isEnvironmentAware && this.m_envData != null)
		{
			this.m_envData.RemoveAkEnvironment(other, this.m_Collider);
		}
	}

	// Token: 0x04009EDF RID: 40671
	[SerializeField]
	private AkGameObjListenerList m_listeners = new AkGameObjListenerList();

	// Token: 0x04009EE0 RID: 40672
	public bool isEnvironmentAware = true;

	// Token: 0x04009EE1 RID: 40673
	[SerializeField]
	private bool isStaticObject;

	// Token: 0x04009EE2 RID: 40674
	private Collider m_Collider;

	// Token: 0x04009EE3 RID: 40675
	private AkGameObjEnvironmentData m_envData;

	// Token: 0x04009EE4 RID: 40676
	private AkGameObjPositionData m_posData;

	// Token: 0x04009EE5 RID: 40677
	public AkGameObjPositionOffsetData m_positionOffsetData;

	// Token: 0x04009EE6 RID: 40678
	private bool isRegistered;

	// Token: 0x04009EE7 RID: 40679
	[SerializeField]
	private AkGameObjPosOffsetData m_posOffsetData;

	// Token: 0x04009EE8 RID: 40680
	private const int AK_NUM_LISTENERS = 8;

	// Token: 0x04009EE9 RID: 40681
	[SerializeField]
	private int listenerMask = 1;
}
