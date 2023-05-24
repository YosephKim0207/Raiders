using System;
using DaikonForge.Tween;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public static class TweenTextExtensions
{
	// Token: 0x06001EF1 RID: 7921 RVA: 0x0008C19C File Offset: 0x0008A39C
	public static Tween<float> TweenAlpha(this TextMesh text)
	{
		return Tween<float>.Obtain().SetStartValue(text.color.a).SetEndValue(text.color.a)
			.SetDuration(1f)
			.OnExecute(delegate(float currentValue)
			{
				Color color = text.color;
				color.a = currentValue;
				text.color = color;
			});
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x0008C208 File Offset: 0x0008A408
	public static Tween<Color> TweenColor(this TextMesh text)
	{
		return Tween<Color>.Obtain().SetStartValue(text.color).SetEndValue(text.color)
			.SetDuration(1f)
			.OnExecute(delegate(Color currentValue)
			{
				text.color = currentValue;
			});
	}
}
