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
    public class HitObjectFollow : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath = "sb/glow.png";
        
        [Configurable]
        public double StartTime = 0;

        [Configurable]
        public int EndTime = 0;

        [Configurable]
        public double Scale = 1;

        [Configurable]
        public bool Rotate = false;

        [Configurable]
        public int TimingPoint;

        [Configurable]
        public double RotateSpeed = 1;

        [Configurable]
        public int BeatDivisor = 8;

        [Configurable]
        public bool Bounce = false;

        [Configurable]
        public double BounceBeatDivisor = 1;

        [Configurable]
        public double BounceScale = 1;
        
        public int ArrayLength(double StartTime, int EndTime)
        {
            int ArrayLength = 0;
            
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject.StartTime >= StartTime && hitobject.StartTime <= EndTime)
                {
                    if(hitobject is OsuSlider){
                        ArrayLength =  ArrayLength + BeatDivisor - 1;
                    }
                    ArrayLength++;
                }
            }

            return ArrayLength;
        }

        public override void Generate()
        {
		    
            var layer = GetLayer("");

            var sprite = layer.CreateSprite(SpritePath, OsbOrigin.Centre);

            var beatDuration = Beatmap.GetTimingPointAt(TimingPoint).BeatDuration;
            
            sprite.Scale(StartTime, Scale);

            Vector2[] positions = new Vector2[ArrayLength(StartTime, EndTime)];
            double[] startTimes = new double[ArrayLength(StartTime, EndTime)];
            int i = 0;

            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject.StartTime >= StartTime && hitobject.StartTime <= EndTime)
                {
                    if(hitobject is OsuSlider){
                        var timestep = (hitobject.EndTime - hitobject.StartTime)/BeatDivisor;
                        var time = hitobject.StartTime;
                        for(int a = 0; a < BeatDivisor; a++){
                            positions[i] = hitobject.PositionAtTime(time);
                            startTimes[i] = time;
                            i++;
                            time = time + timestep;
                        }
                    }
                    else{
                            positions[i] = hitobject.Position;
                            startTimes[i] = hitobject.StartTime; 
                            i++;
                    }
                }
            }
            
            Vector2 currentPosition = new Vector2();
            Vector2 newPosition = new Vector2();

            var positionStartTime = 0.0;
            var positionEndTime = 0.0;

                for (int a = 0; a < i - 1; a++)
                {
                    currentPosition = positions[a];
                    newPosition = positions[a + 1];
                    positionStartTime = startTimes[a];
                    positionEndTime = startTimes[a + 1];

                    sprite.Move(positionStartTime, positionEndTime, currentPosition.X, currentPosition.Y, newPosition.X, newPosition.Y);
                }

            if (Rotate)
            {
                var RotateStartTime = StartTime;
                var currentRotate = 0.0;

                while (RotateStartTime < EndTime)
                {
                    sprite.Rotate(RotateStartTime, RotateStartTime + beatDuration, currentRotate, currentRotate + 1 * RotateSpeed);

                    currentRotate = currentRotate + 1 * RotateSpeed;
                    RotateStartTime = RotateStartTime + beatDuration;
                }
            }

            if (Bounce)
            {
                var BounceSpeed = beatDuration * BounceBeatDivisor;
                var BounceStartTime = StartTime;
                
                while (BounceStartTime < EndTime)
                {
                    sprite.Scale(BounceStartTime, BounceStartTime + BounceSpeed / 2, BounceScale * Scale, Scale);
                    sprite.Scale(BounceStartTime + BounceSpeed / 2, BounceSpeed, Scale, Scale);

                    BounceStartTime = BounceStartTime + BounceSpeed;

                }

            }

        }
    }
}
