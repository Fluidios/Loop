public class CharacterRecognizer : VisualRecognizer
{
    public override bool TryRecognizeAndSaveToMemory(IVisuallyObservable observable, MemoryBlackboard memory)
    {
        if (observable.TryScan<Character>(out var observedCharacter))
        {
            memory.Set(observedCharacter.name, observedCharacter);
            return true;
        }
        else
            return false;
    }
}
