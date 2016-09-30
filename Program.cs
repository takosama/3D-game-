using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using System.Drawing;

namespace Pacraft_c____
{
    class Program
    {
        static void Main(string[] args)
        {
        //   DX. SetOutApplicationLogValidFlag(0);
      //      DX.SetUseDirectDrawFlag(DX.FALSE);
            DX.ChangeWindowMode(1);
            DX.SetGraphMode(1280, 720, 16);//縦横比と画面サイズの設定

            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.SetUseZBufferFlag(1);
            DX.SetWriteZBufferFlag(1);
            Vector v0 = new Vector(0, 0, 10);
            Vector v1 = new Vector(50, 0, 10);
            Vector v2 = new Vector(0, 50, 10);
            Vector[] varrey = { v0, v1, v2 };
            int hdl = DX.LoadGraph("te.png");

            #region CreateTerrain

            BlockTextures.blockTextureArrey[1] = new BlockTexture(hdl, hdl, hdl, hdl, hdl, hdl);

            //誤視聴ありがとナス！
            Chunk[] chunk = new Chunk[64];// new Chunk(new Vector(0, 0, 0));
            for (int chunk_x = 0; chunk_x < 8; chunk_x++)
            {
                for (int chunk_z = 0; chunk_z < 8; chunk_z++)
                {
                    chunk[chunk_x * 8 + chunk_z] = new Chunk(new Vector(chunk_x, 0, chunk_z));
                    for (int cx = 0; cx < 16; cx++)
                        for (int cy = 0; cy < 4; cy++)
                            for (int cz = 0; cz < 16; cz++)
                            {
                                chunk[chunk_x * 8 + chunk_z].SetBlock(new Vector(cx, cy, cz), 1);
                            }
                    chunk[chunk_x * 8 + chunk_z].SendGPU();

                }
            }
            

            #endregion

            float x = 0;
            float y = 2.1f;
            float z = -2;
            float ay = 0;
            float PlayerMoveSpped = 0.05f;

            while (true)
            {
                DX.ClearDrawScreen();

                #region PlayerMove
                if (DX.CheckHitKey(DX.KEY_INPUT_W) == 1)
                {
                    x += (float)(PlayerMoveSpped * Math.Sin(ay));
                    z += (float)(PlayerMoveSpped * Math.Cos(ay));
                }
                if (DX.CheckHitKey(DX.KEY_INPUT_S) == 1)
                {
                    x -= (float)(PlayerMoveSpped * Math.Sin(ay));
                    z -= (float)(PlayerMoveSpped * Math.Cos(ay));
                }
                if (DX.CheckHitKey(DX.KEY_INPUT_A) == 1)
                {
                    x -= (float)(PlayerMoveSpped * Math.Cos(ay));
                    z += (float)(PlayerMoveSpped * Math.Sin(ay));
                }
                if (DX.CheckHitKey(DX.KEY_INPUT_D) == 1)
                {
                    x += (float)(PlayerMoveSpped * Math.Cos(ay));
                    z -= (float)(PlayerMoveSpped * Math.Sin(ay));
                }
                if (DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1) ay -= 0.01f;
                if (DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1) ay += 0.01f;
                if (DX.CheckHitKey(DX.KEY_INPUT_UP) == 1) y   +=PlayerMoveSpped;
                if (DX.CheckHitKey(DX.KEY_INPUT_DOWN) == 1) y -=PlayerMoveSpped;
                #endregion

                DX.SetCameraPositionAndAngle(new Vector(x, y, z), 0, ay, 0);
                DX.SetCameraNearFar(0.1f, 100);
                foreach(var c in chunk)
                c.Draw();



                DX.ScreenFlip();


            }
            DX.WaitKey();
            DX.DxLib_End();
        }
    }
}
