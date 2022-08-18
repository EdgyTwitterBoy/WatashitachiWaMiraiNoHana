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
    public class PreKiaiOne : StoryboardObjectGeneratorPlus
    {
        private Vector2 magicPatternLeftPosition = new Vector2(ScreenMiddle.X - 150, 100);
        private float magicPatternDistance = 300;
        public override void Generate()
        {
		    var layer = GetLayer("PreKiaiOne");
            var blackTop = layer.CreateSprite("sb/pixelBlack.png", OsbOrigin.BottomCentre);
            var blackBottom = layer.CreateSprite("sb/pixelBlack.png", OsbOrigin.TopCentre);
            var magicPatternLeft = layer.CreateSprite("sb/magicPattern.png");
            var magicPatternRight = layer.CreateSprite("sb/magicPattern.png");
            var magicPatternBottom = layer.CreateSprite("sb/magicPattern.png");
            var umiTransparent = layer.CreateSprite("sb/2_transparent.png");
            var white = layer.CreateSprite("sb/pixelWhite.png");

            Vector2 magicPatternRightPosition = new Vector2(magicPatternLeftPosition.X + magicPatternDistance, magicPatternLeftPosition.Y);
            Vector2 magicPatternBottomPosition = new Vector2(magicPatternLeftPosition.X + magicPatternDistance / 2, magicPatternLeftPosition.Y + magicPatternDistance * (float)Math.Sin(DegToRad(60)));

            blackTop.ScaleVec(68057, ScreenScale * 1920, ScreenScale * 1080);
            blackTop.MoveY(OsbEasing.Out, 68057, 68549, 0, MaximumDimensions.Y / 2);
            blackTop.Fade(68057, 71172, 1, 1);

            blackBottom.ScaleVec(68057, ScreenScale * 1920, ScreenScale * 1080);
            blackBottom.MoveY(OsbEasing.Out, 68057, 68549, MaximumDimensions.Y, MaximumDimensions.Y / 2);
            blackBottom.Fade(68057, 71172, 1, 1);

            white.Fade(70188, 70516, 0, 1);
            white.ScaleVec(70188, ScreenScale * 1920, ScreenScale * 1080);
            white.Fade(70516, 71172, 1, 0);

            umiTransparent.Scale(70516, 71172, ScreenScale, ScreenScale);
            umiTransparent.MoveX(OsbEasing.OutQuad, 70516, 71172, ScreenMiddle.X - 50, ScreenMiddle.X + 138);
            

            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 360);

            magicPatternLeft.Move(68713, magicPatternLeftPosition);
            magicPatternLeft.Scale(OsbEasing.OutBack, 68713, 68713 + GetHalfBeatDuration(Beatmap), 0, ScreenScale);
            magicPatternLeft.Fade(68713, 70516, 1, 1);
            magicPatternLeft.Move(OsbEasing.OutQuart, 70024, 70024 + GetBeatDuration(Beatmap), magicPatternLeftPosition, ScreenMiddle);
            magicPatternLeft.Rotate(68713, 70024, DegToRad(randomNumber), DegToRad(randomNumber + 60));
            magicPatternLeft.Rotate(70024, 70024 + GetHalfBeatDuration(Beatmap), DegToRad(randomNumber + 60), 0);
            magicPatternLeft.Rotate(OsbEasing.In, 70188, 70516, 0, DegToRad(180));
            magicPatternLeft.Scale(OsbEasing.In, 70188, 70516, ScreenScale, ScreenScale * 10);
            
            randomNumber = random.Next(0, 360);

            magicPatternRight.Move(69204, magicPatternRightPosition);
            magicPatternRight.Scale(OsbEasing.OutBack, 69204, 69204 + GetHalfBeatDuration(Beatmap), 0, ScreenScale);
            magicPatternRight.Fade(69204, 70516, 1, 1);
            magicPatternRight.Move(OsbEasing.OutQuart, 70024, 70024 + GetBeatDuration(Beatmap), magicPatternRightPosition, ScreenMiddle);
            magicPatternRight.Rotate(69204, 70024, DegToRad(randomNumber), DegToRad(randomNumber +  60 * 0.62));
            magicPatternRight.Rotate(70024, 70024 + GetHalfBeatDuration(Beatmap), DegToRad(randomNumber + 60 * 0.62), 0);
            magicPatternRight.Rotate(OsbEasing.In, 70188, 70516, 0, DegToRad(180));
            magicPatternRight.Scale(OsbEasing.In, 70188, 70516, ScreenScale, ScreenScale * 10);
            
            randomNumber = random.Next(0, 360);

            magicPatternBottom.Move(69696, magicPatternBottomPosition);
            magicPatternBottom.Scale(OsbEasing.OutBack, 69696, 69696 + GetHalfBeatDuration(Beatmap), 0, ScreenScale);
            magicPatternBottom.Fade(69696, 70516, 1, 1);
            magicPatternBottom.Move(OsbEasing.OutQuart, 70024, 70024 + GetBeatDuration(Beatmap), magicPatternBottomPosition, ScreenMiddle);
            magicPatternBottom.Rotate(69696, 70024, DegToRad(randomNumber), DegToRad(randomNumber + 60 * 0.25));
            magicPatternBottom.Rotate(70024, 70024 + GetHalfBeatDuration(Beatmap), DegToRad(randomNumber + 60 * 0.25), 0);
            magicPatternBottom.Rotate(OsbEasing.In, 70188, 70516, 0, DegToRad(180));
            magicPatternBottom.Scale(OsbEasing.In, 70188, 70516, ScreenScale, ScreenScale * 10);
        }
    }
}
