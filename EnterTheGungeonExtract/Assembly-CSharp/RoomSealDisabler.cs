using System;
using Dungeonator;

// Token: 0x020011FD RID: 4605
public class RoomSealDisabler : BraveBehaviour
{
	// Token: 0x060066DA RID: 26330 RVA: 0x00281144 File Offset: 0x0027F344
	private void Start()
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		RoomHandler roomHandler = absoluteRoom;
		roomHandler.OnSealChanged = (Action<bool>)Delegate.Combine(roomHandler.OnSealChanged, new Action<bool>(this.HandleSealStateChanged));
		this.HandleSealStateChanged(false);
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x0028118C File Offset: 0x0027F38C
	private void HandleSealStateChanged(bool isSealed)
	{
		if (this.MatchRoomState)
		{
			if (base.specRigidbody)
			{
				base.specRigidbody.enabled = isSealed;
			}
			base.gameObject.SetActive(isSealed);
		}
		else
		{
			if (base.specRigidbody)
			{
				base.specRigidbody.enabled = !isSealed;
			}
			base.gameObject.SetActive(!isSealed);
		}
	}

	// Token: 0x040062B7 RID: 25271
	public bool MatchRoomState = true;
}
