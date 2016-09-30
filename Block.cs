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
        public int Top = DX.DX_NONE_GRAPH;
        public int Buttom = DX.DX_NONE_GRAPH;
        public int Left = DX.DX_NONE_GRAPH;
        public int Right = DX.DX_NONE_GRAPH;
        public int Front = DX.DX_NONE_GRAPH;
        public int Back = DX.DX_NONE_GRAPH;
        public BlockTexture(int Top, int Buttom, int Left, int Right, int Front, int Back)
        {
            this.Top = Top;
            this.Buttom = Buttom;
            this.Left = Left;
            this.Right = Right;
            this.Front = Front;
            this.Back = Back;
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
            pl = new PolygonList();

            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 16; y++)
                    for (int z = 0; z < 16; z++)
                    {
                        if (blockArrey[x, y, z] == 0) continue;
                        Cube cube = new Cube(new Vector(this.ChunkVector.X * 16 + x, this.ChunkVector.Y * 16 + y, this.ChunkVector.Z * 16 + z), 1);
                        cube.SetTexture(BlockTextures.blockTextureArrey[blockArrey[x, y, z]]);
                        Block b = new Block(blockArrey[x, y, z], cube);
                        pl.Add(b);
                    }
            pl.SendGPU();
        }

        public void Draw()
        {
            pl.DrawGPU();
        }
    }
}