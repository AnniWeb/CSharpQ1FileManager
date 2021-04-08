namespace Q1FileManager.View
{
    public interface IInfoPanel : IPanel
    {
        public void SetFile(string path);
        
        public void SetMsg(string msg, AConsolePanel.MessageType type = AConsolePanel.MessageType.INFO);
    }
}