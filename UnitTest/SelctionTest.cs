using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;

namespace UnitTest
{
    [TestClass]
    public class SelctionTest
    {
        TestDataContainer testData = new TestDataContainer();
        SelectionController selectionController = new SelectionController();

        [TestMethod]
        public void TestMethod_NoInputData()
        {
            selectionController.SelectWinners();
        }

        [TestMethod]
        public void TestMethod_HappyPath()
        {
            testData.FillListContainer_HappyPath();
            selectionController.SelectWinners();
        }
    }
}
