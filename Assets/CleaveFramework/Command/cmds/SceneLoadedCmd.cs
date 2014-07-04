using CleaveFramework.Scene;

namespace CleaveFramework.Commands
{
    public class SceneLoadedCmd : Command
    {
        public SceneView View { get; private set; }
        public SceneLoadedCmd(SceneView view)
        {
            View = view;
        }
    }
}
