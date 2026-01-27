using System.Collections;

public interface ISceneModule
{
    AppSceneState SceneType { get; }

    IEnumerator Enter();
    IEnumerator Exit();
}
