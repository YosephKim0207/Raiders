using System;
using FullInspector;
using UnityEngine;

// Token: 0x020016BC RID: 5820
[Serializable]
public class ScreenShakeSettings : fiInspectorOnly
{
	// Token: 0x06008760 RID: 34656 RVA: 0x003826E0 File Offset: 0x003808E0
	public ScreenShakeSettings()
	{
		this.magnitude = 0.35f;
		this.speed = 6f;
		this.time = 0.06f;
		this.falloff = 0f;
	}

	// Token: 0x06008761 RID: 34657 RVA: 0x00382738 File Offset: 0x00380938
	public ScreenShakeSettings(float mag, float spd, float tim, float foff)
	{
		this.magnitude = mag;
		this.speed = spd;
		this.time = tim;
		this.falloff = foff;
		this.direction = Vector2.zero;
	}

	// Token: 0x06008762 RID: 34658 RVA: 0x0038278C File Offset: 0x0038098C
	public ScreenShakeSettings(float mag, float spd, float tim, float foff, Vector2 dir)
	{
		this.magnitude = mag;
		this.speed = spd;
		this.time = tim;
		this.falloff = foff;
		this.direction = dir;
	}

	// Token: 0x06008763 RID: 34659 RVA: 0x003827DC File Offset: 0x003809DC
	public bool ShowSimpleVibrationParams()
	{
		return this.vibrationType == ScreenShakeSettings.VibrationType.Simple;
	}

	// Token: 0x04008C84 RID: 35972
	public static float GLOBAL_SHAKE_MULTIPLIER = 1f;

	// Token: 0x04008C85 RID: 35973
	public float magnitude;

	// Token: 0x04008C86 RID: 35974
	public float speed;

	// Token: 0x04008C87 RID: 35975
	public float time;

	// Token: 0x04008C88 RID: 35976
	public float falloff;

	// Token: 0x04008C89 RID: 35977
	public Vector2 direction;

	// Token: 0x04008C8A RID: 35978
	public ScreenShakeSettings.VibrationType vibrationType = ScreenShakeSettings.VibrationType.Auto;

	// Token: 0x04008C8B RID: 35979
	[InspectorShowIf("ShowSimpleVibrationParams")]
	[InspectorIndent]
	public Vibration.Time simpleVibrationTime = Vibration.Time.Normal;

	// Token: 0x04008C8C RID: 35980
	[InspectorIndent]
	[InspectorShowIf("ShowSimpleVibrationParams")]
	public Vibration.Strength simpleVibrationStrength = Vibration.Strength.Medium;

	// Token: 0x020016BD RID: 5821
	public enum VibrationType
	{
		// Token: 0x04008C8E RID: 35982
		None,
		// Token: 0x04008C8F RID: 35983
		Auto = 10,
		// Token: 0x04008C90 RID: 35984
		Simple = 20
	}
}
