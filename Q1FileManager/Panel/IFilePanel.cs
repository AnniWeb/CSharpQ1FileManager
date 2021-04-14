namespace Q1FileManager.View
{
    public interface IFilePanel : IPanel
    {
        public void ChangePath(string path);

        public void ScrollUp();
        public void ScrollDown();
        public void ScrollHome();
        public void ScrollEnd();
        public void ScrollPageUp();
        public void ScrollPageDown();
        public void ExecPath();
    }
}