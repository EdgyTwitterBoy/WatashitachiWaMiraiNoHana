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
using System.Drawing;

namespace StorybrewScripts
{
    public class Soshite : StoryboardObjectGeneratorPlus
    {
        [Configurable] public string times;
        [Configurable] public Vector2 letterOffset;
        [Configurable] public Vector2 startPosition;
        [Configurable] public double startOffset;
        [Configurable] public double endTime;
        [Configurable] public double letterScale;

        private double[] offsets = new double[11] {-10, -10, 2, -5, -10, -3, 0, -10, 0, -8, 0};

        private class Letter
        {
            public OsbSprite sprite;
            public int startTime;

            public Letter(OsbSprite sprite, int startTime)
            {
                this.sprite = sprite;
                this.startTime = startTime;
            }
        }

        public override void Generate()
        {
		    var layer = GetLayer("Soshite");
            int[] times = GetTimes();
            List<Letter> letters = new List<Letter>();

            for (int i = 0; i < 11; i++)
            {
                var letterSprite = layer.CreateSprite("sb/soshite/" + i + ".png");
                letters.Add(new Letter(letterSprite, times[i]));
            }

            for(int i = 0; i < times.Length; i++)
            {
                DrawLetter(letters[i].sprite, letters[i].startTime, GetOffset(i), i);
            }
            
        }

        private int[] GetTimes()
        {
            var times = this.times.Split(',');
            var timesInt = new int[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                timesInt[i] = int.Parse(times[i]);
            }
            return timesInt;
        }

        private void DrawLetter(OsbSprite sprite, int startTime, double offset, int index)
        {
            sprite.Scale(startTime, letterScale * ScreenScale);
            sprite.MoveX(startTime, startPosition.X + letterOffset.X * index + offset);
            sprite.MoveY(OsbEasing.OutBack, startTime, startTime + GetBeatDuration(Beatmap), startPosition.Y + startOffset, startPosition.Y);
            sprite.Fade(OsbEasing.Out, startTime, startTime + GetHalfBeatDuration(Beatmap), 0, 0.7);
            sprite.Fade(startTime + GetHalfBeatDuration(Beatmap), 13589, 0.7, 0.7);
            sprite.Fade(13589, endTime, 0.7, 1);
            sprite.Color(13589, endTime, Color4.White, Color4.MidnightBlue);
            sprite.Fade(OsbEasing.Out, endTime, endTime + GetHalfBeatDuration(Beatmap), 0.8, 0);
            sprite.MoveY(OsbEasing.Out, endTime, endTime + GetHalfBeatDuration(Beatmap), startPosition.Y, startPosition.Y + startOffset);
        }

        private double GetOffset(int index)
        {
            double offset = 0;
            for (int i = 0; i < index; i++)
            {
                offset += offsets[i];
            }
            return offset;
        }
    }
}
