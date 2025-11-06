using System;

namespace Hertzole.ScriptableValues.Generator;

internal static class DocumentationHelper
{
    public const string EVENT_SENDER = "The object that sent the event.";
    public const string EVENT_ARGS = "The event arguments.";

    public const string COLLECTION_ARGS = "The collection changed arguments.";

    public const string POOL_ACTION = "The change that just happened on the pool.";
    public const string POOL_ITEM = "The item that was affected by the change.";

    public const string SUBSCRIBED_MASK_SUMMARY = "/// <summary>A bitmask of all the possible subscribed callbacks.</summary>";

    public static ReadOnlySpan<char> GetMethodCallbackDescription(in ReadOnlySpan<char> name, in CallbackType callbackType, in CallbackFlags flags)
    {
        using (ArrayBuilder<char> builder = new ArrayBuilder<char>(64))
        {
            builder.AddRange("/// <summary>Called when <see cref=\"");
            builder.AddRange(name);
            builder.AddRange("\" /> is ");
            if (callbackType == CallbackType.Event)
            {
                builder.AddRange("invoked");
            }
            else if (callbackType == CallbackType.Value && (flags & CallbackFlags.PreInvoke) != 0)
            {
                builder.AddRange("changing");
            }
            else
            {
                builder.AddRange("changed");
            }

            builder.AddRange(".</summary>");
            return builder.AsSpan();
        }
    }

    public static string GetOldValueDescription(in CallbackFlags flags)
    {
        if ((flags & CallbackFlags.PreInvoke) != 0)
        {
            return "The previous value that is being replaced.";
        }

        if ((flags & CallbackFlags.PostInvoke) != 0)
        {
            return "The previous value that was replaced.";
        }

        return $"UNKNOWN FLAG VALUE: {flags}";
    }

    public static string GetNewValueDescription(in CallbackFlags flags)
    {
        if ((flags & CallbackFlags.PreInvoke) != 0)
        {
            return "The new value being set.";
        }

        if ((flags & CallbackFlags.PostInvoke) != 0)
        {
            return "The new value that was set.";
        }

        return $"UNKNOWN FLAG VALUE: {flags}";
    }
}