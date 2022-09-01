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
    public class Kiai2Transparent : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("Kiai2Transparent");
            var six = layer.CreateSprite("sb/halfBGs/6_transparent.png");

            six.Scale(160352, ScreenScale);
            six.Move(160352, MaximumDimensions.X - 249.5, ScreenMiddle.Y);
            six.Fade(160352, 184122, 1, 1);
        }
    }
}
