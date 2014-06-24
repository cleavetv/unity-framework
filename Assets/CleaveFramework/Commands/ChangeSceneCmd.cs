namespace CleaveFramework.Commands
{
    public class ChangeSceneCmd : Command
    {
        public string SceneName { get; private set; }
        public ChangeSceneCmd(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
