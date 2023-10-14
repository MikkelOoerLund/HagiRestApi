namespace HagiRestApi
{
    public static class Assert
    {
        public static T NotNull<T>(T? value) where T : class
        {
            if (value == null)
            {
                string typeName = typeof(T).Name;
                throw new ArgumentNullException(typeName, "The provided value is expected to be non-null.");
            }

            return value;
        }
    }
}
