using System;

namespace SimplexNoise
{
	// Token: 0x02000F66 RID: 3942
	public class Noise
	{
		// Token: 0x060054EB RID: 21739 RVA: 0x00202DEC File Offset: 0x00200FEC
		public static float Generate(float x)
		{
			int num = Noise.FastFloor(x);
			int num2 = num + 1;
			float num3 = x - (float)num;
			float num4 = num3 - 1f;
			float num5 = 1f - num3 * num3;
			num5 *= num5;
			float num6 = num5 * num5 * Noise.grad((int)Noise.perm[num & 255], num3);
			float num7 = 1f - num4 * num4;
			num7 *= num7;
			float num8 = num7 * num7 * Noise.grad((int)Noise.perm[num2 & 255], num4);
			return 0.395f * (num6 + num8);
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x00202E78 File Offset: 0x00201078
		public static float Generate(float x, float y)
		{
			float num = (x + y) * 0.3660254f;
			float num2 = x + num;
			float num3 = y + num;
			int num4 = Noise.FastFloor(num2);
			int num5 = Noise.FastFloor(num3);
			float num6 = (float)(num4 + num5) * 0.21132487f;
			float num7 = (float)num4 - num6;
			float num8 = (float)num5 - num6;
			float num9 = x - num7;
			float num10 = y - num8;
			int num11;
			int num12;
			if (num9 > num10)
			{
				num11 = 1;
				num12 = 0;
			}
			else
			{
				num11 = 0;
				num12 = 1;
			}
			float num13 = num9 - (float)num11 + 0.21132487f;
			float num14 = num10 - (float)num12 + 0.21132487f;
			float num15 = num9 - 1f + 0.42264974f;
			float num16 = num10 - 1f + 0.42264974f;
			int num17 = num4 % 256;
			int num18 = num5 % 256;
			float num19 = 0.5f - num9 * num9 - num10 * num10;
			float num20;
			if (num19 < 0f)
			{
				num20 = 0f;
			}
			else
			{
				num19 *= num19;
				num20 = num19 * num19 * Noise.grad((int)Noise.perm[num17 + (int)Noise.perm[num18]], num9, num10);
			}
			float num21 = 0.5f - num13 * num13 - num14 * num14;
			float num22;
			if (num21 < 0f)
			{
				num22 = 0f;
			}
			else
			{
				num21 *= num21;
				num22 = num21 * num21 * Noise.grad((int)Noise.perm[num17 + num11 + (int)Noise.perm[num18 + num12]], num13, num14);
			}
			float num23 = 0.5f - num15 * num15 - num16 * num16;
			float num24;
			if (num23 < 0f)
			{
				num24 = 0f;
			}
			else
			{
				num23 *= num23;
				num24 = num23 * num23 * Noise.grad((int)Noise.perm[num17 + 1 + (int)Noise.perm[num18 + 1]], num15, num16);
			}
			return 40f * (num20 + num22 + num24);
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x0020304C File Offset: 0x0020124C
		public static float Generate(float x, float y, float z)
		{
			float num = (x + y + z) * 0.33333334f;
			float num2 = x + num;
			float num3 = y + num;
			float num4 = z + num;
			int num5 = Noise.FastFloor(num2);
			int num6 = Noise.FastFloor(num3);
			int num7 = Noise.FastFloor(num4);
			float num8 = (float)(num5 + num6 + num7) * 0.16666667f;
			float num9 = (float)num5 - num8;
			float num10 = (float)num6 - num8;
			float num11 = (float)num7 - num8;
			float num12 = x - num9;
			float num13 = y - num10;
			float num14 = z - num11;
			int num15;
			int num16;
			int num17;
			int num18;
			int num19;
			int num20;
			if (num12 >= num13)
			{
				if (num13 >= num14)
				{
					num15 = 1;
					num16 = 0;
					num17 = 0;
					num18 = 1;
					num19 = 1;
					num20 = 0;
				}
				else if (num12 >= num14)
				{
					num15 = 1;
					num16 = 0;
					num17 = 0;
					num18 = 1;
					num19 = 0;
					num20 = 1;
				}
				else
				{
					num15 = 0;
					num16 = 0;
					num17 = 1;
					num18 = 1;
					num19 = 0;
					num20 = 1;
				}
			}
			else if (num13 < num14)
			{
				num15 = 0;
				num16 = 0;
				num17 = 1;
				num18 = 0;
				num19 = 1;
				num20 = 1;
			}
			else if (num12 < num14)
			{
				num15 = 0;
				num16 = 1;
				num17 = 0;
				num18 = 0;
				num19 = 1;
				num20 = 1;
			}
			else
			{
				num15 = 0;
				num16 = 1;
				num17 = 0;
				num18 = 1;
				num19 = 1;
				num20 = 0;
			}
			float num21 = num12 - (float)num15 + 0.16666667f;
			float num22 = num13 - (float)num16 + 0.16666667f;
			float num23 = num14 - (float)num17 + 0.16666667f;
			float num24 = num12 - (float)num18 + 0.33333334f;
			float num25 = num13 - (float)num19 + 0.33333334f;
			float num26 = num14 - (float)num20 + 0.33333334f;
			float num27 = num12 - 1f + 0.5f;
			float num28 = num13 - 1f + 0.5f;
			float num29 = num14 - 1f + 0.5f;
			int num30 = Noise.Mod(num5, 256);
			int num31 = Noise.Mod(num6, 256);
			int num32 = Noise.Mod(num7, 256);
			float num33 = 0.6f - num12 * num12 - num13 * num13 - num14 * num14;
			float num34;
			if (num33 < 0f)
			{
				num34 = 0f;
			}
			else
			{
				num33 *= num33;
				num34 = num33 * num33 * Noise.grad((int)Noise.perm[num30 + (int)Noise.perm[num31 + (int)Noise.perm[num32]]], num12, num13, num14);
			}
			float num35 = 0.6f - num21 * num21 - num22 * num22 - num23 * num23;
			float num36;
			if (num35 < 0f)
			{
				num36 = 0f;
			}
			else
			{
				num35 *= num35;
				num36 = num35 * num35 * Noise.grad((int)Noise.perm[num30 + num15 + (int)Noise.perm[num31 + num16 + (int)Noise.perm[num32 + num17]]], num21, num22, num23);
			}
			float num37 = 0.6f - num24 * num24 - num25 * num25 - num26 * num26;
			float num38;
			if (num37 < 0f)
			{
				num38 = 0f;
			}
			else
			{
				num37 *= num37;
				num38 = num37 * num37 * Noise.grad((int)Noise.perm[num30 + num18 + (int)Noise.perm[num31 + num19 + (int)Noise.perm[num32 + num20]]], num24, num25, num26);
			}
			float num39 = 0.6f - num27 * num27 - num28 * num28 - num29 * num29;
			float num40;
			if (num39 < 0f)
			{
				num40 = 0f;
			}
			else
			{
				num39 *= num39;
				num40 = num39 * num39 * Noise.grad((int)Noise.perm[num30 + 1 + (int)Noise.perm[num31 + 1 + (int)Noise.perm[num32 + 1]]], num27, num28, num29);
			}
			return 32f * (num34 + num36 + num38 + num40);
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x002033E0 File Offset: 0x002015E0
		private static int FastFloor(float x)
		{
			return (x <= 0f) ? ((int)x - 1) : ((int)x);
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x002033F8 File Offset: 0x002015F8
		private static int Mod(int x, int m)
		{
			int num = x % m;
			return (num >= 0) ? num : (num + m);
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x0020341C File Offset: 0x0020161C
		private static float grad(int hash, float x)
		{
			int num = hash & 15;
			float num2 = 1f + (float)(num & 7);
			if ((num & 8) != 0)
			{
				num2 = -num2;
			}
			return num2 * x;
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x00203448 File Offset: 0x00201648
		private static float grad(int hash, float x, float y)
		{
			int num = hash & 7;
			float num2 = ((num >= 4) ? y : x);
			float num3 = ((num >= 4) ? x : y);
			return (((num & 1) == 0) ? num2 : (-num2)) + (((num & 2) == 0) ? (2f * num3) : (-2f * num3));
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x002034A4 File Offset: 0x002016A4
		private static float grad(int hash, float x, float y, float z)
		{
			int num = hash & 15;
			float num2 = ((num >= 8) ? y : x);
			float num3 = ((num >= 4) ? ((num != 12 && num != 14) ? z : x) : y);
			return (((num & 1) == 0) ? num2 : (-num2)) + (((num & 2) == 0) ? num3 : (-num3));
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x0020350C File Offset: 0x0020170C
		private static float grad(int hash, float x, float y, float z, float t)
		{
			int num = hash & 31;
			float num2 = ((num >= 24) ? y : x);
			float num3 = ((num >= 16) ? z : y);
			float num4 = ((num >= 8) ? t : z);
			return (((num & 1) == 0) ? num2 : (-num2)) + (((num & 2) == 0) ? num3 : (-num3)) + (((num & 4) == 0) ? num4 : (-num4));
		}

		// Token: 0x04004DF7 RID: 19959
		public static byte[] perm = new byte[]
		{
			151, 160, 137, 91, 90, 15, 131, 13, 201, 95,
			96, 53, 194, 233, 7, 225, 140, 36, 103, 30,
			69, 142, 8, 99, 37, 240, 21, 10, 23, 190,
			6, 148, 247, 120, 234, 75, 0, 26, 197, 62,
			94, 252, 219, 203, 117, 35, 11, 32, 57, 177,
			33, 88, 237, 149, 56, 87, 174, 20, 125, 136,
			171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
			27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
			60, 211, 133, 230, 220, 105, 92, 41, 55, 46,
			245, 40, 244, 102, 143, 54, 65, 25, 63, 161,
			1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
			18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
			164, 100, 109, 198, 173, 186, 3, 64, 52, 217,
			226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
			byte.MaxValue, 82, 85, 212, 207, 206, 59, 227, 47, 16,
			58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
			119, 248, 152, 2, 44, 154, 163, 70, 221, 153,
			101, 155, 167, 43, 172, 9, 129, 22, 39, 253,
			19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
			112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
			238, 210, 144, 12, 191, 179, 162, 241, 81, 51,
			145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
			181, 199, 106, 157, 184, 84, 204, 176, 115, 121,
			50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
			222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
			78, 66, 215, 61, 156, 180, 151, 160, 137, 91,
			90, 15, 131, 13, 201, 95, 96, 53, 194, 233,
			7, 225, 140, 36, 103, 30, 69, 142, 8, 99,
			37, 240, 21, 10, 23, 190, 6, 148, 247, 120,
			234, 75, 0, 26, 197, 62, 94, 252, 219, 203,
			117, 35, 11, 32, 57, 177, 33, 88, 237, 149,
			56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
			74, 165, 71, 134, 139, 48, 27, 166, 77, 146,
			158, 231, 83, 111, 229, 122, 60, 211, 133, 230,
			220, 105, 92, 41, 55, 46, 245, 40, 244, 102,
			143, 54, 65, 25, 63, 161, 1, 216, 80, 73,
			209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
			135, 130, 116, 188, 159, 86, 164, 100, 109, 198,
			173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
			5, 202, 38, 147, 118, 126, byte.MaxValue, 82, 85, 212,
			207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
			28, 42, 223, 183, 170, 213, 119, 248, 152, 2,
			44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
			172, 9, 129, 22, 39, 253, 19, 98, 108, 110,
			79, 113, 224, 232, 178, 185, 112, 104, 218, 246,
			97, 228, 251, 34, 242, 193, 238, 210, 144, 12,
			191, 179, 162, 241, 81, 51, 145, 235, 249, 14,
			239, 107, 49, 192, 214, 31, 181, 199, 106, 157,
			184, 84, 204, 176, 115, 121, 50, 45, 127, 4,
			150, 254, 138, 236, 205, 93, 222, 114, 67, 29,
			24, 72, 243, 141, 128, 195, 78, 66, 215, 61,
			156, 180
		};
	}
}
