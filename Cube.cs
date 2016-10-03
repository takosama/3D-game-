using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;


namespace Pacraft_c____
{
    class Cube
    {
        enum SurfaceName
        {
            Top = 0,
            Under = 1,
            Left = 2,
            Right = 3,
            Front = 4,
            Back = 5
        }

        Vector CenterVector;
     public QuadPanel[] Surface { get; private set; } = new QuadPanel[6];
       public int[] Textures { get; private set; } = new int[6];

        public Cube(Vector CenterVector, float Size)
        {
            this.CenterVector = CenterVector;

            //Top
            Surface[0] = new QuadPanel(CenterVector + new Vector(0, Size * 0.5f, 0), new Vector((float)(Math.PI * 0.5f), 0, 0), Size);
            Surface[0].SetColor(new Color(255, 255, 255, 255));
            //Under
            Surface[1] = new QuadPanel(CenterVector + new Vector(0, -Size * 0.5f, 0), new Vector(-(float)(Math.PI * 0.5f), 0, 0), Size);
            Surface[1].SetColor(new Color(255, 255, 255, 255));
            //Left
            Surface[2] = new QuadPanel(CenterVector + new Vector(-Size * 0.5f, 0, 0), new Vector(0, (float)(Math.PI * 0.5f), 0), Size);
            Surface[2].SetColor(new Color(255, 255, 255, 255));
            //Right
            Surface[3] = new QuadPanel(CenterVector + new Vector(Size * 0.5f, 0, 0), new Vector(0, -(float)(Math.PI * 0.5f), 0), Size);
            Surface[3].SetColor(new Color(255, 255, 255, 255));
            //Front
            Surface[4] = new QuadPanel(CenterVector + new Vector(0, 0, -Size * 0.5f), new Vector(0, 0, 0), Size);
            Surface[4].SetColor(new Color(255, 255, 255, 255));
            //Back                                                                                
            Surface[5] = new QuadPanel(CenterVector + new Vector(0, 0, Size * 0.5f), new Vector(0, (float)(-Math.PI), 0), Size);
            Surface[5].SetColor(new Color(255, 255, 255, 255));

            this.SetTextureAll(DX.DX_NONE_GRAPH);
        }

        public void Draw()
        {
       //     foreach (var quad in Surface)
         //       if (quad != null)
           //         quad.Draw(Ghdl);

            for(int i=0;i<6;i++)
            {
                Surface[i].Draw(Textures[i]);
            }
        }

        public void Move(Vector vector)
        {

            foreach (var quad in this.Surface)
                quad.Move(vector);
        }

        public void SetTexture(int Top = DX.DX_NONE_GRAPH, int Under = DX.DX_NONE_GRAPH
                              , int Left = DX.DX_NONE_GRAPH, int Right = DX.DX_NONE_GRAPH
                              , int Front = DX.DX_NONE_GRAPH, int Back = DX.DX_NONE_GRAPH)
        {
            Textures[(int)SurfaceName.Top] = Top;
            Textures[(int)SurfaceName.Under] = Under;
            Textures[(int)SurfaceName.Left] = Left;
            Textures[(int)SurfaceName.Right] = Right;
            Textures[(int)SurfaceName.Front] = Front;
            Textures[(int)SurfaceName.Back] = Back;
        }

        public void SetTexture(BlockTexture texture)
        {
            Textures[(int)SurfaceName.Top] = texture.Top;
            Textures[(int)SurfaceName.Under] = texture.Buttom;
            Textures[(int)SurfaceName.Left] = texture.Left;
            Textures[(int)SurfaceName.Right] = texture.Right;
            Textures[(int)SurfaceName.Front] = texture.Front;
            Textures[(int)SurfaceName.Back] = texture.Back;
        }

        public void SetTextureAll(int Ghdl=DX.DX_NONE_GRAPH)
        {
          this.Textures=  this.Textures.Select(n =>n= Ghdl).ToArray();
        }

        public void SetColor(Color c)
        {
            foreach (var n in Surface)
                n.SetColor(c);
        }
    }
}



