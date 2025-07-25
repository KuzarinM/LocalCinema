using MediatR;
using OnlineŅinema.Core.Extentions;
using OnlineŅinema.Models.Commands.Images;
using OnlineŅinema.Models.Commands.Titles;
using OnlineŅinema.Models.Queries.Synchronisation;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApp();

var app = builder.Build();

app.SettingUpApplication();

//app.Services.CreateScope().ServiceProvider.GetRequiredService<IMediator>().Send(new UploadImageFromDiskCommand()
//{
//    DirName = "F:\\"
//}).Wait();



app.Run();
