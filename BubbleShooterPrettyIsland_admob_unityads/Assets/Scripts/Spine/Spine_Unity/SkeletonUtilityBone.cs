
using System;
using UnityEngine;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/SkeletonUtilityBone")]
	public class SkeletonUtilityBone : MonoBehaviour
	{
		public enum Mode
		{
			Follow,
			Override
		}

		[NonSerialized]
		public bool valid;

		[NonSerialized]
		public SkeletonUtility skeletonUtility;

		[NonSerialized]
		public Bone bone;

		public Mode mode;

		public bool zPosition = true;

		public bool position;

		public bool rotation;

		public bool scale;

		public bool flip;

		public bool flipX;

		[Range(0f, 1f)]
		public float overrideAlpha = 1f;

		public string boneName;

		public Transform parentReference;

		[NonSerialized]
		public bool transformLerpComplete;

		protected Transform cachedTransform;

		protected Transform skeletonTransform;

		private bool disableInheritScaleWarning;

		public bool DisableInheritScaleWarning => disableInheritScaleWarning;

		public void Reset()
		{
			bone = null;
			cachedTransform = base.transform;
			valid = (skeletonUtility != null && skeletonUtility.skeletonRenderer != null && skeletonUtility.skeletonRenderer.valid);
			if (valid)
			{
				skeletonTransform = skeletonUtility.transform;
				skeletonUtility.OnReset -= HandleOnReset;
				skeletonUtility.OnReset += HandleOnReset;
				DoUpdate();
			}
		}

		private void OnEnable()
		{
			skeletonUtility = SkeletonUtility.GetInParent<SkeletonUtility>(base.transform);
			if (!(skeletonUtility == null))
			{
				skeletonUtility.RegisterBone(this);
				skeletonUtility.OnReset += HandleOnReset;
			}
		}

		private void HandleOnReset()
		{
			Reset();
		}

		private void OnDisable()
		{
			if (skeletonUtility != null)
			{
				skeletonUtility.OnReset -= HandleOnReset;
				skeletonUtility.UnregisterBone(this);
			}
		}

		public void DoUpdate()
		{
			if (!valid)
			{
				Reset();
				return;
			}
			Skeleton skeleton = skeletonUtility.skeletonRenderer.skeleton;
			if (bone == null)
			{
				if (boneName == null || boneName.Length == 0)
				{
					return;
				}
				bone = skeleton.FindBone(boneName);
				if (bone == null)
				{
					UnityEngine.Debug.LogError("Bone not found: " + boneName, this);
					return;
				}
			}
			float num = (!(skeleton.flipX ^ skeleton.flipY)) ? 1f : (-1f);
			if (mode == Mode.Follow)
			{
				if (position)
				{
					cachedTransform.localPosition = new Vector3(bone.x, bone.y, 0f);
				}
				if (rotation)
				{
					if (bone.Data.InheritRotation)
					{
						cachedTransform.localRotation = Quaternion.Euler(0f, 0f, bone.AppliedRotation);
					}
					else
					{
						Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
						cachedTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + bone.WorldRotationX * num);
					}
				}
				if (scale)
				{
					cachedTransform.localScale = new Vector3(bone.scaleX, bone.scaleY, bone.WorldSignX);
					disableInheritScaleWarning = !bone.data.inheritScale;
				}
			}
			else
			{
				if (mode != Mode.Override || transformLerpComplete)
				{
					return;
				}
				if (parentReference == null)
				{
					if (position)
					{
						Bone obj = bone;
						float x = bone.x;
						Vector3 localPosition = cachedTransform.localPosition;
						obj.x = Mathf.Lerp(x, localPosition.x, overrideAlpha);
						Bone obj2 = bone;
						float y = bone.y;
						Vector3 localPosition2 = cachedTransform.localPosition;
						obj2.y = Mathf.Lerp(y, localPosition2.y, overrideAlpha);
					}
					if (rotation)
					{
						float a = bone.Rotation;
						Vector3 eulerAngles2 = cachedTransform.localRotation.eulerAngles;
						float appliedRotation = Mathf.LerpAngle(a, eulerAngles2.z, overrideAlpha);
						bone.Rotation = appliedRotation;
						bone.AppliedRotation = appliedRotation;
					}
					if (scale)
					{
						Bone obj3 = bone;
						float scaleX = bone.scaleX;
						Vector3 localScale = cachedTransform.localScale;
						obj3.scaleX = Mathf.Lerp(scaleX, localScale.x, overrideAlpha);
						Bone obj4 = bone;
						float scaleY = bone.scaleY;
						Vector3 localScale2 = cachedTransform.localScale;
						obj4.scaleY = Mathf.Lerp(scaleY, localScale2.y, overrideAlpha);
					}
				}
				else
				{
					if (transformLerpComplete)
					{
						return;
					}
					if (position)
					{
						Vector3 vector = parentReference.InverseTransformPoint(cachedTransform.position);
						bone.x = Mathf.Lerp(bone.x, vector.x, overrideAlpha);
						bone.y = Mathf.Lerp(bone.y, vector.y, overrideAlpha);
					}
					if (rotation)
					{
						float a2 = bone.Rotation;
						Vector3 eulerAngles3 = Quaternion.LookRotation((!flipX) ? Vector3.forward : (Vector3.forward * -1f), parentReference.InverseTransformDirection(cachedTransform.up)).eulerAngles;
						float appliedRotation2 = Mathf.LerpAngle(a2, eulerAngles3.z, overrideAlpha);
						bone.Rotation = appliedRotation2;
						bone.AppliedRotation = appliedRotation2;
					}
					if (scale)
					{
						Bone obj5 = bone;
						float scaleX2 = bone.scaleX;
						Vector3 localScale3 = cachedTransform.localScale;
						obj5.scaleX = Mathf.Lerp(scaleX2, localScale3.x, overrideAlpha);
						Bone obj6 = bone;
						float scaleY2 = bone.scaleY;
						Vector3 localScale4 = cachedTransform.localScale;
						obj6.scaleY = Mathf.Lerp(scaleY2, localScale4.y, overrideAlpha);
					}
					disableInheritScaleWarning = !bone.data.inheritScale;
				}
				transformLerpComplete = true;
			}
		}

		public void AddBoundingBox(string skinName, string slotName, string attachmentName)
		{
			SkeletonUtility.AddBoundingBox(bone.skeleton, skinName, slotName, attachmentName, base.transform);
		}
	}
}
