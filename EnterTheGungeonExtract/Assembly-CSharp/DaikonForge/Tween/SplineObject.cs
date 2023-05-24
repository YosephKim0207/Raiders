using System;
using System.Collections.Generic;
using System.Linq;
using DaikonForge.Editor;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200052D RID: 1325
	[AddComponentMenu("Daikon Forge/Tween/Spline Path")]
	[InspectorGroupOrder(new string[] { "General", "Path", "Control Points" })]
	[ExecuteInEditMode]
	public class SplineObject : MonoBehaviour
	{
		// Token: 0x06001FCE RID: 8142 RVA: 0x0008E900 File Offset: 0x0008CB00
		public void Awake()
		{
			if (this.ControlPoints.Count == 0)
			{
				SplineNode[] componentsInChildren = base.transform.GetComponentsInChildren<SplineNode>();
				this.ControlPoints.AddRange(componentsInChildren.Select((SplineNode x) => x.transform).ToList<Transform>());
			}
			this.CalculateSpline();
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0008E964 File Offset: 0x0008CB64
		public Vector3 Evaluate(float time)
		{
			return this.Spline.GetPosition(time);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0008E974 File Offset: 0x0008CB74
		public float GetTimeAtNode(int nodeIndex)
		{
			this.CalculateSpline();
			Spline.ControlPoint controlPoint = this.Spline.ControlPoints[nodeIndex];
			return controlPoint.Time;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0008E9A0 File Offset: 0x0008CBA0
		public SplineNode AddNode()
		{
			this.CalculateSpline();
			Vector3 vector = base.transform.position;
			if (this.Spline.ControlPoints.Count > 2)
			{
				Vector3 position = this.Spline.ControlPoints.Last<Spline.ControlPoint>().Position;
				Vector3 tangent = this.Spline.GetTangent(0.95f);
				vector = position + tangent;
			}
			return this.AddNode(vector);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0008EA0C File Offset: 0x0008CC0C
		public SplineNode AddNode(Vector3 position)
		{
			GameObject gameObject = new GameObject
			{
				name = "SplineNode" + this.Spline.ControlPoints.Count
			};
			SplineNode splineNode = gameObject.AddComponent<SplineNode>();
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = position;
			this.ControlPoints.Add(gameObject.transform);
			this.CalculateSpline();
			return splineNode;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0008EA84 File Offset: 0x0008CC84
		public void CalculateSpline()
		{
			int i = 0;
			while (i < this.ControlPoints.Count)
			{
				if (this.ControlPoints[i] == null)
				{
					this.ControlPoints.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (this.Spline == null)
			{
				this.Spline = new Spline();
			}
			this.Spline.Wrap = this.Wrap;
			this.Spline.ControlPoints.Clear();
			for (int j = 0; j < this.ControlPoints.Count; j++)
			{
				Transform transform = this.ControlPoints[j];
				if (!(transform == null))
				{
					this.Spline.ControlPoints.Add(new Spline.ControlPoint(transform.position, transform.forward));
				}
			}
			this.Spline.ComputeSpline();
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0008EB74 File Offset: 0x0008CD74
		public Bounds GetBounds()
		{
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			int num = 0;
			for (int i = 0; i < this.ControlPoints.Count; i++)
			{
				if (!(this.ControlPoints[i] == null))
				{
					num++;
					Vector3 position = this.ControlPoints[i].transform.position;
					vector = Vector3.Min(vector, position);
					vector2 = Vector3.Max(vector2, position);
				}
			}
			if (num == 0)
			{
				return new Bounds(base.transform.position, Vector3.zero);
			}
			Vector3 vector3 = vector2 - vector;
			return new Bounds(vector + vector3 * 0.5f, vector3);
		}

		// Token: 0x04001766 RID: 5990
		public Spline Spline;

		// Token: 0x04001767 RID: 5991
		[Inspector("Path", Order = 0, Tooltip = "If set to TRUE, the end of the path will wrap around to the beginning")]
		public bool Wrap;

		// Token: 0x04001768 RID: 5992
		[Inspector("Control Points", Order = 1, Tooltip = "Contains the list of Transforms that represent the control points of the path's curve")]
		public List<Transform> ControlPoints = new List<Transform>();
	}
}
