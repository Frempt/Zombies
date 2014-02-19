using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zombies
{
    public class MapDeck
    {
        private static List<MapTile> tiles;

        private MapDeck()
        {
        }

        public static void Shuffle()
        {
            tiles = new List<MapTile>();

            MapTile townSquare = new MapTile(Frempt.TextureLibrary.MT_townSquare, "Town Square");
            bool[] townSquareCons = new bool[4];
            townSquareCons[0] = true;
            townSquareCons[1] = true;
            townSquareCons[2] = true;
            townSquareCons[3] = true;
            townSquare.Setup(townSquareCons, 0, 0, 0);
            tiles.Add(townSquare);

            List<MapTile> tempDeck = new List<MapTile>();

            //add map tiles here

            Random rng = new Random();

            while (tempDeck.Count > 0)
            {
                int index = rng.Next(0, tempDeck.Count);

                tiles.Add(tempDeck[index]);

                tempDeck.Remove(tempDeck[index]);
            }

            MapTile helipad = new MapTile(Frempt.TextureLibrary.MT_helipad, "Helipad");
            bool[] helipadCons = new bool[4];
            helipadCons[0] = true;
            helipadCons[1] = true;
            helipadCons[2] = true;
            helipadCons[3] = true;
            helipad.Setup(helipadCons, 0, 0, 0);
            tiles.Insert(rng.Next((tiles.Count/2) - 3, (tiles.Count/2) + 3), helipad);
        }

        public static MapTile Draw()
        {
            MapTile returnTile = tiles[0];
            tiles.Remove(returnTile);
            return returnTile;
        }

        public static MapTile Peek()
        {
            return tiles[0];
        }
    }
}
