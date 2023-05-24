using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A8D RID: 2701
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Projects the location found with Get Location Info to a 2d map using common projections.")]
	public class ProjectLocationToMap : FsmStateAction
	{
		// Token: 0x06003950 RID: 14672 RVA: 0x001256CC File Offset: 0x001238CC
		public override void Reset()
		{
			this.GPSLocation = new FsmVector3
			{
				UseVariable = true
			};
			this.mapProjection = ProjectLocationToMap.MapProjection.EquidistantCylindrical;
			this.minLongitude = -180f;
			this.maxLongitude = 180f;
			this.minLatitude = -90f;
			this.maxLatitude = 90f;
			this.minX = 0f;
			this.minY = 0f;
			this.width = 1f;
			this.height = 1f;
			this.normalized = true;
			this.projectedX = null;
			this.projectedY = null;
			this.everyFrame = false;
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x00125798 File Offset: 0x00123998
		public override void OnEnter()
		{
			if (this.GPSLocation.IsNone)
			{
				base.Finish();
				return;
			}
			this.DoProjectGPSLocation();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x001257C8 File Offset: 0x001239C8
		public override void OnUpdate()
		{
			this.DoProjectGPSLocation();
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x001257D0 File Offset: 0x001239D0
		private void DoProjectGPSLocation()
		{
			this.x = Mathf.Clamp(this.GPSLocation.Value.x, this.minLongitude.Value, this.maxLongitude.Value);
			this.y = Mathf.Clamp(this.GPSLocation.Value.y, this.minLatitude.Value, this.maxLatitude.Value);
			ProjectLocationToMap.MapProjection mapProjection = this.mapProjection;
			if (mapProjection != ProjectLocationToMap.MapProjection.EquidistantCylindrical)
			{
				if (mapProjection == ProjectLocationToMap.MapProjection.Mercator)
				{
					this.DoMercatorProjection();
				}
			}
			else
			{
				this.DoEquidistantCylindrical();
			}
			this.x *= this.width.Value;
			this.y *= this.height.Value;
			this.projectedX.Value = ((!this.normalized.Value) ? (this.minX.Value + this.x * (float)Screen.width) : (this.minX.Value + this.x));
			this.projectedY.Value = ((!this.normalized.Value) ? (this.minY.Value + this.y * (float)Screen.height) : (this.minY.Value + this.y));
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x0012593C File Offset: 0x00123B3C
		private void DoEquidistantCylindrical()
		{
			this.x = (this.x - this.minLongitude.Value) / (this.maxLongitude.Value - this.minLongitude.Value);
			this.y = (this.y - this.minLatitude.Value) / (this.maxLatitude.Value - this.minLatitude.Value);
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x001259AC File Offset: 0x00123BAC
		private void DoMercatorProjection()
		{
			this.x = (this.x - this.minLongitude.Value) / (this.maxLongitude.Value - this.minLongitude.Value);
			float num = ProjectLocationToMap.LatitudeToMercator(this.minLatitude.Value);
			float num2 = ProjectLocationToMap.LatitudeToMercator(this.maxLatitude.Value);
			this.y = (ProjectLocationToMap.LatitudeToMercator(this.GPSLocation.Value.y) - num) / (num2 - num);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x00125A30 File Offset: 0x00123C30
		private static float LatitudeToMercator(float latitudeInDegrees)
		{
			float num = Mathf.Clamp(latitudeInDegrees, -85f, 85f);
			num = 0.017453292f * num;
			return Mathf.Log(Mathf.Tan(num / 2f + 0.7853982f));
		}

		// Token: 0x04002B9B RID: 11163
		[Tooltip("Location vector in degrees longitude and latitude. Typically returned by the Get Location Info action.")]
		public FsmVector3 GPSLocation;

		// Token: 0x04002B9C RID: 11164
		[Tooltip("The projection used by the map.")]
		public ProjectLocationToMap.MapProjection mapProjection;

		// Token: 0x04002B9D RID: 11165
		[HasFloatSlider(-180f, 180f)]
		[ActionSection("Map Region")]
		public FsmFloat minLongitude;

		// Token: 0x04002B9E RID: 11166
		[HasFloatSlider(-180f, 180f)]
		public FsmFloat maxLongitude;

		// Token: 0x04002B9F RID: 11167
		[HasFloatSlider(-90f, 90f)]
		public FsmFloat minLatitude;

		// Token: 0x04002BA0 RID: 11168
		[HasFloatSlider(-90f, 90f)]
		public FsmFloat maxLatitude;

		// Token: 0x04002BA1 RID: 11169
		[ActionSection("Screen Region")]
		public FsmFloat minX;

		// Token: 0x04002BA2 RID: 11170
		public FsmFloat minY;

		// Token: 0x04002BA3 RID: 11171
		public FsmFloat width;

		// Token: 0x04002BA4 RID: 11172
		public FsmFloat height;

		// Token: 0x04002BA5 RID: 11173
		[ActionSection("Projection")]
		[Tooltip("Store the projected X coordinate in a Float Variable. Use this to display a marker on the map.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat projectedX;

		// Token: 0x04002BA6 RID: 11174
		[Tooltip("Store the projected Y coordinate in a Float Variable. Use this to display a marker on the map.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat projectedY;

		// Token: 0x04002BA7 RID: 11175
		[Tooltip("If true all coordinates in this action are normalized (0-1); otherwise coordinates are in pixels.")]
		public FsmBool normalized;

		// Token: 0x04002BA8 RID: 11176
		public bool everyFrame;

		// Token: 0x04002BA9 RID: 11177
		private float x;

		// Token: 0x04002BAA RID: 11178
		private float y;

		// Token: 0x02000A8E RID: 2702
		public enum MapProjection
		{
			// Token: 0x04002BAC RID: 11180
			EquidistantCylindrical,
			// Token: 0x04002BAD RID: 11181
			Mercator
		}
	}
}
