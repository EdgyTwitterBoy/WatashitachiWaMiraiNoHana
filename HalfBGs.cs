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
    public class HalfBGs : StoryboardObjectGeneratorPlus
    {
        private Vector2 leftPosition = new Vector2(-107, 240);
        private Vector2 rightPosition = new Vector2(747, 240);
        private int[,] times = new int[10, 2] {
            {21500, 45106},
            {44942, 70844},
            {71172, 92483},
            {92319, 113303},
            {113139, 134286},
            {134122, 160516},
            {160352, 192155},
            {191991, 226254},
            {226090, 257729},
            {257565, 270188}
        };

        private int[] threeFlashTimesPlus = {101336, 101991, 102811, 107401, 103139};

        private int[] threeFastFlashTimes = {110680, 110844, 111008, 111172, 111336, 111500, 111663, 111827, 101172};

        private int[] twoFlashTimes = {71172, 72483, 73795, 75106, 76581, 77729, 79204, 80352, 81827, 83139, 84450, 84942, 85434,
                                       87073, 88221, 89696, 91008, 91336};

        private int[] twoFastFlashTimes = {91663, 91827, 91991, 92155, 92319};

        private List<double> threeFlashTimes = new List<double>();

        public override void Generate()
        {
            StoryboardLayer[] layers = new StoryboardLayer[10];
            OsbSprite[] sprites = new OsbSprite[10];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = GetLayer("HalfBG " + i);
                sprites[i] = layers[i].CreateSprite($"sb/halfBGs/{i}.png", 
                                                    i % 2 == 0 ? OsbOrigin.CentreRight : OsbOrigin.CentreLeft,
                                                    i % 2 == 0 ? rightPosition : leftPosition);
            }

            foreach (var time in threeFlashTimesPlus)
            {
                threeFlashTimes.Add(time);
            }

            for (int i = 0; i < 13; i++)
            {
                threeFlashTimes.Add(92647 + GetBeatDuration(Beatmap) * 2 * i);
            }

            for (int i = 0; i < 7; i++)
            {
                threeFlashTimes.Add(103795 + GetBeatDuration(Beatmap) * 2 * i);
            }

            for (int i = 0; i < 8; i++)
            {
                threeFlashTimes.Add(108057 + GetBeatDuration(Beatmap) * i);
            }

		    var layer = GetLayer("HalfLayers");
            var zero = layer.CreateSprite("sb/halfBGs/0.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));
            var one = layer.CreateSprite("sb/halfBGs/1.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
            var two = layer.CreateSprite("sb/halfBGs/2.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));
            var three = layer.CreateSprite("sb/halfBGs/3.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
            var four = layer.CreateSprite("sb/halfBGs/4.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));
            var five = layer.CreateSprite("sb/halfBGs/5.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
            var six = layer.CreateSprite("sb/halfBGs/6.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));
            var seven = layer.CreateSprite("sb/halfBGs/7.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));
            var eight = layer.CreateSprite("sb/halfBGs/8.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));
            var nine = layer.CreateSprite("sb/halfBGs/9.png", OsbOrigin.CentreLeft, new Vector2(-107, 240));

            for (int i = 0; i < sprites.Length; i++)
            {
                SetProperties(sprites[i], times[i, 0], times[i, 1], i);
            }      
        }

        void SetProperties(OsbSprite sprite, int startTime, int endTime, int index)
        {
            sprite.Scale(startTime, ScreenScale);

            switch(index)
            {
                case 3:
                    HandleThree(sprite, startTime, endTime);
                    break;

                case 2:
                    HandleTwo(sprite, startTime, endTime);
                    break;

                default:
                    sprite.Fade(startTime, endTime, 1, 1);
                    break;
            }
        }

        void HandleTwo(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, 0.5);
            sprite.Rotate(startTime, endTime, 0, 0);
            foreach (var time in twoFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 1, 0.5); 
            }

            foreach (var time in twoFastFlashTimes)
            {
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, 0.7, 0.5);
            }
        }

        void HandleThree(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, 0.5);
            sprite.Rotate(startTime, endTime, 0, 0);
            double fastFlash = 0.6;
            foreach (var time in threeFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 1, 0.5); 
            }

            foreach (var time in threeFastFlashTimes)
            {
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, fastFlash, 0.5);
                fastFlash += 0.05;
            }
        }
    }
}
