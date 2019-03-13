namespace Bach.Model
{
  using System;
  using System.Diagnostics;

  internal static class Contract
  {
    public static void Requires(bool condition,
                                string message = null)
    {
      Requires<ArgumentException>(condition, message);
    }

    public static void Requires<TException>(bool condition,
                                            string message = null)
      where TException: Exception, new()
    {
      if( !condition )
      {
        //https://stackoverflow.com/questions/41397/asking-a-generic-method-to-throw-specific-exception-type-on-fail/41450#41450
        TException exception;

        try
        {
          message = message ?? "Unexpected Condition";
          exception = Activator.CreateInstance(typeof(TException), message) as TException;
        }
        catch( MissingMethodException )
        {
          exception = new TException();
        }

        Debug.Assert(exception != null, nameof(exception) + " != null");
        throw exception;
      }
    }

    public static void RequiresNotNullOrEmpty(string value,
                                              string message = null)
    {
      Requires<ArgumentNullException>(value != null, message);
      Requires<ArgumentException>(value.Length > 0, message);
    }
  }
}
