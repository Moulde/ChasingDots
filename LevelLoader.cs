using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChasingDots
{
    public class LevelLoader
    {

        static List<int[,]> LevelData = new List<int[,]>();
        static List<Dictionary<Point, Point>> TeleportData = new List<Dictionary<Point, Point>>();

        static LevelLoader ()
	{

        #region Level 1

        LevelData.Add(
    SwapXY( new int[,]
    {
        {1,1,1,1,1,1,2,1,1,1,1,1,1},
        {1,4,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,0,1,1,0,1},
        {1,0,1,0,0,0,0,0,0,0,1,0,1},
        {1,0,0,0,1,1,0,1,1,0,0,0,1},
        {1,0,1,0,1,1,0,1,1,0,1,0,1},
        {2,0,1,0,0,0,9,0,0,0,1,0,2},
        {1,0,1,0,1,1,0,1,1,0,1,0,1},
        {1,0,0,0,1,1,0,1,1,0,0,0,1},
        {1,0,1,0,0,0,0,0,0,0,1,0,1},
        {1,0,1,1,0,1,1,1,0,1,1,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,3,1},
        {1,1,1,1,1,1,2,1,1,1,1,1,1}
    }));


        Dictionary<Point, Point> TeleportDestinations = new Dictionary<Point, Point>();

        TeleportDestinations.Add(new Point(0, 6), new Point(12, 6));
        TeleportDestinations.Add(new Point(12, 6), new Point(0, 6));
        TeleportDestinations.Add(new Point(6, 0), new Point(6, 12));
        TeleportDestinations.Add(new Point(6, 12), new Point(6, 0));

        TeleportData.Add(TeleportDestinations);


        #endregion


        #region Level 2

        LevelData.Add(
SwapXY( new int[,] {
            {1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,4,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,0,1,0,2,0,1,0,1,0,1},
            {1,0,0,0,1,0,1,0,1,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,1,1,0,1},
            {1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,2,1,1,0,9,0,1,1,2,0,1},
            {1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,1,1,0,1},
            {1,0,0,0,1,0,1,0,1,0,0,0,1},
            {1,0,1,0,1,0,2,0,1,0,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,3,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1}
            }));

        TeleportDestinations = new Dictionary<Point, Point>();

        TeleportDestinations.Add(new Point(2, 6), new Point(10, 6));
        TeleportDestinations.Add(new Point(10, 6), new Point(2, 6));
        TeleportDestinations.Add(new Point(6, 2), new Point(6, 10));
        TeleportDestinations.Add(new Point(6, 10), new Point(6, 2));

        TeleportData.Add(TeleportDestinations);

        #endregion

        #region Level 3
        LevelData.Add(SwapXY( new int[,]{
            {1,1,1,1,1,1,1,1,1,1,1,2,1},
            {2,0,1,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,0,1,1,0,0,1,0,1,1,1},
            {1,0,0,4,0,1,0,0,1,0,0,0,1},
            {1,0,1,1,0,0,0,0,0,0,1,0,1},
            {1,0,0,0,0,1,0,1,0,1,1,0,1},
            {1,0,0,0,0,0,9,0,0,0,0,0,1},
            {1,0,1,1,0,1,0,1,0,0,0,0,1},
            {1,0,1,0,0,0,0,0,0,1,1,0,1},
            {1,0,0,0,1,0,0,1,0,3,0,0,1},
            {1,1,1,0,1,0,0,1,1,0,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,1,0,2},
            {1,2,1,1,1,1,1,1,1,1,1,1,1}
            }));

        TeleportDestinations = new Dictionary<Point, Point>();

        TeleportDestinations.Add(new Point(0, 1), new Point(12, 11));
        TeleportDestinations.Add(new Point(12, 11), new Point(0, 1));
        TeleportDestinations.Add(new Point(11, 0), new Point(1, 12));
        TeleportDestinations.Add(new Point(1, 12), new Point(11,0 ));

        TeleportData.Add(TeleportDestinations);


        #endregion


        #region Level 4

        LevelData.Add(SwapXY(new int[,] {
            {1,1,1,1,1,1,2,1,1,1,1,1,1},
            {1,0,0,0,0,1,0,1,0,0,0,0,1},
            {1,0,4,1,0,1,0,1,0,1,0,0,1},
            {1,0,1,1,0,0,0,0,0,1,1,0,1},
            {1,0,0,2,0,1,1,1,0,2,0,0,1},
            {1,1,0,1,0,0,0,0,0,1,0,1,1},
            {2,0,0,0,0,1,9,1,0,0,0,0,2},
            {1,1,0,1,0,0,0,0,0,1,0,1,1},
            {1,0,0,2,0,1,1,1,0,2,0,0,1},
            {1,0,1,1,0,0,0,0,0,1,1,0,1},
            {1,0,0,1,0,1,0,1,0,1,3,0,1},
            {1,0,0,0,0,1,0,1,0,0,0,0,1},
            {1,1,1,1,1,1,2,1,1,1,1,1,1}
            }));

        TeleportDestinations = new Dictionary<Point, Point>();

        TeleportDestinations.Add(new Point(0, 6), new Point(12, 6));
        TeleportDestinations.Add(new Point(12, 6), new Point(0, 6));
        TeleportDestinations.Add(new Point(6, 0), new Point(6, 12));
        TeleportDestinations.Add(new Point(6, 12), new Point(6, 0));
        TeleportDestinations.Add(new Point(3, 4), new Point(9, 4));
        TeleportDestinations.Add(new Point(9, 4), new Point(3, 4));
        TeleportDestinations.Add(new Point(3, 8), new Point(9, 8));
        TeleportDestinations.Add(new Point(9, 8), new Point(3, 8));


        TeleportData.Add(TeleportDestinations);


        #endregion  
        }
        public static Level GetLevel(int levelIndex, int tileSize,Vector2 levelOffsetInPixels)
        {

            return new Level(LevelData[levelIndex], tileSize, levelOffsetInPixels, TeleportData[levelIndex]);
        }



        private static int[,] SwapXY(int[,] arrayToSwap)
        {
            int width = arrayToSwap.GetUpperBound(0) + 1;
            int height = arrayToSwap.GetUpperBound(1) + 1;
            int[,] swapArray = new int[width, height];
            for (int y = 0; y < height; y++)
            {


                for (int x = 0; x < width; x++)
                {
                    swapArray[x, y] = arrayToSwap[y, x];
                }
            }
            return swapArray;
        }

    }
}
