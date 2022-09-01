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
    public class CurrentMapper : StoryboardObjectGeneratorPlus
    {
        // 00:06:771 - Paradogi 			
        // 00:21:511 - MakiDonalds
        // 00:34:625 - zesteas
        // 00:44:953 - Kyairie
        // 01:00:844 - murada
        // 01:11:183 - CutoNaito
        // 01:21:838 - laplus
        // 01:32:166 - Cxk
        // 01:42:822 - Paradogi
        // 01:53:150 - 0ppInOsu
        // 02:03:806 - zesteas
        // 02:14:133 - laplus
        // 02:24:789 - MakiDonalds
        // 02:40:363 - Cxk
        // 02:51:019 - Rielle
        // 03:01:510 - Slifer
        // 03:12:002 - Halgoh
        // 03:24:953 - Murada
        // 03:35:609 - 0ppInOsu
        // 03:45:937 - Rielle
        // 03:56:592 - CutoNaito
        // 04:06:920 - Kyairie
        // 04:17:576 - Slifer


        public override void Generate()
        {
		    string[] partMappers = {"Paradogi", "MakiDonalds", "zesteas", "Kyairie", "murada", "CutoNaito",
                                    "laplus", "Cxk", "Paradogi", "0ppInOsu", "zesteas", "laplus", "Rielle", 
                                    "Cxk", "Slifer", "Halgoh", "Murada", "0ppInOsu", "Rielle", "CutoNaito", "Kyairie", "Slifer"};
            int[] partStartTimes = {6770, 21500, 34625, 44953, 60844, 71172, 81827, 92319, 102811,
                                    113139, 123795, 134122 , 144778, 160352, 171008, 191991,
                                    204942, 215598, 226090, 236581, 246909, 257565, 271008};

            PartPosition[] partPositions = {PartPosition.Left, PartPosition.Left, PartPosition.Left,
                                            PartPosition.Right, PartPosition.Right,
                                            PartPosition.Left, PartPosition.Left,
                                            PartPosition.Right, PartPosition.Right,
                                            PartPosition.Left, PartPosition.Left,
                                            PartPosition.Right, PartPosition.Right,
                                            PartPosition.Left, PartPosition.Left,
                                            PartPosition.Right, PartPosition.Left, PartPosition.Left,
                                            PartPosition.Right, PartPosition.Right, PartPosition.Right,
                                            PartPosition.Left};

            List<MapperPart> mapperParts = new List<MapperPart>();

            for (int i = 1; i < partMappers.Length; i++)
            {
                var layer = GetLayer(i + " - " + partMappers[i]);
                mapperParts.Add(new MapperPart(partStartTimes[i], partStartTimes[i + 1], partMappers[i], i, layer, partPositions[i], Beatmap, GetBeatDuration(Beatmap)));
            }
        }
    }

    public class MapperPart : StoryboardObjectGeneratorPlus
    {
        public int StartTime;
        public int EndTime;
        public string Mapper;
        public Action<OsbSprite, OsbSprite, PartPosition, int, int, float, float, float> Action;
        public int Index;
        public Vector2 LeftPosition = new Vector2(640 - 100, 420);
        public Vector2 RightPosition = new Vector2(80, 420);
        public float EndPositionOffset = 100;
        public PartPosition Position;
        public float Scale = 0.5f;
        public float MapperScale = 0.7f;
        public float BarScale = 1;
        private Beatmap beatmap;
        private double beatDuration;

        public MapperPart(int startTime, int endTime, string mapper, int index, StoryboardLayer layer, Action<OsbSprite, OsbSprite, PartPosition, int, int, float, float, float> action, PartPosition position)
        {
            StartTime = startTime;
            EndTime = endTime;
            Mapper = mapper;
            Action = action;
            Index = index;
            Position = position;
            
            var mapperBar = layer.CreateSprite("sb/mapperBar.png", OsbOrigin.Centre);
            var mapperName = layer.CreateSprite($"sb/names/{mapper}.png", OsbOrigin.Centre);

            Action(mapperBar, mapperName, position, startTime, endTime, Scale, MapperScale, BarScale);
        }

        public MapperPart(int startTime, int endTime, string mapper, int index, StoryboardLayer layer, PartPosition position, Beatmap beatmap, double beatDuration)
        {
            StartTime = startTime;
            EndTime = endTime;
            Mapper = mapper;
            Action = null;
            Action = DefaultAction;
            Index = index;
            Position = position;
            this.beatmap = beatmap;
            this.beatDuration = beatDuration;

            var mapperBar = layer.CreateSprite("sb/mapperBar.png", OsbOrigin.Centre);
            var mapperName = layer.CreateSprite($"sb/names/{mapper}.png", OsbOrigin.Centre);

            Action(mapperBar, mapperName, position, startTime, endTime, Scale, MapperScale, BarScale);
        }

        private void DefaultAction(OsbSprite mapperBar, OsbSprite mapperName, PartPosition position, int startTime, int endTime, float scale, float mapperScale, float barScale)
        {
            OsbSprite[] sprites = {mapperBar, mapperName};
            double effectStartTime = startTime + beatDuration / 2;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].MoveX(effectStartTime, position == PartPosition.Left ? LeftPosition.X : RightPosition.X);
                sprites[i].MoveY(effectStartTime, position == PartPosition.Left ? LeftPosition.Y : RightPosition.Y);
                sprites[i].Fade(effectStartTime, endTime + beatDuration, 1, 1);

                if(i == 1)
                {
                    sprites[i].Scale(OsbEasing.OutBack, effectStartTime, effectStartTime + beatDuration, 0, ScreenScale * scale * mapperScale);
                    sprites[i].Scale(OsbEasing.InBack, endTime - beatDuration / 2, endTime, ScreenScale * scale * mapperScale, 0);
                }
                else
                {
                    sprites[i].Scale(OsbEasing.OutBack, effectStartTime, effectStartTime + beatDuration, 0, ScreenScale * scale * barScale);
                    sprites[i].Scale(OsbEasing.InBack, endTime - beatDuration / 2, endTime, ScreenScale * scale * barScale, 0);
                }
            }
        }
        public override void Generate()
        {
            
        }
    }

    public enum PartPosition
    {
        Left,
        Right
    }
}
