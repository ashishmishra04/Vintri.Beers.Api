

using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Vintri.Beers.Model;
using Vintri.Beers.Service;

namespace Vintri.Beers.Api.NUnitTest
{
    public class BeerServiceUnitTest: BaseSettings
    {

        private IBeerService _beerService;
        public BeerServiceUnitTest()
        {
            _beerService = new BeerService(PunkApiUrl, StorageLocationPath);
        }

        [Test]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        public async Task TestBeerService_GetBeerById_Valid(int id)
        {
            var result = await _beerService.Get(id);
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task TestBeerService_GetAllBeerWithRating_IsValid()
        {
            var result = await _beerService.GetAllBeerWithRating();
            Assert.NotNull(result);
            Assert.True(result.Count > -1);
        }

        [Test]
        public async Task TestBeerService_AddUserRating_IsValid()
        {
            var operationResult = await _beerService.AddUserRating(50, new UserRating
            {
                UserName = "ashishmishra@gmail.com",
                Rating = 4,
                Comments = "Best Beer Ever in my life"
            });
            Assert.NotNull(operationResult);
            Assert.True(operationResult.IsSuccess);
            Assert.NotNull(operationResult.Data);
            Assert.True(operationResult.Data.Id == 50);
            Assert.True(operationResult.Data.UserRatings.Count > 0);
            var userRating = operationResult.Data.UserRatings.FirstOrDefault(userRatingItem => userRatingItem.UserName.Equals("ashishmishra@gmail.com"));
            Assert.NotNull(userRating);
            Assert.True(userRating.Rating == 4);
            Assert.True(userRating.Comments == "Best Beer Ever in my life");
        }

        [Test]
        public async Task TestBeerService_AddUserRating_InValidUserName()
        {
            var operationResult = await _beerService.AddUserRating(50, new UserRating
            {
                UserName = "testInvalidUserName",
                Rating = 3,
                Comments = "Best Beer Ever in my life"
            });
            Assert.NotNull(operationResult);
            Assert.False(operationResult.IsSuccess);
            Assert.IsNull(operationResult.Data);
            var errors = operationResult.ValidationResult.Errors.ToList();
            Assert.IsTrue(errors.Count == 1);
            Assert.AreEqual(errors.FirstOrDefault(error => error.Property == "UserName")?.Message, "Invalid username provided: testInvalidUserName, valid email required");
        }

        [Test]
        public async Task TestBeerService_AddUserRating_InValidRating()
        {
            var operationResult = await _beerService.AddUserRating(50, new UserRating
            {
                UserName = "ashishmishra@gmail.com",
                Rating = 200,
                Comments = "Best Beer Ever in my life"
            });
            Assert.NotNull(operationResult);
            Assert.False(operationResult.IsSuccess);
            Assert.IsNull(operationResult.Data);
            var errors = operationResult.ValidationResult.Errors.ToList();
            Assert.IsTrue(errors.Count == 1);
            Assert.AreEqual(errors.FirstOrDefault(error => error.Property == "Rating")?.Message, "Invalid Rating provided: 200, valid number is 1 to 5"); 
        }

        [Test]
        public async Task TestBeerService_AddUserRating_InValidUserNameAndRating()
        {
            var operationResult = await _beerService.AddUserRating(50, new UserRating
            {
                UserName = "ashishmishra",
                Rating = 10,
                Comments = "Best Beer Ever in my life"
            });
            Assert.NotNull(operationResult);
            Assert.False(operationResult.IsSuccess);
            Assert.IsNull(operationResult.Data);
            var errors = operationResult.ValidationResult.Errors.ToList();
            Assert.IsTrue(errors.Count == 2);
            Assert.AreEqual(errors.FirstOrDefault(error => error.Property == "Rating")?.Message, "Invalid Rating provided: 10, valid number is 1 to 5");
            Assert.AreEqual(errors.FirstOrDefault(error => error.Property == "UserName")?.Message, "Invalid username provided: ashishmishra, valid email required");
        }


        [Test]
        [TestCase(10645)]
        [TestCase(50645)]
        [TestCase(100645)]
        [TestCase(200645)]
        public async Task TestBeerService_GetBeerById_InValid(int id)
        {
            var result = await _beerService.Get(id);
            Assert.Null(result);
        }


        [Test]
        [TestCase("Trashy Blonde", 2)]
        [TestCase("Hopped-Up Brown Ale - Prototype Challenge", 82)]
        [TestCase("Comet", 58)]
        [TestCase("Bowman's Beard - B-Sides", 97)]
        public async Task TestBeerService_GetBeerByName_Valid(string name, int id)
        {
            var result = await _beerService.Get(name);
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
        }


        [Test]
        [TestCase("Trashy Blonde Dummy")]
        [TestCase("Hopped-Up Brown Ale - Prototype Challenge Dummy")]
        [TestCase("Comet Dummy")]
        [TestCase("Bowman's Beard - B-Sides Dummy")]
        public async Task TestBeerService_GetBeerByName_InValid(string name)
        {
            var result = await _beerService.Get(name);
            Assert.Null(result);
        }


        
    }
}
