using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010E6 RID: 4326
public class AmbientChatter : MonoBehaviour
{
	// Token: 0x06005F47 RID: 24391 RVA: 0x0024A70C File Offset: 0x0024890C
	private void Start()
	{
		this.m_transform = base.transform;
		this.m_startPosition = this.m_transform.position;
		if (this.WanderInRadius)
		{
			base.StartCoroutine(this.HandleWander());
		}
		base.StartCoroutine(this.HandleAmbientChatter());
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x0024A75C File Offset: 0x0024895C
	private IEnumerator HandleWander()
	{
		Vector2 currentTargetPosition = this.m_startPosition.XY() + UnityEngine.Random.insideUnitCircle * this.WanderRadius;
		for (;;)
		{
			if (Vector2.Distance(currentTargetPosition, this.m_transform.position.XY()) < 0.25f)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
				currentTargetPosition = this.m_startPosition.XY() + UnityEngine.Random.insideUnitCircle * this.WanderRadius;
			}
			this.m_transform.position = Vector3.MoveTowards(this.m_transform.position, currentTargetPosition.ToVector3ZUp(this.m_transform.position.z), 3f * BraveTime.DeltaTime);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005F49 RID: 24393 RVA: 0x0024A778 File Offset: 0x00248978
	private IEnumerator HandleAmbientChatter()
	{
		for (;;)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(this.MinTimeBetweenChatter, this.MaxTimeBetweenChatter));
			TextBoxManager.ShowTextBox(this.SpeakPoint.position, base.transform, this.ChatterDuration, StringTableManager.GetString(this.ChatterStringKey), string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}
		yield break;
	}

	// Token: 0x040059AF RID: 22959
	public float MinTimeBetweenChatter = 10f;

	// Token: 0x040059B0 RID: 22960
	public float MaxTimeBetweenChatter = 20f;

	// Token: 0x040059B1 RID: 22961
	public float ChatterDuration = 5f;

	// Token: 0x040059B2 RID: 22962
	public string ChatterStringKey;

	// Token: 0x040059B3 RID: 22963
	public Transform SpeakPoint;

	// Token: 0x040059B4 RID: 22964
	public bool WanderInRadius;

	// Token: 0x040059B5 RID: 22965
	public float WanderRadius = 3f;

	// Token: 0x040059B6 RID: 22966
	private Transform m_transform;

	// Token: 0x040059B7 RID: 22967
	private Vector3 m_startPosition;
}
