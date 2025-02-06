using Hertzole.ScriptableValues.Helpers;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	public class ThrowHelperTests
	{
		[Test]
		public void ThrowIfNull_NullObject_ThrowsException()
		{
			Assert.Throws<System.ArgumentNullException>(() => ThrowHelper.ThrowIfNull(null, "test"));
		}
		
		[Test]
		public void ThrowIfNull_NotNullObject_DoesNotThrowException()
		{
			Assert.DoesNotThrow(() => ThrowHelper.ThrowIfNull(new object(), "test"));
		}
		
		[Test]
		public void ThrowIfDisposed_Disposed_ThrowsException()
		{
			const bool is_disposed = true;
			Assert.Throws<System.ObjectDisposedException>(() => ThrowHelper.ThrowIfDisposed(is_disposed));
		}
		
		[Test]
		public void ThrowIfDisposed_NotDisposed_DoesNotThrowException()
		{
			const bool is_disposed = false;
			Assert.DoesNotThrow(() => ThrowHelper.ThrowIfDisposed(is_disposed));
		}
	}
}