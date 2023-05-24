using System;
using UnityEngine;

// Token: 0x0200046C RID: 1132
[AddComponentMenu("Daikon Forge/Examples/Radar/Radar Marker")]
public class dfRadarMarker : MonoBehaviour
{
	// Token: 0x06001A24 RID: 6692 RVA: 0x0007A0E8 File Offset: 0x000782E8
	public void OnEnable()
	{
		if (string.IsNullOrEmpty(this.markerType))
		{
			return;
		}
		if (this.radar == null)
		{
			this.radar = UnityEngine.Object.FindObjectOfType(typeof(dfRadarMain)) as dfRadarMain;
			if (this.radar == null)
			{
				Debug.LogWarning("No radar found");
				return;
			}
		}
		this.radar.AddMarker(this);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x0007A15C File Offset: 0x0007835C
	public void OnDisable()
	{
		if (this.radar != null)
		{
			this.radar.RemoveMarker(this);
		}
	}

	// Token: 0x0400147E RID: 5246
	public dfRadarMain radar;

	// Token: 0x0400147F RID: 5247
	public string markerType;

	// Token: 0x04001480 RID: 5248
	public string outOfRangeType;

	// Token: 0x04001481 RID: 5249
	[NonSerialized]
	internal dfControl marker;

	// Token: 0x04001482 RID: 5250
	[NonSerialized]
	internal dfControl outOfRangeMarker;
}
