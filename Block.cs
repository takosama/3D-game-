using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacraft_c____
{

    class Block
    {
        public int ID;
        public Cube cube;

        public Block(int ID, Cube c)
        {
            this.cube = c;
            BlockTexture texture = BlockTextures.blockTextureArrey[ID];
            c.SetTexture(texture);
        }
    }

    class BlockTexture
    {
        public int Top { get; private set; } = DX.DX_NONE_GRAPH;
        public int Buttom { get; private set; } = DX.DX_NONE_GRAPH;
        public int Left { get; private set; } = DX.DX_NONE_GRAPH;
        public int Right { get; private set; } = DX.DX_NONE_GRAPH;
        public int Front { get; private set; } = DX.DX_NONE_GRAPH;
        public int Back { get; private set; } = DX.DX_NONE_GRAPH;
        public int[] Arrey = new int[6];
        public BlockTexture(int Top = DX.DX_NONE_GRAPH, int Buttom = DX.DX_NONE_GRAPH, int Left = DX.DX_NONE_GRAPH, int Right = DX.DX_NONE_GRAPH ,int Front = DX.DX_NONE_GRAPH, int Back = DX.DX_NONE_GRAPH)
        {
            this.Top = Top;
            this.Buttom = Buttom;
            this.Left = Left;
            this.Right = Right;
            this.Front = Front;
            this.Back = Back;


            this.Arrey[0] = Top;
            this.Arrey[1] = Buttom;
            this.Arrey[2] = Left;
            this.Arrey[3] = Right;
            this.Arrey[4] = Front;
            this.Arrey[5] = Back;
        }
    }

    static class BlockTextures
    {
        public static BlockTexture[] blockTextureArrey = new BlockTexture[256];
    }

    class Chunk
    {
        Vector ChunkVector;
        int[,,] blockArrey = new int[16, 16, 16];
        PolygonList pl;
        public Chunk(Vector ChunkVector)
        {
            this.ChunkVector = ChunkVector;
        }

        public void SetBlock(Vector v, int ID)
        {
            blockArrey[(int)v.X, (int)v.Y, (int)v.Z] = ID;
        }

        public void SendGPU()
        {
            int seed = (int)(ChunkVector.X * 256 + ChunkVector.Y * 16 + ChunkVector.Z);
            seed = seed * seed;
            
            Random r = new Random(seed);
          pl = new PolygonList();
            byte R = (byte)r.Next(0, 255);
            byte G = (byte)r.Next(0, 255);
            byte B = (byte)r.Next(0, 255);

            int[,,] polygon = new int[16, 16, 6];
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (blockArrey[x, 0, z] == 0) continue;
                    for (int i = 0; i < 6; i++)
                    {
                        if (x == 0 || x == 15)
                        {
                            polygon[x, z, i] = 1;
                            continue;
                        }
                        if (z == 0 || z == 15)
                        {
                            polygon[x, z, i] = 1;
                            continue;
                        }


                        //top
                        if (i == 0)
                            if (blockArrey[x , 1, z] == 0)
                                polygon[x, z, i] = 1;
                            else
                                polygon[x, z, i] = 0;


                        //buttom
                        if (i == 1)
                        {
                            polygon[x, z, i] = 1;
                        }
                    //        if (blockArrey[x , -1, z] == 0)
                      //          polygon[x, z, i] = 1;
                        //    else
                          //      polygon[x, z, i] = 0;


                        //left
                        if (i == 2)
                                if (blockArrey[x - 1, 0, z] == 0)
                                    polygon[x, z, i] = 1;
                                else
                                    polygon[x, z, i] = 0;

                        //right
                        if (i == 3)
                            if (blockArrey[x + 1, 0, z] == 0)
                                polygon[x, z, i] = 1;
                            else
                                polygon[x, z, i] = 0;


                        //front
                        if (i == 4)
                                if (blockArrey[x , 0, z-1] == 0)
                                    polygon[x, z, i] = 1;
                                else
                                    polygon[x, z, i] = 0;
                        //back
                        if (i == 5)
                            if (blockArrey[x, 0, z + 1] == 0)
                                polygon[x, z, i] = 1;
                            else
                                polygon[x, z, i] = 0;

                        //            else 
                        //          polygon[x, z, i] = 1 ;

                    }
                    }
                }

            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                {

                //     R = (byte)r.Next(0, 255);
                  //   G = (byte)r.Next(0, 255);
                    // B = (byte)r.Next(0, 255);
                        

                    if (blockArrey[x, 0, z] == 0) continue;
                    {
                        Cube cube = new Cube(new Vector(this.ChunkVector.X * 16f + x, this.ChunkVector.Y * 16f + 0, this.ChunkVector.Z * 16f + z), 1f);
                        cube.SetColor(new Color(R, G, B, 255));
                        cube.SetTexture(BlockTextures.blockTextureArrey[blockArrey[x, 0, z]]);

                        for (int i = 0; i < 6; i++)
                        {
                            if (polygon[x, z, i] == 0) continue;
                            pl.Add(cube.Surface[i], BlockTextures.blockTextureArrey[blockArrey[x, 0, z]].Arrey[i]);
                        }
                    }
                }

                        








                             pl.SendGPU();
        }

        public void Draw()
        {
            pl.DrawGPU();
        }

        public void GenerateTerrian()
        {
            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 1; y++)
                    for (int z = 0; z < 16; z++)
                        SetBlock(new Vector(x, y, z), 1);
        }

        public void Dispose()
        {
            pl.Clean();
        }
    }
}