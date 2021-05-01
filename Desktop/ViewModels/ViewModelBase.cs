namespace Macabresoft.GuitarTuner.Desktop.ViewModels {

    using ReactiveUI;
    using System.Runtime.CompilerServices;

    public class ViewModelBase : ReactiveObject {

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
            var result = false;
            if (!Equals(field, value)) {
                field = value;
                this.RaisePropertyChanged(propertyName);
                result = true;
            }

            return result;
        }
    }
}