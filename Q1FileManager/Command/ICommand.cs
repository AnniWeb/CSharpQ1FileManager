namespace Q1FileManager.Command
{
    public interface ICommand
    {
        public string GetCommand();

        public void Exec(string[] commandParams);
    }
}