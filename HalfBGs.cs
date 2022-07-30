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
            {44942, 71336},
            {71172, 92483},
            {92319, 113303},
            {113139, 134286},
            {134122, 160516},
            {160352, 192155},
            {191991, 226254},
            {226090, 257729},
            {257565, 270188}
        };
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
                SetProperties(sprites[i], times[i, 0], times[i, 1]);
            }      
        }

        void SetProperties(OsbSprite sprite, int startTime, int endTime)
        {
            sprite.Fade(startTime, endTime, 1, 1);
            sprite.Scale(startTime, ScreenScale);
        }
    }
}
