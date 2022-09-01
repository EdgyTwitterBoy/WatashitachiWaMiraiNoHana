using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Util;
using System;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class CustomParticles : StoryboardObjectGeneratorPlus
    {
        [Configurable]
        public string Path = "sb/particle.png";

        [Configurable]
        public int StartTime;

        [Configurable]
        public int EndTime;

        [Configurable]
        public int ParticleCount = 32;

        [Configurable]
        public Vector2 Scale = new Vector2(1, 1);

        [Configurable]
        public bool Rotate = false;
        [Configurable]
        public float Rotation = 0;
        [Configurable]
        public float RotateSpeed;

        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;

        [Configurable]
        public Color4 Color = Color4.White;

        [Configurable]
        public float ColorVariance = 0.6f;

        [Configurable]
        public bool Additive = false;

        [Configurable]
        public Vector2 SpawnOrigin = new Vector2(420, 0);

        [Configurable]
        public float SpawnSpread = 360;

        [Configurable]
        public float Angle = 110;

        [Configurable]
        public float AngleSpread = 60;

        [Configurable]
        public float Speed = 480;

        [Configurable]
        public float Lifetime = 1000;

        [Configurable]
        public OsbEasing Easing = OsbEasing.None;

        public override void Generate()
        {
            if (StartTime == EndTime && Beatmap.HitObjects.FirstOrDefault() != null)
            {
                StartTime = (int)Beatmap.HitObjects.First().StartTime;
                EndTime = (int)Beatmap.HitObjects.Last().EndTime;
            }
            EndTime = Math.Min(EndTime, (int)AudioDuration);
            StartTime = Math.Min(StartTime, EndTime);

            var bitmap = GetMapsetBitmap(Path);

            var duration = (double)(EndTime - StartTime);
            var loopCount = Math.Max(1, (int)Math.Floor(duration / Lifetime));

            var layer = GetLayer("CustomParticles"); 
            for (var i = 0; i < ParticleCount; i++)
            {
                var spawnAngle = Random(Math.PI * 2);
                var spawnDistance = (float)(SpawnSpread * Math.Sqrt(Random(1f)));

                var moveAngle = MathHelper.DegreesToRadians(Angle + Random(-AngleSpread, AngleSpread) * 0.5f);
                var moveDistance = Speed * Lifetime * 0.001f;

                var spriteRotation = moveAngle + MathHelper.DegreesToRadians(Rotation);

                var startPosition = SpawnOrigin + new Vector2((float)Math.Cos(spawnAngle), (float)Math.Sin(spawnAngle)) * spawnDistance;
                var endPosition = startPosition + new Vector2((float)Math.Cos(moveAngle), (float)Math.Sin(moveAngle)) * moveDistance;

                var loopDuration = duration / loopCount;
                var startTime = StartTime + (i * loopDuration) / ParticleCount;
                var endTime = startTime + loopDuration * loopCount;

                

                if (!isVisible(bitmap, startPosition, endPosition, (float)spriteRotation, (float)loopDuration))
                    continue;

                var color = Color;
                if (ColorVariance > 0)
                {
                    ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

                    var hsba = Color4.ToHsl(color);
                    var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
                    var sMax = Math.Min(sMin + ColorVariance, 1);
                    var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
                    var vMax = Math.Min(vMin + ColorVariance, 1);

                    color = Color4.FromHsl(new Vector4(
                        hsba.X,
                        (float)Random(sMin, sMax),
                        (float)Random(vMin, vMax),
                        hsba.W));
                }

                var particle = layer.CreateSprite(Path, Origin);
                particle.Fade(EndTime, 0);
                particle.Fade(StartTime, color.A);
                if(Rotate)
                    spriteRotation = MathHelper.DegreesToRadians(Random(0, 360));
                if (spriteRotation != 0)
                    particle.Rotate(startTime, spriteRotation);
                if (color.R != 1 || color.G != 1 || color.B != 1)
                    particle.Color(startTime, color);
                if (Scale.X != 1 || Scale.Y != 1)
                {
                    if (Scale.X != Scale.Y)
                        particle.ScaleVec(startTime, Scale.X, Scale.Y);
                    else particle.Scale(startTime, Scale.X);
                }
                if (Additive)
                    particle.Additive(startTime, endTime);

                particle.StartLoopGroup(startTime, loopCount);
                particle.Move(Easing, 0, loopDuration, startPosition, endPosition);
                if(Rotate) particle.Rotate(Easing, 0, loopDuration, spriteRotation, spriteRotation + MathHelper.DegreesToRadians(RotateSpeed));
                particle.EndGroup();
            }
        }

        private bool isVisible(Bitmap bitmap, Vector2 startPosition, Vector2 endPosition, float rotation, float duration)
        {
            var spriteSize = new Vector2(bitmap.Width * Scale.X, bitmap.Height * Scale.Y);
            var originVector = OsbSprite.GetOriginVector(Origin, spriteSize.X, spriteSize.Y);

            for (var t = 0; t < duration; t += 200)
            {
                var position = Vector2.Lerp(startPosition, endPosition, t / duration);
                if (OsbSprite.InScreenBounds(position, spriteSize, rotation, originVector))
                    return true;
            }
            return false;
        }
    }
}
