using System;
using System.Collections;

// Token: 0x02001167 RID: 4455
public class FoyerCoatRackController : BraveBehaviour
{
	// Token: 0x060062F6 RID: 25334 RVA: 0x00265FA0 File Offset: 0x002641A0
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		base.GetComponent<tk2dBaseSprite>().HeightOffGround = -1f;
		base.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		if (!false)
		{
			base.specRigidbody.enabled = false;
			base.gameObject.SetActive(false);
		}
		yield break;
	}
}
