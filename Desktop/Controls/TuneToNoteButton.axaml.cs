namespace Macabresoft.GuitarTuner.Desktop;

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;

public class TuneToNoteButton : UserControl {
    public static readonly StyledProperty<Note> NoteProperty =
        AvaloniaProperty.Register<TuneToNoteButton, Note>(nameof(Note), Note.Auto, defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<Note> SelectedNoteProperty =
        AvaloniaProperty.Register<TuneToNoteButton, Note>(nameof(SelectedNote), defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<ICommand> SelectNoteCommandProperty =
        AvaloniaProperty.Register<TuneToNoteButton, ICommand>(nameof(SelectNoteCommand), defaultBindingMode: BindingMode.OneWay);

    public TuneToNoteButton() {
        this.InitializeComponent();
    }

    public Note Note {
        get => this.GetValue(NoteProperty);
        set => this.SetValue(NoteProperty, value);
    }

    public Note SelectedNote {
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

    private void ToggleButton_OnClick(object? sender, RoutedEventArgs e) {
        if (sender is ToggleButton && this.SelectedNote.Equals(this.Note)) {
            this.SelectedNote = Note.Empty;
            this.SelectedNote = this.Note;
        }
    }
}