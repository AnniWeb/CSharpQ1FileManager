namespace Q1FileManager.View
{
    public interface IInfoPanel : IPanel
    {
        public void SetFile(string path);
        
        public void SetMsg(string msg, ConsoleView.Color type = ConsoleView.Color.MSG_INFO);
    }
}