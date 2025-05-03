using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using XIVLauncher.Windows.ViewModel;

namespace XIVLauncher.Windows
{
    /// <summary>
    /// Interaction logic for TotpInputDialog.xaml
    /// </summary>
    public partial class TotpInputDialog : Window
    {
        public event Action<string> OnResult;

        private TotpInputDialogViewModel ViewModel => DataContext as TotpInputDialogViewModel;

        public TotpInputDialog()
        {
            InitializeComponent();

            this.DataContext = new TotpInputDialogViewModel();

            MouseMove += TotpInputDialog_OnMouseMove;
            Activated += (_, _) => this.TotpTextBox.Focus();
            GotFocus += (_, _) => this.TotpTextBox.Focus();
        }

        public new bool? ShowDialog()
        {
            this.TotpTextBox.Focus();

            return base.ShowDialog();
        }

        public void TryAcceptTotp(string totp)
        {
            OnResult?.Invoke(totp);

            Dispatcher.Invoke(() =>
            {
                DialogResult = true;
                Hide();
            });
        }

        private void Cancel()
        {
            OnResult?.Invoke(string.Empty);
            DialogResult = false;
            Hide();
        }

        private void TotpInputDialog_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void TotpTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TotpTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Cancel();
            }
            else if (e.Key == Key.Enter)
            {
                this.TryAcceptTotp(this.TotpTextBox.Text);
            }
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.TryAcceptTotp(this.TotpTextBox.Text);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public static string AskForTotp(Action<TotpInputDialog, string> onTotpResult, Window parentWindow)
        {
            if (Dispatcher.CurrentDispatcher != parentWindow.Dispatcher)
                return parentWindow.Dispatcher.Invoke(() => AskForTotp(onTotpResult, parentWindow));

            var dialog = new TotpInputDialog();

            if (parentWindow.IsVisible)
            {
                dialog.Owner = parentWindow;
                dialog.ShowInTaskbar = false;
            }

            string result = string.Empty;
            dialog.OnResult += otp => onTotpResult(dialog, result = otp);
            return dialog.ShowDialog() == true ? result : string.Empty;
        }
    }
}
