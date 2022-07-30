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
    public class ScreenSplitter : StoryboardObjectGeneratorPlus
    {
        [Configurable]public float leftPosition;
        [Configurable]public float rightPosition;

        private bool isLeft = true;
        private int[] changeTimes = {44942, 71172, 92319, 113139, 134122, 160352, 191991, 226090, 257565};
        public override void Generate()
        {
		    var layer = GetLayer("ScreenSplitter");
            var splitter = layer.CreateSprite("sb/screenHalf.png");
            splitter.Scale(21500, ScreenScale);
            splitter.Fade(21500, 270680, 1, 1);
            splitter.MoveX(21500, leftPosition);

            foreach (var time in changeTimes)
            {
                ChangeSpot(splitter, time);
            }
        }

        void ChangeSpot(OsbSprite splitter, int time)
        {
            if(isLeft)
            {
                splitter.MoveX(OsbEasing.OutExpo, time, time + GetBeatDuration(Beatmap), leftPosition, rightPosition);
                isLeft = false;
            }
            else
            {
                splitter.MoveX(OsbEasing.OutExpo, time, time + GetBeatDuration(Beatmap), rightPosition, leftPosition);
                isLeft = true;
            }
        }
    }
}
