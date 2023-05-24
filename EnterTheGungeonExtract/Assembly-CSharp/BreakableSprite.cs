using System;
using UnityEngine;

// Token: 0x02000E14 RID: 3604
public class BreakableSprite : BraveBehaviour
{
	// Token: 0x06004C63 RID: 19555 RVA: 0x001A0774 File Offset: 0x0019E974
	public void Start()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged += this.OnHealthHaverDamaged;
		}
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x001A07A0 File Offset: 0x0019E9A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x001A07A8 File Offset: 0x0019E9A8
	private void OnHealthHaverDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		int i = this.breakFrames.Length - 1;
		while (i >= 0)
		{
			if (resultValue / maxValue <= this.breakFrames[i].healthPercentage / 100f)
			{
				string sprite = this.breakFrames[i].sprite;
				if (this.animations)
				{
					base.spriteAnimator.Play(sprite);
					return;
				}
				base.sprite.SetSprite(this.breakFrames[i].sprite);
				return;
			}
			else
			{
				i--;
			}
		}
	}

	// Token: 0x04004241 RID: 16961
	public bool animations = true;

	// Token: 0x04004242 RID: 16962
	public BreakFrame[] breakFrames;
}
