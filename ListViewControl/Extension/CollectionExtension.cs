namespace ListViewControl.Extension
{
    using System.Collections.Generic;

    public static partial class CollectionExtension
    {
        public static T Next<T>(this IList<T> @this, T item)
        {
            int nextIndex = @this.IndexOf(item) + 1;

            if (nextIndex == @this.Count)
            {
                return default(T);
            }

            return @this[nextIndex];
        }

		internal static T Previous<T>(this IList<T> @this, T item)
		{
			int previousIndex = @this.IndexOf(item) - 1;

			if (previousIndex < 0)
			{
				return default(T);
			}

			return @this[previousIndex];
		}
    }
}
