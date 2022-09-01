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
        private int[,] times = new int[11, 2] {
            {21500, 45106},
            {44942, 70844},
            {71172, 92483},
            {92319, 113303},
            {113139, 134286},
            {134122, 160516},
            {160352, 192155},
            {191991, 205106},
            {204942, 226254},
            {226090, 257729},
            {257565, 270680}
        };

        //3
        private int[] threeFlashTimesPlus = {101336, 101991, 102811, 107401, 103139};
        private int[] threeFastFlashTimes = {110680, 110844, 111008, 111172, 111336, 111500, 111663, 111827, 101172};

        //9
        private int[] nineFlashTimesPlus = {235106, 235762, 236581, 241827, 243139};
        private int[] nineFastFlashTimes = {244450, 244614, 244778, 244942, 245106, 245270, 245434, 245598, 245762, 245926,
                                            246254, 246418, 246745, 234942};
        private int[] nineSpecialFlashTimesFast = {251172, 251336, 251500, 251991, 252483};
        private int[] nineSpecialFlashTimes = {251663, 252155};
        
        //2
        private int[] twoFlashTimes = {71172, 72483, 73795, 75106, 76581, 77729, 79204, 80352, 81827, 83139, 84450, 84942, 85434,
                                       87073, 88221, 89696, 91008, 91336};
        private int[] twoFastFlashTimes = {91663, 91827, 91991, 92155, 92319};

        //6
        private int[] sixFlashTimes = {160352, 161663, 162975, 164286, 165762, 166909, 168385, 169532, 171008, 172319, 173631,
                                       174122, 174614, 176254, 177401, 178877, 180188, 180516};
        private int[] sixFastFlashTimes = {180844, 181008, 181172, 181336};

        //8
        private int[] eightFlashTimes = {204942, 206254, 207565, 208877, 210352, 211500, 212975, 214122, 215598, 216909, 218221,
                                         218713, 219204, 220844, 221991, 223467, 224778, 225106};
        private int[] eightFastFlashTimes = {225434, 225598, 225762, 225926};
        private List<double> threeFlashTimes = new List<double>();
        private List<double> nineFlashTimes = new List<double>();

        public override void Generate()
        {
            StoryboardLayer[] layers = new StoryboardLayer[11];
            OsbSprite[] sprites = new OsbSprite[11];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = GetLayer("HalfBG " + i);
                sprites[i] = layers[i].CreateSprite($"sb/halfBGs/{i}.png", 
                                                    i % 2 == 0 ? OsbOrigin.CentreRight : OsbOrigin.CentreLeft,
                                                    i % 2 == 0 ? rightPosition : leftPosition);
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
            var ten = layer.CreateSprite("sb/halfBGs/10.png", OsbOrigin.CentreRight, new Vector2(640 + 107, 240));

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

                case 6:
                    HandleSix(sprite, startTime, endTime);
                    break;

                case 8:
                    HandleEight(sprite, startTime, endTime);
                    break;

                case 9:
                    HandleNine(sprite, startTime, endTime);
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

        void HandleSix(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, 0.5);
            sprite.Rotate(startTime, endTime, 0, 0);
            foreach (var time in sixFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 1, 0.5); 
            }

            foreach (var time in sixFastFlashTimes)
            {
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, 0.7, 0.5);
            }
        }

        void HandleEight(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, 0.5);
            sprite.Rotate(startTime, endTime, 0, 0);
            foreach (var time in eightFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 1, 0.5); 
            }

            foreach (var time in eightFastFlashTimes)
            {
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, 0.7, 0.5);
            }
        }

        void HandleNine(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, 0.5);
            sprite.Rotate(startTime, endTime, 0, 0);
            double fastFlash = 0.6;

            foreach (var time in nineFlashTimesPlus)
            {
                nineFlashTimes.Add(time);
            }

            for (int i = 0; i < 13; i++)
            {
                nineFlashTimes.Add(226418 + GetBeatDuration(Beatmap) * 2 * i);
            }

            for (int i = 0; i < 8; i++)
            {
                nineFlashTimes.Add(236909 + GetBeatDuration(Beatmap) * 2 * i);
            }
            
            foreach (var time in nineFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 1, 0.5); 
            }

            foreach(var time in nineSpecialFlashTimes)
            {
                sprite.Flash(time, GetBeatDuration(Beatmap) - 1, 0.7, 0.5);
            }

            foreach(var time in nineSpecialFlashTimesFast)
            {
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, 0.7, 0.5);
            }

            foreach (var time in nineFastFlashTimes)
            {
                if(fastFlash > 1) fastFlash = 1;
                sprite.Flash(time, GetHalfBeatDuration(Beatmap) - 1, fastFlash, 0.5);
                fastFlash += 0.05;
            }

            sprite.Fade(246909, 247073, 0.5, 1);
            sprite.Fade(247073, 249368, 1, 1);
            sprite.Fade(249368, 249696, 1, 0.5);

            sprite.Fade(252647, 252975, 0.5, 1);
            sprite.Fade(252975, 254942, 1, 1);
            sprite.Fade(254942, 255270, 1, 0.5);
        }
    }
}
