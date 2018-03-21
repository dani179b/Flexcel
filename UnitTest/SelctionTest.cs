using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class SelctionTest
    {
        private readonly TestDataContainer _testData = new TestDataContainer();
        private readonly SelectionController _selectionController = new SelectionController();

        [TestMethod]
        public void TestMethod_NoInputData()
        {
            _selectionController.SelectWinners();
        }

        [TestMethod]
        public void TestMethod_HappyPath()
        {
            _testData.FillListContainer_HappyPath();
            _selectionController.SelectWinners();
        }
    }
}
