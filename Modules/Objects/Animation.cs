using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{
    public class Animation
    {
        public double EndTime;
        public double StartTime;
        public double Steps = 10;
        public double FullDuration = 250;
        public const double WalkDuration = 250;
        public AnimationCoordinates Position = new AnimationCoordinates();
        public int Id;
        public int PlayerId;

        public double OneStep = 0.1;

        public double StepDuration { get { return FullDuration / Steps; } set { } }

        public double CurrentStep = 1;

        public bool Finished = false;

        public double StartOffsetY;

        public string Text;

        public Animation()
        {
            EndTime = 0;
        }

        public Animation(double startTime, double endTime, double oneStep)
        {
            StartTime = startTime;
            EndTime = endTime;
            OneStep = oneStep;
            Finished = false;
            CurrentStep = 1;
            Steps = 10;
            FullDuration = 250;
        }

        public Animation(double startTime, double endTime, double oneStep, double stepsTotal, double durationOfAnimation)
        {
            StartTime = startTime;
            EndTime = endTime;
            OneStep = oneStep;
            Finished = false;
            CurrentStep = 1;
            Steps = stepsTotal;
            FullDuration = durationOfAnimation;
        }

        public Animation(double duration, double startOffsetY, double steps, double startTime, int playerId, string text = "", int id = 0, AnimationCoordinates pos = null)
        {
            Id = id;
            EndTime = startTime + duration;
            StartTime = startTime;
            Position = pos;
            Steps = steps;
            FullDuration = duration;
            StartOffsetY = startOffsetY;
            Text = text;
            PlayerId = playerId;
        }

        public double GetStep(double currentTime)
        {
            double currentStepEndTime = StartTime + FullDuration - ((Steps - CurrentStep) * StepDuration);
            if (currentTime >= currentStepEndTime)
            {
                if (CurrentStep < Steps)
                    CurrentStep++;
                else
                    Finished = true;

                return OneStep;
            }
            else
            {
                return 0;
            }
        }

        public double OffsetY(double currentTime)
        {
            double currentStepEndTime = StartTime + FullDuration - ((Steps - CurrentStep) * StepDuration);
            if (currentTime >= currentStepEndTime)
            {
                if (CurrentStep < Steps)
                    CurrentStep++;
            }
            return StartOffsetY - (CurrentStep * (StartOffsetY / Steps));
        }
    }
}
