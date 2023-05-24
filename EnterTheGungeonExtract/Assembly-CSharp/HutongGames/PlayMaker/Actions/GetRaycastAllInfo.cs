using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A6 RID: 2470
	[Tooltip("Gets info on the last RaycastAll and store in array variables.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetRaycastAllInfo : FsmStateAction
	{
		// Token: 0x0600358B RID: 13707 RVA: 0x00113704 File Offset: 0x00111904
		public override void Reset()
		{
			this.storeHitObjects = null;
			this.points = null;
			this.normals = null;
			this.distances = null;
			this.everyFrame = false;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x0011372C File Offset: 0x0011192C
		private void StoreRaycastAllInfo()
		{
			if (RaycastAll.RaycastAllHitInfo == null)
			{
				return;
			}
			this.storeHitObjects.Resize(RaycastAll.RaycastAllHitInfo.Length);
			this.points.Resize(RaycastAll.RaycastAllHitInfo.Length);
			this.normals.Resize(RaycastAll.RaycastAllHitInfo.Length);
			this.distances.Resize(RaycastAll.RaycastAllHitInfo.Length);
			for (int i = 0; i < RaycastAll.RaycastAllHitInfo.Length; i++)
			{
				this.storeHitObjects.Values[i] = RaycastAll.RaycastAllHitInfo[i].collider.gameObject;
				this.points.Values[i] = RaycastAll.RaycastAllHitInfo[i].point;
				this.normals.Values[i] = RaycastAll.RaycastAllHitInfo[i].normal;
				this.distances.Values[i] = RaycastAll.RaycastAllHitInfo[i].distance;
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x0011382C File Offset: 0x00111A2C
		public override void OnEnter()
		{
			this.StoreRaycastAllInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00113848 File Offset: 0x00111A48
		public override void OnUpdate()
		{
			this.StoreRaycastAllInfo();
		}

		// Token: 0x040026D7 RID: 9943
		[Tooltip("Store the GameObjects hit in an array variable.")]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[UIHint(UIHint.Variable)]
		public FsmArray storeHitObjects;

		// Token: 0x040026D8 RID: 9944
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Vector3, "", 0, 0, 65536)]
		[Tooltip("Get the world position of all ray hit point and store them in an array variable.")]
		public FsmArray points;

		// Token: 0x040026D9 RID: 9945
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the normal at all hit points and store them in an array variable.")]
		[ArrayEditor(VariableType.Vector3, "", 0, 0, 65536)]
		public FsmArray normals;

		// Token: 0x040026DA RID: 9946
		[Tooltip("Get the distance along the ray to all hit points and store tjem in an array variable.")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Float, "", 0, 0, 65536)]
		public FsmArray distances;

		// Token: 0x040026DB RID: 9947
		[Tooltip("Repeat every frame. Warning, this could be affecting performances")]
		public bool everyFrame;
	}
}
