using Entile.TestApp.Actions;
using Entile.TestApp.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{
    [TestClass]
    public class EntileViewModelTests : SilverlightTest
    {

        public void When_Activating_ViewModel()
        {
            var entileViewModel = new EntileViewModel();
            entileViewModel.RootApiQuery.Execute(null);
        }
    }
}