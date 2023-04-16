namespace DapperAccess.Impl;
internal class ThrowHelper
{
    public static void ValidateRange(int from, int to)
    {
        if (from < 1)
        {
            throw new ArgumentException($"One-based parameter '{nameof(from)}' must be greater or equal to 1.");
        }
        if (to < 1)
        {
            throw new ArgumentException($"One-based parameter '{nameof(to)}' must be greater or equal to 1.");
        }

        if (from > to)
        {
            throw new ArgumentException($"Parameter '{nameof(from)}' must be less or equal to parameter '{nameof(to)}'.");
        }
    }
}
