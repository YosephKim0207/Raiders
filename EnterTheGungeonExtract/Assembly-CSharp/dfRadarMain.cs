using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200046A RID: 1130
[AddComponentMenu("Daikon Forge/Examples/Radar/Radar Main")]
public class dfRadarMain : MonoBehaviour
{
	// Token: 0x06001A19 RID: 6681 RVA: 0x00079CFC File Offset: 0x00077EFC
	public void Start()
	{
		this.ensureControlReference();
		for (int i = 0; i < this.markerTypes.Count; i++)
		{
			this.markerTypes[i].IsVisible = false;
		}
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x00079D40 File Offset: 0x00077F40
	public void LateUpdate()
	{
		this.updateMarkers();
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x00079D48 File Offset: 0x00077F48
	public void AddMarker(dfRadarMarker item)
	{
		if (string.IsNullOrEmpty(item.markerType))
		{
			return;
		}
		this.ensureControlReference();
		item.marker = this.instantiateMarker(item.markerType);
		if (item.marker == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(item.outOfRangeType))
		{
			item.outOfRangeMarker = this.instantiateMarker(item.outOfRangeType);
		}
		this.markers.Add(item);
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x00079DC0 File Offset: 0x00077FC0
	private dfControl instantiateMarker(string markerName)
	{
		dfControl dfControl = this.markerTypes.Find((dfControl x) => x.name == markerName);
		if (dfControl == null)
		{
			Debug.LogError("Marker type not found: " + markerName);
			return null;
		}
		dfControl dfControl2 = UnityEngine.Object.Instantiate<dfControl>(dfControl);
		dfControl2.hideFlags = HideFlags.DontSave;
		dfControl2.IsVisible = true;
		this.control.AddControl(dfControl2);
		return dfControl2;
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x00079E38 File Offset: 0x00078038
	public void RemoveMarker(dfRadarMarker item)
	{
		if (this.markers.Remove(item))
		{
			this.ensureControlReference();
			if (item.marker != null)
			{
				UnityEngine.Object.Destroy(item.marker);
			}
			if (item.outOfRangeMarker != null)
			{
				UnityEngine.Object.Destroy(item.outOfRangeMarker);
			}
			this.control.RemoveControl(item.marker);
		}
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x00079EA8 File Offset: 0x000780A8
	private void ensureControlReference()
	{
		this.control = base.GetComponent<dfControl>();
		if (this.control == null)
		{
			Debug.LogError("Host control not found");
			base.enabled = false;
			return;
		}
		this.control.Pivot = dfPivotPoint.MiddleCenter;
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x00079EE8 File Offset: 0x000780E8
	private void updateMarkers()
	{
		for (int i = 0; i < this.markers.Count; i++)
		{
			this.updateMarker(this.markers[i]);
		}
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x00079F24 File Offset: 0x00078124
	private void updateMarker(dfRadarMarker item)
	{
		Vector3 position = this.target.transform.position;
		Vector3 position2 = item.transform.position;
		float num = position.x - position2.x;
		float num2 = position.z - position2.z;
		float num3 = Mathf.Atan2(num, -num2) * 57.29578f + 90f + this.target.transform.eulerAngles.y;
		float num4 = Vector3.Distance(position, position2);
		if (num4 > this.maxDetectDistance)
		{
			item.marker.IsVisible = false;
			if (item.outOfRangeMarker != null)
			{
				dfControl outOfRangeMarker = item.outOfRangeMarker;
				outOfRangeMarker.IsVisible = true;
				outOfRangeMarker.transform.position = this.control.transform.position;
				outOfRangeMarker.transform.eulerAngles = new Vector3(0f, 0f, num3 - 90f);
			}
			return;
		}
		if (item.outOfRangeMarker != null)
		{
			item.outOfRangeMarker.IsVisible = false;
		}
		float num5 = num4 * Mathf.Cos(num3 * 0.017453292f);
		float num6 = num4 * Mathf.Sin(num3 * 0.017453292f);
		float num7 = (float)this.radarRadius / this.maxDetectDistance * this.control.PixelsToUnits();
		num5 *= num7;
		num6 *= num7;
		item.marker.transform.localPosition = new Vector3(num5, num6, 0f);
		item.marker.IsVisible = true;
		item.marker.Pivot = dfPivotPoint.MiddleCenter;
	}

	// Token: 0x04001477 RID: 5239
	public GameObject target;

	// Token: 0x04001478 RID: 5240
	public float maxDetectDistance = 100f;

	// Token: 0x04001479 RID: 5241
	public int radarRadius = 100;

	// Token: 0x0400147A RID: 5242
	public List<dfControl> markerTypes;

	// Token: 0x0400147B RID: 5243
	private List<dfRadarMarker> markers = new List<dfRadarMarker>();

	// Token: 0x0400147C RID: 5244
	private dfControl control;
}
