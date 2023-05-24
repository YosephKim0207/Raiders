using System;
using UnityEngine;

// Token: 0x02000437 RID: 1079
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Spell Inventory")]
public class SpellInventory : MonoBehaviour
{
	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x060018B2 RID: 6322 RVA: 0x00074A04 File Offset: 0x00072C04
	// (set) Token: 0x060018B3 RID: 6323 RVA: 0x00074A0C File Offset: 0x00072C0C
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

	// Token: 0x060018B4 RID: 6324 RVA: 0x00074A1C File Offset: 0x00072C1C
	private void OnEnable()
	{
		this.refresh();
		dfControl component = base.gameObject.GetComponent<dfControl>();
		component.SizeChanged += delegate(dfControl source, Vector2 value)
		{
			this.needRefresh = true;
		};
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x00074A50 File Offset: 0x00072C50
	private void LateUpdate()
	{
		if (this.needRefresh)
		{
			this.needRefresh = false;
			this.refresh();
		}
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x00074A6C File Offset: 0x00072C6C
	public void OnResolutionChanged()
	{
		this.needRefresh = true;
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00074A78 File Offset: 0x00072C78
	private void refresh()
	{
		dfControl component = base.gameObject.GetComponent<dfControl>();
		dfScrollPanel dfScrollPanel = component.Parent as dfScrollPanel;
		if (dfScrollPanel != null)
		{
			component.Width = dfScrollPanel.Width - (float)dfScrollPanel.ScrollPadding.horizontal;
		}
		SpellSlot componentInChildren = component.GetComponentInChildren<SpellSlot>();
		dfLabel dfLabel = component.Find<dfLabel>("lblCosts");
		dfLabel dfLabel2 = component.Find<dfLabel>("lblName");
		dfLabel dfLabel3 = component.Find<dfLabel>("lblDescription");
		if (dfLabel == null)
		{
			throw new Exception("Not found: lblCosts");
		}
		if (dfLabel2 == null)
		{
			throw new Exception("Not found: lblName");
		}
		if (dfLabel3 == null)
		{
			throw new Exception("Not found: lblDescription");
		}
		SpellDefinition spellDefinition = SpellDefinition.FindByName(this.Spell);
		if (spellDefinition == null)
		{
			componentInChildren.Spell = string.Empty;
			dfLabel.Text = string.Empty;
			dfLabel2.Text = string.Empty;
			dfLabel3.Text = string.Empty;
			return;
		}
		componentInChildren.Spell = this.spellName;
		dfLabel2.Text = spellDefinition.Name;
		dfLabel.Text = string.Format("{0}/{1}/{2}", spellDefinition.Cost, spellDefinition.Recharge, spellDefinition.Delay);
		dfLabel3.Text = spellDefinition.Description;
		float num = dfLabel3.RelativePosition.y + dfLabel3.Size.y;
		float num2 = dfLabel.RelativePosition.y + dfLabel.Size.y;
		component.Height = Mathf.Max(num, num2) + 5f;
	}

	// Token: 0x04001398 RID: 5016
	[SerializeField]
	protected string spellName = string.Empty;

	// Token: 0x04001399 RID: 5017
	private bool needRefresh = true;
}
