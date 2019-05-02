using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using Box2DNet.Common;
using Math = System.Math;

namespace Tynted.Util
{
    public struct Matrix
    {
        // Properties
		public float m11 { get; set; }
		public float m12 { get; set; }
		public float m13 { get; set; }
		public float m21 { get; set; }
		public float m22 { get; set; }
		public float m23 { get; set; }
		public float m31 { get; set; }
		public float m32 { get; set; }
		public float m33 { get; set; }

		/// <summary>
		/// The identity matrix;
		/// </summary>
		public static readonly Matrix Identity = new Matrix(1, 0, 0, 0, 1, 0, 0, 0, 1);

		public Matrix(
			float m11, float m12, float m13, 
			float m21, float m22, float m23, 
			float m31, float m32, float m33)
		{
			this.m11 = m11;
			this.m12 = m12;
			this.m13 = m13;
			this.m21 = m21;
			this.m22 = m22;
			this.m23 = m23;
			this.m31 = m31;
			this.m32 = m32;
			this.m33 = m33;
		}

		#region Additional Constructors

		// 2.b - Create from angle
		public Matrix(float angle)
		{
			m11 = (float)Math.Cos(angle); m12 = (float)-Math.Sin(angle); m13 = 0;
			m21 = (float)Math.Sin(angle); m22 = (float)Math.Cos(angle); m23 = 0;
			m31 = 0; m32 = 0; m33 = 1;
		}

		// 2.c - Create from Transformation (Scale and Shift)
		public Matrix(
			float scaleX, float scaleY, float shiftX, float shiftY)
		{
			m11 = scaleX; m12 = 0; m13 = shiftX;
			m21 = 0; m22 = scaleY; m23 = shiftY;
			m31 = 0; m32 = 0; m33 = 1;
		}

		#endregion

		#region Class Methods
		// 3.a - Transpose a matrix
		public void Transpose()
		{
			Matrix transposed = new Matrix(
				m11, m21, m31,
				m12, m22, m32,
				m13, m23, m33
				);

			m11 = transposed.m11;
			m12 = transposed.m12;
			m13 = transposed.m13;

			m21 = transposed.m21;
			m22 = transposed.m22;
			m23 = transposed.m23;

			m31 = transposed.m31;
			m32 = transposed.m32;
			m33 = transposed.m33;
		}

        // 3.b - Determinant of a matrix
        public float Determinant()
        {
            return (m11 * m22 * m33) + (m12 * m23 * m31) + (m13 * m21 * m32)
                    - (m13 * m22 * m31) - (m12 * m21 * m33) - (m11 * m23 * m32);
        }

		private float Determinant2x2(float m11, float m12, float m21, float m22)
		{
			return (m11 * m22) - (m12 * m21);
		}

		// 3.c - Inverse of a matrix
		public Matrix Inverse()
		{
			Matrix inverted = new Matrix();

			float inverseDet = 1 / Determinant();

			//cf = cofactor
			Matrix cofactorMatrix = new Matrix(
				Determinant2x2(m22, m23, m32, m33),-Determinant2x2(m21, m23, m31, m33), Determinant2x2(m21, m22, m31, m32),
			   -Determinant2x2(m12, m13, m32, m33), Determinant2x2(m11, m13, m31, m33),-Determinant2x2(m11, m12, m31, m32),
				Determinant2x2(m12, m13, m22, m23),-Determinant2x2(m11, m13, m21, m23), Determinant2x2(m11, m12, m21, m22)
				);

			cofactorMatrix.Transpose();

			cofactorMatrix *= inverseDet;

			inverted.m11 = cofactorMatrix.m11;
			inverted.m12 = cofactorMatrix.m12;
			inverted.m13 = cofactorMatrix.m13;

			inverted.m21 = cofactorMatrix.m21;
			inverted.m22 = cofactorMatrix.m22;
			inverted.m23 = cofactorMatrix.m23;

			inverted.m31 = cofactorMatrix.m31;
			inverted.m32 = cofactorMatrix.m32;
			inverted.m33 = cofactorMatrix.m33;

			return inverted;
		}

		#endregion

		#region Overload Operators
		#region Complier Warning Fix
		// the following two methods are to remove the CS0660 and CS0661 compiler warnings
		public override bool Equals(object obj)
        {
            return true;
        }//eom
        public override int GetHashCode()
        {
            return 0;
        }//eom
        #endregion

        // 4.a - Equality of two 3x3 matrices
		public static bool operator ==(Matrix a, Matrix b)
		{
			return
				a.m11 == b.m11 && a.m12 == b.m12 && a.m13 == b.m13 &&
				a.m21 == b.m21 && a.m22 == b.m22 && a.m23 == b.m23 &&
				a.m31 == b.m31 && a.m32 == b.m32 && a.m33 == b.m33;
		}

		// 4.a - Inequality of two 3x3 matrices
		public static bool operator !=(Matrix a, Matrix b)
		{
			return
				a.m11 != b.m11 || a.m12 != b.m12 || a.m13 != b.m13 ||
				a.m21 != b.m21 || a.m22 != b.m22 || a.m23 != b.m23 ||
				a.m31 != b.m31 || a.m32 != b.m32 || a.m33 != b.m33;
		}

		// 4.b - Scale a matrix
		public static Matrix operator *(Matrix matrix, float scale)
		{
			return new Matrix(
				matrix.m11 * scale, matrix.m12 * scale, matrix.m13 * scale,
				matrix.m21 * scale, matrix.m22 * scale, matrix.m23 * scale,
				matrix.m31 * scale, matrix.m32 * scale, matrix.m33 * scale
				);
		}

		// 4.c - Multiply two 3x3 matrices
		public static Matrix operator *(Matrix a, Matrix b)
		{
			return new Matrix(
				a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31, a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32, a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
				a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31, a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32, a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
				a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31, a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32, a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33
				);
		}

		public static Vec3 operator *(Matrix a, Vec3 b)
		{
			return new Vec3(
				b.X * a.m11 + b.Y * a.m12 + b.Z * a.m13,
				b.X * a.m21 + b.Y * a.m22 + b.Z * a.m23,
				b.X * a.m31 + b.Y * a.m32 + b.Z * a.m33
			);
		}

		public static Vec3 operator *(Vec3 a, Matrix b)
		{
			return new Vec3(
				b.m11 * a.X + b.m12 * a.Y + b.m13 * a.Z,
				b.m21 * a.X + b.m22 * a.Y + b.m33 * a.Z,
				b.m31 * a.X + b.m32 * a.Y + b.m33 * a.Z
			);
		}

		#endregion
	}//eoc
}//eon
