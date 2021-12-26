namespace Macabresoft.GuitarTuner.UI.Common;

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;

public class TuneToNoteButtonControl : UserControl {
    public static readonly StyledProperty<Note?> NoteProperty =
        AvaloniaProperty.Register<TuneToNoteButtonControl, Note?>(nameof(Note));

    public static readonly StyledProperty<Note?> SelectedNoteProperty =
        AvaloniaProperty.Register<TuneToNoteButtonControl, Note?>(nameof(SelectedNote));

    public static readonly StyledProperty<ICommand> SelectNoteCommandProperty =
        AvaloniaProperty.Register<TuneToNoteButtonControl, ICommand>(nameof(SelectNoteCommand));

    public TuneToNoteButtonControl() {
        this.InitializeComponent();
    }

    public Note? Note {
        get => this.GetValue(NoteProperty);
        set => this.SetValue(NoteProperty, value);
    }

    public Note? SelectedNote {
        get => this.GetValue(SelectedNoteProperty);
        set => this.SetValue(SelectedNoteProperty, value);
    }

    public ICommand SelectNoteCommand {
        get => this.GetValue(SelectNoteCommandProperty);
        set => this.SetValue(SelectNoteCommandProperty, value);
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}