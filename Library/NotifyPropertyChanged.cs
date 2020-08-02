namespace Macabresoft.Zvukosti.Library {

    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A base class that implements <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged {

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Disposes <see cref="PropertyChanged"/>.
        /// </summary>
        protected void DisposePropertyChanged() {
            this.PropertyChanged = null;
        }

        /// <summary>
        /// Sets the specified field.
        /// </summary>
        /// <typeparam name="T">The type being set.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>

        /// <param name="propertyName">Name of the property.</param>
        /// <returns>A value indicating whether or not the value was successfully set.</returns>
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