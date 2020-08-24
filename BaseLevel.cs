//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace ChasingDots
//{
//    class BaseLevel
//    {
//        public bool IsDoorsUnlocked { get; set; }

//        public int TileSize { get; set; }
//        public Vector2 Offset { get; set; }

//        public Vector2 StartPositionPray { get; set; }
//        public Vector2 StartPositionHunter { get; set; }

//        public int[,] data;

//        private Tile[,] tiles;
//        public Tile[,] Tiles
//        {
//            get
//            {
//                if (data == null)
//                    throw new InvalidOperationException("Level data must be set for the level to work");

//                if (tiles != null)
//                    return tiles;

//                tiles = ConvertLevelData();
//                return tiles;
//            }
//            set
//            {
//                this.tiles = value;
//            }
//        }

//        public BaseLevel(int tileSize, Vector2 offset)
//        {
//            this.TileSize = tileSize;
//            this.Offset = offset;
//            this.StartPositionHunter = new Vector2(3, 2);
//            this.StartPositionPray = new Vector2(13, 3);
//        }

//        public Tile[,] ConvertLevelData()
//        {
//            tiles = new Tile[13, 13];
//            for (int y = 0; y <= tiles.GetUpperBound(0); y++)
//            {
//                for (int x = 0; x <= tiles.GetUpperBound(1); x++)
//                {
//                    tiles[x, y] = new Tile((TileType)data[x, y]);

//                     this.tiles[x, y].TileCenter = new Vector2(
//                        x * this.TileSize + this.Offset.X + this.TileSize / 2,
//                        y * this.TileSize + this.Offset.Y + this.TileSize / 2);
//                }
//            }

//            return this.tiles;
//        }
//    }
//}
