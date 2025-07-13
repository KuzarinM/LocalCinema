using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using LangChain.Providers;
using LangChain.Providers.OpenAI;
using LangChain.Providers.OpenRouter;
using MediatR;
using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Implements;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Queries.Titles;
using OpenRouterConfiguration = OnlineСinema.Core.Configurations.OpenRouterConfiguration;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class CreateTitleDescriptionQueryHandler(
        ILogger<CreateTitleDescriptionQueryHandler> logger, 
        IOptions<OpenRouterConfiguration> openRouterConfig,
        ITitleStorage titleStorage
    ) : AbstractQueryHandler<CreateTitleDescriptionQuery, string>(logger)
    {

        private readonly OpenRouterConfiguration _openRouterConfiguration = openRouterConfig.Value;
        private readonly ITitleStorage _storage = titleStorage;

        private const string GENERATE_DEESCIPTION_PROMPT = 
            @"Ты AI помошник для онлайн кинотеатра. ТВОЯ ЗАДАЧА СОСТАВИТЬ АННОТАЦИЮ ФИЛЬМУ ИЛИ СЕРИАЛУ. 
               В ОТВЕТЕ ДОЛЖЕН БЫТЬ ТОЛЬКО ТЕКСТ ОПСИАНИЯ БЕЗ НАЗЧВАНИЯ ПРОЕКТА В НАЧАЛЕ ИЛИ СЛОВА ОПИСАНИЕ
               БЕЗ СПОЙЛЕРОВ. В ОТВЕТЕ УКАЖИ ТОЛЬКО ОПИСАНИЕ, НИЧЕГО КРОМЕ НЕГО.
             ";


        public async override Task<ResponseModel<string>> HandleAsync(CreateTitleDescriptionQuery request, CancellationToken cancellationToken)
        {
            var title = await _storage.GetTitleById(request.TitleId);

            OpenRouterProvider provider = new(_openRouterConfiguration.Token);

            var model = new OpenRouterModel(provider, _openRouterConfiguration.TitleDescriptorModelName);

            var modelRequest = new ChatRequest()
            {
                Messages = [
                    new(){
                        Role = MessageRole.System,
                        Content = GENERATE_DEESCIPTION_PROMPT
                    },
                    new(){
                        Role = MessageRole.Human,
                        Content = $"{(title.Isfilm? "фильм": "сериал")} {title.Name})"
                    }
                ],
            };

            var settings = new OpenAiChatSettings()
            {
                Temperature = 0,
            };

            string responceText  = string.Empty;

            await foreach (var response in model.GenerateAsync(modelRequest, settings))
            {
                responceText = response.ToString() ?? "placeholder";
            }

            return Success(responceText);
        }
    }
}
