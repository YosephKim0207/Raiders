using System;

// Token: 0x020016CD RID: 5837
public class SpriteAnimatorSync : BraveBehaviour
{
	// Token: 0x060087C8 RID: 34760 RVA: 0x00384B88 File Offset: 0x00382D88
	public void Start()
	{
		if (this.otherSprite.spriteAnimator)
		{
			this.otherSprite.spriteAnimator.alwaysUpdateOffscreen = true;
		}
		this.otherSprite.SpriteChanged += this.OtherSpriteChanged;
		base.sprite.SetSprite(this.otherSprite.Collection, this.otherSprite.spriteId);
	}

	// Token: 0x060087C9 RID: 34761 RVA: 0x00384BF4 File Offset: 0x00382DF4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060087CA RID: 34762 RVA: 0x00384BFC File Offset: 0x00382DFC
	private void OtherSpriteChanged(tk2dBaseSprite tk2DBaseSprite)
	{
		base.sprite.SetSprite(this.otherSprite.spriteId);
	}

	// Token: 0x04008CF5 RID: 36085
	public tk2dBaseSprite otherSprite;
}
