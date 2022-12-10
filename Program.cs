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
    ConnectionString = "Server=.xxxxx",//���ӷ��ִ�
    DbType = DbType.SqlServer,//���ݿ�����
    IsAutoCloseConnection = true //�����trueҪ�ֶ�close
}
  //db => {
  //    //(A)ȫ����Ч���õ�
  //    //����SQL�¼�������ɾ��
  //    db.Aop.OnLogExecuting = (sql, pars) =>
  //    {
  //        Console.WriteLine(sql);//���sql,�鿴ִ��sql
  //                               //5.0.8.2 ��ȡ�޲����� SQL 
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