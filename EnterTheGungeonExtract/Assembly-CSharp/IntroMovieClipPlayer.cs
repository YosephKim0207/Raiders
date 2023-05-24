using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class IntroMovieClipPlayer : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	private void Start()
	{
		GameManager.AttemptSoundEngineInitialization();
		this.m_source = Camera.main.gameObject.AddComponent<AudioSource>();
		this.m_source.clip = this.movieAudio;
		this.m_source.pitch = 1.02f;
		this.movieTexture.loop = false;
		this.guiTexture.Texture = this.movieTexture;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
	public void TriggerMovie()
	{
		base.StartCoroutine(this.Do());
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020D0 File Offset: 0x000002D0
	private IEnumerator Do()
	{
		AkSoundEngine.PostEvent("Play_UI_titleintro", base.gameObject);
		yield return new WaitForSeconds(0.1f);
		this.movieTexture.Play();
		yield return null;
		yield break;
	}

	// Token: 0x04000001 RID: 1
	public MovieTexture movieTexture;

	// Token: 0x04000002 RID: 2
	public AudioClip movieAudio;

	// Token: 0x04000003 RID: 3
	public dfTextureSprite guiTexture;

	// Token: 0x04000004 RID: 4
	private AudioSource m_source;
}
