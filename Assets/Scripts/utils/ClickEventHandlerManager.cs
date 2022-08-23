using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

class ClickEventHandlerToken: IDisposable
{
    public VisualElement element;
    public EventCallback<ClickEvent> eventHandler;

    public void Dispose() => element.UnregisterCallback(eventHandler);
}

public class ClickEventHandlerManager
{ 
    private List<IDisposable> _registeredTokens = new List<IDisposable>();
    
    public void Register(VisualElement visualElement, EventCallback<ClickEvent> eventHandler)
    {
        visualElement.RegisterCallback(eventHandler);
        _registeredTokens.Add(new ClickEventHandlerToken { element = visualElement, eventHandler = eventHandler });
    }
    
    public void UnregisterAll()
    {
        _registeredTokens.ForEach(token => token.Dispose());
        _registeredTokens.Clear();
    }
}