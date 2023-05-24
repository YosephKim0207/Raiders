using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200118B RID: 4491
public class HellDraggerArbitrary : BraveBehaviour
{
	// Token: 0x060063D8 RID: 25560 RVA: 0x0026C07C File Offset: 0x0026A27C
	private IEnumerator HandleGrabbyGrab(PlayerController grabbedPlayer)
	{
		yield return new WaitForSeconds(0.5f);
		grabbedPlayer.IsVisible = false;
		yield break;
	}

	// Token: 0x060063D9 RID: 25561 RVA: 0x0026C098 File Offset: 0x0026A298
	private void GrabPlayer(PlayerController enteredPlayer)
	{
		GameObject gameObject = enteredPlayer.PlayEffectOnActor(this.HellDragVFX, new Vector3(0f, -0.25f, 0f), false, false, false);
		gameObject.transform.position = new Vector3((float)Mathf.RoundToInt(gameObject.transform.position.x + 0.1875f) - 0.1875f, (float)Mathf.RoundToInt(gameObject.transform.position.y - 0.375f) + 0.375f, gameObject.transform.position.z);
		base.StartCoroutine(this.HandleGrabbyGrab(enteredPlayer));
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x0026C144 File Offset: 0x0026A344
	public void Do(PlayerController enteredPlayer)
	{
		if (enteredPlayer)
		{
			this.GrabPlayer(enteredPlayer);
		}
	}

	// Token: 0x04005F6F RID: 24431
	public GameObject HellDragVFX;
}
