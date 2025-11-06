namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     lol
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="oldValue">Test</param>
    /// <param name="newValue">Test 2</param>
    public delegate void ValueEventHandler<in T>(T oldValue, T newValue);
}