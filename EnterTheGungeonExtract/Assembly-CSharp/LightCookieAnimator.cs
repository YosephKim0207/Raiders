using System;
using UnityEngine;

// Token: 0x0200151C RID: 5404
public class LightCookieAnimator : MonoBehaviour
{
	// Token: 0x06007B52 RID: 31570 RVA: 0x003163C0 File Offset: 0x003145C0
	private void Start()
	{
		this.m_light = base.GetComponent<Light>();
		if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW)
		{
			this.m_light.enabled = false;
		}
		this.elapsed = -1f * this.initialDelay;
	}

	// Token: 0x06007B53 RID: 31571 RVA: 0x003163FC File Offset: 0x003145FC
	private void Update()
	{
		this.elapsed += BraveTime.DeltaTime;
		float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this.elapsed / this.duration));
		if (this.elapsed >= this.additionalScreenShakeDelay && this.doScreenShake && !this.m_hasTriggeredSS)
		{
			this.m_hasTriggeredSS = true;
			GameManager.Instance.MainCameraController.DoScreenShake(this.screenShake, null, false);
			AkSoundEngine.PostEvent("Play_OBJ_moondoor_close_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		if (this.doVFX)
		{
			for (int i = 0; i < this.vfxTimes.Length; i++)
			{
				if (this.vfxs[i] != null && !this.vfxs[i].activeSelf && this.elapsed > this.vfxTimes[i])
				{
					this.vfxs[i].SetActive(true);
					this.vfxs[i] = null;
				}
			}
		}
		if (num == 1f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			int num2 = Mathf.FloorToInt((float)this.frames.Length * num);
			this.m_light.cookie = this.frames[num2];
		}
	}

	// Token: 0x04007DD8 RID: 32216
	public Texture2D[] frames;

	// Token: 0x04007DD9 RID: 32217
	public float duration = 4f;

	// Token: 0x04007DDA RID: 32218
	public float initialDelay = 1f;

	// Token: 0x04007DDB RID: 32219
	public float additionalScreenShakeDelay = 0.1f;

	// Token: 0x04007DDC RID: 32220
	public bool doScreenShake;

	// Token: 0x04007DDD RID: 32221
	[ShowInInspectorIf("doScreenShake", false)]
	public ScreenShakeSettings screenShake;

	// Token: 0x04007DDE RID: 32222
	private float elapsed;

	// Token: 0x04007DDF RID: 32223
	private Light m_light;

	// Token: 0x04007DE0 RID: 32224
	private bool m_hasTriggeredSS;

	// Token: 0x04007DE1 RID: 32225
	public bool doVFX;

	// Token: 0x04007DE2 RID: 32226
	public GameObject[] vfxs;

	// Token: 0x04007DE3 RID: 32227
	public float[] vfxTimes;
}
