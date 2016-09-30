using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;


namespace Pacraft_c____
{
    class Color//Dxlib Windows 独自の色のクラス　の架け橋
    {
        public byte R = 0;
        public byte G = 0;
        public byte B = 0;
        public byte A = 0;

        public Color(byte R, byte G, byte B, byte A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public Color(DX.COLOR_U8 color)
        {
            this.R = color.r;
            this.G = color.g;
            this.B = color.b;
            this.A = color.a;
        }

        public DX.COLOR_U8 Get_COLOR_U8
        {
            get
            {
                DX.COLOR_U8 rtn = new DX.COLOR_U8();
                rtn.r = this.R;
                rtn.g = this.G;
                rtn.b = this.B;
                rtn.a = this.A;

                return rtn;
            }
        }

        public uint ToUint
        {
            get
            {
                return DX.GetColor(this.R, this.G, this.B);
            }
        }

        
    }
}


