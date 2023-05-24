using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006A7 RID: 1703
	public struct InputRange
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000A8364 File Offset: 0x000A6564
		private InputRange(float value0, float value1, InputRangeType type)
		{
			this.Value0 = value0;
			this.Value1 = value1;
			this.Type = type;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000A837C File Offset: 0x000A657C
		public InputRange(InputRangeType type)
		{
			this.Value0 = InputRange.TypeToRange[(int)type].Value0;
			this.Value1 = InputRange.TypeToRange[(int)type].Value1;
			this.Type = type;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000A83B4 File Offset: 0x000A65B4
		public bool Includes(float value)
		{
			return !this.Excludes(value);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000A83C0 File Offset: 0x000A65C0
		public bool Excludes(float value)
		{
			return this.Type == InputRangeType.None || value < Mathf.Min(this.Value0, this.Value1) || value > Mathf.Max(this.Value0, this.Value1);
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000A8400 File Offset: 0x000A6600
		public static float Remap(float value, InputRange sourceRange, InputRange targetRange)
		{
			if (sourceRange.Excludes(value))
			{
				return 0f;
			}
			float num = Mathf.InverseLerp(sourceRange.Value0, sourceRange.Value1, value);
			return Mathf.Lerp(targetRange.Value0, targetRange.Value1, num);
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000A844C File Offset: 0x000A664C
		internal static float Remap(float value, InputRangeType sourceRangeType, InputRangeType targetRangeType)
		{
			InputRange inputRange = InputRange.TypeToRange[(int)sourceRangeType];
			InputRange inputRange2 = InputRange.TypeToRange[(int)targetRangeType];
			return InputRange.Remap(value, inputRange, inputRange2);
		}

		// Token: 0x04001B7B RID: 7035
		public static readonly InputRange None = new InputRange(0f, 0f, InputRangeType.None);

		// Token: 0x04001B7C RID: 7036
		public static readonly InputRange MinusOneToOne = new InputRange(-1f, 1f, InputRangeType.MinusOneToOne);

		// Token: 0x04001B7D RID: 7037
		public static readonly InputRange OneToMinusOne = new InputRange(1f, -1f, InputRangeType.OneToMinusOne);

		// Token: 0x04001B7E RID: 7038
		public static readonly InputRange ZeroToOne = new InputRange(0f, 1f, InputRangeType.ZeroToOne);

		// Token: 0x04001B7F RID: 7039
		public static readonly InputRange ZeroToMinusOne = new InputRange(0f, -1f, InputRangeType.ZeroToMinusOne);

		// Token: 0x04001B80 RID: 7040
		public static readonly InputRange OneToZero = new InputRange(1f, 0f, InputRangeType.OneToZero);

		// Token: 0x04001B81 RID: 7041
		public static readonly InputRange MinusOneToZero = new InputRange(-1f, 0f, InputRangeType.MinusOneToZero);

		// Token: 0x04001B82 RID: 7042
		public static readonly InputRange ZeroToNegativeInfinity = new InputRange(0f, float.NegativeInfinity, InputRangeType.ZeroToNegativeInfinity);

		// Token: 0x04001B83 RID: 7043
		public static readonly InputRange ZeroToPositiveInfinity = new InputRange(0f, float.PositiveInfinity, InputRangeType.ZeroToPositiveInfinity);

		// Token: 0x04001B84 RID: 7044
		public static readonly InputRange Everything = new InputRange(float.NegativeInfinity, float.PositiveInfinity, InputRangeType.Everything);

		// Token: 0x04001B85 RID: 7045
		private static readonly InputRange[] TypeToRange = new InputRange[]
		{
			InputRange.None,
			InputRange.MinusOneToOne,
			InputRange.OneToMinusOne,
			InputRange.ZeroToOne,
			InputRange.ZeroToMinusOne,
			InputRange.OneToZero,
			InputRange.MinusOneToZero,
			InputRange.ZeroToNegativeInfinity,
			InputRange.ZeroToPositiveInfinity,
			InputRange.Everything
		};

		// Token: 0x04001B86 RID: 7046
		public readonly float Value0;

		// Token: 0x04001B87 RID: 7047
		public readonly float Value1;

		// Token: 0x04001B88 RID: 7048
		public readonly InputRangeType Type;
	}
}
