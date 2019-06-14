using System;
using System.Collections.Generic;

using Box2DNet.Common;

using Tynted.Util;
using Math = System.Math;

namespace Tynted.Components
{
	public class Transform : IComponent
	{
		public bool Enabled { get; set; }

		private Transform parent;
		private List<Transform> children;

		private Vec2 localPosition = Vec2.Zero;
		private float localRotation = 0;
		private Vec2 localScale = Vec2.One;

		private bool isDirty = false;

		private Matrix localToWorldMatrix = Matrix.Identity;
		private Tuple<Matrix, Matrix> localToWorldMatrixSeperate = new Tuple<Matrix, Matrix>(Matrix.Identity, Matrix.Identity);

		private bool isInverseDirty = false;

		private Matrix worldToLocalMatrix = Matrix.Identity;

        public IComponent Clone
        {
            get
            {
                Transform newTransform = new Transform(localPosition, localRotation, localScale);

                newTransform.Enabled = Enabled;
                newTransform.parent = parent;
                newTransform.children = new List<Transform>(children);
                newTransform.isDirty = isDirty;
                newTransform.localToWorldMatrix = localToWorldMatrix;
                newTransform.localToWorldMatrixSeperate = localToWorldMatrixSeperate;
                newTransform.isInverseDirty = isInverseDirty;
                newTransform.worldToLocalMatrix = worldToLocalMatrix;

                return newTransform;
            }
        }

		public Transform(Vec2 position, float rotation = 0, Vec2 scale = default)
		{
			parent = null;
			children = new List<Transform>();

			this.LocalPosition = position;
			this.LocalRotation = rotation;

			if (scale == default)
				this.LocalScale = Vec2.One;
			else
				this.LocalScale = scale;

			Enabled = true;
		}

		public Transform(Vec2 position, float rotation = 0, Vec2 scale = default, Transform parent = null)
		{
			this.parent = parent;
			children = new List<Transform>();

			this.LocalPosition = position;
			this.LocalRotation = rotation;

			if (scale == default)
				this.LocalScale = Vec2.One;
			else
				this.LocalScale = scale;

			Enabled = true;
		}

		private void SetDirty()
		{
			if (!isDirty)
			{
				isDirty = true;
				isInverseDirty = true;

				foreach (Transform child in children)
				{
					child.SetDirty();
				}
			}
		}

		public void Setparent(Transform value)
		{
			if (parent != null)
			{
				parent.children.Remove(this);
			}

			parent = value;

			if (parent != null)
			{
				parent.children.Add(this);
			}

			SetDirty();
		}

		public Transform GetParent()
		{
			return parent;
		}

		public Matrix CalculateLocalToParentMatrix()
		{
			return new Matrix(localScale.X, localScale.Y, localPosition.X, localPosition.Y) *
				new Matrix(localRotation);
		}

		public Matrix GetLocalToWorldMatrix()
		{
			if (isDirty)
			{
				if (parent == null)
				{
					localToWorldMatrix = CalculateLocalToParentMatrix();
				}
				else
				{
					localToWorldMatrix = parent.GetLocalToWorldMatrix() * CalculateLocalToParentMatrix();
				}

				isDirty = false;
			}

			return localToWorldMatrix;
		}

		private Tuple<Matrix, Matrix> CalculateLocalToParentMatrixSeperate()
		{
			return new Tuple<Matrix, Matrix>(new Matrix(localScale.X, localScale.Y, localPosition.X, localPosition.Y),
				new Matrix(localRotation));
		}

		private Tuple<Matrix, Matrix> GetLocalToWorldMatrixSeperate()
		{
			if (isDirty)
			{
				if (parent == null)
				{
					localToWorldMatrixSeperate = new Tuple<Matrix, Matrix>(
						CalculateLocalToParentMatrixSeperate().Item1, CalculateLocalToParentMatrixSeperate().Item2);
				}
				else
				{
					localToWorldMatrixSeperate = new Tuple<Matrix, Matrix>(
						parent.GetLocalToWorldMatrixSeperate().Item1 * CalculateLocalToParentMatrixSeperate().Item1, 
						parent.GetLocalToWorldMatrixSeperate().Item2 * CalculateLocalToParentMatrixSeperate().Item2);
				}

				isDirty = false;
			}

			return localToWorldMatrixSeperate;
		}

		public Vec2 GetWorldPosition()
		{
			//Matrix transformMatrix = GetLocalToWorldMatrixSeperate().Item1;
			//return new Vec2(transformMatrix.m13, transformMatrix.m23);

            return TransformPoint(new Vec2(0, 0));
		}

		//public float GetWorldRotation()
		//{
		//	Matrix transformMatrix = GetLocalToWorldMatrixSeperate().Item2;
		//	return (float)Math.Atan2(transformMatrix.m21, transformMatrix.m11);
		//}

		//public Vec2 GetWorldScale()
		//{
		//	Matrix transformMatrix = GetLocalToWorldMatrixSeperate().Item1;
		//	return new Vec2(transformMatrix.m11, transformMatrix.m22);
		//}

		private Matrix GetWorldToLocalMatrix()
		{
			if (isInverseDirty)
			{
				worldToLocalMatrix = GetLocalToWorldMatrix().Inverse();

				isInverseDirty = false;
			}

			return worldToLocalMatrix;
		}

		public Vec2 TransformPoint(Vec2 point)
		{
			return GetLocalToWorldMatrix() * new Vec3(point.X, point.Y, 1);
		}

		public Vec2 TransformDirection(Vec2 dir)
		{
			return new Vec3(dir.X, dir.Y, 0) * GetWorldToLocalMatrix();
		}

		public Vec2 InverseTransformPoint(Vec2 point)
		{
			return GetWorldToLocalMatrix() * new Vec3(point.X, point.Y, 1);
		}

		public Vec2 InverseTransformDirection(Vec2 dir)
		{
			return new Vec3(dir.X, dir.Y, 0) * GetLocalToWorldMatrix();
		}

		public Vec2 LocalPosition
		{
			get
			{
				return localPosition;
			}
			set
			{
				localPosition = value;
				SetDirty();
			}
		}

		public float LocalRotation
		{
			get
			{
				return localRotation;
			}
			set
			{
				localRotation = value;
				SetDirty();
			}
		}

		public Vec2 LocalScale
		{
			get
			{
				return localScale;
			}
			set
			{
				localScale = value;
				SetDirty();
			}
		}
	}
}
