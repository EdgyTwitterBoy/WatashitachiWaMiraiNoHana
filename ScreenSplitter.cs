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
        public override void Generate()
        {
		    var layer = GetLayer("ScreenSplitter");
            var splitter = layer.CreateSprite("sb/screenHalf.png");
            splitter.Scale(21500, ScreenScale);
            splitter.Fade(21500, 270680, 1, 1);
            splitter.MoveX(21500, leftPosition);

            foreach (var time in ChangeTimes)
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
