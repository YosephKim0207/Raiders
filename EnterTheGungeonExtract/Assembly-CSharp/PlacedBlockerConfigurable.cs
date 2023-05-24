using System;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;

// Token: 0x020011D8 RID: 4568
public class PlacedBlockerConfigurable : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x060065FE RID: 26110 RVA: 0x0027A338 File Offset: 0x00278538
	public bool ShowSpecifiedPixelCollider()
	{
		return this.colliderSelection == PlacedBlockerConfigurable.ColliderSelection.Single && this.SpecifyPixelCollider;
	}

	// Token: 0x060065FF RID: 26111 RVA: 0x0027A350 File Offset: 0x00278550
	public void Start()
	{
	}

	// Token: 0x06006600 RID: 26112 RVA: 0x0027A354 File Offset: 0x00278554
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.Initialize(room);
	}

	// Token: 0x06006601 RID: 26113 RVA: 0x0027A360 File Offset: 0x00278560
	protected override void OnDestroy()
	{
		if (GameManager.HasInstance && Pathfinder.HasInstance && base.specRigidbody && this.m_allOccupiedCells != null)
		{
			for (int i = 0; i < this.m_allOccupiedCells.Count; i++)
			{
				OccupiedCells occupiedCells = this.m_allOccupiedCells[i];
				if (occupiedCells != null)
				{
					occupiedCells.Clear();
				}
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06006602 RID: 26114 RVA: 0x0027A3D8 File Offset: 0x002785D8
	private void Initialize(RoomHandler room)
	{
		if (this.m_initialized)
		{
			return;
		}
		if (base.specRigidbody)
		{
			base.specRigidbody.Initialize();
			if (this.colliderSelection == PlacedBlockerConfigurable.ColliderSelection.All)
			{
				this.m_allOccupiedCells = new List<OccupiedCells>(base.specRigidbody.PixelColliders.Count);
				for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
				{
					this.m_allOccupiedCells.Add(new OccupiedCells(base.specRigidbody, base.specRigidbody.PixelColliders[i], room));
				}
			}
			else
			{
				this.m_allOccupiedCells = new List<OccupiedCells>(1);
				if (this.SpecifyPixelCollider)
				{
					this.m_allOccupiedCells.Add(new OccupiedCells(base.specRigidbody, base.specRigidbody.PixelColliders[this.SpecifiedPixelCollider], room));
				}
				else
				{
					this.m_allOccupiedCells.Add(new OccupiedCells(base.specRigidbody, room));
				}
			}
		}
		this.m_initialized = true;
	}

	// Token: 0x040061BF RID: 25023
	public PlacedBlockerConfigurable.ColliderSelection colliderSelection;

	// Token: 0x040061C0 RID: 25024
	[ShowInInspectorIf("colliderSelection", 0, false)]
	public bool SpecifyPixelCollider;

	// Token: 0x040061C1 RID: 25025
	[ShowInInspectorIf("SpecifyPixelCollider", false)]
	public int SpecifiedPixelCollider;

	// Token: 0x040061C2 RID: 25026
	private bool m_initialized;

	// Token: 0x040061C3 RID: 25027
	private List<OccupiedCells> m_allOccupiedCells;

	// Token: 0x020011D9 RID: 4569
	public enum ColliderSelection
	{
		// Token: 0x040061C5 RID: 25029
		Single,
		// Token: 0x040061C6 RID: 25030
		All
	}
}
