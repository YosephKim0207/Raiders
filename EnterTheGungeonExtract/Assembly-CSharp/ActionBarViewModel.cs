using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[AddComponentMenu("Daikon Forge/Examples/Actionbar/View Model")]
public class ActionBarViewModel : MonoBehaviour
{
	// Token: 0x14000054 RID: 84
	// (add) Token: 0x0600187E RID: 6270 RVA: 0x00073E88 File Offset: 0x00072088
	// (remove) Token: 0x0600187F RID: 6271 RVA: 0x00073EC0 File Offset: 0x000720C0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ActionBarViewModel.SpellEventHandler SpellActivated;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06001880 RID: 6272 RVA: 0x00073EF8 File Offset: 0x000720F8
	// (remove) Token: 0x06001881 RID: 6273 RVA: 0x00073F30 File Offset: 0x00072130
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ActionBarViewModel.SpellEventHandler SpellDeactivated;

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06001882 RID: 6274 RVA: 0x00073F68 File Offset: 0x00072168
	public int MaxHealth
	{
		get
		{
			return this._maxHealth;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06001883 RID: 6275 RVA: 0x00073F70 File Offset: 0x00072170
	public int MaxEnergy
	{
		get
		{
			return this._maxEnergy;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06001884 RID: 6276 RVA: 0x00073F78 File Offset: 0x00072178
	// (set) Token: 0x06001885 RID: 6277 RVA: 0x00073F84 File Offset: 0x00072184
	public int Health
	{
		get
		{
			return (int)this._health;
		}
		private set
		{
			this._health = (float)Mathf.Max(0, Mathf.Min(this._maxHealth, value));
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06001886 RID: 6278 RVA: 0x00073FA0 File Offset: 0x000721A0
	// (set) Token: 0x06001887 RID: 6279 RVA: 0x00073FAC File Offset: 0x000721AC
	public int Energy
	{
		get
		{
			return (int)this._energy;
		}
		private set
		{
			this._energy = (float)Mathf.Max(0, Mathf.Min(this._maxEnergy, value));
		}
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x00073FC8 File Offset: 0x000721C8
	private void OnEnable()
	{
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x00073FCC File Offset: 0x000721CC
	private void Start()
	{
		this._health = 35f;
		this._energy = 50f;
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x00073FE4 File Offset: 0x000721E4
	private void Update()
	{
		this._health = Mathf.Min((float)this._maxHealth, this._health + BraveTime.DeltaTime * this._healthRegenRate);
		this._energy = Mathf.Min((float)this._maxEnergy, this._energy + BraveTime.DeltaTime * this._energyRegenRate);
		for (int i = this.activeSpells.Count - 1; i >= 0; i--)
		{
			ActionBarViewModel.SpellCastInfo spellCastInfo = this.activeSpells[i];
			float num = Time.realtimeSinceStartup - spellCastInfo.whenCast;
			if (spellCastInfo.spell.Recharge <= num)
			{
				this.activeSpells.RemoveAt(i);
				if (this.SpellDeactivated != null)
				{
					this.SpellDeactivated(spellCastInfo.spell);
				}
			}
		}
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x000740AC File Offset: 0x000722AC
	public void CastSpell(string spellName)
	{
		SpellDefinition spell = SpellDefinition.FindByName(spellName);
		if (spell == null)
		{
			throw new InvalidCastException();
		}
		if (this.activeSpells.Any((ActionBarViewModel.SpellCastInfo activeSpell) => activeSpell.spell == spell))
		{
			return;
		}
		if (this.Energy < spell.Cost)
		{
			return;
		}
		this.Energy -= spell.Cost;
		this.activeSpells.Add(new ActionBarViewModel.SpellCastInfo
		{
			spell = spell,
			whenCast = Time.realtimeSinceStartup
		});
		if (this.SpellActivated != null)
		{
			this.SpellActivated(spell);
		}
	}

	// Token: 0x04001371 RID: 4977
	[SerializeField]
	private float _health;

	// Token: 0x04001372 RID: 4978
	[SerializeField]
	private int _maxHealth = 100;

	// Token: 0x04001373 RID: 4979
	[SerializeField]
	private float _healthRegenRate = 0.5f;

	// Token: 0x04001374 RID: 4980
	[SerializeField]
	private float _energy;

	// Token: 0x04001375 RID: 4981
	[SerializeField]
	private int _maxEnergy = 100;

	// Token: 0x04001376 RID: 4982
	[SerializeField]
	private float _energyRegenRate = 1f;

	// Token: 0x04001377 RID: 4983
	private List<ActionBarViewModel.SpellCastInfo> activeSpells = new List<ActionBarViewModel.SpellCastInfo>();

	// Token: 0x0200042D RID: 1069
	// (Invoke) Token: 0x0600188D RID: 6285
	public delegate void SpellEventHandler(SpellDefinition spell);

	// Token: 0x0200042E RID: 1070
	private class SpellCastInfo
	{
		// Token: 0x04001378 RID: 4984
		public SpellDefinition spell;

		// Token: 0x04001379 RID: 4985
		public float whenCast;
	}
}
