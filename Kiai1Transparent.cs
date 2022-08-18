using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Kiai1Transparent : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("Kiai1Transparent");
            var whiteLayer = GetLayer("Kiai1White");
 
            var two = layer.CreateSprite("sb/halfBGs/2_transparent.png");
            var three = layer.CreateSprite("sb/halfBGs/3_transparent.png");
            
            two.Scale(71172, ScreenScale);
            two.Move(71172, MaximumDimensions.X - 249.5, ScreenMiddle.Y);
            two.Fade(71172, 92483, 1, 1);

            three.Scale(92319, ScreenScale);
            three.Move(92319, MinimumDimensions.X + 249.5, ScreenMiddle.Y);
            three.Fade(92319, 113303, 1, 1);
        }
    }
}
