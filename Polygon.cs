using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Drawing;

namespace Pacraft_c____
{
    class Polygon
    {
     public   DX.VERTEX3D[] VertexData = new DX.VERTEX3D[3];//頂点データを入れる配列
     public   ushort[] IndexData = { 0, 1, 2 };//インデックスデータを入れる配列
        Vector[] VertexVector = new Vector[3];//頂点座標を入れる
        Vector CenterVector;
        public Polygon(Vector[] VertexVector)//コンストラクタ
        {
            if (VertexVector.Length != 3) return;//頂点座標の数をチェック
            this.VertexVector = VertexVector;

            //座標データの代入
            this.VertexData[0].pos = VertexVector[0];
            this.VertexData[1].pos = VertexVector[1];
            this.VertexData[2].pos = VertexVector[2];

            this.VertexData[0].norm = new Vector(0, 1, 0);
            this.VertexData[1].norm = new Vector(0, 1, 0);
            this.VertexData[2].norm = new Vector(0, 1, 0);
        }

        public void SetColor(Color[] ColorArrey)//ポリゴンに色をセット
        {
            VertexData[0].dif = ColorArrey[0].Get_COLOR_U8;
            VertexData[1].dif = ColorArrey[1].Get_COLOR_U8;
            VertexData[2].dif = ColorArrey[2].Get_COLOR_U8;
        }

        public void SetUV(float u,float v,int VertexNumber)
        {
            if (VertexNumber >= 3) return;//例外を弾く
            this.VertexData[VertexNumber].u = u;
            this.VertexData[VertexNumber].v = v;
        }

        public void Draw(int GHdl = DX.DX_NONE_GRAPH)//ポリゴンの描画
        {
            DX.DrawPolygonIndexed3D(out VertexData[0], 3, out IndexData[0], 1, GHdl, DX.TRUE);
        }

        public void Move(Vector vector)//ポリゴンの移動
        {
            for (int i = 0; i < VertexData.Length; i++)
                VertexData[i].pos = (new Vector(VertexData[i].pos) + vector);
        }

        public void Rotate(Vector angle, Vector CenterVector)
        {
            for (int i = 0; i < this.VertexData.Length; i++)
            {
                this.VertexData[i].pos = (new Vector(this.VertexData[i].pos) - CenterVector);
                   this.VertexData[i].pos = new Vector(this.VertexData[i].pos).Rotate(angle);
                this.VertexData[i].pos = (new Vector(this.VertexData[i].pos) + CenterVector);
            }
        }
    }
}

//
