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
    public class Kiai3Transparent : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("Kiai3Transparent");
            var eight = layer.CreateSprite("sb/halfBGs/8_transparent.png");
            var nine = layer.CreateSprite("sb/halfBGs/9_transparent.png");

            eight.Scale(204942, ScreenScale);
            eight.Move(204942, MaximumDimensions.X - 249.5, ScreenMiddle.Y);
            eight.Fade(204942, 226254, 1, 1);
        }
    }
}
