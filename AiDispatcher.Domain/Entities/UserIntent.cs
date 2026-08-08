using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Domain.Entities;

public class UserIntent
{
    public IntentType IntentType { get; private set; }
    
    public void GetIntent(object arg)
    {
        if (arg is byte[])
        {
            IntentType = IntentType.Image;
            return;
        }

        IntentType = IntentType.Reference;
    }
}