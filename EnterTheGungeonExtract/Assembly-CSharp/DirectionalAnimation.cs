using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

// Token: 0x02001087 RID: 4231
[Serializable]
public class DirectionalAnimation
{
	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x06005D1D RID: 23837 RVA: 0x0023AD0C File Offset: 0x00238F0C
	public bool HasAnimation
	{
		get
		{
			return this.Type != DirectionalAnimation.DirectionType.None;
		}
	}

	// Token: 0x06005D1E RID: 23838 RVA: 0x0023AD1C File Offset: 0x00238F1C
	public DirectionalAnimation.Info GetInfo(Vector2 dir, bool frameUpdate = false)
	{
		return this.GetInfo(dir.ToAngle(), frameUpdate);
	}

	// Token: 0x06005D1F RID: 23839 RVA: 0x0023AD2C File Offset: 0x00238F2C
	public DirectionalAnimation.Info GetInfo(float angleDegrees, bool frameUpdate = false)
	{
		if (float.IsNaN(angleDegrees))
		{
			Debug.LogWarning("Warning: NaN Animation Angle!");
			angleDegrees = 0f;
		}
		if (this.Type == DirectionalAnimation.DirectionType.SixteenWayTemp)
		{
			return this.GetInfoSixteenWayTemp(angleDegrees, frameUpdate);
		}
		angleDegrees = BraveMathCollege.ClampAngle360(angleDegrees);
		DirectionalAnimation.SingleAnimation[] array = DirectionalAnimation.m_combined[(int)this.Type];
		if (this.m_lastAnimIndex != -1 && (this.m_lastAnimIndex < 0 || this.m_lastAnimIndex >= array.Length))
		{
			this.m_lastAnimIndex = -1;
		}
		if (this.m_lastAnimIndex != -1)
		{
			float minAngle = array[this.m_lastAnimIndex].minAngle;
			if (minAngle < 0f && angleDegrees >= minAngle + 360f - 2.5f)
			{
				return this.GetInfo(this.m_lastAnimIndex);
			}
			float maxAngle = array[this.m_lastAnimIndex].maxAngle;
			if (angleDegrees >= minAngle - 2.5f && angleDegrees <= maxAngle + 2.5f)
			{
				return this.GetInfo(this.m_lastAnimIndex);
			}
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (this.Type == DirectionalAnimation.DirectionType.Single || this.Flipped[i] != DirectionalAnimation.FlipType.Unused)
			{
				float minAngle2 = array[i].minAngle;
				if (minAngle2 < 0f && angleDegrees >= minAngle2 + 360f)
				{
					if (frameUpdate)
					{
						this.m_lastAnimIndex = i;
					}
					return this.GetInfo(i);
				}
				float maxAngle2 = array[i].maxAngle;
				if (angleDegrees >= minAngle2 && angleDegrees <= maxAngle2)
				{
					if (frameUpdate)
					{
						this.m_lastAnimIndex = i;
					}
					return this.GetInfo(i);
				}
			}
		}
		int num = -1;
		float num2 = float.MaxValue;
		for (int j = 0; j < array.Length; j++)
		{
			if (this.Type == DirectionalAnimation.DirectionType.Single || this.Flipped[j] != DirectionalAnimation.FlipType.Unused)
			{
				float num3 = Mathf.Min(BraveMathCollege.AbsAngleBetween(angleDegrees, array[j].minAngle), BraveMathCollege.AbsAngleBetween(angleDegrees, array[j].maxAngle));
				if (num3 < num2)
				{
					num = j;
					num2 = num3;
				}
			}
		}
		if (num >= 0)
		{
			if (frameUpdate)
			{
				this.m_lastAnimIndex = num;
			}
			return this.GetInfo(num);
		}
		return null;
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x0023AF5C File Offset: 0x0023915C
	private DirectionalAnimation.Info GetInfoSixteenWayTemp(float angleDegrees, bool frameUpdate)
	{
		angleDegrees = BraveMathCollege.ClampAngle360(angleDegrees);
		if (frameUpdate)
		{
			this.m_tempCooldown -= BraveTime.DeltaTime;
		}
		int num;
		if (this.m_tempCooldown > 0f)
		{
			num = this.m_tempIndex;
		}
		else
		{
			float num2 = BraveMathCollege.ClampAngle360(-angleDegrees + 90f + 22.5f);
			int num3 = (int)(num2 / 45f);
			num = num3 * 2;
			if (num3 != -1 && this.m_previousEighthIndex != -1)
			{
				if ((num3 == 0 && this.m_previousEighthIndex == 7) || (num3 == 7 && this.m_previousEighthIndex == 0))
				{
					num = 15;
					this.m_tempIndex = num;
					this.m_tempCooldown = 0.1f;
				}
				else if (num3 == this.m_previousEighthIndex + 1)
				{
					num = this.m_previousEighthIndex * 2 + 1;
					this.m_tempIndex = num;
					this.m_tempCooldown = 0.1f;
				}
				else if (num3 == this.m_previousEighthIndex - 1)
				{
					num = this.m_previousEighthIndex * 2 - 1;
					this.m_tempIndex = num;
					this.m_tempCooldown = 0.1f;
				}
			}
			this.m_previousEighthIndex = num3;
		}
		if (this.Flipped[num] != DirectionalAnimation.FlipType.Unused)
		{
			return this.GetInfo(num);
		}
		int num4 = num + (((float)(num % 1) <= 0.5f) ? (-1) : 1);
		num4 = (num4 + this.AnimNames.Count<string>()) % this.AnimNames.Count<string>();
		return this.GetInfo(num4);
	}

	// Token: 0x06005D21 RID: 23841 RVA: 0x0023B0C8 File Offset: 0x002392C8
	public DirectionalAnimation.Info GetInfo(int index)
	{
		if (this.Type == DirectionalAnimation.DirectionType.Single && index == 0)
		{
			this.m_info.SetAll(this.Prefix, false, -90f);
			return this.m_info;
		}
		if (index > this.Flipped.Length - 1)
		{
			Debug.LogError("shit");
		}
		if (this.Flipped[index] == DirectionalAnimation.FlipType.Mirror)
		{
			index = DirectionalAnimation.GetMirrorIndex(this.Type, index);
			this.m_info.SetAll(this.GetName(index), true, this.GetArtAngle(index));
			return this.m_info;
		}
		this.m_info.SetAll(this.GetName(index), this.Flipped[index] == DirectionalAnimation.FlipType.Flip, this.GetArtAngle(index));
		return this.m_info;
	}

	// Token: 0x06005D22 RID: 23842 RVA: 0x0023B188 File Offset: 0x00239388
	public int GetNumAnimations()
	{
		return DirectionalAnimation.GetNumAnimations(this.Type);
	}

	// Token: 0x06005D23 RID: 23843 RVA: 0x0023B198 File Offset: 0x00239398
	private string GetName(int index)
	{
		if (string.IsNullOrEmpty(this.AnimNames[index]))
		{
			this.AnimNames[index] = DirectionalAnimation.GetDefaultName(this.Prefix, this.Type, index);
		}
		return this.AnimNames[index];
	}

	// Token: 0x06005D24 RID: 23844 RVA: 0x0023B1D0 File Offset: 0x002393D0
	private float GetArtAngle(int index)
	{
		return DirectionalAnimation.m_combined[(int)this.Type][index].artAngle;
	}

	// Token: 0x06005D25 RID: 23845 RVA: 0x0023B1E8 File Offset: 0x002393E8
	public static int GetNumAnimations(DirectionalAnimation.DirectionType type)
	{
		return DirectionalAnimation.m_combined[(int)type].Length;
	}

	// Token: 0x06005D26 RID: 23846 RVA: 0x0023B1F4 File Offset: 0x002393F4
	public static string GetLabel(DirectionalAnimation.DirectionType type, int index)
	{
		string text = DirectionalAnimation.m_combined[(int)type][index].suffix.Replace('_', ' ');
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
	}

	// Token: 0x06005D27 RID: 23847 RVA: 0x0023B22C File Offset: 0x0023942C
	public static string GetSuffix(DirectionalAnimation.DirectionType type, int index)
	{
		return DirectionalAnimation.m_combined[(int)type][index].suffix;
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x0023B23C File Offset: 0x0023943C
	public static string GetDefaultName(string prefix, DirectionalAnimation.DirectionType type, int index)
	{
		if (type == DirectionalAnimation.DirectionType.Single)
		{
			return prefix;
		}
		if (prefix.Contains("{0}"))
		{
			return string.Format(prefix, DirectionalAnimation.GetSuffix(type, index));
		}
		return prefix + "_" + DirectionalAnimation.GetSuffix(type, index);
	}

	// Token: 0x06005D29 RID: 23849 RVA: 0x0023B278 File Offset: 0x00239478
	public static bool HasMirror(DirectionalAnimation.DirectionType type, int index)
	{
		return DirectionalAnimation.m_combined[(int)type][index].mirrorIndex != null;
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x0023B290 File Offset: 0x00239490
	public static int GetMirrorIndex(DirectionalAnimation.DirectionType type, int index)
	{
		return DirectionalAnimation.m_combined[(int)type][index].mirrorIndex.Value;
	}

	// Token: 0x040056F0 RID: 22256
	public const float s_BACKFACING_ANGLE_MAX = 155f;

	// Token: 0x040056F1 RID: 22257
	public const float s_BACKFACING_ANGLE_MIN = 25f;

	// Token: 0x040056F2 RID: 22258
	public const float s_BACKWARDS_ANGLE_MAX = 120f;

	// Token: 0x040056F3 RID: 22259
	public const float s_BACKWARDS_ANGLE_MIN = 60f;

	// Token: 0x040056F4 RID: 22260
	public const float s_FORWARDS_ANGLE_MAX = -60f;

	// Token: 0x040056F5 RID: 22261
	public const float s_FORWARDS_ANGLE_MIN = -120f;

	// Token: 0x040056F6 RID: 22262
	public const float c_AngleBuffer = 2.5f;

	// Token: 0x040056F7 RID: 22263
	public static DirectionalAnimation.SingleAnimation[][] m_combined = new DirectionalAnimation.SingleAnimation[][]
	{
		new DirectionalAnimation.SingleAnimation[0],
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation(string.Empty, 0f, 360f, -90f, null)
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("right", -90f, 90f, 0f, new int?(1)),
			new DirectionalAnimation.SingleAnimation("left", 90f, 270f, 180f, new int?(0))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("back", 0f, 180f, 90f, null),
			new DirectionalAnimation.SingleAnimation("front", 180f, 360f, -90f, null)
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("back_right", 25f, 90f, 45f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("front_right", -90f, 25f, -45f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("front_left", 155f, 270f, -135f, new int?(1)),
			new DirectionalAnimation.SingleAnimation("back_left", 90f, 155f, 135f, new int?(0))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("back", 60f, 120f, 90f, null),
			new DirectionalAnimation.SingleAnimation("back_right", 25f, 60f, 45f, new int?(5)),
			new DirectionalAnimation.SingleAnimation("front_right", -60f, 25f, -45f, new int?(4)),
			new DirectionalAnimation.SingleAnimation("front", 240f, 300f, -90f, null),
			new DirectionalAnimation.SingleAnimation("front_left", 155f, 240f, -135f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("back_left", 120f, 155f, 135f, new int?(1))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("back", 67.5f, 112.5f, 90f, null),
			new DirectionalAnimation.SingleAnimation("back_right", 22.5f, 67.5f, 45f, new int?(7)),
			new DirectionalAnimation.SingleAnimation("right", -22.5f, 22.5f, 0f, new int?(6)),
			new DirectionalAnimation.SingleAnimation("front_right", 292.5f, 337.5f, -45f, new int?(5)),
			new DirectionalAnimation.SingleAnimation("front", 247.5f, 292.5f, -90f, null),
			new DirectionalAnimation.SingleAnimation("front_left", 202.5f, 247.5f, -135f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("left", 157.5f, 202.5f, 180f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("back_left", 112.5f, 157.5f, 135f, new int?(1))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("north", 78.75f, 101.25f, 90f, null),
			new DirectionalAnimation.SingleAnimation("north_northeast", 56.25f, 78.75f, 67.5f, new int?(15)),
			new DirectionalAnimation.SingleAnimation("northeast", 33.75f, 56.25f, 45f, new int?(14)),
			new DirectionalAnimation.SingleAnimation("east_northeast", 11.25f, 33.75f, 22.5f, new int?(13)),
			new DirectionalAnimation.SingleAnimation("east", -11.25f, 11.25f, 0f, new int?(12)),
			new DirectionalAnimation.SingleAnimation("east_southeast", 326.25f, 348.75f, -22.5f, new int?(11)),
			new DirectionalAnimation.SingleAnimation("southeast", 303.75f, 326.25f, -45f, new int?(10)),
			new DirectionalAnimation.SingleAnimation("south_southeast", 281.25f, 303.75f, -67.5f, new int?(9)),
			new DirectionalAnimation.SingleAnimation("south", 258.75f, 281.25f, -90f, null),
			new DirectionalAnimation.SingleAnimation("south_southwest", 236.25f, 258.75f, -112.5f, new int?(7)),
			new DirectionalAnimation.SingleAnimation("southwest", 213.75f, 236.25f, -135f, new int?(6)),
			new DirectionalAnimation.SingleAnimation("west_southwest", 191.25f, 213.75f, -157.5f, new int?(5)),
			new DirectionalAnimation.SingleAnimation("west", 168.75f, 191.25f, 180f, new int?(4)),
			new DirectionalAnimation.SingleAnimation("west_northwest", 146.25f, 168.75f, 157.5f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("northwest", 123.75f, 146.25f, 135f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("north_northwest", 101.25f, 123.75f, 112.5f, new int?(1))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("north", 78.75f, 101.25f, 90f, null),
			new DirectionalAnimation.SingleAnimation("north_northeast", 56.25f, 78.75f, 67.5f, new int?(15)),
			new DirectionalAnimation.SingleAnimation("northeast", 33.75f, 56.25f, 45f, new int?(14)),
			new DirectionalAnimation.SingleAnimation("east_northeast", 11.25f, 33.75f, 22.5f, new int?(13)),
			new DirectionalAnimation.SingleAnimation("east", 348.75f, 11.25f, 0f, new int?(12)),
			new DirectionalAnimation.SingleAnimation("east_southeast", 326.25f, 348.75f, -22.5f, new int?(11)),
			new DirectionalAnimation.SingleAnimation("southeast", 303.75f, 326.25f, -45f, new int?(10)),
			new DirectionalAnimation.SingleAnimation("south_southeast", 281.25f, 303.75f, -67.5f, new int?(9)),
			new DirectionalAnimation.SingleAnimation("south", 258.75f, 281.25f, -90f, null),
			new DirectionalAnimation.SingleAnimation("south_southwest", 236.25f, 258.75f, -112.5f, new int?(7)),
			new DirectionalAnimation.SingleAnimation("southwest", 213.75f, 236.25f, -135f, new int?(6)),
			new DirectionalAnimation.SingleAnimation("west_southwest", 191.25f, 213.75f, -157.5f, new int?(5)),
			new DirectionalAnimation.SingleAnimation("west", 168.75f, 191.25f, 180f, new int?(4)),
			new DirectionalAnimation.SingleAnimation("west_northwest", 146.25f, 168.75f, 157.5f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("northwest", 123.75f, 146.25f, 135f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("north_northwest", 101.25f, 123.75f, 112.5f, new int?(1))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("north", 45f, 135f, 90f, null),
			new DirectionalAnimation.SingleAnimation("east", -45f, 45f, 0f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("south", 225f, 315f, -90f, null),
			new DirectionalAnimation.SingleAnimation("west", 135f, 225f, 180f, new int?(1))
		},
		new DirectionalAnimation.SingleAnimation[]
		{
			new DirectionalAnimation.SingleAnimation("north", 67.5f, 112.5f, 90f, null),
			new DirectionalAnimation.SingleAnimation("northeast", 22.5f, 67.5f, 45f, new int?(7)),
			new DirectionalAnimation.SingleAnimation("east", -22.5f, 22.5f, 0f, new int?(6)),
			new DirectionalAnimation.SingleAnimation("southeast", 292.5f, 337.5f, -45f, new int?(5)),
			new DirectionalAnimation.SingleAnimation("south", 247.5f, 292.5f, -90f, null),
			new DirectionalAnimation.SingleAnimation("southwest", 202.5f, 247.5f, -135f, new int?(3)),
			new DirectionalAnimation.SingleAnimation("west", 157.5f, 202.5f, 180f, new int?(2)),
			new DirectionalAnimation.SingleAnimation("northwest", 112.5f, 157.5f, 135f, new int?(1))
		}
	};

	// Token: 0x040056F8 RID: 22264
	public DirectionalAnimation.DirectionType Type;

	// Token: 0x040056F9 RID: 22265
	public string Prefix;

	// Token: 0x040056FA RID: 22266
	public string[] AnimNames;

	// Token: 0x040056FB RID: 22267
	public DirectionalAnimation.FlipType[] Flipped;

	// Token: 0x040056FC RID: 22268
	private DirectionalAnimation.Info m_info = new DirectionalAnimation.Info();

	// Token: 0x040056FD RID: 22269
	[NonSerialized]
	private int m_lastAnimIndex = -1;

	// Token: 0x040056FE RID: 22270
	[NonSerialized]
	private int m_previousEighthIndex = -1;

	// Token: 0x040056FF RID: 22271
	[NonSerialized]
	private float m_tempCooldown;

	// Token: 0x04005700 RID: 22272
	[NonSerialized]
	private int m_tempIndex;

	// Token: 0x02001088 RID: 4232
	public enum DirectionType
	{
		// Token: 0x04005702 RID: 22274
		None,
		// Token: 0x04005703 RID: 22275
		Single,
		// Token: 0x04005704 RID: 22276
		TwoWayHorizontal,
		// Token: 0x04005705 RID: 22277
		TwoWayVertical,
		// Token: 0x04005706 RID: 22278
		FourWay,
		// Token: 0x04005707 RID: 22279
		SixWay,
		// Token: 0x04005708 RID: 22280
		EightWay,
		// Token: 0x04005709 RID: 22281
		SixteenWay,
		// Token: 0x0400570A RID: 22282
		SixteenWayTemp,
		// Token: 0x0400570B RID: 22283
		FourWayCardinal,
		// Token: 0x0400570C RID: 22284
		EightWayOrdinal
	}

	// Token: 0x02001089 RID: 4233
	public class SingleAnimation
	{
		// Token: 0x06005D2C RID: 23852 RVA: 0x0023BC58 File Offset: 0x00239E58
		public SingleAnimation(string suffix, float minAngle, float maxAngle, float artAngle, int? mirrorIndex = null)
		{
			this.suffix = suffix;
			this.minAngle = minAngle;
			this.maxAngle = maxAngle;
			this.artAngle = artAngle;
			this.mirrorIndex = mirrorIndex;
		}

		// Token: 0x0400570D RID: 22285
		public string suffix;

		// Token: 0x0400570E RID: 22286
		public float minAngle;

		// Token: 0x0400570F RID: 22287
		public float maxAngle;

		// Token: 0x04005710 RID: 22288
		public float artAngle;

		// Token: 0x04005711 RID: 22289
		public int? mirrorIndex;
	}

	// Token: 0x0200108A RID: 4234
	public enum FlipType
	{
		// Token: 0x04005713 RID: 22291
		None,
		// Token: 0x04005714 RID: 22292
		Flip,
		// Token: 0x04005715 RID: 22293
		Unused,
		// Token: 0x04005716 RID: 22294
		Mirror
	}

	// Token: 0x0200108B RID: 4235
	public class Info
	{
		// Token: 0x06005D2E RID: 23854 RVA: 0x0023BC90 File Offset: 0x00239E90
		public void SetAll(string name, bool flipped, float artAngle)
		{
			this.name = name;
			this.flipped = flipped;
			this.artAngle = artAngle;
		}

		// Token: 0x04005717 RID: 22295
		public string name;

		// Token: 0x04005718 RID: 22296
		public bool flipped;

		// Token: 0x04005719 RID: 22297
		public float artAngle;
	}
}
