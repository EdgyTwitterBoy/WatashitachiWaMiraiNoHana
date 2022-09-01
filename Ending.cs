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
    public class Ending : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("Ending");
            var black = layer.CreateSprite("sb/pixelBlack.png");

            black.ScaleVec(270024, 1920 * ScreenScale, 1080 * ScreenScale);
            black.Fade(270024, 270680, 0, 1);
            black.Fade(270680, 284720 + 500, 1, 1);
        }
    }
}
