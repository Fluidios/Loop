using UnityEngine;

public abstract class VisualRecognizer : MonoBehaviour
{
    public abstract bool TryRecognizeAndSaveToMemory(IVisuallyObservable observable, MemoryBlackboard memory);
}
