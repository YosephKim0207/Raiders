using System;

namespace TestSimpleRNG
{
	// Token: 0x02000F6B RID: 3947
	public class SimpleRNG
	{
		// Token: 0x0600550D RID: 21773 RVA: 0x00204DB8 File Offset: 0x00202FB8
		public static void SetSeed(uint u, uint v)
		{
			if (u != 0U)
			{
				SimpleRNG.m_w = u;
			}
			if (v != 0U)
			{
				SimpleRNG.m_z = v;
			}
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x00204DD4 File Offset: 0x00202FD4
		public static void SetSeed(uint u)
		{
			SimpleRNG.m_w = u;
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x00204DDC File Offset: 0x00202FDC
		public static void SetSeedFromSystemTime()
		{
			long num = DateTime.Now.ToFileTime();
			SimpleRNG.SetSeed((uint)(num >> 16), (uint)(num % 4294967296L));
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x00204E10 File Offset: 0x00203010
		public static double GetUniform()
		{
			uint @uint = SimpleRNG.GetUint();
			return (@uint + 1.0) * 2.328306435454494E-10;
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x00204E3C File Offset: 0x0020303C
		private static uint GetUint()
		{
			SimpleRNG.m_z = 36969U * (SimpleRNG.m_z & 65535U) + (SimpleRNG.m_z >> 16);
			SimpleRNG.m_w = 18000U * (SimpleRNG.m_w & 65535U) + (SimpleRNG.m_w >> 16);
			return (SimpleRNG.m_z << 16) + SimpleRNG.m_w;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x00204E98 File Offset: 0x00203098
		public static double GetNormal()
		{
			double uniform = SimpleRNG.GetUniform();
			double uniform2 = SimpleRNG.GetUniform();
			double num = Math.Sqrt(-2.0 * Math.Log(uniform));
			double num2 = 6.283185307179586 * uniform2;
			return num * Math.Sin(num2);
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x00204EDC File Offset: 0x002030DC
		public static double GetNormal(double mean, double standardDeviation)
		{
			if (standardDeviation <= 0.0)
			{
				string text = string.Format("Shape must be positive. Received {0}.", standardDeviation);
				throw new ArgumentOutOfRangeException(text);
			}
			return mean + standardDeviation * SimpleRNG.GetNormal();
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x00204F1C File Offset: 0x0020311C
		public static double GetExponential()
		{
			return -Math.Log(SimpleRNG.GetUniform());
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x00204F2C File Offset: 0x0020312C
		public static double GetExponential(double mean)
		{
			if (mean <= 0.0)
			{
				string text = string.Format("Mean must be positive. Received {0}.", mean);
				throw new ArgumentOutOfRangeException(text);
			}
			return mean * SimpleRNG.GetExponential();
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x00204F68 File Offset: 0x00203168
		public static double GetGamma(double shape, double scale)
		{
			if (shape >= 1.0)
			{
				double num = shape - 0.3333333333333333;
				double num2 = 1.0 / Math.Sqrt(9.0 * num);
				double num3;
				double uniform;
				double num4;
				do
				{
					double normal;
					do
					{
						normal = SimpleRNG.GetNormal();
						num3 = 1.0 + num2 * normal;
					}
					while (num3 <= 0.0);
					num3 = num3 * num3 * num3;
					uniform = SimpleRNG.GetUniform();
					num4 = normal * normal;
				}
				while (uniform >= 1.0 - 0.0331 * num4 * num4 && Math.Log(uniform) >= 0.5 * num4 + num * (1.0 - num3 + Math.Log(num3)));
				return scale * num * num3;
			}
			if (shape <= 0.0)
			{
				string text = string.Format("Shape must be positive. Received {0}.", shape);
				throw new ArgumentOutOfRangeException(text);
			}
			double gamma = SimpleRNG.GetGamma(shape + 1.0, 1.0);
			double uniform2 = SimpleRNG.GetUniform();
			return scale * gamma * Math.Pow(uniform2, 1.0 / shape);
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x002050A4 File Offset: 0x002032A4
		public static double GetChiSquare(double degreesOfFreedom)
		{
			return SimpleRNG.GetGamma(0.5 * degreesOfFreedom, 2.0);
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x002050C0 File Offset: 0x002032C0
		public static double GetInverseGamma(double shape, double scale)
		{
			return 1.0 / SimpleRNG.GetGamma(shape, 1.0 / scale);
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x002050E0 File Offset: 0x002032E0
		public static double GetWeibull(double shape, double scale)
		{
			if (shape <= 0.0 || scale <= 0.0)
			{
				string text = string.Format("Shape and scale parameters must be positive. Recieved shape {0} and scale{1}.", shape, scale);
				throw new ArgumentOutOfRangeException(text);
			}
			return scale * Math.Pow(-Math.Log(SimpleRNG.GetUniform()), 1.0 / shape);
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x00205148 File Offset: 0x00203348
		public static double GetCauchy(double median, double scale)
		{
			if (scale <= 0.0)
			{
				string text = string.Format("Scale must be positive. Received {0}.", scale);
				throw new ArgumentException(text);
			}
			double uniform = SimpleRNG.GetUniform();
			return median + scale * Math.Tan(3.141592653589793 * (uniform - 0.5));
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x002051A0 File Offset: 0x002033A0
		public static double GetStudentT(double degreesOfFreedom)
		{
			if (degreesOfFreedom <= 0.0)
			{
				string text = string.Format("Degrees of freedom must be positive. Received {0}.", degreesOfFreedom);
				throw new ArgumentException(text);
			}
			double normal = SimpleRNG.GetNormal();
			double chiSquare = SimpleRNG.GetChiSquare(degreesOfFreedom);
			return normal / Math.Sqrt(chiSquare / degreesOfFreedom);
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x002051EC File Offset: 0x002033EC
		public static double GetLaplace(double mean, double scale)
		{
			double uniform = SimpleRNG.GetUniform();
			return (uniform >= 0.5) ? (mean - scale * Math.Log(2.0 * (1.0 - uniform))) : (mean + scale * Math.Log(2.0 * uniform));
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x00205248 File Offset: 0x00203448
		public static double GetLogNormal(double mu, double sigma)
		{
			return Math.Exp(SimpleRNG.GetNormal(mu, sigma));
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x00205258 File Offset: 0x00203458
		public static double GetBeta(double a, double b)
		{
			if (a <= 0.0 || b <= 0.0)
			{
				string text = string.Format("Beta parameters must be positive. Received {0} and {1}.", a, b);
				throw new ArgumentOutOfRangeException(text);
			}
			double gamma = SimpleRNG.GetGamma(a, 1.0);
			double gamma2 = SimpleRNG.GetGamma(b, 1.0);
			return gamma / (gamma + gamma2);
		}

		// Token: 0x04004E02 RID: 19970
		private static uint m_w = 521288629U;

		// Token: 0x04004E03 RID: 19971
		private static uint m_z = 362436069U;
	}
}
