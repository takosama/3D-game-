using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pacraft_c____
{

    class QuadPanel
    {
        public Polygon[] polygon = new Polygon[2];//ポリゴン２つで正方形が描ける
        Vector CenterVector;
      
        public QuadPanel(Vector vec, Vector rotate, float size)
        {
            this.CenterVector = vec;
            float distanceFromCenter = 0.5f * size;

            Vector vec0 = vec + new Vector(distanceFromCenter, distanceFromCenter, 0);
            Vector vec1 = vec + new Vector(-distanceFromCenter, distanceFromCenter, 0);
            Vector vec2 = vec + new Vector(distanceFromCenter, -distanceFromCenter, 0);
            Vector vec3 = vec + new Vector(-distanceFromCenter, -distanceFromCenter, 0);

            Vector[] vecArrey0 = { vec0, vec1, vec2 };
            polygon[0] = new Polygon(vecArrey0);
            Vector[] vecArrey1 = { vec1, vec2, vec3 };
            polygon[1] = new Polygon(vecArrey1);

            polygon[0].SetUV(1, 0, 0);
            polygon[0].SetUV(0, 0, 1);
            polygon[0].SetUV(1, 1, 2);

            polygon[1].SetUV(0, 0, 0);
            polygon[1].SetUV(1, 1, 1);
            polygon[1].SetUV(0, 1, 2);


            this.SetColor(new Color(0, 0, 0, 255));
            this.Rotate(rotate);
        }

        public void SetColor(Color color)//色を設定
        {
            Color[] colorArrey = { color, color, color };
            foreach (var n in polygon)
                n.SetColor(colorArrey);
        }

        public void Rotate(Vector Angle)//回転
        {
            polygon[0].Rotate(Angle, CenterVector);
            polygon[1].Rotate(Angle, CenterVector);
        }

        public void Draw(int GHdl = DX.DX_NONE_GRAPH)//描画
        {
            polygon[0].Draw(GHdl);
            polygon[1].Draw(GHdl);
        }

        public void Move(Vector vector)
        {
            foreach (var p in this.polygon)
            {
                p.Move(vector);
            }
        }
    }
}