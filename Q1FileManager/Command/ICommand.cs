namespace Q1FileManager.Command
{
    public interface ICommand
    {
        public delegate bool ForceDelegate (string text, string answer);
        
        public string GetCommand();

        public void Exec(string[] commandParams);
    }
}