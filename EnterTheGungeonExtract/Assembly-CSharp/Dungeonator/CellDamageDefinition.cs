using System;

namespace Dungeonator
{
	// Token: 0x02000EAE RID: 3758
	[Serializable]
	public struct CellDamageDefinition
	{
		// Token: 0x06004F88 RID: 20360 RVA: 0x001B9CB0 File Offset: 0x001B7EB0
		public bool HasChanges()
		{
			return this.damageTypes != CoreDamageTypes.None || this.damageToPlayersPerTick != 0f || this.damageToEnemiesPerTick != 0f || this.tickFrequency != 0f || this.respectsFlying || this.isPoison;
		}

		// Token: 0x0400472E RID: 18222
		public CoreDamageTypes damageTypes;

		// Token: 0x0400472F RID: 18223
		public float damageToPlayersPerTick;

		// Token: 0x04004730 RID: 18224
		public float damageToEnemiesPerTick;

		// Token: 0x04004731 RID: 18225
		public float tickFrequency;

		// Token: 0x04004732 RID: 18226
		public bool respectsFlying;

		// Token: 0x04004733 RID: 18227
		public bool isPoison;
	}
}
