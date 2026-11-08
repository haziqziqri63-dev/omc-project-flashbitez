namespace omc;

public static class Nav
{
    private static readonly Dictionary<Type, Form> OpenForms = [];

    public static void GoTo(Form target, Form current)
    {
        var type = target.GetType();
        if (type == current.GetType())
        {
            target.Dispose();
            return;
        }

        if (OpenForms.TryGetValue(type, out var existing) && !existing.IsDisposed)
        {
            target.Dispose();
            existing.Show();
            existing.BringToFront();
            current.Hide();
            return;
        }

        OpenForms[type] = target;
        target.FormClosed += (_, _) => OpenForms.Remove(type);
        target.Show();
        current.Hide();
    }

    public static void Logout(Form current)
    {
        AppData.CurrentUser = null;
        GoTo(new frmHomePage(), current);
    }
}
