using System;
using UnityEngine;

// Token: 0x020011B8 RID: 4536
public class MirrorDweller : BraveBehaviour
{
	// Token: 0x06006536 RID: 25910 RVA: 0x00275EF4 File Offset: 0x002740F4
	private void Start()
	{
		base.sprite.usesOverrideMaterial = true;
		base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
		base.sprite.renderer.material.SetColor("_TintColor", new Color(0.4f, 0.4f, 0.8f, 0.5f));
		if (this.UsesOverrideTintColor)
		{
			base.sprite.renderer.material.SetColor("_TintColor", this.OverrideTintColor);
		}
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x00275F8C File Offset: 0x0027418C
	private void LateUpdate()
	{
		if (this.TargetSprite != null && this.MirrorSprite)
		{
			if (Mathf.Abs(base.transform.position.x - this.TargetSprite.transform.position.x) < 5f)
			{
				base.sprite.renderer.enabled = true;
				base.sprite.SetSprite(this.TargetSprite.Collection, this.TargetSprite.spriteId);
				float num = this.MirrorSprite.transform.position.y - this.TargetSprite.transform.position.y;
				num /= 2f;
				num += 0.5f;
				base.transform.position = base.transform.position.WithX(this.TargetSprite.transform.position.x).WithY(this.MirrorSprite.transform.position.y + num).Quantize(0.0625f);
			}
			else
			{
				base.sprite.renderer.enabled = false;
			}
		}
		else if (this.TargetPlayer != null && this.MirrorSprite)
		{
			if (Mathf.Abs(base.transform.position.x - this.TargetPlayer.sprite.transform.position.x) < 5f)
			{
				base.sprite.renderer.enabled = true;
				base.sprite.SetSprite(this.TargetPlayer.sprite.Collection, this.TargetPlayer.GetMirrorSpriteID());
				float num2 = this.MirrorSprite.transform.position.y - this.TargetPlayer.transform.position.y;
				num2 /= 2f;
				num2 += 0.5f;
				base.transform.position = base.transform.position.WithX(this.TargetPlayer.transform.position.x).WithY(this.MirrorSprite.transform.position.y + num2).Quantize(0.0625f);
				base.sprite.HeightOffGround = num2 - 0.5f;
				base.sprite.FlipX = this.TargetPlayer.sprite.FlipX;
				if (base.sprite.FlipX)
				{
					base.transform.position += new Vector3(this.TargetPlayer.sprite.GetBounds().size.x, 0f, 0f);
				}
			}
			else
			{
				base.sprite.renderer.enabled = false;
			}
		}
	}

	// Token: 0x040060F6 RID: 24822
	public tk2dBaseSprite TargetSprite;

	// Token: 0x040060F7 RID: 24823
	public PlayerController TargetPlayer;

	// Token: 0x040060F8 RID: 24824
	public tk2dBaseSprite MirrorSprite;

	// Token: 0x040060F9 RID: 24825
	public bool UsesOverrideTintColor;

	// Token: 0x040060FA RID: 24826
	public Color OverrideTintColor;
}
