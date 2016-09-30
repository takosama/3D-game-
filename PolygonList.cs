using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacraft_c____
{
    class PolygonList
    {
        List<List<Polygon>> polygonList = new List<List<Polygon>>();
        List<int> TextureList = new List<int>();
        int GraphicHandle = 0;

        public PolygonList()
        {
            return;
        }

        #region PolygonListに要素を追加

        public void Add(Block b)
        {
            Cube c = b.cube;
            Add__SetUpList(c);//TexureListとPolygonListの設定

            Add__AddPolygonList(c);//ポリゴンを登録
        }
        public void Add(Cube c)
        {
            Add__SetUpList(c);//TexureListとPolygonListの設定

            Add__AddPolygonList(c);//ポリゴンを登録
        }

        public void Add(QuadPanel q, int TexrureHdl)
        {
            Add__SetUpList(q, TexrureHdl);
            Add__AddPolygonList(q, TexrureHdl);
        }

        void Add__SetUpList(Cube c)
        {
            foreach (var SurfaceTexture in c.Textures)
            {
                bool IsInTextureList = false;

                foreach (var Texture in TextureList)
                {
                    if (Texture == SurfaceTexture)
                    {
                        IsInTextureList = true;
                        break;
                    }
                }

                if (!IsInTextureList)
                {
                    TextureList.Add(SurfaceTexture);
                    polygonList.Add(new List<Polygon>());
                }
            }
        }
        void Add__SetUpList(QuadPanel q, int TexureHdl)
        {
            bool IsInTextureList = false;

            foreach (var Texture in TextureList)
            {
                if (Texture == TexureHdl)
                {
                    IsInTextureList = true;
                    break;
                }
            }

            if (!IsInTextureList)
            {
                TextureList.Add(TexureHdl);
                polygonList.Add(new List<Polygon>());
            }

        }
        void Add__AddPolygonList(Cube c)
        {
            for (int SurfaceNo = 0; SurfaceNo < c.Textures.Length; SurfaceNo++)
            {
                if (c.Surface[SurfaceNo] != null)
                {
                    int TextureHandle = c.Textures[SurfaceNo];
                    int PolygonListIndexPosition = TextureList.IndexOf(TextureHandle);

                    Polygon[] SurfacePolygons = c.Surface[SurfaceNo].polygon;
                    polygonList[PolygonListIndexPosition].AddRange(SurfacePolygons);
                }
            }
        }
        void Add__AddPolygonList(QuadPanel q, int TexureHdl)
        {
            int PolygonListIndexPosition = TextureList.IndexOf(TexureHdl);

            Polygon[] Polygons = q.polygon;
            polygonList[PolygonListIndexPosition].AddRange(Polygons);
        }



        #endregion

        public void Draw()
        {
            int TextureListPosition = 0;
            foreach (var polygons in polygonList)
            {
                foreach (var polugon in polygons)
                {
                    polugon.Draw(TextureList[TextureListPosition]);
                }
                TextureListPosition++;
            }
        }

        List<int> VertexBufferList = new List<int>();
        List<int> IndexBufferList = new List<int>();

        unsafe public void DrawGPU()
        {
            //頂点データ、インデックスデータのチェック
            if (VertexBufferList.Count != IndexBufferList.Count)
                return;

            //頂点バッファ、インデックスバッファを使用して描画
            for (int ListPosition = 0; ListPosition < VertexBufferList.Count; ListPosition++)
            {
                int VertexBuffer = VertexBufferList[ListPosition];
                int IndexBuffer = IndexBufferList[ListPosition];
                DX.DrawPolygonIndexed3D_UseVertexBuffer(VertexBuffer, IndexBuffer, TextureList[ListPosition], 1);
            }
        }

        unsafe public void SendGPU()
        {

            #region 頂点バッファ　インデックスバッファ　Handleの作成
            for (int ListPosition = 0; ListPosition < polygonList.Count; ListPosition++)
            {
                int PolygonCount = polygonList[ListPosition].Count;

                //頂点バッファの作成
                int VertexBuffer = DX.CreateVertexBuffer(PolygonCount * 3, DX.DX_VERTEX_TYPE_NORMAL_3D);
                VertexBufferList.Add(VertexBuffer);

                //インデックスバッファの作成
                int IndexBuffer = DX.CreateIndexBuffer(PolygonCount * 3, DX.DX_INDEX_TYPE_16BIT);
                IndexBufferList.Add(IndexBuffer);
            }
            #endregion

            #region Send BufferListData
            for (int ListPosition = 0; ListPosition < polygonList.Count; ListPosition++)
            {
                //頂点データの作成
                List<DX.VERTEX3D> VertexList = new List<DX.VERTEX3D>();
                var Vertexs = polygonList[ListPosition].Select(n => n.VertexData).ToArray();
                foreach (var n in Vertexs)
                    VertexList.AddRange(n);

                //頂点データの転送
                fixed (DX.VERTEX3D* VertexHandle = VertexList.ToArray())
                    DX.SetVertexBufferData(0, (IntPtr)VertexHandle, VertexList.Count(), VertexBufferList[ListPosition]);

                //インデックスデータの作成
                List<ushort> IndexList = new List<ushort>();
                var Indexs = polygonList[ListPosition].Select(n => n.IndexData).ToArray();
                int cnt = 0;
                foreach (var n in Indexs)
                {
                    IndexList.AddRange(n.Select(m => (ushort)(m + cnt * 3)));
                    cnt++;
                }

                //インデックスデータの転送
                fixed (ushort* IndexHandle = IndexList.ToArray())
                    DX.SetIndexBufferData(0, (IntPtr)IndexHandle, IndexList.Count(), IndexBufferList[ListPosition]);
            }
            #endregion
        }

        public void Clean()
        {
            //各種　リストの破棄
            polygonList = new List<List<Polygon>>();
            TextureList = new List<int>();

            //頂点バッファの開放
            foreach (var n in VertexBufferList)
                DX.DeleteVertexBuffer(n);
            //頂点バッファリストの開放
            VertexBufferList = new List<int>();

            //インデックスバッファの開放
            foreach (var n in IndexBufferList)
                DX.DeleteIndexBuffer(n);
            //インデックスバッファの開放
            IndexBufferList = new List<int>();
        }
    }
}
