namespace Macabresoft.GuitarTuner.Desktop;

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

public class ViewLocator : IDataTemplate {
    public IControl Build(object data) {
        var name = data.GetType().FullName?.Replace("ViewModel", "View");
        if (name != null) {
            var type = Type.GetType(name);

            if (type != null && Activator.CreateInstance(type) is IControl control) {
                return control;
            }
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data) {
        return data is ReactiveObject;
    }
}