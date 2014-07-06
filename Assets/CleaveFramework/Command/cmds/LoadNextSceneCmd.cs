namespace CleaveFramework.Commands
{
    class LoadNextSceneCmd : Command
    {
        public string SceneName { get; private set; }
        public LoadNextSceneCmd(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
