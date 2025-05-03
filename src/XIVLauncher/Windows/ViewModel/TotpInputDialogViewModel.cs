using CheapLoc;

namespace XIVLauncher.Windows.ViewModel
{
    class TotpInputDialogViewModel
    {
        public TotpInputDialogViewModel()
        {
            SetupLoc();
        }

        private void SetupLoc()
        {
            TotpInputPromptLoc = Loc.Localize("TotpInputPrompt", "Please enter your TOTP secret.");
            CancelWithShortcutLoc = Loc.Localize("CancelWithShortcut", "_Cancel");
            OkLoc = Loc.Localize("OK", "OK");
            TotpDisableHintLoc = Loc.Localize("TotpDisableHint", $"To disable TOTP authentication,\nPress the {CancelWithShortcutLoc.Replace("_", "")} button.");
        }

        public string TotpInputPromptLoc { get; private set; }
        public string TotpDisableHintLoc { get; private set; }
        public string CancelWithShortcutLoc { get; private set; }
        public string OkLoc { get; private set; }
    }
}
