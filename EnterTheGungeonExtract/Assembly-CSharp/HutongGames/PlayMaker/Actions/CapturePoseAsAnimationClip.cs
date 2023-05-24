using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090A RID: 2314
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Captures the current pose of a hierarchy as an animation clip.\n\nUseful to blend from an arbitrary pose (e.g. a ragdoll death) back to a known animation (e.g. idle).")]
	public class CapturePoseAsAnimationClip : FsmStateAction
	{
		// Token: 0x060032F1 RID: 13041 RVA: 0x0010B748 File Offset: 0x00109948
		public override void Reset()
		{
			this.gameObject = null;
			this.position = false;
			this.rotation = true;
			this.scale = false;
			this.storeAnimationClip = null;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0010B77C File Offset: 0x0010997C
		public override void OnEnter()
		{
			this.DoCaptureAnimationClip();
			base.Finish();
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x0010B78C File Offset: 0x0010998C
		private void DoCaptureAnimationClip()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			AnimationClip animationClip = new AnimationClip();
			IEnumerator enumerator = ownerDefaultTarget.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					this.CaptureTransform(transform, string.Empty, animationClip);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			this.storeAnimationClip.Value = animationClip;
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x0010B82C File Offset: 0x00109A2C
		private void CaptureTransform(Transform transform, string path, AnimationClip clip)
		{
			path += transform.name;
			if (this.position.Value)
			{
				this.CapturePosition(transform, path, clip);
			}
			if (this.rotation.Value)
			{
				this.CaptureRotation(transform, path, clip);
			}
			if (this.scale.Value)
			{
				this.CaptureScale(transform, path, clip);
			}
			IEnumerator enumerator = transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform2 = (Transform)obj;
					this.CaptureTransform(transform2, path + "/", clip);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0010B8F4 File Offset: 0x00109AF4
		private void CapturePosition(Transform transform, string path, AnimationClip clip)
		{
			this.SetConstantCurve(clip, path, "localPosition.x", transform.localPosition.x);
			this.SetConstantCurve(clip, path, "localPosition.y", transform.localPosition.y);
			this.SetConstantCurve(clip, path, "localPosition.z", transform.localPosition.z);
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x0010B954 File Offset: 0x00109B54
		private void CaptureRotation(Transform transform, string path, AnimationClip clip)
		{
			this.SetConstantCurve(clip, path, "localRotation.x", transform.localRotation.x);
			this.SetConstantCurve(clip, path, "localRotation.y", transform.localRotation.y);
			this.SetConstantCurve(clip, path, "localRotation.z", transform.localRotation.z);
			this.SetConstantCurve(clip, path, "localRotation.w", transform.localRotation.w);
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x0010B9D0 File Offset: 0x00109BD0
		private void CaptureScale(Transform transform, string path, AnimationClip clip)
		{
			this.SetConstantCurve(clip, path, "localScale.x", transform.localScale.x);
			this.SetConstantCurve(clip, path, "localScale.y", transform.localScale.y);
			this.SetConstantCurve(clip, path, "localScale.z", transform.localScale.z);
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x0010BA30 File Offset: 0x00109C30
		private void SetConstantCurve(AnimationClip clip, string childPath, string propertyPath, float value)
		{
			AnimationCurve animationCurve = AnimationCurve.Linear(0f, value, 100f, value);
			animationCurve.postWrapMode = WrapMode.Loop;
			clip.SetCurve(childPath, typeof(Transform), propertyPath, animationCurve);
		}

		// Token: 0x0400242A RID: 9258
		[Tooltip("The GameObject root of the hierarchy to capture.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400242B RID: 9259
		[Tooltip("Capture position keys.")]
		public FsmBool position;

		// Token: 0x0400242C RID: 9260
		[Tooltip("Capture rotation keys.")]
		public FsmBool rotation;

		// Token: 0x0400242D RID: 9261
		[Tooltip("Capture scale keys.")]
		public FsmBool scale;

		// Token: 0x0400242E RID: 9262
		[ObjectType(typeof(AnimationClip))]
		[Tooltip("Store the result in an Object variable of type AnimationClip.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject storeAnimationClip;
	}
}
