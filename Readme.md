# Dotnet SDK for OpenAI ChatGPT, Whisper, GPT-4 and DALL·E

[![Betalgo.OpenAI](https://img.shields.io/nuget/v/Betalgo.OpenAI?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI/)

```
Install-Package Betalgo.OpenAI
```

Dotnet SDK for OpenAI Chat GPT, Whisper, GPT-4 ,GPT-3 and DALL·E  
*Unofficial*.  
*OpenAI doesn't have any official .Net SDK.*

#### This library used be to known as `Betalgo.OpenAI.GPT3`, now it has a new package Id `Betalgo.OpenAI`.

## Checkout the wiki page: 
https://github.com/betalgo/openai/wiki  
or  [![Static Badge](https://img.shields.io/badge/API%20Docs-RobiniaDocs-43bc00?logo=readme&logoColor=white)](https://www.robiniadocs.com/d/betalgo-openai/api/OpenAI.ObjectModels.RequestModels.ChatMessage.html)
## Checkout new ***experimantal*** utilities library:
[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)
```
Install-Package Betalgo.OpenAI.Utilities
```
Maintenance of this project is made possible by all the bug reporters, [contributors](https://github.com/betalgo/openai/graphs/contributors) and [sponsors](https://github.com/sponsors/kayhantolga).  
💖 Sponsors:  
[@betalgo](https://github.com/betalgo),
[Laser Cat Eyes](https://lasercateyes.com/)

[@oferavnery](https://github.com/oferavnery)
[@Removable](https://github.com/Removable)
## Features
- [X] [Function Calling](https://github.com/betalgo/openai/wiki/Function-Calling)
- [ ] Plugins (coming soon)
- [x] [Chat GPT](https://github.com/betalgo/openai/wiki/Chat-GPT)
- [x] [Chat GPT-4](https://github.com/betalgo/openai/wiki/Chat-GPT) *(models are supported, Image analyze API not released yet by OpenAI)*
- [x] [Azure OpenAI](https://github.com/betalgo/openai/wiki/Azure-OpenAI)
- [x] [Image DALL·E](https://github.com/betalgo/openai/wiki/Dall-E)
- [x] [Models](https://github.com/betalgo/openai/wiki/Models)
- [x] [Completions](https://github.com/betalgo/openai/wiki/Completions) 
- [x] [Edit](https://github.com/betalgo/openai/wiki/Edit) 
- [x] [Embeddings](https://github.com/betalgo/openai/wiki/Embeddings) 
- [x] [Files](https://github.com/betalgo/openai/wiki/Files) 
- [x] [Chatgpt Fine-Tuning](https://github.com/betalgo/openai/wiki/Chatgpt-Fine-Tuning) 
- [x] [Fine-tunes](https://github.com/betalgo/openai/wiki/Fine-Tuning)
- [x] [Moderation](https://github.com/betalgo/openai/wiki/Moderation)
- [x] [Tokenizer-GPT3](https://github.com/betalgo/openai/wiki/Tokenizer)
- [ ] [Tokenizer](https://github.com/betalgo/openai/wiki/Tokenizer)
- [x] [Whisper](https://github.com/betalgo/openai/wiki/Whisper)
- [x] [Rate limit](https://github.com/betalgo/openai/wiki/Rate-Limit)
- [x] [Proxy](https://github.com/betalgo/openai/wiki/Proxy)


For changelogs please go to end of the document.

## Sample Usages
The repository contains a sample project named **OpenAI.Playground** that you can refer to for a better understanding of how the library works. However, please exercise caution while experimenting with it, as some of the test methods may result in unintended consequences such as file deletion or fine tuning.  

*!! It is highly recommended that you use a separate account instead of your primary account while using the playground. This is because some test methods may add or delete your files and models, which could potentially cause unwanted issues. !!*

Your API Key comes from here --> https://platform.openai.com/account/api-keys   
Your Organization ID comes from here --> https://platform.openai.com/account/org-settings

### Without using dependency injection:
```csharp
var openaiService = new OpenAIService(new OpenAIOptions()
{
    ApiKey =  Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY")
});
```
### Using dependency injection:
#### secrets.json: 

```csharp
 "OpenAIServiceOptions": {
    //"ApiKey":"Your api key goes here"
    //,"Organization": "Your Organization Id goes here (optional)"
  },
```
*(How to use [user secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) ?  
Right click your project name in "solution explorer" then click "Manage User Secret", it is a good way to keep your api keys)*

#### Program.cs
```csharp
serviceCollection.AddOpenAIService();
```

**OR**  
Use it like below but do NOT put your API key directly to your source code. 
#### Program.cs
```csharp
serviceCollection.AddOpenAIService(settings => { settings.ApiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY"); });
```

After injecting your service you will be able to get it from service provider
```csharp
var openaiService = serviceProvider.GetRequiredService<IOpenAIService>();
```

You can set default model(optional):
```csharp
openaiService.SetDefaultModelId(Models.Davinci);
```
## Chat Gpt Sample
```csharp
var completionResult = await openaiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
{
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromSystem("You are a helpful assistant."),
        ChatMessage.FromUser("Who won the world series in 2020?"),
        ChatMessage.FromAssistant("The Los Angeles Dodgers won the World Series in 2020."),
        ChatMessage.FromUser("Where was it played?")
    },
    Model = Models.ChatGpt3_5Turbo,
    MaxTokens = 50//optional
});
if (completionResult.Successful)
{
   Console.WriteLine(completionResult.Choices.First().Message.Content);
}
```
## Function Sample
```csharp
var fn1 = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather")
            .AddParameter("location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA"))
            .AddParameter("format", PropertyDefinition.DefineEnum(new List<string> { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .Validate()
            .Build();

        var fn2 = new FunctionDefinitionBuilder("get_n_day_weather_forecast", "Get an N-day weather forecast")
            .AddParameter("location", new() { Type = "string", Description = "The city and state, e.g. San Francisco, CA" })
            .AddParameter("format", PropertyDefinition.DefineEnum(new List<string> { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .AddParameter("num_days", PropertyDefinition.DefineInteger("The number of days to forecast"))
            .Validate()
            .Build();
        var fn3 = new FunctionDefinitionBuilder("get_current_datetime", "Get the current date and time, e.g. 'Saturday, June 24, 2023 6:14:14 PM'")
            .Build();

        var fn4 = new FunctionDefinitionBuilder("identify_number_sequence", "Get a sequence of numbers present in the user message")
            .AddParameter("values", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Sequence of numbers specified by the user")))
            .Build();

        ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);
        var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."),
                    ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days.")
                },
            Functions = new List<FunctionDefinition> { fn1, fn2, fn3, fn4 },
            MaxTokens = 50,
            Model = Models.Gpt_3_5_Turbo
        });

        if (completionResult.Successful)
        {
            var choice = completionResult.Choices.First();
            Console.WriteLine($"Message:        {choice.Message.Content}");

            var fn = choice.Message.FunctionCall;
            if (fn != null)
            {
                Console.WriteLine($"Function call:  {fn.Name}");
                foreach (var entry in fn.ParseArguments())
                {
                    Console.WriteLine($"  {entry.Key}: {entry.Value}");
                }
            }
        }
```

## Completions Stream Sample
```csharp
var completionResult = openaiService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
   {
      Prompt = "Once upon a time",
      MaxTokens = 50
   }, Models.Davinci);

   await foreach (var completion in completionResult)
   {
      if (completion.Successful)
      {
         Console.Write(completion.Choices.FirstOrDefault()?.Text);
      }
      else
      {
         if (completion.Error == null)
         {
            throw new Exception("Unknown Error");
         }

         Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
      }
   }
   Console.WriteLine("Complete");
```

## DALL·E Sample
```csharp
var imageResult = await openaiService.Image.CreateImage(new ImageCreateRequest
{
    Prompt = "Laser cat eyes",
    N = 2,
    Size = StaticValues.ImageStatics.Size.Size256,
    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
    User = "TestUser"
});


if (imageResult.Successful)
{
    Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
}
```

## Notes:
Please note that due to time constraints, I was unable to thoroughly test all of the methods or fully document the library. If you encounter any issues, please do not hesitate to report them or submit a pull request - your contributions are always appreciated.

I initially developed this SDK for my personal use and later decided to share it with the community. As I have not maintained any open-source projects before, any assistance or feedback would be greatly appreciated. If you would like to contribute in any way, please feel free to reach out to me with your suggestions.

I will always be using the latest libraries, and future releases will frequently include breaking changes. Please take this into consideration before deciding to use the library. I want to make it clear that I cannot accept any responsibility for any damage caused by using the library. If you feel that this is not suitable for your purposes, you are free to explore alternative libraries or the OpenAI Web-API.

I am incredibly busy. If I forgot your name, please accept my apologies and let me know so I can add it to the list.

## Changelog
### Version 7.3.1
- **Reverting a breking change which will be also Breaking Changes(only for 7.3.0):**
    - Reverting the usage of `EnsureStatusCode()` which caused the loss of error information. Initially, I thought it would help in implementing HTTP retry tools, but now I believe it is a bad idea for two reasons.
        1. You can't simply retry if the request wasn't successful because it could fail for various reasons. For example, you might have used too many tokens in your request, causing OpenAI to reject the response, or you might have tried to use a nonexistent model. It would be better to use the Error object in your retry rules. All responses are already derived from this base object.
        2. We will lose error response data.
### Version 7.3.0
- Updated Moderation categories as reported by @dmki.
- **Breaking Changes:**
    - Introduced the use of `EnsureStatusCode()` after making requests.Please adjust your code accordingly for handling failure cases. Thanks to @miroljub1995 for reporting.
    - Previously, we used to override paths in the base domain, but this behavior has now changed. If you were using `abc.com/mypath` as the base domain, we used to ignore `/mypath`. This will no longer be the case, and the code will now respect `/mypath`. Thanks to @Hzw576816 for reporting.
### 7.2.0
- Added Chatgpt Finetununig support thanks to @aghimir3 
- Default Azure Openai version increased thanks to @mac8005
- Fixed Azure Openai Audio endpoint thanks to @mac8005
### 7.1.5
- Added error handling for PlatformNotSupportedException in PostAsStreamAsync when using HttpClient.Send, now falls back to SendRequestPreNet6 for compatibility on platforms like MAUI, Mac. Thanks to  @Almis90
- We now have a function caller describe method that automatically generates function descriptions. This method is available in the utilities library. Thanks to @vbandi
### 7.1.3
- This release was a bit late and took longer than expected due to a couple of reasons. The future was quite big, and I couldn't cover all possibilities. However, I believe I have covered most of the function definitions (with some details missing). Additionally, I added an option to build it manually. If you don't know what I mean, you don't need to worry. I plan to cover the rest of the function definition in the next release. Until then, you can discover this by playing in the playground or in the source code. This version also support using other libraries to export your function definition.
- We now have support for functions! Big cheers to @rzubek for completing most of this feature.
- Additionally, we have made bug fixes and improvements. Thanks to @choshinyoung, @yt3trees, @WeihanLi, @N0ker, and all the bug reporters. (Apologies if I missed any names. Please let me know if I missed your name and you have a commit.) 
