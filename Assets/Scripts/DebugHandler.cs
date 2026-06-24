// You should read IsDebugEnabled before you consider using Debug.Log. This is to reduce lag in the final builds later on.
public static class DebugHandler
{
#if UNITY_EDITOR
    public static readonly bool IsDebugEnabled = true;
#else
    public static readonly bool IsDebugEnabled = false;
#endif
}
