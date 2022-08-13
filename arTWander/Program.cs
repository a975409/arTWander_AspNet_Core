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
    //�Ҧ�controller���n����
    //options.Filters.Add(new AuthorizeFilter());
})
.AddRazorRuntimeCompilation()
.AddNewtonsoftJson(options =>//�w��API�^�Ǫ�JSON�榡���]�w
{
    //�]�w�ɶ��榡
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    //�����Ȭ�null���ݩʭ�
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("arTWanderDatabase")));
builder.Services.AddSession(options =>
{
    //�ק�X�z�� Session ����ɶ�
    options.IdleTimeout = TimeSpan.FromMinutes(5);

    //����u���b HTTPS �s�u�����p�U�A�~���\�ϥ� Session�C�p���@���ܦ��[�K�s�u�A�N���e���Q�d�I�C
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;

    //�p�G cookie �����L�k�ѥΤ�ݸ}���s���A�h�� true;�_�h�� false�C
    options.Cookie.HttpOnly = true;

    //���X�� cookie �O�_�����ε{�����T�B�@�����n���C �Y�� true�A�h�i�ತ�L�P�N��h�ˬd
    options.Cookie.IsEssential = true;

    //���Session �W��
    options.Cookie.Name = "session.user";
});
//�NSession��m����A�������ذO����
builder.Services.AddDistributedMemoryCache();

#region �ۭq���Ҥ��

//�H�U�I����DI�`�J��IOptions<TestAuthenticationOptions> opt�A�Ь�TestAuthenticationHandler.cs
builder.Services.AddOptions<LoginAuthenticationOptions>();

// �s�W���ҥ\��
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = LoginAuthenticationHandler.LOGIN_SCHEM_NAME;
    // �s�W�ڭ̦۩w�q�����Ҥ�צW
    option.AddScheme<LoginAuthenticationHandler>(LoginAuthenticationHandler.LOGIN_SCHEM_NAME, null);
});
#endregion

#region ��g���ر��v�\��A�æۭq���v����
// �s�W���v�\��
builder.Services.AddAuthorization(option =>
{
    // ���U���v�����A�W����demo2��
    option.AddPolicy("Login", c =>
    {
        // �P�ڭ̫e���w�q�����Ҥ��ô��
        // ���v�L�{���H�����ҫ�o��
        // �n�`�N���w���Ҥ��
        c.AddAuthenticationSchemes(LoginAuthenticationHandler.LOGIN_SCHEM_NAME);

        // �n�D�s�b�w�n�J�ϥΪ̪�����
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
