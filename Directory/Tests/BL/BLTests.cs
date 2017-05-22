using BL;
using Common;
using DAL;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tests.DAL;

namespace Tests.BL
{
    [TestClass]
    public class BLTests
    {
        private static RecordOperations _recordOperations;
        private int _userId = 1;
        private static IRepository<Record> _recordRepo;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            ILoggerFactory factory = new LoggerFactory().AddDebug();
            DataServiceMock dataService = new DataServiceMock();
            _recordRepo = dataService.GetRepository<Record>();
            _recordOperations = new RecordOperations(
                dataService,
                new RecordFetchProvider(),
                new Logger<RecordOperations>(factory));
        }

        [TestMethod]
        public void BLRecordAddTest()
        {
            int count = _recordRepo.GetAll().Result.Count();
            Result result = _recordOperations.Save(new RecordModel(), _userId).Result;
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(count + 1, _recordRepo.GetAll().Result.Count());
        }

        [TestMethod]
        public void BLRecordAddFailTest()
        {
            int count = _recordRepo.GetAll().Result.Count();
            Result result = _recordOperations.Save(new RecordModel() { Id = 5 }, _userId).Result;
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(count, _recordRepo.GetAll().Result.Count());
        }

        [TestMethod]
        public void BLRecordUpdateTest()
        {
            int count = _recordRepo.GetAll().Result.Count();
            var record = _recordOperations.Get(1, _userId).Result;
            int id = record.Data.Id;
            var recordResult = _recordOperations.Update(new RecordModel() { Id = id, Info = "test" }, _userId).Result;
            Assert.IsTrue(recordResult.IsSuccess);
            var recordResultTest = _recordOperations.Get(1, _userId).Result;
            var recordModel = (RecordModel)recordResultTest.Data;
            Assert.AreEqual("test", recordModel.Info);
        }
    }
}
