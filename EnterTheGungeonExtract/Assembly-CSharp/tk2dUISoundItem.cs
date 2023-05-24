using System;
using UnityEngine;

// Token: 0x02000C0E RID: 3086
[AddComponentMenu("2D Toolkit/UI/tk2dUISoundItem")]
public class tk2dUISoundItem : tk2dUIBaseItemControl
{
	// Token: 0x060041FB RID: 16891 RVA: 0x00155B5C File Offset: 0x00153D5C
	private void OnEnable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown += this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp += this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick += this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease += this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x00155C1C File Offset: 0x00153E1C
	private void OnDisable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown -= this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp -= this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick -= this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease -= this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x00155CDC File Offset: 0x00153EDC
	private void PlayDownSound()
	{
		this.PlaySound(this.downButtonSound);
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x00155CEC File Offset: 0x00153EEC
	private void PlayUpSound()
	{
		this.PlaySound(this.upButtonSound);
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x00155CFC File Offset: 0x00153EFC
	private void PlayClickSound()
	{
		this.PlaySound(this.clickButtonSound);
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x00155D0C File Offset: 0x00153F0C
	private void PlayReleaseSound()
	{
		this.PlaySound(this.releaseButtonSound);
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x00155D1C File Offset: 0x00153F1C
	private void PlaySound(AudioClip source)
	{
		tk2dUIAudioManager.Instance.Play(source);
	}

	// Token: 0x04003480 RID: 13440
	public AudioClip downButtonSound;

	// Token: 0x04003481 RID: 13441
	public AudioClip upButtonSound;

	// Token: 0x04003482 RID: 13442
	public AudioClip clickButtonSound;

	// Token: 0x04003483 RID: 13443
	public AudioClip releaseButtonSound;
}
