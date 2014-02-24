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
            bool[] crossroadCons = new bool[4];
            crossroadCons[0] = true;
            crossroadCons[1] = true;
            crossroadCons[2] = true;
            crossroadCons[3] = true;

            bool[] straightCons = new bool[4];
            straightCons[0] = true;
            straightCons[1] = false;
            straightCons[2] = true;
            straightCons[3] = false;

            bool[] cornerCons = new bool[4];
            cornerCons[0] = true;
            cornerCons[1] = true;
            cornerCons[2] = false;
            cornerCons[3] = false;

            bool[] tJunctionCons = new bool[4];
            tJunctionCons[0] = true;
            tJunctionCons[1] = true;
            tJunctionCons[2] = false;
            tJunctionCons[3] = true;

            bool[] oneWaycons = new bool[4];
            oneWaycons[0] = false;
            oneWaycons[1] = false;
            oneWaycons[2] = true;
            oneWaycons[3] = false;

            tiles = new List<MapTile>();

            MapTile townSquare = new MapTile(Frempt.TextureLibrary.MT_townSquare, "Town Square");
            townSquare.Setup(crossroadCons, 0, 0, 0);
            tiles.Add(townSquare);

            List<MapTile> tempDeck = new List<MapTile>();

            MapTile crossroads = new MapTile(Frempt.TextureLibrary.MT_crossroads, "Crossroads");
            crossroads.Setup(crossroadCons, 0, 0, 0);
            tempDeck.Add(crossroads);
            tempDeck.Add(crossroads);
            tempDeck.Add(crossroads);

            MapTile straight = new MapTile(Frempt.TextureLibrary.MT_straight, "Straight");
            straight.Setup(straightCons, 0, 0, 0);
            tempDeck.Add(straight);
            tempDeck.Add(straight);
            tempDeck.Add(straight);

            MapTile corner = new MapTile(Frempt.TextureLibrary.MT_corner, "Corner");
            corner.Setup(cornerCons, 0, 0, 0);
            tempDeck.Add(corner);
            tempDeck.Add(corner);
            tempDeck.Add(corner);

            MapTile tJunction = new MapTile(Frempt.TextureLibrary.MT_tJunction, "T-Junction");
            tJunction.Setup(tJunctionCons, 0, 0, 0);
            tempDeck.Add(tJunction);
            tempDeck.Add(tJunction);
            tempDeck.Add(tJunction);

            MapTile hospital = new MapTile(Frempt.TextureLibrary.MT_hospital, "Hospital");
            hospital.Setup(oneWaycons, 8, 0, 4);
            tempDeck.Add(hospital);

            MapTile gardenStore = new MapTile(Frempt.TextureLibrary.MT_gardenStore, "Garden Store");
            gardenStore.Setup(oneWaycons, 0, 0, 0);
            tempDeck.Add(gardenStore);

            Random rng = new Random();

            while (tempDeck.Count > 0)
            {
                int index = rng.Next(0, tempDeck.Count);

                tiles.Add(tempDeck[index]);

                tempDeck.Remove(tempDeck[index]);
            }

            MapTile helipad = new MapTile(Frempt.TextureLibrary.MT_helipad, "Helipad");
            helipad.Setup(crossroadCons, 9, 0, 0);
            tiles.Add(helipad);
            //tiles.Insert(rng.Next((tiles.Count/2) - tiles.Count/5, (tiles.Count/2) + tiles.Count/5), helipad);
        }

        public static MapTile Draw()
        {
            if (tiles.Count > 0)
            {
                MapTile returnTile = tiles[0];
                tiles.RemoveAt(0);
                return returnTile;
            }

            return null;
        }

        public static MapTile Peek()
        {
            if (tiles.Count > 0)
            {
                return tiles[0];
            }
            return null;
        }
    }
}
