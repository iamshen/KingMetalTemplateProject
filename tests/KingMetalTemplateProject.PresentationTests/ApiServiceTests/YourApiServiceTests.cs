namespace KingMetalTemplateProject.PresentationTests;

[Collection(ClusterCollection.Name)]
public class YourApiServiceTests
{
    #region Arrange

    private readonly TestCluster _cluster;

    #endregion
    
    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="fixture"></param>
    public YourApiServiceTests(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    [Fact(DisplayName = "1. 创建")]
    public async Task CreateYourAsync()
    {
        var id = await _cluster.GrainFactory.NewIntegerIdAsync();
        Assert.True(id > 0);
        // Sample
        // var service = _cluster.ServiceProvider.GetRequiredService<IYourApiService>();
        // var (name, account) = GenerateHelper.GenerateCompanyName();
        // var inputFaker = new Faker<CreateYourInputDto>(Constants.FakerLocale)
        //         .RuleFor(x => x.Name, _ => name)
        //         .RuleFor(x => x.Account, f => account + f.Random.Int(100, 100))
        //         .RuleFor(x => x.Password, f => f.Internet.Password())
        //         .RuleFor(x => x.CustomCode, f => f.Random.AlphaNumeric(8).ToUpper())
        //         .RuleFor(x => x.Description, _ => GenerateHelper.GenerateChineseText(50))
        //         .RuleFor(x => x.Logo,
        //             (faker, _) => new ReferResources(faker.Image.PicsumUrl(), GenerateHelper.GenerateChineseText(10)))
        //         .RuleFor(x => x.Contacts,
        //             (faker, _) => new ContactsObject
        //             {
        //                 Email = faker.Internet.Email(),
        //                 Name = faker.Internet.UserName(),
        //                 Phone = faker.Phone.PhoneNumberFormat(1)
        //             })
        //     ;
        //
        // var input = inputFaker.Generate();
        //
        // var result = await service.CreateAsync(input);
        // Assert.NotNull(result);
        //
        // var state = await _cluster.GrainFactory.GetGrain<IYourGrain>(result.Id.CastTo<long>()).GetState() as YourState;
        // Assert.NotNull(state);
        
        Assert.True(true);

        await Task.CompletedTask;
    }
   
}
