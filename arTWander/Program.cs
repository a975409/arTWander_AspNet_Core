using arTWander.Authentications;
using arTWander.Data;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
.AddControllersWithViews(options =>
{
    //所有controller都要驗證
    //options.Filters.Add(new AuthorizeFilter());
})
.AddRazorRuntimeCompilation()
.AddNewtonsoftJson(options =>//針對API回傳的JSON格式做設定
{
    //設定時間格式
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    //忽略值為null的屬性值
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("arTWanderDatabase")));
builder.Services.AddSession(options =>
{
    //修改合理的 Session 到期時間
    options.IdleTimeout = TimeSpan.FromMinutes(5);

    //限制只有在 HTTPS 連線的情況下，才允許使用 Session。如此一來變成加密連線，就不容易被攔截。
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;

    //如果 cookie 必須無法由用戶端腳本存取，則為 true;否則為 false。
    options.Cookie.HttpOnly = true;

    //指出此 cookie 是否為應用程式正確運作的必要項。 若為 true，則可能略過同意原則檢查
    options.Cookie.IsEssential = true;

    //更改Session 名稱
    options.Cookie.Name = "session.user";
});
//將Session放置到伺服器的內建記憶體
builder.Services.AddDistributedMemoryCache();

#region 自訂驗證方案

//以下呼應到DI注入的IOptions<TestAuthenticationOptions> opt，請看TestAuthenticationHandler.cs
builder.Services.AddOptions<LoginAuthenticationOptions>();

// 新增驗證功能
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = LoginAuthenticationHandler.LOGIN_SCHEM_NAME;
    // 新增我們自定義的驗證方案名
    option.AddScheme<LoginAuthenticationHandler>(LoginAuthenticationHandler.LOGIN_SCHEM_NAME, null);
});
#endregion

#region 改寫內建授權功能，並自訂授權策略
// 新增授權功能
builder.Services.AddAuthorization(option =>
{
    // 註冊授權策略，名為“demo2”
    option.AddPolicy("Login", c =>
    {
        // 與我們前面定義的驗證方案繫結
        // 授權過程跟隨該驗證後發生
        // 要注意指定驗證方案
        c.AddAuthenticationSchemes(LoginAuthenticationHandler.LOGIN_SCHEM_NAME);

        // 要求存在已登入使用者的標識
        c.RequireAuthenticatedUser();
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
