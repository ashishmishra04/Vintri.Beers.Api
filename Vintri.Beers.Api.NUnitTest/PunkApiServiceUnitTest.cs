using System.Threading.Tasks;
using NUnit.Framework;
using Vintri.Beers.Service;

namespace Vintri.Beers.Api.NUnitTest
{
    public class PunkApiServiceUnitTest: BaseSettings
    {
        private readonly IPunkApiService _punkApiService;
        public PunkApiServiceUnitTest()
        {
            _punkApiService = new PunkApiService(PunkApiUrl);
        }

        [Test]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        public async Task TestPunkApi_GetBeerById_Valid(int id)
        {
            var result = await _punkApiService.Get(id);
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }


        [Test]
        [TestCase(10645)]
        [TestCase(50645)]
        [TestCase(100645)]
        [TestCase(200645)]
        public async Task TestPunkApi_GetBeerById_InValid(int id)
        {
            var result = await _punkApiService.Get(id);
            Assert.Null(result);
        }


        [Test]
        [TestCase("Trashy Blonde", 2)]
        [TestCase("Hopped-Up Brown Ale - Prototype Challenge", 82)]
        [TestCase("Comet", 58)]
        [TestCase("Bowman's Beard - B-Sides", 97)]
        public async Task TestPunkApi_GetBeerByName_Valid(string name, int id)
        {
            var result = await _punkApiService.Get(name);
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }


        [Test]
        [TestCase("Trashy Blonde Dummy")]
        [TestCase("Hopped-Up Brown Ale - Prototype Challenge Dummy")]
        [TestCase("Comet Dummy")]
        [TestCase("Bowman's Beard - B-Sides Dummy")]
        public async Task TestPunkApi_GetBeerByName_InValid(string name)
        {
            var result = await _punkApiService.Get(name);
            Assert.Null(result);
        }
    }
}