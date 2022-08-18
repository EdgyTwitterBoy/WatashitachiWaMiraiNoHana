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
        public static int[] ChangeTimes = {44942, 70844, 92319, 113139, 134122, 160352, 191991, 226090, 257565};
        protected static int[,] LyricsTimes = {{34942,  40188},                     // 0 Hi ni mukaitai to negau hana
                                                {40188,  45434},                    // 1 Kokoro ni ichirin aru deshou
                                                {45434, 49696},                     // 2 Daiji ni shinagara sorezore o
                                                {49696, 51336},                     // 3 Chigau michi ga 
                                                {51336,54614},                      // 4 ima mattete mo
                                                {54614, 60844},                     // 5 Negai o kanaeru to ashita ni habataku
                                                {60844, 63467},                     // 6 Yuuki ga yobu kireina yume
                                                {63467, 69040},                     // 7 Nee... tomo ni mite itai
                                                {70680, 73795},                     // 8 Kimi yo saite atsui kibou no hate
                                                {73795, 76581},                     // 9 Tabidatsu kono sadame yo
                                                {76581, 81172},                     // 10 kagayaki wa kaze no kanata
                                                {81172, 84450},                     // 11 Itsuka kotae ga michite kuru
                                                {84450, 91663},                     // 12 Yasashisa o wasurenu you ni sakimashou ka
                                                {91663, 96909},                     // 13 Soshite watashitachi wa meguriau
                                                {96909, 101991},                    // 14 Akaku akaku ookina hana
                                                {101991, 107401},                   // 15 Soshite watashitachi wa meguriau
                                                {107401, 111991},                   // 16 Futatabi aeta toki wa
                                                {111991, 116254},                   // 17 kawaru hazu deshou? 
                                                {123795, 129368},                   // 18 Ji ni nezashitai to omou hito
                                                {129368, 134614},                   // 19 Karada wa fukarete mau deshou
                                                {134614, 139040},                   // 20 Yurarete minagara ono ono de
                                                {139040, 140516},                   // 21 Erabu michi ga
                                                {140516, 143795},                   // 22 tada nobiteru no
                                                {143795, 150024},                   // 23 Kanarazu modoru to wa iwanakute wakaru
                                                {150024, 152647},                   // 24 Genki de ite tashika na yume
                                                {152647, 157893},                   // 25 Nee... watashi ni mo mieru!
                                                {159860, 162975},                   // 26 Kimi ga fureta tsuyoi itami ni naku
                                                {162975, 165762},                   // 27 Kinou wa mou sayonara
                                                {165762, 170352},                   // 28 iradachi wa touku natte
                                                {170352, 173631},                   // 29 Doko ni mukaeba ii no ka wa
                                                {173631, 180516},                   // 30 Setsunasa ga kanjita basho ni ikimashou ka
                                                {180516, 184122},                   // 31 Sayonara
                                                {204450, 207565},                   // 32 Kimi yo saite atsui kibou no hate
                                                {207565, 210352},                   // 33 Tabidatsu kono sadame yo
                                                {210352, 214942},                   // 34 kagayaki wa kaze no kanata
                                                {214942, 218221},                   // 35 Itsuka kotae ga michite kuru
                                                {218221, 225434},                   // 36 Yasashisa o wasurenu you ni sakimashou ka
                                                {225434, 230680},                   // 37 Soshite watashitachi wa katariau
                                                {230680, 235926},                   // 38 Akai akai mirai no hana
                                                {235926, 241172},                   // 39 Soshite watashitachi wa katariau
                                                {241172, 245926},                   // 40 Futatabi aeta toki wa 
                                                {245926, 249696},                   // 41 kawaru hazu deshou?
                                                {251172, 254942}};                  // 42 Atarashii futari ni
    }

    public static class AddOns
    {
        public static void Flash(this OsbSprite sprite, double startTime, double duration, double flashPower = 1, double flashFinalState = 0)
        {
            if(duration <= 0) return;

            sprite.Fade(startTime, startTime + duration, flashPower, flashFinalState);
        }

        public static void RandomMovement(this OsbSprite sprite, double startTime, double endTime, double speed, Vector2 maxDistance, Vector2 origin, OsbEasing easing = OsbEasing.None)
        {
            if(startTime >= endTime) return;

            double currentTime = startTime;
            System.Random random = new System.Random();

            while(currentTime < endTime)
            {
                int negative = random.Next(0, 2) == 0 ? 1 : -1;
                float newX = (float)random.NextDouble() * maxDistance.X * negative + origin.X;
                negative = random.Next(0, 2) == 0 ? 1 : -1;
                float newY = (float)random.NextDouble() * maxDistance.Y * negative + origin.Y;
                Vector2 currentPos = sprite.PositionAt(currentTime);

                sprite.Move(easing, currentTime, currentTime + speed, currentPos.X, currentPos.Y, newX, newY);

                currentTime += speed;
            }
            
        }
    }
}