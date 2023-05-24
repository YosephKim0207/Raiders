using System;
using DaikonForge.Tween;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public static class TweenMaterialExtensions
{
	// Token: 0x06001EEB RID: 7915 RVA: 0x0008C084 File Offset: 0x0008A284
	public static Tween<Color> TweenColor(this Material material)
	{
		return Tween<Color>.Obtain().SetStartValue(material.color).SetEndValue(material.color)
			.SetDuration(1f)
			.OnExecute(delegate(Color currentValue)
			{
				material.color = currentValue;
			});
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x0008C0E0 File Offset: 0x0008A2E0
	public static Tween<float> TweenAlpha(this Material material)
	{
		return Tween<float>.Obtain().SetStartValue(material.color.a).SetEndValue(material.color.a)
			.SetDuration(1f)
			.OnExecute(delegate(float currentValue)
			{
				Color color = material.color;
				color.a = currentValue;
				material.color = color;
			});
	}
}
