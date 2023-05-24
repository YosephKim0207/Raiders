using System;
using DaikonForge.Tween;
using UnityEngine;

// Token: 0x0200050F RID: 1295
public static class TweenSpriteExtensions
{
	// Token: 0x06001EF7 RID: 7927 RVA: 0x0008C2B4 File Offset: 0x0008A4B4
	public static Tween<float> TweenAlpha(this SpriteRenderer sprite)
	{
		return Tween<float>.Obtain().SetStartValue(sprite.color.a).SetEndValue(sprite.color.a)
			.SetDuration(1f)
			.OnExecute(delegate(float currentValue)
			{
				Color color = sprite.color;
				color.a = currentValue;
				sprite.color = color;
			});
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x0008C320 File Offset: 0x0008A520
	public static Tween<Color> TweenColor(this SpriteRenderer sprite)
	{
		return Tween<Color>.Obtain().SetStartValue(sprite.color).SetEndValue(sprite.color)
			.SetDuration(1f)
			.OnExecute(delegate(Color currentValue)
			{
				sprite.color = currentValue;
			});
	}
}
