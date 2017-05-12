using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests.Common
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void ResultTest()
        {
            Result result = Result.Success();
            Check(result, 0, Assert.IsTrue);
        }

        [TestMethod]
        public void ResultGenericTest()
        {
            Result<string> result = Result<string>.Success("test");
            CheckGeneric(result, 0, Assert.IsTrue, Assert.IsNotNull);
        }

        [TestMethod]
        public void ResultErrorTest()
        {
            Result result = Result.Error(1);
            Check(result, 1, Assert.IsFalse);
        }

        [TestMethod]
        public void ResultGenericErrorTest()
        {
            Result<string> result = Result<string>.Error(1);
            CheckGeneric(result, 1, Assert.IsFalse, Assert.IsNull);
        }

        [TestMethod]
        public void ResultAddErrorTest()
        {
            Result result = Result.Success();
            result.AddError(1);
            Check(result, 1, Assert.IsFalse);

            Result resultError = Result.Error(1);
            resultError.AddError(2);
            Check(resultError, 2, Assert.IsFalse);
        }

        [TestMethod]
        public void ResultGenericAddErrorTest()
        {
            Result<string> result = Result<string>.Success("test");
            result.AddError(1);
            CheckGeneric(result, 1, Assert.IsFalse, Assert.IsNull);

            Result<string> resultError = Result<string>.Error(1);
            resultError.AddError(2);
            CheckGeneric(resultError, 2, Assert.IsFalse, Assert.IsNull);
        }

        [TestMethod]
        public void ResultAddErrorsTest()
        {
            Result result = Result.Success();
            result.AddErrors(new int[] { 1, 2 });
            Check(result, 2, Assert.IsFalse);

            Result resultError = Result.Error(1);
            resultError.AddErrors(new int[] { 2, 3 });
            Check(resultError, 3, Assert.IsFalse);
            resultError.AddErrors(null);
            Check(resultError, 3, Assert.IsFalse);
            resultError.AddErrors(new int[] { });
            Check(resultError, 3, Assert.IsFalse);
        }

        [TestMethod]
        public void ResultGenericAddErrorsTest()
        {
            Result<string> result = Result<string>.Success("test");
            result.AddErrors(null);
            CheckGeneric(result, 0, Assert.IsTrue, Assert.IsNotNull);
            result.AddErrors(new int[] { });
            CheckGeneric(result, 0, Assert.IsTrue, Assert.IsNotNull);
            result.AddErrors(new int[] { 1, 2 });
            CheckGeneric(result, 2, Assert.IsFalse, Assert.IsNull);

            Result<string> resultError = Result<string>.Error(1);
            resultError.AddErrors(new int[] { 1, 2 });
            CheckGeneric(resultError, 3, Assert.IsFalse, Assert.IsNull);
            resultError.AddErrors(null);
            CheckGeneric(resultError, 3, Assert.IsFalse, Assert.IsNull);
            resultError.AddErrors(new int[] { });
            CheckGeneric(resultError, 3, Assert.IsFalse, Assert.IsNull);
        }

        private void CheckGeneric<T>(Result<T> result, int errorCount, Action<bool> trueFalseAction,
            Action<object> nullAction)
        {
            trueFalseAction(result.IsSuccess);
            nullAction(result.Data);
            Assert.IsNotNull(result.ErrorCodes);
            Assert.AreEqual(errorCount, result.ErrorCodes.Count());
        }

        private void Check(Result result, int errorCount, Action<bool> trueFalseAction)
        {
            trueFalseAction(result.IsSuccess);
            Assert.IsNotNull(result.ErrorCodes);
            Assert.AreEqual(errorCount, result.ErrorCodes.Count());
        }
    }
}
