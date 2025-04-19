using Elearning.Implements;
using Elearning.Interfaces;
using Core;
using EmailService;
using FirebaseAdmin;
using FirebaseService;
using Common.Middlewares;
using FirebaseService.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.UseFirebaseService();
builder.Services.UseEmailService();
builder.Services.UseCore();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IChapterBL, ChapterBL>();
builder.Services.AddTransient<ICourseBL, CourseBL>();
builder.Services.AddTransient<ICoursePackageBL, CoursePackageBL>();
builder.Services.AddTransient<ICoursePackageDetailBL, CoursePackageDetailBL>();
builder.Services.AddTransient<ICourseSessionBL, CourseSessionBL>();
builder.Services.AddTransient<ILessonBL, LessonBL>();
builder.Services.AddTransient<IServiceBL, ServiceBL>();
builder.Services.AddTransient<IUserBL, UserBL>();

builder.Services.AddControllers(o =>
{
    o.Filters.Add<HttpResponseFilter>();
});

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.UseMemberCasing();
});

var firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("./FirebaseConfig/firebase-config.json")
});
builder.Services.AddSingleton(firebaseApp);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
