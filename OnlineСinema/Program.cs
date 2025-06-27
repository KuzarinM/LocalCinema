using MediatR;
using Online—inema.Core.Extentions;
using Online—inema.Models.Commands.Images;
using Online—inema.Models.Commands.Titles;
using Online—inema.Models.Queries.Synchronisation;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApp();

var app = builder.Build();

app.SettingUpApplication();

//app.Services.CreateScope().ServiceProvider.GetRequiredService<IMediator>().Send(new UploadImageFromDiskCommand()
//{
//    DirName = "F:\\"
//}).Wait();



app.Run();
