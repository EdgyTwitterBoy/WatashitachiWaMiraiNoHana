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
    public class BlackBG : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("BlackBG");
            var black = layer.CreateSprite("sb/pixelBlack.png");
            
            black.ScaleVec(-500, ScreenScale * 1920, ScreenScale * 1080);
            black.Fade(-500, 284720, 1, 1);
        }
    }
}
