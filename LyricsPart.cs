using OpenTK;
using StorybrewCommon.Storyboarding;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class LyricsPart : StoryboardObjectGeneratorPlus
    {
        private double linesOffset = 30;
        private double smallLinesScale = 0.2;
        private double bigLinesScale = 0.35;
        private Vector2 positionLeft = new Vector2(MinimumDimensions.X + 210, 240);
        private Vector2 positionRight = new Vector2(MaximumDimensions.X - 210, 240);
        private Vector2 currentPosition;
        private List<LyricsLine> lines = new List<LyricsLine>();
        public override void Generate()
        {
		    var layer = GetLayer("Lyrics");
            currentPosition = positionLeft;
            
            // Create line objects and add them to the list
            for (int i = 0; i < LyricsTimes.GetLength(0); i++)
            {
                var line = layer.CreateSprite($"sb/lyrics/{i}.png", OsbOrigin.Centre);
                lines.Add(new LyricsLine(LyricsTimes[i,0], LyricsTimes[i,1], i, line));
            }

            // Draw lines
            for(int i = 0; i < lines.Count; i++)
            {
                // Check if this line needs custom handling
                if(CheckSpecial(i)) continue;

                OsbSprite line = lines[i].sprite;
                int startTime = lines[i - 1].startTime;
                int endTime = lines[i].endTime;

                // Set the position of the line according to the background splitter
                SetCurrentPos(startTime - GetBeatDuration(Beatmap));

                // Draw new line
                line.MoveX(startTime, currentPosition.X);
                line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
                line.Scale(startTime, smallLinesScale * ScreenScale);
                line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

                // Make the line active
                line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
                line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
                line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

                // Move the line down
                line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
                line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
                line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);

                // Check for position transition
                int changeTime = 0;
                if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
                {
                    // Move the line to the new position
                    Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                    line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
                }

                // Hide line
                line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
                line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);
            }

        }

        private void SetCurrentPos(double startTime)
        {
            int index = 0;

            while(ChangeTimes[index] < startTime)
            {
                index++;
            }

            currentPosition = index % 2 == 0 ? positionLeft : positionRight;
        }

        private bool CheckPosChange(double startTime, double endTime, out int changeTime)
        {
            for (int i = 0; i < ChangeTimes.Length; i++)
            {
                // Ugly way to check if there's a position change while the line is drawn on the screen
                if(Enumerable.Range((int)startTime, (int)endTime).Contains(ChangeTimes[i]))
                {
                    changeTime = ChangeTimes[i];
                    return true;
                }
            }

            changeTime = 0;
            return false;
        }

        private bool CheckSpecial(int index)
        {
            // Handle lines which need special code
            switch(index)
            {
                case 0:
                    HandleZero();
                    return true;
                
                case 1:
                    HandleOne();
                    return true;

                case 42:
                    HandleFortyTwo();
                    return true;

                case 16:
                    HandleSixteen();
                    return true;

                case 17:
                    HandleSeventeen();
                    return true;
                
                case 18:
                    HandleEighteen();
                    return true;

                case 19:
                    HandleNineteen();
                    return true;

                case 31:
                    HandleThirtyOne();
                    return true;

                case 32:
                    HandleThirtyTwo();
                    return true;

                case 33:
                    HandleThirtyThree();
                    return true;
                
                case 40:
                    HandleForty();
                    return true;

                case 41:
                    HandleFortyOne();
                    return true;

                default:
                    return false;
            }
        }

        void HandleNineteen()
        {
            int i = 19;

            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(startTime, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime, startTime + GetBeatDuration(Beatmap), 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);

            // Check for position transition
            int changeTime = 0;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);
        }

        void HandleEighteen()
        {
            int i = 18;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

             // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, positionRight.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            line.Fade(lines[i - 1].endTime - GetBeatDuration(Beatmap), lines[i - 1].endTime, 0.5, 0);

            // Make the line active
            line.MoveY(lines[i].startTime, currentPosition.Y);
            line.Scale(lines[i].startTime, bigLinesScale * ScreenScale);
            line.Fade(lines[i].startTime, lines[i].startTime + GetBeatDuration(Beatmap), 0, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutExpo, lines[i + 1].startTime - GetBeatDuration(Beatmap), lines[i + 1].startTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);
            //line.Fade(lines[i + 1].startTime, lines[i + 1].startTime + GetBeatDuration(Beatmap), 0, 0.5);

            int changeTime = 113139;
            Vector2 newPos = positionLeft;
            line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);

            // Check for position transition
            changeTime = 134122;

            // Move the line to the new position
            newPos = positionRight;
            line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);

            

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);
        }

        void HandleSeventeen()
        {
            int i = 17;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

             // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(lines[i + 1].startTime, currentPosition.Y + linesOffset);
            line.Scale(lines[i + 1].startTime, smallLinesScale * ScreenScale);

            line.Fade(lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0);

            line.Fade(lines[i + 1].startTime, lines[i + 1].startTime + GetBeatDuration(Beatmap), 0, 0.5);

            // Check for position transition
            int changeTime = 0;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);
        }

        void HandleSixteen()
        {
            
            int i = 16;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);

            // Check for position transition
            int changeTime = 0;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.Fade(115926, 116254, 0.5, 0);
        }

        void HandleZero()
        {
            OsbSprite line = lines[0].sprite;
            int startTime = lines[0].startTime;

            line.MoveX(startTime, currentPosition.X);
            line.MoveY(startTime, currentPosition.Y);
            line.Scale(startTime, bigLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 1);

            line.MoveY(OsbEasing.OutQuart, lines[0].endTime - GetBeatDuration(Beatmap), lines[0].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[0].endTime - GetBeatDuration(Beatmap), lines[0].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[0].endTime - GetBeatDuration(Beatmap), lines[0].endTime, 1, 0.5);

            line.MoveY(OsbEasing.OutQuart, lines[1].endTime - GetBeatDuration(Beatmap), lines[1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[1].endTime - GetBeatDuration(Beatmap), lines[1].endTime, 0.5, 0);

            int changeTime = 0;

            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[0].endTime, out changeTime))
            {
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }
        }

        void HandleOne()
        {
            OsbSprite line = lines[1].sprite;
            int startTime = lines[0].startTime;

            line.MoveX(startTime, currentPosition.X);
            line.MoveY(startTime, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            line.MoveY(OsbEasing.OutQuart, lines[1].startTime - GetBeatDuration(Beatmap), lines[1].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[1].startTime - GetBeatDuration(Beatmap), lines[1].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[1].startTime - GetBeatDuration(Beatmap), lines[1].startTime, 0.5, 1);

            line.MoveY(OsbEasing.OutQuart, lines[1].endTime - GetBeatDuration(Beatmap), lines[1].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[1].endTime - GetBeatDuration(Beatmap), lines[1].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[1].endTime - GetBeatDuration(Beatmap), lines[1].endTime, 1, 0.5);

            line.MoveY(OsbEasing.OutQuart, lines[2].endTime - GetBeatDuration(Beatmap), lines[2].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[2].endTime - GetBeatDuration(Beatmap), lines[2].endTime, 0.5, 0);

            int changeTime = 0;

            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[1].endTime, out changeTime))
            {
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }
        }

        void HandleThirtyOne()
        {
            int i = 31;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);

            // Check for position transition
            int changeTime = 0;
            int changeTime2 = 204942;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);

            //Custom script
            line.Fade(184122, 0); 
            line.Fade(204122, 204450, 0, 0.5);

            Vector2 newPos2 = line.PositionAt(changeTime2).X == positionLeft.X ? positionRight : positionLeft;
            line.MoveX(OsbEasing.OutExpo, changeTime2, changeTime2 + GetBeatDuration(Beatmap), line.PositionAt(changeTime2).X, newPos2.X);
        }

        void HandleThirtyTwo()
        {
            int i = 32;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);

            // Check for position transition
            int changeTime = 0;
            int changeTime2 = 204942;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);

            //Custom script
            line.Fade(184122, 0);
            line.Fade(204122, 204450, 0, 1);
            // Move the line to the new position
            Vector2 newPos2 = line.PositionAt(changeTime2).X == positionLeft.X ? positionRight : positionLeft;
            line.MoveX(OsbEasing.OutExpo, changeTime2, changeTime2 + GetBeatDuration(Beatmap), line.PositionAt(changeTime2).X, newPos2.X);
        }

        void HandleThirtyThree()
        {
            int i = 33;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(204122, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(204122, 204450, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, 1, 0.5);

            // Check for position transition
            int changeTime = 0;
            if(CheckPosChange(startTime - GetBeatDuration(Beatmap), lines[i].endTime, out changeTime))
            {
                // Move the line to the new position
                Vector2 newPos = line.PositionAt(changeTime).X == positionLeft.X ? positionRight : positionLeft;
                line.MoveX(OsbEasing.OutExpo, changeTime, changeTime + GetBeatDuration(Beatmap), line.PositionAt(changeTime).X, newPos.X);
            }

            // Hide line
            line.MoveY(OsbEasing.OutQuart, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, currentPosition.Y + linesOffset, currentPosition.Y + linesOffset * 2);
            line.Fade(OsbEasing.Out, lines[i + 1].endTime - GetBeatDuration(Beatmap), lines[i + 1].endTime, 0.5, 0);
        }

        void HandleFortyTwo()
        {
            int i = 42;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // // Draw new line
            line.MoveX(lines[i].startTime - GetBeatDuration(Beatmap), currentPosition.X);
            // line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            // line.Scale(startTime, smallLinesScale * ScreenScale);
            // line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            line.Fade(254942, 257565, 1, 0);
        }

        void HandleForty()
        {
            int i = 40;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),lines[i].startTime, 0.5, 1);

            // Move the line down
            line.MoveY(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, currentPosition.Y, currentPosition.Y + linesOffset);
            line.Scale(OsbEasing.OutQuart, lines[i].endTime - GetBeatDuration(Beatmap), lines[i].endTime, bigLinesScale * ScreenScale, smallLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].endTime - GetBeatDuration(Beatmap), 245844, 1, 0.5);

            line.Fade(245844, 249696, 0.5, 0.5);
            line.Fade(249696, 250680, 0.5, 0);
        }

        void HandleFortyOne()
        {
            int i = 41;
            OsbSprite line = lines[i].sprite;
            int startTime = lines[i - 1].startTime;
            int endTime = lines[i].endTime;

            // Set the position of the line according to the background splitter
            SetCurrentPos(startTime - GetBeatDuration(Beatmap));

            // Draw new line
            line.MoveX(startTime, currentPosition.X);
            line.MoveY(OsbEasing.OutQuart, startTime - GetBeatDuration(Beatmap), startTime, currentPosition.Y - linesOffset * 2, currentPosition.Y - linesOffset);
            line.Scale(startTime, smallLinesScale * ScreenScale);
            line.Fade(startTime - GetBeatDuration(Beatmap), startTime, 0, 0.5);

            // Make the line active
            line.MoveY(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, currentPosition.Y - linesOffset, currentPosition.Y);
            line.Scale(OsbEasing.OutQuart, lines[i].startTime - GetBeatDuration(Beatmap), lines[i].startTime, smallLinesScale * ScreenScale, bigLinesScale * ScreenScale);
            line.Fade(OsbEasing.Out, lines[i].startTime - GetBeatDuration(Beatmap),245844, 0.5, 1);

            line.Fade(245844, 249696, 1, 1);
            line.Fade(249696, 250680, 1, 0);
        }

        public class LyricsLine
        {
            public int startTime;
            public int endTime;
            public int index;

            public OsbSprite sprite;

            public LyricsLine(int startTime, int endTime, int index, OsbSprite sprite)
            {
                this.startTime = startTime;
                this.endTime = endTime;
                this.index = index;
                this.sprite = sprite;
            }
        }
    }
}
