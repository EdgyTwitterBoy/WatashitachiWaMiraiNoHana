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
    public class UmiRotate : StoryboardObjectGeneratorPlus
    {
        [Configurable] public double scale;
        [Configurable] public Vector2 pos;
        [Configurable] public double startTime;
        [Configurable] public double fps;
        public override void Generate()
        {
		    var layer = GetLayer("umiRotate");
            var frameLayer = GetLayer("umiRotateFrame");
            var frameSprite = frameLayer.CreateSprite("sb/frame.png");

            double frameDuration = 1000 / fps;
            double frameTime = startTime;

            frameSprite.Scale(startTime, 21500, ScreenScale * scale, ScreenScale * scale);



            for (int i = 80; i < 300; i++)
            {
                if(frameTime > 21500) break;
                var frame = layer.CreateSprite($"sb/rotat/{i}.png", OsbOrigin.Centre, pos);
                frame.Fade(frameTime, frameTime + frameDuration, 1, 1);
                frame.Scale(frameTime, scale * ScreenScale);
                frameTime += frameDuration;
            }
        }
    }
}
