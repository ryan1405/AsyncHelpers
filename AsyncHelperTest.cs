using ConsoleApp1;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncHelperTests
{
    public class AsyncHelperTest
    {
        [Fact]
        public void RunSyncPreservesCulture()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUiCulture = Thread.CurrentThread.CurrentUICulture;
            var expectedCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = expectedCulture;
            Thread.CurrentThread.CurrentUICulture = expectedCulture;
            try
            {
                var cultures = AsyncHelper.RunSync(GetCultureAsync);
                Assert.Equal(expectedCulture, cultures.Item1);
                Assert.Equal(expectedCulture, cultures.Item2);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUiCulture;
            }
        }

        [Fact]
        public void RunSyncVoidPreservesCulture()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUiCulture = Thread.CurrentThread.CurrentUICulture;
            var expectedCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = expectedCulture;
            Thread.CurrentThread.CurrentUICulture = expectedCulture;
            try
            {
                AsyncHelper.RunSync(() => ExpectCulture(expectedCulture));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUiCulture;
            }
        }

        private static Task<Tuple<CultureInfo, CultureInfo>> GetCultureAsync()
        {
            return Task.FromResult(new Tuple<CultureInfo, CultureInfo>(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture));
        }

        private static Task ExpectCulture(CultureInfo culture)
        {
            Assert.Equal(culture, CultureInfo.CurrentCulture);
            Assert.Equal(culture, CultureInfo.CurrentUICulture);
            return Task.FromResult(0);
        }
    }
}
