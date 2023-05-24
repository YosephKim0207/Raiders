using System;
using UnityEngine;

namespace com.subjectnerd
{
	// Token: 0x0200182C RID: 6188
	public class GLDraw
	{
		// Token: 0x0600927B RID: 37499 RVA: 0x003DDC84 File Offset: 0x003DBE84
		protected static bool clip_test(float p, float q, ref float u1, ref float u2)
		{
			bool flag = true;
			if ((double)p < 0.0)
			{
				float num = q / p;
				if (num > u2)
				{
					flag = false;
				}
				else if (num > u1)
				{
					u1 = num;
				}
			}
			else if ((double)p > 0.0)
			{
				float num = q / p;
				if (num < u1)
				{
					flag = false;
				}
				else if (num < u2)
				{
					u2 = num;
				}
			}
			else if ((double)q < 0.0)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600927C RID: 37500 RVA: 0x003DDD0C File Offset: 0x003DBF0C
		protected static bool segment_rect_intersection(Rect bounds, ref Vector2 p1, ref Vector2 p2)
		{
			float num = 0f;
			float num2 = 1f;
			float num3 = p2.x - p1.x;
			if (GLDraw.clip_test(-num3, p1.x - bounds.xMin, ref num, ref num2) && GLDraw.clip_test(num3, bounds.xMax - p1.x, ref num, ref num2))
			{
				float num4 = p2.y - p1.y;
				if (GLDraw.clip_test(-num4, p1.y - bounds.yMin, ref num, ref num2) && GLDraw.clip_test(num4, bounds.yMax - p1.y, ref num, ref num2))
				{
					if ((double)num2 < 1.0)
					{
						p2.x = p1.x + num2 * num3;
						p2.y = p1.y + num2 * num4;
					}
					if ((double)num > 0.0)
					{
						p1.x += num * num3;
						p1.y += num * num4;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600927D RID: 37501 RVA: 0x003DDE1C File Offset: 0x003DC01C
		public static void BeginGroup(Rect position)
		{
			GLDraw.clippingEnabled = true;
			GLDraw.clippingBounds = new Rect(0f, 0f, position.width, position.height);
			GUI.BeginGroup(position);
		}

		// Token: 0x0600927E RID: 37502 RVA: 0x003DDE4C File Offset: 0x003DC04C
		public static void EndGroup()
		{
			GUI.EndGroup();
			GLDraw.clippingBounds = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
			GLDraw.clippingEnabled = false;
		}

		// Token: 0x0600927F RID: 37503 RVA: 0x003DDE7C File Offset: 0x003DC07C
		public static void CreateMaterial()
		{
			if (GLDraw.lineMaterial == null)
			{
				GLDraw.lineMaterial = new Material(ShaderCache.Acquire("Brave/DebugLines"));
				GLDraw.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
				GLDraw.lineMaterial.shader.hideFlags = HideFlags.None;
			}
		}

		// Token: 0x06009280 RID: 37504 RVA: 0x003DDECC File Offset: 0x003DC0CC
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			if (Event.current == null)
			{
				return;
			}
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (GLDraw.clippingEnabled && !GLDraw.segment_rect_intersection(GLDraw.clippingBounds, ref start, ref end))
			{
				return;
			}
			GLDraw.CreateMaterial();
			GLDraw.lineMaterial.SetPass(0);
			if (width == 1f)
			{
				GL.Begin(1);
				GL.Color(color);
				Vector3 vector = new Vector3(start.x, start.y, 0f);
				Vector3 vector2 = new Vector3(end.x, end.y, 0f);
				GL.Vertex(vector);
				GL.Vertex(vector2);
			}
			else
			{
				GL.Begin(7);
				GL.Color(color);
				Vector3 vector = new Vector3(end.y, start.x, 0f);
				Vector3 vector2 = new Vector3(start.y, end.x, 0f);
				Vector3 vector3 = (vector - vector2).normalized * width;
				Vector3 vector4 = new Vector3(start.x, start.y, 0f);
				Vector3 vector5 = new Vector3(end.x, end.y, 0f);
				GL.Vertex(vector4 - vector3);
				GL.Vertex(vector4 + vector3);
				GL.Vertex(vector5 + vector3);
				GL.Vertex(vector5 - vector3);
			}
			GL.End();
		}

		// Token: 0x06009281 RID: 37505 RVA: 0x003DE044 File Offset: 0x003DC244
		public static void DrawBox(Rect box, Color color, float width)
		{
			Vector2 vector = new Vector2(box.xMin, box.yMin);
			Vector2 vector2 = new Vector2(box.xMax, box.yMin);
			Vector2 vector3 = new Vector2(box.xMax, box.yMax);
			Vector2 vector4 = new Vector2(box.xMin, box.yMax);
			GLDraw.DrawLine(vector, vector2, color, width);
			GLDraw.DrawLine(vector2, vector3, color, width);
			GLDraw.DrawLine(vector3, vector4, color, width);
			GLDraw.DrawLine(vector4, vector, color, width);
		}

		// Token: 0x06009282 RID: 37506 RVA: 0x003DE0CC File Offset: 0x003DC2CC
		public static void DrawBox(Vector2 topLeftCorner, Vector2 bottomRightCorner, Color color, float width)
		{
			Rect rect = new Rect(topLeftCorner.x, topLeftCorner.y, bottomRightCorner.x - topLeftCorner.x, bottomRightCorner.y - topLeftCorner.y);
			GLDraw.DrawBox(rect, color, width);
		}

		// Token: 0x06009283 RID: 37507 RVA: 0x003DE114 File Offset: 0x003DC314
		public static void DrawRoundedBox(Rect box, float radius, Color color, float width)
		{
			Vector2 vector = new Vector2(box.xMin + radius, box.yMin);
			Vector2 vector2 = new Vector2(box.xMax - radius, box.yMin);
			Vector2 vector3 = new Vector2(box.xMax, box.yMin + radius);
			Vector2 vector4 = new Vector2(box.xMax, box.yMax - radius);
			Vector2 vector5 = new Vector2(box.xMax - radius, box.yMax);
			Vector2 vector6 = new Vector2(box.xMin + radius, box.yMax);
			Vector2 vector7 = new Vector2(box.xMin, box.yMax - radius);
			Vector2 vector8 = new Vector2(box.xMin, box.yMin + radius);
			GLDraw.DrawLine(vector, vector2, color, width);
			GLDraw.DrawLine(vector3, vector4, color, width);
			GLDraw.DrawLine(vector5, vector6, color, width);
			GLDraw.DrawLine(vector7, vector8, color, width);
			float num = radius / 2f;
			Vector2 vector9 = new Vector2(vector8.x, vector8.y + num);
			Vector2 vector10 = new Vector2(vector.x - num, vector.y);
			GLDraw.DrawBezier(vector8, vector9, vector, vector10, color, width);
			vector9 = new Vector2(vector2.x + num, vector2.y);
			vector10 = new Vector2(vector3.x, vector3.y - num);
			GLDraw.DrawBezier(vector2, vector9, vector3, vector10, color, width);
			vector9 = new Vector2(vector4.x, vector4.y + num);
			vector10 = new Vector2(vector5.x + num, vector5.y);
			GLDraw.DrawBezier(vector4, vector9, vector5, vector10, color, width);
			vector9 = new Vector2(vector6.x - num, vector6.y);
			vector10 = new Vector2(vector7.x, vector7.y + num);
			GLDraw.DrawBezier(vector6, vector9, vector7, vector10, color, width);
		}

		// Token: 0x06009284 RID: 37508 RVA: 0x003DE304 File Offset: 0x003DC504
		public static void DrawConnectingCurve(Vector2 start, Vector2 end, Color color, float width)
		{
			Vector2 vector = start - end;
			Vector2 vector2 = start;
			vector2.x -= (vector / 2f).x;
			Vector2 vector3 = end;
			vector3.x += (vector / 2f).x;
			int num = Mathf.FloorToInt(vector.magnitude / 20f * 3f);
			GLDraw.DrawBezier(start, vector2, end, vector3, color, width, num);
		}

		// Token: 0x06009285 RID: 37509 RVA: 0x003DE388 File Offset: 0x003DC588
		public static void DrawBezier(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width)
		{
			int num = Mathf.FloorToInt((start - end).magnitude / 20f) * 3;
			GLDraw.DrawBezier(start, startTangent, end, endTangent, color, width, num);
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x003DE3C0 File Offset: 0x003DC5C0
		public static void DrawBezier(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, int segments)
		{
			Vector2 vector = GLDraw.CubeBezier(start, startTangent, end, endTangent, 0f);
			for (int i = 1; i <= segments; i++)
			{
				Vector2 vector2 = GLDraw.CubeBezier(start, startTangent, end, endTangent, (float)i / (float)segments);
				GLDraw.DrawLine(vector, vector2, color, width);
				vector = vector2;
			}
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x003DE40C File Offset: 0x003DC60C
		private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
		{
			float num = 1f - t;
			float num2 = num * t;
			return num * num * num * s + 3f * num * num2 * st + 3f * num2 * t * et + t * t * t * e;
		}

		// Token: 0x04009A14 RID: 39444
		protected static bool clippingEnabled;

		// Token: 0x04009A15 RID: 39445
		protected static Rect clippingBounds;

		// Token: 0x04009A16 RID: 39446
		public static Material lineMaterial;
	}
}
