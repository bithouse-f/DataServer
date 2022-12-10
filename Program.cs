global using FastEndpoints;
global using FastEndpoints.Security;
global using FluentValidation;
global using MiniDevTo.Auth;
global using MongoDB.Entities;
using FastEndpoints.Swagger;
using MiniDevTo.Migrations;
using SqlSugar;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JwtSigningKey"]);
builder.Services.AddSwaggerDoc();
builder.Services.AddSingleton<ISqlSugarClient>(new SqlSugarScope(new ConnectionConfig()
{
    ConnectionString = "Server=.xxxxx",//连接符字串
    DbType = DbType.SqlServer,//数据库类型
    IsAutoCloseConnection = true //不设成true要手动close
}
  //db => {
  //    //(A)全局生效配置点
  //    //调试SQL事件，可以删掉
  //    db.Aop.OnLogExecuting = (sql, pars) =>
  //    {
  //        Console.WriteLine(sql);//输出sql,查看执行sql
  //                               //5.0.8.2 获取无参数化 SQL 
  //                               //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
  //    }
 ));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(s => s.SerializerOptions = o => o.PropertyNamingPolicy = null);
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());

builder.Services

await DB.InitAsync(database: "MiniDevTo", host: "localhost");
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();