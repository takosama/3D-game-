using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

using DxLibVector = DxLibDLL.DX.VECTOR;
using DxLibMatrix = DxLibDLL.DX.MATRIX;

namespace Pacraft_c____
{

    class Vector
    {
        public float X;
        public float Y;
        public float Z;

        public Vector(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Vector(DxLibVector vector)
        {
            this.X = vector.x;
            this.Y = vector.y;
            this.Z = vector.z;
        }

        public static Vector operator +(Vector v0, Vector v1)
        {
            return new Vector(v0.X + v1.X, v0.Y + v1.Y, v0.Z + v1.Z);
        }

        public static Vector operator -(Vector v0, Vector v1)
        {
            return new Vector(v0.X - v1.X, v0.Y - v1.Y, v0.Z - v1.Z);
        }

        public static Vector operator *(Vector v, float n)
        {
            return new Vector(v.X * n, v.Y * n, v.Z * n);
        }

        public static Vector operator /(Vector v, float n)
        {
            return new Vector(v.X / n, v.Y / n, v.Z / n);
        }

        public Vector Rotate(Vector angle)
        {
            #region comment
            /*       float x = (float)(this.X * Math.Cos(angle.X) * Math.Cos(angle.Y) +
                           this.Y * ((Math.Cos(angle.X) * Math.Sin(angle.Y) * Math.Sin(angle.Z) - Math.Sin(angle.X) * Math.Cos(angle.Z))) +
                           this.Z * ((Math.Cos(angle.X) * Math.Sin(angle.Y) * Math.Cos(angle.Z) + Math.Sin(angle.X) * Math.Sin(angle.Z))));

                   float y = (float)(this.X * Math.Sin(angle.X) * Math.Cos(angle.Y) +
                           this.Y * ((Math.Sin(angle.X) * Math.Sin(angle.Y) * Math.Sin(angle.Z) + Math.Cos(angle.X) * Math.Cos(angle.Z))) +
                           this.Z * ((Math.Sin(angle.X) * Math.Sin(angle.Y) * Math.Cos(angle.Z) - Math.Cos(angle.X) * Math.Sin(angle.Z))));

                   float z = (float)(this.X * -Math.Sin(angle.Y) + this.Y * Math.Cos(angle.Y) * Math.Sin(angle.Z) + this.Z * Math.Cos(angle.Y) * Math.Cos(angle.Z));
                return new Vector(x, y, z);       
            もったいないってとっておくのはスパゲッティのもと    */
            #endregion
            DxLibMatrix rotX = DX.MGetRotX(angle.X);
            DxLibMatrix rotY = DX.MGetRotY(angle.Y);
            DxLibMatrix rotZ = DX.MGetRotZ(angle.Z);

            DxLibMatrix rotation = DX.MMult(DX.MMult(rotX, rotY), rotZ);

            return DX.VTransform(this, rotation);
        }

        public static implicit operator DxLibVector(Vector v)
        {
            DxLibVector result = new DxLibVector();
            result.x = v.X;
            result.y = v.Y;
            result.z = v.Z;

            return result;
        }

        public static implicit operator Vector(DxLibVector v)
        {
            Vector result = new Vector(v.x, v.y, v.z);
            return result;
        }
    }
}