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
    public class GuitarPart1 : StoryboardObjectGeneratorPlus
    {
        public override void Generate()
        {
		    var layer = GetLayer("GuitarPart1");
            var orb = layer.CreateSprite("sb/magicPattern.png", OsbOrigin.Centre);

            int startTime = 116254;
            int endTime = 123795;

            int startTime2 = 191991;
            int endTime2 = 204122;

            int startTime3 = 257893;
            int endTime3 = 270680;

            orb.Scale(startTime, 1.5 * ScreenScale);
            orb.Color(startTime, Color4.Black);
            orb.Move(startTime, 100, 240);
            orb.Fade(OsbEasing.In, startTime, startTime + GetBeatDuration(Beatmap) * 2, 0, 1);
            orb.Fade(OsbEasing.Out, endTime - GetBeatDuration(Beatmap) * 2, endTime, 1, 0);
            orb.Rotate(startTime, endTime, 0, 8);

            orb.Move(startTime2, 540, 240);
            orb.Fade(startTime2, 1);
            orb.Fade(OsbEasing.Out, endTime2 - GetBeatDuration(Beatmap), endTime2, 1, 0);
            orb.Rotate(startTime2, endTime2, 0, -12);

            orb.Move(startTime3, 100, 240);
            orb.Fade(OsbEasing.In, startTime3, startTime3 + GetBeatDuration(Beatmap), 0, 1);
            orb.Rotate(startTime3, endTime3, 0, 8);
        }
    }
}
