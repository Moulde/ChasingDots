using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChasingDots
{
//<<<<<<< .mine
    public class Level
//=======
    //class FirstLevel : BaseLevel
//>>>>>>> .r129
    {
//<<<<<<< .mine
        public int TilesWide { get; set; }
        public int TilesHigh { get; set; }
        public int TileSizeInPixels { get; set; }
        public Tile StartTilePrey { get; set; }
        public Tile StartTileHunter { get; set; }
        public Vector2 LevelOffsetInPixels { get; set; }


        bool _teleportersOpen = false;
        public bool TeleportersOpen
//=======
//        public FirstLevel(int tileSize, Vector2 offset) : base(tileSize, offset)
//>>>>>>> .r129
        {
//<<<<<<< .mine
            get { return _teleportersOpen; }
            protected set { _teleportersOpen = value;
            if (_teleportersOpen)
            {
                OpenTeleporters();
            }
            else
            {
                CloseTeleporters();
            }
            }
        }

        Dictionary<Tile, Tile> _teleporterTiles;
        public Dictionary<Tile, Tile> TeleporterTiles
        {
            get
            { return _teleporterTiles; }
            set
            {
                if (value != null)
                {
                    _teleporterTiles = value;
                }
            }
        }


        public Tile[,] Tiles{get;set;}

        public Tile KeyTile { get; set; }



        public void CloseTeleporters()
        {
            _teleportersOpen = false;
            foreach (Tile teleporterTile in TeleporterTiles.Values)
            {
                teleporterTile.Type = TileType.Solid;
            }
            UpdateTeleporterTilesProperty();

        }
        
        public void OpenTeleporters()
        {
            _teleportersOpen = true;
            foreach (Tile teleporterTile in TeleporterTiles.Values)
            {
                teleporterTile.Type = TileType.Teleporter;
            }
            UpdateTeleporterTilesProperty();
        }

        private void UpdateTeleporterTilesProperty()
        {
            
        }

////=======
//            this.data = new int[13, 13] {
//            {0,0,0,0,0,0,2,0,0,0,0,0,0},
//            {0,1,1,1,1,1,1,1,1,1,1,1,0},
//            {0,1,0,0,1,0,0,0,1,0,0,1,0},
//            {0,1,0,1,1,1,1,1,1,1,0,1,0},
//            {0,1,1,1,0,0,1,0,0,1,1,1,0},
//            {0,1,0,1,0,1,1,1,0,1,0,1,0},
//            {2,1,0,1,1,1,1,1,1,1,0,1,2},
//            {0,1,0,1,0,1,1,1,0,1,0,1,0},
//            {0,1,1,1,0,0,1,0,0,1,1,1,0},
//            {0,1,0,1,1,1,1,1,1,1,0,1,0},
//            {0,1,0,0,1,0,0,0,1,0,0,1,0},
//            {0,1,1,1,1,1,1,1,1,1,1,1,0},
//            {0,0,0,0,0,0,2,0,0,0,0,0,0}
//            };
////>>>>>>> .r129
//        }
    

//<<<<<<< .mine

        public Level(int[,] tileData, int tileSizeInPixels, Vector2 levelOffsetInPixels, Dictionary<Point, Point> teleporterIndexes)
        {

            LevelOffsetInPixels = levelOffsetInPixels;
            TileSizeInPixels = tileSizeInPixels;
            this.SetTilesFromIntArray(tileData);
            TeleporterTiles = new Dictionary<Tile, Tile>();
            foreach (Point departurePoint in teleporterIndexes.Keys)
            {
                Tile departureTile = Tiles[departurePoint.X, departurePoint.Y];
                Tile destinationTile = Tiles[teleporterIndexes[departurePoint].X, teleporterIndexes[departurePoint].Y];
                TeleporterTiles.Add(departureTile, destinationTile);
            }
        }
////=======
//    class SecondLevel : BaseLevel
//    {
//        public SecondLevel(int tileSize, Vector2 offset)
//            : base(tileSize, offset)
//        {
//            this.data = new int[13, 13] {
//            {0,0,0,0,0,0,2,0,0,0,0,0,0},
//            {0,1,1,1,1,1,1,1,1,1,1,1,0},
//            {0,1,0,1,0,1,0,1,0,1,0,1,0},
//            {0,1,1,1,0,1,0,1,0,1,1,1,0},
//            {0,1,0,0,0,1,0,1,0,0,0,1,0},
//            {0,1,1,1,1,1,0,1,1,1,1,1,0},
//            {2,1,0,0,0,1,1,1,0,0,0,1,2},
//            {0,1,1,1,1,1,0,1,1,1,1,1,0},
//            {0,1,0,0,0,1,0,1,0,0,0,1,0},
//            {0,1,1,1,0,1,0,1,0,1,1,1,0},
//            {0,1,0,1,0,1,0,1,0,1,0,1,0},
//            {0,1,1,1,1,1,1,1,1,1,1,1,0},
//            {0,0,0,0,0,0,2,0,0,0,0,0,0}
//            };
//>>>>>>> .r129
//        }
//    }

//<<<<<<< .mine
        public void SetTilesFromIntArray(int[,] tileData)
        //=======
        //    class ThirdLevel : BaseLevel
        //    {
        //        public ThirdLevel(int tileSize, Vector2 offset)
        //            : base(tileSize, offset)
        ////>>>>>>> .r129
        {
            //<<<<<<< .mine
            TilesWide = tileData.GetUpperBound(1) + 1;
            TilesHigh = tileData.GetUpperBound(0) + 1;
            Tiles = new Tile[TilesWide, TilesHigh];

            TileType typeOfTile = TileType.Open;

            Vector2 halfTile = new Vector2(TileSizeInPixels / 2, TileSizeInPixels / 2);


            for (int y = 0; y < TilesHigh; y++)
            {
                for (int x = 0; x < TilesWide; x++)
                {
                    typeOfTile = (TileType)tileData[x, y];
                    Tiles[x, y] = new Tile(typeOfTile, x, y);

                    switch (typeOfTile)
                    {
                        case TileType.StartHunter:
                            this.StartTileHunter = Tiles[x, y];
                            break;
                        case TileType.StartPrey:
                            this.StartTilePrey = Tiles[x, y];
                            break;
                        case TileType.Key:
                            this.KeyTile = this.Tiles[x, y];
                            break;
                        default:
                            break;
                    }

                    Tiles[x, y].TileCenter = new Vector2(x * TileSizeInPixels, y * TileSizeInPixels) + halfTile + LevelOffsetInPixels;
                }
            }
        }
//=======
//            this.data = new int[13, 13] {
//            {0,0,0,0,0,0,0,0,0,0,0,2,0},
//            {2,1,0,1,1,1,1,1,1,1,1,1,0},
//            {0,1,0,1,0,0,1,1,0,1,0,0,0},
//            {0,1,1,1,1,0,1,1,0,1,1,1,0},
//            {0,1,0,0,1,1,1,1,1,1,0,1,0},
//            {0,1,1,1,1,1,0,1,1,0,0,1,0},
//            {0,1,1,1,1,0,0,0,1,1,1,1,0},
//            {0,1,0,0,1,1,0,1,1,1,1,1,0},
//            {0,1,0,1,1,1,1,1,1,0,0,1,0},
//            {0,1,1,1,0,1,1,0,1,1,1,1,0},
//            {0,0,0,1,0,1,1,0,0,1,0,1,0},
//            {0,1,1,1,1,1,1,1,1,1,0,1,2},
//            {0,2,0,0,0,0,0,0,0,0,0,0,0}
//            };
//>>>>>>> .r129
        

        //private int[,] SwapXY(int[,] arrayToSwap)
        //{
        //    int width = arrayToSwap.GetUpperBound(0)+1;
        //    int height = arrayToSwap.GetUpperBound(1)+1;
        //    int[,] swapArray = new int[width,height];
        //    for (int y = 0; y < height; y++)
        //    {
                
            
        //    for (int x = 0; x < width; x++)
        //    {
        //        swapArray[x, y] = arrayToSwap[y, x];
        //    }
        //    }
        //    return swapArray;
        //}

        public void ResetLevel()
        {
            CloseTeleporters();
            if (KeyTile != null)
            {
                KeyTile.Type = TileType.Key;
            }
            
        }


    }

    //class FourthLevel : BaseLevel
    //{
    //    public FourthLevel(int tileSize, Vector2 offset)
    //        : base(tileSize, offset)
    //    {
    //        this.data = new int[13, 13] {
    //        {0,0,0,0,0,0,2,0,0,0,0,0,0},
    //        {0,1,1,1,1,0,1,0,1,1,1,1,0},
    //        {0,1,1,0,1,0,1,0,1,0,1,1,0},
    //        {0,1,0,0,1,1,1,1,1,0,0,1,0},
    //        {0,1,1,0,1,0,0,0,1,0,1,1,0},
    //        {0,0,1,0,1,1,1,1,1,0,1,0,0},
    //        {2,1,1,1,1,0,3,0,1,1,1,1,2},
    //        {0,0,1,0,1,1,1,1,1,0,1,0,0},
    //        {0,1,1,0,1,0,0,0,1,0,1,1,0},
    //        {0,1,0,0,1,1,1,1,1,0,0,1,0},
    //        {0,1,1,0,1,0,1,0,1,0,1,1,0},
    //        {0,1,1,1,1,0,1,0,1,1,1,1,0},
    //        {0,0,0,0,0,0,2,0,0,0,0,0,0}
    //        };
    //    }
    //}
}
