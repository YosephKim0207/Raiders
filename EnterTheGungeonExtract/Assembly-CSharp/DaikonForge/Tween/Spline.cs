using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x02000527 RID: 1319
	public class Spline : IPathIterator, IEnumerable<Spline.ControlPoint>, IEnumerable
	{
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0008E22C File Offset: 0x0008C42C
		public float Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0008E234 File Offset: 0x0008C434
		public Vector3 GetPosition(float time)
		{
			time = Mathf.Abs(time) % 1f;
			float num = 1f / (float)this.ControlPoints.Count;
			if (!this.Wrap)
			{
				time *= num * (float)(this.ControlPoints.Count - 1);
			}
			int num2 = Mathf.FloorToInt(time * (float)this.ControlPoints.Count);
			Spline.ControlPoint node = this.getNode(num2 - 1);
			Spline.ControlPoint node2 = this.getNode(num2);
			Spline.ControlPoint node3 = this.getNode(num2 + 1);
			Spline.ControlPoint node4 = this.getNode(num2 + 2);
			float num3 = num * (float)num2;
			time -= num3;
			time /= num;
			return this.SplineMethod.Evaluate(node.Position, node2.Position, node3.Position, node4.Position, time);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0008E2F4 File Offset: 0x0008C4F4
		public float AdjustTimeToConstant(float time)
		{
			if (time < 0f || time > 1f)
			{
				throw new ArgumentException("The length parameter must be a value between 0 and 1 (inclusive)");
			}
			int parameterIndex = this.getParameterIndex(time);
			int num = this.ControlPoints.Count + ((!this.Wrap) ? (-1) : 0);
			float num2 = 1f / (float)num;
			Spline.ControlPoint controlPoint = this.ControlPoints[parameterIndex];
			float num3 = controlPoint.Length / this.length;
			float num4 = (time - controlPoint.Time) / num3;
			return (float)parameterIndex * num2 + num4 * num2;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0008E388 File Offset: 0x0008C588
		public Vector3 GetOrientation(float time)
		{
			int num = this.ControlPoints.Count - ((!this.Wrap) ? 2 : 1);
			while (this.ControlPoints[num].Time > time)
			{
				num--;
			}
			int num2 = ((num != this.ControlPoints.Count - 1) ? (num + 1) : 0);
			Spline.ControlPoint controlPoint = this.ControlPoints[num];
			Spline.ControlPoint controlPoint2 = this.ControlPoints[num2];
			float num3 = controlPoint.Length / this.length;
			float num4 = (time - controlPoint.Time) / num3;
			return Vector3.Lerp(controlPoint.Orientation, controlPoint2.Orientation, num4);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0008E43C File Offset: 0x0008C63C
		public Vector3 GetTangent(float time)
		{
			return this.GetTangent(time, 0.01f);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0008E44C File Offset: 0x0008C64C
		public Vector3 GetTangent(float time, float lookAhead)
		{
			if (!this.Wrap && time > 1f - lookAhead)
			{
				time = 1f - lookAhead;
			}
			Vector3 position = this.GetPosition(time);
			Vector3 position2 = this.GetPosition(time + lookAhead);
			Vector3 vector = position2 - position;
			vector.Normalize();
			return vector;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0008E49C File Offset: 0x0008C69C
		public Spline AddNode(Spline.ControlPoint node)
		{
			this.ControlPoints.Add(node);
			return this;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0008E4AC File Offset: 0x0008C6AC
		public void ComputeSpline()
		{
			this.length = 0f;
			for (int i = 0; i < this.ControlPoints.Count; i++)
			{
				this.ControlPoints[i].Time = 0f;
				this.ControlPoints[i].Length = 0f;
			}
			if (this.ControlPoints.Count < 2)
			{
				return;
			}
			int num = 16;
			int num2 = this.ControlPoints.Count + ((!this.Wrap) ? (-1) : 0);
			float num3 = 1f / (float)num2;
			float num4 = num3 / (float)num;
			for (int j = 0; j < num2; j++)
			{
				float num5 = (float)j * num3;
				Vector3 vector = this.ControlPoints[j].Position;
				for (int k = 1; k < num; k++)
				{
					Vector3 position = this.GetPosition(num5 + (float)k * num4);
					this.ControlPoints[j].Length += Vector3.Distance(vector, position);
					vector = position;
				}
			}
			this.length = 0f;
			for (int l = 0; l < this.ControlPoints.Count; l++)
			{
				this.length += this.ControlPoints[l].Length;
			}
			float num6 = 0f;
			for (int m = 0; m < num2; m++)
			{
				this.ControlPoints[m].Time = num6 / this.length;
				num6 += this.ControlPoints[m].Length;
			}
			if (!this.Wrap)
			{
				this.ControlPoints[this.ControlPoints.Count - 1].Time = 1f;
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0008E694 File Offset: 0x0008C894
		private int getParameterIndex(float time)
		{
			int num = 0;
			for (int i = 1; i < this.ControlPoints.Count; i++)
			{
				Spline.ControlPoint controlPoint = this.ControlPoints[i];
				if (controlPoint.Length == 0f || controlPoint.Time > time)
				{
					break;
				}
				num = i;
			}
			return num;
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0008E6F0 File Offset: 0x0008C8F0
		private Spline.ControlPoint getNode(int nodeIndex)
		{
			if (this.Wrap)
			{
				if (nodeIndex < 0)
				{
					nodeIndex += this.ControlPoints.Count;
				}
				if (nodeIndex >= this.ControlPoints.Count)
				{
					nodeIndex -= this.ControlPoints.Count;
				}
			}
			else
			{
				nodeIndex = Mathf.Clamp(nodeIndex, 0, this.ControlPoints.Count - 1);
			}
			return this.ControlPoints[nodeIndex];
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0008E768 File Offset: 0x0008C968
		private IEnumerator<Spline.ControlPoint> GetCustomEnumerator()
		{
			int index = 0;
			while (index < this.ControlPoints.Count)
			{
				List<Spline.ControlPoint> controlPoints = this.ControlPoints;
				int num;
				index = (num = index) + 1;
				yield return controlPoints[num];
			}
			yield break;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0008E784 File Offset: 0x0008C984
		public IEnumerator<Spline.ControlPoint> GetEnumerator()
		{
			return this.GetCustomEnumerator();
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0008E78C File Offset: 0x0008C98C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetCustomEnumerator();
		}

		// Token: 0x04001759 RID: 5977
		public ISplineInterpolator SplineMethod = new CatmullRomSpline();

		// Token: 0x0400175A RID: 5978
		public List<Spline.ControlPoint> ControlPoints = new List<Spline.ControlPoint>();

		// Token: 0x0400175B RID: 5979
		public bool Wrap;

		// Token: 0x0400175C RID: 5980
		private float length;

		// Token: 0x02000528 RID: 1320
		public class ControlPoint
		{
			// Token: 0x06001FC1 RID: 8129 RVA: 0x0008E794 File Offset: 0x0008C994
			public ControlPoint(Vector3 Position, Vector3 Tangent)
			{
				this.Position = Position;
				this.Orientation = Tangent;
				this.Length = 0f;
			}

			// Token: 0x0400175D RID: 5981
			public Vector3 Position;

			// Token: 0x0400175E RID: 5982
			public Vector3 Orientation;

			// Token: 0x0400175F RID: 5983
			public float Length;

			// Token: 0x04001760 RID: 5984
			public float Time;
		}
	}
}
