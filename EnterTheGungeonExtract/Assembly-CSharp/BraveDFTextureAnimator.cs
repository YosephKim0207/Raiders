using System;
using UnityEngine;

// Token: 0x0200174B RID: 5963
public class BraveDFTextureAnimator : MonoBehaviour
{
	// Token: 0x06008AC6 RID: 35526 RVA: 0x0039D658 File Offset: 0x0039B858
	private void Start()
	{
		this.m_sprite = base.GetComponent<dfTextureSprite>();
		if (this.IsOneShot && this.OneShotDelayTime > 0f)
		{
			this.m_sprite.IsVisible = false;
		}
	}

	// Token: 0x06008AC7 RID: 35527 RVA: 0x0039D690 File Offset: 0x0039B890
	private void OnEnable()
	{
		this.m_currentFrame = 0;
		this.m_elapsed = 0f;
	}

	// Token: 0x06008AC8 RID: 35528 RVA: 0x0039D6A4 File Offset: 0x0039B8A4
	private void Update()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.IsOneShot && this.OneShotDelayTime > 0f)
		{
			this.OneShotDelayTime -= GameManager.INVARIANT_DELTA_TIME;
			return;
		}
		if (this.IsOneShot)
		{
			this.m_sprite.IsVisible = true;
		}
		if (this.timeless)
		{
			this.m_elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		else
		{
			this.m_elapsed += BraveTime.DeltaTime;
		}
		int currentFrame = this.m_currentFrame;
		while (this.m_elapsed > 1f / this.fps)
		{
			this.m_elapsed -= 1f / this.fps;
			if (this.randomLoop)
			{
				this.m_currentFrame += UnityEngine.Random.Range(0, this.textures.Length);
			}
			else
			{
				this.m_currentFrame++;
			}
		}
		if (currentFrame != this.m_currentFrame)
		{
			if (this.IsOneShot && this.m_currentFrame >= this.textures.Length)
			{
				base.enabled = false;
			}
			else
			{
				if (this.m_currentFrame >= this.textures.Length)
				{
					if (this.arbitraryLoopTarget > 0)
					{
						this.m_currentFrame %= this.textures.Length;
						this.m_currentFrame = Mathf.Max(this.arbitraryLoopTarget, this.m_currentFrame);
					}
					else
					{
						this.m_currentFrame %= this.textures.Length;
					}
				}
				this.m_sprite.Texture = this.textures[this.m_currentFrame];
			}
		}
	}

	// Token: 0x04009186 RID: 37254
	public Texture2D[] textures;

	// Token: 0x04009187 RID: 37255
	public float fps = 1f;

	// Token: 0x04009188 RID: 37256
	public bool IsOneShot;

	// Token: 0x04009189 RID: 37257
	public float OneShotDelayTime;

	// Token: 0x0400918A RID: 37258
	public bool randomLoop;

	// Token: 0x0400918B RID: 37259
	public bool timeless;

	// Token: 0x0400918C RID: 37260
	public int arbitraryLoopTarget = -1;

	// Token: 0x0400918D RID: 37261
	private dfTextureSprite m_sprite;

	// Token: 0x0400918E RID: 37262
	private float m_elapsed;

	// Token: 0x0400918F RID: 37263
	private int m_currentFrame;
}
