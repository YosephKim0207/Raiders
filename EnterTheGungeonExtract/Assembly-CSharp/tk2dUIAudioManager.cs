using System;
using UnityEngine;

// Token: 0x02000C17 RID: 3095
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIAudioManager")]
public class tk2dUIAudioManager : MonoBehaviour
{
	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x06004260 RID: 16992 RVA: 0x001575B0 File Offset: 0x001557B0
	public static tk2dUIAudioManager Instance
	{
		get
		{
			if (tk2dUIAudioManager.instance == null)
			{
				tk2dUIAudioManager.instance = UnityEngine.Object.FindObjectOfType(typeof(tk2dUIAudioManager)) as tk2dUIAudioManager;
				if (tk2dUIAudioManager.instance == null)
				{
					tk2dUIAudioManager.instance = new GameObject("tk2dUIAudioManager").AddComponent<tk2dUIAudioManager>();
				}
			}
			return tk2dUIAudioManager.instance;
		}
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x00157610 File Offset: 0x00155810
	private void Awake()
	{
		if (tk2dUIAudioManager.instance == null)
		{
			tk2dUIAudioManager.instance = this;
		}
		else if (tk2dUIAudioManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.Setup();
	}

	// Token: 0x06004262 RID: 16994 RVA: 0x0015764C File Offset: 0x0015584C
	private void Setup()
	{
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.GetComponent<AudioSource>();
		}
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.AddComponent<AudioSource>();
			this.audioSrc.playOnAwake = false;
		}
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x001576AC File Offset: 0x001558AC
	public void Play(AudioClip clip)
	{
		this.audioSrc.PlayOneShot(clip, AudioListener.volume);
	}

	// Token: 0x040034BF RID: 13503
	private static tk2dUIAudioManager instance;

	// Token: 0x040034C0 RID: 13504
	private AudioSource audioSrc;
}
