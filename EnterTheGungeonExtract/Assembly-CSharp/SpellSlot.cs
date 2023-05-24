using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x02000438 RID: 1080
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Spell Slot")]
[ExecuteInEditMode]
public class SpellSlot : MonoBehaviour
{
	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x060018BA RID: 6330 RVA: 0x00074C50 File Offset: 0x00072E50
	// (set) Token: 0x060018BB RID: 6331 RVA: 0x00074C58 File Offset: 0x00072E58
	public bool IsActionSlot
	{
		get
		{
			return this.isActionSlot;
		}
		set
		{
			this.isActionSlot = value;
			this.refresh();
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x060018BC RID: 6332 RVA: 0x00074C68 File Offset: 0x00072E68
	// (set) Token: 0x060018BD RID: 6333 RVA: 0x00074C70 File Offset: 0x00072E70
	public string Spell
	{
		get
		{
			return this.spellName;
		}
		set
		{
			this.spellName = value;
			this.refresh();
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x060018BE RID: 6334 RVA: 0x00074C80 File Offset: 0x00072E80
	// (set) Token: 0x060018BF RID: 6335 RVA: 0x00074C88 File Offset: 0x00072E88
	public int SlotNumber
	{
		get
		{
			return this.slotNumber;
		}
		set
		{
			this.slotNumber = value;
			this.refresh();
		}
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x00074C98 File Offset: 0x00072E98
	private void OnEnable()
	{
		this.refresh();
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x00074CA0 File Offset: 0x00072EA0
	private void Start()
	{
		this.refresh();
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x00074CA8 File Offset: 0x00072EA8
	private void Update()
	{
		if (this.IsActionSlot && !string.IsNullOrEmpty(this.Spell) && Input.GetKeyDown(this.slotNumber + KeyCode.Alpha0))
		{
			this.castSpell();
		}
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00074CE0 File Offset: 0x00072EE0
	public void onSpellActivated(SpellDefinition spell)
	{
		if (spell.Name != this.Spell)
		{
			return;
		}
		base.StartCoroutine(this.showCooldown());
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x00074D08 File Offset: 0x00072F08
	private void OnDoubleClick()
	{
		if (!this.isSpellActive && !string.IsNullOrEmpty(this.Spell))
		{
			this.castSpell();
		}
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x00074D2C File Offset: 0x00072F2C
	private void OnDragStart(dfControl source, dfDragEventArgs args)
	{
		if (this.allowDrag(args))
		{
			if (string.IsNullOrEmpty(this.Spell))
			{
				args.State = dfDragDropState.Denied;
			}
			else
			{
				dfSprite dfSprite = base.GetComponent<dfControl>().Find("Icon") as dfSprite;
				Ray ray = dfSprite.GetCamera().ScreenPointToRay(Input.mousePosition);
				Vector2 zero = Vector2.zero;
				if (!dfSprite.GetHitPosition(ray, out zero))
				{
					return;
				}
				ActionbarsDragCursor.Show(dfSprite, Input.mousePosition, zero);
				if (this.IsActionSlot)
				{
					dfSprite.SpriteName = string.Empty;
				}
				args.State = dfDragDropState.Dragging;
				args.Data = this;
			}
			args.Use();
		}
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x00074DD8 File Offset: 0x00072FD8
	private void OnDragEnd(dfControl source, dfDragEventArgs args)
	{
		ActionbarsDragCursor.Hide();
		if (!this.isActionSlot)
		{
			return;
		}
		if (args.State == dfDragDropState.CancelledNoTarget)
		{
			this.Spell = string.Empty;
		}
		this.refresh();
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x00074E08 File Offset: 0x00073008
	private void OnDragDrop(dfControl source, dfDragEventArgs args)
	{
		if (this.allowDrop(args))
		{
			args.State = dfDragDropState.Dropped;
			SpellSlot spellSlot = args.Data as SpellSlot;
			string text = this.spellName;
			this.Spell = spellSlot.Spell;
			if (spellSlot.IsActionSlot)
			{
				spellSlot.Spell = text;
			}
		}
		else
		{
			args.State = dfDragDropState.Denied;
		}
		args.Use();
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x00074E6C File Offset: 0x0007306C
	private bool allowDrag(dfDragEventArgs args)
	{
		return !this.isSpellActive && !string.IsNullOrEmpty(this.spellName);
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x00074E8C File Offset: 0x0007308C
	private bool allowDrop(dfDragEventArgs args)
	{
		if (this.isSpellActive)
		{
			return false;
		}
		SpellSlot spellSlot = args.Data as SpellSlot;
		return spellSlot != null && this.IsActionSlot;
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x00074EC8 File Offset: 0x000730C8
	private IEnumerator showCooldown()
	{
		this.isSpellActive = true;
		SpellDefinition assignedSpell = SpellDefinition.FindByName(this.Spell);
		dfSprite sprite = base.GetComponent<dfControl>().Find("CoolDown") as dfSprite;
		sprite.IsVisible = true;
		float startTime = Time.realtimeSinceStartup;
		float endTime = startTime + assignedSpell.Recharge;
		while (Time.realtimeSinceStartup < endTime)
		{
			float elapsed = Time.realtimeSinceStartup - startTime;
			float lerp = 1f - elapsed / assignedSpell.Recharge;
			sprite.FillAmount = lerp;
			yield return null;
		}
		sprite.FillAmount = 1f;
		sprite.IsVisible = false;
		this.isSpellActive = false;
		yield break;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x00074EE4 File Offset: 0x000730E4
	private void castSpell()
	{
		ActionBarViewModel actionBarViewModel = UnityEngine.Object.FindObjectsOfType(typeof(ActionBarViewModel)).FirstOrDefault<UnityEngine.Object>() as ActionBarViewModel;
		if (actionBarViewModel != null)
		{
			actionBarViewModel.CastSpell(this.Spell);
		}
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x00074F24 File Offset: 0x00073124
	private void refresh()
	{
		SpellDefinition spellDefinition = SpellDefinition.FindByName(this.Spell);
		dfSprite dfSprite = base.GetComponent<dfControl>().Find<dfSprite>("Icon");
		dfSprite.SpriteName = ((spellDefinition == null) ? string.Empty : spellDefinition.Icon);
		dfButton componentInChildren = base.GetComponentInChildren<dfButton>();
		componentInChildren.IsVisible = this.IsActionSlot;
		componentInChildren.Text = this.slotNumber.ToString();
	}

	// Token: 0x0400139A RID: 5018
	[SerializeField]
	protected string spellName = string.Empty;

	// Token: 0x0400139B RID: 5019
	[SerializeField]
	protected int slotNumber;

	// Token: 0x0400139C RID: 5020
	[SerializeField]
	protected bool isActionSlot;

	// Token: 0x0400139D RID: 5021
	private bool isSpellActive;
}
