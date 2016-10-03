using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacraft_c____
{
    class World
    {
        int LoadDistance = 8;
        Chunk[,] ChunkArrey;
        Vector Position;
        public World()
        {
            ChunkArrey = new Chunk[LoadDistance, LoadDistance];

            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                {
                    ChunkArrey[x, z] = new Chunk(new Vector(x, 0, z));

                    ChunkArrey[x, z].GenerateTerrian();

                }
            Refresh();
        }


        public void SetPlayerPosition(Vector vec)
        {
            int cx;
            int cz;
            int px;
            int pz;

            int bcx;
            int bcz;
            int bpx;
            int bpz;
            GetChunkPosition(vec, out cx, out cz, out px, out pz);
            GetChunkPosition(Position, out bcx, out bcz, out bpx, out bpz);
            Position = vec;
            int moveX = cx - bcx;
            int moveZ = cz - bcz;

            Chunk[,] nchunkarrey = new Chunk[LoadDistance, LoadDistance];
            bool gcf = false;


            ChunkArrey = MovePlayer(ChunkArrey, moveX, moveZ, cx, cz);

        }

        Chunk[,] MovePlayer(Chunk[,] map, int Move_X, int Move_Z, int cx, int cz)
        {
            Chunk[,] Nmap = new Chunk[LoadDistance, LoadDistance];
            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                {
                    //x
                    if (Move_X > 0)
                    {
                        if (x + Move_X < LoadDistance)
                            Nmap[x, z] = map[x + Move_X, z];
                        else
                        {
                            map[LoadDistance - x - Move_X, z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx + x, 0,cz+ z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();
                        }
                    }
                    else if (Move_X < 0)
                    {
                        if (x + Move_X < 0)
                        {
                            map[LoadDistance + x + Move_X, z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx + x, 0,cz+ z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();

                        } //    Nmap[x, z];
                        else
                            Nmap[x, z] = map[x + Move_X, z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }

            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                    map[x, z] = Nmap[x, z];

            //z
            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                {
                    if (Move_Z > 0)
                    {
                        if (z + Move_Z < LoadDistance)
                            Nmap[x, z] = map[x, z + Move_Z];
                        else
                        {
                            map[x, LoadDistance - z - Move_Z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx+x, 0, cz + z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();
                        }
                    }
                    else if (Move_Z < 0)
                    {
                        if (z + Move_Z < 0)
                        {
                            map[ x , LoadDistance + z + Move_Z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx+ x, 0, cz + z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();

                        } //    Nmap[x, z];
                        else
                            Nmap[x, z] = map[x, z + Move_Z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }



            for (int x = 0; x < LoadDistance; x++)
                      for (int z = 0; z < LoadDistance; z++)
                          map[x, z] = Nmap[x, z];

            #region test
            /*
             *  for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                {
                    //x
                    if (Move_X > 0)
                    {
                        if (x + Move_X < LoadDistance)
                            Nmap[x, z] = map[x + Move_X, z];
                        else
                        {
                            map[LoadDistance - x - Move_X, z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx + x, 0, z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();
                        }
                    }
                    else if (Move_X < 0)
                    {
                        if (x + Move_X < 0)
                        {
                            map[LoadDistance + x + Move_X, z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(cx + x, 0, z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();

                        } //    Nmap[x, z];
                        else
                            Nmap[x, z] = map[x + Move_X, z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }

            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                    map[x, z] = Nmap[x, z];

            //z
            for (int x = 0; x < LoadDistance; x++)
                for (int z = 0; z < LoadDistance; z++)
                {
                    if (Move_Z > 0)
                    {
                        if (z + Move_Z < LoadDistance)
                            Nmap[x, z] = map[x, z + Move_Z];
                        else
                        {
                            map[x, LoadDistance - z - Move_Z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(x, 0, cz + z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();
                        }
                    }
                    else if (Move_Z < 0)
                    {
                        if (z + Move_Z < 0)
                        {
                            map[ x , LoadDistance + z + Move_Z].Dispose();
                            Nmap[x, z] = new Chunk(new Vector(x, 0, cz + z));
                            Nmap[x, z].GenerateTerrian();
                            Nmap[x, z].SendGPU();

                        } //    Nmap[x, z];
                        else
                            Nmap[x, z] = map[x, z + Move_Z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }



            for (int x = 0; x < LoadDistance; x++)
                      for (int z = 0; z < LoadDistance; z++)
                          map[x, z] = Nmap[x, z];
                

             */
            #endregion
            return map;
            #region maptest 
            /*
            int[,] Nmap = new int[16, 16];

            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                {
                    //x
                    if (move_X > 0)
                    {
                        if (x + move_X < 16)
                            Nmap[x, z] = map[x + move_X, z];
                        else
                            Nmap[x, z];
                    }
                    else if (move_X < 0)
                    {
                        if (x + move_X < 0)
                            Nmap[x, z];
                        else
                            Nmap[x, z] = map[x + move_X, z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }


            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                    map[x, z] = Nmap[x, z];

            //z
            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                {
                    if (move_Z > 0)
                    {
                        if (z + move_Z < 16)
                            Nmap[x, z] = map[x, z + move_Z];
                        else
                            Nmap[x, z];
                    }
                    else if (move_Z < 0)
                    {
                        if (z + move_Z < 0)
                            Nmap[x, z];
                        else
                            Nmap[x, z] = map[x, z + move_Z];
                    }
                    else
                        Nmap[x, z] = map[x, z];
                }

            for (int x = 0; x < 16; x++)
                for (int z = 0; z < 16; z++)
                    map[x, z] = Nmap[x, z];

            return map;*/
            #endregion

        }
        public void Refresh()
        {
            foreach (var n in ChunkArrey)
                n.SendGPU();
        }

        public void Draw()
        {
            foreach (var n in ChunkArrey)
                if(n!=null)
                n.Draw();
        }

        public void GetChunkPosition(Vector Position, out int Chunk_X, out int Chunk_Z, out int Position_X, out int Position_Z)
        {
            if (Position == null)
            {
                Chunk_X = 0;
                Chunk_Z = 0;
                Position_X = 0;
                Position_Z = 0;
            }
            else
            {
                if (Position.X < 0)
                    Chunk_X = (int)(Position.X / 16) - 1;
                else
                    Chunk_X = (int)(Position.X / 16);

                if (Position.Z < 0)
                    Chunk_Z = (int)(Position.Z / 16) - 1;
                else
                    Chunk_Z = (int)(Position.Z / 16);

                Position_X = (int)(Position.X % 16);
                Position_Z = (int)(Position.Z % 16);
                Console.Write(Chunk_X + ",");
                Console.WriteLine(Chunk_Z);
            }
        }
    }
}
 