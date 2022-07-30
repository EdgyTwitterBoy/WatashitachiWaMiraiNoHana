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
    public class IntroInfo : StoryboardObjectGeneratorPlus
    {
        private Vector2 GeneralPositionSongInfo = new Vector2(90, 180);
        private Vector2 umiKanjiPosition = new Vector2(0, 0);
        private int umiKanjiStartTime = 21827;
        private Vector2 umiRomanjiPosition = new Vector2(0, 30);
        private Vector2 songNameKanjiPosition = new Vector2(0, 100 );
        private Vector2 songNameRomanjiPosition = new Vector2(0, 30);
        private Vector2 GeneralPositionMapInfo = new Vector2(90, 180);
        private Vector2 mapNameKanjiPosition = new Vector2(0, 0);
        private StoryboardLayer layer;

        private Vector2 GeneralPositionMappers = new Vector2(95, 150);
        private Vector2 mapperOffset = new Vector2(130, 40);
        private double bigScale = 0.5;
        private double smallScale = 0.3;


        private Vector2 GeneralPositionOtherInfo = new Vector2(90, 180);
        private Vector2 zesteasOffset = new Vector2(0, 30);
        private Vector2 storyboardOffset = new Vector2(0, 80);
        private Vector2 daliborOffset = new Vector2(0, 35);	

        public override void Generate()
        {
            layer = GetLayer("IntroInfo");

            DrawSongInfo();
            DrawMapInfo();
        }

        private void DrawMapInfo()
        {
            var mappersText = layer.CreateSprite("sb/otherText/mappers.png", OsbOrigin.Centre, new Vector2(110, 80));
            mappersText.Scale(26745, ScreenScale * bigScale);
            SetFade(mappersText, 26745, GetEndTime(26745));

            var storyboard = layer.CreateSprite("sb/otherText/storyboard.png", OsbOrigin.Centre, GeneralPositionOtherInfo + storyboardOffset);
            var hitsounds = layer.CreateSprite("sb/otherText/hitsounds.png", OsbOrigin.Centre, GeneralPositionOtherInfo);
            var dalibor = layer.CreateSprite("sb/names/dalibor.png", OsbOrigin.Centre, GeneralPositionOtherInfo + storyboardOffset + daliborOffset);
            var zesteas = layer.CreateSprite("sb/names/zesteas.png", OsbOrigin.Centre, GeneralPositionOtherInfo + zesteasOffset);

            OsbSprite[] otherInfo = { hitsounds, zesteas, storyboard, dalibor };

            double otherStartTime = 31991;
            bool isBig = true;

            foreach (var text in otherInfo)
            {
                if(isBig)
                {
                    text.Scale(otherStartTime, ScreenScale * bigScale);
                    SetFade(text, otherStartTime, GetOtherEndTime(otherStartTime));
                }
                else
                {
                    text.Scale(otherStartTime, ScreenScale * smallScale);
                    SetFade(text, otherStartTime, GetOtherEndTime(otherStartTime));
                }
                
                isBig = !isBig;
                otherStartTime += GetHalfBeatDuration(Beatmap);
            }

            OsbSprite[,] mappers = GetMappersArray();
            double mapperTime = 26827;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    OsbSprite mapper = mappers[j, i];
                    mapper.Scale(mapperTime, smallScale * ScreenScale);
                    mapper.Move(mapperTime, GeneralPositionMappers.X + mapperOffset.X * j, GeneralPositionMappers.Y + mapperOffset.Y * i);
                    SetFade(mapper, mapperTime, GetEndTime(mapperTime));
                }

                mapperTime += GetHalfBeatDuration(Beatmap);
            }
        }

        private double GetOtherEndTime(double time)
        {
            return time + GetBeatDuration(Beatmap) * 5.5;
        }

        private OsbSprite[,] GetMappersArray()
        {
            List<string> mappersNames = Mappers.ToList();
            mappersNames.Sort();

            int row = 0;
            int collumn = 0;

            OsbSprite[,] mappersArray = new OsbSprite[2, 6];

            foreach (var name in mappersNames)
            {
                OsbSprite mapper = layer.CreateSprite($"sb/names/{name}.png", OsbOrigin.CentreRight);
                mappersArray[row, collumn] = mapper;
                row++;

                if(row > 1)
                {
                    row = 0;
                    collumn++;
                }
            }

            return mappersArray;
        }

        private void DrawSongInfo()
        {
            double umiRomanjiStarTime = umiKanjiStartTime + GetHalfBeatDuration(Beatmap);
            double songNameKanjiStartTime = umiRomanjiStarTime + GetHalfBeatDuration(Beatmap);
            double songNameRomanjiStartTime = songNameKanjiStartTime + GetHalfBeatDuration(Beatmap);

            var umiKanji = layer.CreateSprite("sb/otherText/umiSonodaKanji.png", OsbOrigin.Centre, umiKanjiPosition + GeneralPositionSongInfo);
            var umiRomanji = layer.CreateSprite("sb/otherText/umiSonodaRomanji.png", OsbOrigin.Centre, umiRomanjiPosition + GeneralPositionSongInfo + umiKanjiPosition);
            var songNameKanji = layer.CreateSprite("sb/otherText/songNameKanji.png", OsbOrigin.Centre, songNameKanjiPosition + GeneralPositionSongInfo);
            var songNameRomanji = layer.CreateSprite("sb/otherText/songNameRomanji.png", OsbOrigin.Centre, songNameRomanjiPosition + GeneralPositionSongInfo + songNameKanjiPosition);

            umiKanji.Scale(umiKanjiStartTime, ScreenScale * bigScale);
            SetFade(umiKanji, umiKanjiStartTime, GetEndTime(umiKanjiStartTime));

            umiRomanji.Scale(umiRomanjiStarTime, ScreenScale * smallScale);
            SetFade(umiRomanji, umiRomanjiStarTime, GetEndTime(umiRomanjiStarTime));
            
            songNameKanji.Scale(songNameKanjiStartTime, ScreenScale * bigScale);
            SetFade(songNameKanji, songNameKanjiStartTime, GetEndTime(songNameKanjiStartTime));

            songNameRomanji.Scale(songNameRomanjiStartTime, ScreenScale * smallScale);
            SetFade(songNameRomanji, songNameRomanjiStartTime, GetEndTime(songNameRomanjiStartTime));
        }

        private void SetFade(OsbSprite sprite, double startTime, double endTime)
        {
            sprite.Fade(startTime, startTime + GetBeatDuration(Beatmap) * 2, 0, 1);
            sprite.Fade(startTime + GetBeatDuration(Beatmap) * 2, endTime, 1, 1);
            sprite.Fade(endTime, endTime + GetBeatDuration(Beatmap), 1, 0);
        }

        private double GetEndTime(double time)
        {
            return time + GetBeatDuration(Beatmap) * 12;
        }
    }
}
