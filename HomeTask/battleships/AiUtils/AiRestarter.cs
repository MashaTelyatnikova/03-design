namespace battleships.AiUtils
{
    public class AiRestarter
    {
        private readonly Ai ai;
        private int crashLimit;

        public bool CrashLimitExceeded {get { return crashLimit <= 0; }}

        public AiRestarter(Ai ai, int crashLimit)
        {
            this.ai = ai;
            this.crashLimit = crashLimit;
        }

        public void RestartAi()
        {
            ai.Restart();
            crashLimit--;
        }
    }
}
