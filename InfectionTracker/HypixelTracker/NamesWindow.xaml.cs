using System.Text.RegularExpressions;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HypixelTracker;
/// <summary>
/// Interaction logic for NamesWindow.xaml
/// </summary>
public partial class NamesWindow : Window
{
    public bool IsRequestingTracking { get; private set; }
    public ReadOnlyMemory<string> Names { get; private set; }

    public NamesWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var names = textBox.Text
            .Split(['\r', '\n'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (names.Length == 0 || !names.All(x => MCNameRegex().IsMatch(x)))
        {
            MessageBox.Show(
                "List is empty or some of the names are invalid.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Names = new(names.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());

        IsRequestingTracking = true;
        Close();
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_]{1,16}$")]
    private static partial Regex MCNameRegex();
}
