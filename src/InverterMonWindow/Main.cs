using System.Runtime.InteropServices;

namespace InverterMonWindow;

public partial class Main : Form
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnregisterHotKey(IntPtr hWnd, int id);

    void SetTitleBarColor(Color color)
    {
        var colorValue = color.R | (color.G << 8) | (color.B << 16);
        DwmSetWindowAttribute(Handle, DWMWA_CAPTION_COLOR, ref colorValue, sizeof(int));
    }

    [DllImport("dwmapi.dll")]
    static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    const int DWMWA_CAPTION_COLOR = 35;

    public Main()
    {
        InitializeComponent();
        SetTitleBarColor(Color.Black);
        TrayIcon.Click += TrayIcon_Click;
        Load += Main_Load;
        FormClosed += Main_FormClosed;
        Resize += Main_Resize;
        if (!RegisterHotKey(Handle, 1, (int)KeyModifier.Alt, (int)Keys.I))
            MessageBox.Show("Failed to register hotkey!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    void Main_Resize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            WindowState = FormWindowState.Normal;
            Hide();
            TrayIcon.Visible = true;
        }
    }

    void Main_FormClosed(object? sender, FormClosedEventArgs e)
    {
        Properties.Settings.Default.WindowPosition = Location;
        Properties.Settings.Default.Save();
        UnregisterHotKey(Handle, 1);
    }

    void Main_Load(object? sender, EventArgs e)
    {
        Location = Properties.Settings.Default.WindowPosition;
    }

    void TrayIcon_Click(object? sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        TrayIcon.Visible = false;
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m.Msg == 0x0312)
        {
            switch (m.WParam.ToInt32())
            {
                case 1: // Alt+I hotkey
                    if (WindowState == FormWindowState.Normal)
                        WindowState = FormWindowState.Minimized;
                    else
                        TrayIcon_Click(null, null!);

                    break;
            }
        }
    }
}

[Flags]
public enum KeyModifier
{
    None = 0,
    Alt = 0x0001,
    Control = 0x0002,
    Shift = 0x0004,
    Winkey = 0x0008
}