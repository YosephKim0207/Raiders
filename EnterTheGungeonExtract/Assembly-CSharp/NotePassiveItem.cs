using System;

// Token: 0x02001444 RID: 5188
public class NotePassiveItem : PassiveItem
{
	// Token: 0x060075CA RID: 30154 RVA: 0x002EE708 File Offset: 0x002EC908
	private void Awake()
	{
		if (this.ResourcefulRatNoteIdentifier >= 0)
		{
			string appropriateSpriteName = this.GetAppropriateSpriteName(false);
			if (!string.IsNullOrEmpty(appropriateSpriteName))
			{
				base.sprite.SetSprite(appropriateSpriteName);
			}
		}
	}

	// Token: 0x060075CB RID: 30155 RVA: 0x002EE744 File Offset: 0x002EC944
	public string GetAppropriateSpriteName(bool isAmmonomicon)
	{
		return (!isAmmonomicon) ? "resourcefulrat_note_base" : "resourcefulrat_note_base_001";
	}

	// Token: 0x060075CC RID: 30156 RVA: 0x002EE768 File Offset: 0x002EC968
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400778F RID: 30607
	public int ResourcefulRatNoteIdentifier = -1;
}
