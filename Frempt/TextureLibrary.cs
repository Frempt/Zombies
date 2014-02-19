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

        private TextureLibrary()
        {
        }

        public static void LoadTextures(ContentManager content)
        {
            MT_townSquare = content.Load<Texture2D>("MT_TownSquare");
            MT_helipad = content.Load<Texture2D>("MT_helipad");
            
        }
    }
}
