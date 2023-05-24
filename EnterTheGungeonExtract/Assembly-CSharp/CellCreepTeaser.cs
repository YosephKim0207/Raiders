using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200111E RID: 4382
public class CellCreepTeaser : MonoBehaviour
{
	// Token: 0x060060B3 RID: 24755 RVA: 0x002536C0 File Offset: 0x002518C0
	public void Update()
	{
		if (!this.isPlaying)
		{
			if (!GameManager.Instance.IsPaused && !Dungeon.IsGenerating && !GameManager.Instance.IsLoadingLevel)
			{
				this.bodySprite.Play();
				this.isPlaying = true;
			}
		}
		else
		{
			float num = Mathf.InverseLerp(3.75f, 3.17f, this.bodySprite.ClipTimeSeconds);
			this.shadowSprite.color = this.shadowSprite.color.WithAlpha(num);
			if (!this.bodySprite.Playing)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04005B58 RID: 23384
	public tk2dSpriteAnimator bodySprite;

	// Token: 0x04005B59 RID: 23385
	public tk2dSprite shadowSprite;

	// Token: 0x04005B5A RID: 23386
	private bool isPlaying;
}
