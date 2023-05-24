using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200044A RID: 1098
[AddComponentMenu("Daikon Forge/Examples/Data Binding/Expression Binding Model")]
public class ExpressionBindingDemoModel : MonoBehaviour
{
	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x0600194A RID: 6474 RVA: 0x00076EF0 File Offset: 0x000750F0
	// (set) Token: 0x0600194B RID: 6475 RVA: 0x00076EF8 File Offset: 0x000750F8
	public List<string> SpellsLearned { get; set; }

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x0600194C RID: 6476 RVA: 0x00076F04 File Offset: 0x00075104
	public SpellDefinition SelectedSpell
	{
		get
		{
			return SpellDefinition.FindByName(this.SpellsLearned[this.list.SelectedIndex]);
		}
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x00076F24 File Offset: 0x00075124
	private void Awake()
	{
		this.list = base.GetComponentInChildren<dfListbox>();
		this.list.SelectedIndex = 0;
		this.SpellsLearned = (from x in SpellDefinition.AllSpells
			orderby x.Name
			select x.Name).ToList<string>();
	}

	// Token: 0x040013DD RID: 5085
	private dfListbox list;
}
