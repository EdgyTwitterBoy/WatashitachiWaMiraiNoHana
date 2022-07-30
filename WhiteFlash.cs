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
    public class WhiteFlash : StoryboardObjectGeneratorPlus
    {
        [Configurable] public string flashTimes;
        public override void Generate()
        {
		    var layer = GetLayer("FlashWhite");
            var white = layer.CreateSprite("sb/pixelWhite.png");
            int[] times = flashTimes.Split(',').Select(int.Parse).ToArray();

            white.ScaleVec(times[0], ScreenScale * 1920, ScreenScale * 1080);

            foreach (var time in times)
            {
                white.Flash(time, GetBeatDuration(Beatmap));
            }
        }
    }
}
