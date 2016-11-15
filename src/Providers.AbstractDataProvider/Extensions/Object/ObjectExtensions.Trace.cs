﻿namespace Sitecore.Extensions.Object
{
  using System.Diagnostics;
  using System.Linq;
  using System.Net;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public static class ObjectExtensions
  {  
    [Conditional("DEBUG")]
    public static void Trace([NotNull] this object obj, [CanBeNull] object result, int ignoreThisParam, [NotNull] params object[] arguments)
    {
      Assert.ArgumentNotNull(obj, nameof(obj));
      Assert.ArgumentNotNull(arguments, nameof(arguments));

      var st = new StackTrace();
      var sf = st.GetFrame(1);
      Assert.IsNotNull(sf, nameof(sf));

      var type = obj.GetType().FullName;
      var method = sf.GetMethod().Name;
      var argumentsText = string.Join(", ", arguments.Select(Print));
      var resultText = Print(result);
      var message = $"Tracing call\r\n{type}.{method}({argumentsText}) = {resultText}\r\n";

      Log.Info(message, typeof(ObjectExtensions));
    }

    [NotNull]
    private static string Print(object obj)
    {
      if (obj == null)
      {
        return "null";
      }

      var list = obj as IDList;    
      if (list != null)
      {
        return "\"" + string.Join(", ", list.Cast<object>().Select(Print).ToArray()) + "\"";
      }

      var itemDefinition = obj as ItemDefinition;
      if (itemDefinition != null)
      {
        return "\"" + itemDefinition.ID + "\"";
      }

      return "\"" + obj + "\"";
    }
  }
}
