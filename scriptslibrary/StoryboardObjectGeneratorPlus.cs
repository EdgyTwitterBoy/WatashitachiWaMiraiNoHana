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
    public abstract class StoryboardObjectGeneratorPlus : StoryboardObjectGenerator
    {
        protected static int Width = 1980;
        protected static int Height = 1020; //1,574074
        protected static int SlowPartOffset = 6771;
        protected static int Offset = 16091;
        protected static double ScreenScale = 480.0 / Height;
        protected static double GetBeatDuration(Beatmap beatmap) => beatmap.GetTimingPointAt(Offset).BeatDuration;
        protected static double GetHalfBeatDuration(Beatmap beatmap) => beatmap.GetTimingPointAt(Offset).BeatDuration / 2;
        protected static double GetQuarterBeatDuration(Beatmap beatmap) => beatmap.GetTimingPointAt(Offset).BeatDuration / 4;
        protected static Vector2 MinimumDimensions = new Vector2(-107, 0);
        protected static Vector2 MaximumDimensions = new Vector2(747, 480);
        protected static Vector2 ScreenMiddle = new Vector2(320, 240);
        protected static double DegToRad(double degrees) => degrees * 0.0174532925;
        protected static string MapPath = @"C:\Users\drevo\AppData\Local\osu!\Songs\Sonoda_Umi_CV_Mimori_Suzuko_-_Watashitachi_wa_Mirai_no_Hana";
        protected static string[] Mappers = {"0ppInOsu", "CutoNaito", "Cxk", "Halgoh", "Kyairie", "laplus", "MakiDonalds", "Murada", "Paradogi", "Rielle", "Slifer", "zesteas"};
    }

    public static class AddOns
    {
        public static void Flash(this OsbSprite sprite, double startTime, double duration)
        {
            if(duration <= 0) return;

            sprite.Fade(startTime, startTime + duration, 1, 0);
        }
    }
}