using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine.Util
{
    public class Matrix4x4
    {
		// Properties
		public float m11 { get; set; }
		public float m12 { get; set; }
		public float m13 { get; set; }
		public float m14 { get; set; }
		public float m21 { get; set; }
		public float m22 { get; set; }
		public float m23 { get; set; }
		public float m24 { get; set; }
		public float m31 { get; set; }
		public float m32 { get; set; }
		public float m33 { get; set; }
		public float m34 { get; set; }
		public float m41 { get; set; }
		public float m42 { get; set; }
		public float m43 { get; set; }
		public float m44 { get; set; }

		// Constructors
		public Matrix4x4()
		{
			m11 = 0;
			m12 = 0;
			m13 = 0;
			m14 = 0;
			m21 = 0;
			m22 = 0;
			m23 = 0;
			m24 = 0;
			m31 = 0;
			m32 = 0;
			m33 = 0;
			m34 = 0;
			m41 = 0;
			m42 = 0;
			m43 = 0;
			m44 = 0;
		}

		public Matrix4x4(
			float m11, float m12, float m13, float m14,
			float m21, float m22, float m23, float m24,
			float m31, float m32, float m33, float m34,
			float m41, float m42, float m43, float m44)
		{
			this.m11 = m11;
			this.m12 = m12;
			this.m13 = m13;
			this.m14 = m14;
			this.m21 = m21;
			this.m22 = m22;
			this.m23 = m23;
			this.m24 = m24;
			this.m31 = m31;
			this.m32 = m32;
			this.m33 = m33;
			this.m34 = m34;
			this.m41 = m41;
			this.m42 = m42;
			this.m43 = m43;
			this.m44 = m44;
		}

		#region Additional Constructors
		// 2.a - Create Identity matrix
		public void Identity()
		{
			m11 = 1; m12 = 0; m13 = 0; m14 = 0;
			m21 = 0; m22 = 1; m23 = 0; m24 = 0;
			m31 = 0; m32 = 0; m33 = 1; m34 = 0;
			m41 = 0; m42 = 0; m43 = 0; m44 = 1;
		}

		// 2.b - Create from Transformation (Scale and Shift)
		public Matrix4x4(
			float scaleX, float scaleY, float scaleZ, float shiftX, float shiftY, float shiftZ)
		{
			m11 = scaleX; m12 = 0; m13 = 0; m14 = shiftX;
			m21 = 0; m22 = scaleY; m23 = 0; m24 = shiftY;
			m31 = 0; m32 = 0; m33 = scaleZ; m34 = shiftZ;
			m41 = 0; m42 = 0; m43 = 0; m44 = 1;
		}

        // 2.c - Create a Roll rotation matrix from an angle in degrees
        public void CreateRollMatrix(float degrees)
        {
            float rad = degrees * (float)(Math.PI/180f);

            m11 = (float)Math.Cos(rad); m12 = (float)-Math.Sin(rad); m13 = 0; m14 = 0;
            m21 = (float)Math.Sin(rad); m22 = (float)Math.Cos(rad); m23 = 0; m24 = 0;
            m31 = 0; m32 = 0; m33 = 1; m34 = 0;
            m41 = 0; m42 = 0; m43 = 0; m44 = 1;
        }

        // 2.d - Create a Pitch rotation matrix from an angle in degrees
        public void CreatePitchMatrix(float degrees)
		{
			float rad = degrees * (float)(Math.PI / 180f);

			m11 = 1; m12 = 0; m13 = 0; m14 = 0;
            m21 = 0; m22 = (float)Math.Cos(rad); m23 = (float)-Math.Sin(rad); m24 = 0;
            m31 = 0; m32 = (float)Math.Sin(rad); m33 = (float)Math.Cos(rad); m34 = 0;
            m41 = 0; m42 = 0; m43 = 0; m44 = 1;
        }

        // 2.e - Create a Yaw rotation matrix from an angle in degrees
        public void CreateYawMatrix(float degrees)
		{
			float rad = degrees * (float)(Math.PI / 180f);

			m11 = (float)Math.Cos(rad); m12 = 0; m13 = (float)Math.Sin(rad); m14 = 0;
            m21 = 0; m22 = 1; m23 = 0; m24 = 0;
            m31 = (float)-Math.Sin(rad); m32 = 0; m33 = (float)Math.Cos(rad); m34 = 0;
            m41 = 0; m42 = 0; m43 = 0; m44 = 1;
        }

        #endregion

        #region Class Methods
        // 3.a - Transpose a matrix
        public void Transpose()
        {
            Matrix4x4 transposed = new Matrix4x4(
                m11, m21, m31, m41,
                m12, m22, m32, m42,
                m13, m23, m33, m43,
                m14, m24, m34, m44
                );

            m11 = transposed.m11;
            m12 = transposed.m12;
            m13 = transposed.m13;
            m14 = transposed.m14;

            m21 = transposed.m21;
            m22 = transposed.m22;
            m23 = transposed.m23;
            m24 = transposed.m24;

            m31 = transposed.m31;
            m32 = transposed.m32;
            m33 = transposed.m33;
            m34 = transposed.m34;

            m41 = transposed.m41;
            m42 = transposed.m42;
            m43 = transposed.m43;
            m44 = transposed.m44;
        }

        // 3.b - Determinant of a matrix
        public float Determinant()
        {
            return (m11 * m22 * m33 * m44) + (m12 * m23 * m34 * m41) + (m13 * m24 * m31 * m42) + (m14 * m21 * m32 * m43)
                    - (m14 * m23 * m32 * m41) - (m13 * m22 * m31 * m44) - (m12 * m21 * m34 * m43) - (m11 * m24 * m33 * m42);
        }

        // 3.c - Inverse of a matrix
        public void Invert()
        {
            float inverseDet = 1 / Determinant();

            //cf = cofactor
            Matrix4x4 cofactorMatrix = new Matrix4x4(
                new Matrix3x3(m22, m23, m24, m32, m33, m34, m42, m43, m44).Determinant(), -new Matrix3x3(m21, m23, m24, m31, m33, m34, m41, m43, m44).Determinant(), new Matrix3x3(m21, m22, m24, m31, m32, m34, m41, m42, m44).Determinant(), -new Matrix3x3(m21, m22, m23, m31, m32, m33, m41, m42, m43).Determinant(),
                -new Matrix3x3(m12, m13, m14, m32, m33, m34, m42, m43, m44).Determinant(), new Matrix3x3(m11, m13, m14, m31, m33, m34, m41, m43, m44).Determinant(), -new Matrix3x3(m11, m12, m14, m31, m32, m34, m41, m42, m44).Determinant(), new Matrix3x3(m11, m12, m13, m31, m32, m33, m41, m42, m43).Determinant(),
                new Matrix3x3(m12, m13, m14, m22, m23, m24, m42, m43, m44).Determinant(), -new Matrix3x3(m11, m13, m14, m21, m23, m24, m41, m43, m44).Determinant(), new Matrix3x3(m11, m12, m14, m21, m22, m24, m41, m42, m44).Determinant(), -new Matrix3x3(m11, m12, m13, m21, m22, m23, m41, m42, m43).Determinant(),
                -new Matrix3x3(m12, m13, m14, m22, m23, m24, m32, m33, m34).Determinant(), new Matrix3x3(m11, m13, m14, m21, m23, m24, m31, m33, m34).Determinant(), -new Matrix3x3(m11, m12, m14, m21, m22, m24, m31, m32, m34).Determinant(), new Matrix3x3(m11, m12, m13, m21, m22, m23, m31, m32, m33).Determinant()
            );

            cofactorMatrix.Transpose();

            cofactorMatrix *= inverseDet;

            m11 = cofactorMatrix.m11;
            m12 = cofactorMatrix.m12;
            m13 = cofactorMatrix.m13;
            m14 = cofactorMatrix.m14;

            m21 = cofactorMatrix.m21;
            m22 = cofactorMatrix.m22;
            m23 = cofactorMatrix.m23;
            m24 = cofactorMatrix.m24;

            m31 = cofactorMatrix.m31;
            m32 = cofactorMatrix.m32;
            m33 = cofactorMatrix.m33;
            m34 = cofactorMatrix.m34;

            m41 = cofactorMatrix.m41;
            m42 = cofactorMatrix.m42;
            m43 = cofactorMatrix.m43;
            m44 = cofactorMatrix.m44;
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

        // 4.a - Equality of two 4x4 matrices
        public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
        {
            return
                a.m11 == b.m11 && a.m12 == b.m12 && a.m13 == b.m13 && a.m14 == b.m14 &&
                a.m21 == b.m21 && a.m22 == b.m22 && a.m23 == b.m23 && a.m24 == b.m24 &&
                a.m31 == b.m31 && a.m32 == b.m32 && a.m33 == b.m33 && a.m34 == b.m34 &&
                a.m41 == b.m41 && a.m42 == b.m42 && a.m43 == b.m43 && a.m44 == b.m44;
        }

        // 4.a - Inequality of two 4x4 matrices
        public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
        {
            return
                a.m11 != b.m11 || a.m12 != b.m12 || a.m13 != b.m13 || a.m14 != b.m14 ||
                a.m21 != b.m21 || a.m22 != b.m22 || a.m23 != b.m23 || a.m24 != b.m24 ||
                a.m31 != b.m31 || a.m32 != b.m32 || a.m33 != b.m33 || a.m34 != b.m34 ||
                a.m41 != b.m41 || a.m42 != b.m42 || a.m43 != b.m43 || a.m44 != b.m44;
        }

        // 4.b - Scale a matrix
        public static Matrix4x4 operator *(Matrix4x4 matrix, float scale)
        {
            return new Matrix4x4(
                matrix.m11 * scale, matrix.m12 * scale, matrix.m13 * scale, matrix.m14 * scale,
                matrix.m21 * scale, matrix.m22 * scale, matrix.m23 * scale, matrix.m24 * scale,
                matrix.m31 * scale, matrix.m32 * scale, matrix.m33 * scale, matrix.m34 * scale,
                matrix.m41 * scale, matrix.m42 * scale, matrix.m43 * scale, matrix.m44 * scale
                );
        }

        // 4.c - Multiply two 4x4 matrices
        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41, a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42, a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43, a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44,
                a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41, a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42, a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43, a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44,
                a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41, a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42, a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43, a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44,
                a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41, a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42, a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43, a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44
                );
        }

        #endregion
    }//eoc
}//eon
