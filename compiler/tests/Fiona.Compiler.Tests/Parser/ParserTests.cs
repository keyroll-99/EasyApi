using Fiona.Compiler.Parser.Builders;
using Fiona.Compiler.Parser.Exceptions;
using Fiona.Compiler.Tests.Shared;
using Fiona.Compiler.Tokenizer;
using FluentAssertions;
using System.Text;
using System.Text.RegularExpressions;

namespace Fiona.Compiler.Tests.Parser;

public partial class ParserTests
{
    private readonly Fiona.Compiler.Parser.Parser _parser;

    public ParserTests()
    {
        _parser = new Fiona.Compiler.Parser.Parser();
    }
    
    [Fact]
    public async Task When_Given_TokenizedCode_Should_Return_Parsed_Code()
    {
        // arrange
        ProjectFile projectFile = await GetTestProjectFile(nameof(When_Given_TokenizedCode_Should_Return_Parsed_Code));
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTest!));
        using StreamReader reader = new(stream);

        // act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.Tokenizer.GetTokensAsync(reader);
        ReadOnlyMemory<byte> result = await _parser.ParseAsync(tokens, projectFile);
        string resultReader = Encoding.UTF8.GetString(result.ToArray());
        
        
        // assert
        string expectedResult = """
                                using system;
                                using system.collections;
                                using system.collections.generic;

                                namespace Token.Test;

                                [Controller("/home")]
                                public class TestController
                                {
                                
                                     [Route(HttpMethodType.Get | HttpMethodType.Post, "/test")]
                                     public async Task<User> Index()
                                     {
                                     }
                                }
                                """;
        expectedResult = Regex.Replace(expectedResult, @"\s+", "");
        resultReader = Regex.Replace(resultReader, @"\s+", "");
        resultReader.Should().Be(expectedResult);
    }
    
    [Fact]
    public async Task When_Given_TokenizedCodeWithParameter_Should_Return_Parsed_Code()
    {
        // arrange
        ProjectFile projectFile = await GetTestProjectFile(nameof(When_Given_TokenizedCodeWithParameter_Should_Return_Parsed_Code));
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTestWithParameter!));
        using StreamReader reader = new(stream);

        // act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.Tokenizer.GetTokensAsync(reader);
        ReadOnlyMemory<byte> result = await _parser.ParseAsync(tokens, projectFile);
        string resultReader = Encoding.UTF8.GetString(result.ToArray());
        
        
        // assert
        string expectedResult = """
                                using system;
                                using system.collections;
                                using system.collections.generic;

                                namespace Token.Test;

                                [Controller("/home")]
                                public class TestController
                                {
                                    private readonly IUserService userService;
                                    private readonly ILogger<TestController> logger;
                                    
                                    public TestController(IUserService userService, ILogger<TestController> logger){
                                        this.userService = userService;
                                        this.logger = logger;
                                    }
                                
                                     [Route(HttpMethodType.Get | HttpMethodType.Post, "/{name}", ["age"])]
                                     public async Task<User> Index(string name, [QueryParam] int age, [Body] User user, [Cookie] long userId)
                                     {
                                     }
                                }
                                """;
        expectedResult = Regex.Replace(expectedResult, @"\s+", "");
        resultReader = Regex.Replace(resultReader, @"\s+", "");
        resultReader.Should().Be(expectedResult);
    }
    
    [Fact]
    public async Task When_Given_TokenizedCodeWithParameterAndBody_Should_Return_Parsed_Code()
    {
        // arrange
        ProjectFile projectFile = await GetTestProjectFile(nameof(When_Given_TokenizedCodeWithParameterAndBody_Should_Return_Parsed_Code));
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullControllerWithBody!));
        using StreamReader reader = new(stream);

        // act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.Tokenizer.GetTokensAsync(reader);
        ReadOnlyMemory<byte> result = await _parser.ParseAsync(tokens, projectFile);
        string resultReader = Encoding.UTF8.GetString(result.ToArray());
        
        
        // assert
        string expectedResult = """
                                using system;
                                using system.collections;
                                using system.collections.generic;

                                namespace Token.Test;

                                [Controller("/home")]
                                public class TestController
                                {
                                    private readonly IUserService userService;
                                    private readonly ILogger<TestController> logger;
                                    
                                    public TestController(IUserService userService, ILogger<TestController> logger){
                                        this.userService = userService;
                                        this.logger = logger;
                                    }
                                
                                     [Route(HttpMethodType.Get | HttpMethodType.Post, "/{name}", ["age"])]
                                     public async Task<User> Index(string name, [QueryParam] int age, [Body] User user, [Cookie] long userId)
                                     {
                                        var x = 10;
                                        var y = userService.GetAge();
                                        if(x > y)
                                        {
                                            return user;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                     }
                                }
                                """;
        expectedResult = Regex.Replace(expectedResult, @"\s+", "");
        resultReader = Regex.Replace(resultReader, @"\s+", "");
        resultReader.Should().Be(expectedResult);
    }

    [Theory, MemberData(nameof(InvalidUsingTokenData))]
    internal async Task When_Given_Invalid_Using_Tokens_Then_Throw_Exception(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = await GetTestProjectFile(fileName);

        // Act
        Func<Task<ReadOnlyMemory<byte>>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().ThrowAsync<ParserException>();
    }

    [Theory, MemberData(nameof(ValidUsingTokenData))]
    internal async Task When_Given_Valid_Using_Tokens_Then_Should_Not_Throw_Error(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = await GetTestProjectFile(fileName);

        // Act
        Func<Task<ReadOnlyMemory<byte>>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().NotThrowAsync<ParserException>();
    }
    
    [Theory, MemberData(nameof(InvalidClassTokenData))]
    internal async Task When_Given_Invalid_Class_Tokens_Then_Throw_Exception(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = await GetTestProjectFile(fileName);

        // Act
        Func<Task<ReadOnlyMemory<byte>>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().ThrowAsync<ParserException>();
    }
    
    [Theory, MemberData(nameof(ValidClassTokenData))]
    internal async Task When_Given_Valid_Class_Tokens_Then_Should_Not_Throw_Error(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = await GetTestProjectFile(fileName);

        // Act
        Func<Task<ReadOnlyMemory<byte>>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().NotThrowAsync<ParserException>();
    }
    
    
    private async Task<ProjectFile> GetTestProjectFile(string name)
    {
        string filePath = $"namespace{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.fn";
        if (!Directory.Exists($"namespace{Path.DirectorySeparatorChar}{name}"))
        {
            Directory.CreateDirectory($"namespace{Path.DirectorySeparatorChar}{name}");
        }
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await using (FileStream fileHandler = File.Create(filePath))
        {
        }

        // Create a dummy project file to initialize the content
        ProjectFile projectFile = new(filePath, filePath.Split(System.IO.Path.DirectorySeparatorChar).Last());
        return projectFile;
    }

}