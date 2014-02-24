/*
	File:			TextureLibrary.cs
	Version:		1.0
	Last altered:	28/01/2014.
	Authors:		James MacGilp.

	Description:	- Used to load in textures which are statically accessable. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Frempt
{
    class TextureLibrary
    {
        public static Texture2D MT_townSquare;
        public static Texture2D MT_helipad;
        public static Texture2D MT_crossroads;
        public static Texture2D MT_straight;
        public static Texture2D MT_corner;
        public static Texture2D MT_tJunction;
        public static Texture2D MT_hospital;
        public static Texture2D MT_gardenStore;

        public static Texture2D Illegal;

        private TextureLibrary()
        {
        }

        public static void LoadTextures(ContentManager content)
        {
            MT_townSquare = content.Load<Texture2D>("MT_TownSquare");
            MT_helipad = content.Load<Texture2D>("MT_helipad");
            MT_crossroads = content.Load<Texture2D>("MT_crossroad");
            MT_straight = content.Load<Texture2D>("MT_straight");
            MT_corner = content.Load<Texture2D>("MT_corner");
            MT_tJunction = content.Load<Texture2D>("MT_tJunction");
            MT_hospital = content.Load<Texture2D>("MT_hospital");
            MT_gardenStore = content.Load<Texture2D>("MT_gardenStore");
            Illegal = content.Load<Texture2D>("illegal");
        }
    }
}
